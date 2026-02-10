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

/// Layer configuration and display information
type LayerInfo = {
    displayName: string
    order: int
}

/// Governance document types
[<RequireQualifiedAccess>]
type GovernanceDocType =
    | Policy
    | Instruction
    | Manual
    | Unknown of string

/// Helper functions for GovernanceDocType
module GovernanceDocType =
    let toConceptName (docType: GovernanceDocType) : string =
        match docType with
        | GovernanceDocType.Policy -> "GovernancePolicy"
        | GovernanceDocType.Instruction -> "GovernanceInstruction"
        | GovernanceDocType.Manual -> "GovernanceManual"
        | GovernanceDocType.Unknown value -> value

/// Unified document kinds for the repository
[<RequireQualifiedAccess>]
type DocumentKind =
    | Architecture 
    | Governance

/// Governance-specific metadata
type GovernanceMetadata = {
    approvedBy: string
    effectiveDate: string
}

/// ArchiMate-specific metadata
type ArchimateMetadata = {
    elementType: string
    layerValue: string
    criticality: string option
}




[<RequireQualifiedAccess>]
type DocumentMetaData = 
    | ArchiMateMetaData of  ArchimateMetadata
    | GovernanceDocMetaData of  GovernanceMetadata


/// Unified relation entry between documents
type DocumentRelation = {
    sourceId: string
    targetId: string
    relationType: string
    description: string
}

/// Unified document record
type DocumentRecord = {
    id: string
    slug: string
    title: string
    owner: string option
    status: string option
    version: string option
    lastUpdated: string option
    reviewCycle: string option
    nextReview: string option
    relationships: Relationship list
    tags: string list
    filePath: string
    metadata: DocumentMetaData
    content: string
    rawContent: string
}

let (|ArchitectureDoc|GovernanceDoc|) (doc: DocumentRecord) =
    match doc.metadata with
    | DocumentMetaData.ArchiMateMetaData metadata -> ArchitectureDoc metadata
    | DocumentMetaData.GovernanceDocMetaData metadata -> GovernanceDoc metadata

let getDocumentKind (doc: DocumentRecord) : DocumentKind =
    match doc with
    | ArchitectureDoc _ -> DocumentKind.Architecture
    | GovernanceDoc _ -> DocumentKind.Governance

/// Unified document repository
type DocumentRepository = {
    documents: Map<string, DocumentRecord>
    documentsByKind: Map<DocumentKind, string list>
    documentsByElementType: Map<ElementType, string list>
    documentsByGovernanceType: Map<GovernanceDocType, string list>
    relations: DocumentRelation list
    validationErrors: ValidationError list
}



/// Web UI configuration values
type WebUiConfig = { 
    BaseUrl: string
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
            ("motivation", { displayName = "Motivation Layer"; order = 0 })
            ("strategy", { displayName = "Strategy Layer"; order = 1 })
            ("business", { displayName = "Business Layer"; order = 2 })
            ("application", { displayName = "Application Layer"; order = 3 })
            ("technology", { displayName = "Technology Layer"; order = 4 })
            ("physical", { displayName = "Physical Layer"; order = 5 })
            ("implementation", { displayName = "Implementation & Migration Layer"; order = 6 })
        ]
        |> Map.ofList
    
    let layerOptions =
        [
            "strategy"
            "motivation"
            "business"
            "application"
            "technology"
            "physical"
            "implementation"
        ]

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
    /// Extract layer key from ElementType
    let getLayerKey = function
        | ElementType.Strategy _ -> "strategy"
        | ElementType.Motivation _ -> "motivation"
        | ElementType.Business _ -> "business"
        | ElementType.Application _ -> "application"
        | ElementType.Technology _ -> "technology"
        | ElementType.Physical _ -> "physical"
        | ElementType.Implementation _ -> "implementation"
        | ElementType.Unknown (layer, _) -> layer.Trim().ToLowerInvariant()

    /// Extract display layer name from ElementType
    let getLayerDisplayName (elementType: ElementType) : string =
        let layerKey = getLayerKey elementType
        Config.layerOrder
        |> Map.tryFind layerKey
        |> Option.map (fun info -> info.displayName)
        |> Option.defaultValue layerKey

    /// Extract canonical type key from ElementType
    let getTypeKey (elementType: ElementType) : string =
        let normalize (value: string) : string =
            value.Trim().ToLowerInvariant().Replace(" ", "-")

        match elementType with
        | ElementType.Strategy StrategyElement.Resource -> "resource"
        | ElementType.Strategy StrategyElement.Capability -> "capability"
        | ElementType.Strategy StrategyElement.ValueStream -> "value-stream"
        | ElementType.Strategy StrategyElement.CourseOfAction -> "course-of-action"
        | ElementType.Motivation MotivationElement.Stakeholder -> "stakeholder"
        | ElementType.Motivation MotivationElement.Driver -> "driver"
        | ElementType.Motivation MotivationElement.Assessment -> "assessment"
        | ElementType.Motivation MotivationElement.Goal -> "goal"
        | ElementType.Motivation MotivationElement.Outcome -> "outcome"
        | ElementType.Motivation MotivationElement.Principle -> "principle"
        | ElementType.Motivation MotivationElement.Requirement -> "requirement"
        | ElementType.Motivation MotivationElement.Constraint -> "constraint"
        | ElementType.Motivation MotivationElement.Meaning -> "meaning"
        | ElementType.Motivation MotivationElement.Value -> "value"
        | ElementType.Business BusinessElement.Actor -> "business-actor"
        | ElementType.Business BusinessElement.Role -> "business-role"
        | ElementType.Business BusinessElement.Process -> "business-process"
        | ElementType.Business BusinessElement.Function -> "business-function"
        | ElementType.Business BusinessElement.Service -> "business-service"
        | ElementType.Business BusinessElement.Object -> "business-object"
        | ElementType.Business BusinessElement.Event -> "business-event"
        | ElementType.Business BusinessElement.Product -> "product"
        | ElementType.Application ApplicationElement.Component -> "application-component"
        | ElementType.Application ApplicationElement.Function -> "application-function"
        | ElementType.Application ApplicationElement.Service -> "application-service"
        | ElementType.Application ApplicationElement.Interface -> "application-interface"
        | ElementType.Application ApplicationElement.DataObject -> "data-object"
        | ElementType.Technology TechnologyElement.Technology -> "technology"
        | ElementType.Technology TechnologyElement.Device -> "device"
        | ElementType.Technology TechnologyElement.SystemSoftware -> "system-software"
        | ElementType.Technology TechnologyElement.Service -> "technology-service"
        | ElementType.Technology TechnologyElement.Interface -> "technology-interface"
        | ElementType.Technology TechnologyElement.Artifact -> "artifact"
        | ElementType.Technology TechnologyElement.Node -> "node"
        | ElementType.Technology TechnologyElement.CommunicationNetwork -> "communication-network"
        | ElementType.Physical PhysicalElement.Equipment -> "equipment"
        | ElementType.Physical PhysicalElement.Facility -> "facility"
        | ElementType.Physical PhysicalElement.DistributionNetwork -> "distribution-network"
        | ElementType.Implementation ImplementationElement.WorkPackage -> "work-package"
        | ElementType.Implementation ImplementationElement.Deliverable -> "deliverable"
        | ElementType.Implementation ImplementationElement.ImplementationEvent -> "implementation-event"
        | ElementType.Implementation ImplementationElement.Plateau -> "plateau"
        | ElementType.Implementation ImplementationElement.Gap -> "gap"
        | ElementType.Unknown (_, typeName) -> normalize typeName
    
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
