namespace EAArchive.Views

open System
open System.Net
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Htmx.HtmxAttrs
open Common

module Elements =

    let private formatGovernanceRelationType (value: RelationType) : string =
        ElementType.relationTypeToDisplayName value

    /// Element detail page
    let elementPage (webConfig: WebUiConfig) (detail: ArchimateDetailView) : XmlNode =
        let baseUrl = webConfig.BaseUrl

        let relationItem (related: ArchimateRelationView) (isIncoming: bool) =
            let relClass = if isIncoming then "incoming" else ""
            li [_class $"relation-item {relClass}"] [
                span [_class "relation-type"] [encodedText (ElementType.relationTypeToDisplayName related.relationType)]
                a [_href $"{baseUrl}elements/{related.relatedId}"] [
                    encodedText related.relatedName
                ]
                if related.description <> "" then
                    div [_class "relation-description"] [encodedText related.description]
            ]

        let incomingSection =
            if not (List.isEmpty detail.incomingRelations) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Incoming Relations"]
                        ul [_class "relation-list"] [
                            for rel in detail.incomingRelations do
                                relationItem rel true
                        ]
                    ]
                ]
            else
                []

        let outgoingSection =
            if not (List.isEmpty detail.outgoingRelations) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Outgoing Relations"]
                        ul [_class "relation-list"] [
                            for rel in detail.outgoingRelations do
                                relationItem rel false
                        ]
                    ]
                ]
            else
                []

        let governanceOwnerSection =
            if not (List.isEmpty detail.governanceOwners) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Governance Ownership"]
                        ul [_class "relation-list"] [
                            for doc in detail.governanceOwners |> List.sortBy (fun d -> d.title) do
                                li [_class "relation-item"] [
                                    span [_class "relation-type governance-relation-type"] [encodedText "Owner"]
                                    span [_class "governance-relation-label"] [encodedText (docTypeToString doc.docType)]
                                    a [_href $"{baseUrl}governance/{doc.slug}"] [
                                        encodedText doc.title
                                    ]
                                ]
                        ]
                    ]
                ]
            else
                []

        let governanceIncomingSection =
            if not (List.isEmpty detail.governanceIncoming) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Governance Relations"]
                        ul [_class "relation-list"] [
                            for doc in detail.governanceIncoming |> List.sortBy (fun d -> d.title) do
                                li [_class "relation-item"] [
                                    span [_class "relation-type governance-relation-type"] [encodedText (formatGovernanceRelationType doc.relationType)]
                                    span [_class "governance-relation-label"] [encodedText (docTypeToString doc.docType)]
                                    a [_href $"{baseUrl}governance/{doc.slug}"] [
                                        encodedText doc.title
                                    ]
                                ]
                        ]
                    ]
                ]
            else
                []

        let content = [
            div [_class "container"] [
                div [_class "edit-toolbar"] [
                    button [
                        _type "button"
                        _class "edit-button"
#if DEBUG
                        _hxExt "debug"
#endif
                        _hxGet $"{baseUrl}elements/{detail.id}/edit"
                        _hxTarget "#swapthis"
                        _hxSwap "innerHTML"
                    ] [
                        encodedText "Edit"
                    ]
                ]

                let layerName = detail.layer
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
                    encodedText detail.name
                ]

                div [
                    _class "element-detail"
                    _id "swapthis"
                ] [
                    div [_id "edit-panel"; _class "edit-panel"] []

                    div [_class "element-view"] [
                        h2 [] [encodedText detail.name]
                        let elementTypeStr =
                            match detail.elementType with
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
                                div [_class "metadata-value"] [encodedText detail.id]
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
                        if not (List.isEmpty detail.properties) then
                            div [_class "properties-section"] [
                                h3 [] [encodedText "Properties"]
                                div [_class "metadata"] [
                                    for (label, value) in detail.properties do
                                        div [_class "metadata-item"] [
                                            div [_class "metadata-label"] [encodedText label]
                                            div [_class "metadata-value"] [encodedText value]
                                        ]
                                ]
                            ]

                        if not (List.isEmpty detail.tags) then
                            div [_class "tags"] [
                                for tag in detail.tags do
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
                                a [_href $"{baseUrl}diagrams/context/{detail.id}?depth=1"; _class "diagram-link"] [
                                    encodedText "Direct relationships (1 level)"
                                ]
                                a [_href $"{baseUrl}diagrams/context/{detail.id}?depth=2"; _class "diagram-link"] [
                                    encodedText "Extended relationships (2 levels)"
                                ]
                                a [_href $"{baseUrl}diagrams/context/{detail.id}?depth=3"; _class "diagram-link"] [
                                    encodedText "Full relationships (3 levels)"
                                ]
                            ]
                        ]

                        if detail.content <> "" then
                            let htmlContent = markdownToHtml detail.content
                            div [_class "content-section"] [
                                rawText htmlContent
                            ]

                        yield! incomingSection
                        yield! outgoingSection
                        yield! governanceOwnerSection
                        yield! governanceIncomingSection
                    ]
                ]
            ]
        ]

        htmlPage webConfig detail.name "element" content

    let elementEditFormPartial (webConfig: WebUiConfig) (elem: ArchimateEditView) (elementOptions: (string * string) list) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let propertyValue key =
            elem.properties |> Map.tryFind key |> Option.defaultValue ""

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

        let typeOptions = Config.getTypeOptions elem.layerValue

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
                        ] (selectOptions elem.layerValue layerOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "type"] [encodedText "Type"]
                        select [_id "type"; _name "type"; _class "edit-input"] (selectOptionsWithPlaceholder "Select type" elem.typeValue typeOptions)
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
                            for (elemId, elemName) in elementOptions do
                                rawText $"<option value=\"{WebUtility.HtmlEncode elemId}\">{WebUtility.HtmlEncode elemName}</option>"
                        ]
                        button [
                            _type "button"
                            _class "secondary-button"
                            _hxGet $"{baseUrl}elements/relations/row"
                            _hxTarget "#relations-container"
                            _hxSwap "beforeend"
                            _hxVals $"js:{{sourceId: '{elem.id}', index: document.querySelectorAll('.relation-row').length}}"
                        ] [
                            encodedText "Add relation"
                        ]
                    ]
                    div [_class "form-row"] [
                        label [_for "content"] [encodedText "Content"]
                        textarea [_id "content"; _name "content"; _class "edit-textarea"; _rows "8"] [encodedText elem.content]
                    ]
                    button [_type "submit"; _class "primary-button"] [encodedText "Download"]
                ]
            ]
        ]

    let elementNewFormPartial (webConfig: WebUiConfig) (layerValue: string) (elementOptions: (string * string) list) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let layerOptions = Config.layerOptions
        let typeOptions = Config.getTypeOptions layerValue
        let statusOptions = ["draft"; "proposed"; "active"; "production"; "deprecated"; "retired"]
        let criticalityOptions = ["low"; "medium"; "high"; "critical"]
        let lifecycleOptions = ["plan"; "design"; "build"; "operate"; "retire"]

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

        div [_id "new-element-panel"; _class "edit-panel"] [
            h3 [] [encodedText "New Element"]
            form [
                _method "post"
                _action $"{baseUrl}elements/new/download"
                _class "edit-form"
            ] [
                div [_class "form-stack"] [
                    div [_class "form-row"] [
                        label [_for "name"] [encodedText "Name"]
                        input [_type "text"; _id "name"; _name "name"; _class "edit-input"]
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
                        select [_id "type"; _name "type"; _class "edit-input"] (selectOptionsWithPlaceholder "Select type" "" typeOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "tags"] [encodedText "Tags (comma-separated)"]
                        input [_type "text"; _id "tags"; _name "tags"; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "owner"] [encodedText "Owner"]
                        input [_type "text"; _id "owner"; _name "owner"; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "status"] [encodedText "Status"]
                        select [_id "status"; _name "status"; _class "edit-input"] (selectOptions "" statusOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "criticality"] [encodedText "Criticality"]
                        select [_id "criticality"; _name "criticality"; _class "edit-input"] (selectOptions "" criticalityOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "version"] [encodedText "Version"]
                        input [_type "text"; _id "version"; _name "version"; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [_for "lifecycle-phase"] [encodedText "Lifecycle phase"]
                        select [_id "lifecycle-phase"; _name "lifecycle-phase"; _class "edit-input"] (selectOptions "" lifecycleOptions)
                    ]
                    div [_class "form-row"] [
                        label [_for "last-updated"] [encodedText "Last updated"]
                        input [_type "text"; _id "last-updated"; _name "last-updated"; _class "edit-input"]
                    ]
                    div [_class "form-row"] [
                        label [] [encodedText "Outgoing relations"]
                        div [_id "relations-container"] [
                            Relations.relationRowPartial webConfig "" 0 "" "" ""
                        ]
                        datalist [_id "element-id-list"] [
                            for (elemId, elemName) in elementOptions do
                                rawText $"<option value=\"{WebUtility.HtmlEncode elemId}\">{WebUtility.HtmlEncode elemName}</option>"
                        ]
                        button [
                            _type "button"
                            _class "secondary-button"
                            _hxGet $"{baseUrl}elements/relations/row"
                            _hxTarget "#relations-container"
                            _hxSwap "beforeend"
                            _hxVals "js:{sourceId: '', index: document.querySelectorAll('.relation-row').length}"
                        ] [
                            encodedText "Add relation"
                        ]
                    ]
                    div [_class "form-row"] [
                        label [_for "content"] [encodedText "Content"]
                        textarea [_id "content"; _name "content"; _class "edit-textarea"; _rows "8"] []
                    ]
                    button [_type "submit"; _class "primary-button"] [encodedText "Download"]
                ]
            ]
        ]
