namespace EAArchive

open System

// Lightweight fallback YAML sketch with explicit shape.
type SimpleYaml =
    | Value of string
    | Map of Map<string, SimpleYaml>
    | List of SimpleYaml list
    

module SimpleYaml =

    let private normalizeKey (value: string) : string =
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
                                    let updated = Map.add key (List listItems) acc
                                    loop updated remainingAfter
                                | _ ->
                                    let childMap, remainingAfter = parseMap childIndent afterHeader
                                    let updated = Map.add key (Map childMap) acc
                                    loop updated remainingAfter
                            else
                                match tryParseKeyValue trimmed with
                                | Some (k, v) -> loop (Map.add k (Value v) acc) tail
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
                                    loop (Map itemMap :: acc) remainingAfter
                                else
                                    match tryParseKeyValue itemText with
                                    | Some (k, v) ->
                                        let baseMap = Map.add k (Value v) Map.empty
                                        let extraMap, remainingAfter = parseMap (currentIndent + 2) tail
                                        let merged = mergeMaps baseMap extraMap
                                        loop (Map merged :: acc) remainingAfter
                                    | None ->
                                        loop (Value itemText :: acc) tail
                            else
                                List.rev acc, rest

            loop [] remaining

        let parsed, _ = parseMap 0 lines
        parsed
