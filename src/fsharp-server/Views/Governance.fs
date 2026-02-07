namespace EAArchive.Views

open System
open System.Globalization
open System.Text.RegularExpressions
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Htmx.HtmxAttrs
open Common

module Governance =

    let private docTypeLabel (docType: GovernanceDocType) : string =
        match docType with
        | GovernanceDocType.Policy -> "Policy"
        | GovernanceDocType.Instruction -> "Instruction"
        | GovernanceDocType.Manual -> "Manual"
        | GovernanceDocType.Unknown value -> value

    let private docTypeOrder (docType: GovernanceDocType) : int =
        match docType with
        | GovernanceDocType.Policy -> 0
        | GovernanceDocType.Instruction -> 1
        | GovernanceDocType.Manual -> 2
        | GovernanceDocType.Unknown _ -> 3

    let private docTypePluralLabel (docType: GovernanceDocType) : string =
        match docType with
        | GovernanceDocType.Policy -> "Policies"
        | GovernanceDocType.Instruction -> "Instructions"
        | GovernanceDocType.Manual -> "Manuals"
        | GovernanceDocType.Unknown value -> value

    let private docTypeFilterValue (docType: GovernanceDocType) : string =
        match docType with
        | GovernanceDocType.Policy -> "policy"
        | GovernanceDocType.Instruction -> "instruction"
        | GovernanceDocType.Manual -> "manual"
        | GovernanceDocType.Unknown value -> value.Trim().ToLowerInvariant()

    let private tryGetMetadataValue (key: string) (metadata: Map<string, string>) : string option =
        metadata
        |> Map.tryFind key
        |> Option.bind (fun value ->
            let trimmed = value.Trim()
            if trimmed = "" then None else Some trimmed
        )

    let private tryResolveOwner (registry: ElementRegistry) (ownerValue: string) : (string * string option) option =
        if String.IsNullOrWhiteSpace ownerValue then None
        else
            match ElementRegistry.getElement ownerValue registry with
            | Some elem ->
                let label = elem.name
                Some (label, Some ownerValue)
            | None ->
                Some (ownerValue, None)

    let private buildDocCard (webConfig: WebUiConfig) (registry: ElementRegistry) (doc: GovernanceDocument) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let owner = tryGetMetadataValue "owner" doc.metadata |> Option.defaultValue ""

        div [_class "element-card"] [
            span [_class "element-type"] [encodedText (docTypeLabel doc.docType)]
            h3 [] [
                a [_href $"{baseUrl}governance/{doc.slug}"] [
                    encodedText doc.title
                ]
            ]
            if owner <> "" then
                let ownerLabel, ownerLink =
                    match tryResolveOwner registry owner with
                    | Some (label, linkOpt) -> label, linkOpt
                    | None -> owner, None

                p [_class "element-description"] [
                    encodedText "Owner: "
                    match ownerLink with
                    | Some elemId ->
                        a [_href $"{baseUrl}elements/{elemId}"] [encodedText ownerLabel]
                    | None ->
                        encodedText ownerLabel
                ]
        ]

    let private renderDocumentsSection (webConfig: WebUiConfig) (registry: ElementRegistry) (documents: GovernanceDocument list) : XmlNode list =
        if List.isEmpty documents then
            [
                div [_class "content-section"] [
                    p [] [encodedText "No governance documents found."]
                ]
            ]
        else
            let grouped =
                documents
                |> List.groupBy (fun doc -> doc.docType)
                |> List.sortBy (fun (docType, _) -> docTypeOrder docType)

            [
                for (docType, docs) in grouped do
                    let label = docTypePluralLabel docType
                    let count = List.length docs
                    div [_class "content-section"] [
                        h3 [] [encodedText $"{label} ({count})"]
                        div [_class "element-grid"] [
                            for doc in docs |> List.sortBy (fun d -> d.title) do
                                buildDocCard webConfig registry doc
                        ]
                    ]
            ]

    let documentsPartial (webConfig: WebUiConfig) (registry: ElementRegistry) (documents: GovernanceDocument list) : XmlNode =
        div [_id "governance-documents"] (renderDocumentsSection webConfig registry documents)

    let indexPage
        (webConfig: WebUiConfig)
        (registry: GovernanceDocRegistry)
        (elementRegistry: ElementRegistry)
        (filteredDocuments: GovernanceDocument list)
        (filterValue: string option)
        (docTypeValue: string option)
        (reviewValue: string option) : XmlNode =
        let allDocuments = registry.documents |> Map.toList |> List.map snd
        let docTypeOptions =
            allDocuments
            |> List.map (fun doc -> doc.docType)
            |> List.distinct
            |> List.sortBy docTypeOrder

        let filterAttrs =
            [
                _type "text"
                _id "governance-filter"
                _name "filter"
                _class "filter-input"
                _placeholder "Filter documents by name"
                attr "aria-label" "Filter documents by name"
                _hxGet $"{webConfig.BaseUrl}governance"
                _hxTarget "#governance-documents"
                _hxTrigger "keyup changed delay:300ms"
                attr "hx-include" "#governance-filter, #doctype-filter, #review-filter"
                _hxPushUrl "true"
            ]
            |> List.append (
                match filterValue with
                | Some value -> [ _value value ]
                | None -> []
            )

        let docTypeSelect =
            let placeholder = placeholderOptionNode "All types" (docTypeValue.IsNone)
            let optionNodes =
                docTypeOptions
                |> List.map (fun value ->
                    let optionValue = docTypeFilterValue value
                    optionNode optionValue (docTypeValue = Some optionValue)
                )

            select [
                _id "doctype-filter"
                _name "docType"
                _class "filter-input"
                _hxGet $"{webConfig.BaseUrl}governance"
                _hxTarget "#governance-documents"
                _hxTrigger "change"
                attr "hx-include" "#governance-filter, #doctype-filter, #review-filter"
                _hxPushUrl "true"
                attr "aria-label" "Filter documents by type"
            ] (placeholder :: optionNodes)

        let reviewSelect =
            let placeholder = placeholderOptionNode "All review dates" (reviewValue.IsNone)
            let optionNodes =
                [
                    optionNode "overdue" (reviewValue = Some "overdue")
                    optionNode "due-soon" (reviewValue = Some "due-soon")
                ]

            select [
                _id "review-filter"
                _name "review"
                _class "filter-input"
                _hxGet $"{webConfig.BaseUrl}governance"
                _hxTarget "#governance-documents"
                _hxTrigger "change"
                attr "hx-include" "#governance-filter, #doctype-filter, #review-filter"
                _hxPushUrl "true"
                attr "aria-label" "Filter documents by review date"
            ] (placeholder :: optionNodes)

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Governance System"]
                p [_class "layer-description"] [
                    encodedText "Policies, instructions, and manuals that define how the organization is governed, operated, and improved."
                ]
                div [_class "diagram-section"] [
                    div [_class "diagram-toolbar"] [
                        div [_class "filter-bar"] [
                            label [_class "filter-label"; _for "governance-filter"] [
                                encodedText "Filter:"
                            ]
                            input filterAttrs
                            label [_class "filter-label"; _for "doctype-filter"] [
                                encodedText "Type:"
                            ]
                            docTypeSelect
                            label [_class "filter-label"; _for "review-filter"] [
                                encodedText "Review:"
                            ]
                            reviewSelect
                        ]
                        p [_class "filter-hint"] [
                            encodedText "Search matches title, slug, and owner. Review uses next_review date."
                        ]
                    ]
                ]
                documentsPartial webConfig elementRegistry filteredDocuments
            ]
        ]

        htmlPage webConfig "Governance" "governance" content

    let private stripTitleHeading (content: string) : string =
        let lines = content.Split('\n')
        if lines.Length > 0 && lines.[0].Trim().StartsWith("# ") then
            String.concat "\n" lines.[1..]
        else
            content

    let private linkRelatedDocs (baseUrl: string) (governanceRegistry: GovernanceDocRegistry) (content: string) : string =
        let pattern = "^\\s*[-*]\\s+Related\\s+[^:]+:\\s+([a-z0-9-]+)\\.md\\s*$"
        let regex = Regex(pattern, RegexOptions.IgnoreCase ||| RegexOptions.Multiline)

        regex.Replace(content, fun (m: Match) ->
            let slug = m.Groups.[1].Value
            let prefix = m.Value.Substring(0, m.Value.IndexOf(':') + 1)
            let linkLabel =
                governanceRegistry.documents
                |> Map.tryFind slug
                |> Option.map (fun doc -> doc.title)
                |> Option.defaultValue slug
            $"{prefix} [{linkLabel}]({baseUrl}governance/{slug})"
        )

    let private formatRelationType (value: string) : string =
        let trimmed = value.Trim()
        if trimmed = "" then
            "Relation"
        else
            let normalized = trimmed.Replace("_", " ").Replace("-", " ")
            let spaced = Regex.Replace(normalized, "([a-z])([A-Z])", "$1 $2")
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(spaced)

    let documentPage (webConfig: WebUiConfig) (registry: ElementRegistry) (governanceRegistry: GovernanceDocRegistry) (doc: GovernanceDocument) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let metadataOrder =
            [
                "id", "Document ID"
                "owner", "Owner"
                "approved_by", "Approved by"
                "status", "Status"
                "version", "Version"
                "effective_date", "Effective date"
                "review_cycle", "Review cycle"
                "next_review", "Next review"
            ]

        let metadataItems =
            metadataOrder
            |> List.choose (fun (key, label) ->
                tryGetMetadataValue key doc.metadata
                |> Option.map (fun value -> key, label, value)
            )

        let metadataNodes =
            metadataItems
            |> List.map (fun (key, label, value) ->
                let valueNode =
                    if key = "owner" then
                        match tryResolveOwner registry value with
                        | Some (ownerLabel, Some ownerId) ->
                            a [_href $"{baseUrl}elements/{ownerId}"] [encodedText ownerLabel]
                        | Some (ownerLabel, None) -> encodedText ownerLabel
                        | None -> encodedText value
                    else
                        encodedText value

                div [_class "metadata-item"] [
                    div [_class "metadata-label"] [encodedText label]
                    div [_class "metadata-value"] [valueNode]
                ]
            )

        let relationItems =
            doc.relations
            |> List.map (fun rel ->
                let targetLabel =
                    match ElementRegistry.getElement rel.target registry with
                    | Some elem -> elem.name
                    | None -> rel.target

                let relationLabel = formatRelationType rel.relationType
                li [_class "relation-item"] [
                    span [_class "relation-type governance-relation-type"] [encodedText relationLabel]
                    span [_class "governance-relation-label"] [encodedText "Governance"]
                    match ElementRegistry.getElement rel.target registry with
                    | Some _ ->
                        a [_href $"{baseUrl}elements/{rel.target}"] [
                            encodedText targetLabel
                        ]
                    | None ->
                        span [] [encodedText targetLabel]
                ]
            )

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}governance"] [encodedText "Governance"]
                    encodedText " / "
                    encodedText doc.title
                ]
                div [_class "element-detail"] [
                    div [_class "element-view"] [
                        h2 [] [encodedText doc.title]
                        div [_class "metadata"] [
                            div [_class "metadata-item"] [
                                div [_class "metadata-label"] [encodedText "Type"]
                                div [_class "metadata-value"] [encodedText (docTypeLabel doc.docType)]
                            ]
                            div [_class "metadata-item"] [
                                div [_class "metadata-label"] [encodedText "Slug"]
                                div [_class "metadata-value"] [encodedText doc.slug]
                            ]
                        ]
                        if not (List.isEmpty metadataItems) then
                            div [_class "properties-section"] [
                                h3 [] [encodedText "Metadata"]
                                div [_class "metadata"] [
                                    yield! metadataNodes
                                ]
                            ]

                        if not (List.isEmpty relationItems) then
                            div [_class "relations-section"] [
                                h3 [] [encodedText "Relations"]
                                ul [_class "relation-list"] relationItems
                            ]

                        if not (String.IsNullOrWhiteSpace doc.content) then
                            let htmlContent =
                                doc.content
                                |> stripTitleHeading
                                |> linkRelatedDocs baseUrl governanceRegistry
                                |> markdownToHtml
                            div [_class "content-section"] [
                                rawText htmlContent
                            ]
                    ]
                ]
            ]
        ]

        htmlPage webConfig doc.title "governance" content
