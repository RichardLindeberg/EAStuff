# Quick Start Guide

Get started with the EA Tool in 5 minutes.

## Prerequisites

- Python 3.8 or higher
- PyYAML library (`pip install pyyaml`)
- A text editor (VS Code recommended)

## Step 1: Install Dependencies

```bash
pip install pyyaml
```

## Step 2: Create Your First Element

Create a new file in the `elements` directory. Choose the appropriate subdirectory based on the ArchiMate layer:

```
elements/
  ├── strategy/          # Capabilities, resources
  ├── business/          # Processes, actors, services
  ├── application/       # Applications, data
  ├── technology/        # Infrastructure, servers
  ├── physical/          # Equipment, facilities
  ├── motivation/        # Goals, requirements
  └── implementation/    # Work packages, deliverables
```

## Step 3: Write Element Markdown

Create `elements/application/my-app.md`:

```markdown
---
id: my-app-001
name: My Application
type: application-component
layer: application
relationships:
  - type: serving
    target: some-business-process
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

## Step 4: Validate Your Element

Run the validator:

```bash
python scripts/validator/validate.py elements/application/my-app.md
```

Or validate all elements:

```bash
python scripts/validator/validate.py
```

## Step 5: Review Results

The validator will check:
- ✅ Required fields (id, name, type, layer)
- ✅ Valid element type for the specified layer
- ✅ Valid relationship types
- ✅ Proper YAML structure
- ⚠️ Optional but recommended fields

## Common Element Templates

### Business Process

```yaml
---
id: process-001
name: My Business Process
type: business-process
layer: business
relationships:
  - type: realization
    target: service-001
properties:
  owner: Business Unit
  status: active
---
```

### Application Service

```yaml
---
id: svc-001
name: Authentication Service
type: application-service
layer: application
relationships:
  - type: serving
    target: app-component-001
properties:
  owner: Platform Team
  status: production
  criticality: high
---
```

### Technology Node

```yaml
---
id: server-001
name: Application Server
type: node
layer: technology
relationships:
  - type: realization
    target: app-component-001
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

- Use meaningful IDs following the standard format (e.g., `app-comp-customer-portal-001`, `app-srvc-authentication-001`)
- Keep element names concise but descriptive
- Add detailed descriptions in the markdown section
- Use relationships to show connections between elements
- Tag elements for easier searching and filtering
- Keep properties up to date (status, owner, etc.)

## Getting Help

- See `docs/element-types-reference.md` for all ArchiMate 3.2 element types
- Check `schemas/archimate-3.2-schema.yaml` for validation rules
- Review examples in `elements/` directories
