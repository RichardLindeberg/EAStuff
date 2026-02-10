# Duplicate Removal Plan

## Candidates for shared helpers

1) Governance doc type detection
- Duplicate logic in DocumentRepository.DocumentRepositoryLoader.docTypeFromPath and DocumentQueries.docTypeFromPath.
- Plan: move to a single helper (e.g., DocumentTypeHelpers.getGovernanceDocTypeFromPath) and reuse in both modules.

2) Tag index construction
- Duplicate buildTagIndex in HandlersHelpers and DocumentQueries.
- Plan: move to a shared helper module (e.g., TagIndex.buildTagIndex) used by handlers and query helpers.

3) Layer/type code maps for ArchiMate IDs
- Layer/type code maps in HandlersHelpers (ID generation) and DocumentRepository (ID validation).
- Plan: centralize code maps in a single module (e.g., ElementIdCodes) with lookup + validation helpers.

4) Query string extraction with defaults
- Repeated match ctx.GetQueryStringValue patterns for string values and int parsing in multiple handlers.
- Plan: introduce small helper functions (e.g., tryGetQueryString, tryGetQueryStringInt, tryGetQueryStringLower) in HandlersHelpers or a new HttpContextHelpers module.

5) Repeated 404 handling blocks
- Multiple handlers repeat the same "not found" logging + setStatusCode 404 >=> text message patterns.
- Plan: create a helper (e.g., respondNotFound logger message) to consolidate behavior and text.

## Suggested order

1) Extract pure functions first (doc type detection, tag index).
2) Centralize element code maps to avoid drift between generation and validation.
3) Add HTTP helpers for query parsing and 404 responses.

## Notes

- Keep helpers private-by-default and expose only shared functions.
- Prefer small, composable functions that keep handler logic readable.
