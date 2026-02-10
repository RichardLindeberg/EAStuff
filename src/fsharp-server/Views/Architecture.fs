namespace EAArchive.Views

open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Architecture =

    let private buildElementTypeCards (webConfig: WebUiConfig) (elementTypeCards: (ElementType * int) list) : XmlNode list =
        let baseUrl = webConfig.BaseUrl
        elementTypeCards
        |> List.sortBy (fun (elementType, _) -> elementTypeAndSubTypeToString elementType)
        |> List.choose (fun (elementType, count) ->
            if count = 0 then None
            else
                let layerKey = ElementType.getLayerKey elementType
                let typeKey = ElementType.getTypeKey elementType
                let label = elementTypeAndSubTypeToString elementType
                Some (
                    div [_class "element-card"] [
                        h3 [] [
                            a [_href $"{baseUrl}elements/type/{layerKey}/{typeKey}"] [
                                encodedText label
                            ]
                        ]
                        let elemCountStr = sprintf "%d element%s" count (pluralize count "" "s")
                        p [_class "element-count"] [
                            encodedText elemCountStr
                        ]
                        p [_class "element-description"] [
                            encodedText $"View all {label} elements and their relationships."
                        ]
                    ]
                )
        )

    let private buildLayerCards (webConfig: WebUiConfig) (elementTypeCards: (ElementType * int) list) : XmlNode list =
        let baseUrl = webConfig.BaseUrl
        let layerCounts =
            elementTypeCards
            |> List.groupBy (fun (elementType, _) -> ElementType.getLayerKey elementType)
            |> List.map (fun (layerKey, items) -> layerKey, items |> List.sumBy snd)
            |> Map.ofList

        let orderedLayers =
            Config.layerOrder
            |> Map.toList
            |> List.sortBy (fun (_, info) -> info.order)

        let knownLayerKeys =
            orderedLayers
            |> List.map fst
            |> Set.ofList

        let knownLayers =
            orderedLayers
            |> List.choose (fun (layerKey, info) ->
                match Map.tryFind layerKey layerCounts with
                | Some count when count > 0 ->
                    Some (
                        div [_class "element-card"] [
                            h3 [] [
                                a [_href $"{baseUrl}architecture/layer/{layerKey}"] [
                                    encodedText info.displayName
                                ]
                            ]
                            let elemCountStr = sprintf "%d element%s" count (pluralize count "" "s")
                            p [_class "element-count"] [
                                encodedText elemCountStr
                            ]
                            p [_class "element-description"] [
                                encodedText $"View all {info.displayName} elements and their relationships."
                            ]
                        ]
                    )
                | _ -> None
            )

        let extraLayers =
            layerCounts
            |> Map.toList
            |> List.filter (fun (layerKey, count) -> count > 0 && not (Set.contains layerKey knownLayerKeys))
            |> List.sortBy fst
            |> List.map (fun (layerKey, count) ->
                div [_class "element-card"] [
                    h3 [] [
                        a [_href $"{baseUrl}architecture/layer/{layerKey}"] [
                            encodedText layerKey
                        ]
                    ]
                    let elemCountStr = sprintf "%d element%s" count (pluralize count "" "s")
                    p [_class "element-count"] [
                        encodedText elemCountStr
                    ]
                    p [_class "element-description"] [
                        encodedText $"View all {layerKey} elements and their relationships."
                    ]
                ]
            )

        knownLayers @ extraLayers

    /// Architecture overview page
    let indexPage (webConfig: WebUiConfig) (elementTypeCards: (ElementType * int) list) (currentPage: string) =
        let cards = buildLayerCards webConfig elementTypeCards

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Architecture"]
                p [_class "layer-description"] [
                    encodedText "Explore the enterprise architecture organized by ArchiMate layers, from strategic intent to technical implementation."
                ]
                div [_class "element-grid"] cards
            ]
        ]

        htmlPage webConfig "Architecture" currentPage content

    /// Architecture layer page
    let layerPage (webConfig: WebUiConfig) (layerDisplayName: string) (elementTypeCards: (ElementType * int) list) (currentPage: string) =
        let baseUrl = webConfig.BaseUrl
        let cards = buildElementTypeCards webConfig elementTypeCards

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}architecture"] [encodedText "Architecture"]
                    encodedText " / "
                    encodedText layerDisplayName
                ]
                h2 [_class "layer-title"] [encodedText layerDisplayName]
                p [_class "layer-description"] [
                    encodedText $"View all {layerDisplayName} element types and their relationships."
                ]
                div [_class "element-grid"] cards
            ]
        ]

        htmlPage webConfig layerDisplayName currentPage content
