namespace EAArchive

open System
open System.Collections.Generic
open System.Globalization
open System.IO
open System.Net
open System.Text.Encodings.Web
open System.Text.Json
open System.Xml.Linq

module DiagramGenerators =

    type DiagramAssetConfig =
        { SymbolsPath: string
          IconsPath: string
          SymbolsBaseUrl: string
          IconsBaseUrl: string }

    let private toTitleCase (value: string) =
        let normalized = value.Replace("-", " ").Replace("_", " ").ToLowerInvariant()
        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(normalized)

    let private elementTypeToKey (elementType: ElementType) : string =
        match elementType with
        | ElementType.Strategy st ->
            match st with
            | StrategyElement.Resource -> "resource"
            | StrategyElement.Capability -> "capability"
            | StrategyElement.ValueStream -> "value-stream"
            | StrategyElement.CourseOfAction -> "course-of-action"
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

    let private getSymbolFile (assets: DiagramAssetConfig) (elementType: string) : (string * string) option =
        if String.IsNullOrWhiteSpace assets.SymbolsPath then None
        else
            let fileName = toTitleCase elementType + ".svg"
            let fullPath = Path.Combine(assets.SymbolsPath, fileName)
            if File.Exists(fullPath) then Some (fileName, fullPath) else None

    let private tryGetSymbolFileName (assets: DiagramAssetConfig) (elementType: string) : string option =
        getSymbolFile assets elementType |> Option.map fst

    let private iconViewBox = "120 0 30 30"
    let private iconSizePx = "12"

    let private ensureIconFile (iconsRoot: string) (fileName: string) (sourcePath: string) : bool =
        try
            if String.IsNullOrWhiteSpace iconsRoot then false
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

    let private tryGetIconFileName (assets: DiagramAssetConfig) (elementType: string) : string option =
        match getSymbolFile assets elementType with
        | Some (fileName, sourcePath) ->
            if ensureIconFile assets.IconsPath fileName sourcePath then Some fileName else None
        | None -> None

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

    let private governanceNodeColor = "#e0e7ff"
    let private governanceEdgeColor = "#5a67d8"

    let private getGovernanceDocTypeLabel (docType: GovernanceDocType) : string =
        match docType with
        | GovernanceDocType.Policy -> "Policy"
        | GovernanceDocType.Instruction -> "Instruction"
        | GovernanceDocType.Manual -> "Manual"
        | GovernanceDocType.Unknown value -> value

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

    // ========================================
    // Cytoscape.js Diagram Generation
    // ========================================

    type CytoscapeNodeData = {
        id: string
        label: string
        ``type``: string
        color: string
        icon: string
        shape: string
        kind: string
        slug: string
    }

    type CytoscapeNode = {
        data: CytoscapeNodeData
        classes: string
    }

    type CytoscapeEdgeData = {
        id: string
        source: string
        target: string
        label: string
        relType: string
        color: string
        arrowType: string
        lineStyle: string
        lineWidth: float
        kind: string
    }

    type CytoscapeEdge = {
        data: CytoscapeEdgeData
        classes: string
    }

    let private buildCytoscapeData (nodes: CytoscapeNode list) (edges: CytoscapeEdge list) : string =
        let graph = {| nodes = nodes; edges = edges |}
        let options = JsonSerializerOptions(Encoder = JavaScriptEncoder.Default)
        JsonSerializer.Serialize(graph, options)

    let private buildArchNodes (assets: DiagramAssetConfig) (elements: Element list) : CytoscapeNode list =
        elements
        |> List.map (fun elem ->
            let elementTypeKey = elementTypeToKey elem.elementType
            let color = getArchimateColor elementTypeKey
            let shape = getNodeShape elementTypeKey
            let icon =
                match tryGetIconFileName assets elementTypeKey with
                | Some fileName -> sprintf "%s%s" assets.IconsBaseUrl fileName
                | None ->
                    match tryGetSymbolFileName assets elementTypeKey with
                    | Some fileName -> sprintf "%s%s" assets.SymbolsBaseUrl fileName
                    | None -> ""

            let typeLabel = getElementTypeLabel elementTypeKey
            let labelText = sprintf "[%s]\n%s" typeLabel elem.name
            let classes = "arch-node " + elementTypeKey.Replace("-", "_") + " badge-label"

            {
                data =
                    { id = elem.id
                      label = labelText
                      ``type`` = elementTypeKey
                      color = color
                      icon = icon
                      shape = shape
                      kind = "archimate"
                      slug = "" }
                classes = classes
            }
        )

    let private buildArchEdges (allRelationships: (string * Relationship) list) : CytoscapeEdge list =
        allRelationships
        |> List.map (fun (sourceId, rel) ->
            let relationTypeKey = relationTypeToKey rel.relationType
            let color = getRelationshipColor relationTypeKey
            let arrowType = getRelationshipArrowType relationTypeKey
            let lineStyle = getRelationshipLineStyle relationTypeKey
            let lineWidth = getRelationshipLineWidth relationTypeKey

            {
                data =
                    { id = sprintf "%s_%s" sourceId rel.target
                      source = sourceId
                      target = rel.target
                      label = relationTypeKey
                      relType = relationTypeKey
                      color = color
                      arrowType = arrowType
                      lineStyle = lineStyle
                      lineWidth = lineWidth
                      kind = "archimate" }
                classes = "arch-edge"
            }
        )

    let private buildGovernanceDocGraph (doc: GovernanceDocument) (elementIds: Set<string>) : CytoscapeNode list * CytoscapeEdge list =
        let labelText = sprintf "[%s]\n%s" (getGovernanceDocTypeLabel doc.docType) doc.title
        let nodeId = "gov-" + doc.slug
        let nodeData: CytoscapeNodeData =
            { id = nodeId
              label = labelText
              ``type`` = "governance"
              color = governanceNodeColor
              icon = ""
              shape = "roundrectangle"
              kind = "governance"
              slug = doc.slug }

        let node: CytoscapeNode =
            {
                data = nodeData
                classes = "governance-node badge-label"
            }

        let ownerEdges: CytoscapeEdge list =
            match Map.tryFind "owner" doc.metadata with
            | Some ownerId when elementIds.Contains(ownerId) ->
                [
                    {
                        data =
                            { id = sprintf "govowner_%s_%s" doc.slug ownerId
                              source = nodeId
                              target = ownerId
                              label = "owner"
                              relType = "owner"
                              color = governanceEdgeColor
                              arrowType = "triangle"
                              lineStyle = "dashed"
                              lineWidth = 1.5
                              kind = "governance" }
                        classes = "governance-edge"
                    }
                ]
            | _ -> []

        let relationEdges: CytoscapeEdge list =
            doc.relations
            |> List.filter (fun rel -> elementIds.Contains(rel.target))
            |> List.map (fun rel ->
                {
                    data =
                        { id = sprintf "govrel_%s_%s_%s" doc.slug rel.target rel.relationType
                          source = nodeId
                          target = rel.target
                          label = rel.relationType
                          relType = rel.relationType
                          color = governanceEdgeColor
                          arrowType = "triangle"
                          lineStyle = "dashed"
                          lineWidth = 1.5
                          kind = "governance" }
                    classes = "governance-edge"
                }
            )

        [ node ], ownerEdges @ relationEdges

    let private buildGovernanceGraph (elementIds: Set<string>) (governanceRegistry: GovernanceDocRegistry) : CytoscapeNode list * CytoscapeEdge list =
        let docs = governanceRegistry.documents |> Map.toList |> List.map snd
        let relevantDocs =
            docs
            |> List.filter (fun doc ->
                let ownerMatch =
                    match Map.tryFind "owner" doc.metadata with
                    | Some ownerId when elementIds.Contains(ownerId) -> true
                    | _ -> false

                let relationMatch =
                    doc.relations
                    |> List.exists (fun rel -> elementIds.Contains(rel.target))

                ownerMatch || relationMatch
            )

        let nodes, edges =
            relevantDocs
            |> List.map (fun doc -> buildGovernanceDocGraph doc elementIds)
            |> List.fold (fun (nodesAcc: CytoscapeNode list, edgesAcc: CytoscapeEdge list) (docNodes, docEdges) ->
                nodesAcc @ docNodes, edgesAcc @ docEdges
            ) ([], [])

        nodes, edges

    let buildLayerCytoscape (assets: DiagramAssetConfig) (layer: Layer) (registry: ElementRegistry) (governanceRegistry: GovernanceDocRegistry) : string =
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
        let elementIds = allElements |> List.map (fun e -> e.id) |> Set.ofList

        let archNodes = buildArchNodes assets allElements
        let archEdges = buildArchEdges allRels
        let governanceNodes, governanceEdges = buildGovernanceGraph elementIds governanceRegistry

        buildCytoscapeData (archNodes @ governanceNodes) (archEdges @ governanceEdges)

    let buildContextCytoscape (assets: DiagramAssetConfig) (elementId: string) (depth: int) (registry: ElementRegistry) (governanceRegistry: GovernanceDocRegistry) : string =
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

        let elementIds = allNodeIds
        let archNodes = buildArchNodes assets elements
        let archEdges = buildArchEdges rels
        let governanceNodes, governanceEdges = buildGovernanceGraph elementIds governanceRegistry

        buildCytoscapeData (archNodes @ governanceNodes) (archEdges @ governanceEdges)

    let buildGovernanceDocCytoscape (assets: DiagramAssetConfig) (doc: GovernanceDocument) (elementIds: Set<string>) (registry: ElementRegistry) : string =
        let elements =
            elementIds
            |> Set.toList
            |> List.choose (fun id -> Map.tryFind id registry.elements)

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

        let archNodes = buildArchNodes assets elements
        let archEdges = buildArchEdges rels
        let governanceNodes, governanceEdges = buildGovernanceDocGraph doc validElementIds

        buildCytoscapeData (archNodes @ governanceNodes) (archEdges @ governanceEdges)
