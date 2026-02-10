namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive
open TestHelpers

module ElementLoadTests =

    let private buildArchimateContent (lines: string list) : string =
        String.concat "\n" ([ "---" ] @ lines @ [ "---"; ""; "Content." ])
    
    [<Fact>]
    let ``Invalid frontmatter should produce missing ID error`` () =
        let content = "---\n: invalid\n---\nContent."
        let repo, rootDir = createTempRepository [ ("test.md", content) ] []
        let filePath = Path.Combine(rootDir, "archimate", "test.md")

        try
            let errors = repo.validationErrors |> List.filter (fun e -> e.filePath = filePath)
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "missing-id"))
        finally
            cleanupTempDirectory rootDir
    
    [<Fact>]
    let ``Valid element should load successfully`` () =
        let content = buildArchimateContent [
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

        let repo, rootDir = createTempRepository [ ("test.md", content) ] []

        try
            let doc = Map.find "bus-proc-001-customer-onboarding" repo.documents
            Assert.Equal("Customer Onboarding", doc.title)
            Assert.True(match doc with | ArchitectureDoc _ -> true | GovernanceDoc _ -> false)
            Assert.True(match doc.metadata with | DocumentMetaData.ArchiMateMetaData _ -> true | _ -> false)
            Assert.Empty(repo.validationErrors)
        finally
            cleanupTempDirectory rootDir
    
    [<Fact>]
    let ``Element with invalid layer in YAML should be loaded but have error`` () =
        let content = buildArchimateContent [
            "id: elem-001-invalid"
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
            "  layer: invalid-layer"
        ]

        let repo, rootDir = createTempRepository [ ("test.md", content) ] []
        let filePath = Path.Combine(rootDir, "archimate", "test.md")

        try
            let errors = repo.validationErrors |> List.filter (fun e -> e.filePath = filePath)
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "invalid-layer"))
        finally
            cleanupTempDirectory rootDir
