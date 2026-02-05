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
        | ErrorType.Unknown value -> value
    
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
                logger.LogInformation($"Found {List.length elements} elements in layer {layer}")
                elements |> List.iter (fun elem ->
                    logger.LogDebug($"  - {elem.id}: {elem.name}")
                )
                let html = Views.layerPage layerInfo elements registry
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
    
    /// Tags index handler
    let tagsIndexHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /tags - Tags index page requested")
            let tagIndex = buildTagIndex registry
            logger.LogInformation($"Found {Map.count tagIndex} tags")
            let html = Views.tagsIndexPage tagIndex registry
            htmlView html next ctx

    /// Layer mermaid diagram handler
    let layerDiagramHandler (layer: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /diagrams/layers/{layer} - Layer diagram requested")
            let normalizedLayer = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(layer)
            match Map.tryFind normalizedLayer Config.layerOrder with
            | Some layerInfo ->
                let diagram = buildLayerMermaid normalizedLayer registry
                let html = wrapMermaidHtml ($"{layerInfo.displayName} Diagram") diagram
                htmlString html next ctx
            | None ->
                logger.LogWarning($"Layer not found for diagram: {layer} (normalized: {normalizedLayer})")
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element context diagram handler
    let contextDiagramHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /diagrams/context/{elemId} - Context diagram requested")
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                let depth = 
                    match ctx.GetQueryStringValue "depth" with
                    | Ok value -> 
                        match System.Int32.TryParse(value) with
                        | (true, d) when d > 0 && d <= 3 -> d
                        | _ -> 1
                    | Error _ -> 1
                
                logger.LogInformation($"Found element: {elemId} ({elem.name}), generating context diagram with depth={depth}")
                let diagram = buildContextDiagram elemId depth registry
                let title = sprintf "Context: %s (Depth %d)" elem.name depth
                let html = wrapMermaidHtml title diagram
                htmlString html next ctx
            | None ->
                logger.LogWarning($"Element not found for context diagram: {elemId}")
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx
    
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
                        ("errorType", box err.errorType)
                        ("message", box err.message)
                        ("severity", box err.severity)
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
                        ("errorType", box err.errorType)
                        ("message", box err.message)
                        ("severity", box err.severity)
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
                    |> List.map (fun (eType, errs) -> dict [("type", box eType); ("count", box errs.Length)])
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
                            ("errorType", box err.errorType)
                            ("message", box err.message)
                            ("severity", box err.severity)
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
            routef "/elements/%s" (fun elemId -> elementHandler elemId registry logger)
            
            // Cytoscape diagrams (new default)
            routef "/diagrams/layer/%s" (fun layer -> layerDiagramCytoscapeHandler layer registry logger)
            routef "/diagrams/context/%s" (fun elemId -> contextDiagramCytoscapeHandler elemId registry logger)
            
            // Legacy Mermaid diagrams (backwards compatibility)
            routef "/diagrams/layers/%s" (fun layer -> layerDiagramHandler layer registry logger)
            routef "/diagrams/layers/%s/mermaid" (fun layer -> layerDiagramHandler layer registry logger)
            routef "/diagrams/context/%s/mermaid" (fun elemId -> contextDiagramHandler elemId registry logger)
            
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
