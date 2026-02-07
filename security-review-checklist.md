# F# Code Review Checklist (Security/Best Practices)

Date: 2026-02-05

## High
- [x] Path traversal / arbitrary file read in revalidation endpoint: validate and constrain to elements root before file access. (src/fsharp-server/Handlers.fs, src/fsharp-server/ElementRegistry.fs)
- [x] Script injection risk in Cytoscape HTML: do not interpolate raw JSON into a script block; serialize safely and encode. (src/fsharp-server/DiagramGenerators.fs)

## Medium
- [ ] Response header injection risk in Content-Disposition filename: sanitize `id` before using in header. (src/fsharp-server/Handlers.fs)
- [ ] Inline JS attribute injection via `_hxVals` and `_onclick`: JS-escape values or build JSON safely. (src/fsharp-server/Views.fs)

## Low
- [ ] Potential path traversal / unintended file access for icon lookup with unknown element type: sanitize element type strings before file path usage. (src/fsharp-server/DiagramGenerators.fs)
- [ ] Legacy root Views.fs renders Markdown without sanitization; confirm usage or remove to avoid XSS. (Views.fs)
