# ArchiMate Exchange File Generator

This script generates a proper **ArchiMate 3.1 Model Exchange File** from your markdown-based architecture elements.

## What is an ArchiMate Exchange File?

An ArchiMate exchange file is an XML-based standard format defined by The Open Group that allows you to:
- **Export** your architecture model from one tool
- **Import** it into another tool (ArchiMate modeling tools, enterprise architecture platforms, etc.)
- **Share** your model with stakeholders and other teams
- **Maintain version control** of your architecture in a standard format

Reference: [Open Group ArchiMate Exchange Format](https://www.opengroup.org/xsd/archimate/)

## Usage

### Generate the exchange file

```bash
# Generate with default output location (output/model-exchange.archimate)
python scripts/generate_archimate_exchange.py

# Generate with custom output location
python scripts/generate_archimate_exchange.py /path/to/output.archimate
```

### Output

The script generates:
- **model-exchange.archimate**: The XML file in ArchiMate 3.1 format
- **Console summary**: Statistics about elements, layers, and relationships

## What Gets Generated

### Elements
All markdown files in `elements/` directory are converted to ArchiMate elements:

- **Layers**: Strategy, Business, Application, Technology, Motivation, Implementation
- **Types**: Automatically mapped from your markdown type to official ArchiMate types
- **Properties**: All metadata properties are preserved
- **Documentation**: Extracted from your markdown content

### Relationships
All relationships defined in element metadata are converted to:

- **Structural**: Composition, Aggregation, Assignment, Realization
- **Dependency**: Serving, Access, Influence, Association
- **Dynamic**: Triggering, Flow
- **Other**: Specialization

### Current Model Statistics

```
Elements by Layer:
  motivation: 28 elements (goals, drivers, principles, requirements)
  strategy: 15 elements (capabilities, resources, value streams)
  business: 2 elements (processes, objects)
  application: 1 element (components)
  technology: 1 element (infrastructure)

Total: 47 elements
Total relationships: 91
```

## Type Mappings

### Strategy Layer
- `resource` → Resource
- `capability` → Capability
- `value-stream` → ValueStream
- `course-of-action` → CourseOfAction

### Business Layer
- `business-actor` → BusinessActor
- `business-role` → BusinessRole
- `business-process` → BusinessProcess
- `business-service` → BusinessService
- `business-object` → BusinessObject
- (and 8 more specific types)

### Application Layer
- `application-component` → ApplicationComponent
- `application-interface` → ApplicationInterface
- `application-process` → ApplicationProcess
- `application-service` → ApplicationService
- `data-object` → DataObject
- (and 4 more specific types)

### Technology Layer
- `node` → Node
- `device` → Device
- `system-software` → SystemSoftware
- `artifact` → Artifact
- (and 9 more specific types)

### Motivation Layer
- `driver` → Driver
- `goal` → Goal
- `requirement` → Requirement
- `principle` → Principle
- `constraint` → Constraint
- (and 5 more specific types)

## File Format

The generated file is valid XML conforming to the ArchiMate 3.1 XSD schema:

```xml
<?xml version="1.0"?>
<model xmlns="http://www.opengroup.org/xsd/archimate/3.1"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       identifier="model_001" version="1.0">
  <name>Enterprise Architecture Model</name>
  <metadata>
    <schemaInfo>
      <schema>http://www.opengroup.org/xsd/archimate/3.1</schema>
      <schemaversion>3.1</schemaversion>
    </schemaInfo>
  </metadata>
  <elements>
    <!-- All elements -->
  </elements>
  <relationships>
    <!-- All relationships -->
  </relationships>
</model>
```

## Compatibility

The generated `.archimate` files can be imported into:
- **ArchiMate modeling tools** (Archi, Enterprise Architect, etc.)
- **Enterprise architecture platforms** (Ardoq, LeanIX, etc.)
- **Any tool supporting ArchiMate 3.1 exchange format**

## Next Steps

1. **Review** the generated file: `output/model-exchange.archimate`
2. **Import** into your preferred ArchiMate tool
3. **Visualize** and collaborate with stakeholders
4. **Export** updates back from the tool if needed

## Extending

To customize the generation:

- Modify `TYPE_MAPPING` dictionary to adjust element type conversions
- Modify `RELATIONSHIP_TYPE_MAPPING` for relationship type conversions
- Add additional properties or metadata extraction in `_add_element_to_xml()`
- Implement `generate_archimate_view()` to create view definitions

## Requirements

- Python 3.7+
- `python-frontmatter` package

Install requirements:
```bash
pip install python-frontmatter
```

## References

- [ArchiMate Specification](https://www.opengroup.org/archimate/)
- [ArchiMate Exchange Format XSD](https://www.opengroup.org/xsd/archimate/3.1/archimate3_Model.xsd)
- [ArchiMate Modeling Guide](https://pubs.opengroup.org/architecture/archimate3-doc/)
