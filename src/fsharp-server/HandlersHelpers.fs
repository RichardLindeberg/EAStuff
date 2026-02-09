namespace EAArchive

open System
open System.Collections.Generic
open System.Globalization
open System.Text.RegularExpressions
open YamlDotNet.Serialization

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
        let data = Dictionary<string, obj>()
        data.Add("id", id)
        data.Add("name", name)

        let normalizeKey (value: string) =
            value.Trim().ToLowerInvariant().Replace(" ", "_").Replace("-", "_")

        let propertyMap =
            properties
            |> List.fold (fun acc (key, value) -> Map.add (normalizeKey key) value acc) Map.empty

        let tryGetProperty key =
            propertyMap
            |> Map.tryFind (normalizeKey key)
            |> Option.filter (fun value -> not (String.IsNullOrWhiteSpace value))

        let sharedKeys = set [ "owner"; "status"; "version"; "last_updated"; "review_cycle"; "next_review" ]

        let addSharedField key =
            match tryGetProperty key with
            | Some value -> data.Add(key, value)
            | None -> ()

        addSharedField "owner"
        addSharedField "status"
        addSharedField "version"
        addSharedField "last_updated"
        addSharedField "review_cycle"
        addSharedField "next_review"

        let archimate = Dictionary<string, obj>()
        archimate.Add("type", elementType)
        archimate.Add("layer", layer)
        match tryGetProperty "criticality" with
        | Some value -> archimate.Add("criticality", value)
        | None -> ()
        data.Add("archimate", archimate)

        if not (List.isEmpty relationships) then
            let relList = ResizeArray<obj>()
            for (relType, target, desc) in relationships do
                let rel = Dictionary<string, obj>()
                rel.Add("type", relType)
                rel.Add("target", target)
                if not (String.IsNullOrWhiteSpace desc) then
                    rel.Add("description", desc)
                relList.Add(rel :> obj)
            data.Add("relationships", relList)

        let extensionProps =
            properties
            |> List.choose (fun (key, value) ->
                let normalized = normalizeKey key
                if normalized = "criticality" || Set.contains normalized sharedKeys then
                    None
                elif String.IsNullOrWhiteSpace value then
                    None
                else
                    Some (key, value)
            )

        if not (List.isEmpty extensionProps) then
            let props = Dictionary<string, obj>()
            for (key, value) in extensionProps do
                props.Add(key, value)
            data.Add("properties", props)

        if not (List.isEmpty tags) then
            let tagList = ResizeArray<obj>()
            for tag in tags do
                tagList.Add(tag)
            data.Add("tags", tagList)

        let serializer = SerializerBuilder().Build()
        let yaml = serializer.Serialize(data).TrimEnd()
        $"---\n{yaml}\n---"

    let layerCodes =
        Map.ofList [
            ("strategy", "str")
            ("business", "bus")
            ("application", "app")
            ("technology", "tec")
            ("physical", "phy")
            ("motivation", "mot")
            ("implementation", "imp")
        ]

    let typeCodes =
        Map.ofList [
            ("resource", "rsrc")
            ("capability", "capa")
            ("value-stream", "vstr")
            ("course-of-action", "cact")
            ("business-actor", "actr")
            ("business-role", "role")
            ("business-collaboration", "colab")
            ("business-interface", "intf")
            ("business-process", "proc")
            ("business-function", "func")
            ("business-interaction", "intr")
            ("business-event", "evnt")
            ("business-service", "srvc")
            ("business-object", "objt")
            ("contract", "cntr")
            ("representation", "repr")
            ("product", "prod")
            ("application-component", "comp")
            ("application-collaboration", "colab")
            ("application-interface", "intf")
            ("application-function", "func")
            ("application-interaction", "intr")
            ("application-process", "proc")
            ("application-event", "evnt")
            ("application-service", "srvc")
            ("data-object", "data")
            ("node", "node")
            ("device", "devc")
            ("system-software", "sysw")
            ("technology-collaboration", "colab")
            ("technology-interface", "intf")
            ("path", "path")
            ("communication-network", "netw")
            ("technology-function", "func")
            ("technology-process", "proc")
            ("technology-interaction", "intr")
            ("technology-event", "evnt")
            ("technology-service", "srvc")
            ("artifact", "artf")
            ("equipment", "equi")
            ("facility", "faci")
            ("distribution-network", "dist")
            ("material", "matr")
            ("stakeholder", "stkh")
            ("driver", "drvr")
            ("assessment", "asmt")
            ("goal", "goal")
            ("outcome", "outc")
            ("principle", "prin")
            ("requirement", "reqt")
            ("constraint", "cnst")
            ("meaning", "mean")
            ("value", "valu")
            ("work-package", "work")
            ("deliverable", "delv")
            ("implementation-event", "evnt")
            ("plateau", "plat")
            ("gap", "gap")
        ]

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
        let layerCode = layerCodes |> Map.tryFind layerKey |> Option.defaultValue "unk"
        let typeCode = typeCodes |> Map.tryFind typeKey |> Option.defaultValue "type"
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

    /// Build tag index from document list
    let buildTagIndex (docs: DocumentRecord list) : Map<string, string list> =
        docs
        |> List.fold (fun acc doc ->
            doc.metadata.tags
            |> List.fold (fun tagMap tag ->
                match Map.tryFind tag tagMap with
                | Some ids -> Map.add tag (doc.id :: ids) tagMap
                | None -> Map.add tag [doc.id] tagMap
            ) acc
        ) Map.empty
