namespace EAArchive

open System
open System.Collections
open System.IO
open System.Text.RegularExpressions
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions

module GovernanceRegistryLoader =

    let private governanceIdPattern = Regex("^ms-(policy|instruction|manual)-\\d{3}-[a-z0-9-]+$", RegexOptions.IgnoreCase)

    let private docTypeFromPath (filePath: string) : GovernanceDocType =
        let lower = filePath.ToLowerInvariant()
        if lower.Contains("\\policies\\") || lower.Contains("/policies/") then
            GovernanceDocType.Policy
        elif lower.Contains("\\instructions\\") || lower.Contains("/instructions/") then
            GovernanceDocType.Instruction
        elif lower.Contains("\\manuals\\") || lower.Contains("/manuals/") then
            GovernanceDocType.Manual
        elif lower.Contains("ms-policy-") then
            GovernanceDocType.Policy
        elif lower.Contains("ms-instruction-") then
            GovernanceDocType.Instruction
        elif lower.Contains("ms-manual-") then
            GovernanceDocType.Manual
        else
            GovernanceDocType.Unknown "unknown"

    let private getString (key: string) (metadata: Map<string, obj>) : string option =
        ElementRegistry.getString key metadata

    let private getStringFromObj (value: obj) : string option =
        if isNull value then None
        else
            let text = value.ToString().Trim()
            if text = "" then None else Some text

    let private parseGovernanceRelations (metadata: Map<string, obj>) : GovernanceRelation list =
        let tryGetRelations key =
            metadata
            |> Map.tryFind key
            |> Option.bind (fun v ->
                match v with
                | :? System.Collections.Generic.List<obj> as list -> Some list
                | _ -> None
            )

        let relationList =
            match tryGetRelations "relationships" with
            | Some list -> Some list
            | None -> tryGetRelations "relations"

        relationList
        |> Option.map (fun list ->
            list
            |> Seq.choose (fun item ->
                match item with
                | :? IDictionary as dict ->
                    let relType = if dict.Contains("type") then getStringFromObj dict.["type"] else None
                    let target = if dict.Contains("target") then getStringFromObj dict.["target"] else None
                    match relType, target with
                    | Some relTypeValue, Some targetValue ->
                        Some { relationType = relTypeValue; target = targetValue }
                    | _ -> None
                | _ -> None
            )
            |> Seq.toList
        )
        |> Option.defaultValue []

    let private tryGetTitle (content: string) : string option =
        content.Split('\n')
        |> Array.tryPick (fun line ->
            let trimmed = line.Trim()
            if trimmed.StartsWith("# ") then
                Some (trimmed.Substring(2).Trim())
            else
                None
        )

    let private parseDocument (filePath: string) (content: string) : GovernanceDocument =
        let slug = Path.GetFileNameWithoutExtension(filePath)
        let docType = docTypeFromPath filePath
        let metadataObj, contentWithoutMetadata = ElementRegistry.parseFrontmatter content
        let relations = parseGovernanceRelations metadataObj
        let metadata =
            metadataObj
            |> Map.remove "relationships"
            |> Map.remove "relations"
            |> Map.toSeq
            |> Seq.choose (fun (key, value) ->
                getStringFromObj value
                |> Option.map (fun text -> key.ToLowerInvariant(), text)
            )
            |> Map.ofSeq
        let title =
            getString "title" metadataObj
            |> Option.orElseWith (fun () -> tryGetTitle contentWithoutMetadata)
            |> Option.defaultValue slug

        let docId =
            getString "id" metadataObj
            |> Option.filter (fun value -> not (String.IsNullOrWhiteSpace value))
            |> Option.defaultValue slug

        {
            slug = slug
            docId = docId
            title = title
            docType = docType
            filePath = filePath
            metadata = metadata
            relations = relations
            content = contentWithoutMetadata.Trim()
            rawContent = content
        }

    let private validateRequiredField
        (filePath: string)
        (docId: string option)
        (key: string)
        (label: string)
        (metadata: Map<string, string>)
        : ValidationError list =
        match Map.tryFind key metadata with
        | Some value when not (String.IsNullOrWhiteSpace value) -> []
        | _ ->
            [
                {
                    filePath = filePath
                    elementId = docId
                    errorType = ErrorType.MissingRequiredField
                    message = sprintf "Governance document must have a '%s' field" label
                    severity = Severity.Error
                }
            ]

    let private validateDocument
        (doc: GovernanceDocument)
        (elementRegistry: ElementRegistry)
        : ValidationError list =
        let errors = ResizeArray<ValidationError>()
        let docId = if String.IsNullOrWhiteSpace doc.docId then None else Some doc.docId

        if docId.IsNone then
            errors.Add({
                filePath = doc.filePath
                elementId = None
                errorType = ErrorType.MissingId
                message = "Governance document must have an 'id' field"
                severity = Severity.Error
            })
        else
            let docIdValue = doc.docId
            if not (governanceIdPattern.IsMatch docIdValue) then
                errors.Add({
                    filePath = doc.filePath
                    elementId = docId
                    errorType = ErrorType.Unknown "invalid-governance-id-format"
                    message = sprintf "ID '%s' should match pattern: ms-[policy|instruction|manual]-[###]-[descriptive-name]" docIdValue
                    severity = Severity.Error
                })

        errors.AddRange(validateRequiredField doc.filePath docId "owner" "owner" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "approved_by" "approved_by" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "status" "status" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "effective_date" "effective_date" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "review_cycle" "review_cycle" doc.metadata)

        match Map.tryFind "owner" doc.metadata |> Option.map (fun value -> value.Trim()) with
        | Some ownerId when ownerId <> "" ->
            match Map.tryFind ownerId elementRegistry.elements with
            | Some elem ->
                match elem.elementType with
                | ElementType.Business BusinessElement.Role -> ()
                | _ ->
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.InvalidType
                        message = sprintf "Owner '%s' must reference a business-role element" ownerId
                        severity = Severity.Error
                    })
            | None ->
                errors.Add({
                    filePath = doc.filePath
                    elementId = docId
                    errorType = ErrorType.RelationshipTargetNotFound (doc.docId, ownerId)
                    message = sprintf "Owner '%s' was not found in ArchiMate elements" ownerId
                    severity = Severity.Error
                })
        | _ -> ()

        if List.isEmpty doc.relations then
            errors.Add({
                filePath = doc.filePath
                elementId = docId
                errorType = ErrorType.MissingRequiredField
                message = "Governance document must declare at least one relationship"
                severity = Severity.Error
            })
        else
            for relation in doc.relations do
                if String.IsNullOrWhiteSpace relation.relationType then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.InvalidRelationshipType relation.relationType
                        message = "Relationship type is required for governance relations"
                        severity = Severity.Error
                    })
                if String.IsNullOrWhiteSpace relation.target then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.MissingRequiredField
                        message = "Relationship target is required for governance relations"
                        severity = Severity.Error
                    })
                elif not (Map.containsKey relation.target elementRegistry.elements) then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.RelationshipTargetNotFound (doc.docId, relation.target)
                        message = sprintf "Relationship target '%s' was not found in ArchiMate elements" relation.target
                        severity = Severity.Error
                    })

        List.ofSeq errors

    let createWithLoggerAndElements
        (managementSystemPath: string)
        (elementRegistry: ElementRegistry)
        (logger: ILogger)
        : GovernanceDocRegistry =
        if Directory.Exists(managementSystemPath) then
            let documents =
                Directory.EnumerateFiles(managementSystemPath, "*.md", SearchOption.AllDirectories)
                |> Seq.map (fun filePath ->
                    let content = File.ReadAllText(filePath)
                    let doc = parseDocument filePath content
                    logger.LogDebug("Loaded governance document {slug} from {path}", doc.slug, filePath)
                    doc
                )
                |> Seq.toList

            let docMap =
                documents
                |> List.fold (fun acc doc -> Map.add doc.slug doc acc) Map.empty

            let docsByType =
                documents
                |> List.groupBy (fun doc -> doc.docType)
                |> List.map (fun (docType, docs) -> docType, docs |> List.map (fun doc -> doc.slug) |> List.sort)
                |> Map.ofList

            let validationErrors =
                documents
                |> List.collect (fun doc -> validateDocument doc elementRegistry)

            if not validationErrors.IsEmpty then
                logger.LogWarning("Governance validation found {errorCount} errors", validationErrors.Length)

            {
                documents = docMap
                documentsByType = docsByType
                managementSystemPath = managementSystemPath
                validationErrors = validationErrors
            }
        else
            logger.LogWarning("Governance management system path not found: {path}", managementSystemPath)
            {
                documents = Map.empty
                documentsByType = Map.empty
                managementSystemPath = managementSystemPath
                validationErrors = []
            }

    let create (managementSystemPath: string) (elementRegistry: ElementRegistry) : GovernanceDocRegistry =
        createWithLoggerAndElements managementSystemPath elementRegistry (NullLogger.Instance)

    let getValidationErrors (registry: GovernanceDocRegistry) : ValidationError list =
        registry.validationErrors
