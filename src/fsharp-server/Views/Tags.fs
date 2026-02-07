namespace EAArchive.Views

open System
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Tags =

    /// Tags index page
    let tagsIndexPage (webConfig: WebUiConfig) (tagIndex: Map<string, string list>) (registry: ElementRegistry) =
        let baseUrl = webConfig.BaseUrl
        let tagCards =
            tagIndex
            |> Map.toList
            |> List.sortBy fst
            |> List.map (fun (tag, elemIds) ->
                let count = List.length elemIds
                div [_class "element-card"] [
                    h3 [] [
                        a [_href $"{baseUrl}tags/{tag}"] [
                            encodedText tag
                        ]
                    ]
                    p [_class "element-count"] [
                        let countStr = sprintf "%d element%s" count (pluralize count "" "s")
                        encodedText countStr
                    ]
                ]
            )

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / Tags"
                ]
                h2 [_class "layer-title"] [encodedText "Tags"]
                let tagsCountStr = let count = Map.count tagIndex in sprintf "%d tag%s" count (pluralize count "" "s")
                p [_class "element-count"] [
                    encodedText tagsCountStr
                ]
                div [_class "element-grid"] tagCards
            ]
        ]

        htmlPage webConfig "Tags" "tags" content

    /// Tag detail page
    let tagPage (webConfig: WebUiConfig) (tag: string) (elements: Element list) =
        let baseUrl = webConfig.BaseUrl
        let elementCards =
            elements
            |> List.sortBy (fun e -> e.name)
            |> List.map (fun elem ->
                let description =
                    elem.content.Split('\n')
                    |> Array.filter (fun line ->
                        let trimmed = line.Trim()
                        trimmed <> "" && not (trimmed.StartsWith("#"))
                    )
                    |> Array.tryHead
                    |> Option.map (fun line ->
                        if line.Length > 150 then line.Substring(0, 150) + "..." else line
                    )
                    |> Option.defaultValue ""

                div [_class "element-card"] [
                    span [_class "element-type"] [encodedText (elementTypeToString elem.elementType)]
                    h3 [] [
                        a [_href $"{baseUrl}elements/{elem.id}"] [
                            encodedText elem.name
                        ]
                    ]
                    p [_class "element-description"] [encodedText description]
                ]
            )

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}tags"] [encodedText "Tags"]
                    encodedText " / "
                    encodedText tag
                ]
                h2 [_class "layer-title"] [encodedText tag]
                let tagDetailCountStr = let count = List.length elements in sprintf "%d element%s" count (pluralize count "" "s")
                p [_class "element-count"] [
                    encodedText tagDetailCountStr
                ]
                div [_class "element-grid"] elementCards
            ]
        ]

        htmlPage webConfig tag "tags" content
