namespace EAArchive.Views

open System
open System.Net
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Htmx.HtmxAttrs

module Relations =

    let relationRowPartial
        (webConfig: WebUiConfig)
        (sourceId: string)
        (index: int)
        (targetValue: string)
        (relationValue: string)
        (descriptionValue: string)
        : XmlNode =
        let baseUrl = webConfig.BaseUrl
        div [_class "relation-row"] [
            div [_class "relation-row-header"] [
                span [_class "relation-row-title"] [encodedText $"Relation {index + 1}"]
                button [
                    _type "button"
                    _class "relation-delete"
                    attr "hx-on:click" "this.closest('.relation-row').remove();"
                    attr "aria-label" "Delete relation"
                ] [
                    encodedText "Delete"
                ]
            ]
            div [_class "form-row"] [
                label [_for $"rel-target-{index}"] [encodedText "Target element"]
                input [
                    _type "text"
                    _id $"rel-target-{index}"
                    _name $"rel-target-{index}"
                    _class "edit-input rel-target"
                    _value targetValue
                    attr "list" "element-id-list"
                    _hxGet $"{baseUrl}elements/relations/types"
                    _hxTrigger "load, change"
                    _hxTarget $"#rel-type-{index}"
                    _hxSwap "outerHTML"
                    _hxInclude "this"
                    _hxVals $"js:{{sourceId: '{sourceId}', sourceType: document.getElementById('type') ? document.getElementById('type').value : '', index: {index}, current: '{relationValue}'}}"
                ]
            ]
            div [_class "form-row"] [
                label [_for $"rel-type-{index}"] [encodedText "Relation type"]
                select [_id $"rel-type-{index}"; _name $"rel-type-{index}"; _class "edit-input"] [
                    placeholderOptionNode "Select relation type" (relationValue.Trim() = "")
                    if relationValue.Trim() <> "" then
                        optionNode relationValue true
                ]
            ]
            div [_class "form-row"] [
                label [_for $"rel-desc-{index}"] [encodedText "Description"]
                input [
                    _type "text"
                    _id $"rel-desc-{index}"
                    _name $"rel-desc-{index}"
                    _class "edit-input"
                    _value descriptionValue
                ]
            ]
        ]

    let relationTypeSelectPartial (index: int) (allowedTypes: string list) (currentValue: string) : XmlNode =
        let normalized = currentValue.Trim()
        if List.isEmpty allowedTypes then
            select [_id $"rel-type-{index}"; _name $"rel-type-{index}"; _class "edit-input"] [
                placeholderOptionNode "No allowed relations for this target" true
            ]
        else
            let placeholder = placeholderOptionNode "Select relation type" (normalized = "")

            let optionNodes =
                allowedTypes
                |> List.distinct
                |> List.map (fun value -> optionNode value (value = normalized))

            let extraOption =
                if normalized <> "" && not (allowedTypes |> List.exists (fun value -> value = normalized)) then
                    [ optionNode normalized true ]
                else
                    []

            select [_id $"rel-type-{index}"; _name $"rel-type-{index}"; _class "edit-input"] (placeholder :: extraOption @ optionNodes)

    let elementTypeSelectPartial (layerValue: string) (currentValue: string) : XmlNode =
        let options = Config.getTypeOptions layerValue

        let normalizedCurrent = currentValue.Trim()
        let placeholder =
            placeholderOptionNode "Select type" (normalizedCurrent = "")

        let optionNodes =
            options
            |> List.distinct
            |> List.map (fun value -> optionNode value (value = normalizedCurrent))

        let extraOption =
            if normalizedCurrent <> "" && not (options |> List.exists (fun value -> value = normalizedCurrent)) then
                [ optionNode normalizedCurrent true ]
            else
                []

        select [_id "type"; _name "type"; _class "edit-input"] (placeholder :: extraOption @ optionNodes)
