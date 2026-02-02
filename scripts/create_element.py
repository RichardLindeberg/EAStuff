#!/usr/bin/env python3
"""
Interactive script to create new ArchiMate element markdown files.
Guides the user step-by-step through the element creation process.
"""

import os
import sys
import re
import glob
from datetime import datetime
from pathlib import Path


# Layer codes for ID generation
LAYER_CODES = {
    "strategy": "str",
    "business": "bus",
    "application": "app",
    "technology": "tec",
    "physical": "phy",
    "motivation": "mot",
    "implementation": "imp"
}

# Type codes for ID generation
TYPE_CODES = {
    # Strategy
    "resource": "rsrc",
    "capability": "capa",
    "value-stream": "vstr",
    "course-of-action": "cact",
    # Business
    "business-actor": "actr",
    "business-role": "role",
    "business-collaboration": "colab",
    "business-interface": "intf",
    "business-process": "proc",
    "business-function": "func",
    "business-interaction": "intr",
    "business-event": "evnt",
    "business-service": "srvc",
    "business-object": "objt",
    "contract": "cntr",
    "representation": "repr",
    "product": "prod",
    # Application
    "application-component": "comp",
    "application-collaboration": "colab",
    "application-interface": "intf",
    "application-function": "func",
    "application-interaction": "intr",
    "application-process": "proc",
    "application-event": "evnt",
    "application-service": "srvc",
    "data-object": "data",
    # Technology
    "node": "node",
    "device": "devc",
    "system-software": "sysw",
    "technology-collaboration": "colab",
    "technology-interface": "intf",
    "path": "path",
    "communication-network": "netw",
    "technology-function": "func",
    "technology-process": "proc",
    "technology-interaction": "intr",
    "technology-event": "evnt",
    "technology-service": "srvc",
    "artifact": "artf",
    # Physical
    "equipment": "equi",
    "facility": "faci",
    "distribution-network": "dist",
    "material": "matr",
    # Motivation
    "stakeholder": "stkh",
    "driver": "drvr",
    "assessment": "asmt",
    "goal": "goal",
    "outcome": "outc",
    "principle": "prin",
    "requirement": "reqt",
    "constraint": "cnst",
    "meaning": "mean",
    "value": "valu",
    # Implementation
    "work-package": "work",
    "deliverable": "delv",
    "implementation-event": "evnt",
    "plateau": "plat",
    "gap": "gap_"
}

# Define element types by layer
ELEMENT_TYPES = {
    "strategy": [
        "resource", "capability", "value-stream", "course-of-action"
    ],
    "business": [
        "business-actor", "business-role", "business-collaboration",
        "business-interface", "business-process", "business-function",
        "business-interaction", "business-event", "business-service",
        "business-object", "contract", "representation", "product"
    ],
    "application": [
        "application-component", "application-collaboration",
        "application-interface", "application-function",
        "application-interaction", "application-process",
        "application-event", "application-service", "data-object"
    ],
    "technology": [
        "node", "device", "system-software", "technology-collaboration",
        "technology-interface", "path", "communication-network",
        "technology-function", "technology-process",
        "technology-interaction", "technology-event",
        "technology-service", "artifact"
    ],
    "physical": [
        "equipment", "facility", "distribution-network", "material"
    ],
    "motivation": [
        "stakeholder", "driver", "assessment", "goal", "outcome",
        "principle", "requirement", "constraint", "meaning", "value"
    ],
    "implementation": [
        "work-package", "deliverable", "implementation-event",
        "plateau", "gap"
    ]
}

RELATIONSHIP_TYPES = [
    "composition", "aggregation", "assignment", "realization",
    "serving", "access", "influence", "association",
    "triggering", "flow", "specialization", "junction"
]


def print_header(text):
    """Print a formatted header."""
    print("\n" + "=" * 70)
    print(f"  {text}")
    print("=" * 70 + "\n")


def print_options(options, columns=3):
    """Print options in columns."""
    for i, option in enumerate(options, 1):
        print(f"{i:2d}. {option:30s}", end="")
        if i % columns == 0:
            print()
    if len(options) % columns != 0:
        print()


