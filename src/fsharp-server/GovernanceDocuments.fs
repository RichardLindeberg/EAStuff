namespace EAArchive

open System
open System.Collections
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions
open YamlDotNet.Serialization

module GovernanceRegistryLoader =

    let private parseFrontmatter (content: string) : (Map<string, obj> * string) =
        let lines = content.Split('\n')
        if lines.Length > 0 && lines.[0].Trim() = "---" then
            let mutable endIdx = -1
            for i in 1 .. lines.Length - 1 do
                if lines.[i].Trim() = "---" then
                    endIdx <- i

            if endIdx > 1 then
                let yamlContent = String.concat "\n" lines.[1 .. endIdx - 1]
                let markdownContent =
                    if endIdx + 1 < lines.Length then
                        String.concat "\n" lines.[endIdx + 1 ..]
                    else
                        ""

                try
                    let deserializer = DeserializerBuilder().Build()
                    let data = deserializer.Deserialize<Dictionary<string, obj>>(yamlContent)
                    (data |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Map.ofSeq, markdownContent)
                with
                | _ -> (Map.empty, content)
            else
                (Map.empty, content)
        else
            (Map.empty, content)

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
        metadata
        |> Map.tryFind key
        |> Option.bind (fun v ->
            match v with
            | :? string as s -> Some s
            | _ ->
                try Some (v.ToString())
                with _ -> None
        )

    let private getStringFromObj (value: obj) : string option =
        if isNull value then None
        else
            let text = value.ToString().Trim()
            if text = "" then None else Some text

    let private parseGovernanceRelations (metadata: Map<string, obj>) : Relationship list =
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
                        Some {
                            relationType = ElementType.parseRelationType relTypeValue
                            target = targetValue
                            description = ""
                        }
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
        let metadataObj, contentWithoutMetadata = parseFrontmatter content
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
            getString "name" metadataObj
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
        (elements: Map<string, Element>)
        (knownDocumentIds: Set<string>)
        (allowedRelationTypes: Set<RelationType>)
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

        errors.AddRange(validateRequiredField doc.filePath docId "name" "name" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "owner" "owner" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "approved_by" "approved_by" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "status" "status" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "effective_date" "effective_date" doc.metadata)
        errors.AddRange(validateRequiredField doc.filePath docId "review_cycle" "review_cycle" doc.metadata)

        match Map.tryFind "owner" doc.metadata |> Option.map (fun value -> value.Trim()) with
        | Some ownerId when ownerId <> "" ->
            match Map.tryFind ownerId elements with
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
                match relation.relationType with
                | RelationType.Unknown value when String.IsNullOrWhiteSpace value ->
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.InvalidRelationshipType value
                        message = "Relationship type is required for governance relations"
                        severity = Severity.Error
                    })
                | _ -> ()
                if String.IsNullOrWhiteSpace relation.target then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.MissingRequiredField
                        message = "Relationship target is required for governance relations"
                        severity = Severity.Error
                    })
                elif not (Set.contains relation.target knownDocumentIds) then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.RelationshipTargetNotFound (doc.docId, relation.target)
                        message = sprintf "Relationship target '%s' was not found in repository documents" relation.target
                        severity = Severity.Error
                    })
                else
                    match relation.relationType with
                    | RelationType.Unknown value ->
                        errors.Add({
                            filePath = doc.filePath
                            elementId = docId
                            errorType = ErrorType.InvalidRelationshipType value
                            message = sprintf "Relationship type '%s' is not recognized" value
                            severity = Severity.Error
                        })
                    | relationType ->
                        if not (Set.contains relationType allowedRelationTypes) then
                            let relationLabel = ElementType.relationTypeToString relationType
                            errors.Add({
                                filePath = doc.filePath
                                elementId = docId
                                errorType = ErrorType.InvalidRelationshipType relationLabel
                                message = sprintf "Relationship type '%s' is not allowed for governance documents" relationLabel
                                severity = Severity.Error
                            })

        List.ofSeq errors

    type GovernanceLoadResult = {
        documents: GovernanceDocument list
        documentsByType: Map<GovernanceDocType, string list>
        validationErrors: ValidationError list
        relations: DocumentRelation list
    }

    let loadDocuments
        (managementSystemPath: string)
        (elements: Map<string, Element>)
        (elementIds: Set<string>)
        (allowedRelationTypes: Set<RelationType>)
        (logger: ILogger)
        : GovernanceLoadResult =
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

            let docsByType =
                documents
                |> List.groupBy (fun doc -> doc.docType)
                |> List.map (fun (docType, docs) -> docType, docs |> List.map (fun doc -> doc.slug) |> List.sort)
                |> Map.ofList

            let documentIds =
                documents
                |> List.map (fun doc -> doc.docId)
                |> List.filter (fun value -> not (String.IsNullOrWhiteSpace value))
                |> Set.ofList

            let knownDocumentIds = Set.union elementIds documentIds

            let validationErrors =
                documents
                |> List.collect (fun doc -> validateDocument doc elements knownDocumentIds allowedRelationTypes)

            let relations =
                documents
                |> List.collect (fun doc ->
                    doc.relations
                    |> List.map (fun rel ->
                        {
                            sourceId = doc.docId
                            targetId = rel.target
                            relationType = ElementType.relationTypeToString rel.relationType
                            description = ""
                        }
                    )
                )

            if not validationErrors.IsEmpty then
                logger.LogWarning("Governance validation found {errorCount} errors", validationErrors.Length)

            {
                documents = documents
                documentsByType = docsByType
                validationErrors = validationErrors
                relations = relations
            }
        else
            logger.LogWarning("Governance management system path not found: {path}", managementSystemPath)
            {
                documents = []
                documentsByType = Map.empty
                validationErrors = []
                relations = []
            }
