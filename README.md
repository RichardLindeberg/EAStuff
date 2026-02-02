# EA Tool - ArchiMate 3.2 Markdown Edition

A simplified Enterprise Architecture tool using markdown files with YAML frontmatter, compliant with ArchiMate 3.2 specification.

## Overview

This tool allows you to define enterprise architecture elements using simple markdown files with YAML metadata. Each element is stored in a separate `.md` file with structured metadata and a markdown description.

## File Format

Each EA element is stored as a markdown file with YAML frontmatter:

```markdown
---
id: element-001
name: Customer Portal
type: application-component
layer: application
relationships:
  - type: serving
    target: customer-service-001
  - type: composition
    target: web-ui-001
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
  /physical        # Physical layer elements
  /motivation      # Stakeholder, Driver, Goal, Requirement
/schemas           # ArchiMate schemas and validation rules
/scripts           # All Python scripts
  /generators      # Diagram generation tools
  /validator       # Validation and compliance checking
/diagrams          # Generated diagrams (PlantUML & Mermaid)
/docs              # Documentation and guides
```

## Usage

### Creating an Element

1. Create a new markdown file in the appropriate layer directory
2. Add YAML frontmatter with required fields (id, name, type, layer)
3. Add markdown description

### Validating Elements

Run the validator to check compliance:

```bash
python scripts/validator/validate.py
```

Or validate a specific file:

```bash
python scripts/validator/validate.py elements/application/customer-portal.md
```

### Generating PlantUML Diagrams

Create visual diagrams from your elements:

```bash
# Generate full architecture diagram
python scripts/generators/generate_puml.py

# Generate layer-specific diagram
python scripts/generators/generate_puml.py --layer application

# Generate element context diagram
python scripts/generators/generate_puml.py --element app-comp-customer-portal-001

# List all elements
python scripts/generators/generate_puml.py --list
```

### Generating Static Website

Create a browsable HTML website of your architecture elements organized by layer:

```bash
# Quick way: Generate website with bash script (auto-opens in browser)
./generate-website.sh

# Or run Python script directly
python3 scripts/generate_website.py

# Specify custom output directory
python3 scripts/generate_website.py /path/to/output/dir
```

**Features:**
- ✓ **Layer-based navigation** - Browse elements by ArchiMate layer
- ✓ **Incoming & Outgoing relations** - See all relationships for each element
- ✓ **Clickable links** - Navigate between related elements
- ✓ **Responsive design** - Works on desktop and mobile
- ✓ **Metadata display** - Shows all element properties and tags
- ✓ **Markdown rendering** - Element content rendered as HTML

**Output Structure:**
```
output/website/
  ├── index.html              # Home page with layer overview
  ├── strategy.html           # Strategy layer elements
  ├── motivation.html         # Motivation layer elements
  ├── business.html           # Business layer elements
  ├── application.html        # Application layer elements
  ├── technology.html         # Technology layer elements
  └── elements/               # Individual element pages
      ├── elem-id-001.html
      └── ...
```

See [Website Generator Documentation](docs/website-generator.md) for more details.

### Generating Interactive Mermaid Diagrams

Create **clickable** Mermaid diagrams with links to element markdown files:

```bash
# Generate full architecture diagram as HTML (clickable in browser)
python scripts/generators/generate_mermaid.py

# Generate complete static website (diagrams + element HTML pages)
python scripts/generators/generate_mermaid.py --html-elements

# Generate layer-specific diagram
python scripts/generators/generate_mermaid.py --layer application

# Generate element context diagram
python scripts/generators/generate_mermaid.py --element app-comp-customer-portal-001 2

# Output as markdown instead of HTML
python scripts/generators/generate_mermaid.py --md

# List all elements
python scripts/generators/generate_mermaid.py --list
```

**Features:**
- ✓ **Clickable elements** - Click any element to view its documentation
- ✓ **Color-coded layers** - Visual distinction between strategy, business, application, technology layers
- ✓ **HTML output** - Self-contained HTML files with Mermaid.js (no installation needed)
- ✓ **Markdown output** - Compatible with GitHub, VS Code, and other markdown viewers
- ✓ **Context diagrams** - Show element relationships at different depths
- ✓ **Static website** - Generate HTML pages for all elements with `--html-elements`
- ✓ **Deploy-ready** - Output folder ready to push to any web server

**Static Website Generation:**

Use the `--html-elements` flag to create a complete static website in the `output/` folder:

```bash
# Quick way: Generate everything at once
python scripts/generators/generate_website.py

# Manual way: Generate specific diagrams with element HTML pages
python scripts/generators/generate_mermaid.py --html-elements
python scripts/generators/generate_mermaid.py --layer application --html-elements
python scripts/generators/generate_mermaid.py --layer business --html-elements
```

**Output Structure:**
```
output/
  ├── index.html                    # Landing page
  ├── diagrams/                     # Interactive Mermaid diagrams
  │   ├── full-architecture-mermaid.html
  │   ├── application-layer-mermaid.html
  │   └── ...
  └── elements/                     # Element documentation
      ├── application/*.html
      ├── business/*.html
      └── ...
```

**Deployment:**
The `output/` folder is completely self-contained and ready to deploy:
- Upload to any web server
- Deploy to GitHub Pages, Netlify, Vercel, etc.
- Share as a zip file
- No build process or server-side code required

**To view:**
- Open `output/index.html` in any web browser
- Click on diagram elements to view detailed documentation
- Navigate between related elements using links
- No web server required - works directly from filesystem

**Viewing diagrams:**
1. Install "PlantUML" extension in VS Code (by jebbs)
2. Open any `.puml` file in the `diagrams/` folder
3. Press `Alt+D` to preview

See [PlantUML Setup Guide](docs/plantuml-setup.md) for detailed instructions and troubleshooting.

## Getting Started

See example elements in the `/elements` directory to understand the format.
