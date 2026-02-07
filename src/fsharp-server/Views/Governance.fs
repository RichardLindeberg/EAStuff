namespace EAArchive.Views

open System
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
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

    let private buildDocCard (webConfig: WebUiConfig) (doc: GovernanceDocument) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let owner = tryGetMetadataValue "owner" doc.metadata |> Option.defaultValue ""
        let reviewCycle = tryGetMetadataValue "review cycle" doc.metadata |> Option.defaultValue ""

        div [_class "element-card"] [
            span [_class "element-type"] [encodedText (docTypeLabel doc.docType)]
            h3 [] [
                a [_href $"{baseUrl}governance/{doc.slug}"] [
                    encodedText doc.title
                ]
            ]
            if owner <> "" then
                p [_class "element-description"] [encodedText $"Owner: {owner}"]
            if reviewCycle <> "" then
                p [_style "margin-top: 0.25rem; font-size: 0.85rem; color: #888;"] [
                    encodedText $"Review cycle: {reviewCycle}"
                ]
        ]

    let indexPage (webConfig: WebUiConfig) (registry: GovernanceRegistry) : XmlNode =
        let documents = registry.documents |> Map.toList |> List.map snd
        let grouped =
            documents
            |> List.groupBy (fun doc -> doc.docType)
            |> List.sortBy (fun (docType, _) -> docTypeOrder docType)

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Governance System"]
                p [_class "layer-description"] [
                    encodedText "Policies, instructions, and manuals that define how the organization is governed, operated, and improved."
                ]
                if List.isEmpty documents then
                    div [_class "content-section"] [
                        p [] [encodedText "No governance documents found yet."]
                    ]
                else
                    for (docType, docs) in grouped do
                        let label = docTypePluralLabel docType
                        let count = List.length docs
                        div [_class "content-section"] [
                            h3 [] [encodedText $"{label}s ({count})"]
                            div [_class "element-grid"] [
                                for doc in docs |> List.sortBy (fun d -> d.title) do
                                    buildDocCard webConfig doc
                            ]
                        ]
            ]
        ]

        htmlPage webConfig "Governance" "governance" content

    let private stripTitleHeading (content: string) : string =
        let lines = content.Split('\n')
        if lines.Length > 0 && lines.[0].Trim().StartsWith("# ") then
            String.concat "\n" lines.[1..]
        else
            content

    let documentPage (webConfig: WebUiConfig) (registry: ElementRegistry) (doc: GovernanceDocument) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let metadataOrder =
            [
                "document id", "Document ID"
                "owner", "Owner"
                "approved by", "Approved by"
                "status", "Status"
                "version", "Version"
                "effective date", "Effective date"
                "review cycle", "Review cycle"
                "next review", "Next review"
            ]

        let metadataItems =
            metadataOrder
            |> List.choose (fun (key, label) ->
                tryGetMetadataValue key doc.metadata
                |> Option.map (fun value -> label, value)
            )

        let relationItems =
            doc.relations
            |> List.map (fun rel ->
                let targetLabel =
                    match ElementRegistry.getElement rel.target registry with
                    | Some elem -> elem.name
                    | None -> rel.target

                li [_class "relation-item"] [
                    span [_class "relation-type"] [encodedText rel.relationType]
                    a [_href $"{baseUrl}elements/{rel.target}"] [
                        encodedText targetLabel
                    ]
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
                                    for (label, value) in metadataItems do
                                        div [_class "metadata-item"] [
                                            div [_class "metadata-label"] [encodedText label]
                                            div [_class "metadata-value"] [encodedText value]
                                        ]
                                ]
                            ]

                        if not (List.isEmpty relationItems) then
                            div [_class "relations-section"] [
                                h3 [] [encodedText "Relations"]
                                ul [_class "relation-list"] relationItems
                            ]

                        if not (String.IsNullOrWhiteSpace doc.content) then
                            let htmlContent = doc.content |> stripTitleHeading |> markdownToHtml
                            div [_class "content-section"] [
                                rawText htmlContent
                            ]
                    ]
                ]
            ]
        ]

        htmlPage webConfig doc.title "governance" content
