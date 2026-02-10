namespace EAArchive

open System
open System.IO
open DocumentRecordHelpers
open DocumentTypeHelpers

module DocumentQueries =

    let private docTypeFromPath (filePath: string) : GovernanceDocType =
        getGovernanceDocTypeFromPath filePath

    let getGovernanceDocType (doc: DocumentRecord) : GovernanceDocType =
        docTypeFromPath doc.filePath

    let getArchimateDocuments (repo: DocumentRepository) : DocumentRecord list =
        repo.documents
        |> Map.toList
        |> List.map snd
        |> List.filter (fun doc -> match doc with | ArchitectureDoc _ -> true | GovernanceDoc _ -> false)

    let getGovernanceDocuments (repo: DocumentRepository) : DocumentRecord list =
        repo.documents
        |> Map.toList
        |> List.map snd
        |> List.filter (fun doc -> match doc with | GovernanceDoc _ -> true | ArchitectureDoc _ -> false)

    let tryGetDocumentById (repo: DocumentRepository) (docId: string) : DocumentRecord option =
        Map.tryFind docId repo.documents

    let tryGetGovernanceBySlug (repo: DocumentRepository) (slug: string) : DocumentRecord option =
        repo.documents
        |> Map.toList
        |> List.map snd
        |> List.tryFind (fun doc ->
            match doc with
            | GovernanceDoc _ -> doc.slug = slug
            | ArchitectureDoc _ -> false)

    let private tryGetArchimateMetadata (doc: DocumentRecord) : ArchimateMetadata option =
        match doc.metadata with
        | DocumentMetaData.ArchiMateMetaData metadata -> Some metadata
        | _ -> None

    let private tryGetGovernanceMetadata (doc: DocumentRecord) : GovernanceMetadata option =
        match doc.metadata with
        | DocumentMetaData.GovernanceDocMetaData metadata -> Some metadata
        | _ -> None

    let getArchimateTypeValue (doc: DocumentRecord) : string =
        tryGetArchimateMetadata doc
        |> Option.map (fun archimate -> archimate.elementType)
        |> Option.defaultValue "unknown"

    let getArchimateLayerValue (doc: DocumentRecord) : string =
        tryGetArchimateMetadata doc
        |> Option.map (fun archimate -> archimate.layerValue)
        |> Option.defaultValue "unknown"

    let getArchimateElementType (doc: DocumentRecord) : ElementType =
        let layerValue = getArchimateLayerValue doc
        let typeValue = getArchimateTypeValue doc
        ElementType.parseElementType layerValue typeValue

    let getArchimateElementTypeKey (doc: DocumentRecord) : string * string =
        let elementType = getArchimateElementType doc
        let layerKey = ElementType.getLayerKey elementType
        let typeKey = ElementType.getTypeKey elementType
        layerKey, typeKey

    let getArchimateProperties (doc: DocumentRecord) : Map<string, string> =
        let sharedValues =
            [
                "owner", doc.owner
                "status", doc.status
                "version", doc.version
                "last-updated", doc.lastUpdated
                "criticality", tryGetArchimateMetadata doc |> Option.bind (fun archimate -> archimate.criticality)
                "review-cycle", doc.reviewCycle
                "next-review", doc.nextReview
            ]
            |> List.choose (fun (key, valueOpt) -> valueOpt |> Option.map (fun value -> key, value))
            |> Map.ofList

        sharedValues

    let getArchimatePropertyValue (doc: DocumentRecord) (key: string) : string option =
        getArchimateProperties doc |> Map.tryFind key

    let getGovernanceMetadataMap (doc: DocumentRecord) : Map<string, string> =
        let governanceFields =
            [
                "approved_by", tryGetGovernanceMetadata doc |> Option.map (fun g -> g.approvedBy)
                "effective_date", tryGetGovernanceMetadata doc |> Option.map (fun g -> g.effectiveDate)
            ]
            |> List.choose (fun (key, valueOpt) -> valueOpt |> Option.map (fun value -> key, value))
            |> Map.ofList

        let sharedFields =
            [
                "id", Some doc.id
                "owner", doc.owner
                "status", doc.status
                "version", doc.version
                "last_updated", doc.lastUpdated
                "review_cycle", doc.reviewCycle
                "next_review", doc.nextReview
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
                | Some sourceDoc when (match sourceDoc with | ArchitectureDoc _ -> true | GovernanceDoc _ -> false) ->
                    Some (sourceDoc, { target = rel.targetId; relationType = ElementType.parseRelationType rel.relationType; description = rel.description })
                | _ -> None
        )

    let getOutgoingArchimateRelations (repo: DocumentRepository) (doc: DocumentRecord) : (DocumentRecord * Relationship) list =
        doc.relationships
        |> List.choose (fun rel ->
            match Map.tryFind rel.target repo.documents with
            | Some targetDoc when (match targetDoc with | ArchitectureDoc _ -> true | GovernanceDoc _ -> false) ->
                Some (targetDoc, rel)
            | _ -> None
        )

    let getGovernanceOwnerDocs (repo: DocumentRepository) (elementId: string) : DocumentRecord list =
        getGovernanceDocuments repo
        |> List.filter (fun doc ->
            match doc.owner with
            | Some ownerId -> ownerId.Equals(elementId, StringComparison.OrdinalIgnoreCase)
            | None -> false
        )

    let getIncomingGovernanceRelations (repo: DocumentRepository) (elementId: string) : (DocumentRecord * Relationship) list =
        getGovernanceDocuments repo
        |> List.collect (fun doc ->
            doc.relationships
            |> List.filter (fun rel -> rel.target.Equals(elementId, StringComparison.OrdinalIgnoreCase))
            |> List.map (fun rel -> doc, rel)
        )

    let selectGovernanceDocs (repo: DocumentRepository) (elementIds: Set<string>) : DocumentRecord list =
        getGovernanceDocuments repo
        |> List.filter (fun doc ->
            let ownerMatch =
                match doc.owner with
                | Some ownerId -> elementIds.Contains(ownerId)
                | None -> false

            let relationMatch =
                doc.relationships
                |> List.exists (fun rel -> elementIds.Contains(rel.target))

            ownerMatch || relationMatch
        )

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
            doc.relationships
            |> List.filter (fun rel ->
                match Map.tryFind rel.target repo.documents with
                | Some targetDoc when (match targetDoc with | ArchitectureDoc _ -> true | GovernanceDoc _ -> false) -> true
                | _ -> false
            )
            |> List.length

        {
            id = doc.id
            name = doc.title
            elementTypeLabel = global.EAArchive.ViewHelpers.elementTypeToString elementType
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

        let governanceLookup =
            getGovernanceDocuments repo
            |> List.map (fun governanceDoc -> governanceDoc.id, governanceDoc)
            |> Map.ofList

        let governanceOutgoing =
            doc.relationships
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
            content = doc.content
            tags = doc.tags
            properties = properties
            incomingRelations = incoming
            outgoingRelations = outgoing
            governanceOwners = governanceOwners
            governanceIncoming = governanceIncoming
            governanceOutgoing = governanceOutgoing
        }

    let createArchimateEdit (doc: DocumentRecord) : ArchimateEditView =
        {
            id = doc.id
            name = doc.title
            typeValue = getArchimateTypeValue doc
            layerValue = getArchimateLayerValue doc
            tags = doc.tags
            properties = getArchimateProperties doc
            relationships = doc.relationships
            content = doc.content
        }

    let createGovernanceCard (archimateLookup: Map<string, DocumentRecord>) (doc: DocumentRecord) : GovernanceCardView =
        let ownerLabel, ownerId =
            match doc.owner with
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
            doc.relationships
            |> List.choose (fun rel ->
                match Map.tryFind rel.target repo.documents with
                | Some target when (match target with | ArchitectureDoc _ -> true | GovernanceDoc _ -> false) ->
                    Some {
                        relatedId = target.id
                        relatedName = target.title
                        relationType = rel.relationType
                        description = rel.description
                    }
                | _ -> None
            )

        let governanceRelations =
            doc.relationships
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

        let archimateIncomingRelations =
            getIncomingArchimateRelations repo doc.id
            |> List.map (fun (source, rel) ->
                {
                    relatedId = source.id
                    relatedName = source.title
                    relationType = rel.relationType
                    description = rel.description
                }
            )

        {
            slug = doc.slug
            title = doc.title
            docType = getGovernanceDocType doc
            metadataItems = metadataItems
            governanceRelations = governanceRelations
            archimateRelations = archimateRelations
            archimateIncomingRelations = archimateIncomingRelations
            content = doc.content
        }
