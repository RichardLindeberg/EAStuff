namespace EAArchive

open System
open System.Net
open Giraffe.ViewEngine
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
                link [_rel "stylesheet"; _href "/css/mermaid-diagram.css"]
                script [_src "https://unpkg.com/htmx.org@1.9.10"] []
                script [_src "/js/validation.js"] []
                script [_src "/js/cytoscape-diagram.js"] []
                script [_src "/js/mermaid-diagram.js"] []
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
    
    /// Layer page
    let layerPage (layer: LayerInfo) (elements: Element list) (registry: ElementRegistry) =
        let elementCards =
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
        
        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    encodedText layer.displayName
                ]
                h2 [_class "layer-title"] [encodedText layer.displayName]
                let layerElemCountStr = let count = List.length elements in sprintf "%d element%s" count (pluralize count "" "s")
                p [_class "element-count"] [
                    encodedText layerElemCountStr
                ]
                div [_class "diagram-section"] [
                       a [_class "diagram-link"; _href $"{baseUrl}diagrams/layers/{layer.displayName}"; _target "_blank"; _rel "noopener"] [
                            encodedText "Open diagram ↗"
                        ]                ]
                div [_class "element-grid"] elementCards
            ]
        ]
        
        htmlPage layer.displayName layer.displayName content
    
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
        
        let content = [
            div [_class "container"] [
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
                
                div [_class "element-detail"] [
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
        
        htmlPage elem.name "element" content
    
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
