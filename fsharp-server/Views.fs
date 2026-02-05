namespace EAArchive

open System
open System.Net
open System.Collections
open System.Collections.Generic
open Giraffe.ViewEngine
open Htmx.HtmxAttrs
open Markdig

module Views =
    
    let baseUrl = Config.baseUrl
    
    /// Convert markdown to HTML
    let markdownToHtml (markdown: string) : string =
        try
            let pipeline = MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            Markdown.ToHtml(markdown, pipeline)
        with
        | ex ->
            sprintf "<p>Error rendering markdown: %s</p>" (WebUtility.HtmlEncode ex.Message)
    
    /// Helper to pluralize words
    let pluralize (count: int) (singular: string) (plural: string) : string =
        if count <> 1 then plural else singular
    
    /// Convert ElementType to string for display
    let elementTypeToString (elementType: ElementType) : string =
        match elementType with
        | ElementType.Strategy st -> sprintf "Strategy %A" st
        | ElementType.Motivation mt -> sprintf "Motivation %A" mt
        | ElementType.Business bt -> sprintf "Business %A" bt
        | ElementType.Application at -> sprintf "Application %A" at
        | ElementType.Technology tt -> sprintf "Technology %A" tt
        | ElementType.Physical pt -> sprintf "Physical %A" pt
        | ElementType.Implementation it -> sprintf "Implementation %A" it
        | ElementType.Unknown (layer, typeName) -> sprintf "%s %s" layer typeName
    
    /// Convert RelationType to string for display
    let relationTypeToString (relationType: RelationType) : string =
        match relationType with
        | RelationType.Composition -> "Composition"
        | RelationType.Aggregation -> "Aggregation"
        | RelationType.Assignment -> "Assignment"
        | RelationType.Realization -> "Realization"
        | RelationType.Specialization -> "Specialization"
        | RelationType.Association -> "Association"
        | RelationType.Access -> "Access"
        | RelationType.Influence -> "Influence"
        | RelationType.Serving -> "Serving"
        | RelationType.Triggering -> "Triggering"
        | RelationType.Flow -> "Flow"
        | RelationType.Unknown s -> s
    
    /// HTML header with navigation
    let htmlHeader (title: string) (currentPage: string) =
        let navItems =
            Config.layerOrder
            |> Map.toList
            |> List.sortBy (fun (_, layerInfo) -> layerInfo.order)
            |> List.map (fun (layerKey, layerInfo) ->
                let isActive = currentPage = layerKey
                let activeClass = if isActive then "active" else ""
                a [_href $"{baseUrl}{layerKey.ToLowerInvariant()}"; _class $"nav-link {activeClass}"] [
                    encodedText layerInfo.displayName
                ]
            )
        
        let tagsActive = if currentPage = "tags" then "active" else ""
        let validationActive = if currentPage = "validation" then "active" else ""
        let navLinks = 
            navItems @ [
                a [_href $"{baseUrl}tags"; _class $"nav-link {tagsActive}"] [
                    encodedText "Tags"
                ]
                a [_href $"{baseUrl}validation"; _class $"nav-link {validationActive}"] [
                    encodedText "⚠ Validation"
                ]
            ]
        
        header [_class "header"] [
            div [_class "header-content"] [
                h1 [] [encodedText "ArchiMate Architecture Repository"]
                nav [] navLinks
            ]
        ]
    
    /// HTML footer
    let htmlFooter () =
        footer [_class "footer"] [
            p [] [encodedText "Generated from ArchiMate elements | © 2026"]
        ]
    
    /// Full HTML page template
    let htmlPage (pageTitle: string) (currentPage: string) (content: XmlNode list) =
        html [_lang "en"] [
            head [] [
                meta [_charset "UTF-8"]
                meta [_name "viewport"; _content "width=device-width, initial-scale=1.0"]
                title [] [encodedText $"{pageTitle} - ArchiMate Architecture"]
                link [_rel "stylesheet"; _href "/css/site.css"]
                link [_rel "stylesheet"; _href "/css/cytoscape-diagram.css"]
                script [_src "https://unpkg.com/htmx.org@1.9.10"] []
                script [_src "/js/validation.js"] []
                script [_src "/js/cytoscape-diagram.js"] []
                script [ _src "https://unpkg.com/htmx.org@1.9.12/dist/ext/debug.js" ] []
            ]
            body [] (
                [htmlHeader pageTitle currentPage] @ content @ [htmlFooter ()]
            )
        ]
    
    /// Index page
    let indexPage (registry: ElementRegistry) =
        let layerCards =
            Config.layerOrder
            |> Map.toList
            |> List.sortBy (fun (_, layerInfo) -> layerInfo.order)
            |> List.choose (fun (layerKey, layerInfo) ->
                let layerKeyLower = layerKey.ToLowerInvariant()
                let elements = ElementRegistry.getLayerElements layerKeyLower registry
                if List.isEmpty elements then None
                else
                    Some (
                        div [_class "element-card"] [
                            h3 [] [
                                a [_href $"{baseUrl}{layerKeyLower}"] [
                                    encodedText layerInfo.displayName
                                ]
                            ]
                            let elemCountStr = let count = List.length elements in sprintf "%d element%s" count (pluralize count "" "s")
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
                h2 [_class "layer-title"] [encodedText "Architecture Overview"]
                p [_class "layer-description"] [
                    encodedText "This repository contains the enterprise architecture organized by ArchiMate layers. Each layer represents a different aspect of the architecture, from strategic goals to technical implementation."
                ]
                div [_class "element-grid"] layerCards
            ]
        ]
        
        htmlPage "Home" "index" content
    
    let private buildElementCards (elements: Element list) (registry: ElementRegistry) : XmlNode list =
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

    let layerElementsPartial (elements: Element list) (registry: ElementRegistry) : XmlNode =
        let elementCards = buildElementCards elements registry
        let count = List.length elements
        let layerElemCountStr = sprintf "%d element%s" count (pluralize count "" "s")

        div [_id "layer-elements"] [
            p [_class "element-count"] [
                encodedText layerElemCountStr
            ]
            div [_class "element-grid"] elementCards
        ]

    /// Layer page
    let layerPage (layerKey: string) (layer: LayerInfo) (elements: Element list) (registry: ElementRegistry) (filterValue: string option) =
        let filterAttrs =
            [
                _type "text"
                _id "layer-filter"
                _name "filter"
                _class "filter-input"
                _placeholder "Filter elements by name"
                attr "aria-label" "Filter elements by name"
                _hxGet  $"{baseUrl}{layerKey}"
                _hxTarget "#layer-elements"
                _hxTrigger "keyup changed delay:300ms"
                _hxInclude "this"
                _hxPushUrl "true"
            ]
            |> List.append (
                match filterValue with
                | Some value -> [ _value value ]
                | None -> []
            )

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
                        ]
                        a [_class "diagram-link"; _href $"{baseUrl}diagrams/layer/{layerKey}"; _target "_blank"; _rel "noopener"] [
                            encodedText "Open diagram"
                        ]
                    ]
                ]
                layerElementsPartial elements registry
            ]
        ]

        htmlPage layer.displayName layer.displayName content

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

    let private optionNode (value: string) (selected: bool) : XmlNode =
        let encodedValue = WebUtility.HtmlEncode value
        let selectedAttr = if selected then " selected" else ""
        rawText $"<option value=\"{encodedValue}\"{selectedAttr}>{encodedValue}</option>"

    let private placeholderOptionNode (label: string) (selected: bool) : XmlNode =
        let encodedLabel = WebUtility.HtmlEncode label
        let selectedAttr = if selected then " selected" else ""
        rawText $"<option value=\"\"{selectedAttr}>{encodedLabel}</option>"
    
    /// Element detail page
    let elementPage (elemWithRels: ElementWithRelations) =
        let elem = elemWithRels.element
        
        let relationItem (related: Element) (rel: Relationship) (isIncoming: bool) =
            let relClass = if isIncoming then "incoming" else ""
            li [_class $"relation-item {relClass}"] [
                span [_class "relation-type"] [encodedText (relationTypeToString rel.relationType)]
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
            else []
        
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
            else []

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
                            _hxExt "debug"
                            _hxGet   $"{baseUrl}elements/{elem.id}/edit"
                            _hxTarget "#swapthis"
                            _hxSwap "innerHTML"
                        ] [
                            encodedText "Edit"
                        ]
                    ]


                let layerName = ElementType.getLayer elem.elementType
                let layerNameLower = layerName.ToLowerInvariant()
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}{layerNameLower}"] [encodedText (
                        Config.layerOrder
                        |> Map.tryFind layerName
                        |> Option.map (fun l -> l.displayName)
                        |> Option.defaultValue layerName
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
                                div [_class "metadata-value"] [encodedText layerName]
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
        
        htmlPage elem.name "element" content

    let relationRowPartial (sourceId: string) (index: int) (targetValue: string) (relationValue: string) (descriptionValue: string) : XmlNode =
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
                    _hxVals $"js:{{sourceId: '{sourceId}', index: {index}, current: '{relationValue}'}}"
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

    let elementEditFormPartial (elem: Element) (registry: ElementRegistry) : XmlNode =
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
            match relationType with
            | RelationType.Composition -> "composition"
            | RelationType.Aggregation -> "aggregation"
            | RelationType.Assignment -> "assignment"
            | RelationType.Realization -> "realization"
            | RelationType.Specialization -> "specialization"
            | RelationType.Association -> "association"
            | RelationType.Access -> "access"
            | RelationType.Influence -> "influence"
            | RelationType.Serving -> "serving"
            | RelationType.Triggering -> "triggering"
            | RelationType.Flow -> "flow"
            | RelationType.Unknown value -> value

        let selectOptions (currentValue: string) (options: string list) : XmlNode list =
            let normalized = currentValue.Trim()
            let baseOptions =
                options
                |> List.distinct
                |> List.map (fun value ->
                    optionNode value (value = normalized)
                )

            if normalized <> "" && not (options |> List.exists (fun v -> v = normalized)) then
                optionNode normalized true
                :: baseOptions
            else
                baseOptions

        let selectOptionsWithPlaceholder (placeholder: string) (currentValue: string) (options: string list) : XmlNode list =
            let normalized = currentValue.Trim()
            let placeholderOption = placeholderOptionNode placeholder (normalized = "")
            placeholderOption :: selectOptions currentValue options

        let layerOptions = ["strategy"; "business"; "application"; "technology"; "physical"; "motivation"; "implementation"]

        let typeOptionsByLayer =
            Map.ofList [
                ("strategy", ["resource"; "capability"; "value-stream"; "course-of-action"])
                ("business", [
                    "business-actor"; "business-role"; "business-collaboration"; "business-interface"
                    "business-process"; "business-function"; "business-interaction"; "business-event"
                    "business-service"; "business-object"; "contract"; "representation"; "product"
                ])
                ("application", [
                    "application-component"; "application-collaboration"; "application-interface"
                    "application-function"; "application-interaction"; "application-process"
                    "application-event"; "application-service"; "data-object"
                ])
                ("technology", [
                    "node"; "device"; "system-software"; "technology-collaboration"
                    "technology-interface"; "path"; "communication-network"; "technology-function"
                    "technology-process"; "technology-interaction"; "technology-event"
                    "technology-service"; "artifact"
                ])
                ("physical", ["equipment"; "facility"; "distribution-network"; "material"])
                ("motivation", [
                    "stakeholder"; "driver"; "assessment"; "goal"; "outcome"; "principle"
                    "requirement"; "constraint"; "meaning"; "value"
                ])
                ("implementation", ["work-package"; "deliverable"; "implementation-event"; "plateau"; "gap"])
            ]

        let allTypeOptions =
            typeOptionsByLayer
            |> Map.toList
            |> List.collect snd
            |> List.distinct

        let typeOptions =
            let normalizedLayer = layerValue.Trim().ToLowerInvariant()
            typeOptionsByLayer
            |> Map.tryFind normalizedLayer
            |> Option.defaultValue allTypeOptions

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
                                relationRowPartial elem.id index rel.target (relationTypeValue rel.relationType) rel.description
                            if List.isEmpty elem.relationships then
                                relationRowPartial elem.id 0 "" "" ""
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

    let elementTypeSelectPartial (layerValue: string) (currentValue: string) : XmlNode =
        let normalizedLayer = layerValue.Trim().ToLowerInvariant()
        let options =
            match normalizedLayer with
            | "strategy" ->
                ["resource"; "capability"; "value-stream"; "course-of-action"]
            | "business" ->
                [
                    "business-actor"; "business-role"; "business-collaboration"; "business-interface"
                    "business-process"; "business-function"; "business-interaction"; "business-event"
                    "business-service"; "business-object"; "contract"; "representation"; "product"
                ]
            | "application" ->
                [
                    "application-component"; "application-collaboration"; "application-interface"
                    "application-function"; "application-interaction"; "application-process"
                    "application-event"; "application-service"; "data-object"
                ]
            | "technology" ->
                [
                    "node"; "device"; "system-software"; "technology-collaboration"
                    "technology-interface"; "path"; "communication-network"; "technology-function"
                    "technology-process"; "technology-interaction"; "technology-event"
                    "technology-service"; "artifact"
                ]
            | "physical" ->
                ["equipment"; "facility"; "distribution-network"; "material"]
            | "motivation" ->
                [
                    "stakeholder"; "driver"; "assessment"; "goal"; "outcome"; "principle"
                    "requirement"; "constraint"; "meaning"; "value"
                ]
            | "implementation" ->
                ["work-package"; "deliverable"; "implementation-event"; "plateau"; "gap"]
            | _ ->
                []

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
    
    /// Tags index page
    let tagsIndexPage (tagIndex: Map<string, string list>) (registry: ElementRegistry) =
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
        
        htmlPage "Tags" "tags" content
    
    /// Tag detail page
    let tagPage (tag: string) (elements: Element list) =
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
        
        htmlPage tag "tags" content

    /// Validation errors page
    let validationPage (errors: ValidationError list) =
        let errorsByFile =
            errors
            |> List.groupBy (fun e -> e.filePath)
            |> List.sortBy fst
        
        let fileCards =
            errorsByFile
            |> List.map (fun (filePath, fileErrors) ->
                let errorCount = List.length fileErrors
                let hasErrors = fileErrors |> List.exists (fun e -> e.severity = Severity.Error)
                let errorClass = if hasErrors then "has-errors" else "has-warnings"
                let severityBadges =
                    fileErrors
                    |> List.groupBy (fun e -> e.severity)
                    |> List.map (fun (severity, errs) ->
                        let badgeClass = if severity = Severity.Error then "badge-error" else "badge-warning"
                        let sevStr = match severity with Severity.Error -> "Error" | Severity.Warning -> "Warning"
                        span [_class $"badge {badgeClass}"] [
                            encodedText (sprintf "%s: %d" sevStr errs.Length)
                        ]
                    )
                
                let errorDetails =
                    fileErrors
                    |> List.map (fun err ->
                        let elemId = err.elementId |> Option.defaultValue "N/A"
                        let severityStr = match err.severity with Severity.Error -> "error" | Severity.Warning -> "warning"
                        let severityClass = $"severity-{severityStr}"
                        let errorTypeStr =
                            match err.errorType with
                            | ErrorType.MissingId -> "Missing ID"
                            | ErrorType.InvalidType -> "Invalid Type"
                            | ErrorType.InvalidLayer -> "Invalid Layer"
                            | ErrorType.MissingRequiredField -> "Missing Required Field"
                            | ErrorType.InvalidRelationshipType _ -> "Invalid Relationship Type"
                            | ErrorType.RelationshipTargetNotFound _ -> "Relationship Target Not Found"
                            | ErrorType.InvalidRelationshipCombination _ -> "Invalid Relationship Combination"
                            | ErrorType.SelfReference _ -> "Self Reference"
                            | ErrorType.DuplicateRelationship _ -> "Duplicate Relationship"
                            | ErrorType.Unknown s -> s
                        div [_class $"error-detail {severityClass}"] [
                            div [_class "error-header"] [
                                span [_class "error-type"] [encodedText errorTypeStr]
                                span [_class "element-ref"] [encodedText $"ID: {elemId}"]
                            ]
                            p [_class "error-message"] [encodedText err.message]
                        ]
                    )
                
                let relativeFilePath =
                    if filePath.Contains("\\elements\\") then
                        let idx = filePath.LastIndexOf("\\elements\\")
                        filePath.Substring(idx + 1)
                    else
                        filePath
                
                div [_class $"file-card {errorClass}"] [
                    div [_class "file-header"] [
                        h3 [] [encodedText relativeFilePath]
                        div [_class "badges"] severityBadges
                    ]
                    div [_class "error-list"] errorDetails
                    button [
                        _type "button"
                        _class "btn-reload"
                        _onclick $"reloadFile('{relativeFilePath}')"
                    ] [
                        encodedText "Reload File"
                    ]
                ]
            )
        
        let stats =
            let totalFiles = List.length errorsByFile
            let totalErrors = errors |> List.filter (fun e -> e.severity = Severity.Error) |> List.length
            let totalWarnings = errors |> List.filter (fun e -> e.severity = Severity.Warning) |> List.length
            let errorClass = if totalErrors > 0 then "error" else ""
            let warningClass = if totalWarnings > 0 then "warning" else ""
            
            div [_class "validation-stats"] [
                div [_class "stat-item"] [
                    span [_class "stat-label"] [encodedText "Files with Issues:"]
                    span [_class "stat-value"] [encodedText (string totalFiles)]
                ]
                div [_class "stat-item"] [
                    span [_class "stat-label"] [encodedText "Errors:"]
                    span [_class $"stat-value {errorClass}"] [encodedText (string totalErrors)]
                ]
                div [_class "stat-item"] [
                    span [_class "stat-label"] [encodedText "Warnings:"]
                    span [_class $"stat-value {warningClass}"] [encodedText (string totalWarnings)]
                ]
            ]
        
        let content = [
            div [_class "container"] [
                h1 [_class "page-title"] [encodedText "Validation Errors Report"]
                stats
                div [_class "file-list"] fileCards
            ]
        ]
        
        htmlPage "Validation Report" "validation" content

module HtmlEncode =
    let htmlEncode (text: string) : string =
        text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;")
