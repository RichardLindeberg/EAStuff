namespace EAArchive.Views

open System
open System.Collections
open System.Collections.Generic
open System.Net
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Htmx.HtmxAttrs
open Common

module Elements =

    let private getMetadataString (key: string) (metadata: Map<string, obj>) : string option =
        ElementRegistry.getString key metadata

    let private buildPropertiesMap (elem: Element) : Map<string, obj> =
        elem.properties
        |> Map.tryFind "properties"
        |> Option.bind (fun value ->
            match value with
            | :? IDictionary as dict ->
                dict
                |> Seq.cast<obj>
                |> Seq.choose (fun entry ->
                    match entry with
                    | :? KeyValuePair<obj, obj> as kvp ->
                        match kvp.Key with
                        | :? string as key -> Some (key, kvp.Value)
                        | _ -> None
                    | :? DictionaryEntry as de ->
                        match de.Key with
                        | :? string as key -> Some (key, de.Value)
                        | _ -> None
                    | _ -> None
                )
                |> Map.ofSeq
                |> Some
            | _ -> None
        )
        |> Option.defaultValue Map.empty

    let private tryGetPropertyValue (propertiesMap: Map<string, obj>) (key: string) : string option =
        match Map.tryFind key propertiesMap with
        | Some value ->
            let text = if isNull value then "" else value.ToString()
            let trimmed = text.Trim()
            if trimmed = "" then None else Some trimmed
        | None -> None

    /// Element detail page
    let elementPage (webConfig: WebUiConfig) (elemWithRels: ElementWithRelations) =
        let baseUrl = webConfig.BaseUrl
        let elem = elemWithRels.element

        let relationItem (related: Element) (rel: Relationship) (isIncoming: bool) =
            let relClass = if isIncoming then "incoming" else ""
            li [_class $"relation-item {relClass}"] [
                span [_class "relation-type"] [encodedText (ElementType.relationTypeToDisplayName rel.relationType)]
                a [_href $"{baseUrl}elements/{related.id}"] [
                    encodedText related.name
                ]
                if rel.description <> "" then
                    div [_class "relation-description"] [encodedText rel.description]
            ]

        let incomingSection =
            if not (List.isEmpty elemWithRels.incomingRelations) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Incoming Relations"]
                        ul [_class "relation-list"] [
                            for (source, rel) in elemWithRels.incomingRelations do
                                relationItem source rel true
                        ]
                    ]
                ]
            else
                []

        let outgoingSection =
            if not (List.isEmpty elemWithRels.outgoingRelations) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Outgoing Relations"]
                        ul [_class "relation-list"] [
                            for (target, rel) in elemWithRels.outgoingRelations do
                                relationItem target rel false
                        ]
                    ]
                ]
            else
                []

        let propertiesMap = buildPropertiesMap elem

        let tryGetProperty (key: string) : string option =
            tryGetPropertyValue propertiesMap key

        let propertyItems =
            [
                ("owner", "Owner")
                ("status", "Status")
                ("criticality", "Criticality")
                ("version", "Version")
                ("lifecycle-phase", "Lifecycle phase")
                ("last-updated", "Last updated")
            ]
            |> List.choose (fun (key, label) ->
                tryGetProperty key
                |> Option.map (fun value -> (label, value))
            )

        let content = [
            div [_class "container"] [
                div [_class "edit-toolbar"] [
                    button [
                        _type "button"
                        _class "edit-button"
#if DEBUG
                        _hxExt "debug"
#endif
                        _hxGet $"{baseUrl}elements/{elem.id}/edit"
                        _hxTarget "#swapthis"
                        _hxSwap "innerHTML"
                    ] [
                        encodedText "Edit"
                    ]
                ]

                let layerName = ElementType.getLayer elem.elementType
                let layerNameLower = Layer.toKey layerName
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}{layerNameLower}"] [encodedText (
                        Config.layerOrder
                        |> Map.tryFind layerName
                        |> Option.map (fun l -> l.displayName)
                        |> Option.defaultValue (Layer.toString layerName)
                    )]
                    encodedText " / "
                    encodedText elem.name
                ]

                div [
                    _class "element-detail"
                    _id "swapthis"
                ] [
                    div [_id "edit-panel"; _class "edit-panel"] []

                    div [_class "element-view"] [
                        h2 [] [encodedText elem.name]
                        let elementTypeStr =
                            match elem.elementType with
                            | ElementType.Strategy st -> sprintf "Strategy - %A" st
                            | ElementType.Motivation mt -> sprintf "Motivation - %A" mt
                            | ElementType.Business bt -> sprintf "Business - %A" bt
                            | ElementType.Application at -> sprintf "Application - %A" at
                            | ElementType.Technology tt -> sprintf "Technology - %A" tt
                            | ElementType.Physical pt -> sprintf "Physical - %A" pt
                            | ElementType.Implementation it -> sprintf "Implementation - %A" it
                            | ElementType.Unknown (layer, typeName) -> sprintf "%s - %s" layer typeName

                        div [_class "metadata"] [
                            div [_class "metadata-item"] [
                                div [_class "metadata-label"] [encodedText "ID"]
                                div [_class "metadata-value"] [encodedText elem.id]
                            ]
                            div [_class "metadata-item"] [
                                div [_class "metadata-label"] [encodedText "Type"]
                                div [_class "metadata-value"] [encodedText elementTypeStr]
                            ]
                            div [_class "metadata-item"] [
                                div [_class "metadata-label"] [encodedText "Layer"]
                                div [_class "metadata-value"] [encodedText (Layer.toString layerName)]
                            ]
                        ]
                        if not (List.isEmpty propertyItems) then
                            div [_class "properties-section"] [
                                h3 [] [encodedText "Properties"]
                                div [_class "metadata"] [
                                    for (label, value) in propertyItems do
                                        div [_class "metadata-item"] [
                                            div [_class "metadata-label"] [encodedText label]
                                            div [_class "metadata-value"] [encodedText value]
                                        ]
                                ]
                            ]

                        if not (List.isEmpty elem.tags) then
                            div [_class "tags"] [
                                for tag in elem.tags do
                                    a [_href $"{baseUrl}tags/{tag}"; _class "tag"] [
                                        encodedText tag
                                    ]
                            ]

                        div [_class "diagram-section"] [
                            div [_class "diagram-header"] [
                                h3 [] [encodedText "Context Diagram"]
                            ]
                            div [_class "diagram-links"] [
                                p [] [encodedText "View this element's relationships in different depths:"]
                                a [_href $"{baseUrl}diagrams/context/{elem.id}?depth=1"; _class "diagram-link"] [
                                    encodedText "Direct relationships (1 level)"
                                ]
                                a [_href $"{baseUrl}diagrams/context/{elem.id}?depth=2"; _class "diagram-link"] [
                                    encodedText "Extended relationships (2 levels)"
                                ]
                                a [_href $"{baseUrl}diagrams/context/{elem.id}?depth=3"; _class "diagram-link"] [
                                    encodedText "Full relationships (3 levels)"
                                ]
                            ]
                        ]

                        if elem.content <> "" then
                            let htmlContent = markdownToHtml elem.content
                            div [_class "content-section"] [
                                rawText htmlContent
                            ]

                        yield! incomingSection
                        yield! outgoingSection
                    ]
                ]
            ]
        ]

        htmlPage webConfig elem.name "element" content

    let elementEditFormPartial (webConfig: WebUiConfig) (elem: Element) (registry: ElementRegistry) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let propertiesMap = buildPropertiesMap elem
        let propertyValue key =
            tryGetPropertyValue propertiesMap key |> Option.defaultValue ""

        let layerValue =
            getMetadataString "layer" elem.properties |> Option.defaultValue ""

        let typeValue =
            getMetadataString "type" elem.properties |> Option.defaultValue ""

        let tagValue =
            if List.isEmpty elem.tags then "" else String.concat ", " elem.tags

        let relationTypeValue (relationType: RelationType) : string =
            ElementType.relationTypeToString relationType

        let selectOptions (currentValue: string) (options: string list) : XmlNode list =
            let normalized = currentValue.Trim()
            let baseOptions =
                options
                |> List.distinct
                |> List.map (fun value -> optionNode value (value = normalized))

            if normalized <> "" && not (options |> List.exists (fun v -> v = normalized)) then
                optionNode normalized true
                :: baseOptions
            else
                baseOptions

        let selectOptionsWithPlaceholder (placeholder: string) (currentValue: string) (options: string list) : XmlNode list =
            let normalized = currentValue.Trim()
            let placeholderOption = placeholderOptionNode placeholder (normalized = "")
            placeholderOption :: selectOptions currentValue options

        let layerOptions = Config.layerOptions

        let typeOptions = Config.getTypeOptions layerValue

        let statusOptions = ["draft"; "proposed"; "active"; "production"; "deprecated"; "retired"]

        let criticalityOptions = ["low"; "medium"; "high"; "critical"]

        let lifecycleOptions = ["plan"; "design"; "build"; "operate"; "retire"]

        div [_id "edit-panel"; _class "edit-panel"] [
            h3 [] [encodedText "Edit Element"]
            form [
                _method "post"
                _action $"{baseUrl}elements/{elem.id}/download"
                _class "edit-form"
            ] [
                input [_type "hidden"; _id "id"; _name "id"; _value elem.id]
                div [_class "form-stack"] [
                    div [_class "form-row"] [
                        label [_for "name"] [encodedText "Name"]
                        input [_type "text"; _id "name"; _name "name"; _value elem.name; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "layer"] [encodedText "Layer"]
                        select [
                            _id "layer"
                            _name "layer"
                            _class "edit-input"
                            attr "hx-get" $"{baseUrl}elements/types"
                            attr "hx-trigger" "change"
                            attr "hx-target" "#type"
                            attr "hx-include" "#type"
                        ] (selectOptions layerValue layerOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "type"] [encodedText "Type"]
                        select [_id "type"; _name "type"; _class "edit-input"] (selectOptionsWithPlaceholder "Select type" typeValue typeOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "tags"] [encodedText "Tags (comma-separated)"]
                        input [_type "text"; _id "tags"; _name "tags"; _value tagValue; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "owner"] [encodedText "Owner"]
                        input [_type "text"; _id "owner"; _name "owner"; _value (propertyValue "owner"); _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "status"] [encodedText "Status"]
                        select [_id "status"; _name "status"; _class "edit-input"] (selectOptions (propertyValue "status") statusOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "criticality"] [encodedText "Criticality"]
                        select [_id "criticality"; _name "criticality"; _class "edit-input"] (selectOptions (propertyValue "criticality") criticalityOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "version"] [encodedText "Version"]
                        input [_type "text"; _id "version"; _name "version"; _value (propertyValue "version"); _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "lifecycle-phase"] [encodedText "Lifecycle phase"]
                        select [_id "lifecycle-phase"; _name "lifecycle-phase"; _class "edit-input"] (selectOptions (propertyValue "lifecycle-phase") lifecycleOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "last-updated"] [encodedText "Last updated"]
                        input [_type "text"; _id "last-updated"; _name "last-updated"; _value (propertyValue "last-updated"); _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [] [encodedText "Outgoing relations"]
                        div [_id "relations-container"] [
                            for (index, rel) in elem.relationships |> List.mapi (fun i r -> (i, r)) do
                                Relations.relationRowPartial webConfig elem.id index rel.target (relationTypeValue rel.relationType) rel.description
                            if List.isEmpty elem.relationships then
                                Relations.relationRowPartial webConfig elem.id 0 "" "" ""
                        ]
                        datalist [_id "element-id-list"] [
                            for (_, elemValue) in registry.elements |> Map.toList |> List.sortBy (fun (_, e) -> e.name) do
                                rawText $"<option value=\"{WebUtility.HtmlEncode elemValue.id}\">{WebUtility.HtmlEncode elemValue.name}</option>"
                        ]
                        button [
                            _type "button"
                            _class "secondary-button"
                            _hxGet $"{baseUrl}elements/relations/row"
                            _hxTarget "#relations-container"
                            _hxSwap "beforeend"
                            attr "hx-vals" "js:{index: document.querySelectorAll('.relation-row').length, sourceId: document.getElementById('id').value}"
                        ] [
                            encodedText "Add relation"
                        ]
                    ]
                ]
                div [_class "form-row"] [
                    label [_for "content"] [encodedText "Content (Markdown)"]
                    textarea [_id "content"; _name "content"; _rows "18"; _class "edit-textarea"] [
                        encodedText elem.content
                    ]
                ]
                div [_class "form-actions"] [
                    button [_type "submit"; _class "primary-button"] [encodedText "Download updated file"]
                    button [
                        _type "button"
                        _class "secondary-button"
                        attr "hx-on:click" "this.closest('.element-detail').classList.remove('is-editing');document.getElementById('edit-panel').innerHTML='';"
                    ] [
                        encodedText "Cancel"
                    ]
                ]
            ]
        ]

    let elementNewFormPartial (webConfig: WebUiConfig) (layerValue: string) (registry: ElementRegistry) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let emptyValue = ""
        let tagValue = ""

        let selectOptions (currentValue: string) (options: string list) : XmlNode list =
            let normalized = currentValue.Trim()
            let baseOptions =
                options
                |> List.distinct
                |> List.map (fun value -> optionNode value (value = normalized))

            if normalized <> "" && not (options |> List.exists (fun v -> v = normalized)) then
                optionNode normalized true
                :: baseOptions
            else
                baseOptions

        let selectOptionsWithPlaceholder (placeholder: string) (currentValue: string) (options: string list) : XmlNode list =
            let normalized = currentValue.Trim()
            let placeholderOption = placeholderOptionNode placeholder (normalized = "")
            placeholderOption :: selectOptions currentValue options

        let layerOptions = Config.layerOptions

        let typeOptions = Config.getTypeOptions layerValue

        let statusOptions = ["draft"; "proposed"; "active"; "production"; "deprecated"; "retired"]
        let criticalityOptions = ["low"; "medium"; "high"; "critical"]
        let lifecycleOptions = ["plan"; "design"; "build"; "operate"; "retire"]

        div [_class "new-element-panel"] [
            h3 [] [encodedText "New Element"]
            form [
                _method "post"
                _action $"{baseUrl}elements/new/download"
                _class "edit-form"
            ] [
                input [_type "hidden"; _id "id"; _name "id"; _value emptyValue]
                div [_class "form-stack"] [
                    div [_class "form-row"] [
                        label [_for "name"] [encodedText "Name"]
                        input [_type "text"; _id "name"; _name "name"; _value emptyValue; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "layer"] [encodedText "Layer"]
                        select [
                            _id "layer"
                            _name "layer"
                            _class "edit-input"
                            attr "hx-get" $"{baseUrl}elements/types"
                            attr "hx-trigger" "change"
                            attr "hx-target" "#type"
                            attr "hx-swap" "outerHTML"
                            attr "hx-include" "#type"
                        ] (selectOptions layerValue layerOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "type"] [encodedText "Type"]
                        select [_id "type"; _name "type"; _class "edit-input"] (selectOptionsWithPlaceholder "Select type" emptyValue typeOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "tags"] [encodedText "Tags (comma-separated)"]
                        input [_type "text"; _id "tags"; _name "tags"; _value tagValue; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "owner"] [encodedText "Owner"]
                        input [_type "text"; _id "owner"; _name "owner"; _value emptyValue; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "status"] [encodedText "Status"]
                        select [_id "status"; _name "status"; _class "edit-input"] (selectOptions emptyValue statusOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "criticality"] [encodedText "Criticality"]
                        select [_id "criticality"; _name "criticality"; _class "edit-input"] (selectOptions emptyValue criticalityOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "version"] [encodedText "Version"]
                        input [_type "text"; _id "version"; _name "version"; _value emptyValue; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "lifecycle-phase"] [encodedText "Lifecycle phase"]
                        select [_id "lifecycle-phase"; _name "lifecycle-phase"; _class "edit-input"] (selectOptions emptyValue lifecycleOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "last-updated"] [encodedText "Last updated"]
                        input [_type "text"; _id "last-updated"; _name "last-updated"; _value emptyValue; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [] [encodedText "Outgoing relations"]
                        div [_id "relations-container"] [
                            Relations.relationRowPartial webConfig emptyValue 0 "" "" ""
                        ]
                        datalist [_id "element-id-list"] [
                            for (_, elemValue) in registry.elements |> Map.toList |> List.sortBy (fun (_, e) -> e.name) do
                                rawText $"<option value=\"{WebUtility.HtmlEncode elemValue.id}\">{WebUtility.HtmlEncode elemValue.name}</option>"
                        ]
                        button [
                            _type "button"
                            _class "secondary-button"
                            _hxGet $"{baseUrl}elements/relations/row"
                            _hxTarget "#relations-container"
                            _hxSwap "beforeend"
                            attr "hx-vals" "js:{index: document.querySelectorAll('.relation-row').length, sourceId: ''}"
                        ] [
                            encodedText "Add relation"
                        ]
                    ]
                ]
                div [_class "form-row"] [
                    label [_for "content"] [encodedText "Content (Markdown)"]
                    textarea [_id "content"; _name "content"; _rows "18"; _class "edit-textarea"] [
                        encodedText emptyValue
                    ]
                ]
                div [_class "form-actions"] [
                    button [_type "submit"; _class "primary-button"] [encodedText "Download new file"]
                    button [
                        _type "button"
                        _class "secondary-button"
                        attr "hx-on:click" "this.closest('.new-element-panel').innerHTML='';"
                    ] [
                        encodedText "Cancel"
                    ]
                ]
            ]
        ]
