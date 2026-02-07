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
        data.Add("type", elementType)
        data.Add("layer", layer)

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

        if not (List.isEmpty properties) then
            let props = Dictionary<string, obj>()
            for (key, value) in properties do
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

    let private nextSequence (registry: ElementRegistry) (layerCode: string) (typeCode: string) (namePart: string) : int =
        let pattern =
            let escapedName = Regex.Escape(namePart)
            Regex($"^{layerCode}-{typeCode}-(\\d{{3}})-{escapedName}$")

        registry.elements
        |> Map.toSeq
        |> Seq.choose (fun (elemId, _) ->
            let matchResult = pattern.Match(elemId)
            if matchResult.Success then
                Some (Int32.Parse(matchResult.Groups.[1].Value))
            else
                None
        )
        |> Seq.fold (fun acc value -> max acc value) 0
        |> fun maxValue -> maxValue + 1

    let generateElementId (registry: ElementRegistry) (layer: string) (typeValue: string) (name: string) : string =
        let layerKey = layer.Trim().ToLowerInvariant()
        let typeKey = typeValue.Trim().ToLowerInvariant()
        let layerCode = layerCodes |> Map.tryFind layerKey |> Option.defaultValue "unk"
        let typeCode = typeCodes |> Map.tryFind typeKey |> Option.defaultValue "type"
        let namePart =
            let sanitized = sanitizeName name
            if sanitized = "" then "new-element" else sanitized
        let sequenceNumber = nextSequence registry layerCode typeCode namePart
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

    /// Build tag index from registry
    let buildTagIndex (registry: ElementRegistry) : Map<string, string list> =
        registry.elements
        |> Map.fold (fun acc elemId elem ->
            elem.tags
            |> List.fold (fun tagMap tag ->
                match Map.tryFind tag tagMap with
                | Some ids -> Map.add tag (elemId :: ids) tagMap
                | None -> Map.add tag [elemId] tagMap
            ) acc
        ) Map.empty
