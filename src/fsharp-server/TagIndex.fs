namespace EAArchive

module TagIndex =

    let buildTagIndex (docs: DocumentRecord list) : Map<string, string list> =
        docs
        |> List.fold (fun acc doc ->
            doc.tags
            |> List.fold (fun tagMap tag ->
                match Map.tryFind tag tagMap with
                | Some ids -> Map.add tag (doc.id :: ids) tagMap
                | None -> Map.add tag [ doc.id ] tagMap
            ) acc
        ) Map.empty
