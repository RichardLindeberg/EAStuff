# How-to: Create Your First ArchiMate Element

Use this guide to add a valid ArchiMate element that passes current validation.

## Prerequisites

- .NET 8.0 SDK or later
- A text editor (VS Code recommended)

## 1) Choose the correct folder

Create a new file under `data/archimate/<layer>/`.

```
data/archimate/
  ├── strategy/
  ├── business/
  ├── application/
  ├── technology/
  ├── physical/
  ├── motivation/
  └── implementation/
```

## 2) Add required frontmatter

The repository validates a shared set of fields and an `archimate` section.

```yaml
---
id: app-comp-001-my-app
name: My Application
owner: bus-role-001-application-owner
status: draft
version: "0.1"
last_updated: "2026-02-10"
review_cycle: annual
next_review: "2027-02-10"
archimate:
  type: application-component
  layer: application
  criticality: medium
relationships:
  - type: serving
    target: bus-proc-001-customer-onboarding
    description: Supports customer onboarding
tags:
  - customer-facing
  - web
extensions:
  properties:
    lifecycle-phase: build
---
```

## 3) Write the markdown body

Add a short summary and any details you want to capture.

```markdown
# My Application

Short description of what this application does.

## Details

Add scope, dependencies, and constraints here.
```

## 4) Run the server and check validation

From the `src/fsharp-server` directory:

```bash
dotnet build
dotnet run
```

Open `http://localhost:5000` and check the validation warnings for your new element.

## Notes

- `id`, `name`, `owner`, `status`, `version`, `last_updated`, `review_cycle`, and `next_review` are required.
- `archimate.type` and `archimate.layer` are required for architecture elements.
- `relationships` and `tags` are optional but recommended.
- `extensions` is allowed for custom fields that are not part of the core schema.

## See also

- [Element Types Reference](../reference/element-types-reference.md)
- [Element ID Naming Standard](../reference/id-naming-standard.md)
- [Web UI Guide](../operations/web-ui-guide.md)
