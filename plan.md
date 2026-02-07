# Cytoscape.js Implementation Specification

**Document Version:** 1.0  
**Date:** February 4, 2026  
**Target:** F# Server Interactive ArchiMate Diagrams

## Overview

Migrate the F# server's diagram generation from Mermaid to Cytoscape.js to enable interactive node repositioning, ArchiMate-compliant relationship styling with custom colors, and position persistence. This addresses limitations in Mermaid that prevent manual layout control and per-relationship color customization.

## Goals

1. **Manual Node Positioning**: Enable users to drag and reposition nodes without affecting other nodes (no force-directed physics)
2. **Position Persistence**: Save and restore user-defined node positions across sessions
3. **ArchiMate-Compliant Styling**: Apply correct node shapes, colors, and relationship arrow styles per ArchiMate 3.2 standard
4. **Custom Relationship Colors**: Color-code relationships by type (serving, realization, composition, etc.)
5. **Maintain Existing Features**: Preserve SVG icons, clickable navigation, layer colors, and zoom/pan functionality
6. **Performance**: Handle large diagrams (100+ nodes) efficiently

## Current State Analysis

### Existing Implementation (Mermaid-based)

**File:** `src/fsharp-server/Handlers.fs`

**Key Functions:**
- `buildLayerMermaid` (lines 97-172): Generates layer-specific diagrams
- `buildContextDiagram` (lines 174-268): Generates element context diagrams
- `wrapMermaidHtml` (lines 271-469): Wraps Mermaid in HTML with svg-pan-zoom
- `buildMermaidLabel` (lines 44-51): Creates HTML labels with SVG icons
- `getArchimateColor` (lines 54-94): Maps element types to ArchiMate colors

**Current Features:**
- ✅ ArchiMate 3.2 standard colors for nodes
- ✅ SVG icons from `/assets/archimate-symbols/`
- ✅ Clickable nodes linking to element detail pages
- ✅ Zoom/pan with svg-pan-zoom library
- ✅ Layer-based and context-based diagram views

