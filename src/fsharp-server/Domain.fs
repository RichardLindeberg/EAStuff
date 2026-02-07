namespace EAArchive

open System
open System.Xml.Linq
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions

/// Strategy layer element types
[<RequireQualifiedAccess>]
type StrategyElement =
    | Resource
    | Capability
    | ValueStream
    | CourseOfAction

/// Motivation layer element types
[<RequireQualifiedAccess>]
type MotivationElement =
    | Stakeholder
    | Driver
    | Assessment
    | Goal
    | Outcome
    | Principle
    | Requirement
    | Constraint
    | Meaning
    | Value

/// Business layer element types
[<RequireQualifiedAccess>]
type BusinessElement =
    | Actor
    | Role
    | Process
    | Function
    | Service
    | Object
    | Event
    | Product

/// Application layer element types
[<RequireQualifiedAccess>]
type ApplicationElement =
    | Component
    | Function
    | Service
    | Interface
    | DataObject

/// Technology layer element types
[<RequireQualifiedAccess>]
type TechnologyElement =
    | Technology
    | Device
    | SystemSoftware
    | Service
    | Interface
    | Artifact
    | Node
    | CommunicationNetwork

/// Physical layer element types
[<RequireQualifiedAccess>]
type PhysicalElement =
    | Equipment
    | Facility
    | DistributionNetwork

/// Implementation layer element types
[<RequireQualifiedAccess>]
type ImplementationElement =
    | WorkPackage
    | Deliverable
    | ImplementationEvent
    | Plateau
    | Gap

/// All valid element types - layer + subtype combined
[<RequireQualifiedAccess>]
type ElementType =
    | Strategy of StrategyElement
    | Motivation of MotivationElement
    | Business of BusinessElement
    | Application of ApplicationElement
    | Technology of TechnologyElement
    | Physical of PhysicalElement
    | Implementation of ImplementationElement
    | Unknown of layer: string * elementTypeName: string

/// Relationship types
[<RequireQualifiedAccess>]
type RelationType =
    | Composition
    | Aggregation
    | Assignment
    | Realization
    | Specialization
    | Association
    | Access
    | Influence
    | Serving
    | Triggering
    | Flow
    | Unknown of string

/// Severity level for validation errors
[<RequireQualifiedAccess>]
type Severity =
    | Error
    | Warning

/// Error classification
[<RequireQualifiedAccess>]
type ErrorType =
    | MissingId
    | InvalidType
    | InvalidLayer
    | MissingRequiredField
    | InvalidRelationshipType of string
    | RelationshipTargetNotFound of string * string
    | InvalidRelationshipCombination of string * string * string
    | SelfReference of string
    | DuplicateRelationship of string * string
    | Unknown of string

/// Validation error for element files
type ValidationError = {
    filePath: string
    elementId: string option
    errorType: ErrorType
    message: string
    severity: Severity
}

/// Element relationship information
type Relationship = {
    target: string
    relationType: RelationType
    description: string
}

/// Map of (sourceType, targetType) -> Set of allowed relationship code chars
type RelationshipRules = Map<string * string, Set<char>>

/// ArchiMate element core data
type Element = {
    id: string
    name: string
    elementType: ElementType
    content: string
    properties: Map<string, obj>
    tags: string list
    relationships: Relationship list
}

/// Element with computed relationship metadata
type ElementWithRelations = {
    element: Element
    incomingRelations: (Element * Relationship) list
    outgoingRelations: (Element * Relationship) list
}

/// Layer configuration and display information
type LayerInfo = {
    displayName: string
    order: int
}

/// Layer names in the domain model
[<RequireQualifiedAccess>]
type Layer =
    | Strategy
    | Motivation
    | Business
    | Application
    | Technology
    | Physical
    | Implementation
    | Unknown of string

/// Helper functions for Layer
module Layer =
    let toString (layer: Layer) : string =
        match layer with
        | Layer.Strategy -> "Strategy"
        | Layer.Motivation -> "Motivation"
        | Layer.Business -> "Business"
        | Layer.Application -> "Application"
        | Layer.Technology -> "Technology"
        | Layer.Physical -> "Physical"
        | Layer.Implementation -> "Implementation"
        | Layer.Unknown value -> value

    let toKey (layer: Layer) : string =
        toString layer |> fun value -> value.ToLowerInvariant()

    let tryParse (value: string) : Layer option =
        match value.Trim().ToLowerInvariant() with
        | "strategy" -> Some Layer.Strategy
        | "motivation" -> Some Layer.Motivation
        | "business" -> Some Layer.Business
        | "application" -> Some Layer.Application
        | "technology" -> Some Layer.Technology
        | "physical" -> Some Layer.Physical
        | "implementation" -> Some Layer.Implementation
        | _ -> None

    let parse (value: string) : Layer =
        match tryParse value with
        | Some layer -> layer
        | None -> Layer.Unknown value

