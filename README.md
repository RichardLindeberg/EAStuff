# EA Tool - ArchiMate 3.2 Markdown Edition

A simplified Enterprise Architecture tool using markdown files with YAML frontmatter, compliant with ArchiMate 3.2 specification.

## Overview

This tool allows you to define enterprise architecture elements using simple markdown files with YAML metadata. Each element is stored in a separate `.md` file with structured metadata and a markdown description.

## File Format

Each EA element is stored as a markdown file with YAML frontmatter:

```markdown
---
id: app-comp-001-customer-portal
name: Customer Portal
type: application-component
layer: application
relationships:
  - type: serving
    target: bus-srvc-001-customer-service
  - type: composition
    target: app-intf-001-web-ui
properties:
  owner: IT Department
  criticality: high
---

# Customer Portal

Customer-facing web application that provides access to services.

## Description

The Customer Portal is the primary interface for customers to interact with our services...
```

## ArchiMate 3.2 Layers

- **Strategy Layer**: Resource, Capability, Course of Action
- **Business Layer**: Business Actor, Business Role, Business Process, etc.
- **Application Layer**: Application Component, Application Service, Data Object
- **Technology Layer**: Node, Device, System Software, Technology Service
- **Physical Layer**: Equipment, Facility, Distribution Network
- **Implementation & Migration**: Work Package, Deliverable, Implementation Event

## Directory Structure

```
/elements          # All EA elements
  /strategy        # Strategy layer elements
  /business        # Business layer elements
  /application     # Application layer elements
  /technology      # Technology layer elements
  /physical        # Physical layer elements (optional; create when needed)
  /motivation      # Stakeholder, Driver, Goal, Requirement
  /implementation  # Implementation & Migration elements (optional; create when needed)
/schemas           # ArchiMate schemas and validation rules
/docs              # Documentation and guides
/fsharp-server     # F# web server (current source of truth)
```

## Usage

### Prerequisites

- .NET 8.0 SDK or later

### Creating an Element

1. Create a new markdown file in the appropriate layer directory
2. Add YAML frontmatter with required fields (id, name, type, layer)
3. Add markdown description

### Running the F# Server

From the `fsharp-server` directory:

```bash
dotnet build
dotnet run
```

Then open `http://localhost:5000` to browse the architecture.

### Relationship Validation (F# Server)

The server validates relationships using ArchiMate rules from [schemas/relations.xml](schemas/relations.xml). Any issues are reported as warnings when browsing elements.

## Documentation

- [docs/quick-start.md](docs/quick-start.md)
- [docs/best-practices.md](docs/best-practices.md)
- [docs/element-types-reference.md](docs/element-types-reference.md)
- [docs/id-naming-standard.md](docs/id-naming-standard.md)
- [docs/mermaid-guide.md](docs/mermaid-guide.md)
- [docs/deployment-guide.md](docs/deployment-guide.md)
- [docs/repository-structure.md](docs/repository-structure.md)

## Getting Started

See example elements in the `/elements` directory to understand the format. For server usage, see [fsharp-server/QUICK-START.md](fsharp-server/QUICK-START.md).
