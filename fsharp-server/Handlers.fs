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
    open HandlersHelpers
    
    /// Index/home page handler
    let indexHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET / - Home page requested")
            let layerCounts = 
                registry.elementsByLayer
                |> Map.map (fun _ ids -> List.length ids)
            logger.LogDebug("Layer summary: {layerSummary}", layerCounts)
            let html = Views.indexPage registry
            htmlView html next ctx
    
    /// Layer page handler
    let layerHandler (layer: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /{layer} - Layer page requested", layer)
            match Layer.tryParse layer with
            | Some layerValue ->
                match Map.tryFind layerValue Config.layerOrder with
                | Some layerInfo ->
                    let elements = ElementRegistry.getLayerElements layerValue registry
                    let filterValue =
                        match ctx.GetQueryStringValue "filter" with
                        | Ok value when not (String.IsNullOrWhiteSpace value) -> Some value
                        | _ -> None

                    let subtypeValue =
                        match ctx.GetQueryStringValue "subtype" with
                        | Ok value when not (String.IsNullOrWhiteSpace value) -> Some value
                        | _ -> None

                    let subtypeOptions =
                        elements
                        |> List.choose (fun elem -> ElementRegistry.getString "type" elem.properties)
                        |> List.distinct
                        |> List.sort

                    let filteredElements =
                        elements
                        |> List.filter (fun elem ->
                            let nameMatches =
                                match filterValue with
                                | Some term -> elem.name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                                | None -> true

                            let subtypeMatches =
                                match subtypeValue with
                                | Some subtype ->
                                    match ElementRegistry.getString "type" elem.properties with
                                    | Some value -> value.Equals(subtype, StringComparison.OrdinalIgnoreCase)
                                    | None -> false
                                | None -> true

                            nameMatches && subtypeMatches
                        )

                    logger.LogInformation("Found {elementCount} elements in layer {layer}", List.length elements, layer)
                    elements |> List.iter (fun elem ->
                        logger.LogDebug("  - {elementId}: {elementName}", elem.id, elem.name)
                    )
                    let isHxRequest = ctx.Request.Headers.ContainsKey "HX-Request"
                    if isHxRequest then
                        let partial = Views.layerElementsPartial filteredElements registry
                        htmlView partial next ctx
                    else
                        let layerKey = Layer.toKey layerValue
                        let html = Views.layerPage layerKey layerInfo filteredElements registry filterValue subtypeOptions subtypeValue
                        htmlView html next ctx
                | None -> 
                    logger.LogWarning("Layer not found: {layer}", layer)
                    setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
            | None ->
                logger.LogWarning("Layer not found: {layer}", layer)
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element detail page handler
    let elementHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /elements/{elementId} - Element detail requested", elemId)
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                logger.LogInformation("Found element: {elementId} ({elementName})", elemId, elem.name)
                let incoming = ElementRegistry.getIncomingRelations elemId registry
                let outgoing = elem.relationships
                logger.LogInformation("  Incoming relations: {incomingCount}, Outgoing relations: {outgoingCount}", List.length incoming, List.length outgoing)
                
                if List.length outgoing > 0 then
                    logger.LogInformation("  Outgoing targets:")
                    outgoing |> List.iter (fun rel ->
                        logger.LogInformation("    - {targetId}", rel.target)
                    )
                
                let elemWithRels = ElementRegistry.withRelations elem registry
                logger.LogInformation(
                    "  After withRelations: incoming={incomingCount}, outgoing={outgoingCount}",
                    List.length elemWithRels.incomingRelations,
                    List.length elemWithRels.outgoingRelations
                )
                
                let html = Views.elementPage elemWithRels
                htmlView html next ctx
            | None ->
                logger.LogWarning("Element not found: {elementId}", elemId)
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx

    /// Element edit form partial handler
    let elementEditHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /elements/{elementId}/edit - Element edit form requested", elemId)
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                let html = Views.elementEditFormPartial elem registry
                htmlView html next ctx
            | None ->
                logger.LogWarning("Element not found: {elementId}", elemId)
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx

    /// New element form handler
    let elementNewHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let layerValue =
                match ctx.GetQueryStringValue "layer" with
                | Ok value -> value
                | Error _ -> ""
            logger.LogInformation("GET /elements/new - layer={layer}", layerValue)
            let html = Views.elementNewFormPartial layerValue registry
            htmlView html next ctx

    /// Relation type options handler (HTMX)
    let relationTypeOptionsHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
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

            let tryGetConceptName (elem: Element) : string =
                match ElementRegistry.getString "type" elem.properties with
                | Some typeValue when not (String.IsNullOrWhiteSpace typeValue) ->
                    normalizeConceptName typeValue
                | _ ->
                    ElementType.elementTypeToConceptName elem.elementType

            let getValue (key: string) : string =
                match ctx.GetQueryStringValue key with
                | Ok value -> value
                | Error _ -> ""

            let index =
                match Int32.TryParse(getValue "index") with
                | true, value -> value
                | _ -> 0

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
            let allowedTypes =
                match Map.tryFind sourceId registry.elements, Map.tryFind targetId registry.elements with
                | Some sourceElem, Some targetElem ->
                    let rules = registry.relationshipRules
                    let sourceConcept = tryGetConceptName sourceElem
                    let targetConcept = tryGetConceptName targetElem
                    logger.LogInformation("Relation lookup: sourceConcept={sourceConcept}, targetConcept={targetConcept}", sourceConcept, targetConcept)
                    match Map.tryFind (sourceConcept, targetConcept) rules with
                    | Some codes ->
                        logger.LogInformation("Relation rules hit: {allowedCount} allowed codes", Set.count codes)
                        codes
                        |> Set.toList
                        |> List.choose relationCodeToName
                    | None -> []
                | None, Some targetElem ->
                    if String.IsNullOrWhiteSpace sourceTypeOverride then
                        logger.LogWarning("Relation lookup failed: sourceId '{sourceId}' not found", sourceId)
                        []
                    else
                        let rules = registry.relationshipRules
                        let sourceConcept = normalizeConceptName sourceTypeOverride
                        let targetConcept = tryGetConceptName targetElem
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
            let selectNode = Views.relationTypeSelectPartial index allowedTypes currentValue
            htmlView selectNode next ctx

    /// Relation row partial handler (HTMX)
    let relationRowHandler (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let index =
                match ctx.GetQueryStringValue "index" with
                | Ok value ->
                    match Int32.TryParse(value) with
                    | true, parsed -> parsed
                    | _ -> 0
                | Error _ -> 0

            let sourceId =
                match ctx.GetQueryStringValue "sourceId" with
                | Ok value -> value
                | Error _ -> ""

            logger.LogInformation("GET /elements/relations/row - index={index}", index)
            let row = Views.relationRowPartial sourceId index "" "" ""
            htmlView row next ctx

    /// Element type options handler (HTMX)
    let elementTypeOptionsHandler (logger: ILogger) : HttpHandler =
        fun next ctx ->
            let layerValue =
                match ctx.GetQueryStringValue "layer" with
                | Ok value -> value
                | Error _ -> ""

            let currentValue =
                match ctx.GetQueryStringValue "type" with
                | Ok value -> value
                | Error _ -> ""

            logger.LogInformation("GET /elements/types - layer={layer}", layerValue)
            let selectNode = Views.elementTypeSelectPartial layerValue currentValue
            htmlView selectNode next ctx

    /// Element download handler
    let elementDownloadHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            task {
                logger.LogInformation("POST /elements/{elementId}/download - Element download requested", elemId)
                match ElementRegistry.getElement elemId registry with
                | None ->
                    logger.LogWarning("Element not found: {elementId}", elemId)
                    return! (setStatusCode 404 >=> text "Element not found") next ctx
                | Some elem ->
                    let! form = ctx.Request.ReadFormAsync()

                    let getRaw (key: string) : string =
                        if form.ContainsKey key then form.[key].ToString() else ""

                    let getTrimmed (key: string) : string =
                        getRaw key |> fun value -> value.Trim()

                    let getOrDefault (value: string) (fallback: string) : string =
                        if String.IsNullOrWhiteSpace value then fallback else value

                    let defaultType = ElementRegistry.getString "type" elem.properties |> Option.defaultValue ""
                    let defaultLayer = ElementRegistry.getString "layer" elem.properties |> Option.defaultValue ""

                    let name = getOrDefault (getTrimmed "name") elem.name
                    let elementType = getOrDefault (getTrimmed "type") defaultType
                    let layer = getOrDefault (getTrimmed "layer") defaultLayer
                    let content = getRaw "content"

                    let rawId = getTrimmed "id"
                    let id =
                        if String.IsNullOrWhiteSpace rawId then
                            generateElementId registry layer elementType name
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

    /// New element download handler
    let elementNewDownloadHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            task {
                logger.LogInformation("POST /elements/new/download - New element download requested")
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
                        generateElementId registry layer elementType name
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
    let tagsIndexHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /tags - Tags index page requested")
            let tagIndex = buildTagIndex registry
            logger.LogInformation("Found {tagCount} tags", Map.count tagIndex)
            let html = Views.tagsIndexPage tagIndex registry
            htmlView html next ctx

    /// Layer Cytoscape diagram handler
    let layerDiagramCytoscapeHandler (layer: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /diagrams/layer/{layer} - Cytoscape layer diagram requested", layer)
            match Layer.tryParse layer with
            | Some layerValue ->
                match Map.tryFind layerValue Config.layerOrder with
                | Some layerInfo ->
                    let data = buildLayerCytoscape layerValue registry
                    let html = wrapCytoscapeHtml (sprintf "%s Layer" layerInfo.displayName) data true
                    htmlString html next ctx
                | None ->
                    logger.LogWarning("Layer not found for Cytoscape diagram: {layer}", layer)
                    setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
            | None ->
                logger.LogWarning("Layer not found for Cytoscape diagram: {layer}", layer)
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element context Cytoscape diagram handler
    let contextDiagramCytoscapeHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /diagrams/context/{elementId}/cytoscape - Cytoscape context diagram requested", elemId)
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                let depth = 
                    match ctx.GetQueryStringValue "depth" with
                    | Ok value -> 
                        match System.Int32.TryParse(value) with
                        | (true, d) when d > 0 && d <= 3 -> d
                        | _ -> 1
                    | Error _ -> 1
                
                logger.LogInformation(
                    "Found element: {elementId} ({elementName}), generating Cytoscape context diagram with depth={depth}",
                    elemId,
                    elem.name,
                    depth
                )
                let data = buildContextCytoscape elemId depth registry
                let title = sprintf "Context: %s (Depth %d)" elem.name depth
                let html = wrapCytoscapeHtml title data false
                htmlString html next ctx
            | None ->
                logger.LogWarning("Element not found for Cytoscape context diagram: {elementId}", elemId)
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx
    
    /// Validation errors API handler - list all errors
    let validationErrorsHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/errors - Validation errors requested")
            let errors = ElementRegistry.getValidationErrors registry
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
    let fileValidationErrorsHandler (filePath: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/file - Validation errors for file: {filePath}", filePath)
            let decodedPath = Uri.UnescapeDataString(filePath)
            let errors = ElementRegistry.getFileValidationErrors decodedPath registry
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
    let validationStatsHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/stats - Validation statistics requested")
            let errors = ElementRegistry.getValidationErrors registry
            let errors_list = ElementRegistry.getErrorsBySeverity Severity.Error registry
            let warnings_list = ElementRegistry.getErrorsBySeverity Severity.Warning registry
            
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
    let validationPageHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /validation - Validation page requested")
            let errors = ElementRegistry.getValidationErrors registry
            logger.LogInformation("Displaying {errorCount} validation errors", List.length errors)
            let html = Views.validationPage errors
            htmlView html next ctx
    
    /// Revalidate file handler
    let revalidateFileHandler (filePath: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("POST /api/validation/revalidate - Revalidating file: {filePath}", filePath)
            let decodedPath = Uri.UnescapeDataString(filePath)
            let basePath =
                registry.elementsPath
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
                ElementRegistry.revalidateFile requestedPath registry logger
                let errors = ElementRegistry.getFileValidationErrors requestedPath registry

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
    let tagHandler (tag: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /tags/{tag} - Tag page requested", tag)
            let tagIndex = buildTagIndex registry
            match Map.tryFind tag tagIndex with
            | Some elemIds ->
                logger.LogInformation("Found {elementCount} elements with tag '{tag}'", List.length elemIds, tag)
                let elements =
                    elemIds
                    |> List.choose (fun id -> ElementRegistry.getElement id registry)
                    |> List.sortBy (fun e -> e.name)
                let html = Views.tagPage tag elements
                htmlView html next ctx
            | None ->
                logger.LogWarning("Tag not found: {tag}", tag)
                setStatusCode 404 >=> text "Tag not found" |> fun handler -> handler next ctx
    
    /// Create route handlers
    let createHandlers (registry: ElementRegistry) (loggerFactory: ILoggerFactory) : HttpHandler =
        let logger = loggerFactory.CreateLogger("Handlers")
        
        logger.LogInformation("Initializing route handlers")
        logger.LogInformation("Registry contains {elementCount} elements", Map.count registry.elements)
        
        choose [
            route "/" >=> indexHandler registry logger
            route "/index.html" >=> indexHandler registry logger
            route "/elements/types" >=> elementTypeOptionsHandler logger
            route "/elements/new" >=> elementNewHandler registry logger
            route "/elements/new/download" >=> elementNewDownloadHandler registry logger
            route "/elements/relations/types" >=> relationTypeOptionsHandler registry logger
            route "/elements/relations/row" >=> relationRowHandler logger
            routef "/elements/%s/edit" (fun elemId -> elementEditHandler elemId registry logger)
            routef "/elements/%s/download" (fun elemId -> elementDownloadHandler elemId registry logger)
            routef "/elements/%s" (fun elemId -> elementHandler elemId registry logger)
            
            // Diagram routes
            routef "/diagrams/layer/%s" (fun layer -> layerDiagramCytoscapeHandler layer registry logger)
            routef "/diagrams/context/%s" (fun elemId -> contextDiagramCytoscapeHandler elemId registry logger)
            
            // Validation page and API endpoints
            route "/validation" >=> validationPageHandler registry logger
            route "/api/validation/errors" >=> validationErrorsHandler registry logger
            routef "/api/validation/file/%s" (fun filePath -> fileValidationErrorsHandler filePath registry logger)
            route "/api/validation/stats" >=> validationStatsHandler registry logger
            routef "/api/validation/revalidate/%s" (fun filePath -> revalidateFileHandler filePath registry logger)
            
            route "/tags" >=> tagsIndexHandler registry logger
            routef "/tags/%s" (fun tag -> tagHandler (Uri.UnescapeDataString tag) registry logger)
            routef "/%s" (fun layer -> layerHandler layer registry logger)
        ]
