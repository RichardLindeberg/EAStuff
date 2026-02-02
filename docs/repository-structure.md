# Repository Structure

Updated: February 2, 2026

## Directory Layout

```
EA Stuff/
├── scripts/                        # All Python scripts (consolidated)
│   ├── __init__.py                 # Package initialization
│   ├── generators/                 # Diagram generation tools
│   │   ├── __init__.py
│   │   ├── generate_puml.py        # PlantUML diagram generator
│   │   └── generate_mermaid.py     # Mermaid diagram generator (interactive)
│   └── validator/                  # Validation tools
│       ├── __init__.py
│       └── validate.py             # ArchiMate 3.2 compliance validator
│
├── elements/                       # EA element definitions
│   ├── strategy/                   # Strategy layer elements
│   ├── business/                   # Business layer elements
│   ├── application/                # Application layer elements
│   ├── technology/                 # Technology layer elements
│   ├── physical/                   # Physical layer elements
│   └── motivation/                 # Motivation layer elements
│
├── diagrams/                       # Generated diagrams (output)
│   ├── *.puml                      # PlantUML diagrams
│   ├── *-mermaid.html              # Interactive Mermaid diagrams (HTML)
│   └── *-mermaid.md                # Mermaid diagrams (Markdown)
│
├── schemas/                        # Validation schemas
│   └── archimate-3.2-schema.yaml   # ArchiMate 3.2 specification
│
├── docs/                           # Documentation
│   ├── quick-start.md
│   ├── plantuml-guide.md
│   ├── mermaid-guide.md
│   ├── plantuml-setup.md
│   ├── element-types-reference.md
│   └── best-practices.md
│
├── examples/                       # Example files
│   ├── invalid-missing-fields.md
│   └── invalid-wrong-layer.md
│
├── README.md                       # Main documentation
└── requirements.txt                # Python dependencies

```

## Why This Structure?

### Before
```
generator/
  generate_puml.py
  generate_mermaid.py
validator/
  validate.py
```

### After
```
scripts/
  generators/
    generate_puml.py
    generate_mermaid.py
  validator/
    validate.py
```

### Benefits

1. **Consolidation**: All Python scripts in one top-level folder
2. **Clear Organization**: Subfolders group related functionality
3. **Scalability**: Easy to add new script categories
4. **Package Structure**: Can be imported as Python packages
5. **Consistency**: Follows Python project conventions

## Usage

All commands now use the `scripts/` prefix:

### Validation
```bash
python scripts/validator/validate.py
python scripts/validator/validate.py elements/application/customer-portal.md
```

### PlantUML Generation
```bash
python scripts/generators/generate_puml.py
python scripts/generators/generate_puml.py --layer application
python scripts/generators/generate_puml.py --element app-comp-customer-portal-001
```

### Mermaid Generation
```bash
python scripts/generators/generate_mermaid.py
python scripts/generators/generate_mermaid.py --layer application
python scripts/generators/generate_mermaid.py --element app-comp-customer-portal-001 2
python scripts/generators/generate_mermaid.py --md
```

## Migration Notes

### What Changed
- ✅ Moved `generator/` → `scripts/generators/`
- ✅ Moved `validator/` → `scripts/validator/`
- ✅ Updated all documentation with new paths
- ✅ Fixed path calculations in all scripts
- ✅ Added `__init__.py` files for package structure
- ✅ Verified all scripts work correctly

### What Stayed the Same
- Element files location (`elements/`)
- Output location (`diagrams/`)
- Schema location (`schemas/`)
- Documentation location (`docs/`)
- All script functionality and command-line arguments

### Breaking Changes
None! All scripts work exactly the same, just with updated paths.

## Future Additions

The new structure makes it easy to add:

```
scripts/
  generators/
    generate_puml.py
    generate_mermaid.py
    generate_archimate.py      # New: ArchiMate XML export
    generate_json.py           # New: JSON API
  validator/
    validate.py
    validate_strict.py         # New: Strict mode validator
  importers/                   # New: Import from other tools
    import_sparx.py
    import_archi.py
  exporters/                   # New: Export to other formats
    export_csv.py
    export_excel.py
```

## Developer Notes

### Path Calculation
All scripts use:
```python
script_dir = Path(__file__).parent
workspace_dir = script_dir.parent.parent  # generators -> scripts -> workspace
```

### Package Imports
Scripts can now be imported as modules:
```python
from scripts.generators.generate_mermaid import MermaidGenerator
from scripts.validator.validate import ArchiMateValidator
```

### Testing
Run all scripts from workspace root:
```bash
cd "EA Stuff"
python scripts/generators/generate_mermaid.py --list
python scripts/generators/generate_puml.py --list
python scripts/validator/validate.py
```
