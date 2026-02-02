# PlantUML Generator Guide

Generate visual ArchiMate diagrams from your EA markdown files using PlantUML.

## Installation

### Python Dependencies
Already included in requirements.txt:
```bash
pip install pyyaml
```

### PlantUML Rendering

**Option 1: VS Code Extension (Recommended)**
1. Install "PlantUML" extension by jebbs
2. Open any `.puml` file
3. Press `Alt+D` to preview

**Option 2: Online Renderer**
- Visit http://www.plantuml.com/plantuml/
- Copy/paste `.puml` file content

**Option 3: Local PlantUML**
```bash
# Requires Java
java -jar plantuml.jar diagram.puml
```

## Usage

### Generate Full Architecture Diagram

```bash
python scripts/generators/generate_puml.py
```

Creates: `diagrams/full-architecture.puml` with all elements and relationships.

### Generate Layer-Specific Diagram

```bash
python scripts/generators/generate_puml.py --layer application
python scripts/generators/generate_puml.py --layer business
python scripts/generators/generate_puml.py --layer technology
```

Creates: `diagrams/<layer>-layer.puml` with elements from that layer only.

### Generate Element Context Diagram

Show an element and its immediate connections:

```bash
python scripts/generators/generate_puml.py --element app-comp-customer-portal-001
```

Show connections up to 2 levels deep:

```bash
python scripts/generators/generate_puml.py --element app-comp-customer-portal-001 2
```

Creates: `diagrams/<element-id>-context.puml`

### List All Elements

```bash
python scripts/generators/generate_puml.py --list
```

Shows all available elements organized by layer.

## PlantUML Syntax

The generator creates PlantUML files using ArchiMate notation:

```plantuml
@startuml
!include <archimate/Archimate>

title My Architecture

' Elements
Archimate_ApplicationComponent(portal, "Customer Portal")
Archimate_BusinessProcess(process, "Customer Service")
Archimate_Node(server, "Web Server")

' Relationships
Rel_Serving(portal, process, "Supports")
Rel_Realization(server, portal, "Hosts")

@enduml
```

## Customizing Diagrams

### Manual Editing

After generation, you can edit `.puml` files to:
- Add colors: `Archimate_ApplicationComponent(id, "Name" #lightblue)`
- Group elements: Use `package` or `rectangle` blocks
- Add notes: `note right of element : Note text`
- Change layout: Add `left to right direction`

### Example Customization

```plantuml
@startuml
!include <archimate/Archimate>

left to right direction

package "Frontend" {
  Archimate_ApplicationComponent(portal, "Customer Portal" #lightgreen)
}

package "Backend" {
  Archimate_ApplicationComponent(api, "API Gateway" #lightblue)
}

Rel_Serving(portal, api, "Calls")

note right of portal
  Main customer-facing
  application
end note

@enduml
```

## Tips

### For Large Diagrams
- Generate layer-specific diagrams instead of full architecture
- Use element context diagrams to focus on specific areas
- Edit generated files to remove less important elements

### Layout Optimization
Add these directives to generated files:
```plantuml
left to right direction
skinparam ranksep 20
skinparam nodesep 10
```

### Styling
```plantuml
skinparam backgroundColor white
skinparam shadowing false
skinparam ArrowColor #666666
```

## Workflow Example

1. **Create/Update EA Elements**
   ```bash
   # Edit markdown files in elements/
   ```

2. **Validate**
   ```bash
   python scripts/validator/validate.py
   ```

3. **Generate Diagrams**
   ```bash
   python scripts/generators/generate_puml.py
   python scripts/generators/generate_puml.py --layer application
   ```

4. **Preview**
   - Open `.puml` file in VS Code
   - Press `Alt+D` to see diagram

5. **Export**
   - Right-click diagram → Export
   - Choose PNG, SVG, or PDF format

## Troubleshooting

**"Element not found" error:**
- Run `python scripts/generators/generate_puml.py --list` to see available elements
- Check element ID spelling

**Diagram too cluttered:**
- Generate layer-specific or context diagrams instead
- Manually edit to remove elements
- Split into multiple focused diagrams

**Relationships not showing:**
- Verify both source and target elements exist
- Check relationship `target` IDs in markdown files
- Validate files first: `python scripts/validator/validate.py`

**PlantUML rendering issues:**
- Ensure you have the ArchiMate library included
- Check PlantUML version (need recent version for ArchiMate)
- Try online renderer if local rendering fails

## Advanced Usage

### Batch Generation

Create all diagrams at once:

```bash
# Windows PowerShell
python scripts/generators/generate_puml.py
python scripts/generators/generate_puml.py --layer strategy
python scripts/generators/generate_puml.py --layer business
python scripts/generators/generate_puml.py --layer application
python scripts/generators/generate_puml.py --layer technology
python scripts/generators/generate_puml.py --layer motivation
```

### Integration with Documentation

Reference diagrams in markdown documentation:

```markdown
## Architecture Overview

![Full Architecture](diagrams/full-architecture.png)

## Application Layer

![Application Layer](diagrams/application-layer.png)
```

### Version Control

Commit both `.puml` and exported image files:
- `.puml` files for editing and regeneration
- `.png` or `.svg` files for quick viewing

## Next Steps

- Explore PlantUML documentation: https://plantuml.com/archimate-diagram
- Customize generated diagrams
- Create custom diagram templates
- Automate diagram generation in CI/CD pipeline
