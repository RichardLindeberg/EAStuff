namespace EAArchive

open System
open System.Collections
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions
open Microsoft.Extensions.Logging
open YamlDotNet.Serialization


module DocumentRepositoryLoader =
    let getHeaderAndContent (content: string) : Result<string * string, string> =
            let getSeparatorRow fromIndex =
                try
                    let a = (content.IndexOf("---", fromIndex, StringComparison.Ordinal)) 
                    if a >= 0 then Some a else None
                with
                | _ -> None

            let startIndex = 
                match getSeparatorRow 0 with 
                | Some idx -> 
                    if idx > 2 then 
                        Error (sprintf "Frontmatter start --- must be within first 3 lines in \"%A\"" content)
                    else
                        Ok idx
                | None -> Error (sprintf "No start --- found in \"%s\"" content)
            
            let endIndex startIdx =
                match getSeparatorRow (startIdx + 1) with 
                | Some idx -> Ok (startIdx, idx)
                | None -> sprintf "No end --- found in \"%s\"" content |> failwith
            
            match startIndex with 
            | Ok startIdx ->
                match endIndex startIdx with
                | Ok (s, e) -> (content.Substring(s + 1, e - 1), content.Substring(e+1)) |> Ok
                | Error e -> Error e
            | Error e -> Error e
            
                

            
            
    let private parseFrontmatter (content: string) =
        let ht = getHeaderAndContent content
        ht
        |> Result.map (fun (header, body) ->
            (SimpleYaml.parse header), body
            )

                     
                    
        
       
           



    let private normalizeKey (value: string) : string =
        value.Trim().ToLowerInvariant().Replace(" ", "_").Replace("-", "_")

    let rec private normalizeYaml (value: SimpleYaml) : SimpleYaml =
        match value with
        | SimpleYaml.Value s -> SimpleYaml.Value s
        | SimpleYaml.Map map ->
            map
            |> Map.fold (fun acc key item -> Map.add (normalizeKey key) (normalizeYaml item) acc) Map.empty
            |> SimpleYaml.Map
        | SimpleYaml.List items ->
            items |> List.map normalizeYaml |> SimpleYaml.List

    let private normalizeMetadata (metadata: Map<string, SimpleYaml>) : Map<string, SimpleYaml> =
        metadata
        |> Map.fold (fun acc key value -> Map.add (normalizeKey key) (normalizeYaml value) acc) Map.empty

    let private toStringOption (value: string) : string option =
        let text = value.Trim()
        if text = "" then None else Some text

    let rec private toObj (value: SimpleYaml) : obj =
        match value with
        | SimpleYaml.Value s -> box s
        | SimpleYaml.Map map -> map |> Map.map (fun _ item -> toObj item) |> box
        | SimpleYaml.List items -> items |> List.map toObj |> box

    let private tryGetValue (key: string) (metadata: Map<string, SimpleYaml>) : SimpleYaml option =
        metadata |> Map.tryFind (normalizeKey key)

    let private tryGetString (key: string) (metadata: Map<string, SimpleYaml>) : string option =
        metadata
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (fun value ->
            match value with
            | SimpleYaml.Value s -> toStringOption s
            | _ -> None
        )   

    let private tryGetMap (key: string) (metadata: Map<string, SimpleYaml>) : Map<string, SimpleYaml> option =
        metadata
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (function
            | SimpleYaml.Map map -> Some map
            | _ -> None
        )

    let private tryGetList (key: string) (metadata: Map<string, SimpleYaml>) : SimpleYaml list option =
        metadata
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (function
            | SimpleYaml.List items -> Some items
            | _ -> None
        )

    let private tryGetStringFromMap (key: string) (metadata: Map<string, SimpleYaml>) : string option =
        metadata
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (function
            | SimpleYaml.Value s -> toStringOption s
            | _ -> None
        )

    let private parseTags (metadata: Map<string, SimpleYaml>) : string list =
        match Map.tryFind (normalizeKey "tags") metadata with
        | Some (SimpleYaml.Value s) ->
            s.Split(',')
            |> Array.map (fun t -> t.Trim())
            |> Array.filter (fun t -> t <> "")
            |> Array.toList
        | Some (SimpleYaml.List items) ->
            items
            |> List.choose (function
                | SimpleYaml.Value s ->
                    let trimmed = s.Trim()
                    if trimmed = "" then None else Some trimmed
                | _ -> None
            )
        | _ -> []

    let private parseRelationships (metadata: Map<string, SimpleYaml>) : Relationship list =
        let parseListItem (section: Map<string, SimpleYaml>) : Relationship option =
            let relType = tryGetString "type" section
            let relTarget = tryGetString "target" section
            let relDescription = tryGetString "description" section

            match relType, relTarget with
            | Some sType, Some target ->
                Some {
                    target = target
                    relationType = ElementType.parseRelationType sType
                    description = relDescription |> Option.defaultValue ""
                }
            | _ -> None

        Map.tryFind "relationships" metadata
        |> Option.bind (fun value ->
            match value with
            | SimpleYaml.List items ->
                items
                |> List.choose (fun item ->
                    match item with
                    | SimpleYaml.Map section -> parseListItem section
                    | _ -> None
                )
                |> Some
            | _ -> None
        )
        |> Option.defaultValue []

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

    let private governanceIdPattern = Regex("^ms-(policy|instruction|manual)-\\d{3}-[a-z0-9-]+$", RegexOptions.IgnoreCase)

    let private knownExtensionKeys =
        set [
            "id"
            "name"
            "owner"
            "status"
            "version"
            "last_updated"
            "review_cycle"
            "next_review"
            "relationships"
            "tags"
            "governance"
            "archimate"
            "extensions"
        ]

    let private buildExtensions (metadata: Map<string, SimpleYaml>) : Map<string, obj> =
        let normalized = normalizeMetadata metadata
        let extensionsSection =
            tryGetMap "extensions" normalized
            |> Option.defaultValue Map.empty

        let extras =
            normalized
            |> Map.filter (fun key _ -> not (Set.contains key knownExtensionKeys))

        let baseMap = extensionsSection |> Map.map (fun _ value -> toObj value)
        let extrasMap = extras |> Map.map (fun _ value -> toObj value)

        extrasMap
        |> Map.fold (fun acc key value ->
            if Map.containsKey key acc then acc else Map.add key value acc
        ) baseMap

    let private parseArchimateMetadata (metadata: Map<string, SimpleYaml>) : ArchimateMetadata option =
        let archimateSection =
            tryGetMap "archimate" metadata
            |> Option.map normalizeMetadata
            |> Option.defaultValue Map.empty

        let typeValue = tryGetStringFromMap "type" archimateSection
        let layerValue = tryGetStringFromMap "layer" archimateSection

        match typeValue, layerValue with
        | Some typeValue, Some layerValue ->
            let criticality = tryGetStringFromMap "criticality" archimateSection
            Some {
                elementType = typeValue
                layerValue = layerValue
                criticality = criticality
            }
        | _ -> None

    let private parseGovernanceMetadata (metadata: Map<string, SimpleYaml>) : GovernanceMetadata option =
        let governanceSection =
            tryGetMap "governance" metadata
            |> Option.map normalizeMetadata
            |> Option.defaultValue Map.empty

        let approvedBy = tryGetStringFromMap "approved_by" governanceSection
        let effectiveDate = tryGetStringFromMap "effective_date" governanceSection

        match approvedBy, effectiveDate with
        | Some approvedBy, Some effectiveDate ->
            Some {
                approvedBy = approvedBy
                effectiveDate = effectiveDate
            }
        | _ -> None

    let private buildDocumentMetadata
        (idValue: string)
        (hasExplicitId: bool)
        (hasExplicitName: bool)
        (metadata: Map<string, SimpleYaml>)
        (relationships: Relationship list)
        (governance: GovernanceMetadata option)
        (archimate: ArchimateMetadata option)
        (tags: string list)
        : DocumentMetaData =
        {
            id = idValue
            hasExplicitId = hasExplicitId
            hasExplicitName = hasExplicitName
            owner = tryGetString "owner" metadata
            status = tryGetString "status" metadata
            version = tryGetString "version" metadata
            lastUpdated = tryGetString "last_updated" metadata
            reviewCycle = tryGetString "review_cycle" metadata
            nextReview = tryGetString "next_review" metadata
            relationships = relationships
            governance = governance
            archimate = archimate
            extensions = buildExtensions metadata
            tags = tags
        }

    let private parseArchitectureDocument (filePath: string) (content: string) : DocumentRecord =
        let slug = Path.GetFileNameWithoutExtension(filePath)

        

        let metadataRaw, contentWithoutMetadata = 
            match parseFrontmatter content with
            | Error e -> failwithf "Error parsing frontmatter in file %s: %s" filePath e
            | Ok (metadataRaw, contentWithoutMetadata) -> metadataRaw, contentWithoutMetadata
        let metadata = normalizeMetadata metadataRaw
        let relationships = parseRelationships metadata
        let tags = parseTags metadata

        let name = tryGetString "name" metadata |> Option.defaultValue slug
        let idValue = tryGetString "id" metadata |> Option.defaultValue slug
        let hasExplicitId = tryGetString "id" metadata |> Option.isSome
        let hasExplicitName = tryGetString "name" metadata |> Option.isSome
        let governance = None
        let archimate = parseArchimateMetadata metadata

        let documentMetadata =
            buildDocumentMetadata idValue hasExplicitId hasExplicitName metadata relationships governance archimate tags

        {
            id = idValue
            slug = slug
            title = name
            filePath = filePath
            kind = DocumentKind.Architecture
            metadata = documentMetadata
            content = contentWithoutMetadata.Trim()
            rawContent = content
        }

    let private parseGovernanceDocument (filePath: string) (content: string) : DocumentRecord =
        let slug = Path.GetFileNameWithoutExtension(filePath)
        let metadataRaw, contentWithoutMetadata = 
            match parseFrontmatter content with
            | Error e -> failwithf "Error parsing frontmatter in file %s: %s" filePath e
            | Ok (metadataRaw, contentWithoutMetadata) -> metadataRaw, contentWithoutMetadata
        let metadata = normalizeMetadata metadataRaw
        let relationships = parseRelationships metadata
        let tags = parseTags metadata

        let title = tryGetString "name" metadata |> Option.defaultValue slug
        let idValue = tryGetString "id" metadata |> Option.defaultValue slug
        let hasExplicitId = tryGetString "id" metadata |> Option.isSome
        let hasExplicitName = tryGetString "name" metadata |> Option.isSome
        let governance = parseGovernanceMetadata metadata
        let archimate = None

        let documentMetadata =
            buildDocumentMetadata idValue hasExplicitId hasExplicitName metadata relationships governance archimate tags

        {
            id = idValue
            slug = slug
            title = title
            filePath = filePath
            kind = DocumentKind.Governance
            metadata = documentMetadata
            content = contentWithoutMetadata.Trim()
            rawContent = content
        }

    let private validateMissingField (filePath: string) (docId: string option) (label: string) : ValidationError =
        {
            filePath = filePath
            elementId = docId
            errorType = ErrorType.MissingRequiredField
            message = sprintf "Document must have a '%s' field" label
            severity = Severity.Error
        }

    let private validateSharedFields (doc: DocumentRecord) : ValidationError list =
        let docId = if String.IsNullOrWhiteSpace doc.id then None else Some doc.id
        [
            if doc.kind = DocumentKind.Architecture && not doc.metadata.hasExplicitName then
                yield validateMissingField doc.filePath docId "name"
            if doc.metadata.owner |> Option.forall String.IsNullOrWhiteSpace then
                yield validateMissingField doc.filePath docId "owner"
            if doc.metadata.status |> Option.forall String.IsNullOrWhiteSpace then
                yield validateMissingField doc.filePath docId "status"
            if doc.metadata.version |> Option.forall String.IsNullOrWhiteSpace then
                yield validateMissingField doc.filePath docId "version"
            if doc.metadata.lastUpdated |> Option.forall String.IsNullOrWhiteSpace then
                yield validateMissingField doc.filePath docId "last_updated"
            if doc.metadata.reviewCycle |> Option.forall String.IsNullOrWhiteSpace then
                yield validateMissingField doc.filePath docId "review_cycle"
            if doc.metadata.nextReview |> Option.forall String.IsNullOrWhiteSpace then
                yield validateMissingField doc.filePath docId "next_review"
        ]

    let private validateGovernanceMetadata (doc: DocumentRecord) : ValidationError list =
        let docId = if String.IsNullOrWhiteSpace doc.id then None else Some doc.id
        match doc.metadata.governance with
        | Some governance ->
            [
                if String.IsNullOrWhiteSpace governance.approvedBy then
                    yield validateMissingField doc.filePath docId "approved_by"
                if String.IsNullOrWhiteSpace governance.effectiveDate then
                    yield validateMissingField doc.filePath docId "effective_date"
            ]
        | None ->
            [ validateMissingField doc.filePath docId "governance" ]

    let private validateArchimateMetadata (doc: DocumentRecord) : ValidationError list =
        let docId = if String.IsNullOrWhiteSpace doc.id then None else Some doc.id
        match doc.metadata.archimate with
        | Some archimate ->
            let errors = ResizeArray<ValidationError>()
            if String.IsNullOrWhiteSpace archimate.elementType then
                errors.Add(validateMissingField doc.filePath docId "type")
            if String.IsNullOrWhiteSpace archimate.layerValue then
                errors.Add(validateMissingField doc.filePath docId "layer")
            let normalizedLayer = archimate.layerValue.Trim().ToLowerInvariant()
            if Config.layerOptions |> List.exists (fun value -> value = normalizedLayer) then
                ()
            else
                errors.Add({
                    filePath = doc.filePath
                    elementId = docId
                    errorType = ErrorType.InvalidLayer
                    message = sprintf "Invalid layer '%s'. Must be one of: %s" archimate.layerValue (String.concat ", " Config.layerOptions)
                    severity = Severity.Error
                })

            // Validate ArchiMate ID format against layer and type codes
            if doc.metadata.hasExplicitId && not (String.IsNullOrWhiteSpace doc.id) then
                let idVal = doc.id
                if not (Regex.IsMatch(idVal, "^[a-z0-9]+-[a-z0-9]+-\\d{3}-[a-z0-9]+(-[a-z0-9]+)*$")) then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.Unknown "invalid-id-format"
                        message = sprintf "ID '%s' should match pattern: [layer-code]-[type-code]-[###]-[descriptive-name]" idVal
                        severity = Severity.Error
                    })
                else
                    let parts = idVal.Split('-')
                    let layerCodes =
                        Map.ofList [
                            ("str", "strategy")
                            ("bus", "business")
                            ("app", "application")
                            ("tec", "technology")
                            ("phy", "physical")
                            ("mot", "motivation")
                            ("imp", "implementation")
                        ]
                    let typeCodes =
                        Map.ofList [
                            ("str", ["rsrc"; "capa"; "vstr"; "cact"])
                            ("bus", ["actr"; "role"; "colab"; "intf"; "proc"; "func"; "intr"; "evnt"; "srvc"; "objt"; "cntr"; "repr"; "prod"])
                            ("app", ["comp"; "colab"; "intf"; "func"; "intr"; "proc"; "evnt"; "srvc"; "data"])
                            ("tec", ["node"; "devc"; "sysw"; "colab"; "intf"; "path"; "netw"; "func"; "proc"; "intr"; "evnt"; "srvc"; "artf"])
                            ("phy", ["equi"; "faci"; "dist"; "matr"])
                            ("mot", ["stkh"; "drvr"; "asmt"; "goal"; "outc"; "prin"; "reqt"; "cnst"; "mean"; "valu"])
                            ("imp", ["work"; "delv"; "evnt"; "plat"; "gap"])
                        ]

                    if parts.Length >= 1 then
                        let layerCode = parts.[0]
                        if layerCode.Length <> 3 then
                            errors.Add({
                                filePath = doc.filePath
                                elementId = docId
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = sprintf "Layer code '%s' must be exactly 3 characters" layerCode
                                severity = Severity.Error
                            })
                        elif not (Map.containsKey layerCode layerCodes) then
                            errors.Add({
                                filePath = doc.filePath
                                elementId = docId
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = sprintf "Layer code '%s' is not valid. Must be: str, bus, app, tec, phy, mot, imp" layerCode
                                severity = Severity.Error
                            })

                        if parts.Length >= 2 then
                            let typeCode = parts.[1]
                            if typeCode.Length <> 4 && typeCode <> "colab" then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = sprintf "Type code '%s' must be exactly 4 characters (or 'colab' for collaboration)" typeCode
                                    severity = Severity.Error
                                })
                            elif Map.containsKey layerCode typeCodes then
                                let validTypes = Map.find layerCode typeCodes
                                if not (List.contains typeCode validTypes) then
                                    errors.Add({
                                        filePath = doc.filePath
                                        elementId = docId
                                        errorType = ErrorType.Unknown "invalid-id-format"
                                        message = sprintf "Type code '%s' is not valid for layer '%s'" typeCode layerCode
                                        severity = Severity.Error
                                    })

                        if parts.Length >= 3 then
                            let seqNum = parts.[2]
                            if seqNum.Length <> 3 || not (Char.IsDigit seqNum.[0] && Char.IsDigit seqNum.[1] && Char.IsDigit seqNum.[2]) then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = sprintf "Sequential number '%s' must be exactly 3 digits (001-999)" seqNum
                                    severity = Severity.Error
                                })
                            elif seqNum = "000" then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = "Sequential number must start at 001, not 000"
                                    severity = Severity.Error
                                })

                        if parts.Length >= 4 then
                            let descriptiveName = String.concat "-" (parts.[3..])
                            let wordCount = parts.Length - 3
                            if wordCount < 1 then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = "Descriptive name must have at least 1 word"
                                    severity = Severity.Error
                                })
                            elif wordCount > 6 then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = sprintf "Descriptive name has %d words, maximum is 6" wordCount
                                    severity = Severity.Error
                                })

                            if not (Regex.IsMatch(descriptiveName, "^[a-z0-9]+(-[a-z0-9]+)*$")) then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = "Descriptive name must contain only lowercase letters, numbers, and hyphens"
                                    severity = Severity.Error
                                })

                            if descriptiveName.StartsWith("-") || descriptiveName.EndsWith("-") || descriptiveName.Contains("--") then
                                errors.Add({
                                    filePath = doc.filePath
                                    elementId = docId
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = "Descriptive name cannot have leading/trailing hyphens or consecutive hyphens"
                                    severity = Severity.Error
                                })

            List.ofSeq errors
        | None ->
            [ validateMissingField doc.filePath docId "archimate" ]

    let private validateRelationshipTargets (doc: DocumentRecord) (knownIds: Set<string>) : ValidationError list =
        if doc.kind = DocumentKind.Architecture then
            []
        else
            let docId = if String.IsNullOrWhiteSpace doc.id then None else Some doc.id
            doc.metadata.relationships
            |> List.collect (fun rel ->
                let errors = ResizeArray<ValidationError>()
                match rel.relationType with
                | RelationType.Unknown value when String.IsNullOrWhiteSpace value ->
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.InvalidRelationshipType value
                        message = "Relationship type is required"
                        severity = Severity.Error
                    })
                | _ -> ()

                if String.IsNullOrWhiteSpace rel.target then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.MissingRequiredField
                        message = "Relationship target is required"
                        severity = Severity.Error
                    })
                elif not (Set.contains rel.target knownIds) then
                    errors.Add({
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.RelationshipTargetNotFound (doc.id, rel.target)
                        message = sprintf "Relationship target '%s' was not found in repository documents" rel.target
                        severity = Severity.Error
                    })

                List.ofSeq errors
            )

    let private validateOwnerReference (doc: DocumentRecord) (archimateById: Map<string, ElementType>) : ValidationError list =
        let docId = if String.IsNullOrWhiteSpace doc.id then None else Some doc.id
        match doc.metadata.owner |> Option.map (fun value -> value.Trim()) with
        | Some ownerId when ownerId <> "" ->
            match Map.tryFind ownerId archimateById with
            | Some (ElementType.Business BusinessElement.Role) -> []
            | Some _ ->
                [
                    {
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.InvalidType
                        message = sprintf "Owner '%s' must reference a business-role element" ownerId
                        severity = Severity.Error
                    }
                ]
            | None ->
                [
                    {
                        filePath = doc.filePath
                        elementId = docId
                        errorType = ErrorType.RelationshipTargetNotFound (doc.id, ownerId)
                        message = sprintf "Owner '%s' was not found in ArchiMate elements" ownerId
                        severity = Severity.Error
                    }
                ]
        | _ -> []

    let private validateDocument
        (doc: DocumentRecord)
        (knownIds: Set<string>)
        (archimateById: Map<string, ElementType>)
        : ValidationError list =
        let errors = ResizeArray<ValidationError>()
        let docId = if doc.metadata.hasExplicitId && not (String.IsNullOrWhiteSpace doc.id) then Some doc.id else None

        if not doc.metadata.hasExplicitId then
            errors.Add({
                filePath = doc.filePath
                elementId = None
                errorType = ErrorType.MissingId
                message = "Document must have an 'id' field"
                severity = Severity.Error
            })

        errors.AddRange(validateSharedFields doc)

        match doc.kind with
        | DocumentKind.Governance ->
            if doc.metadata.hasExplicitId && not (governanceIdPattern.IsMatch doc.id) then
                errors.Add({
                    filePath = doc.filePath
                    elementId = docId
                    errorType = ErrorType.Unknown "invalid-governance-id-format"
                    message = sprintf "ID '%s' should match pattern: ms-[policy|instruction|manual]-[###]-[descriptive-name]" doc.id
                    severity = Severity.Error
                })
            errors.AddRange(validateGovernanceMetadata doc)
            errors.AddRange(validateOwnerReference doc archimateById)
        | DocumentKind.Architecture ->
            errors.AddRange(validateArchimateMetadata doc)

        errors.AddRange(validateRelationshipTargets doc knownIds)

        List.ofSeq errors

    let loadRepository
        (elementsPath: string)
        (managementSystemPath: string)
        (logger: ILogger)
        : DocumentRepository =
        let archimateDocs =
            if Directory.Exists(elementsPath) then
                Directory.EnumerateFiles(elementsPath, "*.md", SearchOption.AllDirectories)
                |> Seq.map (fun filePath ->
                    let content = File.ReadAllText(filePath)
                    parseArchitectureDocument filePath content
                )
                |> Seq.toList
            else
                []

        let governanceDocs =
            if Directory.Exists(managementSystemPath) then
                Directory.EnumerateFiles(managementSystemPath, "*.md", SearchOption.AllDirectories)
                |> Seq.map (fun filePath ->
                    let content = File.ReadAllText(filePath)
                    parseGovernanceDocument filePath content
                )
                |> Seq.toList
            else
                []

        let documents = archimateDocs @ governanceDocs
        let documentsById =
            documents
            |> List.fold (fun acc doc -> Map.add doc.id doc acc) Map.empty

        let duplicates =
            documents
            |> List.groupBy (fun doc -> doc.id)
            |> List.choose (fun (docId, items) -> if items.Length > 1 then Some docId else None)

        let knownIds =
            documents
            |> List.map (fun doc -> doc.id)
            |> Set.ofList

        let archimateById =
            archimateDocs
            |> List.choose (fun doc ->
                doc.metadata.archimate
                |> Option.map (fun archimate ->
                    doc.id, ElementType.parseElementType archimate.layerValue archimate.elementType
                )
            )
            |> Map.ofList

        let validationErrors =
            let errors = ResizeArray<ValidationError>()
            errors.AddRange(
                documents
                |> List.collect (fun doc -> validateDocument doc knownIds archimateById)
            )

            for dupId in duplicates do
                errors.Add({
                    filePath = elementsPath
                    elementId = Some dupId
                    errorType = ErrorType.Unknown "duplicate-id"
                    message = sprintf "Duplicate ID '%s' appears in multiple documents" dupId
                    severity = Severity.Error
                })

            List.ofSeq errors

        let documentsByKind =
            documents
            |> List.groupBy (fun doc -> doc.kind)
            |> List.map (fun (kind, docs) -> kind, docs |> List.map (fun doc -> doc.id) |> List.sort)
            |> Map.ofList

        let documentsByElementType =
            archimateDocs
            |> List.choose (fun doc ->
                doc.metadata.archimate
                |> Option.map (fun archimate ->
                    ElementType.parseElementType archimate.layerValue archimate.elementType, doc.id
                )
            )
            |> List.groupBy fst
            |> List.map (fun (elementType, items) -> elementType, items |> List.map snd |> List.sort)
            |> Map.ofList

        let documentsByGovernanceType =
            governanceDocs
            |> List.groupBy (fun doc -> docTypeFromPath doc.filePath)
            |> List.map (fun (docType, docs) -> docType, docs |> List.map (fun doc -> doc.id) |> List.sort)
            |> Map.ofList

        let relations =
            documents
            |> List.collect (fun doc ->
                doc.metadata.relationships
                |> List.map (fun rel ->
                    {
                        sourceId = doc.id
                        targetId = rel.target
                        relationType = ElementType.relationTypeToString rel.relationType
                        description = rel.description
                    }
                )
            )

        if not validationErrors.IsEmpty then
            logger.LogWarning("Unified repository validation found {errorCount} errors", validationErrors.Length)

        {
            documents = documentsById
            documentsByKind = documentsByKind
            documentsByElementType = documentsByElementType
            documentsByGovernanceType = documentsByGovernanceType
            relations = relations
            validationErrors = validationErrors
        }