def get_choice(prompt, options, allow_empty=False):
    """Get a valid choice from a list of options."""
    while True:
        try:
            choice = input(f"\n{prompt} [1-{len(options)}]{' (or Enter to skip)' if allow_empty else ''}: ").strip()
            if allow_empty and choice == "":
                return None
            idx = int(choice) - 1
            if 0 <= idx < len(options):
                return options[idx]
            print(f"❌ Please enter a number between 1 and {len(options)}")
        except ValueError:
            print("❌ Please enter a valid number")
        except KeyboardInterrupt:
            print("\n\n⚠️  Operation cancelled by user")
            sys.exit(0)


def get_input(prompt, required=True, default=None):
    """Get text input from user."""
    while True:
        try:
            if default:
                value = input(f"{prompt} [{default}]: ").strip()
                if not value:
                    return default
                return value
            else:
                value = input(f"{prompt}: ").strip()
                if value or not required:
                    return value
                print("❌ This field is required")
        except KeyboardInterrupt:
            print("\n\n⚠️  Operation cancelled by user")
            sys.exit(0)


def get_yes_no(prompt, default=True):
    """Get yes/no input from user."""
    default_str = "Y/n" if default else "y/N"
    while True:
        try:
            choice = input(f"{prompt} [{default_str}]: ").strip().lower()
            if not choice:
                return default
            if choice in ['y', 'yes']:
                return True
            if choice in ['n', 'no']:
                return False
            print("❌ Please enter 'y' or 'n'")
        except KeyboardInterrupt:
            print("\n\n⚠️  Operation cancelled by user")
            sys.exit(0)


def sanitize_name_for_id(name):
    """Convert element name to ID-friendly format."""
    # Convert to lowercase
    name = name.lower()
    # Replace spaces and underscores with hyphens
    name = re.sub(r'[\s_]+', '-', name)
    # Remove any characters that aren't alphanumeric or hyphens
    name = re.sub(r'[^a-z0-9-]', '', name)
    # Remove leading/trailing hyphens
    name = name.strip('-')
    # Collapse multiple hyphens
    name = re.sub(r'-+', '-', name)
    # Limit length to reasonable size (max 30 chars)
    if len(name) > 30:
        # Try to split on hyphens and take first few words
        parts = name.split('-')
        name = '-'.join(parts[:4])[:30].rstrip('-')
    return name


def find_next_sequence_number(elements_dir, id_prefix):
    """Find the next available sequence number for a given ID prefix."""
    if not elements_dir.exists():
        return 1
    
    # Search all markdown files for IDs matching the prefix
    max_num = 0
    pattern = re.compile(rf'^id:\s*{re.escape(id_prefix)}-(\d{{3}})\s*$', re.MULTILINE)
    
    for md_file in elements_dir.glob('*.md'):
        try:
            content = md_file.read_text(encoding='utf-8')
            matches = pattern.findall(content)
            for match in matches:
                num = int(match)
                if num > max_num:
                    max_num = num
        except Exception:
            continue
    
    return max_num + 1


def generate_element_id(layer, element_type, element_name, elements_dir):
    """Generate a standardized element ID following the naming convention."""
    layer_code = LAYER_CODES.get(layer, "unk")
    type_code = TYPE_CODES.get(element_type, "type")
    name_part = sanitize_name_for_id(element_name)
    
    # Build the ID prefix
    id_prefix = f"{layer_code}-{type_code}-{name_part}"
    
    # Find next available sequence number
    seq_num = find_next_sequence_number(elements_dir, id_prefix)
    
    # Generate full ID
    element_id = f"{id_prefix}-{seq_num:03d}"
    
    return element_id


