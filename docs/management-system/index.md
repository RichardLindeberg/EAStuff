# Management System Documentation

This section explains how management system documents are organized and how to author them. These documents are separate from ArchiMate element definitions in data/archimate and do not use YAML frontmatter.

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

Add a small metadata block near the top of each document to capture ownership and who the document is for. Use a relations-style list similar to ArchiMate, and use ArchiMate IDs for roles/actors (for example, business-role or business-actor IDs).

Format:

- Document ID:
- Owner:
- Effective date:
- Review cycle:
- Relations:
	- type: appliesTo
		target:
	- type: ownedBy
		target:

## Policies

Location: data/management-system/policies/

- Template: [data/management-system/policies/ms-policy-001-template.md](../../data/management-system/policies/ms-policy-001-template.md)

## Instructions

Location: data/management-system/instructions/

- Template: [data/management-system/instructions/ms-instruction-001-template.md](../../data/management-system/instructions/ms-instruction-001-template.md)

## Manuals

Location: data/management-system/manuals/

- Template: [data/management-system/manuals/ms-manual-001-template.md](../../data/management-system/manuals/ms-manual-001-template.md)

## Glossary

- Management system: The set of governed documents and practices that define how work is controlled, performed, and improved.
- Document owner: The person accountable for keeping a document current and approved.
- Effective date: The date a document becomes active and should be followed.
- Review cycle: The cadence for reviewing and updating a document.
