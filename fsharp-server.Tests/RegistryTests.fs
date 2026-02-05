namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive
open TestHelpers

module RegistryTests =
    
    [<Fact>]
    let ``Registry should collect all validation errors`` () =
        let tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        Directory.CreateDirectory(tempDir) |> ignore
        
        try
            let validFile = Path.Combine(tempDir, "valid.md")
            let validContent = """---
id: bus-proc-001
name: Valid Element
type: Business Process
layer: business
---

Content.
"""
            File.WriteAllText(validFile, validContent)
            
            let invalidFile = Path.Combine(tempDir, "invalid.md")
            let invalidContent = """---
id: elem-002
type: Test
layer: business
---

Content.
"""
            File.WriteAllText(invalidFile, invalidContent)
            
            let registry = ElementRegistry.create tempDir
            let allErrors = ElementRegistry.getValidationErrors registry
            
            Assert.NotEmpty(allErrors)
            Assert.True(allErrors |> List.exists (fun e -> e.filePath.Contains("invalid.md")))
        finally
            Directory.Delete(tempDir, true)
    
    [<Fact>]
    let ``getFileValidationErrors should filter by file path`` () =
        let tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        Directory.CreateDirectory(tempDir) |> ignore
        
        try
            let file1 = Path.Combine(tempDir, "file1.md")
            let content1 = """---
id: elem-001
type: Test
---

Content.
"""
            File.WriteAllText(file1, content1)
            
            let registry = ElementRegistry.create tempDir
            let fileErrors = ElementRegistry.getFileValidationErrors file1 registry
            
            Assert.NotEmpty(fileErrors)
            Assert.True(fileErrors |> List.forall (fun e -> e.filePath = file1))
        finally
            Directory.Delete(tempDir, true)
    
    [<Fact>]
    let ``getErrorsBySeverity should filter by severity`` () =
        let tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        Directory.CreateDirectory(tempDir) |> ignore
        
        try
            // Create a file with missing name (error) - ID format is correct
            let file1 = Path.Combine(tempDir, "file1.md")
            let content1 = """---
id: bus-proc-001-order-processing
type: Test
layer: business
---

Content.
"""
            File.WriteAllText(file1, content1)
            
            let registry = ElementRegistry.create tempDir
            let errors = ElementRegistry.getErrorsBySeverity Severity.Error registry
            
            // Should have at least one error for missing name
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> ElementType.severityToString e.severity = "error"))
        finally
            Directory.Delete(tempDir, true)
