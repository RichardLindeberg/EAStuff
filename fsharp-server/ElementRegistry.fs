namespace EAArchive

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions
open YamlDotNet.Serialization
open Microsoft.Extensions.Logging

/// Registry for all elements and relationships
type ElementRegistry = {
    elements: Map<string, Element>
    elementsByLayer: Map<string, string list>
    incomingRelations: Map<string, (string * string * string) list> // target -> [(source, relType, desc)]
}

module ElementRegistry =
    
    /// Parse YAML frontmatter from a markdown file
    let parseFrontmatter (content: string) : (Map<string, obj> * string) =
        let lines = content.Split('\n')
        if lines.Length > 0 && lines.[0].Trim() = "---" then
            let mutable endIdx = -1
            for i in 1..lines.Length - 1 do
                if lines.[i].Trim() = "---" then
                    endIdx <- i
            
            if endIdx > 1 then
                let yamlContent = String.concat "\n" lines.[1..endIdx-1]
                let markdownContent = 
                    if endIdx + 1 < lines.Length 
                    then String.concat "\n" lines.[endIdx+1..] 
                    else ""
                
                try
                    let deserializer = DeserializerBuilder().Build()
                    let data = deserializer.Deserialize<Dictionary<string, obj>>(yamlContent)
                    (data |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Map.ofSeq, markdownContent)
                with
                | _ -> (Map.empty, content)
            else
                (Map.empty, content)
        else
            (Map.empty, content)
    
    /// Extract value from metadata with type safety
    let getString (key: string) (metadata: Map<string, obj>) : string option =
        metadata 
        |> Map.tryFind key
        |> Option.bind (fun v -> 
            match v with
            | :? string as s -> Some s
            | _ -> 
                try Some (v.ToString()) 
                with _ -> None
        )
    
    let getStringList (key: string) (metadata: Map<string, obj>) : string list =
        metadata 
        |> Map.tryFind key
        |> Option.map (fun v ->
            match v with
            | :? string as s -> 
                s.Split(',')
                |> Array.map (fun t -> t.Trim())
                |> Array.toList
            | :? System.Collections.Generic.List<obj> as lst ->
                lst
                |> Seq.map (fun x -> x.ToString())
                |> Seq.toList
            | :? System.Collections.Generic.List<string> as lst ->
                List.ofSeq lst
            | _ -> []
        )
        |> Option.defaultValue []
    
    /// Parse relationships from metadata
    let parseRelationships (metadata: Map<string, obj>) : Relationship list =
        metadata
        |> Map.tryFind "relationships"
        |> Option.bind (fun v ->
            match v with
            | :? System.Collections.Generic.List<obj> as list ->
                try
                    list
                    |> Seq.map (fun item ->
                        match item with
                        | :? Dictionary<string, obj> as dict ->
                            {
                                target = dict.TryGetValue("target") |> fun (ok, v) -> if ok then v.ToString() else ""
                                relationType = dict.TryGetValue("type") |> fun (ok, v) -> if ok then v.ToString() else ""
                                description = dict.TryGetValue("description") |> fun (ok, v) -> if ok then v.ToString() else ""
                            }
                        | _ -> { target = ""; relationType = ""; description = "" }
                    )
                    |> Seq.filter (fun r -> r.target <> "")
                    |> Seq.toList
                    |> Some
                with _ -> None
            | _ -> None
        )
        |> Option.defaultValue []
    
    /// Load and parse a single markdown element file
    let loadElement (filePath: string) (logger: ILogger) : Element option =
        try
            let content = File.ReadAllText(filePath)
            let (metadata, mdContent) = parseFrontmatter content
            
            let id = getString "id" metadata |> Option.defaultValue ""
            if id = "" then 
                logger.LogWarning($"Element in {filePath} has no ID, skipping")
                None
            else
                let name = getString "name" metadata |> Option.defaultValue "Unnamed"
                let layer = getString "layer" metadata |> Option.defaultValue "unknown"
                logger.LogDebug($"Loaded element: {id} ({name}) in layer {layer} from {filePath}")
                let element = {
                    id = id
                    name = name
                    elementType = getString "type" metadata |> Option.defaultValue "unknown"
                    layer = layer
                    content = mdContent.Trim()
                    properties = metadata
                    tags = getStringList "tags" metadata
                    relationships = parseRelationships metadata
                }
                Some element
        with ex ->
            logger.LogError($"Error loading element from {filePath}: {ex.Message}")
            None
    
    /// Build the element registry from all markdown files
    let create (elementsPath: string) : ElementRegistry =
        let logger = LoggerFactory.Create(fun builder -> builder.AddConsole() |> ignore).CreateLogger("ElementRegistry")
        
        logger.LogInformation($"Creating element registry from: {elementsPath}")
        
        let mutable elements = Map.empty
        let mutable elementsByLayer = Map.empty
        let mutable incomingRelations = Map.empty
        
        if Directory.Exists(elementsPath) then
            let mdFiles = 
                Directory.EnumerateFiles(elementsPath, "*.md", SearchOption.AllDirectories)
                |> Seq.toList
            
            logger.LogInformation($"Found {mdFiles.Length} markdown files")
            
            // Load all elements
            mdFiles
            |> List.choose (fun filePath -> loadElement filePath logger)
            |> List.iter (fun elem ->
                elements <- elements |> Map.add elem.id elem
                
                // Index by layer
                let layerElems = 
                    elementsByLayer 
                    |> Map.tryFind elem.layer 
                    |> Option.defaultValue []
                elementsByLayer <- elementsByLayer |> Map.add elem.layer (elem.id :: layerElems)
                
                // Index incoming relationships
                elem.relationships
                |> List.iter (fun rel ->
                    let existing = 
                        incomingRelations 
                        |> Map.tryFind rel.target 
                        |> Option.defaultValue []
                    incomingRelations <- 
                        incomingRelations 
                        |> Map.add rel.target ((elem.id, rel.relationType, rel.description) :: existing)
                )
            )
            
            logger.LogInformation($"Successfully loaded {Map.count elements} elements into registry")
        else
            logger.LogError($"Elements directory does not exist: {elementsPath}")
        
        {
            elements = elements
            elementsByLayer = elementsByLayer
            incomingRelations = incomingRelations
        }
    
    /// Get an element by ID
    let getElement (id: string) (registry: ElementRegistry) : Element option =
        Map.tryFind id registry.elements
    
    /// Get all elements in a layer
    let getLayerElements (layer: string) (registry: ElementRegistry) : Element list =
        registry.elementsByLayer
        |> Map.tryFind layer
        |> Option.defaultValue []
        |> List.map (fun id -> Map.find id registry.elements)
        |> List.sortBy (fun e -> e.name)
    
    /// Get incoming relationships for an element
    let getIncomingRelations (elemId: string) (registry: ElementRegistry) : (Element * Relationship) list =
        registry.incomingRelations
        |> Map.tryFind elemId
        |> Option.defaultValue []
        |> List.map (fun (sourceId, relType, desc) ->
            match Map.tryFind sourceId registry.elements with
            | Some elem ->
                let rel = { target = elemId; relationType = relType; description = desc }
                (elem, rel)
            | None -> 
                let dummy = { 
                    id = sourceId
                    name = sourceId
                    elementType = "unknown"
                    layer = "unknown"
                    content = ""
                    properties = Map.empty
                    tags = []
                    relationships = []
                }
                (dummy, { target = elemId; relationType = relType; description = desc })
        )
    
    /// Build element with its relationships
    let withRelations (elem: Element) (registry: ElementRegistry) : ElementWithRelations =
        let outgoing =
            elem.relationships
            |> List.choose (fun rel ->
                getElement rel.target registry
                |> Option.map (fun targetElem -> (targetElem, rel))
            )
        
        let incoming = getIncomingRelations elem.id registry
        
        {
            element = elem
            incomingRelations = incoming
            outgoingRelations = outgoing
        }
