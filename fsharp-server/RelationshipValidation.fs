namespace EAArchive

open System
open System.Xml.Linq

/// Helper functions for relationship validation
module RelationshipValidation =
    
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
        | ElementType.Strategy StrategyElement.Stakeholder -> "Stakeholder"
        | ElementType.Strategy StrategyElement.Driver -> "Driver"
        | ElementType.Strategy StrategyElement.Assessment -> "Assessment"
        | ElementType.Strategy StrategyElement.Goal -> "Goal"
        | ElementType.Strategy StrategyElement.Outcome -> "Outcome"
        | ElementType.Strategy StrategyElement.Principle -> "Principle"
        | ElementType.Strategy StrategyElement.Requirement -> "Requirement"
        | ElementType.Strategy StrategyElement.Constraint -> "Constraint"
        
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
    let parseRelationshipRules (xmlPath: string) : RelationshipRules =
        try
            let doc = XDocument.Load(xmlPath)
            let sources = doc.Descendants(XName.Get("source"))
            
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
        with ex ->
            printfn "Warning: Failed to parse relationship rules: %s" ex.Message
            Map.empty
