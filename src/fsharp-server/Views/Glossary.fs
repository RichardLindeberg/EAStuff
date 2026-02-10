namespace EAArchive.Views

open System
open EAArchive
open EAArchive.DocumentQueries
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Htmx.HtmxAttrs
open Common

module Glossary =

    let private buildTermCard (baseUrl: string) (doc: DocumentRecord) : XmlNode =
        let glossaryMeta =
            match doc.metadata with
            | DocumentMetaData.GlossaryMetaData meta -> Some meta
            | _ -> None

        let definition =
            match glossaryMeta with
            | Some meta -> meta.definition
            | None -> ""

        let truncatedDef =
            if definition.Length > 150 then
                definition.Substring(0, 150) + "..."
            else
                definition

        div [_class "element-card"] [
            span [_class "element-type"] [encodedText "Term"]
            h3 [] [
                a [_href $"{baseUrl}glossary/{doc.id}"] [
                    encodedText doc.title
                ]
            ]
            p [_class "element-description"] [
                encodedText truncatedDef
            ]
        ]

    let termsPartial (baseUrl: string) (searchValue: string) (documents: DocumentRecord list) : XmlNode =
        let filteredDocs =
            if String.IsNullOrWhiteSpace searchValue then
                documents
            else
                let query = searchValue.ToLowerInvariant()
                documents
                |> List.filter (fun doc ->
                    doc.title.ToLowerInvariant().Contains(query) ||
                    doc.id.ToLowerInvariant().Contains(query) ||
                    match doc.metadata with
                    | DocumentMetaData.GlossaryMetaData meta ->
                        match meta.aliases with
                        | Some aliases ->
                            aliases
                            |> List.exists (fun alias -> alias.ToLowerInvariant().Contains(query))
                        | None -> false
                    | _ -> false
                )

        let docCount = List.length filteredDocs
        let sortedDocs = filteredDocs |> List.sortBy (fun d -> d.title)

        let childNodes =
            [
                div [_class "filter-bar"] [
                    p [_class "filter-hint"] [
                        encodedText (sprintf "Found %d term%s" docCount (if docCount = 1 then "" else "s"))
                    ]
                ]
            ] @
            (
                if List.isEmpty filteredDocs then
                    [
                        div [_class "content-section"] [
                            p [] [encodedText "No glossary terms found matching your search."]
                        ]
                    ]
                else
                    [
                        div [_class "content-section"] [
                            div [_class "element-grid"] [
                                for doc in sortedDocs do
                                    buildTermCard baseUrl doc
                            ]
                        ]
                    ]
            )

        div [_id "glossary-terms"] childNodes

    let listPage (webConfig: WebUiConfig) (searchQuery: string option) (documents: DocumentRecord list) : XmlNode =
        let baseUrl = webConfig.BaseUrl
        let searchValue = searchQuery |> Option.defaultValue ""

        let filteredDocs =
            if String.IsNullOrWhiteSpace searchValue then
                documents
            else
                let query = searchValue.ToLowerInvariant()
                documents
                |> List.filter (fun doc ->
                    doc.title.ToLowerInvariant().Contains(query) ||
                    doc.id.ToLowerInvariant().Contains(query) ||
                    match doc.metadata with
                    | DocumentMetaData.GlossaryMetaData meta ->
                        match meta.aliases with
                        | Some aliases ->
                            aliases
                            |> List.exists (fun alias -> alias.ToLowerInvariant().Contains(query))
                        | None -> false
                    | _ -> false
                )

        let docCount = List.length filteredDocs
        let sortedDocs = filteredDocs |> List.sortBy (fun d -> d.title)

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Glossary"]
                p [_class "layer-description"] [
                    encodedText "A comprehensive collection of key business and technical terms used across the organization."
                ]

                div [_class "diagram-section"] [
                    div [_class "diagram-toolbar"] [
                        div [_class "filter-bar"] [
                            label [_class "filter-label"; _for "glossary-search"] [
                                encodedText "Search Terms:"
                            ]
                            input [
                                _id "glossary-search"
                                _type "text"
                                _class "filter-input"
                                _name "q"
                                _value searchValue
                                _placeholder "Search by term, definition, or alias..."
                                _hxGet $"{baseUrl}glossary"
                                _hxTarget "#glossary-terms"
                                _hxTrigger "keyup changed delay:300ms"
                                _hxPushUrl "true"
                                attr "aria-label" "Search glossary terms"
                            ]
                        ]
                        p [_class "filter-hint"] [
                            encodedText (sprintf "Found %d term%s" docCount (if docCount = 1 then "" else "s"))
                        ]
                    ]
                ]

                termsPartial baseUrl searchValue filteredDocs
            ]
        ]

        htmlPage webConfig "Glossary" "glossary" content

    let private stripTitleHeading (content: string) : string =
        let lines = content.Split('\n')
        if lines.Length > 0 && lines.[0].Trim().StartsWith("# ") then
            String.concat "\n" lines.[1..]
        else
            content

    let private formatStatus (status: string option) : string =
        match status with
        | Some s -> 
            let formatted = s.ToLower().Replace('_', ' ')
            System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formatted)
        | None -> "Unknown"

    let detailPage (webConfig: WebUiConfig) (detail: GlossaryDetailView) : XmlNode =
        let baseUrl = webConfig.BaseUrl

        let metadataNodes =
            let items = []
            let items = if detail.status.IsSome then items @ ["Status", formatStatus detail.status] else items
            let items = if detail.owner.IsSome then items @ ["Owner", detail.owner.Value] else items

            items
            |> List.map (fun (label, value) ->
                div [_class "metadata-item"] [
                    div [_class "metadata-label"] [encodedText label]
                    div [_class "metadata-value"] [encodedText value]
                ]
            )

        let aliasesNode =
            if detail.aliases.IsSome && not (List.isEmpty detail.aliases.Value) then
                [
                    div [_class "metadata-item"] [
                        div [_class "metadata-label"] [encodedText "Aliases"]
                        div [_class "metadata-value"] [
                            encodedText (String.concat ", " detail.aliases.Value)
                        ]
                    ]
                ]
            else
                []

        let tagsNode =
            if not (List.isEmpty detail.tags) then
                [
                    div [_class "metadata-item"] [
                        div [_class "metadata-label"] [encodedText "Tags"]
                        div [_class "metadata-value"] [
                            div [_class "tag-list"] [
                                for tag in detail.tags do
                                    span [_class "tag"] [encodedText tag]
                            ]
                        ]
                    ]
                ]
            else
                []

        let relatedTermsSection =
            if not (List.isEmpty detail.relatedTerms) then
                let relationItems =
                    detail.relatedTerms
                    |> List.map (fun rel ->
                        li [_class "relation-item"] [
                            a [_href $"{baseUrl}glossary/{rel.targetId}"] [
                                encodedText rel.targetName
                            ]
                            if rel.description.IsSome then
                                span [_class "relation-description"] [
                                    encodedText $" – {rel.description.Value}"
                                ]
                        ]
                    )
                [
                    details [_class "metadata-details"] [
                        summary [_class "metadata-summary"] [
                            encodedText "Related Terms"
                        ]
                        div [_class "relations-section"] [
                            ul [_class "relation-list"] relationItems
                        ]
                    ]
                ]
            else
                []

        let additionalContent =
            if not (String.IsNullOrWhiteSpace detail.content) then
                [
                    div [_class "content-section"] [
                        rawText (Markdig.Markdown.ToHtml(stripTitleHeading detail.content))
                    ]
                ]
            else
                []

        let metadataContent =
            metadataNodes @ aliasesNode @ tagsNode

        let definitionSection =
            [
                div [_class "content-section"] [
                    rawText (Markdig.Markdown.ToHtml detail.definition)
                ]
            ]

        let elementViewChildren =
            [
                h2 [] [encodedText detail.name]
            ] @ definitionSection @
            (
                if not (List.isEmpty metadataContent) then
                    [
                        details [_class "metadata-details"] [
                            summary [_class "metadata-summary"] [
                                encodedText "Metadata"
                            ]
                            div [_class "metadata"] metadataContent
                        ]
                    ]
                else
                    []
            ) @ relatedTermsSection @ additionalContent

        let content = [
            div [_class "container"] [
                div [_class "breadcrumb"] [
                    a [_href $"{baseUrl}"] [encodedText "Home"]
                    encodedText " / "
                    a [_href $"{baseUrl}glossary"] [encodedText "Glossary"]
                    encodedText " / "
                    encodedText detail.name
                ]
                div [_class "element-detail"] [
                    div [_class "element-view"] elementViewChildren
                ]
            ]
        ]

        htmlPage webConfig detail.name "glossary" content
