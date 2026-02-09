namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive
open TestHelpers

module RegistryTests =

    let private buildArchimateContent (lines: string list) : string =
        String.concat "\n" ([ "---" ] @ lines @ [ "---"; ""; "Content." ])
    
    [<Fact>]
    let ``Registry should collect all validation errors`` () =
        let validContent = buildArchimateContent [
            "id: bus-proc-001-valid"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Valid Element"
            "archimate:"
            "  type: business-process"
            "  layer: business"
        ]

        let invalidContent = buildArchimateContent [
            "id: elem-002-invalid"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test"
            "  layer: business"
        ]

        let repo, rootDir =
            createTempRepository [ ("valid.md", validContent); ("invalid.md", invalidContent) ] []

        try
            let allErrors = repo.validationErrors
            Assert.NotEmpty(allErrors)
            Assert.True(allErrors |> List.exists (fun e -> e.filePath.Contains("invalid.md")))
        finally
            cleanupTempDirectory rootDir
    
    [<Fact>]
    let ``getFileValidationErrors should filter by file path`` () =
        let content1 = buildArchimateContent [
            "id: elem-001"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test"
            "  layer: business"
        ]

        let repo, rootDir = createTempRepository [ ("file1.md", content1) ] []
        let filePath = Path.Combine(rootDir, "archimate", "file1.md")

        try
            let fileErrors = repo.validationErrors |> List.filter (fun e -> e.filePath = filePath)
            Assert.NotEmpty(fileErrors)
            Assert.True(fileErrors |> List.forall (fun e -> e.filePath = filePath))
        finally
            cleanupTempDirectory rootDir
    
    [<Fact>]
    let ``getErrorsBySeverity should filter by severity`` () =
        let content1 = buildArchimateContent [
            "id: bus-proc-001-order-processing"
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "archimate:"
            "  type: test"
            "  layer: business"
        ]

        let repo, rootDir = createTempRepository [ ("file1.md", content1) ] []

        try
            let errors = repo.validationErrors |> List.filter (fun e -> e.severity = Severity.Error)
            Assert.NotEmpty(errors)
        finally
            cleanupTempDirectory rootDir
