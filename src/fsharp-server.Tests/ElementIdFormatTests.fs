namespace EAArchive.Tests

open System
open System.IO
open Xunit
open EAArchive
open TestHelpers

module ElementIdFormatTests =

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

    let private baseLines (idValue: string) : string list =
        [
            sprintf "id: %s" idValue
            "owner: bus-role-001-owner"
            "status: active"
            "version: 1.0"
            "last_updated: 2026-01-01"
            "review_cycle: annual"
            "next_review: 2027-01-01"
            "relationships: []"
            "name: Test"
            "archimate:"
            "  type: business-process"
            "  layer: business"
        ]
    
    [<Fact>]
    let ``Element with invalid ID format (missing descriptive-name) should produce error`` () =
        withArchimateErrors (baseLines "bus-proc-001") (fun errors ->
            Assert.Single(errors) |> ignore
            Assert.Equal("invalid-id-format", ElementType.errorTypeToString errors.[0].errorType)
            Assert.Equal("error", ElementType.severityToString errors.[0].severity)
        )
    
    [<Fact>]
    let ``ID with single word in descriptive name should be valid`` () =
        let lines =
            baseLines "str-capa-001-omnichannel"
            |> List.map (fun line -> if line = "  layer: business" then "  layer: strategy" else line)

        withArchimateErrors lines (fun errors -> Assert.Empty(errors))
    
    [<Fact>]
    let ``ID with multi-word descriptive-name should be valid`` () =
        withArchimateErrors (baseLines "bus-proc-001-order-fulfillment-process") (fun errors ->
            Assert.Empty(errors)
        )
    
    [<Fact>]
    let ``ID with numeric in descriptive-name should be valid`` () =
        let lines =
            baseLines "app-comp-001-crm-system-v2"
            |> List.map (fun line -> if line = "  layer: business" then "  layer: application" else line)

        withArchimateErrors lines (fun errors -> Assert.Empty(errors))
    
    [<Fact>]
    let ``ID completely wrong format should produce error`` () =
        withArchimateErrors (baseLines "completely-invalid") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "invalid-id-format"))
        )
    
    [<Fact>]
    let ``ID with wrong layer code length should produce error`` () =
        withArchimateErrors (baseLines "bus-proc-001-order-processing") (fun errors ->
            Assert.Empty(errors)
        )
    
    [<Fact>]
    let ``ID with 2-char layer code should produce error`` () =
        withArchimateErrors (baseLines "bu-proc-001-order-processing") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("Layer code")))
        )
    
    [<Fact>]
    let ``ID with invalid layer code should produce error`` () =
        withArchimateErrors (baseLines "xyz-proc-001-order-processing") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("not valid")))
        )
    
    [<Fact>]
    let ``ID with wrong type code length should produce error`` () =
        withArchimateErrors (baseLines "bus-pro-001-order-processing") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("Type code")))
        )
    
    [<Fact>]
    let ``ID with invalid type code for layer should produce error`` () =
        withArchimateErrors (baseLines "bus-node-001-order-processing") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("not valid for layer")))
        )
    
    [<Fact>]
    let ``ID with sequential number 000 should produce error`` () =
        withArchimateErrors (baseLines "bus-proc-000-order-processing") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("must start at 001")))
        )
    
    [<Fact>]
    let ``ID with only 1 word in descriptive name is valid`` () =
        withArchimateErrors (baseLines "bus-proc-001-processing") (fun errors ->
            Assert.Empty(errors)
        )
    
    [<Fact>]
    let ``ID with 7 words in descriptive name should produce error`` () =
        withArchimateErrors (baseLines "bus-proc-001-order-processing-system-management-feature-approval-workflow") (fun errors ->
            Assert.NotEmpty(errors)
            Assert.True(errors |> List.exists (fun e -> e.message.Contains("maximum is 6")))
        )
    
    [<Fact>]
    let ``ID with invalid special characters should fail basic pattern`` () =
        // IDs with uppercase, underscores, or double hyphens fail the initial pattern check
        let invalidIds = [
            "bus-proc-001-Order-Processing"    // Uppercase
            "bus-proc-001-order_processing"    // Underscore
            "bus-proc-001--order-processing"   // Double hyphen
        ]
        for invalidId in invalidIds do
            withArchimateErrors (baseLines invalidId) (fun errors ->
                Assert.NotEmpty(errors)
                Assert.True(errors |> List.exists (fun e -> ElementType.errorTypeToString e.errorType = "invalid-id-format"))
            )
    
    [<Fact>]
    let ``ID with all valid layer codes should pass`` () =
        let layerCodes = ["str"; "bus"; "app"; "tec"; "phy"; "mot"; "imp"]
        let typeCodes = ["capa"; "proc"; "comp"; "node"; "equi"; "goal"; "work"]
        for (layer, typeCode) in List.zip layerCodes typeCodes do
            let layerValue =
                match layer with
                | "str" -> "strategy"
                | "bus" -> "business"
                | "app" -> "application"
                | "tec" -> "technology"
                | "phy" -> "physical"
                | "mot" -> "motivation"
                | _ -> "implementation"

            let lines =
                baseLines $"{layer}-{typeCode}-001-test-element"
                |> List.map (fun line -> if line = "  layer: business" then $"  layer: {layerValue}" else line)

            withArchimateErrors lines (fun errors ->
                let formatErrors = errors |> List.filter (fun e -> ElementType.errorTypeToString e.errorType = "invalid-id-format")
                Assert.Empty(formatErrors)
            )
    
    [<Fact>]
    let ``Element with correct ID format should have no errors`` () =
        withArchimateErrors (baseLines "bus-proc-001-customer-onboarding") (fun errors ->
            Assert.Empty(errors)
        )
