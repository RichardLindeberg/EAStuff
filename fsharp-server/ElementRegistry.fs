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
    incomingRelations: Map<string, (string * RelationType * string) list> // target -> [(source, relType, desc)]
    validationErrors: ValidationError list ref  // Using ref for mutability
    elementsPath: string
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
                        | :? System.Collections.IDictionary as dict ->
                            {
                                target = 
                                    if dict.Contains("target") then dict.["target"].ToString() else ""
                                relationType = 
                                    if dict.Contains("type") then 
                                        ElementType.parseRelationType (dict.["type"].ToString())
                                    else 
                                        RelationType.Unknown ""
                                description = 
                                    if dict.Contains("description") then dict.["description"].ToString() else ""
                            }
                        | _ -> 
                            { target = ""; relationType = RelationType.Unknown ""; description = "" }
                    )
                    |> Seq.filter (fun r -> r.target <> "")
                    |> Seq.toList
                    |> Some
                with _ -> None
            | _ -> None
        )
        |> Option.defaultValue []
    
    /// Validate element metadata
    let validateElement (filePath: string) (metadata: Map<string, obj>) : ValidationError list =
        let errors = System.Collections.Generic.List<ValidationError>()
        let id = getString "id" metadata
        
        // Valid layer codes from the standard
        let layerCodes = 
            Map.ofList [
                ("str", "strategy")
                ("bus", "business")
                ("app", "application")
                ("tec", "technology")
                ("phy", "physical")
                ("mot", "motivation")
                ("imp", "implementation")
            ]
        
        // Valid type codes for each layer
        let typeCodes =
            Map.ofList [
                // Strategy Layer
                ("str", ["rsrc"; "capa"; "vstr"; "cact"])
                // Business Layer
                ("bus", ["actr"; "role"; "colab"; "intf"; "proc"; "func"; "intr"; "evnt"; "srvc"; "objt"; "cntr"; "repr"; "prod"])
                // Application Layer
                ("app", ["comp"; "colab"; "intf"; "func"; "intr"; "proc"; "evnt"; "srvc"; "data"])
                // Technology Layer
                ("tec", ["node"; "devc"; "sysw"; "colab"; "intf"; "path"; "netw"; "func"; "proc"; "intr"; "evnt"; "srvc"; "artf"])
                // Physical Layer
                ("phy", ["equi"; "faci"; "dist"; "matr"])
                // Motivation Layer
                ("mot", ["stkh"; "drvr"; "asmt"; "goal"; "outc"; "prin"; "reqt"; "cnst"; "mean"; "valu"])
                // Implementation Layer
                ("imp", ["work"; "delv"; "evnt"; "plat"; "gap_"])
            ]
        
        // Required fields
        if Option.isNone id || (Option.isSome id && (id |> Option.defaultValue "").Trim() = "") then
            errors.Add({
                filePath = filePath
                elementId = None
                errorType = ErrorType.MissingId
                message = "Element must have an 'id' field"
                severity = Severity.Error
            })
        
        if Option.isNone (getString "name" metadata) then
            errors.Add({
                filePath = filePath
                elementId = id
                errorType = ErrorType.MissingRequiredField
                message = "Element must have a 'name' field"
                severity = Severity.Error
            })
        
        if Option.isNone (getString "type" metadata) then
            errors.Add({
                filePath = filePath
                elementId = id
                errorType = ErrorType.MissingRequiredField
                message = "Element must have a 'type' field"
                severity = Severity.Error
            })
        
        if Option.isNone (getString "layer" metadata) then
            errors.Add({
                filePath = filePath
                elementId = id
                errorType = ErrorType.MissingRequiredField
                message = "Element must have a 'layer' field"
                severity = Severity.Error
            })
        
        // Validate layer value
        let validLayers = ["strategy"; "motivation"; "business"; "application"; "technology"; "physical"; "implementation"]
        match getString "layer" metadata with
        | Some layer when not (List.contains (layer.ToLower()) validLayers) ->
            errors.Add({
                filePath = filePath
                elementId = id
                errorType = ErrorType.InvalidLayer
                message = sprintf "Invalid layer '%s'. Must be one of: %s" layer (String.concat ", " validLayers)
                severity = Severity.Error
            })
        | _ -> ()
        
        // Validate ID format comprehensively
        match id with
        | Some idVal when idVal.Trim() <> "" ->
            // Check overall pattern first
            if not (System.Text.RegularExpressions.Regex.IsMatch(idVal, @"^[a-z0-9]+-[a-z0-9]+-\d{3}-[a-z0-9]+(-[a-z0-9]+)*$")) then
                errors.Add({
                    filePath = filePath
                    elementId = id
                    errorType = ErrorType.Unknown "invalid-id-format"
                    message = sprintf "ID '%s' should match pattern: [layer-code]-[type-code]-[###]-[descriptive-name]" idVal
                    severity = Severity.Error
                })
            else
                let parts = idVal.Split('-')
                
                // Validate layer code (must be exactly 3 chars and valid)
                if parts.Length >= 1 then
                    let layerCode = parts.[0]
                    if layerCode.Length <> 3 then
                        errors.Add({
                            filePath = filePath
                            elementId = id
                            errorType = ErrorType.Unknown "invalid-id-format"
                            message = sprintf "Layer code '%s' must be exactly 3 characters" layerCode
                            severity = Severity.Error
                        })
                    elif not (Map.containsKey layerCode layerCodes) then
                        errors.Add({
                            filePath = filePath
                            elementId = id
                            errorType = ErrorType.Unknown "invalid-id-format"
                            message = sprintf "Layer code '%s' is not valid. Must be: str, bus, app, tec, phy, mot, imp" layerCode
                            severity = Severity.Error
                        })
                    
                    // Validate type code (must be exactly 4 chars, except for collaboration which uses 5)
                    if parts.Length >= 2 then
                        let typeCode = parts.[1]
                        if typeCode.Length <> 4 && typeCode <> "colab" then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = sprintf "Type code '%s' must be exactly 4 characters (or 'colab' for collaboration)" typeCode
                                severity = Severity.Error
                            })
                        elif Map.containsKey layerCode typeCodes then
                            let validTypes = Map.find layerCode typeCodes
                            if not (List.contains typeCode validTypes) then
                                errors.Add({
                                    filePath = filePath
                                    elementId = id
                                    errorType = ErrorType.Unknown "invalid-id-format"
                                    message = sprintf "Type code '%s' is not valid for layer '%s'" typeCode layerCode
                                    severity = Severity.Error
                                })
                    
                    // Validate sequential number (must be exactly 3 digits: 001-999)
                    if parts.Length >= 3 then
                        let seqNum = parts.[2]
                        if seqNum.Length <> 3 || not (System.Char.IsDigit(seqNum.[0]) && System.Char.IsDigit(seqNum.[1]) && System.Char.IsDigit(seqNum.[2])) then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = sprintf "Sequential number '%s' must be exactly 3 digits (001-999)" seqNum
                                severity = Severity.Error
                            })
                        elif seqNum = "000" then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = "Sequential number must start at 001, not 000"
                                severity = Severity.Error
                            })
                    
                    // Validate descriptive name (1-4 words, lowercase, hyphens only)
                    if parts.Length >= 4 then
                        let descriptiveName = String.concat "-" (parts.[3..])
                        let wordCount = (parts.Length - 3)
                        
                        if wordCount < 1 then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = "Descriptive name must have at least 1 word"
                                severity = Severity.Error
                            })
                        elif wordCount > 4 then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = sprintf "Descriptive name has %d words, maximum is 4" wordCount
                                severity = Severity.Error
                            })
                        
                        // Check for invalid characters (only lowercase letters, numbers, hyphens)
                        if not (System.Text.RegularExpressions.Regex.IsMatch(descriptiveName, @"^[a-z0-9]+(-[a-z0-9]+)*$")) then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = "Descriptive name must contain only lowercase letters, numbers, and hyphens"
                                severity = Severity.Error
                            })
                        
                        // Check for invalid patterns (leading/trailing hyphens, double hyphens)
                        if descriptiveName.StartsWith("-") || descriptiveName.EndsWith("-") || descriptiveName.Contains("--") then
                            errors.Add({
                                filePath = filePath
                                elementId = id
                                errorType = ErrorType.Unknown "invalid-id-format"
                                message = "Descriptive name cannot have leading/trailing hyphens or consecutive hyphens"
                                severity = Severity.Error
                            })
        | _ -> ()
        
        List.ofSeq errors
    
    /// Load and parse a single markdown element file with validation
    let loadElementWithValidation (filePath: string) (logger: ILogger) : (Element option * ValidationError list) =
        try
            let content = File.ReadAllText(filePath)
            let (metadata, mdContent) = 
                try 
                    parseFrontmatter content
                with ex ->
                    logger.LogError($"Error parsing frontmatter from {filePath}: {ex.Message}")
                    raise (System.FormatException($"Failed to parse YAML frontmatter: {ex.Message}", ex))
            
            let validationErrors = validateElement filePath metadata
            
            let id = getString "id" metadata |> Option.defaultValue ""
            if id = "" then 
                logger.LogWarning($"Element in {filePath} has no ID, skipping")
                (None, validationErrors)
            else
                let name = getString "name" metadata |> Option.defaultValue "Unnamed"
                let layerStr = getString "layer" metadata |> Option.defaultValue "unknown"
                let typeStr = getString "type" metadata |> Option.defaultValue "unknown"
                let elementType = ElementType.parseElementType layerStr typeStr
                
                logger.LogDebug($"Loaded element: {id} ({name}) in layer {layerStr} from {filePath}")
                
                let element = {
                    id = id
                    name = name
                    elementType = elementType
                    content = mdContent.Trim()
                    properties = metadata
                    tags = getStringList "tags" metadata
                    relationships = parseRelationships metadata
                }
                (Some element, validationErrors)
        with ex ->
            logger.LogError($"Error loading element from {filePath}: {ex.Message}")
            ({
                filePath = filePath
                elementId = None
                errorType = ErrorType.Unknown "parse-error"
                message = $"Failed to parse file: {ex.Message}"
                severity = Severity.Error
            })
            |> List.singleton
            |> (fun errs -> (None, errs))
    
   
    
    /// Validate relationships for a single element against ArchiMate rules
    let validateRelationships (rules: RelationshipRules) (registry: ElementRegistry) (elem: Element) : ValidationError list =
        let errors = ResizeArray<ValidationError>()
        let filePath = $"{registry.elementsPath}/{ElementType.getLayer elem.elementType}/{elem.id}.md"
        
        for rel in elem.relationships do
            // Check 1: Target element must exist
            if not (Map.containsKey rel.target registry.elements) then
                errors.Add {
                    filePath = filePath
                    elementId = Some elem.id
                    errorType = ErrorType.RelationshipTargetNotFound (elem.id, rel.target)
                    message = $"Relationship target '{rel.target}' not found"
                    severity = Severity.Warning
                }
            
            // Check 2: No self-references
            if rel.target = elem.id then
                errors.Add {
                    filePath = filePath
                    elementId = Some elem.id
                    errorType = ErrorType.SelfReference elem.id
                    message = "Element cannot have relationship to itself"
                    severity = Severity.Warning
                }
            
            // Check 3 & 4: ArchiMate rules validation (only if target exists)
            match Map.tryFind rel.target registry.elements with
            | Some targetElem ->
                let sourceConcept = ElementType.elementTypeToConceptName elem.elementType
                let targetConcept = ElementType.elementTypeToConceptName targetElem.elementType
                
                match ElementType.relationTypeToCode rel.relationType with
                | Some relCode ->
                    match Map.tryFind (sourceConcept, targetConcept) rules with
                    | Some allowedCodes when not (Set.contains relCode allowedCodes) ->
                        let allowedTypes = 
                            allowedCodes 
                            |> Set.toList 
                            |> List.choose (fun c ->
                                match c with
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
                            )
                            |> String.concat ", "
                        
                        let relTypeStr = 
                            match rel.relationType with
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
                            | RelationType.Unknown s -> s
                        
                        errors.Add {
                            filePath = filePath
                            elementId = Some elem.id
                            errorType = ErrorType.InvalidRelationshipCombination (sourceConcept, targetConcept, relTypeStr)
                            message = $"Relationship '{relTypeStr}' not allowed from {sourceConcept} to {targetConcept}. Valid types: {allowedTypes}"
                            severity = Severity.Warning
                        }
                    | None ->
                        // No rules found for this combination - log but don't error
                        ()
                    | _ -> ()  // Relationship is valid
                | None ->
                    // Unknown relationship type
                    let relTypeStr = match rel.relationType with | RelationType.Unknown s -> s | _ -> "unknown"
                    errors.Add {
                        filePath = filePath
                        elementId = Some elem.id
                        errorType = ErrorType.InvalidRelationshipType relTypeStr
                        message = $"Unrecognized relationship type '{relTypeStr}'"
                        severity = Severity.Warning
                    }
            | None -> ()  // Target not found error already added above
        
        // Check 5: Duplicate relationships
        let duplicates =
            elem.relationships
            |> List.groupBy (fun r -> (r.target, r.relationType))
            |> List.filter (fun (_, rels) -> rels.Length > 1)
            |> List.map fst
        
        for (target, _) in duplicates do
            errors.Add {
                filePath = filePath
                elementId = Some elem.id
                errorType = ErrorType.DuplicateRelationship (elem.id, target)
                message = $"Duplicate relationship to '{target}'"
                severity = Severity.Warning
            }
        
        List.ofSeq errors
    
    /// Build the element registry from all markdown files
    let create (elementsPath: string) : ElementRegistry =
        let logger = LoggerFactory.Create(fun builder -> builder.AddConsole() |> ignore).CreateLogger("ElementRegistry")
        
        logger.LogInformation($"Creating element registry from: {elementsPath}")
        
        // Load relationship rules from relations.xml
        let relationsXmlPathCandidates =
            [
                Path.Combine(elementsPath, "relations.xml")
                Path.Combine(elementsPath, "schemas", "relations.xml")
                Path.Combine(elementsPath, "..", "schemas", "relations.xml")
            ]

        let relationsXmlPathOpt =
            relationsXmlPathCandidates
            |> List.tryFind File.Exists

        let relationsXmlPath =
            relationsXmlPathOpt
            |> Option.defaultValue relationsXmlPathCandidates.Head

        if relationsXmlPathOpt.IsNone then
            let triedPaths = String.concat "; " relationsXmlPathCandidates
            logger.LogWarning($"Relationship rules file not found. Tried: {triedPaths}")

        let relationshipRules = ElementType.parseRelationshipRules relationsXmlPath
        logger.LogInformation($"Loaded {Map.count relationshipRules} relationship rules from {relationsXmlPath}")
        
        let mutable elements = Map.empty
        let mutable elementsByLayer = Map.empty
        let mutable incomingRelations = Map.empty
        let mutable allValidationErrors = []
        
        if Directory.Exists(elementsPath) then
            let mdFiles = 
                Directory.EnumerateFiles(elementsPath, "*.md", SearchOption.AllDirectories)
                |> Seq.toList
            
            logger.LogInformation($"Found {mdFiles.Length} markdown files")
            
            // Load all elements with validation
            mdFiles
            |> List.iter (fun filePath ->
                let (elemOpt, errors) = loadElementWithValidation filePath logger
                allValidationErrors <- allValidationErrors @ errors
                
                match elemOpt with
                | Some elem ->
                    elements <- elements |> Map.add elem.id elem
                    
                    // Index by layer
                    let layer = ElementType.getLayer elem.elementType
                    let layerElems = 
                        elementsByLayer 
                        |> Map.tryFind layer 
                        |> Option.defaultValue []
                    elementsByLayer <- elementsByLayer |> Map.add layer (elem.id :: layerElems)
                    
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
                | None -> ()
            )
            
            logger.LogInformation($"Successfully loaded {Map.count elements} elements into registry")
            if allValidationErrors.Length > 0 then
                logger.LogWarning($"Found {allValidationErrors.Length} validation errors/warnings during load")
            
            // Second pass: Validate relationships
            logger.LogInformation("Starting relationship validation pass")
            let tempRegistry = {
                elements = elements
                elementsByLayer = elementsByLayer
                incomingRelations = incomingRelations
                validationErrors = ref []
                elementsPath = elementsPath
            }
            
            let relationshipErrors =
                elements
                |> Map.values
                |> Seq.collect (fun elem -> validateRelationships relationshipRules tempRegistry elem)
                |> List.ofSeq
            
            allValidationErrors <- allValidationErrors @ relationshipErrors
            logger.LogInformation($"Relationship validation found {relationshipErrors.Length} warnings")
        else
            logger.LogError($"Elements directory does not exist: {elementsPath}")
        
        {
            elements = elements
            elementsByLayer = elementsByLayer
            incomingRelations = incomingRelations
            validationErrors = ref allValidationErrors
            elementsPath = elementsPath
        }

    
    /// Get all validation errors
    let getValidationErrors (registry: ElementRegistry) : ValidationError list =
        !registry.validationErrors
    
    /// Get validation errors for a specific file
    let getFileValidationErrors (filePath: string) (registry: ElementRegistry) : ValidationError list =
        !registry.validationErrors
        |> List.filter (fun err -> err.filePath = filePath)
    
    /// Get validation errors by severity
    let getErrorsBySeverity (severity: Severity) (registry: ElementRegistry) : ValidationError list =
        !registry.validationErrors
        |> List.filter (fun err -> err.severity = severity)
    
    /// Reload validation for a single file and update registry
    let revalidateFile (filePath: string) (registry: ElementRegistry) (logger: ILogger) : unit =
        if File.Exists(filePath) then
            let (elemOpt, newErrors) = loadElementWithValidation filePath logger
            
            // Remove old validation errors for this file
            let updatedErrors = 
                !registry.validationErrors
                |> List.filter (fun err -> err.filePath <> filePath)
                |> (@) newErrors
            
            // Update the ref with new errors
            registry.validationErrors := updatedErrors
            
            logger.LogInformation($"Revalidated file {filePath}: {newErrors.Length} errors found")
        else
            logger.LogWarning($"File not found for revalidation: {filePath}")
    
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
        |> List.choose (fun (sourceId, relType, desc) ->
            match Map.tryFind sourceId registry.elements with
            | Some elem ->
                let rel = { target = elemId; relationType = relType; description = desc }
                Some (elem, rel)
            | None -> 
                None  // Skip relations to elements that don't exist
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