/// Web UI configuration values
type WebUiConfig =
        { BaseUrl: string
            SiteCssUrl: string
            DiagramCssUrl: string
            ValidationScriptUrl: string
            DiagramScriptUrl: string
            HtmxScriptUrl: string
            HtmxDebugScriptUrl: string
            CytoscapeScriptUrl: string
            DagreScriptUrl: string
            CytoscapeDagreScriptUrl: string
            LodashScriptUrl: string }

/// Application constants
module Config =
    let layerOrder = 
        [
            (Layer.Motivation, { displayName = "Motivation Layer"; order = 0 })
            (Layer.Strategy, { displayName = "Strategy Layer"; order = 1 })
            (Layer.Business, { displayName = "Business Layer"; order = 2 })
            (Layer.Application, { displayName = "Application Layer"; order = 3 })
            (Layer.Technology, { displayName = "Technology Layer"; order = 4 })
            (Layer.Physical, { displayName = "Physical Layer"; order = 5 })
            (Layer.Implementation, { displayName = "Implementation & Migration Layer"; order = 6 })
        ]
        |> Map.ofList
    
    let layerOptions =
        [
            Layer.Strategy
            Layer.Motivation
            Layer.Business
            Layer.Application
            Layer.Technology
            Layer.Physical
            Layer.Implementation
        ]
        |> List.map Layer.toKey

    let typeOptionsByLayer =
        Map.ofList [
            ("strategy", [ "resource"; "capability"; "value-stream"; "course-of-action" ])
            ("business", [
                "business-actor"; "business-role"; "business-collaboration"; "business-interface"
                "business-process"; "business-function"; "business-interaction"; "business-event"
                "business-service"; "business-object"; "contract"; "representation"; "product"
            ])
            ("application", [
                "application-component"; "application-collaboration"; "application-interface"
                "application-function"; "application-interaction"; "application-process"
                "application-event"; "application-service"; "data-object"
            ])
            ("technology", [
                "node"; "device"; "system-software"; "technology-collaboration"
                "technology-interface"; "path"; "communication-network"; "technology-function"
                "technology-process"; "technology-interaction"; "technology-event"
                "technology-service"; "artifact"
            ])
            ("physical", [ "equipment"; "facility"; "distribution-network"; "material" ])
            ("motivation", [
                "stakeholder"; "driver"; "assessment"; "goal"; "outcome"; "principle"
                "requirement"; "constraint"; "meaning"; "value"
            ])
            ("implementation", [ "work-package"; "deliverable"; "implementation-event"; "plateau"; "gap" ])
        ]

    let allTypeOptions =
        typeOptionsByLayer
        |> Map.toList
        |> List.collect snd
        |> List.distinct

    let getTypeOptions (layerValue: string) : string list =
        let normalized = layerValue.Trim().ToLowerInvariant()
        typeOptionsByLayer
        |> Map.tryFind normalized
        |> Option.defaultValue allTypeOptions

