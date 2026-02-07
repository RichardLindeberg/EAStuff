Phase 1: Domain Types & Schema Parsing
[x] Add new error types to ErrorType in Domain.fs:113-121

[x] Add InvalidRelationshipType of string (for unrecognized relationship types)
[x] Add RelationshipTargetNotFound of string * string (elementId * targetId)
[x] Add InvalidRelationshipCombination of string * string * string (sourceType * targetType * relationType)
[x] Add SelfReference of string (elementId)
[x] Add DuplicateRelationship of string * string (elementId * targetId)
[x] Create relationship rules type in Domain.fs

[x] Define type RelationshipRules = Map<string * string, Set<char>>
[x] Map key: (sourceElementType, targetElementType) as ArchiMate concept names
[x] Map value: Set of allowed relationship code chars (a, c, f, g, i, n, o, r, s, t, v)
[x] Create XML parser function in Domain.fs

[x] Add parseRelationshipRules: string -> RelationshipRules function
[x] Parse relations.xml using System.Xml.Linq
[x] For each <source concept="X">, iterate <target concept="Y" relations="abc"/>
[x] Build map from (X, Y) → Set of relation chars
[x] Handle parse errors gracefully (return empty map with logged warning)
[x] Add RelationType to char mapping in Domain.fs

[x] Create relationTypeToCode: RelationType -> char option function
[x] Map: Composition→'c', Aggregation→'g', Assignment→'i', Realization→'r', Specialization→'s'
[x] Map: Association→'o', Access→'a', Influence→'n', Serving→'v', Triggering→'t', Flow→'f'
[x] Unknown types return None
[x] Add ElementType to concept name mapping in Domain.fs

[x] Create elementTypeToConceptName: ElementType -> string function
[x] Map F# types (e.g., ApplicationElement.Component) to ArchiMate names (e.g., "ApplicationComponent")
[x] Cover all 60+ element types across all layers
Phase 2: ElementRegistry Validation Logic
[x] Load relationship rules at startup in ElementRegistry.fs

[x] In create function (line ~373), load rules before processing elements
[x] Call Domain.parseRelationshipRules with path to relations.xml
[x] Store rules in a local variable for validation use
[x] Log success/failure of rules loading
[x] Create relationship validation function in ElementRegistry.fs

[x] Add validateRelationships: RelationshipRules -> ElementRegistry -> Element -> ValidationError list
[x] Takes: rules map, registry (for target lookup), source element
[x] Returns: list of validation warnings
[x] Implement target existence check

[x] For each relationship in element.relationships
[x] Check if Map.containsKey relationship.target registry.elements
[x] If not found, create RelationshipTargetNotFound warning with Severity.Warning
[x] Implement self-reference check

[x] For each relationship in element.relationships
[x] Check if relationship.target = element.id
[x] If true, create SelfReference warning with Severity.Warning
[x] Implement duplicate relationship check

[x] Group relationships by target
[x] Check for multiple relationships to same target with same type
[x] Create DuplicateRelationship warning with Severity.Warning
[x] Implement ArchiMate rules validation

[x] For each relationship in element.relationships
[x] Get source concept name: elementTypeToConceptName element.elementType
[x] Get target element from registry, get target concept name
[x] Get relationship code: relationTypeToCode relationship.relationType
[x] Look up allowed codes: Map.tryFind (sourceConcept, targetConcept) rules
[x] If relationship code not in allowed set, create InvalidRelationshipCombination warning
[x] Include helpful message listing valid relationship types for that combination
[x] Add second validation pass in ElementRegistry.fs

[x] After line 433 in create function (after all elements loaded)
[x] Iterate through all loaded elements in registry
[x] Call validateRelationships for each element
[x] Accumulate all relationship warnings
[x] Append to allValidationErrors list
[x] Update registry.validationErrors ref with combined errors
Phase 3: Error Message Formatting
[x] Update error message generation in ElementRegistry.fs or Handlers.fs

[x] Add pattern match cases for new error types in error-to-string conversion
[x] RelationshipTargetNotFound: "Relationship target '{targetId}' not found"
[x] InvalidRelationshipCombination: "Relationship '{relType}' not allowed from {sourceType} to {targetType}. Valid types: {validTypes}"
[x] SelfReference: "Element cannot have relationship to itself"
[x] DuplicateRelationship: "Duplicate relationship to '{targetId}'"
[x] InvalidRelationshipType: "Unrecognized relationship type '{type}'"
[x] Enhance validation view in Views.fs

[x] Update error display to show source and target element links for relationship errors
[x] Add special formatting for relationship warnings (distinct from other warnings)
[x] Include relationship type and description in error display
Phase 4: Testing
[x] Create test file at src/fsharp-server.Tests/RelationshipValidationTests.fs

[x] Add to EAArchive.Tests.fsproj
[x] Write XML parsing tests

[x] Test parsing valid relations.xml snippet
[x] Test handling malformed XML gracefully
[x] Verify correct map structure created
[x] Write validation tests

[x] Test missing target detection (element references non-existent target)
[x] Test self-reference detection (element references itself)
[x] Test duplicate relationship detection (same target, same type)
[x] Test invalid relationship combination (BusinessActor -composition-> ApplicationComponent)
[x] Test valid relationships produce no warnings
[x] Test unknown relationship types produce warnings
[x] Write integration tests

[x] Create test .md files with various invalid relationships
[x] Load into test registry
[x] Assert correct warnings generated
[x] Verify warnings have Severity.Warning (not Error)
Phase 5: Integration & Verification
 Test with existing elements

 Run server locally: dotnet run in src/fsharp-server
 Navigate to http://localhost:5000/validation
 Verify relationship warnings appear (if any exist)
 Check warning styling (yellow, not red)
 Verify API endpoints

 Test GET /api/validation/errors returns relationship warnings
 Test GET /api/validation/stats counts relationship warnings correctly
 Verify JSON structure includes new error types
 Create test elements for validation

 Create element with missing target reference
 Create element with self-reference
 Create element with invalid relationship type (e.g., composition from BusinessActor to ApplicationComponent)
 Verify all appear in validation report
 Documentation

 [x] Update README.md mentioning relationship validation
 [x] Consider adding validation rules documentation reference
Phase 6: Optional Enhancements
 Add validation statistics

 Track count of relationship warnings separately
 Display in validation stats endpoint
 Add filtering in UI

 Allow filtering validation page by error type
 Add "Relationship Warnings" filter option
 Performance optimization

 Profile relationship validation performance on large repositories
 Add caching if needed for relationship rules lookup
 Helper endpoint

 Create GET /api/relationships/allowed/{sourceType}/{targetType} endpoint
 Returns list of valid relationship types for given source/target combo
 Useful for developers creating new relationships