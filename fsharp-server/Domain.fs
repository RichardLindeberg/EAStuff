namespace EAArchive

open System

/// Strategy layer element types
[<RequireQualifiedAccess>]
type StrategyElement =
    | Stakeholder
    | Driver
    | Assessment
    | Goal
    | Outcome
    | Principle
    | Requirement
    | Constraint

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

/// Application constants
module Config =
    let layerOrder = 
        [
            ("Motivation", { displayName = "Motivation Layer"; order = 0 })
            ("Strategy", { displayName = "Strategy Layer"; order = 1 })
            ("Business", { displayName = "Business Layer"; order = 2 })
            ("Application", { displayName = "Application Layer"; order = 3 })
            ("Technology", { displayName = "Technology Layer"; order = 4 })
            ("Physical", { displayName = "Physical Layer"; order = 5 })
            ("Implementation", { displayName = "Implementation & Migration Layer"; order = 6 })
        ]
        |> Map.ofList
    
    let elementsPath = "../elements"
    let baseUrl = "/"

/// Helper functions to work with ElementType and parsing
module ElementType =
    /// Extract layer name from ElementType
    let getLayer = function
        | ElementType.Strategy _ -> "Strategy"
        | ElementType.Motivation _ -> "Motivation"
        | ElementType.Business _ -> "Business"
        | ElementType.Application _ -> "Application"
        | ElementType.Technology _ -> "Technology"
        | ElementType.Physical _ -> "Physical"
        | ElementType.Implementation _ -> "Implementation"
        | ElementType.Unknown (layer, _) -> layer
    
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
        | ErrorType.Unknown s -> s
    
    /// Convert severity to string
    let severityToString (sev: Severity) : string =
        match sev with
        | Severity.Error -> "error"
        | Severity.Warning -> "warning"
    
    /// Parse element type from layer and type name strings
    let parseElementType (layerStr: string) (typeStr: string) : ElementType =
        let layer = layerStr.ToLowerInvariant()
        let lower = typeStr.ToLowerInvariant()
        
        match layer with
        | "strategy" ->
            (match lower with
            | "stakeholder" -> ElementType.Strategy StrategyElement.Stakeholder
            | "driver" -> ElementType.Strategy StrategyElement.Driver
            | "assessment" -> ElementType.Strategy StrategyElement.Assessment
            | "goal" -> ElementType.Strategy StrategyElement.Goal
            | "outcome" -> ElementType.Strategy StrategyElement.Outcome
            | "principle" -> ElementType.Strategy StrategyElement.Principle
            | "requirement" -> ElementType.Strategy StrategyElement.Requirement
            | "constraint" -> ElementType.Strategy StrategyElement.Constraint
            | s -> ElementType.Unknown ("Strategy", s))
        
        | "motivation" ->
            (match lower with
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
            | s -> ElementType.Unknown ("Motivation", s))
        
        | "business" ->
            (match lower with
            | "actor" -> ElementType.Business BusinessElement.Actor
            | "role" -> ElementType.Business BusinessElement.Role
            | "process" -> ElementType.Business BusinessElement.Process
            | "function" -> ElementType.Business BusinessElement.Function
            | "service" -> ElementType.Business BusinessElement.Service
            | "object" -> ElementType.Business BusinessElement.Object
            | "event" -> ElementType.Business BusinessElement.Event
            | "product" -> ElementType.Business BusinessElement.Product
            | s -> ElementType.Unknown ("Business", s))
        
        | "application" ->
            (match lower with
            | "component" -> ElementType.Application ApplicationElement.Component
            | "function" -> ElementType.Application ApplicationElement.Function
            | "service" -> ElementType.Application ApplicationElement.Service
            | "interface" -> ElementType.Application ApplicationElement.Interface
            | "dataobject" | "data-object" | "data object" -> ElementType.Application ApplicationElement.DataObject
            | s -> ElementType.Unknown ("Application", s))
        
        | "technology" ->
            (match lower with
            | "technology" -> ElementType.Technology TechnologyElement.Technology
            | "device" -> ElementType.Technology TechnologyElement.Device
            | "systemsoftware" | "system-software" | "system software" -> ElementType.Technology TechnologyElement.SystemSoftware
            | "service" -> ElementType.Technology TechnologyElement.Service
            | "interface" -> ElementType.Technology TechnologyElement.Interface
            | "artifact" -> ElementType.Technology TechnologyElement.Artifact
            | "node" -> ElementType.Technology TechnologyElement.Node
            | "communicationnetwork" | "communication-network" | "communication network" -> ElementType.Technology TechnologyElement.CommunicationNetwork
            | s -> ElementType.Unknown ("Technology", s))
        
        | "physical" ->
            (match lower with
            | "equipment" -> ElementType.Physical PhysicalElement.Equipment
            | "facility" -> ElementType.Physical PhysicalElement.Facility
            | "distributionnetwork" | "distribution-network" | "distribution network" -> ElementType.Physical PhysicalElement.DistributionNetwork
            | s -> ElementType.Unknown ("Physical", s))
        
        | "implementation" ->
            (match lower with
            | "workpackage" | "work-package" | "work package" -> ElementType.Implementation ImplementationElement.WorkPackage
            | "deliverable" -> ElementType.Implementation ImplementationElement.Deliverable
            | "implementationevent" | "implementation-event" | "implementation event" -> ElementType.Implementation ImplementationElement.ImplementationEvent
            | "plateau" -> ElementType.Implementation ImplementationElement.Plateau
            | "gap" -> ElementType.Implementation ImplementationElement.Gap
            | s -> ElementType.Unknown ("Implementation", s))
        
        | _ -> ElementType.Unknown (layerStr, typeStr)
