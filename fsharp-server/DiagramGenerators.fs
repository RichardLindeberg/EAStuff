namespace EAArchive

open System
open System.Collections.Generic
open System.Globalization
open System.IO
open System.Net
open System.Xml.Linq

module DiagramGenerators =

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

    let buildLayerMermaid (layer: string) (registry: ElementRegistry) =
        let elements = ElementRegistry.getLayerElements layer registry
        let elementMap = registry.elements

        let lines = List<string>()
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

        String.Join("\n", lines)

    let buildContextDiagram (elementId: string) (depth: int) (registry: ElementRegistry) =
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

        let lines = List<string>()
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

        String.Join("\n", lines)

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

    let buildLayerCytoscape (layer: string) (registry: ElementRegistry) : string =
        let layerElements = ElementRegistry.getLayerElements layer registry

        // Collect all relationships involving layer elements
        let allRels =
            registry.elements
            |> Map.toList
            |> List.collect (fun (id, elem) ->
                if layerElements |> List.exists (fun le -> le.id = id) then
                    elem.relationships |> List.map (fun rel -> (id, rel))
                else [])

        // Include related elements from other layers
        let relatedIds = allRels |> List.map (fun (_, rel) -> rel.target) |> Set.ofList
        let relatedElements =
            registry.elements
            |> Map.toList
            |> List.choose (fun (id, elem) ->
                if Set.contains id relatedIds && not (layerElements |> List.exists (fun le -> le.id = id))
                then Some elem
                else None)

        let allElements = layerElements @ relatedElements

        buildCytoscapeData allElements allRels

    let buildContextCytoscape (elementId: string) (depth: int) (registry: ElementRegistry) : string =
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
                                    else None)
                            outgoing @ incoming
                        | None -> [])
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
                |> List.map (fun rel -> (elem.id, rel)))

        buildCytoscapeData elements rels

    /// Wrap Cytoscape diagram in HTML with interactive controls
    let wrapCytoscapeHtml (title: string) (data: string) (enableSave: bool) : string =
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
    let wrapMermaidHtml (title: string) (diagram: string) : string =
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