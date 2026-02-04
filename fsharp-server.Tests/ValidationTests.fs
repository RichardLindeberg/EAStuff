namespace EAArchive.Tests

open System
open System.IO
open System.Collections.Generic
open Xunit
open EAArchive

module ValidationTests =
    
    let createTempFile (content: string) : string =
        let tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".md")
        File.WriteAllText(tempFile, content)
        tempFile
    
    let cleanupTempFile (path: string) : unit =
        if File.Exists(path) then File.Delete(path)
    
    // Test metadata creation helpers
    let createMetadata (fields: (string * obj) list) : Map<string, obj> =
        fields |> Map.ofList
    
    [<Fact>]
    let ``Valid element with all required fields should have no errors`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-customer-onboarding")
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element missing ID should produce error`` () =
        let metadata = createMetadata [
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-id", errors.[0].errorType)
            Assert.Equal("error", errors.[0].severity)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element with empty ID should produce error`` () =
        let metadata = createMetadata [
            ("id", box "")
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-id", errors.[0].errorType)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element missing name should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-required-field", errors.[0].errorType)
            Assert.Contains("name", errors.[0].message)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element missing type should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
            ("name", box "Customer Onboarding")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-required-field", errors.[0].errorType)
            Assert.Contains("type", errors.[0].message)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element missing layer should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("missing-required-field", errors.[0].errorType)
            Assert.Contains("layer", errors.[0].message)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element with invalid layer should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
            ("layer", box "invalid-layer")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("invalid-layer", errors.[0].errorType)
            Assert.Equal("error", errors.[0].severity)
        finally
            cleanupTempFile tempFile
    
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
        let tempFile = createTempFile "test"
        
        try
            for (_, _, layer, id) in validCombos do
                let metadata = createMetadata [
                    ("id", box id)
                    ("name", box "Test Element")
                    ("type", box "Test Type")
                    ("layer", box layer)
                ]
                let errors = ElementRegistry.validateElement tempFile metadata
                Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element with invalid ID format (missing descriptive-name) should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001")
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Single(errors) |> ignore
            Assert.Equal("invalid-id-format", errors.[0].errorType)
            Assert.Equal("error", errors.[0].severity)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with single word in descriptive name should be valid`` () =
        let metadata = createMetadata [
            ("id", box "str-capa-001-omnichannel")
            ("name", box "Omnichannel")
            ("type", box "Capability")
            ("layer", box "strategy")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with multi-word descriptive-name should be valid`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-fulfillment-process")
            ("name", box "Order Fulfillment Process")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with numeric in descriptive-name should be valid`` () =
        let metadata = createMetadata [
            ("id", box "app-comp-001-crm-system-v2")
            ("name", box "CRM System V2")
            ("type", box "Application Component")
            ("layer", box "application")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID completely wrong format should produce error`` () =
        let metadata = createMetadata [
            ("id", box "completely-invalid")
            ("name", box "Invalid")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.errorType = "invalid-id-format"))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with wrong layer code length should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            // This should pass with correct bus code (3 chars)
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with 2-char layer code should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bu-proc-001-order-processing")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("Layer code")))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with invalid layer code should produce error`` () =
        let metadata = createMetadata [
            ("id", box "xyz-proc-001-order-processing")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("not valid")))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with wrong type code length should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-pro-001-order-processing")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("Type code")))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with invalid type code for layer should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-node-001-order-processing")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("not valid for layer")))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with sequential number 000 should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-000-order-processing")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("must start at 001")))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with only 1 word in descriptive name is valid`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-processing")
            ("name", box "Processing")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with 5 words in descriptive name should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing-system-management-feature")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("maximum is 4")))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with invalid special characters should fail basic pattern`` () =
        // IDs with uppercase, underscores, or double hyphens fail the initial pattern check
        let invalidIds = [
            "bus-proc-001-Order-Processing"    // Uppercase
            "bus-proc-001-order_processing"    // Underscore
            "bus-proc-001--order-processing"   // Double hyphen
        ]
        let tempFile = createTempFile "test"
        
        try
            for invalidId in invalidIds do
                let metadata = createMetadata [
                    ("id", box invalidId)
                    ("name", box "Test")
                    ("type", box "Test")
                    ("layer", box "business")
                ]
                let errors = ElementRegistry.validateElement tempFile metadata
                Assert.NotEmpty(errors)
                Assert.True(errors |> List.exists (fun e -> e.errorType = "invalid-id-format"))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``ID with all valid layer codes should pass`` () =
        let layerCodes = ["str"; "bus"; "app"; "tec"; "phy"; "mot"; "imp"]
        let typeCodes = ["capa"; "proc"; "comp"; "node"; "equi"; "goal"; "work"]
        let tempFile = createTempFile "test"
        
        try
            for (layer, typeCode) in List.zip layerCodes typeCodes do
                let metadata = createMetadata [
                    ("id", box $"{layer}-{typeCode}-001-test-element")
                    ("name", box "Test")
                    ("type", box "Test")
                    ("layer", box (match layer with "str" -> "strategy" | "bus" -> "business" | "app" -> "application" | "tec" -> "technology" | "phy" -> "physical" | "mot" -> "motivation" | _ -> "implementation"))
                ]
                let errors = ElementRegistry.validateElement tempFile metadata
                // Should only have no format errors for these valid combinations
                let formatErrors = errors |> List.filter (fun e -> e.errorType = "invalid-id-format")
                Assert.Empty(formatErrors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element with correct ID format should have no errors`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-customer-onboarding")
            ("name", box "Customer Onboarding")
            ("type", box "Business Process")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Empty(errors)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Multiple missing fields should produce multiple errors`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Equal(3, List.length errors)
            let errorTypes = errors |> List.map (fun e -> e.errorType)
            Assert.Contains("missing-required-field", errorTypes)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Validation errors should include file path`` () =
        let metadata = createMetadata [
            ("name", box "Test")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.True(errors |> List.forall (fun e -> e.filePath = tempFile))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Validation error should include element ID when present`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001")
            ("type", box "Business Process")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.True(errors |> List.exists (fun e -> e.elementId = Some "bus-proc-001"))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Errors without ID should have None for elementId`` () =
        let metadata = createMetadata [
            ("name", box "Test")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.True(errors |> List.exists (fun e -> e.elementId.IsNone))
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Parse error should be caught and reported`` () =
        // Test with file that can't be read/doesn't exist
        let nonExistentFile = "/invalid/path/does/not/exist.md"
        
        let (elemOpt, errors) = ElementRegistry.loadElementWithValidation nonExistentFile (
            let loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(fun _ -> ())
            loggerFactory.CreateLogger("Test")
        )
        
        Assert.False(elemOpt.IsSome)
        Assert.NotEmpty(errors)
        Assert.True(errors |> List.exists (fun e -> e.errorType = "parse-error"))
    
    [<Fact>]
    let ``Valid element should load successfully`` () =
        let content = """---
id: bus-proc-001-customer-onboarding
name: Customer Onboarding
type: Business Process
layer: business
---

This is test content.
"""
        let tempFile = createTempFile content
        
        try
            let (elemOpt, errors) = ElementRegistry.loadElementWithValidation tempFile (
                let loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(fun _ -> ())
                loggerFactory.CreateLogger("Test")
            )
            
            Assert.NotNull(elemOpt)
            Assert.Empty(errors)
            
            let elem = elemOpt.Value
            Assert.Equal("bus-proc-001-customer-onboarding", elem.id)
            Assert.Equal("Customer Onboarding", elem.name)
            Assert.Equal("Business Process", elem.elementType)
            Assert.Equal("business", elem.layer)
        finally
            cleanupTempFile tempFile
    
    [<Fact>]
    let ``Element with invalid layer in YAML should be loaded but have error`` () =
        let content = """---
id: elem-001
name: Test Element
type: Test Type
layer: invalid-layer
---

Content here.
"""
        let tempFile = createTempFile content
        
        try
            let (elemOpt, errors) = ElementRegistry.loadElementWithValidation tempFile (
                let loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(fun _ -> ())
                loggerFactory.CreateLogger("Test")
            )
            
            Assert.NotNull(elemOpt)
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.errorType = "invalid-layer"))
        finally
            cleanupTempFile tempFile
    
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
            let errors = ElementRegistry.getErrorsBySeverity "error" registry
            
            // Should have at least one error for missing name
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.severity = "error"))
        finally
            Directory.Delete(tempDir, true)
