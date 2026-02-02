# Using Your ArchiMate Exchange File

Your enterprise architecture model has been successfully converted to a proper ArchiMate 3.1 Model Exchange File. Here's what you can do with it.

## Generated Files

- **`output/model-exchange.archimate`** - The main exchange file (57 KB)
  - 47 architecture elements across 5 layers
  - 91 relationships between elements
  - All properties and metadata preserved
  - Valid ArchiMate 3.1 XML format

## Importing into ArchiMate Tools

### Archi (Open Source - Recommended)

1. Download [Archi](https://www.archimatetool.com/) (free, open-source)
2. Open Archi
3. File → Import → Import model from file
4. Select `output/model-exchange.archimate`
5. Your model will be imported with all elements and relationships

### Enterprise Architect (Sparx)

1. Open Enterprise Architect
2. File → Import → Import ArchiMate Model
3. Select `output/model-exchange.archimate`
4. Configure mapping options if prompted
5. The model will be imported into your project

### Ardoq

1. Go to [Ardoq Platform](https://www.ardoq.com/)
2. Create new workspace
3. Import → ArchiMate Model
4. Upload `output/model-exchange.archimate`
5. Configure component mappings

### LeanIX

1. Open LeanIX workspace
2. Import → Model Import
3. Select ArchiMate format
4. Upload `output/model-exchange.archimate`
5. Map fields if needed

## What Gets Imported

### Elements (47 total)

**Motivation Layer (28 elements):**
- 6 Goals (digital transformation, regulatory compliance, customer trust, etc.)
- 5 Drivers (regulatory, market, cost, sustainability)
- 5 Principles (customer-centricity, regulatory excellence, etc.)
- 12 Requirements (GDPR, MiFID2, DORA, APIs, cloud, automation, etc.)

**Strategy Layer (15 elements):**
- 9 Capabilities (digital banking, cloud, open banking, AI, automation, resilience, etc.)
- 3 Value Streams (digital customer journey, operational excellence, secure operations)
- 2 Resources (digital talent, technology investment)
- 1 Course of Action (digital transformation roadmap)

**Business Layer (2 elements):**
- Business Process (customer service)
- Business Object (customer data)

**Application Layer (1 element):**
- Application Component (customer portal)

**Technology Layer (1 element):**
- Technology Node (web application server)

### Relationships (91 total)

**By Type:**
- 38 Realization relationships (implements/fulfills)
- 27 Influence relationships (affects/modifies)
- 13 Serving relationships (provides functionality)
- 12 Association relationships (generic connections)
- 1 Assignment relationship (allocates responsibility)

**Key Relationships:**
- Goals ↔ Drivers (what drives what)
- Goals → Requirements (how goals are realized)
- Goals → Capabilities (what capabilities support goals)
- Requirements → Goals (requirements serve goals)
- Elements → Objectives (everything ties to strategic objectives)

## After Importing

### 1. Visualize Your Model

Once imported, you can create views showing:
- **Motivation view**: Goals, drivers, principles, requirements hierarchy
- **Strategy view**: Capabilities and value streams
- **Business view**: Business processes and objects
- **Technology view**: Infrastructure and systems
- **Implementation view**: Roadmaps and initiatives

### 2. Validate and Refine

- Check for missing relationships
- Add visual diagrams and layouts
- Assign colors and shapes per layer
- Document additional details
- Add stakeholder information

### 3. Generate Reports

Most tools support exporting:
- Architecture documentation
- Traceability matrices
- Gap analyses
- Compliance mappings
- Roadmaps and timelines

### 4. Maintain Consistency

- Keep markdown source files updated
- Regenerate .archimate file periodically
- Version control both sources
- Sync with tools as needed

## Export Back to Markdown

If you make changes in the ArchiMate tool, you can export back:

1. Export from tool to ArchiMate format
2. Parse the XML to update markdown files
3. Update version tags and relationships

A future script can automate this: `parse_archimate_to_markdown.py`

## Integration with Your Workflow

### Current Workflow

```
Markdown Elements (.md)
        ↓
generate_archimate_exchange.py
        ↓
ArchiMate Exchange File (.archimate)
        ↓
Import into ArchiMate Tool
        ↓
Visualize & Collaborate
```

### Recommended Updates

```
Markdown Elements (.md)
        ↓
generate_archimate_exchange.py
        ↓
ArchiMate Exchange File (.archimate)
        ↓
[Version Control in Git]
        ↓
Import into ArchiMate Tool
        ↓
        │
        ├─→ Generate Reports
        ├─→ Create Diagrams
        └─→ Validate Model
```

## Technical Details

### File Format

- **Format**: XML (text-based)
- **Schema**: ArchiMate 3.1 (Open Group)
- **Namespace**: `http://www.opengroup.org/xsd/archimate/3.1`
- **Size**: ~57 KB (47 elements + 91 relationships)

### Valid Elements

All element types in your model are mapped to official ArchiMate types:

```
Strategy Layer:
  resource → Resource
  capability → Capability
  value-stream → ValueStream
  course-of-action → CourseOfAction

Business Layer:
  business-process → BusinessProcess
  business-object → BusinessObject

Application Layer:
  application-component → ApplicationComponent

Technology Layer:
  node → Node

Motivation Layer:
  goal → Goal
  driver → Driver
  principle → Principle
  requirement → Requirement
```

### Valid Relationships

All relationship types conform to ArchiMate 3.1:

```
Structural:
  composition, aggregation, assignment, realization

Dependency:
  serving, access, influence, association

Dynamic:
  triggering, flow

Other:
  specialization
```

## Troubleshooting

### "Invalid XML" or "Parse Error"

- Verify file integrity: `python scripts/validate_archimate_exchange.py`
- Check that all element IDs are unique
- Ensure all relationships reference valid elements

### "Unsupported Element Type"

- Check that element types match official ArchiMate types
- Review `TYPE_MAPPING` in generation script
- Some tools may not support all ArchiMate 3.1 elements

### "Missing Relationships"

- Relationships are only included if both source and target exist
- Check relationship types in markdown YAML frontmatter
- Verify target IDs match element IDs exactly

### "File Won't Import"

- Try validating first: `python scripts/validate_archimate_exchange.py`
- Check tool documentation for supported ArchiMate versions
- Try importing as "generic XML" if tool supports it
- Contact tool vendor support

## Next Steps

1. ✓ **Generated exchange file** - Done!
2. → **Choose ArchiMate tool** - Archi, Enterprise Architect, Ardoq, LeanIX, etc.
3. → **Import the file** - Follow tool-specific import process
4. → **Review and validate** - Check all elements imported correctly
5. → **Create visualizations** - Build diagrams and views
6. → **Share with stakeholders** - Collaborate and get feedback
7. → **Document decisions** - Update markdown as things change
8. → **Regenerate as needed** - Periodically sync markdown ↔ ArchiMate

## Resources

- [ArchiMate Specification](https://www.opengroup.org/archimate/)
- [ArchiMate Model Exchange Format](https://www.opengroup.org/xsd/archimate/)
- [Archi (Free Tool)](https://www.archimatetool.com/)
- [ArchiMate Modeling Guide](https://pubs.opengroup.org/architecture/archimate3-doc/)

## Support

For issues with:
- **Generation**: Review `docs/archimate-exchange-generation.md`
- **Markdown format**: Review `docs/quick-start.md` and example files
- **Tool-specific help**: Contact your ArchiMate tool vendor
- **ArchiMate standard**: Consult Open Group documentation
