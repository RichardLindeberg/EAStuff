# Quick Start Guide

Get started with the EA Tool in 5 minutes.

## Prerequisites

- .NET 8.0 SDK or later
- A text editor (VS Code recommended)

## Step 2: Create Your First Element

Create a new file in the `elements` directory. Choose the appropriate subdirectory based on the ArchiMate layer:

```
elements/
  ├── strategy/          # Capabilities, resources
  ├── business/          # Processes, actors, services
  ├── application/       # Applications, data
  ├── technology/        # Infrastructure, servers
  ├── physical/          # Equipment, facilities (optional; create when needed)
  ├── motivation/        # Goals, requirements
  └── implementation/    # Work packages, deliverables (optional; create when needed)
```

## Step 3: Write Element Markdown

Create `elements/application/my-app.md`:

```markdown
---
id: app-comp-001-my-app
name: My Application
type: application-component
layer: application
relationships:
  - type: serving
    target: bus-proc-001-customer-onboarding
    description: Supports business process
properties:
  owner: IT Team
  status: development
  criticality: medium
---

# My Application

Brief description of what this application does.

## Detailed Description

Add more details here about the application's purpose,
features, and how it fits into the overall architecture.
```

## Step 4: Run the F# Server

From the `fsharp-server` directory:

```bash
dotnet build
dotnet run
```

Open `http://localhost:5000` to browse your elements.

## Step 5: Review Validation Warnings

The server validates elements and relationships at startup and while rendering pages. Watch the console output and element pages for warnings about missing targets, invalid relationships, or schema issues.

## Common Element Templates

### Business Process

```yaml
---
id: bus-proc-001-customer-onboarding
name: My Business Process
type: business-process
layer: business
relationships:
  - type: realization
    target: bus-srvc-001-customer-onboarding
properties:
  owner: Business Unit
  status: active
---
```

### Application Service

```yaml
---
id: app-srvc-001-authentication
name: Authentication Service
type: application-service
layer: application
relationships:
  - type: serving
    target: bus-proc-001-customer-onboarding
properties:
  owner: Platform Team
  status: production
  criticality: high
---
```

### Technology Node

```yaml
---
id: tec-node-001-application-server
name: Application Server
type: node
layer: technology
relationships:
  - type: realization
    target: app-comp-001-customer-portal
properties:
  owner: Infrastructure Team
  status: production
---
```

## Next Steps

1. Review example elements in the `elements/` directory
2. Check the [Element Types Reference](element-types-reference.md) for all available types
3. Explore relationship types in the schema file
4. Start documenting your architecture!

## Tips

- Use meaningful IDs following the standard format (e.g., `app-comp-001-customer-portal`, `app-srvc-001-authentication`)
- Keep element names concise but descriptive
- Add detailed descriptions in the markdown section
- Use relationships to show connections between elements
- Tag elements for easier searching and filtering
- Keep properties up to date (status, owner, etc.)

## Getting Help

- See [element-types-reference.md](element-types-reference.md) for all ArchiMate 3.2 element types
- Check [../schemas/archimate-3.2-schema.yaml](../schemas/archimate-3.2-schema.yaml) for validation rules
- Review examples in the elements/ directories
- Browse the UI guide in [mermaid-guide.md](mermaid-guide.md)
- Review standards in [best-practices.md](best-practices.md)
