namespace EAArchive.Views

open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Index =

    /// Index page
    let indexPage (webConfig: WebUiConfig) (registry: ElementRegistry) (governanceRegistry: GovernanceDocRegistry) =
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

        let governanceCard =
            let docCount = governanceRegistry.documents |> Map.count
            let docCountText = sprintf "%d document%s" docCount (pluralize docCount "" "s")
            div [_class "element-card"] [
                h3 [] [
                    a [_href $"{baseUrl}governance"] [
                        encodedText "Governance System"
                    ]
                ]
                p [_class "element-count"] [
                    encodedText docCountText
                ]
                p [_class "element-description"] [
                    encodedText "Policies, instructions, and manuals that govern how work is performed."
                ]
            ]

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Architecture Overview"]
                p [_class "layer-description"] [
                    encodedText "This repository contains the enterprise architecture organized by ArchiMate layers. Each layer represents a different aspect of the architecture, from strategic goals to technical implementation."
                ]
                div [_class "element-grid"] (layerCards @ [governanceCard])
            ]
        ]

        htmlPage webConfig "Home" "index" content
