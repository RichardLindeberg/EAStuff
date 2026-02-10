namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive
open TestHelpers

module RelationshipValidationTests =
    let private buildArchimateContent (lines: string list) : string =
        String.concat "\n" ([ "---" ] @ lines @ [ "---"; ""; "Content." ])

    let private buildGovernanceContent (lines: string list) : string =
        String.concat "\n" ([ "---" ] @ lines @ [ "---"; ""; "Content." ])

    let private withGovernanceErrors (govLines: string list) (assertFn: ValidationError list -> unit) : unit =
        let ownerElement = buildArchimateContent [
            "id: bus-role-001-owner"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Owner Role"
            "archimate:"
            "  type: business-role"
            "  layer: business"
        ]

        let targetElement = buildArchimateContent [
            "id: bus-role-002-target"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Target Role"
            "archimate:"
            "  type: business-role"
            "  layer: business"
        ]

        let governanceContent = buildGovernanceContent govLines
        let repo, rootDir =
            createTempRepository
                [ ("owner.md", ownerElement); ("target.md", targetElement) ]
                [ ("policy.md", governanceContent) ]
        let filePath = Path.Combine(rootDir, "management-system", "policy.md")

        try
            let errors = repo.validationErrors |> List.filter (fun e -> e.filePath = filePath)
            assertFn errors
        finally
            cleanupTempDirectory rootDir

    [<Fact>]
    let ``parseRelationshipRules should load valid XML`` () =
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

        let tempPath = Path.GetTempFileName()
        try
            File.WriteAllText(tempPath, testXml)

            let rules = ElementType.parseRelationshipRules tempPath

            Assert.NotEmpty(rules)
            Assert.True(Map.count rules > 0)

            match Map.tryFind ("BusinessActor", "BusinessRole") rules with
            | Some codes ->
                Assert.True(Set.contains 'i' codes)
                Assert.True(Set.contains 'o' codes)
                Assert.False(Set.contains 'c' codes)
            | None -> Assert.True(false, "Expected BusinessActor->BusinessRole rule not found")

            match Map.tryFind ("ApplicationComponent", "DataObject") rules with
            | Some codes ->
                Assert.True(Set.contains 'a' codes)
                Assert.False(Set.contains 'c' codes)
            | None -> Assert.True(false, "Expected ApplicationComponent->DataObject rule not found")
        finally
            File.Delete(tempPath)

    [<Fact>]
    let ``parseRelationshipRules should handle malformed XML gracefully`` () =
        let badXml = "<invalid>xml</content>"
        let tempPath = Path.GetTempFileName()
        try
            File.WriteAllText(tempPath, badXml)

            let rules = ElementType.parseRelationshipRules tempPath

            Assert.Empty(rules)
        finally
            File.Delete(tempPath)

    [<Fact>]
    let ``parseRelationshipRules should handle missing file gracefully`` () =
        let rules = ElementType.parseRelationshipRules "nonexistent-file.xml"
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
    let ``governance relationship with missing target should be ignored`` () =
        let lines = [
            "id: ms-policy-001-test"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships:"
            "  - type: association"
            "name: Test Policy"
            "governance:"
            "  approved_by: Board"
            "  effective_date: 2026-01-01"
        ]

        withGovernanceErrors lines (fun errors ->
            Assert.Empty(errors)
        )

    [<Fact>]
    let ``governance relationship with unknown target should produce error`` () =
        let lines = [
            "id: ms-policy-002-test"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships:"
            "  - type: association"
            "    target: missing-target"
            "name: Test Policy"
            "governance:"
            "  approved_by: Board"
            "  effective_date: 2026-01-01"
        ]

        withGovernanceErrors lines (fun errors ->
            Assert.True(errors |> List.exists (fun e ->
                match e.errorType with
                | ErrorType.RelationshipTargetNotFound _ -> true
                | _ -> false
            ))
        )

    [<Fact>]
    let ``governance relationship with missing type should be ignored`` () =
        let lines = [
            "id: ms-policy-003-test"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships:"
            "  - target: bus-role-002-target"
            "name: Test Policy"
            "governance:"
            "  approved_by: Board"
            "  effective_date: 2026-01-01"
        ]

        withGovernanceErrors lines (fun errors ->
            Assert.Empty(errors)
        )
