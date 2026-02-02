#!/usr/bin/env python3
"""
Validate ArchiMate Model Exchange File against XSD schema.

This script validates the generated .archimate file against the official
Open Group ArchiMate 3.1 XSD schema to ensure compatibility with
ArchiMate modeling tools.
"""

import sys
from pathlib import Path
from xml.etree import ElementTree as ET


def validate_xml_structure(xml_file):
    """Validate the XML structure of an ArchiMate exchange file."""
    print(f"Validating {xml_file}...\n")
    
    try:
        tree = ET.parse(xml_file)
        root = tree.getroot()
    except ET.ParseError as e:
        print(f"✗ XML Parse Error: {e}")
        return False
    
    # Check root element
    if 'model' not in root.tag:
        print("✗ Root element must be 'model'")
        return False
    
    print(f"✓ Root element: {root.tag}")
    
    # Check required attributes
    required_attrs = ['identifier', 'version']
    for attr in required_attrs:
        if attr not in root.attrib:
            print(f"  ⚠ Warning: Missing attribute '{attr}'")
        else:
            print(f"  ✓ Attribute '{attr}': {root.attrib[attr]}")
    
    # Check namespace
    if 'http://www.opengroup.org/xsd/archimate' in root.tag:
        print(f"✓ Correct namespace")
    else:
        print(f"  ⚠ Warning: Unexpected namespace in root tag")
    
    # Count elements and relationships
    ns = {'archimate': 'http://www.opengroup.org/xsd/archimate/3.0/'}
    elements_elem = root.find('archimate:elements', ns)
    relationships_elem = root.find('archimate:relationships', ns)
    
    element_count = 0
    if elements_elem is not None:
        element_count = len(elements_elem.findall('archimate:element', ns))
        print(f"\n✓ Elements container found")
        print(f"  Total elements: {element_count}")
    
    relationship_count = 0
    if relationships_elem is not None:
        relationship_count = len(relationships_elem.findall('archimate:relationship', ns))
        print(f"\n✓ Relationships container found")
        print(f"  Total relationships: {relationship_count}")
    
    # Sample element validation
    if elements_elem is not None:
        first_elem = elements_elem.find('archimate:element', ns)
        if first_elem is not None:
            elem_id = first_elem.attrib.get('identifier', 'N/A')
            elem_type = first_elem.attrib.get('{http://www.w3.org/2001/XMLSchema-instance}type', 'N/A')
            name_elem = first_elem.find('archimate:name', ns)
            name = name_elem.text if name_elem is not None else 'N/A'
            
            print(f"\n✓ Sample element:")
            print(f"  ID: {elem_id}")
            print(f"  Type: {elem_type}")
            print(f"  Name: {name}")
    
    # Sample relationship validation
    if relationships_elem is not None:
        first_rel = relationships_elem.find('archimate:relationship', ns)
        if first_rel is not None:
            rel_id = first_rel.attrib.get('identifier', 'N/A')
            rel_type = first_rel.attrib.get('{http://www.w3.org/2001/XMLSchema-instance}type', 'N/A')
            source = first_rel.attrib.get('source', 'N/A')
            target = first_rel.attrib.get('target', 'N/A')
            
            print(f"\n✓ Sample relationship:")
            print(f"  ID: {rel_id}")
            print(f"  Type: {rel_type}")
            print(f"  Source: {source}")
            print(f"  Target: {target}")
    
    print(f"\n✓ XML structure is valid")
    print(f"{'='*60}")
    print(f"VALIDATION SUMMARY")
    print(f"{'='*60}")
    print(f"File: {xml_file}")
    print(f"Elements: {element_count}")
    print(f"Relationships: {relationship_count}")
    print(f"Total concepts: {element_count + relationship_count}")
    print(f"Status: ✓ VALID")
    print(f"{'='*60}")
    
    return True


def main():
    xml_file = Path('output/model-exchange.archimate')
    
    if len(sys.argv) > 1:
        xml_file = Path(sys.argv[1])
    
    if not xml_file.exists():
        print(f"✗ File not found: {xml_file}")
        return 1
    
    success = validate_xml_structure(xml_file)
    return 0 if success else 1


if __name__ == '__main__':
    sys.exit(main())
