# Metadata Comparison: Governance vs ArchiMate Elements

This page compares the YAML frontmatter used in governance documents (management system) and ArchiMate element documents.

## Side-by-Side Comparison

| Area | Governance documents (data/management-system) | ArchiMate elements (data/archimate) |
| --- | --- | --- |
| Purpose | Governance ownership, approvals, lifecycle, applicability. | Architecture modeling of elements and their relationships. |
| Required fields | id, owner, approved_by, status, version, effective_date, review_cycle, next_review. | id, name, type, layer. |
| Relationships | relationships list; can point to governance docs or ArchiMate roles/actors. | relationships list to other ArchiMate elements (and governance docs are allowed targets). |
| Properties | Governance metadata is first-class fields (owner, approval, dates). | Extra metadata goes under properties (owner, status, criticality, last-updated). |
| Validation focus | Governance lifecycle and ownership completeness, duplicate IDs, allowed relation types. | ArchiMate ID format, valid layer/type combinations, ArchiMate relationship rules. |
| File naming | ms-policy-###-short-title.md, ms-instruction-###-short-title.md, ms-manual-###-short-title.md. | [layer-code]-[type-code]-[###]-[descriptive-name].md. |

## Minimal Governance Frontmatter (Example)

```yaml
---
id: ms-policy-001-short-title
owner: bus-role-001-sample-owner
approved_by: bus-role-002-approval-authority
status: active
version: 1.0
effective_date: 2026-01-01
review_cycle: annual
next_review: 2027-01-01
relationships:
  - type: association
    target: bus-role-003-compliance
  - type: ownedBy
    target: bus-role-001-sample-owner
---
```

## Minimal ArchiMate Element Frontmatter (Example)

```yaml
---
id: app-comp-001-customer-portal
name: Customer Portal
type: application-component
layer: application
relationships:
  - type: serving
    target: bus-proc-001-customer-onboarding
    description: Supports onboarding
properties:
  owner: Platform Team
  status: production
  criticality: high
---
```


## Unified Model (Proposal)

This is a proposed unified frontmatter layout with shared fields at the top and domain-specific fields nested under `governance` and `archimate`. Implementing this would require loader and validation changes.

```yaml
---
id: ms-policy-001-short-title
owner: bus-role-001-sample-owner
status: active
version: 1.0
last_updated: 2026-01-01
review_cycle: annual
next_review: 2027-01-01
relationships:
  - type: association
    target: bus-role-003-compliance
  - type: ownedBy
    target: bus-role-001-sample-owner
governance:
  approved_by: bus-role-002-approval-authority
  effective_date: 2026-01-01
archimate:
  type: application-component
  layer: application
  criticality: high
---
```

## Unified Implementation Plan

This plan is ordered for the fastest, safest implementation. Each phase is intended to be a mergeable step.

### Phase 0: Lock the schema

Define the unified frontmatter schema once and treat it as the contract for all subsequent work.

Shared required fields:

- id
- owner
- status
- version
- last_updated
- review_cycle
- next_review

Governance required fields:

- approved_by
- effective_date

ArchiMate required fields:

- type
- layer

### Phase 1: Add typed models

Add the new typed metadata and repository types without changing runtime behavior.

Typed metadata shape (conceptual):

- DocumentMetaData
  - id
  - owner
  - status
  - version
  - lastUpdated
  - reviewCycle
  - nextReview
  - relationships
  - governance: GovernanceMetadata option
  - archimate: ArchimateMetadata option
  - extensions: Map<string, obj> (optional)

Repository shape (Document-based):

- DocumentRepository
  - documents: Map<string, Document>
  - documentsByKind: Map<DocumentKind, string list>
  - documentsByLayer: Map<Layer, string list>
  - documentsByGovernanceType: Map<GovernanceDocType, string list>
  - relations: DocumentRelation list
  - validationErrors: ValidationError list

F# type sketch:

```fsharp
[<RequireQualifiedAccess>]
type DocumentKind =
    | Architecture
    | Governance

type DocumentRepository = {
    documents: Map<string, Document>
    documentsByKind: Map<DocumentKind, string list>
    documentsByLayer: Map<Layer, string list>
    documentsByGovernanceType: Map<GovernanceDocType, string list>
    relations: DocumentRelation list
    validationErrors: ValidationError list
}
```

### Phase 2: Typed parsing with compatibility

Keep current files working while producing typed metadata.

- Deserialize frontmatter directly into typed structures.
- Add a compatibility mapper that projects the current format into the new model.
- Normalize field casing and handle missing optional fields explicitly.

### Phase 3: Build the unified repository

Introduce a new loader that produces `DocumentRepository` while keeping existing registries as adapters.

