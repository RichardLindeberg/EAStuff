namespace EAArchive

open System
open System.IO
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions

module GovernanceRegistryLoader =

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

    let private normalizeKey (value: string) : string =
        value.Trim().TrimEnd(':').ToLowerInvariant()

    let private tryParseKeyValue (line: string) : (string * string) option =
        let parts = line.Split([| ':' |], 2, StringSplitOptions.None)
        if parts.Length = 2 then
            Some (normalizeKey parts.[0], parts.[1].Trim())
        else
            None

    let private splitMetadata (lines: string array) : (Map<string, string> * GovernanceRelation list * string) =
        let metadataIndex =
            lines
            |> Array.tryFindIndex (fun line -> line.Trim().Equals("**Metadata**", StringComparison.OrdinalIgnoreCase))

        match metadataIndex with
        | None -> Map.empty, [], String.concat "\n" lines
        | Some index ->
            let mutable endIdx = index + 1
            while endIdx < lines.Length && not (String.IsNullOrWhiteSpace(lines.[endIdx])) do
                endIdx <- endIdx + 1

            let metadataLines =
                if endIdx > index + 1 then
                    lines.[index + 1..endIdx - 1]
                else
                    [||]

            let mutable inRelations = false
            let metadata = System.Collections.Generic.Dictionary<string, string>()
            let relations = System.Collections.Generic.List<GovernanceRelation>()
            let mutable currentType: string option = None

            let tryCommitRelation (targetValue: string) =
                match currentType with
                | Some relType when not (String.IsNullOrWhiteSpace targetValue) ->
                    relations.Add({ relationType = relType; target = targetValue })
                    currentType <- None
                | _ -> ()

            for rawLine in metadataLines do
                let trimmed = rawLine.Trim()
                if trimmed.StartsWith("- ") then
                    let entry = trimmed.Substring(2).Trim()
                    if entry.StartsWith("Relations", StringComparison.OrdinalIgnoreCase) then
                        inRelations <- true
                    elif inRelations && (entry.StartsWith("type", StringComparison.OrdinalIgnoreCase) || entry.StartsWith("target", StringComparison.OrdinalIgnoreCase)) then
                        match tryParseKeyValue entry with
                        | Some (key, value) when key = "type" && value <> "" ->
                            currentType <- Some value
                        | Some (key, value) when key = "target" ->
                            tryCommitRelation value
                        | _ -> ()
                    else
                        if inRelations then
                            inRelations <- false

                        match tryParseKeyValue entry with
                        | Some (key, value) when value <> "" -> metadata.[key] <- value
                        | _ -> ()
                else if inRelations then
                    let relationLine = trimmed

                    match tryParseKeyValue relationLine with
                    | Some (key, value) when key = "type" && value <> "" ->
                        currentType <- Some value
                    | Some (key, value) when key = "target" ->
                        tryCommitRelation value
                    | _ -> ()

            let contentLines =
                let cutEnd = if endIdx < lines.Length && String.IsNullOrWhiteSpace(lines.[endIdx]) then endIdx else endIdx - 1
                lines
                |> Array.mapi (fun i line ->
                    if i >= index && i <= cutEnd then None else Some line
                )
                |> Array.choose id

            let metadataMap =
                metadata
                |> Seq.map (fun kvp -> kvp.Key, kvp.Value)
                |> Map.ofSeq

            metadataMap, relations |> Seq.toList, String.concat "\n" contentLines

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
        let lines = content.Split([| "\r\n"; "\n" |], StringSplitOptions.None)
        let metadata, relations, contentWithoutMetadata = splitMetadata lines
        let title =
            tryGetTitle contentWithoutMetadata
            |> Option.defaultValue slug

        let docId =
            metadata
            |> Map.tryFind "document id"
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

    let createWithLogger (managementSystemPath: string) (logger: ILogger) : GovernanceRegistry =
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

            {
                documents = docMap
                documentsByType = docsByType
                managementSystemPath = managementSystemPath
            }
        else
            logger.LogWarning("Governance management system path not found: {path}", managementSystemPath)
            {
                documents = Map.empty
                documentsByType = Map.empty
                managementSystemPath = managementSystemPath
            }

    let create (managementSystemPath: string) : GovernanceRegistry =
        createWithLogger managementSystemPath (NullLogger.Instance)
