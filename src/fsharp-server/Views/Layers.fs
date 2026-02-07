namespace EAArchive.Views

open System
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Htmx.HtmxAttrs
open Common

module Layers =

    let private buildElementCards (webConfig: WebUiConfig) (elements: Element list) (registry: ElementRegistry) : XmlNode list =
        let baseUrl = webConfig.BaseUrl
        elements
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

            let incoming = ElementRegistry.getIncomingRelations elem.id registry |> List.length
            let outgoing = List.length elem.relationships

            div [_class "element-card"] [
                span [_class "element-type"] [encodedText (elementTypeToString elem.elementType)]
                h3 [] [
                    a [_href $"{baseUrl}elements/{elem.id}"] [
                        encodedText elem.name
                    ]
                ]
                p [_class "element-description"] [encodedText description]
                let relStr = sprintf "%d outgoing, %d incoming relation%s" outgoing incoming (pluralize incoming "" "s")
                p [_style "margin-top: 0.75rem; font-size: 0.85rem; color: #888;"] [
                    encodedText relStr
                ]
            ]
        )

    let layerElementsPartial (webConfig: WebUiConfig) (elements: Element list) (registry: ElementRegistry) : XmlNode =
        let elementCards = buildElementCards webConfig elements registry
        let count = List.length elements
        let layerElemCountStr = sprintf "%d element%s" count (pluralize count "" "s")

        div [_id "layer-elements"] [
            p [_class "element-count"] [
                encodedText layerElemCountStr
            ]
            div [_class "element-grid"] elementCards
        ]

    /// Layer page
    let layerPage
        (webConfig: WebUiConfig)
        (layerKey: string)
        (layer: LayerInfo)
        (elements: Element list)
        (registry: ElementRegistry)
        (filterValue: string option)
        (subtypeOptions: string list)
        (subtypeValue: string option) =
        let baseUrl = webConfig.BaseUrl
        let filterAttrs =
            [
                _type "text"
                _id "layer-filter"
                _name "filter"
                _class "filter-input"
                _placeholder "Filter elements by name"
                attr "aria-label" "Filter elements by name"
                _hxGet $"{baseUrl}{layerKey}"
                _hxTarget "#layer-elements"
                _hxTrigger "keyup changed delay:300ms"
                attr "hx-include" "#layer-filter, #subtype-filter"
                _hxPushUrl "true"
            ]
            |> List.append (
                match filterValue with
                | Some value -> [ _value value ]
                | None -> []
            )

        let subtypeSelect =
            let placeholder = placeholderOptionNode "All types" (subtypeValue.IsNone)
            let optionNodes =
                subtypeOptions
                |> List.map (fun value -> optionNode value (subtypeValue = Some value))

            select [
                _id "subtype-filter"
                _name "subtype"
                _class "filter-input"
                _hxGet $"{baseUrl}{layerKey}"
                _hxTarget "#layer-elements"
                _hxTrigger "change"
                attr "hx-include" "#layer-filter, #subtype-filter"
                _hxPushUrl "true"
                attr "aria-label" "Filter elements by subtype"
            ] (placeholder :: optionNodes)

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    encodedText layer.displayName
                ]
                h2 [_class "layer-title"] [encodedText layer.displayName]
                div [_class "diagram-section"] [
                    div [_class "diagram-toolbar"] [
                        div [_class "filter-bar"] [
                            label [_class "filter-label"; _for "layer-filter"] [
                                encodedText "Filter:"
                            ]
                            input filterAttrs
                            label [_class "filter-label"; _for "subtype-filter"] [
                                encodedText "Subtype:"
                            ]
                            subtypeSelect
                        ]
                        button [
                            _type "button"
                            _class "diagram-link"
                            _hxGet $"{baseUrl}elements/new?layer={layerKey}"
                            _hxTarget "#new-element-panel"
                            _hxSwap "innerHTML"
                        ] [
                            encodedText "New"
                        ]
                        a [_class "diagram-link"; _href $"{baseUrl}diagrams/layer/{layerKey}"; _target "_blank"; _rel "noopener"] [
                            encodedText "Open diagram"
                        ]
                    ]
                ]
                div [_id "new-element-panel"] []
                layerElementsPartial webConfig elements registry
            ]
        ]

        htmlPage webConfig layer.displayName layerKey content