- Load all markdown from both `data/archimate` and `data/management-system` into `Document` instances.
- Normalize IDs early and ensure no duplicates across document kinds.
- Build derived indexes from the unified list.
- Treat relations as `DocumentRelation` and derive incoming/outgoing views by filtering on `targetId` vs `sourceId`.

### Phase 4: Validation on typed fields

Switch validation to use the typed metadata from the unified repository.

- Governance validation reads from `DocumentMetaData.governance` and shared fields.
- ArchiMate validation reads from `DocumentMetaData.archimate` and shared fields.
- Relationship validation operates over the unified repository so targets resolve across both kinds.
- Validation errors reference unified schema field names.

### Phase 5: UI and handler switch-over

Update handlers and views to consume `DocumentRepository` and typed metadata.

- Keep existing URLs stable by selecting from the unified repository.
- Render metadata from typed fields instead of raw maps.

### Phase 6: Content migration

Migrate frontmatter to the unified schema and remove compatibility paths.

- Add the new nested sections to existing frontmatter.
- Move current `properties` fields into the appropriate nested structure or an `extensions` bag.
- Remove adapters and old registries once all files are migrated.

### Phase 7: Tests and rollout

- Add tests for typed parsing and validation across both document kinds.
- Add migration tests to ensure legacy frontmatter still loads during the transition.
- Remove legacy parsing once all documents are updated.

### Acceptance checklist

- All documents load into `DocumentRepository` with no duplicate IDs.
- Validation warnings/errors match or improve current behavior.
- Governance and element pages render the same information from typed metadata.
- Legacy frontmatter is no longer required.

## Implementation Status (Current)

Completed in the codebase so far:

- Completed: Added unified metadata and repository types.
- Completed: Built a unified repository loader with compatibility mapping for current frontmatter.
- Completed: Switched registry construction to the unified repository as the source of truth.
- Completed: Unified validation (shared fields + domain-specific fields) is now derived from typed metadata.
- Completed: Element frontmatter generation now emits the unified structure while preserving extensions.
- Completed: Tests updated for the new registry signature and validation behavior.
- Completed: Updated UI/view layers to consume `DocumentRepository` and typed metadata directly.
- Completed: Migrated all content to the unified schema and removed compatibility parsing paths.
- Completed: Removed legacy registries and adapters (ElementRegistry/GovernanceDocuments).

## Plan: Retire `ElementRegistry`

The goal is to move all application code to use `DocumentRepository` (and supporting typed models) directly, then remove `ElementRegistry` entirely.

### Step 1: Introduce a repository service abstraction (Completed)

- Create `DocumentRepositoryService` that loads and caches `DocumentRepository` and exposes accessors for:
  - `getDocumentById`
  - `getDocumentsByKind`
  - `getDocumentsByLayer`
  - `getGovernanceDocumentsByType`
  - `getValidationErrors`
- Register the service in DI and update `Program.fs` to inject it into handlers.

### Step 2: Add typed query helpers (Completed)

- Add helper functions to translate `DocumentRecord` into view models for:
  - Architecture element pages
  - Governance document pages
  - Diagrams
  - Validation views
- Keep these helpers in a dedicated module (`DocumentQueries` or `DocumentViewModels`) to avoid handler-level logic duplication.

### Step 3: Update handlers to use the repository (Completed)

- Replace `ElementRegistry` usage in:
  - `Handlers.fs`
  - `Routes.fs`
  - Diagram generators
  - Validation endpoints
- Ensure the URLs and query semantics remain identical.

### Step 4: Update views to consume typed metadata (Completed)

- Replace raw `Map<string, obj>` usage with typed `DocumentMetaData` fields.
- Preserve current UI output and ordering (labels, metadata layouts, and relation lists).

### Step 5: Replace diagram generators (Completed)

- Build diagram nodes and edges from `DocumentRepository.relations` and `DocumentRecord` kinds.
- Use `DocumentKind` to style governance vs architecture nodes.

### Step 6: Delete adapters and legacy registries (Completed)

- Remove any adapter code that maps `DocumentRepository` back into `Element`/`GovernanceDocument`.
- Delete `ElementRegistry.fs` and `GovernanceDocuments.fs` once no references remain.

### Step 7: Cleanup and verify (In Progress)

- Update tests to use repository services directly.
- Remove deprecated helper functions and old validation paths.
- Run full test suite and validation pages for parity.



## References

- Governance metadata guidance: [docs/management-system/index.md](docs/management-system/index.md)
- ArchiMate element guidance: [docs/quick-start.md](docs/quick-start.md)
- ArchiMate best practices: [docs/best-practices.md](docs/best-practices.md)