/// Helper functions to work with ElementType and parsing
module ElementType =
    /// Extract layer name from ElementType
    let getLayer = function
        | ElementType.Strategy _ -> Layer.Strategy
        | ElementType.Motivation _ -> Layer.Motivation
        | ElementType.Business _ -> Layer.Business
        | ElementType.Application _ -> Layer.Application
        | ElementType.Technology _ -> Layer.Technology
        | ElementType.Physical _ -> Layer.Physical
        | ElementType.Implementation _ -> Layer.Implementation
        | ElementType.Unknown (layer, _) -> Layer.Unknown layer
    
    /// Parse relationship type from string
    let parseRelationType (relTypeStr: string) : RelationType =
        let lower = relTypeStr.ToLowerInvariant()
        match lower with
        | "composition" -> RelationType.Composition
        | "aggregation" -> RelationType.Aggregation
        | "assignment" -> RelationType.Assignment
        | "realization" -> RelationType.Realization
        | "specialization" -> RelationType.Specialization
        | "association" -> RelationType.Association
        | "access" -> RelationType.Access
        | "influence" -> RelationType.Influence
        | "serving" -> RelationType.Serving
        | "triggering" -> RelationType.Triggering
        | "flow" -> RelationType.Flow
        | s -> RelationType.Unknown s

    /// Convert RelationType to canonical lowercase string
    let relationTypeToString (relType: RelationType) : string =
        match relType with
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

    /// Convert RelationType to display name
    let relationTypeToDisplayName (relType: RelationType) : string =
        let value = relationTypeToString relType
        if String.IsNullOrWhiteSpace value then
            ""
        else
            value.Substring(0, 1).ToUpperInvariant() + value.Substring(1)
    
    /// Parse severity from string
    let parseSeverity (sevStr: string) : Severity =
        let lower = sevStr.ToLowerInvariant()
        match lower with
        | "error" -> Severity.Error
        | "warning" -> Severity.Warning
        | _ -> Severity.Error
    
    /// Parse error type from string
    let parseErrorType (errTypeStr: string) : ErrorType =
        let lower = errTypeStr.ToLowerInvariant()
        match lower with
        | "missing-id" | "missingid" -> ErrorType.MissingId
        | "invalid-type" | "invalidtype" -> ErrorType.InvalidType
        | "invalid-layer" | "invalidlayer" -> ErrorType.InvalidLayer
        | "missing-required-field" | "missingrequiredfield" -> ErrorType.MissingRequiredField
        | "invalid-id-format" | "invalididformat" -> ErrorType.Unknown "invalid-id-format"
        | "parse-error" | "parseerror" -> ErrorType.Unknown "parse-error"
        | s -> ErrorType.Unknown s
    
    /// Convert error type to string
    let errorTypeToString (errType: ErrorType) : string =
        match errType with
        | ErrorType.MissingId -> "missing-id"
        | ErrorType.InvalidType -> "invalid-type"
        | ErrorType.InvalidLayer -> "invalid-layer"
        | ErrorType.MissingRequiredField -> "missing-required-field"
        | ErrorType.InvalidRelationshipType _ -> "invalid-relationship-type"
        | ErrorType.RelationshipTargetNotFound _ -> "relationship-target-not-found"
        | ErrorType.InvalidRelationshipCombination _ -> "invalid-relationship-combination"
        | ErrorType.SelfReference _ -> "self-reference"
        | ErrorType.DuplicateRelationship _ -> "duplicate-relationship"
        | ErrorType.Unknown s -> s

    /// Convert error type to display name
    let errorTypeToDisplayName (errType: ErrorType) : string =
        match errType with
        | ErrorType.MissingId -> "Missing ID"
        | ErrorType.InvalidType -> "Invalid Type"
        | ErrorType.InvalidLayer -> "Invalid Layer"
        | ErrorType.MissingRequiredField -> "Missing Required Field"
        | ErrorType.InvalidRelationshipType _ -> "Invalid Relationship Type"
        | ErrorType.RelationshipTargetNotFound _ -> "Relationship Target Not Found"
        | ErrorType.InvalidRelationshipCombination _ -> "Invalid Relationship Combination"
        | ErrorType.SelfReference _ -> "Self Reference"
        | ErrorType.DuplicateRelationship _ -> "Duplicate Relationship"
        | ErrorType.Unknown s -> s
    
    /// Convert severity to string
    let severityToString (sev: Severity) : string =
        match sev with
        | Severity.Error -> "error"
        | Severity.Warning -> "warning"
    
    /// Parse element type from layer and type name strings
    let parseElementType (layerStr: string) (typeStr: string) : ElementType =
        let layer = layerStr.ToLowerInvariant().Trim()
        let typeLower = typeStr.ToLowerInvariant().Trim()

        let stripLayerPrefix (value: string) : string =
            if value.StartsWith(layer + " ") then value.Substring(layer.Length + 1)
            elif value.StartsWith(layer + "-") then value.Substring(layer.Length + 1)
            else value

        let normalized =
            typeLower
            |> stripLayerPrefix
            |> fun v -> v.Replace(" ", "").Replace("-", "")

        match layer with
        | "strategy" ->
            match normalized with
            | "resource" -> ElementType.Strategy StrategyElement.Resource
            | "capability" -> ElementType.Strategy StrategyElement.Capability
            | "valuestream" -> ElementType.Strategy StrategyElement.ValueStream
            | "courseofaction" -> ElementType.Strategy StrategyElement.CourseOfAction
            | s -> ElementType.Unknown ("Strategy", s)

        | "motivation" ->
            match normalized with
            | "stakeholder" -> ElementType.Motivation MotivationElement.Stakeholder
            | "driver" -> ElementType.Motivation MotivationElement.Driver
            | "assessment" -> ElementType.Motivation MotivationElement.Assessment
            | "goal" -> ElementType.Motivation MotivationElement.Goal
            | "outcome" -> ElementType.Motivation MotivationElement.Outcome
            | "principle" -> ElementType.Motivation MotivationElement.Principle
            | "requirement" -> ElementType.Motivation MotivationElement.Requirement
            | "constraint" -> ElementType.Motivation MotivationElement.Constraint
            | "meaning" -> ElementType.Motivation MotivationElement.Meaning
            | "value" -> ElementType.Motivation MotivationElement.Value
            | s -> ElementType.Unknown ("Motivation", s)

        | "business" ->
            match normalized with
            | "actor" -> ElementType.Business BusinessElement.Actor
            | "role" -> ElementType.Business BusinessElement.Role
            | "process" -> ElementType.Business BusinessElement.Process
            | "function" -> ElementType.Business BusinessElement.Function
            | "service" -> ElementType.Business BusinessElement.Service
            | "object" -> ElementType.Business BusinessElement.Object
            | "event" -> ElementType.Business BusinessElement.Event
            | "product" -> ElementType.Business BusinessElement.Product
            | s -> ElementType.Unknown ("Business", s)

        | "application" ->
            match normalized with
            | "component" -> ElementType.Application ApplicationElement.Component
            | "function" -> ElementType.Application ApplicationElement.Function
            | "service" -> ElementType.Application ApplicationElement.Service
            | "interface" -> ElementType.Application ApplicationElement.Interface
            | "dataobject" -> ElementType.Application ApplicationElement.DataObject
            | s -> ElementType.Unknown ("Application", s)

        | "technology" ->
            match normalized with
            | "technology" -> ElementType.Technology TechnologyElement.Technology
            | "device" -> ElementType.Technology TechnologyElement.Device
            | "systemsoftware" -> ElementType.Technology TechnologyElement.SystemSoftware
            | "service" -> ElementType.Technology TechnologyElement.Service
            | "interface" -> ElementType.Technology TechnologyElement.Interface
            | "artifact" -> ElementType.Technology TechnologyElement.Artifact
            | "node" -> ElementType.Technology TechnologyElement.Node
            | "communicationnetwork" -> ElementType.Technology TechnologyElement.CommunicationNetwork
            | s -> ElementType.Unknown ("Technology", s)

        | "physical" ->
            match normalized with
            | "equipment" -> ElementType.Physical PhysicalElement.Equipment
            | "facility" -> ElementType.Physical PhysicalElement.Facility
            | "distributionnetwork" -> ElementType.Physical PhysicalElement.DistributionNetwork
            | s -> ElementType.Unknown ("Physical", s)

        | "implementation" ->
            match normalized with
            | "workpackage" -> ElementType.Implementation ImplementationElement.WorkPackage
            | "deliverable" -> ElementType.Implementation ImplementationElement.Deliverable
            | "implementationevent" -> ElementType.Implementation ImplementationElement.ImplementationEvent
            | "plateau" -> ElementType.Implementation ImplementationElement.Plateau
            | "gap" -> ElementType.Implementation ImplementationElement.Gap
            | s -> ElementType.Unknown ("Implementation", s)

        | _ -> ElementType.Unknown (layerStr, typeStr)
    
    /// Convert RelationType to ArchiMate relationship code character
    let relationTypeToCode (relType: RelationType) : char option =
        match relType with
        | RelationType.Composition -> Some 'c'
        | RelationType.Aggregation -> Some 'g'
        | RelationType.Assignment -> Some 'i'
        | RelationType.Realization -> Some 'r'
        | RelationType.Specialization -> Some 's'
        | RelationType.Association -> Some 'o'
        | RelationType.Access -> Some 'a'
        | RelationType.Influence -> Some 'n'
        | RelationType.Serving -> Some 'v'
        | RelationType.Triggering -> Some 't'
        | RelationType.Flow -> Some 'f'
        | RelationType.Unknown _ -> None
    
    /// Convert ElementType to ArchiMate concept name
    let elementTypeToConceptName (elemType: ElementType) : string =
        match elemType with
        // Strategy Layer
        | ElementType.Strategy StrategyElement.Resource -> "Resource"
        | ElementType.Strategy StrategyElement.Capability -> "Capability"
        | ElementType.Strategy StrategyElement.ValueStream -> "ValueStream"
        | ElementType.Strategy StrategyElement.CourseOfAction -> "CourseOfAction"
        
        // Motivation Layer
        | ElementType.Motivation MotivationElement.Stakeholder -> "Stakeholder"
        | ElementType.Motivation MotivationElement.Driver -> "Driver"
        | ElementType.Motivation MotivationElement.Assessment -> "Assessment"
        | ElementType.Motivation MotivationElement.Goal -> "Goal"
        | ElementType.Motivation MotivationElement.Outcome -> "Outcome"
        | ElementType.Motivation MotivationElement.Principle -> "Principle"
        | ElementType.Motivation MotivationElement.Requirement -> "Requirement"
        | ElementType.Motivation MotivationElement.Constraint -> "Constraint"
        | ElementType.Motivation MotivationElement.Meaning -> "Meaning"
        | ElementType.Motivation MotivationElement.Value -> "Value"
        
        // Business Layer
        | ElementType.Business BusinessElement.Actor -> "BusinessActor"
        | ElementType.Business BusinessElement.Role -> "BusinessRole"
        | ElementType.Business BusinessElement.Process -> "BusinessProcess"
        | ElementType.Business BusinessElement.Function -> "BusinessFunction"
        | ElementType.Business BusinessElement.Service -> "BusinessService"
        | ElementType.Business BusinessElement.Object -> "BusinessObject"
        | ElementType.Business BusinessElement.Event -> "BusinessEvent"
        | ElementType.Business BusinessElement.Product -> "Product"
        
        // Application Layer
        | ElementType.Application ApplicationElement.Component -> "ApplicationComponent"
        | ElementType.Application ApplicationElement.Function -> "ApplicationFunction"
        | ElementType.Application ApplicationElement.Service -> "ApplicationService"
        | ElementType.Application ApplicationElement.Interface -> "ApplicationInterface"
        | ElementType.Application ApplicationElement.DataObject -> "DataObject"
        
        // Technology Layer
        | ElementType.Technology TechnologyElement.Technology -> "Technology"
        | ElementType.Technology TechnologyElement.Device -> "Device"
        | ElementType.Technology TechnologyElement.SystemSoftware -> "SystemSoftware"
        | ElementType.Technology TechnologyElement.Service -> "TechnologyService"
        | ElementType.Technology TechnologyElement.Interface -> "TechnologyInterface"
        | ElementType.Technology TechnologyElement.Artifact -> "Artifact"
        | ElementType.Technology TechnologyElement.Node -> "Node"
        | ElementType.Technology TechnologyElement.CommunicationNetwork -> "CommunicationNetwork"
        
        // Physical Layer
        | ElementType.Physical PhysicalElement.Equipment -> "Equipment"
        | ElementType.Physical PhysicalElement.Facility -> "Facility"
        | ElementType.Physical PhysicalElement.DistributionNetwork -> "DistributionNetwork"
        
        // Implementation Layer
        | ElementType.Implementation ImplementationElement.WorkPackage -> "WorkPackage"
        | ElementType.Implementation ImplementationElement.Deliverable -> "Deliverable"
        | ElementType.Implementation ImplementationElement.ImplementationEvent -> "ImplementationEvent"
        | ElementType.Implementation ImplementationElement.Plateau -> "Plateau"
        | ElementType.Implementation ImplementationElement.Gap -> "Gap"
        
        // Unknown
        | ElementType.Unknown (_, typeName) -> typeName
    
    /// Parse relationship rules from relations.xml file
    let parseRelationshipRulesWithLogger (xmlPath: string) (logger: ILogger) : Result<RelationshipRules, string> =
        try
            let doc = XDocument.Load(xmlPath)
            let sources = doc.Descendants(XName.Get("source"))
            
            let rules =
                sources
                |> Seq.collect (fun source ->
                    let sourceConcept = source.Attribute(XName.Get("concept")).Value
                    source.Descendants(XName.Get("target"))
                    |> Seq.map (fun target ->
                        let targetConcept = target.Attribute(XName.Get("concept")).Value
                        let relations = target.Attribute(XName.Get("relations")).Value
                        let relationSet = relations |> Set.ofSeq
                        ((sourceConcept, targetConcept), relationSet)
                    )
                )
                |> Map.ofSeq

            Ok rules
        with ex ->
            logger.LogError(ex, "Failed to parse relationship rules from {rulesPath}", xmlPath)
            Error ex.Message

    let parseRelationshipRules (xmlPath: string) : RelationshipRules =
        match parseRelationshipRulesWithLogger xmlPath (NullLogger.Instance) with
        | Ok rules -> rules
        | Error _ -> Map.empty