def get_relationships():
    """Collect relationship information."""
    relationships = []
    
    print("\n" + "-" * 70)
    print("  RELATIONSHIPS")
    print("-" * 70)
    print("Define relationships to other elements (optional)")
    
    while True:
        if not get_yes_no("\nAdd a relationship?", default=False):
            break
        
        print("\nRelationship Types:")
        print_options(RELATIONSHIP_TYPES, columns=2)
        rel_type = get_choice("Select relationship type", RELATIONSHIP_TYPES)
        
        target = get_input("Target element ID")
        description = get_input("Relationship description", required=False)
        
        relationships.append({
            "type": rel_type,
            "target": target,
            "description": description
        })
        
        print(f"✓ Added {rel_type} relationship to {target}")
    
    return relationships


def get_properties():
    """Collect property information."""
    properties = {}
    
    print("\n" + "-" * 70)
    print("  PROPERTIES")
    print("-" * 70)
    
    properties["owner"] = get_input("Owner/Department", required=False)
    
    status_options = ["draft", "proposed", "active", "production", "deprecated", "retired"]
    print("\nStatus options:")
    print_options(status_options, columns=3)
    properties["status"] = get_choice("Select status", status_options, allow_empty=True)
    
    criticality_options = ["low", "medium", "high", "critical"]
    print("\nCriticality options:")
    print_options(criticality_options, columns=4)
    properties["criticality"] = get_choice("Select criticality", criticality_options, allow_empty=True)
    
    properties["version"] = get_input("Version", required=False)
    
    lifecycle_options = ["plan", "design", "build", "operate", "retire"]
    print("\nLifecycle phase options:")
    print_options(lifecycle_options, columns=5)
    properties["lifecycle-phase"] = get_choice("Select lifecycle phase", lifecycle_options, allow_empty=True)
    
    # Add current date
    properties["last-updated"] = datetime.now().strftime("%Y-%m-%d")
    
    # Filter out None values
    return {k: v for k, v in properties.items() if v is not None}


def get_tags():
    """Collect tags."""
    print("\n" + "-" * 70)
    print("  TAGS")
    print("-" * 70)
    print("Enter tags (comma-separated, or press Enter to skip)")
    
    tags_input = get_input("Tags", required=False)
    if tags_input:
        return [tag.strip() for tag in tags_input.split(",") if tag.strip()]
    return []


def generate_markdown(element_data):
    """Generate the markdown file content."""
    lines = ["---"]
    
    # Required fields
    lines.append(f'id: {element_data["id"]}')
    lines.append(f'name: {element_data["name"]}')
    lines.append(f'type: {element_data["type"]}')
    lines.append(f'layer: {element_data["layer"]}')
    
    # Relationships
    if element_data.get("relationships"):
        lines.append("relationships:")
        for rel in element_data["relationships"]:
            lines.append(f'  - type: {rel["type"]}')
            lines.append(f'    target: {rel["target"]}')
            if rel.get("description"):
                lines.append(f'    description: {rel["description"]}')
    
    # Properties
    if element_data.get("properties"):
        lines.append("properties:")
        for key, value in element_data["properties"].items():
            # Quote string values
            if isinstance(value, str) and not value.replace(".", "").replace("-", "").isdigit():
                lines.append(f'  {key}: "{value}"')
            else:
                lines.append(f'  {key}: {value}')
    
    # Tags
    if element_data.get("tags"):
        lines.append("tags:")
        for tag in element_data["tags"]:
            lines.append(f'  - {tag}')
    
    lines.append("---")
    lines.append("")
    lines.append(f'# {element_data["name"]}')
    lines.append("")
    
    # Add description
    if element_data.get("short_description"):
        lines.append(element_data["short_description"])
        lines.append("")
    
    lines.append("## Description")
    lines.append("")
    if element_data.get("detailed_description"):
        lines.append(element_data["detailed_description"])
    else:
        lines.append(f'[Add detailed description of the {element_data["name"]} here]')
    lines.append("")
    
    lines.append("## Additional Information")
    lines.append("")
    lines.append("[Add any additional details, documentation, or notes here]")
    lines.append("")
    
    return "\n".join(lines)


