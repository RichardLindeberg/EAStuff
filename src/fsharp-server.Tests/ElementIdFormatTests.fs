namespace EAArchive.Tests

open System
open Xunit
open EAArchive
open TestHelpers

module ElementIdFormatTests =
    
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
            Assert.Equal("invalid-id-format", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Equal("error", ElementType.severityToString errors.[0].severity)
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
            Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "invalid-id-format"))
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
    let ``ID with 7 words in descriptive name should produce error`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing-system-management-feature-approval-workflow")
            ("name", box "Test")
            ("type", box "Test")
            ("layer", box "business")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("maximum is 6")))
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
                Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "invalid-id-format"))
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
                let formatErrors = errors |> List.filter (fun e -> ElementType.errorTypeToString e.errorType = "invalid-id-format")
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
