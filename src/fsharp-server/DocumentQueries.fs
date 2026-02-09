namespace EAArchive

open System
open System.Collections
open System.Collections.Generic
open System.IO
open global.EAArchive.ViewHelpers

module DocumentQueries =

    let private docTypeFromPath (filePath: string) : GovernanceDocType =
        let lower = filePath.ToLowerInvariant()
        if lower.Contains("\\policies\\") || lower.Contains("/policies/") then
            GovernanceDocType.Policy
        elif lower.Contains("\\instructions\\") || lower.Contains("/instructions/") then
            GovernanceDocType.Instruction
        elif lower.Contains("\\manuals\\") || lower.Contains("/manuals/") then
            GovernanceDocType.Manual
        elif lower.Contains("ms-policy-") then
            GovernanceDocType.Policy
        elif lower.Contains("ms-instruction-") then
            GovernanceDocType.Instruction
        elif lower.Contains("ms-manual-") then
            GovernanceDocType.Manual
        else
            GovernanceDocType.Unknown "unknown"

    let getGovernanceDocType (doc: DocumentRecord) : GovernanceDocType =
        docTypeFromPath doc.filePath

    let getArchimateDocuments (repo: DocumentRepository) : DocumentRecord list =
        repo.documents
        |> Map.toList
        |> List.map snd
        |> List.filter (fun doc -> doc.kind = DocumentKind.Architecture)

    let getGovernanceDocuments (repo: DocumentRepository) : DocumentRecord list =
        repo.documents
        |> Map.toList
        |> List.map snd
        |> List.filter (fun doc -> doc.kind = DocumentKind.Governance)

    let tryGetDocumentById (repo: DocumentRepository) (docId: string) : DocumentRecord option =
        Map.tryFind docId repo.documents

    let tryGetGovernanceBySlug (repo: DocumentRepository) (slug: string) : DocumentRecord option =
        repo.documents
        |> Map.toList
        |> List.map snd
        |> List.tryFind (fun doc -> doc.kind = DocumentKind.Governance && doc.slug = slug)

    let private tryGetPropertiesMap (extensions: Map<string, obj>) : Map<string, obj> =
        match Map.tryFind "properties" extensions with
        | Some (:? Map<string, obj> as props) -> props
        | Some (:? IDictionary as dict) ->
            dict
            |> Seq.cast<obj>
            |> Seq.choose (fun entry ->
                match entry with
                | :? DictionaryEntry as de ->
                    match de.Key with
                    | :? string as key -> Some (key, de.Value)
                    | _ -> None
                | :? KeyValuePair<obj, obj> as kvp ->
                    match kvp.Key with
                    | :? string as key -> Some (key, kvp.Value)
                    | _ -> None
                | _ -> None
            )
            |> Map.ofSeq
        | _ -> Map.empty

    let private objToString (value: obj) : string option =
        if isNull value then None
        else
            let text = value.ToString().Trim()
            if text = "" then None else Some text

    let getArchimateTypeValue (doc: DocumentRecord) : string =
        doc.metadata.archimate
        |> Option.map (fun archimate -> archimate.elementType)
        |> Option.defaultValue "unknown"

    let getArchimateLayerValue (doc: DocumentRecord) : string =
        doc.metadata.archimate
        |> Option.map (fun archimate -> archimate.layerValue)
        |> Option.defaultValue "unknown"

    let getArchimateLayer (doc: DocumentRecord) : Layer =
        doc.metadata.archimate
        |> Option.map (fun archimate -> archimate.layer)
        |> Option.defaultValue (Layer.Unknown "unknown")

    let getArchimateElementType (doc: DocumentRecord) : ElementType =
        let layerValue = getArchimateLayerValue doc
        let typeValue = getArchimateTypeValue doc
        ElementType.parseElementType layerValue typeValue

    let getArchimateProperties (doc: DocumentRecord) : Map<string, string> =
        let sharedValues =
            [
                "owner", doc.metadata.owner
                "status", doc.metadata.status
                "version", doc.metadata.version
                "last-updated", doc.metadata.lastUpdated
                "criticality", doc.metadata.archimate |> Option.bind (fun archimate -> archimate.criticality)
                "review-cycle", doc.metadata.reviewCycle
                "next-review", doc.metadata.nextReview
            ]
            |> List.choose (fun (key, valueOpt) -> valueOpt |> Option.map (fun value -> key, value))
            |> Map.ofList

        let extensionValues =
            tryGetPropertiesMap doc.metadata.extensions
            |> Map.toList
            |> List.choose (fun (key, value) -> objToString value |> Option.map (fun text -> key, text))
            |> Map.ofList

        sharedValues
        |> Map.fold (fun acc key value -> Map.add key value acc) extensionValues

    let getArchimatePropertyValue (doc: DocumentRecord) (key: string) : string option =
        getArchimateProperties doc |> Map.tryFind key

    let getGovernanceMetadataMap (doc: DocumentRecord) : Map<string, string> =
        let governanceFields =
            [
                "approved_by", doc.metadata.governance |> Option.map (fun g -> g.approvedBy)
                "effective_date", doc.metadata.governance |> Option.map (fun g -> g.effectiveDate)
            ]
            |> List.choose (fun (key, valueOpt) -> valueOpt |> Option.map (fun value -> key, value))
            |> Map.ofList

        let sharedFields =
            [
                "id", Some doc.id
                "owner", doc.metadata.owner
                "status", doc.metadata.status
                "version", doc.metadata.version
                "last_updated", doc.metadata.lastUpdated
                "review_cycle", doc.metadata.reviewCycle
                "next_review", doc.metadata.nextReview
            ]
            |> List.choose (fun (key, valueOpt) -> valueOpt |> Option.map (fun value -> key, value))
            |> Map.ofList

        sharedFields
        |> Map.fold (fun acc key value -> Map.add key value acc) governanceFields

    let getIncomingArchimateRelations (repo: DocumentRepository) (docId: string) : (DocumentRecord * Relationship) list =
        repo.relations
        |> List.choose (fun rel ->
            if rel.targetId <> docId then None
            else
                match Map.tryFind rel.sourceId repo.documents with
                | Some sourceDoc when sourceDoc.kind = DocumentKind.Architecture ->
                    Some (sourceDoc, { target = rel.targetId; relationType = ElementType.parseRelationType rel.relationType; description = rel.description })
                | _ -> None
        )

    let getOutgoingArchimateRelations (repo: DocumentRepository) (doc: DocumentRecord) : (DocumentRecord * Relationship) list =
        doc.metadata.relationships
        |> List.choose (fun rel ->
            match Map.tryFind rel.target repo.documents with
            | Some targetDoc when targetDoc.kind = DocumentKind.Architecture -> Some (targetDoc, rel)
            | _ -> None
        )

    let getGovernanceOwnerDocs (repo: DocumentRepository) (elementId: string) : DocumentRecord list =
        getGovernanceDocuments repo
        |> List.filter (fun doc ->
            match doc.metadata.owner with
            | Some ownerId -> ownerId.Equals(elementId, StringComparison.OrdinalIgnoreCase)
            | None -> false
        )

    let getIncomingGovernanceRelations (repo: DocumentRepository) (elementId: string) : (DocumentRecord * Relationship) list =
        getGovernanceDocuments repo
        |> List.collect (fun doc ->
            doc.metadata.relationships
            |> List.filter (fun rel -> rel.target.Equals(elementId, StringComparison.OrdinalIgnoreCase))
            |> List.map (fun rel -> doc, rel)
        )

    let selectGovernanceDocs (repo: DocumentRepository) (elementIds: Set<string>) : DocumentRecord list =
        getGovernanceDocuments repo
        |> List.filter (fun doc ->
            let ownerMatch =
                match doc.metadata.owner with
                | Some ownerId -> elementIds.Contains(ownerId)
                | None -> false

            let relationMatch =
                doc.metadata.relationships
                |> List.exists (fun rel -> elementIds.Contains(rel.target))

            ownerMatch || relationMatch
        )

    let buildTagIndex (docs: DocumentRecord list) : Map<string, string list> =
        docs
        |> List.fold (fun acc doc ->
            doc.metadata.tags
            |> List.fold (fun tagMap tag ->
                match Map.tryFind tag tagMap with
                | Some ids -> Map.add tag (doc.id :: ids) tagMap
                | None -> Map.add tag [doc.id] tagMap
            ) acc
        ) Map.empty

    let createArchimateCard (repo: DocumentRepository) (doc: DocumentRecord) : ArchimateCard =
        let elementType = getArchimateElementType doc
        let description =
            doc.content.Split('\n')
            |> Array.filter (fun line ->
                let trimmed = line.Trim()
                trimmed <> "" && not (trimmed.StartsWith("#"))
            )
            |> Array.tryHead
            |> Option.map (fun line -> if line.Length > 150 then line.Substring(0, 150) + "..." else line)
            |> Option.defaultValue ""

        let incomingCount = getIncomingArchimateRelations repo doc.id |> List.length
        let outgoingCount =
            doc.metadata.relationships
            |> List.filter (fun rel ->
                match Map.tryFind rel.target repo.documents with
                | Some targetDoc when targetDoc.kind = DocumentKind.Architecture -> true
                | _ -> false
            )
            |> List.length

        {
            id = doc.id
            name = doc.title
            elementTypeLabel = elementTypeToString elementType
            description = description
            incomingCount = incomingCount
            outgoingCount = outgoingCount
        }

    let createArchimateDetail (repo: DocumentRepository) (doc: DocumentRecord) : ArchimateDetailView =
        let elementType = getArchimateElementType doc
        let incoming =
            getIncomingArchimateRelations repo doc.id
            |> List.map (fun (source, rel) ->
                {
                    relatedId = source.id
                    relatedName = source.title
                    relationType = rel.relationType
                    description = rel.description
                }
            )

        let outgoing =
            getOutgoingArchimateRelations repo doc
            |> List.map (fun (target, rel) ->
                {
                    relatedId = target.id
                    relatedName = target.title
                    relationType = rel.relationType
                    description = rel.description
                }
            )

        let governanceOwners =
            getGovernanceOwnerDocs repo doc.id
            |> List.map (fun governanceDoc ->
                {
                    docId = governanceDoc.id
                    slug = governanceDoc.slug
                    title = governanceDoc.title
                    docType = getGovernanceDocType governanceDoc
                    relationType = RelationType.Association
                }
            )

        let governanceIncoming =
            getIncomingGovernanceRelations repo doc.id
            |> List.map (fun (governanceDoc, rel) ->
                {
                    docId = governanceDoc.id
                    slug = governanceDoc.slug
                    title = governanceDoc.title
                    docType = getGovernanceDocType governanceDoc
                    relationType = rel.relationType
                }
            )

        let properties =
            [
                "owner", "Owner"
                "status", "Status"
                "criticality", "Criticality"
                "version", "Version"
                "lifecycle-phase", "Lifecycle phase"
                "last-updated", "Last updated"
            ]
            |> List.choose (fun (key, label) ->
                getArchimatePropertyValue doc key
                |> Option.map (fun value -> label, value)
            )

        {
            id = doc.id
            name = doc.title
            elementType = elementType
            layer = getArchimateLayer doc
            content = doc.content
            tags = doc.metadata.tags
            properties = properties
            incomingRelations = incoming
            outgoingRelations = outgoing
            governanceOwners = governanceOwners
            governanceIncoming = governanceIncoming
        }

    let createArchimateEdit (doc: DocumentRecord) : ArchimateEditView =
        {
            id = doc.id
            name = doc.title
            typeValue = getArchimateTypeValue doc
            layerValue = getArchimateLayerValue doc
            tags = doc.metadata.tags
            properties = getArchimateProperties doc
            relationships = doc.metadata.relationships
            content = doc.content
        }

    let createGovernanceCard (archimateLookup: Map<string, DocumentRecord>) (doc: DocumentRecord) : GovernanceCardView =
        let ownerLabel, ownerId =
            match doc.metadata.owner with
            | Some ownerValue when not (String.IsNullOrWhiteSpace ownerValue) ->
                match Map.tryFind ownerValue archimateLookup with
                | Some elem -> Some elem.title, Some ownerValue
                | None -> Some ownerValue, None
            | _ -> None, None

        {
            slug = doc.slug
            title = doc.title
            docType = getGovernanceDocType doc
            ownerLabel = ownerLabel
            ownerId = ownerId
        }

    let createGovernanceDetail (repo: DocumentRepository) (doc: DocumentRecord) : GovernanceDetailView =
        let metadataMap = getGovernanceMetadataMap doc
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
                Map.tryFind key metadataMap
                |> Option.map (fun value -> label, value)
            )

        let governanceLookup =
            getGovernanceDocuments repo
            |> List.map (fun doc -> doc.id, doc)
            |> Map.ofList

        let archimateRelations =
            doc.metadata.relationships
            |> List.choose (fun rel ->
                match Map.tryFind rel.target repo.documents with
                | Some target when target.kind = DocumentKind.Architecture ->
                    Some {
                        relatedId = target.id
                        relatedName = target.title
                        relationType = rel.relationType
                        description = rel.description
                    }
                | _ -> None
            )

        let governanceRelations =
            doc.metadata.relationships
            |> List.choose (fun rel ->
                match Map.tryFind rel.target governanceLookup with
                | Some target ->
                    Some {
                        docId = target.id
                        slug = target.slug
                        title = target.title
                        docType = getGovernanceDocType target
                        relationType = rel.relationType
                    }
                | None -> None
            )

        {
            slug = doc.slug
            title = doc.title
            docType = getGovernanceDocType doc
            metadataItems = metadataItems
            governanceRelations = governanceRelations
            archimateRelations = archimateRelations
            content = doc.content
        }
