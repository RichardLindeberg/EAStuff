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
/data
  /archimate       # All EA elements
    /strategy      # Strategy layer elements
    /business      # Business layer elements
    /application   # Application layer elements
    /technology    # Technology layer elements
    /physical      # Physical layer elements (optional; create when needed)
    /motivation    # Stakeholder, Driver, Goal, Requirement
    /implementation # Implementation & Migration elements (optional; create when needed)
  /management-system # Policies, instructions, manuals
/schemas            # ArchiMate schemas and validation rules
/docs               # Documentation and guides
/src/fsharp-server  # F# web server (current source of truth)
```

## Usage

### Prerequisites

- .NET 8.0 SDK or later

### Creating an Element

1. Create a new markdown file in the appropriate layer directory
2. Add YAML frontmatter with required fields (id, name, type, layer)
3. Add markdown description

### Running the F# Server

From the `src/fsharp-server` directory:

```bash
dotnet build
dotnet run
```

Then open `http://localhost:5000` to browse the architecture.

### Data Location Policy

The server does not bundle `data/archimate` in publish output. Production deployments must provide the data folder externally and set `EAArchive:ElementsPath` to its absolute path (see `src/fsharp-server/appsettings.Production.json`).

### Configuration

Required settings are defined in appsettings files under `src/fsharp-server`.

```json
{
  "EAArchive": {
    "ElementsPath": "..\\..\\data\\archimate",
    "RelationsPath": "wwwroot\\schemas\\relations.xml",
    "Assets": {
      "SymbolsPath": "wwwroot\\assets\\archimate-symbols",
      "IconsPath": "wwwroot\\assets\\archimate-icons",
      "SymbolsBaseUrl": "/assets/archimate-symbols/",
      "IconsBaseUrl": "/assets/archimate-icons/"
    },
    "Web": {
      "BaseUrl": "/",
      "SiteCssUrl": "/css/site.css",
      "DiagramCssUrl": "/css/cytoscape-diagram.css",
      "ValidationScriptUrl": "/js/validation.js",
      "DiagramScriptUrl": "/js/cytoscape-diagram.js",
      "HtmxScriptUrl": "https://unpkg.com/htmx.org@1.9.10",
      "HtmxDebugScriptUrl": "https://unpkg.com/htmx.org@1.9.12/dist/ext/debug.js",
      "CytoscapeScriptUrl": "https://cdn.jsdelivr.net/npm/cytoscape@3.26.0/dist/cytoscape.min.js",
      "DagreScriptUrl": "https://cdn.jsdelivr.net/npm/dagre@0.8.5/dist/dagre.min.js",
      "CytoscapeDagreScriptUrl": "https://cdn.jsdelivr.net/npm/cytoscape-dagre@2.5.0/cytoscape-dagre.js",
      "LodashScriptUrl": "https://cdn.jsdelivr.net/npm/lodash@4.17.21/lodash.min.js"
    }
  }
}
```

Use environment-specific overrides:

- Development: `src/fsharp-server/appsettings.Development.json`
- Production: `src/fsharp-server/appsettings.Production.json`

### Relationship Validation (F# Server)

The server validates relationships using ArchiMate rules from [schemas/relations.xml](schemas/relations.xml). Any issues are reported as warnings when browsing elements.

## Documentation

- [docs/index.md](docs/index.md)
- [docs/management-system/index.md](docs/management-system/index.md)

## Getting Started

See example elements in the `/data/archimate` directory to understand the format. For server usage, see [src/fsharp-server/QUICK-START.md](src/fsharp-server/QUICK-START.md).
