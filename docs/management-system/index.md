# Governance System Documentation

This section explains how governance system documents are organized and how to author them. These documents are separate from ArchiMate element definitions in data/archimate, but they do use YAML frontmatter for metadata.

## What Each Document Type Means

- Policy: A mandatory rule or principle that defines what must be followed and why.
- Instruction: A step-by-step procedure for how to carry out a policy or task.
- Manual: A broader reference guide that explains processes, context, and detailed usage.

## Where Files Live

The documentation about the management system lives here in docs/management-system. The actual policies, instructions, and manuals live under data/management-system so they are clearly separated from product documentation.

## Naming Convention

Use the following format for file names:

- Policies: ms-policy-001-short-title.md
- Instructions: ms-instruction-001-short-title.md
- Manuals: ms-manual-001-short-title.md

Keep the numeric sequence per type and use short, lowercase, hyphenated titles.

## Minimal Metadata

Add a YAML frontmatter block near the top of each document to capture governance ownership, approvals, and applicability. Use a relationships list similar to ArchiMate, and use ArchiMate IDs for roles/actors (for example, business-role or business-actor IDs).

Format:

---
id: ms-policy-001-short-title
owner:
approved_by:
status:
version:
effective_date:
review_cycle:
next_review:
relationships:
  - type: appliesTo
    target:
  - type: ownedBy
    target:
---

## Policies

Location: data/management-system/policies/

## Instructions

Location: data/management-system/instructions/

## Manuals

Location: data/management-system/manuals/

## Glossary

- Governance system: The set of governed documents and practices that define how work is controlled, performed, and improved.
- Document owner: The person accountable for keeping a document current and approved.
- Effective date: The date a document becomes active and should be followed.
- Review cycle: The cadence for reviewing and updating a document.
