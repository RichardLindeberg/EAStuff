namespace EAArchive.Tests

open System
open Xunit
open EAArchive
open TestHelpers

module ElementLoadTests =
    
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
        Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "parse-error"))
    
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
            let elemTypeStr = Views.elementTypeToString elem.elementType
            Assert.Equal("Business Process", elemTypeStr)
            let layer = ElementType.getLayer elem.elementType
            Assert.Equal("Business", layer)
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
            Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "invalid-layer"))
        finally
            cleanupTempFile tempFile
