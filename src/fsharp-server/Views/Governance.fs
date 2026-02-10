namespace EAArchive.Views

open System
open System.Globalization
open System.Text.RegularExpressions
open EAArchive
open EAArchive.DocumentQueries
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

    let private tryGetMetadataValue (key: string) (metadata: Map<string, string>) : string option =
        metadata
        |> Map.tryFind key
        |> Option.bind (fun value ->
            let trimmed = value.Trim()
            if trimmed = "" then None else Some trimmed
        )

    let private tryResolveOwner (archimateLookup: Map<string, DocumentRecord>) (ownerValue: string) : (string * string option) option =
        if String.IsNullOrWhiteSpace ownerValue then None
        else
            match Map.tryFind ownerValue archimateLookup with
            | Some elem -> Some (elem.title, Some ownerValue)
            | None -> Some (ownerValue, None)

    let private buildDocCard (webConfig: WebUiConfig) (archimateLookup: Map<string, DocumentRecord>) (doc: DocumentRecord) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let metadata = getGovernanceMetadataMap doc
        let owner = tryGetMetadataValue "owner" metadata |> Option.defaultValue ""
        let docType = getGovernanceDocType doc

        div [_class "element-card"] [
            span [_class "element-type"] [encodedText (docTypeLabel docType)]
            h3 [] [
                a [_href $"{baseUrl}governance/{doc.slug}"] [
                    encodedText doc.title
                ]
            ]
            if owner <> "" then
                let ownerLabel, ownerLink =
                    match tryResolveOwner archimateLookup owner with
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

    let private renderDocumentsSection (webConfig: WebUiConfig) (archimateLookup: Map<string, DocumentRecord>) (documents: DocumentRecord list) : XmlNode list =
        if List.isEmpty documents then
            [
                div [_class "content-section"] [
                    p [] [encodedText "No governance documents found."]
                ]
            ]
        else
            let grouped =
                documents
                |> List.groupBy getGovernanceDocType
                |> List.sortBy (fun (docType, _) -> docTypeOrder docType)

            [
                for (docType, docs) in grouped do
                    let label = docTypePluralLabel docType
                    let count = List.length docs
                    div [_class "content-section"] [
                        h3 [] [encodedText $"{label} ({count})"]
                        div [_class "element-grid"] [
                            for doc in docs |> List.sortBy (fun d -> d.title) do
                                buildDocCard webConfig archimateLookup doc
                        ]
                    ]
            ]

    let documentsPartial (webConfig: WebUiConfig) (archimateLookup: Map<string, DocumentRecord>) (documents: DocumentRecord list) : XmlNode =
        div [_id "governance-documents"] (renderDocumentsSection webConfig archimateLookup documents)

    let indexPage
        (webConfig: WebUiConfig)
        (archimateLookup: Map<string, DocumentRecord>)
        (documents: DocumentRecord list)
        (filteredDocuments: DocumentRecord list)
        (filterValue: string option)
        (docTypeValue: string option)
        (reviewValue: string option) : XmlNode =
        let docTypeOptions =
            documents
            |> List.map getGovernanceDocType
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
                    let optionValue = docTypeToString value
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
                documentsPartial webConfig archimateLookup filteredDocuments
            ]
        ]

        htmlPage webConfig "Governance" "governance" content

    let private stripTitleHeading (content: string) : string =
        let lines = content.Split('\n')
        if lines.Length > 0 && lines.[0].Trim().StartsWith("# ") then
            String.concat "\n" lines.[1..]
        else
            content

    let private linkRelatedDocs (baseUrl: string) (governanceBySlug: Map<string, DocumentRecord>) (content: string) : string =
        let pattern = "^\\s*[-*]\\s+Related\\s+[^:]+:\\s+([a-z0-9-]+)\\.md\\s*$"
        let regex = Regex(pattern, RegexOptions.IgnoreCase ||| RegexOptions.Multiline)

        regex.Replace(content, fun (m: Match) ->
            let slug = m.Groups.[1].Value
            let prefix = m.Value.Substring(0, m.Value.IndexOf(':') + 1)
            let linkLabel =
                governanceBySlug
                |> Map.tryFind slug
                |> Option.map (fun doc -> doc.title)
                |> Option.defaultValue slug
            $"{prefix} [{linkLabel}]({baseUrl}governance/{slug})"
        )

    let private formatRelationType (value: RelationType) : string =
        ElementType.relationTypeToDisplayName value

    let documentPage
        (webConfig: WebUiConfig)
        (archimateLookup: Map<string, DocumentRecord>)
        (governanceById: Map<string, DocumentRecord>)
        (governanceBySlug: Map<string, DocumentRecord>)
        (detail: GovernanceDetailView) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let metadataItems = detail.metadataItems

        let metadataNodes =
            metadataItems
            |> List.map (fun (label, value) ->
                let valueNode =
                    if label = "Owner" then
                        match tryResolveOwner archimateLookup value with
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

        let buildRelationItem (relationLabel: string) (targetType: string) (targetLabel: string) (targetLink: string option) : XmlNode =
            li [_class "relation-item"] [
                span [_class "relation-type governance-relation-type"] [encodedText relationLabel]
                span [_class "governance-relation-label"] [encodedText targetType]
                match targetLink with
                | Some link ->
                    a [_href link] [
                        encodedText targetLabel
                    ]
                | None ->
                    span [] [encodedText targetLabel]
            ]

        let governanceRelationItems =
            detail.governanceRelations
            |> List.map (fun rel ->
                let relationLabel = formatRelationType rel.relationType
                let targetType = docTypeToString rel.docType
                let targetLink = Some $"{baseUrl}governance/{rel.slug}"
                buildRelationItem relationLabel targetType rel.title targetLink
            )

        let archimateRelationItems =
            detail.archimateRelations
            |> List.map (fun rel ->
                let relationLabel = formatRelationType rel.relationType
                let targetType = "Architecture"
                let targetLink = Some $"{baseUrl}elements/{rel.relatedId}"
                buildRelationItem relationLabel targetType rel.relatedName targetLink
            )

        let archimateIncomingItems =
            detail.archimateIncomingRelations
            |> List.map (fun rel ->
                let relationLabel = formatRelationType rel.relationType
                let targetType = "Architecture"
                let targetLink = Some $"{baseUrl}elements/{rel.relatedId}"
                buildRelationItem relationLabel targetType rel.relatedName targetLink
            )

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}governance"] [encodedText "Governance"]
                    encodedText " / "
                    encodedText detail.title
                ]
                div [_class "element-detail"] [
                    div [_class "element-view"] [
                        h2 [] [encodedText detail.title]
                        details [_class "metadata-details"] [
                            summary [_class "metadata-summary"] [
                                encodedText "Metadata"
                            ]
                            div [_class "metadata"] [
                                div [_class "metadata-item"] [
                                    div [_class "metadata-label"] [encodedText "Type"]
                                    div [_class "metadata-value"] [encodedText (docTypeLabel detail.docType)]
                                ]
                                div [_class "metadata-item"] [
                                    div [_class "metadata-label"] [encodedText "Slug"]
                                    div [_class "metadata-value"] [encodedText detail.slug]
                                ]
                                if not (List.isEmpty metadataItems) then
                                    yield! metadataNodes
                            ]
                                
                        ]

                        details [_class "metadata-details"] [
                            summary [_class "metadata-summary"] [
                                encodedText "Diagram"
                            ]
                            div [_class "diagram-section"] [
                                div [_class "diagram-links"] [
                                    p [] [encodedText "View related architecture and governance links for this document:"]
                                    a [_href $"{baseUrl}diagrams/governance/{detail.slug}"; _class "diagram-link"; _target "_blank"; _rel "noopener"] [
                                        encodedText "Open governance diagram"
                                    ]
                                ]
                            ]
                        ]

                        details [_class "metadata-details"] [
                            summary [_class "metadata-summary"] [
                                encodedText "Relations"
                            ]
                            if not (List.isEmpty governanceRelationItems) then
                                div [_class "relations-section"] [
                                    h3 [] [encodedText "Governance Relations"]
                                    ul [_class "relation-list"] governanceRelationItems
                                ]

                            if not (List.isEmpty archimateRelationItems) then
                                div [_class "relations-section"] [
                                    h3 [] [encodedText "Architecture Relations"]
                                    ul [_class "relation-list"] archimateRelationItems
                                ]

                            if not (List.isEmpty archimateIncomingItems) then
                                div [_class "relations-section"] [
                                    h3 [] [encodedText "Referenced By Architecture"]
                                    ul [_class "relation-list"] archimateIncomingItems
                                ]
                        ]

                        if not (String.IsNullOrWhiteSpace detail.content) then
                            let htmlContent =
                                detail.content
                                |> stripTitleHeading
                                |> linkRelatedDocs baseUrl governanceBySlug
                                |> markdownToHtml
                            div [_class "content-section"] [
                                rawText htmlContent
                            ]
                    ]
                ]
            ]
        ]

        htmlPage webConfig detail.title "governance" content
