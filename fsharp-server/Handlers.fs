namespace EAArchive

open System
open System.Collections.Generic
open Microsoft.Extensions.Logging
open Giraffe
open Giraffe.ViewEngine

module Handlers =
    
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
            logger.LogInformation($"GET /{layer}.html - Layer page requested")
            match Config.layerOrder |> List.tryFind (fun l -> l.key = layer) with
            | Some layerInfo ->
                let elements = ElementRegistry.getLayerElements layer registry
                logger.LogInformation($"Found {List.length elements} elements in layer {layer}")
                elements |> List.iter (fun elem ->
                    logger.LogDebug($"  - {elem.id}: {elem.name}")
                )
                let html = Views.layerPage layerInfo elements registry
                htmlView html next ctx
            | None -> 
                logger.LogWarning($"Layer not found: {layer}")
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element detail page handler
    let elementHandler (elemId: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /elements/{elemId}.html - Element detail requested")
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                logger.LogInformation($"Found element: {elemId} ({elem.name}) in layer {elem.layer}")
                let incoming = ElementRegistry.getIncomingRelations elemId registry
                let outgoing = elem.relationships
                logger.LogDebug($"  Incoming relations: {List.length incoming}, Outgoing relations: {List.length outgoing}")
                let elemWithRels = ElementRegistry.withRelations elem registry
                let html = Views.elementPage elemWithRels
                htmlView html next ctx
            | None ->
                logger.LogWarning($"Element not found: {elemId}")
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx
    
    /// Tags index handler
    let tagsIndexHandler (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation("GET /tags.html - Tags index page requested")
            let tagIndex = buildTagIndex registry
            logger.LogInformation($"Found {Map.count tagIndex} tags")
            let html = Views.tagsIndexPage tagIndex registry
            htmlView html next ctx
    
    /// Individual tag page handler
    let tagHandler (tag: string) (registry: ElementRegistry) (logger: ILogger) : HttpHandler =
        fun next ctx ->
            logger.LogInformation($"GET /tags/{tag}.html - Tag page requested")
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
            routef "/elements/%s.html" (fun elemId -> elementHandler elemId registry logger)
            route "/tags.html" >=> tagsIndexHandler registry logger
            routef "/tags/%s.html" (fun tag -> tagHandler (Uri.UnescapeDataString tag) registry logger)
            routef "/%s.html" (fun layer -> layerHandler layer registry logger)
        ]
