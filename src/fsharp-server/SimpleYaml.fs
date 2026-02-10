namespace EAArchive

open System

// Lightweight fallback YAML sketch with explicit shape.
[<RequireQualifiedAccess>]
type SimpleYaml =
    | Value of string
    | Map of Map<string, SimpleYaml>
    | List of SimpleYaml list
    

module SimpleYaml =

    let normalizeKey (value: string) : string =
        value.Trim().ToLowerInvariant().Replace(" ", "_").Replace("-", "_")

    let private tryParseKeyValue (text: string) : (string * string) option =
        let idx = text.IndexOf(':')
        if idx <= 0 then None
        else
            let key = text.Substring(0, idx).Trim() |> normalizeKey
            let value = text.Substring(idx + 1).Trim().Trim('"', '\'')
            Some (key, value)

    let private isIndented (line: string) =
        line.StartsWith(" ") || line.StartsWith("\t")

    // Parses a minimal subset:
    // - top-level key/value
    // - sections with nested key/values
    // - a list section with dash items
    let parse (yamlContent: string) : Map<string, SimpleYaml>  =
        let lines = yamlContent.Split('\n') |> Array.toList

        let indentOf (line: string) =
            line
            |> Seq.takeWhile (fun c -> c = ' ' || c = '\t')
            |> Seq.length

        let rec skipNoise (remaining: string list) : string list =
            match remaining with
            | [] -> []
            | line :: rest ->
                let trimmed = line.Trim()
                if trimmed = "" || trimmed.StartsWith("#") then
                    skipNoise rest
                else
                    remaining

        let mergeMaps (left: Map<string, SimpleYaml>) (right: Map<string, SimpleYaml>) : Map<string, SimpleYaml> =
            right |> Map.fold (fun acc key value -> Map.add key value acc) left

        let rec parseMap (currentIndent: int) (remaining: string list) : Map<string, SimpleYaml> * string list =
            let rec loop (acc: Map<string, SimpleYaml>) (rest: string list) : Map<string, SimpleYaml> * string list =
                match rest with
                | [] -> acc, []
                | line :: tail ->
                    let trimmed = line.Trim()
                    if trimmed = "" || trimmed.StartsWith("#") then
                        loop acc tail
                    else
                        let indent = indentOf line
                        if indent < currentIndent then
                            acc, rest
                        elif indent > currentIndent then
                            loop acc tail
                        else
                            if trimmed.EndsWith(":") then
                                let key = trimmed.TrimEnd(':').Trim() |> normalizeKey
                                let afterHeader = skipNoise tail
                                let childIndent =
                                    match afterHeader with
                                    | nextLine :: _ -> max (indentOf nextLine) (currentIndent + 2)
                                    | [] -> currentIndent + 2

                                match afterHeader with
                                | nextLine :: _ when nextLine.Trim().StartsWith("-") ->
                                    let listItems, remainingAfter = parseList childIndent afterHeader
                                    let updated = Map.add key (SimpleYaml.List listItems) acc
                                    loop updated remainingAfter
                                | _ ->
                                    let childMap, remainingAfter = parseMap childIndent afterHeader
                                    let updated = Map.add key (SimpleYaml.Map childMap) acc
                                    loop updated remainingAfter
                            else
                                match tryParseKeyValue trimmed with
                                | Some (k, v) -> loop (Map.add k (SimpleYaml.Value v) acc) tail
                                | None -> loop acc tail

            loop Map.empty remaining

        and parseList (currentIndent: int) (remaining: string list) : SimpleYaml list * string list =
            let rec loop (acc: SimpleYaml list) (rest: string list) : SimpleYaml list * string list =
                match rest with
                | [] -> List.rev acc, []
                | line :: tail ->
                    let trimmed = line.Trim()
                    if trimmed = "" || trimmed.StartsWith("#") then
                        loop acc tail
                    else
                        let indent = indentOf line
                        if indent < currentIndent then
                            List.rev acc, rest
                        elif indent > currentIndent then
                            loop acc tail
                        else
                            if trimmed.StartsWith("-") then
                                let itemText = trimmed.TrimStart('-').Trim()
                                if itemText = "" then
                                    let itemMap, remainingAfter = parseMap (currentIndent + 2) tail
                                    loop (SimpleYaml.Map itemMap :: acc) remainingAfter
                                else
                                    match tryParseKeyValue itemText with
                                    | Some (k, v) ->
                                        let baseMap = Map.add k (SimpleYaml.Value v) Map.empty
                                        let extraMap, remainingAfter = parseMap (currentIndent + 2) tail
                                        let merged = mergeMaps baseMap extraMap
                                        loop (SimpleYaml.Map merged :: acc) remainingAfter
                                    | None ->
                                        loop (SimpleYaml.Value itemText :: acc) tail
                            else
                                List.rev acc, rest

            loop [] remaining

        let parsed, _ = parseMap 0 lines
        parsed

    let toStringOption (value: string) : string option =
        let text = value.Trim()
        if text = "" then None else Some text

    let tryGetValue (key: string) (map: Map<string, SimpleYaml>) : SimpleYaml option =
        map |> Map.tryFind (normalizeKey key)

    let tryGetString (key: string) (map: Map<string, SimpleYaml>) : string option =
        map
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (function
            | SimpleYaml.Value s -> toStringOption s
            | _ -> None
        )

    let tryGetMap (key: string) (map: Map<string, SimpleYaml>) : Map<string, SimpleYaml> option =
        map
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (function
            | SimpleYaml.Map items -> Some items
            | _ -> None
        )

    let tryGetList (key: string) (map: Map<string, SimpleYaml>) : SimpleYaml list option =
        map
        |> Map.tryFind (normalizeKey key)
        |> Option.bind (function
            | SimpleYaml.List items -> Some items
            | _ -> None
        )

    let rec normalizeValue (value: SimpleYaml) : SimpleYaml =
        match value with
        | SimpleYaml.Value s -> SimpleYaml.Value s
        | SimpleYaml.Map map ->
            map
            |> Map.fold (fun acc key item -> Map.add (normalizeKey key) (normalizeValue item) acc) Map.empty
            |> SimpleYaml.Map
        | SimpleYaml.List items ->
            items |> List.map normalizeValue |> SimpleYaml.List

    let normalizeMap (map: Map<string, SimpleYaml>) : Map<string, SimpleYaml> =
        map
        |> Map.fold (fun acc key value -> Map.add (normalizeKey key) (normalizeValue value) acc) Map.empty

    let private renderScalar (value: string) : string =
        let trimmed = value.Trim()
        let needsQuotes =
            trimmed = "" ||
            trimmed.StartsWith("-") ||
            trimmed.Contains(":") ||
            trimmed.Contains("#") ||
            trimmed.StartsWith("[") ||
            trimmed.StartsWith("{") ||
            trimmed.EndsWith(" ")

        if needsQuotes then
            let escaped = trimmed.Replace("\"", "\\\"")
            sprintf "\"%s\"" escaped
        else
            trimmed

    let rec private renderYaml (indent: int) (value: SimpleYaml) : string list =
        let indentText = String.replicate indent "  "
        match value with
        | SimpleYaml.Value text -> [ indentText + renderScalar text ]
        | SimpleYaml.Map entries ->
            entries
            |> Map.toList
            |> List.collect (fun (key, entryValue) ->
                match entryValue with
                | SimpleYaml.Value text -> [ sprintf "%s%s: %s" indentText key (renderScalar text) ]
                | _ ->
                    let header = sprintf "%s%s:" indentText key
                    header :: renderYaml (indent + 1) entryValue
            )
        | SimpleYaml.List items ->
            items
            |> List.collect (fun item ->
                match item with
                | SimpleYaml.Value text -> [ sprintf "%s- %s" indentText (renderScalar text) ]
                | _ ->
                    let header = sprintf "%s-" indentText
                    header :: renderYaml (indent + 1) item
            )

    let render (value: SimpleYaml) : string =
        renderYaml 0 value |> String.concat "\n"