**Mermaid Limitations:**
- ❌ No manual node repositioning (automatic Dagre layout only)
- ❌ No per-relationship colors (all use same gray #333)
- ❌ Limited arrow customization (basic types only)
- ❌ No position persistence
- ❌ Performance issues with large diagrams

## Technical Requirements

### Data Structures

#### Cytoscape JSON Format

**Nodes:**
```json
{
  "data": {
    "id": "bus-proc-001",
    "label": "Customer Onboarding",
    "type": "business-process",
    "color": "#ffffb5",
    "icon": "/assets/archimate-symbols/Business Process.svg"
  },
  "position": {
    "x": 100,
    "y": 200
  },
  "classes": "business-process",
  "grabbable": true
}
```

**Edges:**
```json
{
  "data": {
    "id": "rel-001",
    "source": "bus-proc-001",
    "target": "app-comp-001",
    "label": "serving",
    "relType": "serving",
    "color": "#0066cc",
    "arrowType": "triangle"
  }
}
```

### ArchiMate Styling Rules

#### Element Type → Node Shape Mapping

| Element Type | Cytoscape Shape | Description |
|--------------|-----------------|-------------|
| Business Process, Function, Interaction, Event, Service | `roundrectangle` | Behavior elements |
| Business Actor, Role, Collaboration, Interface | `rectangle` | Structure elements |
| Business Object, Representation, Product, Contract | `rectangle` | Passive structure |
| Application Component, Interface, Service, Function | `roundrectangle` | Application elements |
| Node, Device, System Software, Technology Interface | `rectangle` | Technology elements |
| Goal, Outcome, Principle, Requirement, Constraint | `hexagon` | Motivation elements |
| Stakeholder, Driver, Assessment, Value | `ellipse` | Motivation stakeholders |
| Capability, Resource, Course of Action, Value Stream | `roundrectangle` | Strategy elements |

#### Element Type → Color Mapping (ArchiMate 3.2)

| Layer | Color Code | Elements |
|-------|------------|----------|
| Strategy | `#f5deaa` | Capability, Resource, Course of Action, Value Stream |
| Business | `#ffffb5` | Actor, Role, Process, Function, Service, Object, etc. |
| Application | `#b5ffff` | Component, Interface, Service, Function, Data Object |
| Technology | `#c9e7b7` | Node, Device, System Software, Network, Artifact, etc. |
| Motivation | `#ccccff` | Goal, Outcome, Principle, Requirement, Driver, etc. |
| Implementation | `#ffe0e0` | Work Package, Deliverable, Implementation Event, Plateau |

#### Relationship Type → Arrow Style Mapping

| Relationship Type | Arrow Style | Color | Line Style | Description |
|-------------------|-------------|-------|------------|-------------|
| `composition` | `triangle` | `#333333` | `solid, width: 2.5px` | Strong ownership |
| `aggregation` | `diamond` | `#666666` | `solid, width: 2px` | Weak ownership |
| `assignment` | `triangle` | `#9933cc` | `dashed` | Resource allocation |
| `realization` | `triangle-tee` | `#cc6600` | `solid, width: 2.5px` | Implements/fulfills |
| `serving` | `triangle` | `#0066cc` | `solid` | Provides service |
| `access` | `vee` | `#00cc66` | `solid` | Read/write access |
| `influence` | `vee` | `#cc3366` | `dashed` | Affects outcome |
| `triggering` | `triangle` | `#0099cc` | `solid` | Causes activation |
| `flow` | `triangle` | `#0099cc` | `solid` | Data/process flow |
| `specialization` | `triangle-tee` | `#666666` | `solid` | Is-a relationship |
| `association` | `none` | `#999999` | `solid` | Generic connection |

### F# Code Changes

#### 1. Add Relationship Color Mapping

**Location:** `src/fsharp-server/Handlers.fs` (add after `getArchimateColor` function)

```fsharp
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
```

#### 2. Add Node Shape Mapping

**Location:** `src/fsharp-server/Handlers.fs` (add after relationship functions)

```fsharp
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
```

#### 3. Create Cytoscape Data Builder

**Location:** `src/fsharp-server/Handlers.fs` (replace `buildLayerMermaid`)

```fsharp
let private buildCytoscapeData (elements: Element list) (allRelationships: (string * Relationship) list) : string =
    let nodes = 
        elements 
        |> List.map (fun elem ->
            let color = getArchimateColor elem.elementType
            let shape = getNodeShape elem.elementType
            let icon = 
                match tryGetSymbolFileName elem.elementType with
                | Some fileName -> sprintf "%s%s" symbolsBaseUrl fileName
                | None -> ""
            
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
                (escapeMermaidLabel elem.name)
                elem.elementType
                color
                icon
                shape
                (sanitizeMermaidId elem.elementType)
        )
    
    let edges = 
        allRelationships
        |> List.map (fun (sourceId, rel) ->
            let color = getRelationshipColor rel.relationType
            let arrowType = getRelationshipArrowType rel.relationType
            let lineStyle = getRelationshipLineStyle rel.relationType
            let lineWidth = getRelationshipLineWidth rel.relationType
            
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
                rel.relationType
                rel.relationType
                color
                arrowType
                lineStyle
                lineWidth
        )
    
    sprintf """{ "nodes": [%s], "edges": [%s] }""" 
        (String.concat "," nodes) 
        (String.concat "," edges)
```

#### 4. Update Layer Diagram Function

**Location:** `src/fsharp-server/Handlers.fs` (replace `buildLayerMermaid`)

```fsharp
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
```

#### 5. Update Context Diagram Function

**Location:** `src/fsharp-server/Handlers.fs` (replace `buildContextDiagram`)

```fsharp
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
```

#### 6. Create Cytoscape HTML Wrapper

**Location:** `src/fsharp-server/Handlers.fs` (replace `wrapMermaidHtml`)

```fsharp
let private wrapCytoscapeHtml (title: string) (data: string) (enableSave: bool) : string =
    let saveScript = 
        if enableSave then
            """
            // Save positions to localStorage
            let savePositions = () => {
                const positions = {};
                cy.nodes().forEach(node => {
                    positions[node.id()] = node.position();
                });
                localStorage.setItem('cytoscape_positions_' + document.title, JSON.stringify(positions));
            };
            
            // Load positions from localStorage
            const savedPositions = localStorage.getItem('cytoscape_positions_' + document.title);
            if (savedPositions) {
                const positions = JSON.parse(savedPositions);
                cy.nodes().forEach(node => {
                    if (positions[node.id()]) {
                        node.position(positions[node.id()]);
                    }
                });
            }
            
            // Save on position change
            cy.on('position', 'node', _.debounce(savePositions, 500));
            
            // Reset button
            document.getElementById('resetLayout').addEventListener('click', () => {
                localStorage.removeItem('cytoscape_positions_' + document.title);
                cy.layout({ name: 'dagre', rankDir: 'TB', nodeSep: 80, rankSep: 100 }).run();
            });
            """
        else ""
    
    sprintf """<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>%s</title>
    <script src="https://cdn.jsdelivr.net/npm/cytoscape@3.26.0/dist/cytoscape.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/dagre@0.8.5/dist/dagre.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/cytoscape-dagre@2.5.0/cytoscape-dagre.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/lodash@4.17.21/lodash.min.js"></script>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        #cy {
            width: 100%%;
            height: 100vh;
            background-color: #fafafa;
        }
        .controls {
            position: absolute;
            top: 20px;
            right: 20px;
            z-index: 1000;
            background: white;
            padding: 15px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        .controls button {
            display: block;
            width: 100%%;
            margin: 5px 0;
            padding: 8px 15px;
            border: none;
            background: #0066cc;
            color: white;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }
        .controls button:hover {
            background: #0052a3;
        }
        .legend {
            position: absolute;
            bottom: 20px;
            left: 20px;
            z-index: 1000;
            background: white;
            padding: 15px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            font-size: 12px;
        }
        .legend h4 {
            margin: 0 0 10px 0;
            font-size: 14px;
        }
        .legend-item {
            display: flex;
            align-items: center;
            margin: 5px 0;
        }
        .legend-line {
            width: 30px;
            height: 2px;
            margin-right: 10px;
        }
    </style>
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
        
        const cy = cytoscape({
            container: document.getElementById('cy'),
            
            elements: {
                nodes: graphData.nodes,
                edges: graphData.edges
            },
            
            style: [
                {
                    selector: 'node',
                    style: {
                        'label': 'data(label)',
                        'text-valign': 'center',
                        'text-halign': 'center',
                        'text-wrap': 'wrap',
                        'text-max-width': '100px',
                        'font-size': '12px',
                        'font-weight': 'normal',
                        'background-color': 'data(color)',
                        'background-image': 'data(icon)',
                        'background-fit': 'contain',
                        'background-clip': 'none',
                        'background-width': '32px',
                        'background-height': '32px',
                        'background-offset-y': '-15px',
                        'border-width': 2,
                        'border-color': '#333',
                        'shape': 'data(shape)',
                        'width': 120,
                        'height': 80,
                        'padding': '10px',
                        'text-margin-y': 20
                    }
                },
                {
                    selector: 'node:selected',
                    style: {
                        'border-width': 3,
                        'border-color': '#0066cc',
                        'overlay-opacity': 0.2,
                        'overlay-color': '#0066cc'
                    }
                },
                {
                    selector: 'edge',
                    style: {
                        'width': 'data(lineWidth)',
                        'line-color': 'data(color)',
                        'target-arrow-color': 'data(color)',
                        'target-arrow-shape': 'data(arrowType)',
                        'curve-style': 'bezier',
                        'label': 'data(label)',
                        'font-size': '10px',
                        'text-rotation': 'autorotate',
                        'text-margin-y': -10,
                        'line-style': 'data(lineStyle)',
                        'line-dash-pattern': [5, 5],
                        'arrow-scale': 1.2
                    }
                },
                {
                    selector: 'edge:selected',
                    style: {
                        'line-color': '#0066cc',
                        'target-arrow-color': '#0066cc',
                        'width': 3
                    }
                }
            ],
            
            layout: {
                name: 'dagre',
                rankDir: 'TB',
                nodeSep: 80,
                rankSep: 100,
                animate: true,
                animationDuration: 500
            },
            
            minZoom: 0.2,
            maxZoom: 3,
            wheelSensitivity: 0.2
        });
        
        // Click handler for navigation
        cy.on('tap', 'node', function(evt) {
            const nodeId = evt.target.id();
            window.location.href = '/elements/' + nodeId;
        });
        
        // Control buttons
        document.getElementById('fitView').addEventListener('click', () => {
            cy.fit(null, 50);
        });
        
        document.getElementById('zoomIn').addEventListener('click', () => {
            cy.zoom(cy.zoom() * 1.2);
        });
        
        document.getElementById('zoomOut').addEventListener('click', () => {
            cy.zoom(cy.zoom() * 0.8);
        });
        
        document.getElementById('exportPNG').addEventListener('click', () => {
            const png = cy.png({ full: true, scale: 2 });
            const link = document.createElement('a');
            link.download = 'diagram.png';
            link.href = png;
            link.click();
        });
        
        %s
        
        // Highlight connected nodes on hover
        cy.on('mouseover', 'node', function(evt) {
            const node = evt.target;
            const connectedEdges = node.connectedEdges();
            const connectedNodes = connectedEdges.connectedNodes();
            
            cy.elements().style('opacity', 0.3);
            node.style('opacity', 1);
            connectedNodes.style('opacity', 1);
            connectedEdges.style('opacity', 1);
        });
        
        cy.on('mouseout', 'node', function() {
            cy.elements().style('opacity', 1);
        });
    </script>
</body>
</html>""" title data saveScript
```

#### 7. Update HTTP Handlers

**Location:** `src/fsharp-server/Handlers.fs`

```fsharp
let layerDiagramCytoscape (layer: string) : HttpHandler =
    fun next ctx ->
        let logger = ctx.GetLogger("Handlers")
        let registry = ctx.GetService<ElementRegistry>()
        
        try
            let data = buildLayerCytoscape layer registry
            let html = wrapCytoscapeHtml (sprintf "%s Layer" layer) data true
            ctx.WriteHtmlStringAsync html
        with ex ->
            logger.LogError(ex, "Error generating Cytoscape layer diagram for {Layer}", layer)
            RequestErrors.INTERNAL_ERROR "Failed to generate diagram" next ctx

let contextDiagramCytoscape (elementId: string) (depth: int) : HttpHandler =
    fun next ctx ->
        let logger = ctx.GetLogger("Handlers")
        let registry = ctx.GetService<ElementRegistry>()
        
        try
            let data = buildContextCytoscape elementId depth registry
            let html = wrapCytoscapeHtml (sprintf "Context: %s" elementId) data false
            ctx.WriteHtmlStringAsync html
        with ex ->
            logger.LogError(ex, "Error generating Cytoscape context diagram for {ElementId}", elementId)
            RequestErrors.INTERNAL_ERROR "Failed to generate diagram" next ctx
```

#### 8. Update Routes

**Location:** `src/fsharp-server/Program.fs`

```fsharp
let webApp =
    choose [
        GET >=> choose [
            route "/" >=> Views.indexView
            routef "/elements/%s" Views.elementDetailView
            
            // Cytoscape diagrams (new default)
            routef "/diagrams/layer/%s" Handlers.layerDiagramCytoscape
            routef "/diagrams/context/%s/%i" Handlers.contextDiagramCytoscape
            
            // Legacy Mermaid diagrams (keep for backwards compatibility)
            routef "/diagrams/layer/%s/mermaid" Handlers.layerDiagramMermaid
            routef "/diagrams/context/%s/%i/mermaid" Handlers.contextDiagramMermaid
        ]
        setStatusCode 404 >=> text "Not Found"
    ]
```

## Implementation Phases

### Phase 1: Core Cytoscape Integration
**Goal:** Get basic Cytoscape diagrams rendering

**Tasks:**
1. Add relationship color/arrow/style mapping functions
2. Add node shape mapping function
3. Create `buildCytoscapeData` function
4. Update `buildLayerCytoscape` and `buildContextCytoscape` functions
5. Create `wrapCytoscapeHtml` function (without position persistence initially)
6. Add new routes for Cytoscape endpoints
7. Test basic rendering of layer and context diagrams

**Validation:**
- [ ] Layer diagrams render with Cytoscape
- [ ] Context diagrams render with Cytoscape
- [ ] Nodes show correct ArchiMate colors
- [ ] Relationships show correct colors
- [ ] Nodes are draggable
- [ ] Zoom/pan works

### Phase 2: ArchiMate Styling
**Goal:** Apply full ArchiMate-compliant styling

**Tasks:**
1. Test and refine node shape mapping for all element types
2. Verify relationship arrow types match ArchiMate standard
3. Integrate SVG icons from `wwwroot/assets/archimate-symbols/`
4. Apply correct line styles (solid/dashed)
5. Tune node sizes, padding, label positioning
6. Add relationship type legend to UI

**Validation:**
- [ ] All element types have correct shapes
- [ ] All relationship types have correct arrows
- [ ] SVG icons display correctly in nodes
- [ ] Colors match ArchiMate 3.2 standard
- [ ] Labels are readable and properly positioned
- [ ] Legend shows relationship types

### Phase 3: Position Persistence
**Goal:** Enable save/restore of user-defined layouts

**Tasks:**
1. Add localStorage save on node position change (debounced)
2. Add localStorage load on page init
3. Add "Reset Layout" button to clear saved positions
4. Test persistence across browser sessions
5. Handle multiple diagrams (unique keys per diagram)

**Validation:**
- [ ] Node positions persist after refresh
- [ ] "Reset Layout" restores automatic layout
- [ ] Different diagrams have independent layouts
- [ ] Position data is reasonable size in localStorage

### Phase 4: Enhanced Interactivity
**Goal:** Add polish and usability features

**Tasks:**
1. Add "Export PNG" functionality
2. Add hover highlighting of connected nodes
3. Add selection highlighting
4. Optimize performance for large diagrams (100+ nodes)
5. Add loading indicator for slow renders
6. Test touch/mobile interaction

**Validation:**
- [ ] PNG export works correctly
- [ ] Hover highlights connections clearly
- [ ] Selection styling is distinct
- [ ] Large diagrams (100+ nodes) render smoothly
- [ ] Works on tablet/touch devices

### Phase 5: Backwards Compatibility & Cleanup
**Goal:** Finalize migration

**Tasks:**
1. Keep Mermaid routes as `/diagrams/.../mermaid` fallback
2. Update documentation to reference Cytoscape as primary
3. Update element detail pages to link to Cytoscape diagrams
4. Test all existing functionality still works
5. Performance testing and optimization

**Validation:**
- [ ] All element detail pages link to Cytoscape diagrams
- [ ] Legacy Mermaid endpoints still accessible
- [ ] Documentation updated
- [ ] No broken links in application
- [ ] Performance meets requirements

## Testing Checklist

### Functional Testing
- [ ] Layer diagrams render for all layers (Strategy, Business, Application, Technology, Motivation, Implementation)
- [ ] Context diagrams render with depth 1, 2, and 3
- [ ] Nodes can be dragged individually
- [ ] Other nodes do not move when dragging one node
- [ ] Node positions persist after page refresh
- [ ] "Reset Layout" button restores automatic layout
- [ ] "Fit to View" button works correctly
- [ ] Zoom In/Out buttons work
- [ ] Mouse wheel zoom works
- [ ] Pan by dragging background works
- [ ] Clicking nodes navigates to element detail pages
- [ ] "Export PNG" generates valid image
- [ ] Hover highlights connected nodes
- [ ] Selection styling appears on selected nodes/edges

### Visual/Styling Testing
- [ ] ArchiMate colors correct for all element types
- [ ] Node shapes match element type requirements
- [ ] SVG icons display in nodes
- [ ] Icons are properly sized and positioned
- [ ] Labels are readable (not overlapping, good contrast)
- [ ] Relationship colors match relationship types
- [ ] Arrow types match relationship types (triangle, diamond, vee, etc.)
- [ ] Dashed lines appear for assignment/influence
- [ ] Line widths appropriate for relationship types
- [ ] Legend shows correct relationship types

### Performance Testing
- [ ] Diagrams with 10 nodes render instantly
- [ ] Diagrams with 50 nodes render within 1 second
- [ ] Diagrams with 100 nodes render within 2 seconds
- [ ] Diagrams with 200+ nodes render within 5 seconds
- [ ] Dragging is smooth (no lag)
- [ ] Zoom is smooth
- [ ] No memory leaks on repeated renders

### Browser Compatibility
- [ ] Works in Chrome
- [ ] Works in Firefox
- [ ] Works in Edge
- [ ] Works in Safari
- [ ] Responsive on mobile (basic functionality)
- [ ] Touch gestures work on tablets

### Integration Testing
- [ ] Element detail pages link to correct diagrams
- [ ] Navigation between elements works
- [ ] Element registry properly loaded
- [ ] Relationships correctly retrieved
- [ ] No 404 errors for assets (SVG icons, scripts)
- [ ] Console has no errors

## Configuration

### Required CDN Libraries
```html
<script src="https://cdn.jsdelivr.net/npm/cytoscape@3.26.0/dist/cytoscape.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/dagre@0.8.5/dist/dagre.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/cytoscape-dagre@2.5.0/cytoscape-dagre.js"></script>
<script src="https://cdn.jsdelivr.net/npm/lodash@4.17.21/lodash.min.js"></script>
```

### SVG Icon Directory
- Location: `src/fsharp-server/wwwroot/assets/archimate-symbols/`
- Format: `{Element Type Title Case}.svg` (e.g., `Business Process.svg`)
- Size: 32x32px recommended
- Style: ArchiMate standard symbols

### localStorage Keys
- Pattern: `cytoscape_positions_{diagram_title}`
- Format: JSON object `{ "element-id": { "x": 100, "y": 200 }, ... }`
- Cleanup: User-initiated via "Reset Layout" button

## Success Criteria

1. ✅ Users can manually reposition nodes without affecting other nodes
2. ✅ Node positions persist across browser sessions
3. ✅ Relationships display with correct ArchiMate colors
4. ✅ All ArchiMate element types have appropriate shapes
5. ✅ SVG icons display correctly in nodes
6. ✅ Navigation from diagrams to element details works
7. ✅ Performance is acceptable for diagrams up to 200 nodes
8. ✅ Diagrams work in all major browsers
9. ✅ Legacy Mermaid endpoints remain functional
10. ✅ No regressions in existing functionality

## References

- **Cytoscape.js Documentation**: https://js.cytoscape.org/
- **ArchiMate 3.2 Standard**: https://pubs.opengroup.org/architecture/archimate32-doc/
- **ArchiMate Visual Reference**: https://gbruneau.github.io/ArchiMate/
- **Dagre Layout Algorithm**: https://github.com/dagrejs/dagre
- **Current F# Server Code**: `src/fsharp-server/Handlers.fs`, `src/fsharp-server/Program.fs`
