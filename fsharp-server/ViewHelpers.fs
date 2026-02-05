namespace EAArchive

open System
open System.Net
open Giraffe.ViewEngine
open Markdig
open Ganss.Xss

module ViewHelpers =

    let private htmlSanitizer: HtmlSanitizer =
        let sanitizer = HtmlSanitizer()
        sanitizer.AllowedTags.Add("table") |> ignore
        sanitizer.AllowedTags.Add("thead") |> ignore
        sanitizer.AllowedTags.Add("tbody") |> ignore
        sanitizer.AllowedTags.Add("tfoot") |> ignore
        sanitizer.AllowedTags.Add("tr") |> ignore
        sanitizer.AllowedTags.Add("th") |> ignore
        sanitizer.AllowedTags.Add("td") |> ignore
        sanitizer

    /// Convert markdown to HTML
    let markdownToHtml (markdown: string) : string =
        try
            let pipeline = MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            let html = Markdown.ToHtml(markdown, pipeline)
            htmlSanitizer.Sanitize(html)
        with
        | ex ->
            sprintf "<p>Error rendering markdown: %s</p>" (WebUtility.HtmlEncode ex.Message)

    /// Helper to pluralize words
    let pluralize (count: int) (singular: string) (plural: string) : string =
        if count <> 1 then plural else singular

    let optionNode (value: string) (selected: bool) : XmlNode =
        let encodedValue = WebUtility.HtmlEncode value
        let selectedAttr = if selected then " selected" else ""
        rawText $"<option value=\"{encodedValue}\"{selectedAttr}>{encodedValue}</option>"

    let placeholderOptionNode (label: string) (selected: bool) : XmlNode =
        let encodedLabel = WebUtility.HtmlEncode label
        let selectedAttr = if selected then " selected" else ""
        rawText $"<option value=\"\"{selectedAttr}>{encodedLabel}</option>"

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
