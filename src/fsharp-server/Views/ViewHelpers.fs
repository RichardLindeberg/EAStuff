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

    /// Convert GovernanceDocType to string for display
    let docTypeToString (docType: GovernanceDocType) : string =
        match docType with
        | GovernanceDocType.Policy -> "policy"
        | GovernanceDocType.Instruction -> "instruction"
        | GovernanceDocType.Manual -> "manual"
        | GovernanceDocType.Unknown value -> value.Trim().ToLowerInvariant()

    /// Convert ElementType to string for display with explicit subtype
    let elementTypeAndSubTypeToString (elementType: ElementType) : string =
        match elementType with
        | ElementType.Strategy StrategyElement.Resource -> "Strategy - Resource"
        | ElementType.Strategy StrategyElement.Capability -> "Strategy - Capability"
        | ElementType.Strategy StrategyElement.ValueStream -> "Strategy - Value Stream"
        | ElementType.Strategy StrategyElement.CourseOfAction -> "Strategy - Course Of Action"

        | ElementType.Motivation MotivationElement.Stakeholder -> "Motivation - Stakeholder"
        | ElementType.Motivation MotivationElement.Driver -> "Motivation - Driver"
        | ElementType.Motivation MotivationElement.Assessment -> "Motivation - Assessment"
        | ElementType.Motivation MotivationElement.Goal -> "Motivation - Goal"
        | ElementType.Motivation MotivationElement.Outcome -> "Motivation - Outcome"
        | ElementType.Motivation MotivationElement.Principle -> "Motivation - Principle"
        | ElementType.Motivation MotivationElement.Requirement -> "Motivation - Requirement"
        | ElementType.Motivation MotivationElement.Constraint -> "Motivation - Constraint"
        | ElementType.Motivation MotivationElement.Meaning -> "Motivation - Meaning"
        | ElementType.Motivation MotivationElement.Value -> "Motivation - Value"

        | ElementType.Business BusinessElement.Actor -> "Business - Actor"
        | ElementType.Business BusinessElement.Role -> "Business - Role"
        | ElementType.Business BusinessElement.Process -> "Business - Process"
        | ElementType.Business BusinessElement.Function -> "Business - Function"
        | ElementType.Business BusinessElement.Service -> "Business - Service"
        | ElementType.Business BusinessElement.Object -> "Business - Object"
        | ElementType.Business BusinessElement.Event -> "Business - Event"
        | ElementType.Business BusinessElement.Product -> "Business - Product"

        | ElementType.Application ApplicationElement.Component -> "Application - Component"
        | ElementType.Application ApplicationElement.Function -> "Application - Function"
        | ElementType.Application ApplicationElement.Service -> "Application - Service"
        | ElementType.Application ApplicationElement.Interface -> "Application - Interface"
        | ElementType.Application ApplicationElement.DataObject -> "Application - Data Object"

        | ElementType.Technology TechnologyElement.Technology -> "Technology - Technology"
        | ElementType.Technology TechnologyElement.Device -> "Technology - Device"
        | ElementType.Technology TechnologyElement.SystemSoftware -> "Technology - System Software"
        | ElementType.Technology TechnologyElement.Service -> "Technology - Service"
        | ElementType.Technology TechnologyElement.Interface -> "Technology - Interface"
        | ElementType.Technology TechnologyElement.Artifact -> "Technology - Artifact"
        | ElementType.Technology TechnologyElement.Node -> "Technology - Node"
        | ElementType.Technology TechnologyElement.CommunicationNetwork -> "Technology - Communication Network"

        | ElementType.Physical PhysicalElement.Equipment -> "Physical - Equipment"
        | ElementType.Physical PhysicalElement.Facility -> "Physical - Facility"
        | ElementType.Physical PhysicalElement.DistributionNetwork -> "Physical - Distribution Network"

        | ElementType.Implementation ImplementationElement.WorkPackage -> "Implementation - Work Package"
        | ElementType.Implementation ImplementationElement.Deliverable -> "Implementation - Deliverable"
        | ElementType.Implementation ImplementationElement.ImplementationEvent -> "Implementation - Implementation Event"
        | ElementType.Implementation ImplementationElement.Plateau -> "Implementation - Plateau"
        | ElementType.Implementation ImplementationElement.Gap -> "Implementation - Gap"

        | ElementType.Unknown (layer, typeName) ->
            if String.IsNullOrWhiteSpace typeName then
                layer
            else
                sprintf "%s - %s" layer typeName
