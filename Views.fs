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
                    encodedText "âš  Validation"
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
            p [] [encodedText "Generated from ArchiMate elements | Â© 2026"]
        ]
    
    /// CSS stylesheet
    // Styles moved to wwwroot/css/site.css

                    function reloadFile(filePath) {
                        const encodedPath = encodeURIComponent(filePath);
                        fetch(`/api/validation/revalidate/${encodedPath}`, { method: 'POST' })
                            .then(r => r.json())
                            .then(data => {
                                alert(`File reloaded: ${data.errorCount} errors found`);
                                location.reload();
                            })
                            .catch(err => alert(`Error: ${err.message}`));
                    }
                    """
                ]
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
