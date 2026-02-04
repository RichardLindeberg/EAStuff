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

    let private sanitizeMermaidId (value: string) =
        value
            .Replace("-", "_")
            .Replace(" ", "_")
            .Replace("/", "_")
            .Replace(".", "_")

    let private escapeMermaidLabel (value: string) =
        value.Replace("\"", "\\\"")
    
    let private symbolsBaseUrl = "/assets/archimate-symbols/"
    let private iconsBaseUrl = "/assets/archimate-icons/"
    let private symbolsRoot =
        let cwdRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "archimate-symbols")
        if Directory.Exists(cwdRoot) then cwdRoot
        else
            let baseRoot = Path.Combine(AppContext.BaseDirectory, "wwwroot", "assets", "archimate-symbols")
            if Directory.Exists(baseRoot) then baseRoot else ""
    
    let private symbolsEnabled = symbolsRoot <> ""
    let private iconsRoot =
        if symbolsEnabled then
            let assetsRoot = Directory.GetParent(symbolsRoot).FullName
            Path.Combine(assetsRoot, "archimate-icons")
        else ""
    let private iconsEnabled = iconsRoot <> ""
    
    let private toTitleCase (value: string) =
        let normalized = value.Replace("-", " ").Replace("_", " ").ToLowerInvariant()
        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(normalized)

    let private elementTypeToKey (elementType: ElementType) : string =
        match elementType with
        | ElementType.Strategy st ->
            match st with
            | StrategyElement.Stakeholder -> "stakeholder"
            | StrategyElement.Driver -> "driver"
            | StrategyElement.Assessment -> "assessment"
            | StrategyElement.Goal -> "goal"
            | StrategyElement.Outcome -> "outcome"
            | StrategyElement.Principle -> "principle"
            | StrategyElement.Requirement -> "requirement"
            | StrategyElement.Constraint -> "constraint"
        | ElementType.Motivation mt ->
            match mt with
            | MotivationElement.Stakeholder -> "stakeholder"
            | MotivationElement.Driver -> "driver"
            | MotivationElement.Assessment -> "assessment"
            | MotivationElement.Goal -> "goal"
            | MotivationElement.Outcome -> "outcome"
            | MotivationElement.Principle -> "principle"
            | MotivationElement.Requirement -> "requirement"
            | MotivationElement.Constraint -> "constraint"
            | MotivationElement.Meaning -> "meaning"
            | MotivationElement.Value -> "value"
        | ElementType.Business bt ->
            match bt with
            | BusinessElement.Actor -> "business-actor"
            | BusinessElement.Role -> "business-role"
            | BusinessElement.Process -> "business-process"
            | BusinessElement.Function -> "business-function"
            | BusinessElement.Service -> "business-service"
            | BusinessElement.Object -> "business-object"
            | BusinessElement.Event -> "business-event"
            | BusinessElement.Product -> "product"
        | ElementType.Application at ->
            match at with
            | ApplicationElement.Component -> "application-component"
            | ApplicationElement.Function -> "application-function"
            | ApplicationElement.Service -> "application-service"
            | ApplicationElement.Interface -> "application-interface"
            | ApplicationElement.DataObject -> "data-object"
        | ElementType.Technology tt ->
            match tt with
            | TechnologyElement.Technology -> "technology"
            | TechnologyElement.Device -> "device"
            | TechnologyElement.SystemSoftware -> "system-software"
            | TechnologyElement.Service -> "technology-service"
            | TechnologyElement.Interface -> "technology-interface"
            | TechnologyElement.Artifact -> "artifact"
            | TechnologyElement.Node -> "node"
            | TechnologyElement.CommunicationNetwork -> "communication-network"
        | ElementType.Physical pt ->
            match pt with
            | PhysicalElement.Equipment -> "equipment"
            | PhysicalElement.Facility -> "facility"
            | PhysicalElement.DistributionNetwork -> "distribution-network"
        | ElementType.Implementation it ->
            match it with
            | ImplementationElement.WorkPackage -> "work-package"
            | ImplementationElement.Deliverable -> "deliverable"
            | ImplementationElement.ImplementationEvent -> "implementation-event"
            | ImplementationElement.Plateau -> "plateau"
            | ImplementationElement.Gap -> "gap"
        | ElementType.Unknown (_, elementTypeName) ->
            if String.IsNullOrWhiteSpace elementTypeName then "unknown" else elementTypeName.ToLowerInvariant()

    let private relationTypeToKey (relationType: RelationType) : string =
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

    let private getElementTypeLabel (elementType: string) : string =
        let formatted = toTitleCase elementType
        // Abbreviate long type names for badge display
        match formatted.Length with
        | len when len > 20 -> formatted.Substring(0, 20) + "..."
        | _ -> formatted
    
    let private getSymbolFile (elementType: string) : (string * string) option =
        if not symbolsEnabled then None
        else
            let fileName = toTitleCase elementType + ".svg"
            let fullPath = Path.Combine(symbolsRoot, fileName)
            if File.Exists(fullPath) then Some (fileName, fullPath) else None
    
    let private tryGetSymbolFileName (elementType: string) : string option =
        getSymbolFile elementType |> Option.map fst

    let private iconViewBox = "120 0 30 30"
    let private iconSizePx = "12"
    
    let private ensureIconFile (fileName: string) (sourcePath: string) : bool =
        try
            if not iconsEnabled then false
            else
                if not (Directory.Exists(iconsRoot)) then
                    Directory.CreateDirectory(iconsRoot) |> ignore
                let iconPath = Path.Combine(iconsRoot, fileName)
                let doc = XDocument.Load(sourcePath)
                let svgNs = XNamespace.Get("http://www.w3.org/2000/svg")
                let g = doc.Root.Element(svgNs + "g")
                if isNull g then false
                else
                    let children = g.Elements() |> Seq.toList
                    if children.Length = 0 then false
                    else
                        let iconElements = if children.Length > 1 then children |> List.tail else children
                        let newSvg =
                            XElement(svgNs + "svg",
                                XAttribute("xmlns", svgNs.NamespaceName),
                                XAttribute("width", iconSizePx),
                                XAttribute("height", iconSizePx),
                                XAttribute("viewBox", iconViewBox))
                        let newG = XElement(svgNs + "g")
                        iconElements |> List.iter (fun el -> newG.Add(XElement(el)))
                        newSvg.Add(newG)
                        let newDoc = XDocument(newSvg)
                        newDoc.Save(iconPath)
                        true
        with _ -> false
    
    let private tryGetIconFileName (elementType: string) : string option =
        match getSymbolFile elementType with
        | Some (fileName, sourcePath) ->
            if ensureIconFile fileName sourcePath then Some fileName else None
        | None -> None
    
    let private buildMermaidLabel (elem: Element) : string =
        let safeName = WebUtility.HtmlEncode elem.name
        sprintf "<div class='archimate-node'><div class='archimate-label'>%s</div></div>" safeName
    
    /// ArchiMate 3.2 Standard Color Scheme
    let private getArchimateColor (elementType: string) : string =
        match elementType.ToLower() with
        // Strategy Layer - Light Orange
        | "resource" | "capability" | "value-stream" | "course-of-action" -> "#f5deaa"
        // Business Layer - Light Yellow
        | "business-actor" | "business-role" | "business-collaboration" 
        | "business-interface" | "business-process" | "business-function" 
        | "business-interaction" | "business-event" | "business-service" 
        | "product" | "contract" | "representation" -> "#ffffb5"
        | "business-object" -> "#ffffb5"
        // Application Layer - Light Blue
        | "application-component" | "application-collaboration" 
        | "application-interface" | "application-function" 
        | "application-interaction" | "application-process" 
        | "application-event" | "application-service" -> "#b5ffff"
        | "data-object" -> "#b5ffff"
        // Technology Layer - Light Green
        | "node" | "device" | "system-software" | "technology-collaboration" 
        | "technology-interface" | "path" | "communication-network" 
        | "technology-function" | "technology-process" 
        | "technology-interaction" | "technology-event" 
        | "technology-service" | "artifact" | "equipment" 
        | "facility" | "distribution-network" | "material" -> "#c9e7b7"
        // Motivation Layer - Light Purple/Blue
        | "stakeholder" | "driver" | "assessment" | "goal" 
        | "outcome" | "principle" | "requirement" 
        | "constraint" | "meaning" | "value" -> "#ccccff"
        // Implementation Layer - Light Pink
        | "work-package" | "deliverable" | "implementation-event" 
        | "plateau" | "gap" -> "#ffe0e0"
        // Default
        | _ -> "#ffffff"

    // ========================================
    // Cytoscape.js Relationship Styling
    // ========================================
    
    let private getRelationshipColor (relType: string) : string =
        match relType.ToLower() with
        | "composition" -> "#333333"
        | "aggregation" -> "#666666"
        | "assignment" -> "#9933cc"
        | "realization" -> "#cc6600"
        | "serving" -> "#0066cc"
        | "access" -> "#00cc66"
        | "influence" -> "#cc3366"
        | "triggering" -> "#0099cc"
        | "flow" -> "#0099cc"
        | "specialization" -> "#666666"
        | "association" -> "#999999"
        | _ -> "#999999"

    let private getRelationshipArrowType (relType: string) : string =
        match relType.ToLower() with
        | "composition" | "realization" | "serving" | "triggering" | "flow" -> "triangle"
        | "aggregation" -> "diamond"
        | "assignment" | "influence" -> "vee"
        | "specialization" -> "triangle-tee"
        | "association" -> "none"
        | _ -> "triangle"

    let private getRelationshipLineStyle (relType: string) : string =
        match relType.ToLower() with
        | "assignment" | "influence" -> "dashed"
        | _ -> "solid"

    let private getRelationshipLineWidth (relType: string) : float =
        match relType.ToLower() with
        | "composition" | "realization" -> 2.5
        | "aggregation" -> 2.0
        | _ -> 1.5

    let private getNodeShape (elementType: string) : string =
        match elementType.ToLower() with
        // Business behavior
        | t when t.Contains("process") || t.Contains("function") 
              || t.Contains("interaction") || t.Contains("event") 
              || t.Contains("service") -> "roundrectangle"
        
        // Motivation hexagons
        | t when t.Contains("goal") || t.Contains("outcome") 
              || t.Contains("principle") || t.Contains("requirement") 
              || t.Contains("constraint") -> "hexagon"
        
        // Motivation ellipses
        | t when t.Contains("stakeholder") || t.Contains("driver") 
              || t.Contains("assessment") || t.Contains("value") -> "ellipse"
        
        // Strategy rounded
        | t when t.Contains("capability") || t.Contains("resource") 
              || t.Contains("course-of-action") || t.Contains("value-stream") -> "roundrectangle"
        
        // Default rectangle for structure
        | _ -> "rectangle"

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
                let label = buildMermaidLabel elem
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
                    let relLabel = escapeMermaidLabel (relationTypeToKey rel.relationType)
                    lines.Add($"{fromId} -->|{relLabel}| {toId}")
                | _ -> ()

        lines.Add("")
        lines.Add("%% Clickable links")
        for nodeId in allNodeIds do
            let sanitizedId = sanitizeMermaidId nodeId
            lines.Add($"click {sanitizedId} \"/elements/{nodeId}\" \"View details\"")
        
        // Add ArchiMate color styling
        lines.Add("")
        lines.Add("%% ArchiMate 3.2 Standard Colors")
        let elementsByType = 
            allNodeIds 
            |> Seq.choose (fun nodeId -> Map.tryFind nodeId elementMap)
            |> Seq.groupBy (fun elem -> elementTypeToKey elem.elementType)
            |> Seq.toList
        
        for (elemType, elems) in elementsByType do
            let color = getArchimateColor elemType
            let className = (sanitizeMermaidId elemType) + "Style"
            let ids = elems |> Seq.map (fun e -> sanitizeMermaidId e.id) |> String.concat ","
            lines.Add($"classDef {className} fill:{color},stroke:#333,stroke-width:2px")
            lines.Add($"class {ids} {className}")

        System.String.Join("\n", lines)

    let private buildContextDiagram (elementId: string) (depth: int) (registry: ElementRegistry) =
        let elementMap = registry.elements
        
        // Find all connected elements up to specified depth
        let mutable allNodeIds = Set.empty<string>
        let mutable currentLevel = Set.singleton elementId
        allNodeIds <- Set.add elementId allNodeIds
        
        for _ in 1..depth do
            let mutable nextLevel = Set.empty<string>
            for elemId in currentLevel do
                match Map.tryFind elemId elementMap with
                | Some elem ->
                    // Add outgoing relationships
                    for rel in elem.relationships do
                        if Map.containsKey rel.target elementMap then
                            allNodeIds <- Set.add rel.target allNodeIds
                            nextLevel <- Set.add rel.target nextLevel
                | None -> ()
            
            // Also check incoming relationships
            for rel in elementMap |> Map.toList |> List.collect (fun (_, elem) -> elem.relationships |> List.map (fun r -> (elem.id, r))) do
                let sourceId, rel = rel
                if Set.contains rel.target currentLevel && Map.containsKey sourceId elementMap then
                    allNodeIds <- Set.add sourceId allNodeIds
                    nextLevel <- Set.add sourceId nextLevel
            
            currentLevel <- nextLevel
        
        let lines = System.Collections.Generic.List<string>()
        lines.Add("graph TD")
        
        // Create nodes for all collected elements
        for nodeId in allNodeIds do
            match Map.tryFind nodeId elementMap with
            | Some elem ->
                let sanitizedId = sanitizeMermaidId nodeId
                let label = buildMermaidLabel elem
                lines.Add($"{sanitizedId}[\"{label}\"]")
            | None -> ()
        
        lines.Add("")
        lines.Add("%% Relationships")
        
        // Add relationships between collected elements
        for rel in elementMap |> Map.toList |> List.collect (fun (_, elem) -> elem.relationships |> List.map (fun r -> (elem.id, r))) do
            let sourceId, rel = rel
            if Set.contains sourceId allNodeIds && Set.contains rel.target allNodeIds then
                match Map.tryFind sourceId elementMap, Map.tryFind rel.target elementMap with
                | Some _, Some _ ->
                    let fromId = sanitizeMermaidId sourceId
                    let toId = sanitizeMermaidId rel.target
                    let relLabel = escapeMermaidLabel (relationTypeToKey rel.relationType)
                    lines.Add($"{fromId} -->|{relLabel}| {toId}")
                | _ -> ()
        
        lines.Add("")
        lines.Add("%% ArchiMate 3.2 Standard Colors")
        let elementsByType = 
            allNodeIds 
            |> Seq.choose (fun nodeId -> 
                if nodeId <> elementId then Map.tryFind nodeId elementMap else None)
            |> Seq.groupBy (fun elem -> elementTypeToKey elem.elementType)
            |> Seq.toList
        
        for (elemType, elems) in elementsByType do
            let color = getArchimateColor elemType
            let className = (sanitizeMermaidId elemType) + "Style"
            let ids = elems |> Seq.map (fun e -> sanitizeMermaidId e.id) |> String.concat ","
            lines.Add($"classDef {className} fill:{color},stroke:#333,stroke-width:2px")
            lines.Add($"class {ids} {className}")
        
        lines.Add("")
        lines.Add("%% Highlight the central element")
        let centralId = sanitizeMermaidId elementId
        lines.Add($"classDef centerStyle fill:#e3f2fd,stroke:#1976d2,stroke-width:4px")
        lines.Add($"class {centralId} centerStyle")
        
        lines.Add("")
        lines.Add("%% Clickable links")
        for nodeId in allNodeIds do
            let sanitizedId = sanitizeMermaidId nodeId
            lines.Add($"click {sanitizedId} \"/elements/{nodeId}\" \"View details\"")
        
        System.String.Join("\n", lines)
    
    // ========================================
    // Cytoscape.js Diagram Generation
    // ========================================
    
    let private buildCytoscapeData (elements: Element list) (allRelationships: (string * Relationship) list) : string =
        let nodes = 
            elements 
            |> List.map (fun elem ->
                let elementTypeKey = elementTypeToKey elem.elementType
                let color = getArchimateColor elementTypeKey
                let shape = getNodeShape elementTypeKey
                let icon = 
                    match tryGetIconFileName elementTypeKey with
                    | Some fileName -> sprintf "%s%s" iconsBaseUrl fileName
                    | None ->
                        match tryGetSymbolFileName elementTypeKey with
                        | Some fileName -> sprintf "%s%s" symbolsBaseUrl fileName
                        | None -> ""
                
                let typeLabel = getElementTypeLabel elementTypeKey
                let labelText = sprintf "[%s]\\n%s" typeLabel (escapeMermaidLabel elem.name)
                let classes = (sanitizeMermaidId elementTypeKey) + " badge-label"
                sprintf """{
                data: { 
                    id: "%s", 
                    label: "%s", 
                    type: "%s",
                    color: "%s",
                    icon: "%s",
                    shape: "%s"
                },
                classes: "%s"
            }""" 
                    elem.id 
                    labelText
                    elementTypeKey
                    color
                    icon
                    shape
                    classes
            )
        
        let edges = 
            allRelationships
            |> List.map (fun (sourceId, rel) ->
                let relationTypeKey = relationTypeToKey rel.relationType
                let color = getRelationshipColor relationTypeKey
                let arrowType = getRelationshipArrowType relationTypeKey
                let lineStyle = getRelationshipLineStyle relationTypeKey
                let lineWidth = getRelationshipLineWidth relationTypeKey
                
                sprintf """{
                data: { 
                    id: "%s_%s",
                    source: "%s", 
                    target: "%s", 
                    label: "%s",
                    relType: "%s",
                    color: "%s",
                    arrowType: "%s",
                    lineStyle: "%s",
                    lineWidth: %f
                }
            }""" 
                    sourceId
                    rel.target
                    sourceId
                    rel.target
                    relationTypeKey
                    relationTypeKey
                    color
                    arrowType
                    lineStyle
                    lineWidth
            )
        
        sprintf """{ "nodes": [%s], "edges": [%s] }""" 
            (String.concat "," nodes) 
            (String.concat "," edges)

    let private buildLayerCytoscape (layer: string) (registry: ElementRegistry) : string =
        let layerElements = ElementRegistry.getLayerElements layer registry
        
        // Collect all relationships involving layer elements
        let allRels = 
            registry.elements 
            |> Map.toList 
            |> List.collect (fun (id, elem) ->
                if layerElements |> List.exists (fun le -> le.id = id) then
                    elem.relationships |> List.map (fun rel -> (id, rel))
                else []
            )
        
        // Include related elements from other layers
        let relatedIds = allRels |> List.map (fun (_, rel) -> rel.target) |> Set.ofList
        let relatedElements = 
            registry.elements 
            |> Map.toList 
            |> List.choose (fun (id, elem) ->
                if Set.contains id relatedIds && not (layerElements |> List.exists (fun le -> le.id = id)) 
                then Some elem 
                else None
            )
        
        let allElements = layerElements @ relatedElements
        
        buildCytoscapeData allElements allRels

    let private buildContextCytoscape (elementId: string) (depth: int) (registry: ElementRegistry) : string =
        let rec collectNeighbors (currentIds: Set<string>) (currentDepth: int) : Set<string> =
            if currentDepth >= depth then currentIds
            else
                let nextIds = 
                    currentIds 
                    |> Set.toList 
                    |> List.collect (fun id ->
                        match Map.tryFind id registry.elements with
                        | Some elem ->
                            let outgoing = elem.relationships |> List.map (fun r -> r.target)
                            let incoming = 
                                registry.elements 
                                |> Map.toList 
                                |> List.choose (fun (srcId, srcElem) ->
                                    if srcElem.relationships |> List.exists (fun r -> r.target = id) 
                                    then Some srcId 
                                    else None
                                )
                            outgoing @ incoming
                        | None -> []
                    )
                    |> Set.ofList
                
                let combined = Set.union currentIds nextIds
                collectNeighbors combined (currentDepth + 1)
        
        let allNodeIds = collectNeighbors (Set.singleton elementId) 0
        
        let elements = 
            allNodeIds 
            |> Set.toList 
            |> List.choose (fun id -> Map.tryFind id registry.elements)
        
        let rels = 
            elements 
            |> List.collect (fun elem ->
                elem.relationships 
                |> List.filter (fun rel -> Set.contains rel.target allNodeIds)
                |> List.map (fun rel -> (elem.id, rel))
            )
        
        buildCytoscapeData elements rels
    
    /// Wrap Cytoscape diagram in HTML with interactive controls
    let private wrapCytoscapeHtml (title: string) (data: string) (enableSave: bool) : string =
        // Note: enableSave parameter kept for API compatibility
        let _enableSave = enableSave
        sprintf """<!DOCTYPE html
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>%s</title>
    <link rel="stylesheet" href="/css/cytoscape-diagram.css" />
    <script src="https://cdn.jsdelivr.net/npm/cytoscape@3.26.0/dist/cytoscape.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/dagre@0.8.5/dist/dagre.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/cytoscape-dagre@2.5.0/cytoscape-dagre.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/lodash@4.17.21/lodash.min.js"></script>
</head>
<body>
    <div id="cy"></div>
    
    <div class="controls">
        <button id="fitView">Fit to View</button>
        <button id="zoomIn">Zoom In</button>
        <button id="zoomOut">Zoom Out</button>
        <button id="resetLayout">Reset Layout</button>
        <button id="exportPNG">Export PNG</button>
    </div>
    
    <div class="legend">
        <h4>Relationships</h4>
        <div class="legend-item">
            <div class="legend-line" style="background: #0066cc;"></div>
            <span>Serving</span>
        </div>
        <div class="legend-item">
            <div class="legend-line" style="background: #cc6600;"></div>
            <span>Realization</span>
        </div>
        <div class="legend-item">
            <div class="legend-line" style="background: #333333;"></div>
            <span>Composition</span>
        </div>
        <div class="legend-item">
            <div class="legend-line" style="background: #cc3366; border-top: 2px dashed;"></div>
            <span>Influence</span>
        </div>
    </div>
    
    <script>
        const graphData = %s;
    </script>
    <script src="/js/cytoscape-diagram.js"></script>
</body>
</html>""" title data
    
    /// Wrap mermaid diagram in HTML with zoom controls
    let private wrapMermaidHtml (title: string) (diagram: string) : string =
        sprintf """<!DOCTYPE html
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>%s</title>
    <link rel="stylesheet" href="/css/mermaid-diagram.css" />
    <script src="https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/svg-pan-zoom@3.6.1/dist/svg-pan-zoom.min.js"></script>
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
    <script src="/js/mermaid-diagram.js"></script>
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
