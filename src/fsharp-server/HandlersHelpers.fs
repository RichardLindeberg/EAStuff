namespace EAArchive

open System
open System.Collections.Generic
open System.Globalization
open System.Text.RegularExpressions
open Microsoft.Extensions.Logging
open Giraffe

module HandlersHelpers =

    let buildFrontmatter
        (id: string)
        (name: string)
        (elementType: string)
        (layer: string)
        (relationships: (string * string * string) list)
        (properties: (string * string) list)
        (tags: string list)
        : string =
        let baseEntries = ResizeArray<string * SimpleYaml>()
        baseEntries.Add("id", SimpleYaml.Value id)
        baseEntries.Add("name", SimpleYaml.Value name)

        let propertyMap =
            properties
            |> List.fold (fun acc (key, value) -> Map.add (SimpleYaml.normalizeKey key) value acc) Map.empty

        let tryGetProperty key =
            propertyMap
            |> Map.tryFind (SimpleYaml.normalizeKey key)
            |> Option.filter (fun value -> not (String.IsNullOrWhiteSpace value))

        let sharedKeys = set [ "owner"; "status"; "version"; "last_updated"; "review_cycle"; "next_review" ]

        let addSharedField key =
            match tryGetProperty key with
            | Some value -> baseEntries.Add(key, SimpleYaml.Value value)
            | None -> ()

        addSharedField "owner"
        addSharedField "status"
        addSharedField "version"
        addSharedField "last_updated"
        addSharedField "review_cycle"
        addSharedField "next_review"

        let archimateEntries = ResizeArray<string * SimpleYaml>()
        archimateEntries.Add("type", SimpleYaml.Value elementType)
        archimateEntries.Add("layer", SimpleYaml.Value layer)
        match tryGetProperty "criticality" with
        | Some value -> archimateEntries.Add("criticality", SimpleYaml.Value value)
        | None -> ()
        baseEntries.Add("archimate", SimpleYaml.Map (Map.ofList (List.ofSeq archimateEntries)))

        if not (List.isEmpty relationships) then
            let relItems =
                relationships
                |> List.map (fun (relType, target, desc) ->
                    let relEntries = ResizeArray<string * SimpleYaml>()
                    relEntries.Add("type", SimpleYaml.Value relType)
                    relEntries.Add("target", SimpleYaml.Value target)
                    if not (String.IsNullOrWhiteSpace desc) then
                        relEntries.Add("description", SimpleYaml.Value desc)
                    SimpleYaml.Map (Map.ofList (List.ofSeq relEntries))
                )
            baseEntries.Add("relationships", SimpleYaml.List relItems)

        let extensionProps =
            properties
            |> List.choose (fun (key, value) ->
                let normalized = SimpleYaml.normalizeKey key
                if normalized = "criticality" || Set.contains normalized sharedKeys then
                    None
                elif String.IsNullOrWhiteSpace value then
                    None
                else
                    Some (key, value)
            )

        if not (List.isEmpty extensionProps) then
            let props =
                extensionProps
                |> List.map (fun (key, value) -> key, SimpleYaml.Value value)
                |> Map.ofList
            baseEntries.Add("properties", SimpleYaml.Map props)

        if not (List.isEmpty tags) then
            baseEntries.Add("tags", SimpleYaml.List (tags |> List.map SimpleYaml.Value))

        let yaml =
            SimpleYaml.Map (Map.ofList (List.ofSeq baseEntries))
            |> SimpleYaml.render

        $"---\n{yaml}\n---"

    let private sanitizeName (value: string) : string =
        let lower = value.Trim().ToLowerInvariant()
        let spaced = Regex.Replace(lower, "[\s_]+", "-")
        let cleaned = Regex.Replace(spaced, "[^a-z0-9-]", "")
        let trimmed = cleaned.Trim('-')
        let collapsed = Regex.Replace(trimmed, "-+", "-")
        if collapsed.Length > 30 then
            let parts = collapsed.Split('-') |> Array.toList
            let prefix = parts |> List.truncate 4 |> String.concat "-"
            prefix.Substring(0, min 30 prefix.Length).TrimEnd('-')
        else
            collapsed

    let private nextSequence (existingIds: string list) (layerCode: string) (typeCode: string) (namePart: string) : int =
        let pattern =
            let escapedName = Regex.Escape(namePart)
            Regex($"^{layerCode}-{typeCode}-(\\d{{3}})-{escapedName}$")

        existingIds
        |> Seq.choose (fun elemId ->
            let matchResult = pattern.Match(elemId)
            if matchResult.Success then
                Some (Int32.Parse(matchResult.Groups.[1].Value))
            else
                None
        )
        |> Seq.fold (fun acc value -> max acc value) 0
        |> fun maxValue -> maxValue + 1

    let generateElementId (existingIds: string list) (layer: string) (typeValue: string) (name: string) : string =
        let layerKey = layer.Trim().ToLowerInvariant()
        let typeKey = typeValue.Trim().ToLowerInvariant()
        let layerCode = ElementIdCodes.layerNameToCode |> Map.tryFind layerKey |> Option.defaultValue "unk"
        let typeCode = ElementIdCodes.typeNameToCode |> Map.tryFind typeKey |> Option.defaultValue "type"
        let namePart =
            let sanitized = sanitizeName name
            if sanitized = "" then "new-element" else sanitized
        let sequenceNumber = nextSequence existingIds layerCode typeCode namePart
        let sequenceText = sequenceNumber.ToString("000")
        $"{layerCode}-{typeCode}-{sequenceText}-{namePart}"

    let relationCodeToName (code: char) : string option =
        match code with
        | 'c' -> Some "composition"
        | 'g' -> Some "aggregation"
        | 'i' -> Some "assignment"
        | 'r' -> Some "realization"
        | 's' -> Some "specialization"
        | 'o' -> Some "association"
        | 'a' -> Some "access"
        | 'n' -> Some "influence"
        | 'v' -> Some "serving"
        | 't' -> Some "triggering"
        | 'f' -> Some "flow"
        | _ -> None

    let respondNotFound (logger: ILogger) (logTemplate: string) (value: string) (message: string) : HttpHandler =
        logger.LogWarning(logTemplate, value)
        setStatusCode 404 >=> text message
