namespace EAArchive

open System
open System.Collections.Generic
open System.Globalization
open System.IO
open System.Net
open System.Xml.Linq
open Microsoft.Extensions.Logging
open Giraffe
open Giraffe.ViewEngine

module Handlers =

    open EAArchive.DiagramGenerators

    let private severityToString (severity: Severity) : string =
        match severity with
        | Severity.Error -> "error"
        | Severity.Warning -> "warning"

    let private errorTypeToString (errorType: ErrorType) : string =
        match errorType with
        | ErrorType.MissingId -> "missing-id"
        | ErrorType.InvalidType -> "invalid-type"
        | ErrorType.InvalidLayer -> "invalid-layer"
        | ErrorType.MissingRequiredField -> "missing-required-field"
        | ErrorType.InvalidRelationshipType _ -> "invalid-relationship-type"
        | ErrorType.RelationshipTargetNotFound _ -> "relationship-target-not-found"
        | ErrorType.InvalidRelationshipCombination _ -> "invalid-relationship-combination"
        | ErrorType.SelfReference _ -> "self-reference"
        | ErrorType.DuplicateRelationship _ -> "duplicate-relationship"
        | ErrorType.Unknown value -> value

    let private relationTypeToYaml (relationType: RelationType) : string =
        match relationType with
        | RelationType.Composition -> "composition"
        | RelationType.Aggregation -> "aggregation"
        | RelationType.Assignment -> "assignment"
        | RelationType.Realization -> "realization"
        | RelationType.Specialization -> "specialization"
        | RelationType.Association -> "association"
        | RelationType.Access -> "access"
        | RelationType.Influence -> "influence"
        | RelationType.Serving -> "serving"
        | RelationType.Triggering -> "triggering"
        | RelationType.Flow -> "flow"
        | RelationType.Unknown value -> value

    let private isNumber (value: string) : bool =
        let mutable number = 0.0
        Double.TryParse(value, NumberStyles.AllowLeadingSign ||| NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, &number)

    let private yamlScalar (value: string) : string =
        let trimmed = value.Trim()
        if trimmed = "" then "\"\""
        elif isNumber trimmed then trimmed
        else trimmed.Replace("\"", "\\\"") |> sprintf "\"%s\""

    let private relationCodeToName (code: char) : string option =
        match code with
        | 'c' -> Some "composition"
        | 'g' -> Some "aggregation"
        | 'i' -> Some "assignment"
        | 'r' -> Some "realization"
        | 's' -> Some "specialization"
        | 'o' -> Some "association"
        | 'a' -> Some "access"
        | 'n' -> Some "influence"
        | 'v' -> Some "serving"
        | 't' -> Some "triggering"
        | 'f' -> Some "flow"
        | _ -> None

    
    /// Build tag index from registry
    let buildTagIndex (registry: ElementRegistry) : Map<string, string list> =
        registry.elements
        |> Map.fold (fun acc elemId elem ->
            elem.tags
            |> List.fold (fun tagMap tag ->
                match Map.tryFind tag tagMap with
                | Some ids -> Map.add tag (elemId :: ids) tagMap
                | None -> Map.add tag [elemId] tagMap
            ) acc
        ) Map.empty
    
    /// Index/home page handler
    let indexHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET / - Home page requested")
            let layerCounts = 
                registry.elementsByLayer
                |> Map.map (fun _ ids -> List.length ids)
            logger.LogDebug($"Layer summary: {layerCounts}")
            let html = Views.indexPage registry
            htmlView html next ctx
    
    /// Layer page handler
    let layerHandler (layer: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /{layer} - Layer page requested")
            let normalizedLayer = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(layer)
            match Map.tryFind normalizedLayer Config.layerOrder with
            | Some layerInfo ->
                let elements = ElementRegistry.getLayerElements normalizedLayer registry
                let filterValue =
                    match ctx.GetQueryStringValue "filter" with
                    | Ok value when not (String.IsNullOrWhiteSpace value) -> Some value
                    | _ -> None

                let filteredElements =
                    match filterValue with
                    | Some term ->
                        elements
                        |> List.filter (fun elem ->
                            elem.name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                        )
                    | None -> elements

                logger.LogInformation($"Found {List.length elements} elements in layer {layer}")
                elements |> List.iter (fun elem ->
                    logger.LogDebug($"  - {elem.id}: {elem.name}")
                )
                let isHxRequest = ctx.Request.Headers.ContainsKey "HX-Request"
                if isHxRequest then
                    let partial = Views.layerElementsPartial filteredElements registry
                    htmlView partial next ctx
                else
                    let html = Views.layerPage (layer.ToLowerInvariant()) layerInfo filteredElements registry filterValue
                    htmlView html next ctx
            | None -> 
                logger.LogWarning($"Layer not found: {layer} (normalized: {normalizedLayer})")
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element detail page handler
    let elementHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /elements/{elemId} - Element detail requested")
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                logger.LogInformation($"Found element: {elemId} ({elem.name})")
                let incoming = ElementRegistry.getIncomingRelations elemId registry
                let outgoing = elem.relationships
                logger.LogInformation($"  Incoming relations: {List.length incoming}, Outgoing relations: {List.length outgoing}")
                
                if List.length outgoing > 0 then
                    logger.LogInformation($"  Outgoing targets:")
                    outgoing |> List.iter (fun rel ->
                        logger.LogInformation($"    - {rel.target}")
                    )
                
                let elemWithRels = ElementRegistry.withRelations elem registry
                logger.LogInformation($"  After withRelations: incoming={List.length elemWithRels.incomingRelations}, outgoing={List.length elemWithRels.outgoingRelations}")
                
                let html = Views.elementPage elemWithRels
                htmlView html next ctx
            | None ->
                logger.LogWarning($"Element not found: {elemId}")
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx

    /// Element edit form partial handler
    let elementEditHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /elements/{elemId}/edit - Element edit form requested")
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                let html = Views.elementEditFormPartial elem registry
                htmlView html next ctx
            | None ->
                logger.LogWarning($"Element not found: {elemId}")
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx

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
            logger.LogDebug($"relationTypeOptionsHandler: sourceId='{sourceId}', targetId='{targetId}', currentValue='{currentValue}', index={index}")  
            let allowedTypes =
                match Map.tryFind sourceId registry.elements, Map.tryFind targetId registry.elements with
                | Some sourceElem, Some targetElem ->
                    let rules = registry.relationshipRules
                    let sourceConcept = tryGetConceptName sourceElem
                    let targetConcept = tryGetConceptName targetElem
                    logger.LogInformation($"Relation lookup: sourceConcept={sourceConcept}, targetConcept={targetConcept}")
                    match Map.tryFind (sourceConcept, targetConcept) rules with
                    | Some codes ->
                        logger.LogInformation($"Relation rules hit: {Set.count codes} allowed codes")
                        codes
                        |> Set.toList
                        |> List.choose relationCodeToName
                    | None -> []
                | None, _ ->
                    logger.LogWarning($"Relation lookup failed: sourceId '{sourceId}' not found")
                    []
                | _, None ->
                    logger.LogWarning($"Relation lookup failed: targetId '{targetId}' not found")
                    []

            logger.LogInformation($"GET /elements/relations/types - source={sourceId}, target={targetId}")
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

            logger.LogInformation($"GET /elements/relations/row - index={index}")
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

            logger.LogInformation($"GET /elements/types - layer={layerValue}")
            let selectNode = Views.elementTypeSelectPartial layerValue currentValue
            htmlView selectNode next ctx

    /// Element download handler
    let elementDownloadHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            task {
                logger.LogInformation($"POST /elements/{elemId}/download - Element download requested")
                match ElementRegistry.getElement elemId registry with
                | None ->
                    logger.LogWarning($"Element not found: {elemId}")
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

                    let id = getOrDefault (getTrimmed "id") elem.id
                    let name = getOrDefault (getTrimmed "name") elem.name
                    let elementType = getOrDefault (getTrimmed "type") defaultType
                    let layer = getOrDefault (getTrimmed "layer") defaultLayer
                    let content = getRaw "content"

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

                    let lines = ResizeArray<string>()
                    lines.Add("---")
                    lines.Add($"id: {id}")
                    lines.Add($"name: {yamlScalar name}")
                    lines.Add($"type: {elementType}")
                    lines.Add($"layer: {layer}")

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

                    if not (List.isEmpty relationships) then
                        lines.Add("relationships:")
                        for (relType, target, desc) in relationships do
                            lines.Add($"  - type: {relType}")
                            lines.Add($"    target: {target}")
                            if not (String.IsNullOrWhiteSpace desc) then
                                lines.Add($"    description: {yamlScalar desc}")

                    if not (List.isEmpty properties) then
                        lines.Add("properties:")
                        for (key, value) in properties do
                            lines.Add($"  {key}: {yamlScalar value}")

                    if not (List.isEmpty tags) then
                        lines.Add("tags:")
                        for tag in tags do
                            lines.Add($"  - {yamlScalar tag}")

                    lines.Add("---")
                    lines.Add("")
                    lines.Add(content.TrimEnd())

                    let markdown = String.Join("\n", lines)
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
            logger.LogInformation($"Found {Map.count tagIndex} tags")
            let html = Views.tagsIndexPage tagIndex registry
            htmlView html next ctx

    /// Layer Cytoscape diagram handler
    let layerDiagramCytoscapeHandler (layer: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /diagrams/layer/{layer} - Cytoscape layer diagram requested")
            let normalizedLayer = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(layer)
            match Map.tryFind normalizedLayer Config.layerOrder with
            | Some layerInfo ->
                let data = buildLayerCytoscape normalizedLayer registry
                let html = wrapCytoscapeHtml (sprintf "%s Layer" layerInfo.displayName) data true
                htmlString html next ctx
            | None ->
                logger.LogWarning($"Layer not found for Cytoscape diagram: {layer} (normalized: {normalizedLayer})")
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element context Cytoscape diagram handler
    let contextDiagramCytoscapeHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /diagrams/context/{elemId}/cytoscape - Cytoscape context diagram requested")
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                let depth = 
                    match ctx.GetQueryStringValue "depth" with
                    | Ok value -> 
                        match System.Int32.TryParse(value) with
                        | (true, d) when d > 0 && d <= 3 -> d
                        | _ -> 1
                    | Error _ -> 1
                
                logger.LogInformation($"Found element: {elemId} ({elem.name}), generating Cytoscape context diagram with depth={depth}")
                let data = buildContextCytoscape elemId depth registry
                let title = sprintf "Context: %s (Depth %d)" elem.name depth
                let html = wrapCytoscapeHtml title data false
                htmlString html next ctx
            | None ->
                logger.LogWarning($"Element not found for Cytoscape context diagram: {elemId}")
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx
    
    /// Validation errors API handler - list all errors
    let validationErrorsHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /api/validation/errors - Validation errors requested")
            let errors = ElementRegistry.getValidationErrors registry
            logger.LogInformation($"Returning {List.length errors} validation errors")
            
            let errorsList =
                errors
                |> List.map (fun err ->
                    dict [
                        ("filePath", box err.filePath)
                        ("elementId", box (err.elementId |> Option.defaultValue ""))
                        ("errorType", box (errorTypeToString err.errorType))
                        ("message", box err.message)
                        ("severity", box (severityToString err.severity))
                    ]
                )
            
            json errorsList next ctx
    
    /// Validation errors by file handler
    let fileValidationErrorsHandler (filePath: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /api/validation/file - Validation errors for file: {filePath}")
            let decodedPath = Uri.UnescapeDataString(filePath)
            let errors = ElementRegistry.getFileValidationErrors decodedPath registry
            logger.LogInformation($"File '{decodedPath}' has {List.length errors} validation errors")
            
            let errorsList =
                errors
                |> List.map (fun err ->
                    dict [
                        ("filePath", box err.filePath)
                        ("elementId", box (err.elementId |> Option.defaultValue ""))
                        ("errorType", box (errorTypeToString err.errorType))
                        ("message", box err.message)
                        ("severity", box (severityToString err.severity))
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
                    |> List.map (fun (eType, errs) -> dict [("type", box (errorTypeToString eType)); ("count", box errs.Length)])
                ))
            ]
            
            json stats next ctx
    
    /// Validation page handler
    let validationPageHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /validation - Validation page requested")
            let errors = ElementRegistry.getValidationErrors registry
            logger.LogInformation($"Displaying {List.length errors} validation errors")
            let html = Views.validationPage errors
            htmlView html next ctx
    
    /// Revalidate file handler
    let revalidateFileHandler (filePath: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"POST /api/validation/revalidate - Revalidating file: {filePath}")
            let decodedPath = Uri.UnescapeDataString(filePath)
            
            ElementRegistry.revalidateFile decodedPath registry logger
            let errors = ElementRegistry.getFileValidationErrors decodedPath registry
            
            let result = dict [
                ("filePath", box decodedPath)
                ("revalidated", box true)
                ("errorCount", box errors.Length)
                ("errors", box (
                    errors
                    |> List.map (fun err ->
                        dict [
                            ("errorType", box (errorTypeToString err.errorType))
                            ("message", box err.message)
                            ("severity", box (severityToString err.severity))
                        ]
                    )
                ))
            ]
            
            json result next ctx
    
    /// Individual tag page handler
    let tagHandler (tag: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /tags/{tag} - Tag page requested")
            let tagIndex = buildTagIndex registry
            match Map.tryFind tag tagIndex with
            | Some elemIds ->
                logger.LogInformation($"Found {List.length elemIds} elements with tag '{tag}'")
                let elements =
                    elemIds
                    |> List.choose (fun id -> ElementRegistry.getElement id registry)
                    |> List.sortBy (fun e -> e.name)
                let html = Views.tagPage tag elements
                htmlView html next ctx
            | None ->
                logger.LogWarning($"Tag not found: {tag}")
                setStatusCode 404 >=> text "Tag not found" |> fun handler -> handler next ctx
    
    /// Create route handlers
    let createHandlers (registry: ElementRegistry) (loggerFactory: ILoggerFactory) : HttpHandler =
        let logger = loggerFactory.CreateLogger("Handlers")
        
        logger.LogInformation("Initializing route handlers")
        logger.LogInformation($"Registry contains {Map.count registry.elements} elements")
        
        choose [
            route "/" >=> indexHandler registry logger
            route "/index.html" >=> indexHandler registry logger
            route "/elements/types" >=> elementTypeOptionsHandler logger
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
