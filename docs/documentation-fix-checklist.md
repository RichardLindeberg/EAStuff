# Documentation Fix Checklist

Use this checklist to track a full documentation refresh. Mark each item as it is completed.

## 1) Inventory and scope
- [x] List all documentation sources (root README, docs/, src/fsharp-server docs, examples)
- [x] Identify primary audiences and their top tasks
- [x] Confirm the current source of truth (code, configs, schemas, servers)

### Inventory notes
- Sources: root README, docs/, src/fsharp-server/README.md, src/fsharp-server/QUICK-START.md, examples/
- Primary audience: developers/architects using the tool
- Source of truth: current repo contents (F# server, schemas, configs, and actual scripts)

## 2) Commands and paths validation
- [x] Verify every command exists and runs from the stated location
- [x] Verify all script paths are correct and consistent
- [x] Remove or replace references to missing tools/scripts
- [x] Ensure dependency install steps match current requirements

### Validation notes
- Python scripts are removed; F# server workflows are the only source of truth
- All Python command references have been removed or replaced with F# server equivalents

## 3) Version and standards alignment
- [x] Align ArchiMate version references with the active schema
- [x] Align element naming and ID format references with current rules
- [x] Ensure examples reflect current validation rules

## 4) Structure and format consistency
- [x] Confirm repository structure descriptions match actual folders
- [x] Ensure element format examples match actual element files
- [x] Normalize terminology across docs (layers, element types, views)

## 5) Cross-linking and navigation
- [x] Fix broken links and outdated anchors
- [x] Add or update cross-links between related guides
- [x] Ensure entry points (README, quick start) point to correct docs

## 6) Quality review
- [x] Remove stale or contradictory statements
- [x] Check for missing prerequisites or implicit steps
- [x] Run a final read-through for clarity and completeness

## 7) Sign-off
- [x] Record date and reviewer
- [x] Note any follow-up tasks or deferred items

---

### Review log
- Date: 2026-02-07
- Reviewer: Richard
- Notes: None
