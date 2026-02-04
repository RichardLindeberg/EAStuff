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
    
    /// HTML header with navigation
    let htmlHeader (title: string) (currentPage: string) =
        let navItems =
            Config.layerOrder
            |> List.map (fun layer ->
                let isActive = currentPage = layer.key
                let activeClass = if isActive then "active" else ""
                a [_href $"{baseUrl}{layer.key}"; _class $"nav-link {activeClass}"] [
                    encodedText layer.displayName
                ]
            )
        
        let tagsActive = if currentPage = "tags" then "active" else ""
        let navLinks = 
            navItems @ [
                a [_href $"{baseUrl}tags"; _class $"nav-link {tagsActive}"] [
                    encodedText "Tags"
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
    
    /// CSS stylesheet
    let stylesheet =
        """
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            line-height: 1.6;
            color: #333;
            background: #f5f5f5;
        }
        
        header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 1.5rem 0;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        
        .header-content {
            max-width: 1400px;
            margin: 0 auto;
            padding: 0 2rem;
        }
        
        header h1 {
            font-size: 2rem;
            margin-bottom: 0.5rem;
        }
        
        nav {
            display: flex;
            flex-wrap: wrap;
            gap: 0.5rem;
            margin-top: 1rem;
        }
        
        .nav-link {
            color: white;
            text-decoration: none;
            padding: 0.5rem 1rem;
            background: rgba(255,255,255,0.1);
            border-radius: 4px;
            transition: all 0.3s;
            font-size: 0.9rem;
        }
        
        .nav-link:hover {
            background: rgba(255,255,255,0.2);
            transform: translateY(-2px);
        }
        
        .nav-link.active {
            background: rgba(255,255,255,0.3);
            font-weight: 600;
        }
        
        .container {
            max-width: 1400px;
            margin: 2rem auto;
            padding: 0 2rem;
        }
        
        .breadcrumb {
            margin-bottom: 1.5rem;
            color: #666;
        }
        
        .breadcrumb a {
            color: #667eea;
            text-decoration: none;
        }
        
        .breadcrumb a:hover {
            text-decoration: underline;
        }
        
        .element-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 1.5rem;
            margin-top: 2rem;
        }
        
        .element-card {
            background: white;
            border-radius: 8px;
            padding: 1.5rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            transition: all 0.3s;
            border-left: 4px solid #667eea;
        }
        
        .element-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 4px 16px rgba(0,0,0,0.15);
        }
        
        .element-card h3 {
            color: #667eea;
            margin-bottom: 0.5rem;
            font-size: 1.2rem;
        }
        
        .element-card h3 a {
            color: inherit;
            text-decoration: none;
        }
        
        .element-card h3 a:hover {
            text-decoration: underline;
        }
        
        .element-type {
            display: inline-block;
            padding: 0.25rem 0.75rem;
            background: #e0e7ff;
            color: #4c51bf;
            border-radius: 12px;
            font-size: 0.85rem;
            font-weight: 500;
            margin-bottom: 0.5rem;
        }
        
        .element-description {
            color: #666;
            font-size: 0.95rem;
            margin-top: 0.5rem;
        }
        
        .element-detail {
            background: white;
            border-radius: 8px;
            padding: 2rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        
        .element-detail h2 {
            color: #667eea;
            margin-bottom: 1rem;
            font-size: 2rem;
        }
        
        .metadata {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1rem;
            margin: 1.5rem 0;
            padding: 1rem;
            background: #f8f9fa;
            border-radius: 6px;
        }
        
        .metadata-item {
            display: flex;
            flex-direction: column;
        }
        
        .metadata-label {
            font-weight: 600;
            color: #4a5568;
            font-size: 0.85rem;
            text-transform: uppercase;
            margin-bottom: 0.25rem;
        }
        
        .metadata-value {
            color: #2d3748;
        }
        
        .relations-section {
            margin-top: 2rem;
        }
        
        .relations-section h3 {
            color: #4a5568;
            margin-bottom: 1rem;
            font-size: 1.3rem;
            border-bottom: 2px solid #e2e8f0;
            padding-bottom: 0.5rem;
        }
        
        .relation-list {
            list-style: none;
            margin-top: 1rem;
        }
        
        .relation-item {
            padding: 1rem;
            background: #f8f9fa;
            border-left: 3px solid #667eea;
            margin-bottom: 0.75rem;
            border-radius: 4px;
        }
        
        .relation-item.incoming {
            border-left-color: #48bb78;
        }
        
        .relation-type {
            display: inline-block;
            padding: 0.2rem 0.6rem;
            background: #667eea;
            color: white;
            border-radius: 10px;
            font-size: 0.8rem;
            font-weight: 500;
            margin-right: 0.5rem;
        }
        
        .relation-item.incoming .relation-type {
            background: #48bb78;
        }
        
        .relation-item a {
            color: #667eea;
            text-decoration: none;
            font-weight: 500;
        }
        
        .relation-item a:hover {
            text-decoration: underline;
        }
        
        .relation-description {
            color: #666;
            font-size: 0.9rem;
            margin-top: 0.25rem;
        }
        
        .tags {
            display: flex;
            flex-wrap: wrap;
            gap: 0.5rem;
            margin-top: 1rem;
        }
        
        .tag {
            padding: 0.25rem 0.75rem;
            background: #edf2f7;
            color: #4a5568;
            border-radius: 12px;
            font-size: 0.85rem;
            text-decoration: none;
            transition: all 0.2s;
        }
        
        .tag:hover {
            background: #e0e7ff;
            color: #4c51bf;
        }
        
        .content-section {
            margin-top: 2rem;
            line-height: 1.8;
        }
        
        .content-section h2,
        .content-section h3 {
            color: #2d3748;
            margin-top: 1.5rem;
            margin-bottom: 0.75rem;
        }
        
        .content-section ul {
            margin-left: 2rem;
            margin-top: 0.5rem;
        }
        
        .content-section code {
            background: #f7fafc;
            padding: 0.2rem 0.4rem;
            border-radius: 3px;
            font-family: 'Courier New', monospace;
            font-size: 0.9em;
        }
        
        .layer-title {
            font-size: 2.5rem;
            color: #2d3748;
            margin-bottom: 0.5rem;
        }
        
        .layer-description {
            color: #666;
            font-size: 1.1rem;
            margin-bottom: 1rem;
        }
        
        .element-count {
            color: #666;
            font-size: 1rem;
        }
        
        footer {
            text-align: center;
            padding: 2rem;
            color: #666;
            font-size: 0.9rem;
            margin-top: 3rem;
        }
        
        .htmx-swapping {
            opacity: 0;
            transition: opacity 0.2s ease-out;
        }
        
        .htmx-settling {
            opacity: 1;
            transition: opacity 0.2s ease-out;
        }
        """
    
    /// Full HTML page template
    let htmlPage (pageTitle: string) (currentPage: string) (content: XmlNode list) =
        html [_lang "en"] [
            head [] [
                meta [_charset "UTF-8"]
                meta [_name "viewport"; _content "width=device-width, initial-scale=1.0"]
                title [] [encodedText $"{pageTitle} - ArchiMate Architecture"]
                style [] [rawText stylesheet]
                script [_src "https://unpkg.com/htmx.org@1.9.10"] []
            ]
            body [] (
                [htmlHeader pageTitle currentPage] @ content @ [htmlFooter ()]
            )
        ]
    
    /// Index page
    let indexPage (registry: ElementRegistry) =
        let layerCards =
            Config.layerOrder
            |> List.choose (fun layer ->
                let elements = ElementRegistry.getLayerElements layer.key registry
                if List.isEmpty elements then None
                else
                    Some (
                        div [_class "element-card"] [
                            h3 [] [
                                a [_href $"{baseUrl}{layer.key}"] [
                                    encodedText layer.displayName
                                ]
                            ]
                            let elemCountStr = let count = List.length elements in sprintf "%d element%s" count (pluralize count "" "s")
                            p [_class "element-count"] [
                                encodedText elemCountStr
                            ]
                            p [_class "element-description"] [
                                encodedText $"View all {layer.key} layer elements and their relationships."
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
                    span [_class "element-type"] [encodedText elem.elementType]
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
                div [_class "element-grid"] elementCards
            ]
        ]
        
        htmlPage layer.displayName layer.key content
    
    /// Element detail page
    let elementPage (elemWithRels: ElementWithRelations) =
        let elem = elemWithRels.element
        
        let relationItem (related: Element) (rel: Relationship) (isIncoming: bool) =
            let relClass = if isIncoming then "incoming" else ""
            li [_class $"relation-item {relClass}"] [
                span [_class "relation-type"] [encodedText rel.relationType]
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
        
        let tagsHtml =
            if not (List.isEmpty elem.tags) then
                [
                    div [_class "relations-section"] [
                        h3 [] [encodedText "Tags"]
                        div [_class "tags"] [
                            for tag in elem.tags do
                                a [_href $"{baseUrl}tags/{tag}"; _class "tag"] [
                                    encodedText tag
                                ]
                        ]
                    ]
                ]
            else []
        
        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}{elem.layer}"] [encodedText (
                        Config.layerOrder
                        |> List.tryFind (fun l -> l.key = elem.layer)
                        |> Option.map (fun l -> l.displayName)
                        |> Option.defaultValue elem.layer
                    )]
                    encodedText " / "
                    encodedText elem.name
                ]
                
                div [_class "element-detail"] [
                    h2 [] [encodedText elem.name]
                    div [_class "metadata"] [
                        div [_class "metadata-item"] [
                            div [_class "metadata-label"] [encodedText "ID"]
                            div [_class "metadata-value"] [encodedText elem.id]
                        ]
                        div [_class "metadata-item"] [
                            div [_class "metadata-label"] [encodedText "Type"]
                            div [_class "metadata-value"] [encodedText elem.elementType]
                        ]
                        div [_class "metadata-item"] [
                            div [_class "metadata-label"] [encodedText "Layer"]
                            div [_class "metadata-value"] [encodedText elem.layer]
                        ]
                    ]
                    
                    if elem.content <> "" then
                        let htmlContent = markdownToHtml elem.content
                        div [_class "content-section"] [
                            rawText htmlContent
                        ]
                    
                    yield! incomingSection
                    yield! outgoingSection
                    yield! tagsHtml
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
                    span [_class "element-type"] [encodedText elem.elementType]
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

module HtmlEncode =
    let htmlEncode (text: string) : string =
        text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;")
