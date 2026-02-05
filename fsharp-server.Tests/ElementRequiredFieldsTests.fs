namespace EAArchive.Tests

open System
open Xunit
open EAArchive
open TestHelpers

module ElementRequiredFieldsTests =
    
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
            Assert.Equal("missing-id", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Equal("error", ElementType.severityToString errors.[0].severity)
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
            Assert.Equal("missing-id", ElementType.errorTypeToString errors.[0].errorType)
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
            Assert.Equal("missing-required-field", ElementType.errorTypeToString errors.[0].errorType)
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
            Assert.Equal("missing-required-field", ElementType.errorTypeToString errors.[0].errorType)
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
            Assert.Equal("missing-required-field", ElementType.errorTypeToString errors.[0].errorType)
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
            Assert.Equal("invalid-layer", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Equal("error", ElementType.severityToString errors.[0].severity)
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
    let ``Multiple missing fields should produce multiple errors`` () =
        let metadata = createMetadata [
            ("id", box "bus-proc-001-order-processing")
        ]
        let tempFile = createTempFile "test"
        
        try
            let errors = ElementRegistry.validateElement tempFile metadata
            Assert.Equal(3, List.length errors)
            let errorTypes = errors |> List.map (fun e -> ElementType.errorTypeToString e.errorType)
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
