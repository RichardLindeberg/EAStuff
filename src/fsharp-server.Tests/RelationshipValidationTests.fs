namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive

module RelationshipValidationTests =
    let private withTempRegistry (elements: (string * string) list) (rulesXml: string) (assertFn: ElementRegistry -> unit) : unit =
        let rootDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        let elementsPath = Path.Combine(rootDir, "archimate")
        let schemasPath = Path.Combine(rootDir, "schemas")

        Directory.CreateDirectory(elementsPath) |> ignore
        Directory.CreateDirectory(schemasPath) |> ignore

        let relationsPath = Path.Combine(schemasPath, "relations.xml")
        File.WriteAllText(relationsPath, rulesXml)

        elements
        |> List.iter (fun (fileName, content) ->
            File.WriteAllText(Path.Combine(elementsPath, fileName), content)
        )

        try
            let registry = ElementRegistry.create elementsPath relationsPath
            assertFn registry
        finally
            Directory.Delete(rootDir, true)

    let private createRegistry (elements: Element list) : ElementRegistry =
        let elementsMap =
            elements
            |> List.map (fun e -> e.id, e)
            |> Map.ofList

        {
            elements = elementsMap
            elementsByLayer = Map.empty
            incomingRelations = Map.empty
            validationErrors = []
            validationErrorsLock = obj ()
            elementsPath = Path.Combine(Path.GetTempPath(), "archimate")
            relationshipRules = Map.empty
        }

    let private makeElement (id: string) (elemType: ElementType) (relationships: Relationship list) : Element =
        {
            id = id
            name = "Test"
            elementType = elemType
            content = ""
            properties = Map.empty
            tags = []
            relationships = relationships
        }

    let private rulesFor (source: string, target: string, rels: char list) : RelationshipRules =
        Map.ofList [ (source, target), Set.ofList rels ]
    
    [<Fact>]
    let ``parseRelationshipRules should load valid XML`` () =
        // Create a minimal test XML content
        let testXml = """<?xml version="1.0" encoding="UTF-8"?>
<relations>
    <source concept="BusinessActor">
        <target concept="BusinessRole" relations="io"/>
        <target concept="ApplicationComponent" relations="fotv"/>
    </source>
    <source concept="ApplicationComponent">
        <target concept="ApplicationComponent" relations="cfgorstv"/>
        <target concept="DataObject" relations="a"/>
    </source>
</relations>"""
        
        // Write to temp file
        let tempPath = System.IO.Path.GetTempFileName()
        try
            System.IO.File.WriteAllText(tempPath, testXml)
            
            let rules = ElementType.parseRelationshipRules tempPath
            
            // Verify rules were loaded
            Assert.NotEmpty(rules)
            Assert.True(Map.count rules > 0)
            
            // Verify specific rules
            match Map.tryFind ("BusinessActor", "BusinessRole") rules with
            | Some codes ->
                Assert.True(Set.contains 'i' codes)  // assignment
                Assert.True(Set.contains 'o' codes)  // association
                Assert.False(Set.contains 'c' codes) // composition not allowed
            | None -> Assert.True(false, "Expected BusinessActor->BusinessRole rule not found")
            
            match Map.tryFind ("ApplicationComponent", "DataObject") rules with
            | Some codes ->
                Assert.True(Set.contains 'a' codes)  // access
                Assert.False(Set.contains 'c' codes) // composition not allowed
            | None -> Assert.True(false, "Expected ApplicationComponent->DataObject rule not found")
        finally
            System.IO.File.Delete(tempPath)
    
    [<Fact>]
    let ``parseRelationshipRules should handle malformed XML gracefully`` () =
        let badXml = "<invalid>xml</content>"
        let tempPath = System.IO.Path.GetTempFileName()
        try
            System.IO.File.WriteAllText(tempPath, badXml)
            
            let rules = ElementType.parseRelationshipRules tempPath
            
            // Should return empty map on error, not throw
            Assert.Empty(rules)
        finally
            System.IO.File.Delete(tempPath)
    
    [<Fact>]
    let ``parseRelationshipRules should handle missing file gracefully`` () =
        let rules = ElementType.parseRelationshipRules "nonexistent-file.xml"
        
        // Should return empty map on error, not throw
        Assert.Empty(rules)
    
    [<Fact>]
    let ``relationTypeToCode should map all relationship types correctly`` () =
        Assert.Equal(Some 'c', ElementType.relationTypeToCode RelationType.Composition)
        Assert.Equal(Some 'g', ElementType.relationTypeToCode RelationType.Aggregation)
        Assert.Equal(Some 'i', ElementType.relationTypeToCode RelationType.Assignment)
        Assert.Equal(Some 'r', ElementType.relationTypeToCode RelationType.Realization)
        Assert.Equal(Some 's', ElementType.relationTypeToCode RelationType.Specialization)
        Assert.Equal(Some 'o', ElementType.relationTypeToCode RelationType.Association)
        Assert.Equal(Some 'a', ElementType.relationTypeToCode RelationType.Access)
        Assert.Equal(Some 'n', ElementType.relationTypeToCode RelationType.Influence)
        Assert.Equal(Some 'v', ElementType.relationTypeToCode RelationType.Serving)
        Assert.Equal(Some 't', ElementType.relationTypeToCode RelationType.Triggering)
        Assert.Equal(Some 'f', ElementType.relationTypeToCode RelationType.Flow)
        Assert.Equal(None, ElementType.relationTypeToCode (RelationType.Unknown "invalid"))
    
    [<Fact>]
    let ``elementTypeToConceptName should map business elements correctly`` () =
        Assert.Equal("BusinessActor", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Actor))
        Assert.Equal("BusinessRole", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Role))
        Assert.Equal("BusinessProcess", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Process))
        Assert.Equal("BusinessFunction", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Function))
        Assert.Equal("BusinessService", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Service))
        Assert.Equal("BusinessObject", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Object))
        Assert.Equal("BusinessEvent", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Event))
        Assert.Equal("Product", ElementType.elementTypeToConceptName (ElementType.Business BusinessElement.Product))
    
    [<Fact>]
    let ``elementTypeToConceptName should map application elements correctly`` () =
        Assert.Equal("ApplicationComponent", ElementType.elementTypeToConceptName (ElementType.Application ApplicationElement.Component))
        Assert.Equal("ApplicationFunction", ElementType.elementTypeToConceptName (ElementType.Application ApplicationElement.Function))
        Assert.Equal("ApplicationService", ElementType.elementTypeToConceptName (ElementType.Application ApplicationElement.Service))
        Assert.Equal("ApplicationInterface", ElementType.elementTypeToConceptName (ElementType.Application ApplicationElement.Interface))
        Assert.Equal("DataObject", ElementType.elementTypeToConceptName (ElementType.Application ApplicationElement.DataObject))
    
    [<Fact>]
    let ``elementTypeToConceptName should map technology elements correctly`` () =
        Assert.Equal("Node", ElementType.elementTypeToConceptName (ElementType.Technology TechnologyElement.Node))
        Assert.Equal("Device", ElementType.elementTypeToConceptName (ElementType.Technology TechnologyElement.Device))
        Assert.Equal("SystemSoftware", ElementType.elementTypeToConceptName (ElementType.Technology TechnologyElement.SystemSoftware))
        Assert.Equal("Artifact", ElementType.elementTypeToConceptName (ElementType.Technology TechnologyElement.Artifact))
    
    [<Fact>]
    let ``new error types should serialize to correct strings`` () =
        Assert.Equal("invalid-relationship-type", ElementType.errorTypeToString (ErrorType.InvalidRelationshipType "test"))
        Assert.Equal("relationship-target-not-found", ElementType.errorTypeToString (ErrorType.RelationshipTargetNotFound ("elem1", "elem2")))
        Assert.Equal("invalid-relationship-combination", ElementType.errorTypeToString (ErrorType.InvalidRelationshipCombination ("src", "tgt", "rel")))
        Assert.Equal("self-reference", ElementType.errorTypeToString (ErrorType.SelfReference "elem1"))
        Assert.Equal("duplicate-relationship", ElementType.errorTypeToString (ErrorType.DuplicateRelationship ("elem1", "elem2")))

    [<Fact>]
    let ``validateRelationships should warn on missing target`` () =
        let rel = { target = "missing-target"; relationType = RelationType.Association; description = "" }
        let source = makeElement "bus-actr-001-test" (ElementType.Business BusinessElement.Actor) [ rel ]
        let registry = createRegistry [ source ]
        let rules = rulesFor ("BusinessActor", "BusinessActor", [ 'o' ])

        let errors = ElementRegistry.validateRelationships rules registry source

        Assert.Single(errors) |> ignore
        Assert.Equal(Severity.Warning, errors.Head.severity)
        Assert.Equal("relationship-target-not-found", ElementType.errorTypeToString errors.Head.errorType)

    [<Fact>]
    let ``validateRelationships should warn on self reference`` () =
        let rel = { target = "bus-actr-001-test"; relationType = RelationType.Association; description = "" }
        let source = makeElement "bus-actr-001-test" (ElementType.Business BusinessElement.Actor) [ rel ]
        let registry = createRegistry [ source ]
        let rules = rulesFor ("BusinessActor", "BusinessActor", [ 'o' ])

        let errors = ElementRegistry.validateRelationships rules registry source

        Assert.True(errors |> List.exists (fun e -> e.errorType = ErrorType.SelfReference "bus-actr-001-test"))
        Assert.True(errors |> List.forall (fun e -> e.severity = Severity.Warning))

    [<Fact>]
    let ``validateRelationships should warn on duplicate relationships`` () =
        let rel1 = { target = "bus-actr-002-target"; relationType = RelationType.Association; description = "" }
        let rel2 = { target = "bus-actr-002-target"; relationType = RelationType.Association; description = "" }
        let source = makeElement "bus-actr-001-test" (ElementType.Business BusinessElement.Actor) [ rel1; rel2 ]
        let target = makeElement "bus-actr-002-target" (ElementType.Business BusinessElement.Actor) []
        let registry = createRegistry [ source; target ]
        let rules = rulesFor ("BusinessActor", "BusinessActor", [ 'o' ])

        let errors = ElementRegistry.validateRelationships rules registry source

        Assert.True(errors |> List.exists (fun e -> e.errorType = ErrorType.DuplicateRelationship ("bus-actr-001-test", "bus-actr-002-target")))

    [<Fact>]
    let ``validateRelationships should warn on invalid relationship combination`` () =
        let rel = { target = "app-comp-001-target"; relationType = RelationType.Composition; description = "" }
        let source = makeElement "bus-actr-001-test" (ElementType.Business BusinessElement.Actor) [ rel ]
        let target = makeElement "app-comp-001-target" (ElementType.Application ApplicationElement.Component) []
        let registry = createRegistry [ source; target ]
        let rules = rulesFor ("BusinessActor", "ApplicationComponent", [ 'o' ])

        let errors = ElementRegistry.validateRelationships rules registry source

        Assert.True(errors |> List.exists (fun e ->
            match e.errorType with
            | ErrorType.InvalidRelationshipCombination ("BusinessActor", "ApplicationComponent", "composition") -> true
            | _ -> false
        ))

    [<Fact>]
    let ``validateRelationships should not warn on valid relationship`` () =
        let rel = { target = "bus-actr-002-target"; relationType = RelationType.Association; description = "" }
        let source = makeElement "bus-actr-001-test" (ElementType.Business BusinessElement.Actor) [ rel ]
        let target = makeElement "bus-actr-002-target" (ElementType.Business BusinessElement.Actor) []
        let registry = createRegistry [ source; target ]
        let rules = rulesFor ("BusinessActor", "BusinessActor", [ 'o' ])

        let errors = ElementRegistry.validateRelationships rules registry source

        Assert.Empty(errors)

    [<Fact>]
    let ``validateRelationships should warn on unknown relationship type`` () =
        let rel = { target = "bus-actr-002-target"; relationType = RelationType.Unknown "weird"; description = "" }
        let source = makeElement "bus-actr-001-test" (ElementType.Business BusinessElement.Actor) [ rel ]
        let target = makeElement "bus-actr-002-target" (ElementType.Business BusinessElement.Actor) []
        let registry = createRegistry [ source; target ]
        let rules = rulesFor ("BusinessActor", "BusinessActor", [ 'o' ])

        let errors = ElementRegistry.validateRelationships rules registry source

        Assert.True(errors |> List.exists (fun e -> e.errorType = ErrorType.InvalidRelationshipType "weird"))

    [<Fact>]
    let ``ElementRegistry should surface relationship warnings from markdown files`` () =
        let rulesXml = """<?xml version="1.0" encoding="UTF-8"?>
<relations>
    <source concept="BusinessActor">
        <target concept="BusinessActor" relations="o"/>
        <target concept="ApplicationComponent" relations="o"/>
    </source>
</relations>"""

        let sourceElement = """---
id: bus-actr-001-source
name: Source
type: actor
layer: business
relationships:
- type: association
  target: bus-actr-002-target
  description: Valid relationship
- type: composition
  target: app-comp-001-target
  description: Invalid combination
- type: association
  target: missing-target
  description: Missing target
---

Content.
"""

        let targetElement = """---
id: bus-actr-002-target
name: Target
type: actor
layer: business
---

Content.
"""

        let appTarget = """---
id: app-comp-001-target
name: App Target
type: component
layer: application
---

Content.
"""

        let elements =
            [
                ("source.md", sourceElement)
                ("target.md", targetElement)
                ("app-target.md", appTarget)
            ]

        withTempRegistry elements rulesXml (fun registry ->
            let errors = ElementRegistry.getValidationErrors registry

            Assert.True(errors |> List.exists (fun e -> e.errorType = ErrorType.RelationshipTargetNotFound ("bus-actr-001-source", "missing-target")))
            Assert.True(errors |> List.exists (fun e ->
                match e.errorType with
                | ErrorType.InvalidRelationshipCombination ("BusinessActor", "ApplicationComponent", "composition") -> true
                | _ -> false
            ))
            Assert.True(errors |> List.forall (fun e -> e.severity = Severity.Warning))
        )

    [<Fact>]
    let ``ElementRegistry should not warn on valid relationships from markdown files`` () =
        let rulesXml = """<?xml version="1.0" encoding="UTF-8"?>
<relations>
    <source concept="BusinessActor">
        <target concept="BusinessActor" relations="o"/>
    </source>
</relations>"""

        let sourceElement = """---
id: bus-actr-010-source
name: Source
type: actor
layer: business
relationships:
- type: association
  target: bus-actr-011-target
  description: Valid relationship
---

Content.
"""

        let targetElement = """---
id: bus-actr-011-target
name: Target
type: actor
layer: business
---

Content.
"""

        let elements =
            [
                ("source.md", sourceElement)
                ("target.md", targetElement)
            ]

        withTempRegistry elements rulesXml (fun registry ->
            let errors = ElementRegistry.getValidationErrors registry
            Assert.Empty(errors)
        )

    [<Fact>]
    let ``ElementRegistry should warn on unknown relationship types from markdown files`` () =
        let rulesXml = """<?xml version="1.0" encoding="UTF-8"?>
<relations>
    <source concept="BusinessActor">
        <target concept="BusinessActor" relations="o"/>
    </source>
</relations>"""

        let sourceElement = """---
id: bus-actr-020-source
name: Source
type: actor
layer: business
relationships:
- type: weird
  target: bus-actr-021-target
  description: Unknown relationship
---

Content.
"""

        let targetElement = """---
id: bus-actr-021-target
name: Target
type: actor
layer: business
---

Content.
"""

        let elements =
            [
                ("source.md", sourceElement)
                ("target.md", targetElement)
            ]

        withTempRegistry elements rulesXml (fun registry ->
            let errors = ElementRegistry.getValidationErrors registry
            Assert.True(errors |> List.exists (fun e -> e.errorType = ErrorType.InvalidRelationshipType "weird"))
            Assert.True(errors |> List.forall (fun e -> e.severity = Severity.Warning))
        )

    [<Fact>]
    let ``ElementRegistry should warn on invalid motivation relationship combinations`` () =
        let rulesXml = """<?xml version="1.0" encoding="UTF-8"?>
<relations>
    <source concept="Requirement">
        <target concept="Principle" relations="n"/>
    </source>
</relations>"""

        let requirementElement = """---
id: mot-reqt-003-mobile-first-banking-design
name: Mobile-First Banking Design
type: requirement
layer: motivation
relationships:
- type: triggering
  target: mot-prin-001-customer-centric-banking
  description: Realizes customer-centric principle
---

Content.
"""

        let principleElement = """---
id: mot-prin-001-customer-centric-banking
name: Customer-Centric Banking
type: principle
layer: motivation
---

Content.
"""

        let elements =
            [
                ("requirement.md", requirementElement)
                ("principle.md", principleElement)
            ]

        withTempRegistry elements rulesXml (fun registry ->
            let errors = ElementRegistry.getValidationErrors registry
            Assert.True(errors |> List.exists (fun e ->
                match e.errorType with
                | ErrorType.InvalidRelationshipCombination ("Requirement", "Principle", "triggering") -> true
                | _ -> false
            ))
            Assert.True(errors |> List.forall (fun e -> e.severity = Severity.Warning))
        )
