namespace EAArchive

open System
open System.Globalization
open System.IO
open System.Net
open System.Xml.Linq
open Microsoft.Extensions.Logging
open Giraffe
open Giraffe.ViewEngine

module Handlers =

    open EAArchive.DiagramGenerators
    open EAArchive.ViewHelpers
    open HandlersHelpers
    open HttpContextHelpers
    open DocumentQueries
    open DocumentRecordHelpers
    
    /// Index/home page handler
    let indexHandler (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET / - Home page requested")
            let repo = repoService.Repository
            let elementTypeCounts =
                getArchimateDocuments repo
                |> List.groupBy getArchimateElementType
                |> List.map (fun (elementType, docs) -> elementType, docs.Length)
                |> Map.ofList
            logger.LogDebug("Element type summary: {elementTypeSummary}", elementTypeCounts)
            let html = Views.Index.indexPage webConfig "index"
            htmlView html next ctx

    /// Architecture index handler
    let architectureIndexHandler (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /architecture - Architecture overview requested")
            let repo = repoService.Repository
            let elementTypeCounts =
                getArchimateDocuments repo
                |> List.groupBy getArchimateElementType
                |> List.map (fun (elementType, docs) -> elementType, docs.Length)

            let html = Views.Architecture.indexPage webConfig elementTypeCounts "architecture"
            htmlView html next ctx

    /// Architecture layer handler
    let architectureLayerHandler (layerKey: string) (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /architecture/layer/{layer} - Architecture layer requested", layerKey)
            let normalizedLayer = layerKey.Trim().ToLowerInvariant()
            let repo = repoService.Repository
            let elementTypeCounts =
                getArchimateDocuments repo
                |> List.groupBy getArchimateElementType
                |> List.map (fun (elementType, docs) -> elementType, docs.Length)
                |> List.filter (fun (elementType, _) -> ElementType.getLayerKey elementType = normalizedLayer)

            let layerDisplayName =
                Config.layerOrder
                |> Map.tryFind normalizedLayer
                |> Option.map (fun info -> info.displayName)
                |> Option.defaultValue layerKey

            let html = Views.Architecture.layerPage webConfig layerDisplayName elementTypeCounts "architecture"
            htmlView html next ctx

    /// Governance system index handler
    let governanceIndexHandler (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /governance - Governance index requested")
            let filterValue = tryGetQueryStringValue ctx "filter"
            let docTypeValue = tryGetQueryStringValueLower ctx "docType"
            let reviewValue = tryGetQueryStringValueLower ctx "review"

            let repo = repoService.Repository
            let documents = getGovernanceDocuments repo
            let archimateLookup =
                getArchimateDocuments repo
                |> List.map (fun doc -> doc.id, doc)
                |> Map.ofList

            let tryParseReviewDate (value: string) : DateTime option =
                let trimmed = value.Trim()
                if trimmed = "" then None
                else
                    match DateTime.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal) with
                    | true, parsed -> Some parsed.Date
                    | _ -> None

            let filteredDocuments =
                documents
                |> List.filter (fun doc ->
                    let metadataMap = getGovernanceMetadataMap doc
                    let ownerId = metadataMap |> Map.tryFind "owner" |> Option.defaultValue ""
                    let ownerLabel =
                        if String.IsNullOrWhiteSpace ownerId then ""
                        else
                            match Map.tryFind ownerId archimateLookup with
                            | Some elem -> elem.title
                            | None -> ownerId
                    let reviewDate =
                        metadataMap |> Map.tryFind "next_review"
                        |> Option.orElseWith (fun () -> metadataMap |> Map.tryFind "next review")
                        |> Option.bind tryParseReviewDate

                    let nameMatches =
                        match filterValue with
                        | Some term ->
                            doc.title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                            || doc.slug.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                            || ownerLabel.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                        | None -> true

                    let typeMatches =
                        match docTypeValue with
                        | Some value ->
                            let docTypeValue = docTypeToString (getGovernanceDocType doc)
                            docTypeValue = value
                        | None -> true

                    let reviewMatches =
                        match reviewValue with
                        | Some "overdue" ->
                            match reviewDate with
                            | Some value -> value < DateTime.UtcNow.Date
                            | None -> false
                        | Some "due-soon" ->
                            match reviewDate with
                            | Some value ->
                                let today = DateTime.UtcNow.Date
                                value >= today && value <= today.AddDays(60.0)
                            | None -> false
                        | Some _ -> false
                        | None -> true

                    nameMatches && typeMatches && reviewMatches
                )

            let isHxRequest = ctx.Request.Headers.ContainsKey "HX-Request"
            if isHxRequest then
                let partial = Views.Governance.documentsPartial webConfig archimateLookup filteredDocuments
                htmlView partial next ctx
            else
                let html = Views.Governance.indexPage webConfig archimateLookup documents filteredDocuments filterValue docTypeValue reviewValue
                htmlView html next ctx

    /// Governance document detail handler
    let governanceDocHandler (slug: string) (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /governance/{slug} - Governance document requested", slug)
            let repo = repoService.Repository
            let archimateLookup =
                getArchimateDocuments repo
                |> List.map (fun doc -> doc.id, doc)
                |> Map.ofList
            let governanceById =
                getGovernanceDocuments repo
                |> List.map (fun doc -> doc.id, doc)
                |> Map.ofList
            let governanceBySlug =
                getGovernanceDocuments repo
                |> List.map (fun doc -> doc.slug, doc)
                |> Map.ofList
            match tryGetGovernanceBySlug repo slug with
            | Some doc ->
                let detail = createGovernanceDetail repo doc
                let html = Views.Governance.documentPage webConfig archimateLookup governanceById governanceBySlug detail
                htmlView html next ctx
            | None ->
                respondNotFound logger "Governance document not found: {slug}" slug "Governance document not found"
                |> fun handler -> handler next ctx
    
    /// Element type page handler
    let elementTypeHandler (layer: string) (typeValue: string) (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /elements/type/{layer}/{type} - Element type page requested", layer, typeValue)
            let normalizedLayer = layer.Trim().ToLowerInvariant()
            let normalizedType = typeValue.Trim().ToLowerInvariant()
            let validLayer = Config.layerOptions |> List.exists (fun value -> value = normalizedLayer)
            let validType = Config.getTypeOptions normalizedLayer |> List.exists (fun value -> value = normalizedType)

            if not validLayer || not validType then
                respondNotFound logger "Element type not found: {layerAndType}" (sprintf "%s/%s" layer typeValue) "Element type not found"
                |> fun handler -> handler next ctx
            else
                let repo = repoService.Repository
                let elementType = ElementType.parseElementType normalizedLayer normalizedType
                let elements =
                    getArchimateDocuments repo
                    |> List.filter (fun doc -> getArchimateElementType doc = elementType)
                    |> List.sortBy (fun doc -> doc.title)
                let filterValue = tryGetQueryStringValue ctx "filter"

                let filteredElements =
                    elements
                    |> List.filter (fun elem ->
                        match filterValue with
                        | Some term -> elem.title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                        | None -> true
                    )

                logger.LogInformation("Found {elementCount} elements for element type {elementType}", List.length elements, elementType)
                let isHxRequest = ctx.Request.Headers.ContainsKey "HX-Request"
                let elementCards = filteredElements |> List.map (createArchimateCard repo)
                if isHxRequest then
                    let partial = Views.Layers.elementTypeElementsPartial webConfig elementCards
                    htmlView partial next ctx
                else
                    let elementTypeLabel = elementTypeAndSubTypeToString elementType
                    let html = Views.Layers.elementTypePage webConfig normalizedLayer normalizedType elementTypeLabel elementCards filterValue
                    htmlView html next ctx
    
    /// Element detail page handler
    let elementHandler (elemId: string) (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /elements/{elementId} - Element detail requested", elemId)
            let repo = repoService.Repository
            match tryGetDocumentById repo elemId with
            | Some doc when isArchitecture doc ->
                logger.LogInformation("Found element: {elementId} ({elementName})", elemId, doc.title)
                let detail = createArchimateDetail repo doc
                let html = Views.Elements.elementPage webConfig detail
                htmlView html next ctx
            | Some _ ->
                respondNotFound logger "Element not found: {elementId}" elemId "Element not found"
                |> fun handler -> handler next ctx
            | None ->
                respondNotFound logger "Element not found: {elementId}" elemId "Element not found"
                |> fun handler -> handler next ctx

    /// Element edit form partial handler
    let elementEditHandler (elemId: string) (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /elements/{elementId}/edit - Element edit form requested", elemId)
            let repo = repoService.Repository
            match tryGetDocumentById repo elemId with
            | Some doc when isArchitecture doc ->
                let editModel = createArchimateEdit doc
                let elementOptions =
                    getArchimateDocuments repo
                    |> List.sortBy (fun d -> d.title)
                    |> List.map (fun d -> d.id, d.title)
                let html = Views.Elements.elementEditFormPartial webConfig editModel elementOptions
                htmlView html next ctx
            | Some _ ->
                respondNotFound logger "Element not found: {elementId}" elemId "Element not found"
                |> fun handler -> handler next ctx
            | None ->
                respondNotFound logger "Element not found: {elementId}" elemId "Element not found"
                |> fun handler -> handler next ctx

    /// New element form handler
    let elementNewHandler (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let layerValue = getQueryStringValueOrEmpty ctx "layer"
            let typeValue = getQueryStringValueOrEmpty ctx "type"
            logger.LogInformation("GET /elements/new - layer={layer}, type={typeValue}", layerValue, typeValue)
            let repo = repoService.Repository
            let elementOptions =
                getArchimateDocuments repo
                |> List.sortBy (fun d -> d.title)
                |> List.map (fun d -> d.id, d.title)
            let html = Views.Elements.elementNewFormPartial webConfig layerValue typeValue elementOptions
            htmlView html next ctx

    /// Relation type options handler (HTMX)
    let relationTypeOptionsHandler (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let normalizeConceptName (value: string) : string =
                let parts = value.Split([| '-'; ' ' |], StringSplitOptions.RemoveEmptyEntries)
                parts
                |> Array.map (fun part ->
                    if part.Length = 0 then ""
                    elif part.Length = 1 then part.ToUpperInvariant()
                    else part.Substring(0, 1).ToUpperInvariant() + part.Substring(1)
                )
                |> String.Concat

            let getValue (key: string) : string =
                getQueryStringValueOrEmpty ctx key

            let index =
                tryGetQueryStringValueInt ctx "index" |> Option.defaultValue 0

            let sourceId = getValue "sourceId"
            let sourceTypeOverride = getValue "sourceType"
            let targetFromField = getValue $"rel-target-{index}"
            let targetId =
                if not (String.IsNullOrWhiteSpace targetFromField) then
                    targetFromField
                else
                    let direct = getValue "targetId"
                    let isMissing =
                        String.IsNullOrWhiteSpace direct
                        || direct.Equals("undefined", StringComparison.OrdinalIgnoreCase)
                        || direct.Equals("null", StringComparison.OrdinalIgnoreCase)
                    if isMissing then "" else direct

            let currentValue = getValue "current"
            logger.LogDebug(
                "relationTypeOptionsHandler: sourceId='{sourceId}', targetId='{targetId}', currentValue='{currentValue}', index={index}",
                sourceId,
                targetId,
                currentValue,
                index
            )
            let repo = repoService.Repository
            let tryGetConceptName (doc: DocumentRecord) : string =
                match doc with
                | ArchitectureDoc _ ->
                    doc
                    |> getArchimateElementType
                    |> ElementType.elementTypeToConceptName
                | GovernanceDoc _ ->
                    getGovernanceDocType doc |> GovernanceDocType.toConceptName

            let allowedTypes =
                match Map.tryFind sourceId repo.documents, Map.tryFind targetId repo.documents with
                | Some sourceDoc, Some targetDoc ->
                    let rules = repoService.RelationshipRules
                    let sourceConcept = tryGetConceptName sourceDoc
                    let targetConcept = tryGetConceptName targetDoc
                    logger.LogInformation("Relation lookup: sourceConcept={sourceConcept}, targetConcept={targetConcept}", sourceConcept, targetConcept)
                    match Map.tryFind (sourceConcept, targetConcept) rules with
                    | Some codes ->
                        logger.LogInformation("Relation rules hit: {allowedCount} allowed codes", Set.count codes)
                        codes
                        |> Set.toList
                        |> List.choose relationCodeToName
                    | None -> []
                | None, Some targetDoc ->
                    if String.IsNullOrWhiteSpace sourceTypeOverride then
                        logger.LogWarning("Relation lookup failed: sourceId '{sourceId}' not found", sourceId)
                        []
                    else
                        let rules = repoService.RelationshipRules
                        let sourceConcept = normalizeConceptName sourceTypeOverride
                        let targetConcept = tryGetConceptName targetDoc
                        logger.LogInformation("Relation lookup (override): sourceConcept={sourceConcept}, targetConcept={targetConcept}", sourceConcept, targetConcept)
                        match Map.tryFind (sourceConcept, targetConcept) rules with
                        | Some codes ->
                            logger.LogInformation("Relation rules hit (override): {allowedCount} allowed codes", Set.count codes)
                            codes
                            |> Set.toList
                            |> List.choose relationCodeToName
                        | None -> []
                | _, None ->
                    logger.LogWarning("Relation lookup failed: targetId '{targetId}' not found", targetId)
                    []

            logger.LogInformation("GET /elements/relations/types - source={sourceId}, target={targetId}", sourceId, targetId)
            let selectNode = Views.Relations.relationTypeSelectPartial index allowedTypes currentValue
            htmlView selectNode next ctx

    /// Relation row partial handler (HTMX)
    let relationRowHandler (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let index = tryGetQueryStringValueInt ctx "index" |> Option.defaultValue 0
            let sourceId = getQueryStringValueOrEmpty ctx "sourceId"

            logger.LogInformation("GET /elements/relations/row - index={index}", index)
            let row = Views.Relations.relationRowPartial webConfig sourceId index "" "" ""
            htmlView row next ctx

    /// Element type options handler (HTMX)
    let elementTypeOptionsHandler (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let layerValue = getQueryStringValueOrEmpty ctx "layer"
            let currentValue = getQueryStringValueOrEmpty ctx "type"

            logger.LogInformation("GET /elements/types - layer={layer}", layerValue)
            let selectNode = Views.Relations.elementTypeSelectPartial layerValue currentValue
            htmlView selectNode next ctx

    /// Element download handler
    let elementDownloadHandler (elemId: string) (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            task {
                logger.LogInformation("POST /elements/{elementId}/download - Element download requested", elemId)
                let repo = repoService.Repository
                match tryGetDocumentById repo elemId with
                | Some doc when isArchitecture doc ->
                    let! form = ctx.Request.ReadFormAsync()

                    let getRaw (key: string) : string =
                        if form.ContainsKey key then form.[key].ToString() else ""

                    let getTrimmed (key: string) : string =
                        getRaw key |> fun value -> value.Trim()

                    let getOrDefault (value: string) (fallback: string) : string =
                        if String.IsNullOrWhiteSpace value then fallback else value

                    let defaultType = getArchimateTypeValue doc
                    let defaultLayer = getArchimateLayerValue doc

                    let name = getOrDefault (getTrimmed "name") doc.title
                    let elementType = getOrDefault (getTrimmed "type") defaultType
                    let layer = getOrDefault (getTrimmed "layer") defaultLayer
                    let content = getRaw "content"

                    let rawId = getTrimmed "id"
                    let id =
                        if String.IsNullOrWhiteSpace rawId then
                            let existingIds = getArchimateDocuments repo |> List.map (fun d -> d.id)
                            generateElementId existingIds layer elementType name
                        else
                            rawId

                    let tags =
                        getTrimmed "tags"
                        |> fun value -> value.Split([| ',' |], StringSplitOptions.RemoveEmptyEntries)
                        |> Array.map (fun t -> t.Trim())
                        |> Array.filter (fun t -> t <> "")
                        |> Array.toList

                    let properties =
                        [
                            ("owner", getTrimmed "owner")
                            ("status", getTrimmed "status")
                            ("criticality", getTrimmed "criticality")
                            ("version", getTrimmed "version")
                            ("lifecycle-phase", getTrimmed "lifecycle-phase")
                            ("last-updated", getTrimmed "last-updated")
                        ]
                        |> List.choose (fun (key, value) ->
                            if String.IsNullOrWhiteSpace value then None else Some (key, value)
                        )

                    let relationTargets =
                        form.Keys
                        |> Seq.choose (fun key ->
                            if key.StartsWith("rel-target-") then
                                let indexPart = key.Substring("rel-target-".Length)
                                match Int32.TryParse(indexPart) with
                                | true, index -> Some (index, form.[key].ToString())
                                | _ -> None
                            else
                                None
                        )
                        |> Seq.sortBy fst
                        |> Seq.toList

                    let relationships =
                        relationTargets
                        |> List.choose (fun (index, target) ->
                            let targetValue = target.Trim()
                            let relType = getTrimmed $"rel-type-{index}"
                            let desc = getTrimmed $"rel-desc-{index}"
                            if String.IsNullOrWhiteSpace targetValue || String.IsNullOrWhiteSpace relType then
                                None
                            else
                                Some (relType, targetValue, desc)
                        )

                    let frontmatter = buildFrontmatter id name elementType layer relationships properties tags
                    let markdown = String.Join("\n", [| frontmatter; ""; content.TrimEnd() |])
                    let fileName = $"{id}.md"

                    return!
                        (setHttpHeader "Content-Disposition" $"attachment; filename={fileName}"
                         >=> setHttpHeader "Content-Type" "text/markdown; charset=utf-8"
                         >=> text markdown)
                            next
                            ctx
                | _ ->
                    return! (respondNotFound logger "Element not found: {elementId}" elemId "Element not found") next ctx
            }

    /// New element download handler
    let elementNewDownloadHandler (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            task {
                logger.LogInformation("POST /elements/new/download - New element download requested")
                let repo = repoService.Repository
                let! form = ctx.Request.ReadFormAsync()

                let getRaw (key: string) : string =
                    if form.ContainsKey key then form.[key].ToString() else ""

                let getTrimmed (key: string) : string =
                    getRaw key |> fun value -> value.Trim()

                let name = getTrimmed "name"
                let elementType = getTrimmed "type"
                let layer = getTrimmed "layer"
                let content = getRaw "content"

                let rawId = getTrimmed "id"
                let id =
                    if String.IsNullOrWhiteSpace rawId then
                        let existingIds = getArchimateDocuments repo |> List.map (fun d -> d.id)
                        generateElementId existingIds layer elementType name
                    else
                        rawId

                let tags =
                    getTrimmed "tags"
                    |> fun value -> value.Split([| ',' |], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map (fun t -> t.Trim())
                    |> Array.filter (fun t -> t <> "")
                    |> Array.toList

                let properties =
                    [
                        ("owner", getTrimmed "owner")
                        ("status", getTrimmed "status")
                        ("criticality", getTrimmed "criticality")
                        ("version", getTrimmed "version")
                        ("lifecycle-phase", getTrimmed "lifecycle-phase")
                        ("last-updated", getTrimmed "last-updated")
                    ]
                    |> List.choose (fun (key, value) ->
                        if String.IsNullOrWhiteSpace value then None else Some (key, value)
                    )

                let relationTargets =
                    form.Keys
                    |> Seq.choose (fun key ->
                        if key.StartsWith("rel-target-") then
                            let indexPart = key.Substring("rel-target-".Length)
                            match Int32.TryParse(indexPart) with
                            | true, index -> Some (index, form.[key].ToString())
                            | _ -> None
                        else
                            None
                    )
                    |> Seq.sortBy fst
                    |> Seq.toList

                let relationships =
                    relationTargets
                    |> List.choose (fun (index, target) ->
                        let targetValue = target.Trim()
                        let relType = getTrimmed $"rel-type-{index}"
                        let desc = getTrimmed $"rel-desc-{index}"
                        if String.IsNullOrWhiteSpace targetValue || String.IsNullOrWhiteSpace relType then
                            None
                        else
                            Some (relType, targetValue, desc)
                    )

                let frontmatter = buildFrontmatter id name elementType layer relationships properties tags
                let markdown = String.Join("\n", [| frontmatter; ""; content.TrimEnd() |])
                let fileName = $"{id}.md"

                return!
                    (setHttpHeader "Content-Disposition" $"attachment; filename={fileName}"
                     >=> setHttpHeader "Content-Type" "text/markdown; charset=utf-8"
                     >=> text markdown)
                        next
                        ctx
            }
    
    /// Tags index handler
    let tagsIndexHandler (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /tags - Tags index page requested")
            let repo = repoService.Repository
            let tagIndex = TagIndex.buildTagIndex (getArchimateDocuments repo)
            logger.LogInformation("Found {tagCount} tags", Map.count tagIndex)
            let html = Views.Tags.tagsIndexPage webConfig tagIndex
            htmlView html next ctx

    /// Element type Cytoscape diagram handler
    let elementTypeDiagramCytoscapeHandler (layer: string) (typeValue: string) (repoService: DocumentRepositoryService) (assets: DiagramAssetConfig) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /diagrams/type/{layer}/{type} - Cytoscape element type diagram requested", layer, typeValue)
            let normalizedLayer = layer.Trim().ToLowerInvariant()
            let normalizedType = typeValue.Trim().ToLowerInvariant()
            let validLayer = Config.layerOptions |> List.exists (fun value -> value = normalizedLayer)
            let validType = Config.getTypeOptions normalizedLayer |> List.exists (fun value -> value = normalizedType)
            if not validLayer || not validType then
                respondNotFound logger "Element type not found for Cytoscape diagram: {layerAndType}" (sprintf "%s/%s" layer typeValue) "Element type not found"
                |> fun handler -> handler next ctx
            else
                let repo = repoService.Repository
                let elementType = ElementType.parseElementType normalizedLayer normalizedType
                let layerElements =
                    getArchimateDocuments repo
                    |> List.filter (fun doc -> getArchimateElementType doc = elementType)

                let allRels =
                    layerElements
                    |> List.collect (fun doc ->
                        doc.relationships
                        |> List.map (fun rel -> (doc.id, rel))
                    )

                let relatedIds = allRels |> List.map (fun (_, rel) -> rel.target) |> Set.ofList
                let relatedElements =
                    relatedIds
                    |> Set.toList
                    |> List.choose (fun id ->
                        match Map.tryFind id repo.documents with
                        | Some doc when isArchitecture doc ->
                            if layerElements |> List.exists (fun le -> le.id = id) then None else Some doc
                        | _ -> None
                    )

                let allElements = layerElements @ relatedElements
                let elementIds = allElements |> List.map (fun e -> e.id) |> Set.ofList
                let governanceDocs = selectGovernanceDocs repo elementIds

                let data = buildCytoscapeDiagram assets allElements allRels governanceDocs
                let label = elementTypeAndSubTypeToString elementType
                let view = Views.Diagrams.cytoscapeDiagramPage webConfig label data
                htmlView view next ctx
    
    /// Element context Cytoscape diagram handler
    let contextDiagramCytoscapeHandler (elemId: string) (repoService: DocumentRepositoryService) (assets: DiagramAssetConfig) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /diagrams/context/{elementId}/cytoscape - Cytoscape context diagram requested", elemId)
            let repo = repoService.Repository
            match tryGetDocumentById repo elemId with
            | Some elem when isArchitecture elem ->
                let depth =
                    match tryGetQueryStringValueInt ctx "depth" with
                    | Some value when value > 0 && value <= 3 -> value
                    | _ -> 1
                
                logger.LogInformation(
                    "Found element: {elementId} ({elementName}), generating Cytoscape context diagram with depth={depth}",
                    elemId,
                    elem.title,
                    depth
                )
                let rec collectNeighbors (currentIds: Set<string>) (currentDepth: int) : Set<string> =
                    if currentDepth >= depth then currentIds
                    else
                        let nextIds =
                            currentIds
                            |> Set.toList
                            |> List.collect (fun id ->
                                match Map.tryFind id repo.documents with
                                | Some doc when isArchitecture doc ->
                                    let outgoing = doc.relationships |> List.map (fun r -> r.target)
                                    let incoming =
                                        repo.relations
                                        |> List.choose (fun rel -> if rel.targetId = id then Some rel.sourceId else None)
                                    outgoing @ incoming
                                | _ -> [])
                            |> Set.ofList

                        let combined = Set.union currentIds nextIds
                        collectNeighbors combined (currentDepth + 1)

                let allNodeIds = collectNeighbors (Set.singleton elemId) 0

                let elements =
                    allNodeIds
                    |> Set.toList
                    |> List.choose (fun id ->
                        match Map.tryFind id repo.documents with
                        | Some doc when isArchitecture doc -> Some doc
                        | _ -> None)

                let rels =
                    elements
                    |> List.collect (fun elem ->
                        elem.relationships
                        |> List.filter (fun rel -> Set.contains rel.target allNodeIds)
                        |> List.map (fun rel -> (elem.id, rel)))

                let elementIds =
                    elements
                    |> List.map (fun e -> e.id)
                    |> Set.ofList

                let governanceDocs = selectGovernanceDocs repo elementIds

                let data = buildCytoscapeDiagram assets elements rels governanceDocs
                let title = sprintf "Context: %s (Depth %d)" elem.title depth
                let view = Views.Diagrams.cytoscapeDiagramPage webConfig title data
                htmlView view next ctx
            | Some _ ->
                respondNotFound logger "Element not found for Cytoscape context diagram: {elementId}" elemId "Element not found"
                |> fun handler -> handler next ctx
            | None ->
                respondNotFound logger "Element not found for Cytoscape context diagram: {elementId}" elemId "Element not found"
                |> fun handler -> handler next ctx

    /// Governance document Cytoscape diagram handler
    let governanceDiagramCytoscapeHandler (slug: string) (repoService: DocumentRepositoryService) (assets: DiagramAssetConfig) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /diagrams/governance/{slug} - Cytoscape governance diagram requested", slug)
            let repo = repoService.Repository
            match tryGetGovernanceBySlug repo slug with
            | Some doc ->
                let ownerId =
                    match doc.owner with
                    | Some value when not (String.IsNullOrWhiteSpace value) -> Some (value.Trim())
                    | _ -> None

                let relationTargets =
                    doc.relationships
                    |> List.map (fun rel -> rel.target.Trim())
                    |> List.filter (fun value -> not (String.IsNullOrWhiteSpace value))

                let elementIds =
                    match ownerId with
                    | Some owner -> owner :: relationTargets
                    | None -> relationTargets
                    |> Set.ofList

                let governanceDocs =
                    doc :: selectGovernanceDocs repo elementIds
                    |> List.distinctBy (fun d -> d.slug)

                let relatedElementIds =
                    governanceDocs
                    |> List.collect (fun governanceDoc ->
                        let ownerId =
                            match governanceDoc.owner with
                            | Some value when not (String.IsNullOrWhiteSpace value) -> [ value.Trim() ]
                            | _ -> []

                        let relationTargets =
                            governanceDoc.relationships
                            |> List.map (fun rel -> rel.target.Trim())
                            |> List.filter (fun value -> not (String.IsNullOrWhiteSpace value))

                        ownerId @ relationTargets
                    )
                    |> Set.ofList

                let allElementIds = Set.union elementIds relatedElementIds

                let elements =
                    allElementIds
                    |> Set.toList
                    |> List.choose (fun id ->
                        match Map.tryFind id repo.documents with
                        | Some elem when isArchitecture elem -> Some elem
                        | _ -> None)

                let validElementIds =
                    elements
                    |> List.map (fun elem -> elem.id)
                    |> Set.ofList

                let rels =
                    elements
                    |> List.collect (fun elem ->
                        elem.relationships
                        |> List.filter (fun rel -> Set.contains rel.target validElementIds)
                        |> List.map (fun rel -> (elem.id, rel)))

                let data = buildCytoscapeDiagram assets elements rels governanceDocs
                let title = sprintf "Governance: %s" doc.title
                let view = Views.Diagrams.cytoscapeDiagramPage webConfig title data
                htmlView view next ctx
            | None ->
                respondNotFound logger "Governance document not found for Cytoscape diagram: {slug}" slug "Governance document not found"
                |> fun handler -> handler next ctx

    /// Diagram expansion API handler - returns related nodes and edges
    let diagramExpandHandler (elementId: string) (repoService: DocumentRepositoryService) (assets: DiagramAssetConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/diagrams/expand/{elementId} - Diagram expansion requested", elementId)
            let repo = repoService.Repository

            let respondWithGraph (elements: DocumentRecord list) (rels: (string * Relationship) list) (governanceDocs: DocumentRecord list) =
                let data = buildCytoscapeDiagram assets elements rels governanceDocs
                (setHttpHeader "Content-Type" "application/json" >=> text data) next ctx

            if elementId.StartsWith("gov-", StringComparison.OrdinalIgnoreCase) then
                let slug = elementId.Substring(4)
                match tryGetGovernanceBySlug repo slug with
                | Some doc ->
                    let ownerId =
                        match doc.owner with
                        | Some value when not (String.IsNullOrWhiteSpace value) -> Some (value.Trim())
                        | _ -> None

                    let relationTargets =
                        doc.relationships
                        |> List.map (fun rel -> rel.target.Trim())
                        |> List.filter (fun value -> not (String.IsNullOrWhiteSpace value))

                    let elementIds =
                        match ownerId with
                        | Some owner -> owner :: relationTargets
                        | None -> relationTargets
                        |> Set.ofList

                    let governanceDocs =
                        doc :: selectGovernanceDocs repo elementIds
                        |> List.distinctBy (fun d -> d.slug)

                    let relatedElementIds =
                        governanceDocs
                        |> List.collect (fun governanceDoc ->
                            let ownerId =
                                match governanceDoc.owner with
                                | Some value when not (String.IsNullOrWhiteSpace value) -> [ value.Trim() ]
                                | _ -> []

                            let relationTargets =
                                governanceDoc.relationships
                                |> List.map (fun rel -> rel.target.Trim())
                                |> List.filter (fun value -> not (String.IsNullOrWhiteSpace value))

                            ownerId @ relationTargets
                        )
                        |> Set.ofList

                    let allElementIds = Set.union elementIds relatedElementIds

                    let elements =
                        allElementIds
                        |> Set.toList
                        |> List.choose (fun id ->
                            match Map.tryFind id repo.documents with
                            | Some elem when isArchitecture elem -> Some elem
                            | _ -> None)

                    let validElementIds =
                        elements
                        |> List.map (fun elem -> elem.id)
                        |> Set.ofList

                    let rels =
                        elements
                        |> List.collect (fun elem ->
                            elem.relationships
                            |> List.filter (fun rel -> Set.contains rel.target validElementIds)
                            |> List.map (fun rel -> (elem.id, rel)))

                    respondWithGraph elements rels governanceDocs
                | None ->
                    respondNotFound logger "Governance document not found for diagram expansion: {slug}" slug "Governance document not found"
                    |> fun handler -> handler next ctx
            else
                match tryGetDocumentById repo elementId with
                | Some elem when isArchitecture elem ->
                    let outgoing = elem.relationships |> List.map (fun rel -> rel.target)
                    let incoming =
                        repo.relations
                        |> List.choose (fun rel -> if rel.targetId = elementId then Some rel.sourceId else None)

                    let relatedIds =
                        (elementId :: (outgoing @ incoming))
                        |> List.filter (fun value -> not (String.IsNullOrWhiteSpace value))
                        |> Set.ofList

                    let elements =
                        relatedIds
                        |> Set.toList
                        |> List.choose (fun id ->
                            match Map.tryFind id repo.documents with
                            | Some doc when isArchitecture doc -> Some doc
                            | _ -> None)

                    let elementIds =
                        elements
                        |> List.map (fun doc -> doc.id)
                        |> Set.ofList

                    let rels =
                        elements
                        |> List.collect (fun doc ->
                            doc.relationships
                            |> List.filter (fun rel -> Set.contains rel.target elementIds)
                            |> List.map (fun rel -> (doc.id, rel)))

                    let governanceDocs = selectGovernanceDocs repo elementIds

                    respondWithGraph elements rels governanceDocs
                | Some _ ->
                    respondNotFound logger "Element not found for diagram expansion: {elementId}" elementId "Element not found"
                    |> fun handler -> handler next ctx
                | None ->
                    respondNotFound logger "Element not found for diagram expansion: {elementId}" elementId "Element not found"
                    |> fun handler -> handler next ctx
    
    /// Validation errors API handler - list all errors
    let validationErrorsHandler (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/errors - Validation errors requested")
            let errors = repoService.Repository.validationErrors
            logger.LogInformation("Returning {errorCount} validation errors", List.length errors)
            
            let errorsList =
                errors
                |> List.map (fun err ->
                    dict [
                        ("filePath", box err.filePath)
                        ("elementId", box (err.elementId |> Option.defaultValue ""))
                        ("errorType", box (ElementType.errorTypeToString err.errorType))
                        ("message", box err.message)
                        ("severity", box (ElementType.severityToString err.severity))
                    ]
                )
            
            json errorsList next ctx
    
    /// Validation errors by file handler
    let fileValidationErrorsHandler (filePath: string) (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/file - Validation errors for file: {filePath}", filePath)
            let decodedPath = Uri.UnescapeDataString(filePath)
            let errors =
                repoService.Repository.validationErrors
                |> List.filter (fun err -> err.filePath = decodedPath)
            logger.LogInformation("File '{filePath}' has {errorCount} validation errors", decodedPath, List.length errors)
            
            let errorsList =
                errors
                |> List.map (fun err ->
                    dict [
                        ("filePath", box err.filePath)
                        ("elementId", box (err.elementId |> Option.defaultValue ""))
                        ("errorType", box (ElementType.errorTypeToString err.errorType))
                        ("message", box err.message)
                        ("severity", box (ElementType.severityToString err.severity))
                    ]
                )
            
            json errorsList next ctx
    
    /// Validation statistics handler
    let validationStatsHandler (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/stats - Validation statistics requested")
            let errors = repoService.Repository.validationErrors
            let errors_list = errors |> List.filter (fun e -> e.severity = Severity.Error)
            let warnings_list = errors |> List.filter (fun e -> e.severity = Severity.Warning)
            
            let stats = dict [
                ("totalFiles", box (errors |> List.map (fun e -> e.filePath) |> List.distinct |> List.length))
                ("totalErrors", box errors_list.Length)
                ("totalWarnings", box warnings_list.Length)
                ("errorsByType", box (
                    errors
                    |> List.groupBy (fun e -> e.errorType)
                    |> List.map (fun (eType, errs) -> dict [("type", box (ElementType.errorTypeToString eType)); ("count", box errs.Length)])
                ))
            ]
            
            json stats next ctx
    
    /// Validation page handler
    let validationPageHandler (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /validation - Validation page requested")
            let errors = repoService.Repository.validationErrors
            logger.LogInformation("Displaying {errorCount} validation errors", List.length errors)
            let html = Views.Validation.validationPage webConfig repoService.BasePaths errors
            htmlView html next ctx
    
    /// Revalidate file handler
    let revalidateFileHandler (filePath: string) (repoService: DocumentRepositoryService) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("POST /api/validation/revalidate - Revalidating file: {filePath}", filePath)
            let decodedPath = Uri.UnescapeDataString(filePath)
            let basePath =
                repoService.ElementsPath
                |> Path.GetFullPath
                |> fun path -> path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)

            let requestedPath =
                if Path.IsPathRooted decodedPath then
                    Path.GetFullPath decodedPath
                else
                    Path.GetFullPath(Path.Combine(basePath, decodedPath))

            let isWithinBase =
                requestedPath.StartsWith(basePath + string Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                || requestedPath.Equals(basePath, StringComparison.OrdinalIgnoreCase)

            if not isWithinBase then
                logger.LogWarning("Rejected revalidate path outside elements root: {requestedPath}", requestedPath)
                (setStatusCode 400 >=> text "Invalid file path") next ctx
            else
                repoService.Reload()
                let errors =
                    repoService.Repository.validationErrors
                    |> List.filter (fun err -> err.filePath = requestedPath)

                let result = dict [
                    ("filePath", box requestedPath)
                    ("revalidated", box true)
                    ("errorCount", box errors.Length)
                    ("errors", box (
                        errors
                        |> List.map (fun err ->
                            dict [
                                ("errorType", box (ElementType.errorTypeToString err.errorType))
                                ("message", box err.message)
                                ("severity", box (ElementType.severityToString err.severity))
                            ]
                        )
                    ))
                ]

                json result next ctx
    
    /// Individual tag page handler
    let tagHandler (tag: string) (repoService: DocumentRepositoryService) (webConfig: WebUiConfig) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /tags/{tag} - Tag page requested", tag)
            let repo = repoService.Repository
            let archimateDocs = getArchimateDocuments repo
            let tagIndex = TagIndex.buildTagIndex archimateDocs
            match Map.tryFind tag tagIndex with
            | Some elemIds ->
                logger.LogInformation("Found {elementCount} elements with tag '{tag}'", List.length elemIds, tag)
                let elements =
                    elemIds
                    |> List.choose (fun id -> tryGetDocumentById repo id)
                    |> List.filter isArchitecture
                    |> List.map (createArchimateCard repo)
                    |> List.sortBy (fun e -> e.name)
                let html = Views.Tags.tagPage webConfig tag elements
                htmlView html next ctx
            | None ->
                respondNotFound logger "Tag not found: {tag}" tag "Tag not found"
                |> fun handler -> handler next ctx
    