def main():
    """Main function to run the interactive element creation process."""
    print_header("ArchiMate Element Creator")
    print("This interactive tool will guide you through creating a new")
    print("ArchiMate element markdown file.\n")
    print("Press Ctrl+C at any time to cancel.\n")
    
    element_data = {}
    
    # Step 1: Select Layer
    print_header("STEP 1: Select Layer")
    layers = list(ELEMENT_TYPES.keys())
    print_options(layers, columns=4)
    layer = get_choice("Select layer", layers)
    element_data["layer"] = layer
    
    # Step 2: Select Element Type
    print_header("STEP 2: Select Element Type")
    types = ELEMENT_TYPES[layer]
    print_options(types, columns=2)
    element_type = get_choice("Select element type", types)
    element_data["type"] = element_type
    
    # Step 3: Basic Information
    print_header("STEP 3: Basic Information")
    element_data["name"] = get_input("Element name (e.g., 'Customer Portal')")
    
    # Determine file path for ID generation
    script_dir = Path(__file__).parent
    project_root = script_dir.parent
    elements_dir = project_root / "elements" / layer
    
    # Auto-generate ID
    auto_id = generate_element_id(layer, element_type, element_data["name"], elements_dir)
    print(f"\n💡 Auto-generated ID: {auto_id}")
    print(f"   Format: [{LAYER_CODES[layer]}]-[{TYPE_CODES[element_type]}]-[name]-[###]")
    
    if get_yes_no("Use this ID?", default=True):
        element_data["id"] = auto_id
    else:
        element_data["id"] = get_input("Enter custom ID (follow standard format)")
    
    # Step 4: Descriptions
    print_header("STEP 4: Descriptions")
    element_data["short_description"] = get_input(
        "Short description (one line summary)", 
        required=False
    )
    print("\nDetailed description (press Enter twice to finish):")
    desc_lines = []
    empty_count = 0
    while empty_count < 1:
        try:
            line = input()
            if not line:
                empty_count += 1
            else:
                empty_count = 0
                desc_lines.append(line)
        except KeyboardInterrupt:
            print("\n\n⚠️  Operation cancelled by user")
            sys.exit(0)
    
    if desc_lines:
        element_data["detailed_description"] = "\n".join(desc_lines)
    
    # Step 5: Relationships
    print_header("STEP 5: Relationships")
    element_data["relationships"] = get_relationships()
    
    # Step 6: Properties
    print_header("STEP 6: Properties")
    element_data["properties"] = get_properties()
    
    # Step 7: Tags
    print_header("STEP 7: Tags")
    element_data["tags"] = get_tags()
    
    # Generate markdown content
    markdown_content = generate_markdown(element_data)
    
    # Show preview
    print_header("PREVIEW")
    print(markdown_content)
    
    # Confirm creation
    print_header("CONFIRMATION")
    if not get_yes_no("Create this element file?", default=True):
        print("\n⚠️  Element creation cancelled")
        sys.exit(0)
    
    # Determine file path
    script_dir = Path(__file__).parent
    project_root = script_dir.parent
    elements_dir = project_root / "elements" / layer
    
    # Create directory if it doesn't exist
    elements_dir.mkdir(parents=True, exist_ok=True)
    
    # Generate filename from element name
    filename = element_data["name"].lower().replace(" ", "-").replace("_", "-")
    filename = "".join(c for c in filename if c.isalnum() or c == "-")
    filepath = elements_dir / f"{filename}.md"
    
    # Check if file exists
    if filepath.exists():
        print(f"\n⚠️  File already exists: {filepath}")
        if not get_yes_no("Overwrite existing file?", default=False):
            print("\n⚠️  Element creation cancelled")
            sys.exit(0)
    
    # Write file
    try:
        with open(filepath, "w", encoding="utf-8") as f:
            f.write(markdown_content)
        
        print_header("SUCCESS")
        print(f"✅ Element created successfully!")
        print(f"\n📁 File location: {filepath}")
        print(f"\n💡 Next steps:")
        print(f"   1. Review and edit the file to add more details")
        print(f"   2. Run validation: python scripts/validator/validate.py")
        print(f"   3. Generate diagrams: python scripts/generators/generate_mermaid.py")
        
    except Exception as e:
        print(f"\n❌ Error creating file: {e}")
        sys.exit(1)


if __name__ == "__main__":
    main()
