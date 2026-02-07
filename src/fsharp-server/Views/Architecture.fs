namespace EAArchive.Views

open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Architecture =

    /// Architecture overview page
    let indexPage (webConfig: WebUiConfig) (registry: ElementRegistry) (currentPage: string) =
        let baseUrl = webConfig.BaseUrl
        let layerCards =
            Config.layerOrder
            |> Map.toList
            |> List.sortBy (fun (_, layerInfo) -> layerInfo.order)
            |> List.choose (fun (layerKey, layerInfo) ->
                let layerKeyLower = Layer.toKey layerKey
                let elements = ElementRegistry.getLayerElements layerKey registry
                if List.isEmpty elements then None
                else
                    Some (
                        div [_class "element-card"] [
                            h3 [] [
                                a [_href $"{baseUrl}{layerKeyLower}"] [
                                    encodedText layerInfo.displayName
                                ]
                            ]
                            let elemCountStr =
                                let count = List.length elements
                                sprintf "%d element%s" count (pluralize count "" "s")
                            p [_class "element-count"] [
                                encodedText elemCountStr
                            ]
                            p [_class "element-description"] [
                                encodedText $"View all {layerKeyLower} layer elements and their relationships."
                            ]
                        ]
                    )
            )

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Architecture"]
                p [_class "layer-description"] [
                    encodedText "Explore the enterprise architecture organized by ArchiMate layers, from strategic intent to technical implementation."
                ]
                div [_class "element-grid"] layerCards
            ]
        ]

        htmlPage webConfig "Architecture" currentPage content
