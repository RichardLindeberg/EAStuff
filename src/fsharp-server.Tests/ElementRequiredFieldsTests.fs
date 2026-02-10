namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive
open TestHelpers

module ElementRequiredFieldsTests =

    let private buildArchimateContent (lines: string list) : string =
        String.concat "\n" ([ "---" ] @ lines @ [ "---"; ""; "Content." ])

    let private withArchimateErrors (lines: string list) (assertFn: ValidationError list -> unit) : unit =
        let content = buildArchimateContent lines
        let repo, rootDir = createTempRepository [ ("test.md", content) ] []
        let filePath = Path.Combine(rootDir, "archimate", "test.md")

        try
            let errors = repo.validationErrors |> List.filter (fun e -> e.filePath = filePath)
            assertFn errors
        finally
            cleanupTempDirectory rootDir
    
    [<Fact>]
    let ``Valid element with all required fields should have no errors`` () =
        let lines = [
            "id: bus-proc-001-customer-onboarding"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Customer Onboarding"
            "archimate:"
            "  type: business-process"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors -> Assert.Empty(errors))
    
    [<Fact>]
    let ``Element missing ID should produce error`` () =
        let lines = [
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Customer Onboarding"
            "archimate:"
            "  type: business-process"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-id", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Equal("error", ElementType.severityToString errors.[0].severity)
        )
    
    [<Fact>]
    let ``Element with empty ID should produce error`` () =
        let lines = [
            "id:"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Customer Onboarding"
            "archimate:"
            "  type: business-process"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-id", ElementType.errorTypeToString errors.[0].errorType)
        )
    
    [<Fact>]
    let ``Element missing name should produce error`` () =
        let lines = [
            "id: bus-proc-001-order-processing"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: business-process"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-required-field", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Contains("name", errors.[0].message)
        )
    
    [<Fact>]
    let ``Element missing type should produce error`` () =
        let lines = [
            "id: bus-proc-001-order-processing"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Customer Onboarding"
            "archimate:"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-required-field", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Contains("archimate", errors.[0].message)
        )
    
    [<Fact>]
    let ``Element missing layer should produce error`` () =
        let lines = [
            "id: bus-proc-001-order-processing"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Customer Onboarding"
            "archimate:"
            "  type: business-process"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-required-field", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Contains("archimate", errors.[0].message)
        )
    
    [<Fact>]
    let ``Element with invalid layer should produce error`` () =
        let lines = [
            "id: bus-proc-001-order-processing"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Customer Onboarding"
            "archimate:"
            "  type: business-process"
            "  layer: invalid-layer"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("invalid-layer", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Equal("error", ElementType.severityToString errors.[0].severity)
        )
    
    [<Fact>]
    let ``Valid layers should not produce errors`` () =
        let validCombos = [
            ("str", "capa", "strategy", "str-capa-001-omnichannel")
            ("bus", "proc", "business", "bus-proc-001-order-processing")
            ("app", "comp", "application", "app-comp-001-customer-portal")
            ("tec", "node", "technology", "tec-node-001-web-server")
            ("phy", "equi", "physical", "phy-equi-001-data-center")
            ("mot", "goal", "motivation", "mot-goal-001-revenue")
            ("imp", "work", "implementation", "imp-work-001-cloud-migration")
        ]
        for (_, _, layer, id) in validCombos do
            let lines = [
                sprintf "id: %s" id
                "owner: bus-role-001-owner"
                "status: active"
                "version: 1.0"
                "last_updated: 2026-01-01"
                "review_cycle: annual"
                "next_review: 2027-01-01"
                "relationships: []"
                "name: Test Element"
                "archimate:"
                "  type: test-type"
                sprintf "  layer: %s" layer
            ]

            withArchimateErrors lines (fun errors -> Assert.Empty(errors))

    [<Fact>]
    let ``Multiple missing fields should produce multiple errors`` () =
        let lines = [
            "id: bus-proc-001-order-processing"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test-type"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.Equal(1, List.length errors)
            let errorTypes = errors |> List.map (fun e -> ElementType.errorTypeToString e.errorType)
            Assert.Contains("missing-required-field", errorTypes)
        )
    
    [<Fact>]
    let ``Validation errors should include file path`` () =
        let lines = [
            "name: Test"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test-type"
            "  layer: business"
        ]

        let content = buildArchimateContent lines
        let repo, rootDir = createTempRepository [ ("test.md", content) ] []
        let filePath = Path.Combine(rootDir, "archimate", "test.md")

        try
            let errors = repo.validationErrors |> List.filter (fun e -> e.filePath = filePath)
            Assert.True(errors |> List.forall (fun e -> e.filePath = filePath))
        finally
            cleanupTempDirectory rootDir
    
    [<Fact>]
    let ``Validation error should include element ID when present`` () =
        let lines = [
            "id: bus-proc-001"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test-type"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.True(errors |> List.exists (fun e -> e.elementId = Some "bus-proc-001"))
        )
    
    [<Fact>]
    let ``Errors without ID should have None for elementId`` () =
        let lines = [
            "name: Test"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test-type"
            "  layer: business"
        ]

        withArchimateErrors lines (fun errors ->
            Assert.True(errors |> List.exists (fun e -> e.elementId.IsNone))
        )
