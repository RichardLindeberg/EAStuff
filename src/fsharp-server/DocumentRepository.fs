namespace EAArchive

open System
open System.Collections
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions
open Microsoft.Extensions.Logging
open YamlDotNet.Serialization

module DocumentRepositoryLoader =

    let private parseFrontmatterFallback (yamlContent: string) : Map<string, obj> =
        let lines = yamlContent.Split('\n')
        let top = Dictionary<string, obj>()
        let relationships = List<obj>()
        let mutable currentSection: string option = None
        let mutable currentRel: Dictionary<string, obj> option = None

        let setTop (key: string) (value: obj) : unit =
            if top.ContainsKey key then
                top.[key] <- value
            else
                top.Add(key, value)

        let tryParseKeyValue (text: string) : (string * string) option =
            let idx = text.IndexOf(':')
            if idx <= 0 then None
            else
                let key = text.Substring(0, idx).Trim()
                let value = text.Substring(idx + 1).Trim().Trim('"', '\'')
                Some (key, value)

        for line in lines do
            let trimmed = line.Trim()
            if trimmed = "" then
                ()
            elif trimmed.StartsWith("#") then
                ()
            elif trimmed.EndsWith(":") then
                let section = trimmed.TrimEnd(':').Trim()
                match section with
                | "relationships" ->
                    currentSection <- Some "relationships"
                    currentRel <- None
                    if not (top.ContainsKey "relationships") then
                        setTop "relationships" (relationships :> obj)
                | "archimate"
                | "governance" ->
                    let nested = Dictionary<string, obj>()
                    setTop section (nested :> obj)
                    currentSection <- Some section
                    currentRel <- None
                | _ ->
                    currentSection <- None
                    currentRel <- None
            elif trimmed.StartsWith("relationships:") then
                currentSection <- Some "relationships"
                currentRel <- None
                if trimmed.EndsWith("[]") then
                    setTop "relationships" (List<obj>() :> obj)
                elif not (top.ContainsKey "relationships") then
                    setTop "relationships" (relationships :> obj)
            elif trimmed.StartsWith("-") then
                match currentSection with
                | Some "relationships" ->
                    let rel = Dictionary<string, obj>()
                    relationships.Add(rel :> obj)
                    currentRel <- Some rel
                    let remainder = trimmed.TrimStart('-').Trim()
                    match tryParseKeyValue remainder with
                    | Some (key, value) -> rel.[key] <- value :> obj
                    | None -> ()
                | _ -> ()
            else
                let isIndented = line.StartsWith(" ") || line.StartsWith("\t")
                match tryParseKeyValue trimmed with
                | Some (key, value) ->
                    match currentSection with
                    | Some section when section = "archimate" || section = "governance" ->
                        match Map.tryFind section (top |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Map.ofSeq) with
                        | Some (:? Dictionary<string, obj> as nested) -> nested.[key] <- value :> obj
                        | _ ->
                            let nested = Dictionary<string, obj>()
                            nested.[key] <- value :> obj
                            setTop section (nested :> obj)
                    | Some "relationships" when isIndented ->
                        match currentRel with
                        | Some rel -> rel.[key] <- value :> obj
                        | None -> ()
                    | _ ->
                        currentSection <- None
                        currentRel <- None
                        setTop key (value :> obj)
                | None -> ()

        top
        |> Seq.map (fun kvp -> kvp.Key, kvp.Value)
        |> Map.ofSeq

    let private parseFrontmatter (content: string) : (Map<string, obj> * string) =
        let normalizedContent =
            if content.StartsWith("\uFEFF") then
                content.Substring(1)
            else
                content

        let lines = normalizedContent.Split('\n')
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
                    let parsed =
                        if isNull data then
                            Map.empty
                        else
                            data |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Map.ofSeq

                    let fallback = parseFrontmatterFallback yamlContent
                    let merged =
                        fallback
                        |> Map.fold (fun acc key value ->
                            if Map.containsKey key acc then acc else Map.add key value acc
                        ) parsed

                    if merged.Count = 0 && not (String.IsNullOrWhiteSpace yamlContent) then
                        (fallback, markdownContent)
                    else
                        (merged, markdownContent)
                with
                | _ -> (parseFrontmatterFallback yamlContent, markdownContent)
            else
                (Map.empty, content)
        else
            (Map.empty, content)

    let private normalizeKey (value: string) : string =
        value.Trim().ToLowerInvariant().Replace(" ", "_").Replace("-", "_")

    let private toStringOption (value: obj) : string option =
        if isNull value then None
        else
            let text = value.ToString().Trim()
            if text = "" then None else Some text

    let private toString (value: obj) : string =
        toStringOption value |> Option.defaultValue ""

    let private normalizeMetadata (metadata: Map<string, obj>) : Map<string, obj> =
        metadata
        |> Map.fold (fun acc key value ->
            let normalized = normalizeKey key
            Map.add normalized value acc
        ) Map.empty

    let private tryGetValue (key: string) (metadata: Map<string, obj>) : obj option =
        metadata |> Map.tryFind (normalizeKey key)

    let private tryGetString (key: string) (metadata: Map<string, obj>) : string option =
        tryGetValue key metadata |> Option.bind toStringOption

    let private tryGetStringFromMap (key: string) (metadata: Map<string, obj>) : string option =
        metadata |> Map.tryFind (normalizeKey key) |> Option.bind toStringOption

    let private tryGetDict (key: string) (metadata: Map<string, obj>) : Map<string, obj> option =
        tryGetValue key metadata
        |> Option.bind (fun value ->
            match value with
            | :? IDictionary as dict ->
                dict
                |> Seq.cast<obj>
                |> Seq.choose (fun entry ->
                    match entry with
                    | :? DictionaryEntry as de ->
                        match de.Key with
                        | :? string as key -> Some (key, de.Value)
                        | _ -> None
                    | :? KeyValuePair<obj, obj> as kvp ->
                        match kvp.Key with
                        | :? string as key -> Some (key, kvp.Value)
                        | _ -> None
                    | _ -> None
                )
                |> Map.ofSeq
                |> Some
            | _ -> None
        )

    let private parseTags (metadata: Map<string, obj>) : string list =
        tryGetValue "tags" metadata
        |> Option.map (fun value ->
            match value with
            | :? string as s ->
                s.Split(',')
                |> Array.map (fun t -> t.Trim())
                |> Array.filter (fun t -> t <> "")
                |> Array.toList
            | :? List<obj> as list ->
                list
                |> Seq.choose toStringOption
                |> Seq.toList
            | :? List<string> as list ->
                list
                |> Seq.map (fun t -> t.Trim())
                |> Seq.filter (fun t -> t <> "")
                |> Seq.toList
            | _ -> []
        )
        |> Option.defaultValue []

    let private parseRelationships (metadata: Map<string, obj>) : Relationship list =
        tryGetValue "relationships" metadata
        |> Option.bind (fun value ->
            match value with
            | :? List<obj> as list ->
                list
                |> Seq.choose (fun item ->
                    match item with
                    | :? IDictionary as dict ->
                        let relType = if dict.Contains("type") then toStringOption dict.["type"] else None
                        let target = if dict.Contains("target") then toStringOption dict.["target"] else None
                        let description = if dict.Contains("description") then toStringOption dict.["description"] else None
                        match relType, target with
                        | None, None -> None
                        | _ ->
                            let relTypeValue = relType |> Option.defaultValue ""
                            let targetValue = target |> Option.defaultValue ""
                            Some {
                                target = targetValue
                                relationType = ElementType.parseRelationType relTypeValue
                                description = description |> Option.defaultValue ""
                            }
                    | _ -> None
                )
                |> Seq.toList
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

    let private buildExtensions (metadata: Map<string, obj>) : Map<string, obj> =
        let normalized = normalizeMetadata metadata
        let extensionsSection =
            tryGetDict "extensions" metadata
            |> Option.defaultValue Map.empty

        let extras =
            normalized
            |> Map.filter (fun key _ -> not (Set.contains key knownExtensionKeys))

        extras
        |> Map.fold (fun acc key value ->
            if Map.containsKey key acc then acc else Map.add key value acc
        ) extensionsSection

    let private parseArchimateMetadata (metadata: Map<string, obj>) : ArchimateMetadata option =
        let archimateSection =
            tryGetDict "archimate" metadata
            |> Option.map normalizeMetadata
            |> Option.defaultValue Map.empty

        let typeValue = tryGetStringFromMap "type" archimateSection
        let layerValue = tryGetStringFromMap "layer" archimateSection

        match typeValue, layerValue with
        | Some typeValue, Some layerValue ->
            let criticality = tryGetStringFromMap "criticality" archimateSection
            Some {
                elementType = typeValue
                layer = Layer.parse layerValue
                layerValue = layerValue
                criticality = criticality
            }
        | _ -> None

    let private parseGovernanceMetadata (metadata: Map<string, obj>) : GovernanceMetadata option =
        let governanceSection =
            tryGetDict "governance" metadata
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
        (metadata: Map<string, obj>)
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
        let metadataRaw, contentWithoutMetadata = parseFrontmatter content
        let metadata = normalizeMetadata metadataRaw
        let relationships = parseRelationships metadataRaw
        let tags = parseTags metadataRaw

        let name = tryGetString "name" metadata |> Option.defaultValue slug
        let idValue = tryGetString "id" metadata |> Option.defaultValue slug
        let hasExplicitId = tryGetString "id" metadata |> Option.isSome
        let hasExplicitName = tryGetString "name" metadata |> Option.isSome
        let governance = None
        let archimate = parseArchimateMetadata metadataRaw

        let documentMetadata =
            buildDocumentMetadata idValue hasExplicitId hasExplicitName metadataRaw relationships governance archimate tags

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
        let metadataRaw, contentWithoutMetadata = parseFrontmatter content
        let metadata = normalizeMetadata metadataRaw
        let relationships = parseRelationships metadataRaw
        let tags = parseTags metadataRaw

        let title = tryGetString "name" metadata |> Option.defaultValue slug
        let idValue = tryGetString "id" metadata |> Option.defaultValue slug
        let hasExplicitId = tryGetString "id" metadata |> Option.isSome
        let hasExplicitName = tryGetString "name" metadata |> Option.isSome
        let governance = parseGovernanceMetadata metadataRaw
        let archimate = None

        let documentMetadata =
            buildDocumentMetadata idValue hasExplicitId hasExplicitName metadataRaw relationships governance archimate tags

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
            match Layer.tryParse archimate.layerValue with
            | Some _ -> ()
            | None ->
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

        let documentsByLayer =
            archimateDocs
            |> List.choose (fun doc ->
                doc.metadata.archimate
                |> Option.map (fun archimate -> archimate.layer, doc.id)
            )
            |> List.groupBy fst
            |> List.map (fun (layer, items) -> layer, items |> List.map snd |> List.sort)
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
            documentsByLayer = documentsByLayer
            documentsByGovernanceType = documentsByGovernanceType
            relations = relations
            validationErrors = validationErrors
        }
