namespace EAArchive

open System
open System.Collections.Generic
open Microsoft.Extensions.Logging
open Giraffe
open Giraffe.ViewEngine

module Handlers =

    let private sanitizeMermaidId (value: string) =
        value
            .Replace("-", "_")
            .Replace(" ", "_")
            .Replace("/", "_")
            .Replace(".", "_")

    let private escapeMermaidLabel (value: string) =
        value.Replace("\"", "\\\"")

    let private buildLayerMermaid (layer: string) (registry: ElementRegistry) =
        let elements = ElementRegistry.getLayerElements layer registry
        let elementMap = registry.elements

        let lines = System.Collections.Generic.List<string>()
        lines.Add("graph TD")

        let layerIds = elements |> List.map (fun e -> e.id) |> Set.ofList

        // Collect all element IDs that need nodes (layer elements + relationship targets)
        let mutable allNodeIds = Set.empty<string>
        for elem in elements do
            allNodeIds <- Set.add elem.id allNodeIds
            for rel in elem.relationships do
                if Map.containsKey rel.target elementMap then
                    allNodeIds <- Set.add rel.target allNodeIds

        // Also check incoming relationships
        for rel in registry.elements |> Map.toList |> List.collect (fun (_, elem) -> elem.relationships |> List.map (fun r -> (elem.id, r))) do
            let sourceId, rel = rel
            if Set.contains rel.target layerIds then
                if Map.containsKey sourceId elementMap then
                    allNodeIds <- Set.add sourceId allNodeIds

        // Create nodes for all referenced elements
        for nodeId in allNodeIds do
            match Map.tryFind nodeId elementMap with
            | Some elem ->
                let sanitizedId = sanitizeMermaidId nodeId
                let label = escapeMermaidLabel elem.name
                lines.Add($"{sanitizedId}[\"{label}\"]")
            | None -> ()

        lines.Add("")
        lines.Add("%% Relationships")

        for rel in registry.elements |> Map.toList |> List.collect (fun (_, elem) -> elem.relationships |> List.map (fun r -> (elem.id, r))) do
            let sourceId, rel = rel
            if Set.contains sourceId layerIds || Set.contains rel.target layerIds then
                match Map.tryFind sourceId elementMap, Map.tryFind rel.target elementMap with
                | Some _, Some _ ->
                    let fromId = sanitizeMermaidId sourceId
                    let toId = sanitizeMermaidId rel.target
                    let relLabel = escapeMermaidLabel rel.relationType
                    lines.Add($"{fromId} -->|{relLabel}| {toId}")
                | _ -> ()

        lines.Add("")
        lines.Add("%% Clickable links")
        for nodeId in allNodeIds do
            let sanitizedId = sanitizeMermaidId nodeId
            lines.Add($"click {sanitizedId} \"/elements/{nodeId}\" \"View details\"")

        System.String.Join("\n", lines)

    let private wrapMermaidHtml (title: string) (diagram: string) =
        sprintf """<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>%s</title>
    <script src="https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/svg-pan-zoom@3.6.1/dist/svg-pan-zoom.min.js"></script>
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }
        .container {
            max-width: 1400px;
            margin: 0 auto;
            background: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        h1 {
            color: #333;
            border-bottom: 2px solid #667eea;
            padding-bottom: 10px;
            margin-bottom: 30px;
            margin-top: 0;
        }
        .diagram-toolbar {
            display: flex;
            gap: 8px;
            margin: 10px 0 20px;
        }
        .diagram-toolbar button {
            background: #667eea;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 8px 16px;
            cursor: pointer;
            font-size: 14px;
            font-weight: 500;
            transition: background 0.2s;
        }
        .diagram-toolbar button:hover {
            background: #5568d3;
        }
        .diagram-wrapper {
            position: relative;
            width: 100%%;
            height: 75vh;
            min-height: 500px;
            border: 1px solid #e0e0e0;
            border-radius: 6px;
            overflow: hidden;
            background: #fff;
        }
        #diagram-container {
            width: 100%%;
            height: 100%%;
            overflow: hidden;
        }
        .mermaid {
            display: flex;
            align-items: center;
            justify-content: center;
            width: 100%%;
            height: 100%%;
        }
        .mermaid svg {
            width: 100%% !important;
            height: 100%% !important;
            max-width: none;
        }
        .info {
            background: #eef2ff;
            padding: 15px;
            border-left: 4px solid #667eea;
            margin: 20px 0;
            border-radius: 4px;
        }
        .info p {
            margin: 5px 0;
            color: #333;
        }
    </style>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            if (window.mermaid) {
                window.mermaid.initialize({ startOnLoad: true, theme: 'default' });
                window.mermaid.init(undefined, document.querySelectorAll('.mermaid'));
            }
        });
    </script>
</head>
<body>
    <div class="container">
        <h1>%s</h1>
        <div class="info">
            <p><strong>💡 Tip:</strong> Click on any element in the diagram to view its detailed documentation.</p>
            <p><strong>🧭 Navigation:</strong> Use the zoom controls below or scroll to zoom and drag to pan.</p>
        </div>
        <div class="diagram-toolbar">
            <button type="button" id="zoom-in">Zoom In</button>
            <button type="button" id="zoom-out">Zoom Out</button>
            <button type="button" id="zoom-reset">Reset</button>
        </div>
        <div class="diagram-wrapper">
            <div id="diagram-container">
                <div class="mermaid">%s</div>
            </div>
        </div>
    </div>
    <script>
        let panZoomInstance = null;

        function initPanZoom() {
            const svg = document.querySelector('.mermaid svg');
            if (!svg) {
                setTimeout(initPanZoom, 100);
                return;
            }

            if (panZoomInstance) {
                panZoomInstance.destroy();
                panZoomInstance = null;
            }

            // Ensure SVG has viewBox
            if (!svg.getAttribute('viewBox')) {
                try {
                    const bbox = svg.getBBox();
                    svg.setAttribute('viewBox', `${bbox.x} ${bbox.y} ${bbox.width} ${bbox.height}`);
                } catch (e) {
                    console.warn('Could not set viewBox:', e);
                }
            }

            // Remove any size constraints that might cause issues
            svg.removeAttribute('height');
            svg.removeAttribute('width');
            svg.style.width = '100%%';
            svg.style.height = '100%%';

            try {
                panZoomInstance = svgPanZoom(svg, {
                    zoomEnabled: true,
                    controlIconsEnabled: false,
                    fit: true,
                    center: true,
                    minZoom: 0.1,
                    maxZoom: 20,
                    zoomScaleSensitivity: 0.3
                });
            } catch (e) {
                console.error('Error initializing pan-zoom:', e);
            }
        }

        // Wait for Mermaid to render
        setTimeout(initPanZoom, 500);

        document.getElementById('zoom-in').addEventListener('click', () => {
            if (panZoomInstance) panZoomInstance.zoomIn();
        });

        document.getElementById('zoom-out').addEventListener('click', () => {
            if (panZoomInstance) panZoomInstance.zoomOut();
        });

        document.getElementById('zoom-reset').addEventListener('click', () => {
            if (panZoomInstance) {
                panZoomInstance.reset();
                panZoomInstance.fit();
                panZoomInstance.center();
            }
        });
    </script>
</body>
</html>""" title title diagram
    
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
            logger.LogInformation($"GET /elements/{elemId} - Element detail requested")
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                logger.LogInformation($"Found element: {elemId} ({elem.name}) in layer {elem.layer}")
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
            match Config.layerOrder |> List.tryFind (fun l -> l.key = layer) with
            | Some layerInfo ->
                let diagram = buildLayerMermaid layer registry
                let html = wrapMermaidHtml ($"{layerInfo.displayName} Diagram") diagram
                htmlString html next ctx
            | None ->
                logger.LogWarning($"Layer not found for diagram: {layer}")
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
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
            routef "/diagrams/layers/%s" (fun layer -> layerDiagramHandler layer registry logger)
            route "/tags" >=> tagsIndexHandler registry logger
            routef "/tags/%s" (fun tag -> tagHandler (Uri.UnescapeDataString tag) registry logger)
            routef "/%s" (fun layer -> layerHandler layer registry logger)
        ]
