namespace EAArchive.Views

open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Architecture =

    /// Architecture overview page
    let indexPage (webConfig: WebUiConfig) (elementTypeCards: (ElementType * int) list) (currentPage: string) =
        let baseUrl = webConfig.BaseUrl
        let cards =
            elementTypeCards
            |> List.sortBy (fun (elementType, _) -> ElementType.getLayerKey elementType)
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

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Architecture"]
                p [_class "layer-description"] [
                    encodedText "Explore the enterprise architecture organized by ArchiMate element types, from strategic intent to technical implementation."
                ]
                div [_class "element-grid"] cards
            ]
        ]

        htmlPage webConfig "Architecture" currentPage content
