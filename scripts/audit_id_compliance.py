#!/usr/bin/env python3
"""
Audit element files for ID naming standard compliance.
Checks both file names and IDs within files against the standard.
"""

import os
import re
import yaml
from pathlib import Path
from typing import Dict, List, Tuple
from collections import defaultdict

# Standard format: [layer-code]-[type-code]-[###]-[descriptive-name]
ID_PATTERN = re.compile(r'^([a-z]{3})-([a-z]{4,5})-(\d{3})-([a-z0-9-]+)$')

# Valid layer codes
LAYER_CODES = {
    'str': 'strategy',
    'bus': 'business',
    'app': 'application',
    'tec': 'technology',
    'phy': 'physical',
    'mot': 'motivation',
    'imp': 'implementation'
}

# Valid type codes per layer
TYPE_CODES = {
    'strategy': ['rsrc', 'capa', 'vstr', 'cact'],
    'business': ['actr', 'role', 'colab', 'intf', 'proc', 'func', 'intr', 'evnt', 'srvc', 'objt', 'cntr', 'repr', 'prod'],
    'application': ['comp', 'colab', 'intf', 'func', 'intr', 'proc', 'evnt', 'srvc', 'data'],
    'technology': ['node', 'devc', 'sysw', 'colab', 'intf', 'path', 'netw', 'func', 'proc', 'intr', 'evnt', 'srvc', 'artf'],
    'physical': ['equi', 'faci', 'dist', 'matr'],
    'motivation': ['stkh', 'drvr', 'asmt', 'goal', 'outc', 'prin', 'reqt', 'cnst', 'mean', 'valu'],
    'implementation': ['work', 'delv', 'evnt', 'plat', 'gap_']
}

def parse_element_file(file_path: Path) -> Dict:
    """Parse element file and extract frontmatter."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Extract YAML frontmatter
        if content.startswith('---'):
            parts = content.split('---', 2)
            if len(parts) >= 3:
                frontmatter = yaml.safe_load(parts[1])
                return frontmatter
    except Exception as e:
        return {'error': str(e)}
    return {}

def validate_id(element_id: str, layer: str) -> Tuple[bool, str]:
    """Validate ID against naming standard."""
    if not element_id:
        return False, "Missing ID"
    
    match = ID_PATTERN.match(element_id)
    if not match:
        return False, f"Does not match format [layer]-[type]-[###]-[name]"
    
    layer_code, type_code, seq_num, desc_name = match.groups()
    
    # Check layer code
    if layer_code not in LAYER_CODES:
        return False, f"Invalid layer code '{layer_code}'"
    
    expected_layer = LAYER_CODES[layer_code]
    if layer and expected_layer != layer:
        return False, f"Layer code '{layer_code}' doesn't match layer '{layer}'"
    
    # Check type code
    if expected_layer not in TYPE_CODES:
        return False, f"Unknown layer '{expected_layer}'"
    
    if type_code not in TYPE_CODES[expected_layer]:
        return False, f"Invalid type code '{type_code}' for layer '{expected_layer}'"
    
    # Check descriptive name
    if not re.match(r'^[a-z0-9-]+$', desc_name):
        return False, f"Descriptive name contains invalid characters"
    
    if desc_name.startswith('-') or desc_name.endswith('-'):
        return False, f"Descriptive name cannot start or end with hyphen"
    
    if '--' in desc_name:
        return False, f"Descriptive name cannot contain consecutive hyphens"
    
    return True, "Valid"

def validate_filename(filename: str, element_id: str) -> Tuple[bool, str]:
    """Check if filename matches ID."""
    expected_filename = f"{element_id}.md"
    if filename == expected_filename:
        return True, "Matches ID"
    else:
        return False, f"Should be '{expected_filename}'"

def generate_correct_id(frontmatter: Dict, current_id: str, preserve_sequence: bool = True) -> str:
    """Generate correct ID based on element data."""
    layer = frontmatter.get('layer', '')
    element_type = frontmatter.get('type', '')
    name = frontmatter.get('name', '')
    
    # Find layer code
    layer_code = None
    for code, layer_name in LAYER_CODES.items():
        if layer_name == layer:
            layer_code = code
            break
    
    if not layer_code:
        return current_id
    
    # Find type code
    type_code = None
    if layer in TYPE_CODES:
        # Map element type to type code
        type_mapping = {
            'capability': 'capa',
            'resource': 'rsrc',
            'value-stream': 'vstr',
            'course-of-action': 'cact',
            'business-actor': 'actr',
            'business-role': 'role',
            'business-collaboration': 'colab',
            'business-interface': 'intf',
            'business-process': 'proc',
            'business-function': 'func',
            'business-interaction': 'intr',
            'business-event': 'evnt',
            'business-service': 'srvc',
            'business-object': 'objt',
            'contract': 'cntr',
            'representation': 'repr',
            'product': 'prod',
            'application-component': 'comp',
            'application-collaboration': 'colab',
            'application-interface': 'intf',
            'application-function': 'func',
            'application-interaction': 'intr',
            'application-process': 'proc',
            'application-event': 'evnt',
            'application-service': 'srvc',
            'data-object': 'data',
            'node': 'node',
            'device': 'devc',
            'system-software': 'sysw',
            'technology-collaboration': 'colab',
            'technology-interface': 'intf',
            'path': 'path',
            'communication-network': 'netw',
            'technology-function': 'func',
            'technology-process': 'proc',
            'technology-interaction': 'intr',
            'technology-event': 'evnt',
            'technology-service': 'srvc',
            'artifact': 'artf',
            'equipment': 'equi',
            'facility': 'faci',
            'distribution-network': 'dist',
            'material': 'matr',
            'stakeholder': 'stkh',
            'driver': 'drvr',
            'assessment': 'asmt',
            'goal': 'goal',
            'outcome': 'outc',
            'principle': 'prin',
            'requirement': 'reqt',
            'constraint': 'cnst',
            'meaning': 'mean',
            'value': 'valu',
            'work-package': 'work',
            'deliverable': 'delv',
            'implementation-event': 'evnt',
            'plateau': 'plat',
            'gap': 'gap_'
        }
        type_code = type_mapping.get(element_type)
    
    if not type_code:
        return current_id
    
    # Extract sequence number from current ID if present and preserve_sequence is True
    seq_num = '001'
    if current_id and preserve_sequence:
        # Try to extract from different ID formats
        # Format 1: [layer]-[type]-[###]-[name]
        match = ID_PATTERN.match(current_id)
        if match:
            seq_num = match.group(3)
        else:
            # Format 2: [layer]-[type]-[name]-[###] (old format)
            old_pattern = re.compile(r'^[a-z]{3}-[a-z]{4,5}-[a-z0-9-]+-(\d{3})$')
            match = old_pattern.match(current_id)
            if match:
                seq_num = match.group(1)
            else:
                # Format 3: [layer]-[code]-[###]-[name] where code might vary
                partial_pattern = re.compile(r'^[a-z]{3}-[a-z]{4,}-(\d{3})-')
                match = partial_pattern.match(current_id)
                if match:
                    seq_num = match.group(1)
    
    # Generate descriptive name from element name
    desc_name = name.lower()
    desc_name = re.sub(r'[^a-z0-9\s-]', '', desc_name)
    desc_name = re.sub(r'\s+', '-', desc_name)
    desc_name = re.sub(r'-+', '-', desc_name)
    desc_name = desc_name.strip('-')
    
    # Limit to reasonable length (4 words max, ~40 chars)
    words = desc_name.split('-')
    if len(words) > 4:
        desc_name = '-'.join(words[:4])
    if len(desc_name) > 40:
        desc_name = desc_name[:40].rstrip('-')
    
    return f"{layer_code}-{type_code}-{seq_num}-{desc_name}"

def audit_elements(elements_dir: Path) -> Dict:
    """Audit all element files."""
    results = {
        'compliant': [],
        'non_compliant_id': [],
        'non_compliant_filename': [],
        'errors': [],
        'id_references': defaultdict(list)
    }
    
    # Scan all .md files
    for md_file in elements_dir.rglob('*.md'):
        relative_path = md_file.relative_to(elements_dir)
        filename = md_file.name
        
        # Parse file
        frontmatter = parse_element_file(md_file)
        
        if 'error' in frontmatter:
            results['errors'].append({
                'file': str(relative_path),
                'error': frontmatter['error']
            })
            continue
        
        element_id = frontmatter.get('id', '')
        layer = frontmatter.get('layer', '')
        name = frontmatter.get('name', '')
        
        # Validate ID
        id_valid, id_message = validate_id(element_id, layer)
        
        # Validate filename
        filename_valid, filename_message = validate_filename(filename, element_id)
        
        # Generate suggested correct ID
        suggested_id = generate_correct_id(frontmatter, element_id)
        suggested_filename = f"{suggested_id}.md"
        
        file_info = {
            'file': str(relative_path),
            'current_filename': filename,
            'current_id': element_id,
            'name': name,
            'layer': layer,
            'type': frontmatter.get('type', ''),
            'id_valid': id_valid,
            'id_message': id_message,
            'filename_valid': filename_valid,
            'filename_message': filename_message,
            'suggested_id': suggested_id,
            'suggested_filename': suggested_filename
        }
        
        # Categorize
        if id_valid and filename_valid:
            results['compliant'].append(file_info)
        else:
            if not id_valid:
                results['non_compliant_id'].append(file_info)
            if not filename_valid:
                results['non_compliant_filename'].append(file_info)
        
        # Track relationships for reference analysis
        relationships = frontmatter.get('relationships', [])
        if relationships:
            for rel in relationships:
                target = rel.get('target', '')
                if target:
                    results['id_references'][target].append({
                        'source_file': str(relative_path),
                        'source_id': element_id,
                        'relationship_type': rel.get('type', '')
                    })
    
    return results

def print_report(results: Dict):
    """Print audit report."""
    print("\n" + "=" * 80)
    print("ELEMENT ID NAMING STANDARD AUDIT REPORT")
    print("=" * 80)
    
    total_files = len(results['compliant']) + len(results['non_compliant_id']) + len(results['non_compliant_filename'])
    print(f"\nTotal files scanned: {total_files}")
    print(f"✓ Compliant: {len(results['compliant'])}")
    print(f"✗ Non-compliant IDs: {len(results['non_compliant_id'])}")
    print(f"✗ Non-compliant filenames: {len(results['non_compliant_filename'])}")
    print(f"⚠ Errors: {len(results['errors'])}")
    
    if results['errors']:
        print("\n" + "-" * 80)
        print("ERRORS")
        print("-" * 80)
        for error in results['errors']:
            print(f"\n{error['file']}")
            print(f"  Error: {error['error']}")
    
    if results['non_compliant_id']:
        print("\n" + "-" * 80)
        print("NON-COMPLIANT IDs")
        print("-" * 80)
        for item in results['non_compliant_id']:
            print(f"\n{item['file']}")
            print(f"  Current ID: {item['current_id']}")
            print(f"  Issue: {item['id_message']}")
            print(f"  Suggested: {item['suggested_id']}")
            
            # Check if ID is referenced
            if item['current_id'] in results['id_references']:
                refs = results['id_references'][item['current_id']]
                print(f"  ⚠ Referenced by {len(refs)} file(s):")
                for ref in refs[:3]:  # Show first 3
                    print(f"    - {ref['source_file']}")
                if len(refs) > 3:
                    print(f"    ... and {len(refs) - 3} more")
    
    if results['non_compliant_filename']:
        print("\n" + "-" * 80)
        print("NON-COMPLIANT FILENAMES")
        print("-" * 80)
        for item in results['non_compliant_filename']:
            if item not in results['non_compliant_id']:  # Don't duplicate
                print(f"\n{item['file']}")
                print(f"  Current: {item['current_filename']}")
                print(f"  Suggested: {item['suggested_filename']}")
    
    print("\n" + "=" * 80)
    print("END OF REPORT")
    print("=" * 80)

def save_report(results: Dict, output_file: Path):
    """Save detailed report to file."""
    with open(output_file, 'w', encoding='utf-8') as f:
        f.write("# Element ID Naming Standard Audit Report\n\n")
        
        total_files = len(results['compliant']) + len(results['non_compliant_id']) + len(results['non_compliant_filename'])
        f.write(f"**Total files scanned:** {total_files}\n\n")
        f.write(f"- ✓ Compliant: {len(results['compliant'])}\n")
        f.write(f"- ✗ Non-compliant IDs: {len(results['non_compliant_id'])}\n")
        f.write(f"- ✗ Non-compliant filenames: {len(results['non_compliant_filename'])}\n")
        f.write(f"- ⚠ Errors: {len(results['errors'])}\n\n")
        
        if results['non_compliant_id']:
            f.write("## Non-Compliant IDs\n\n")
            for item in results['non_compliant_id']:
                f.write(f"### {item['file']}\n\n")
                f.write(f"- **Current ID:** `{item['current_id']}`\n")
                f.write(f"- **Issue:** {item['id_message']}\n")
                f.write(f"- **Suggested:** `{item['suggested_id']}`\n")
                f.write(f"- **Element Name:** {item['name']}\n")
                f.write(f"- **Layer:** {item['layer']}\n")
                f.write(f"- **Type:** {item['type']}\n")
                
                if item['current_id'] in results['id_references']:
                    refs = results['id_references'][item['current_id']]
                    f.write(f"- **Referenced by:** {len(refs)} file(s)\n")
                    for ref in refs:
                        f.write(f"  - {ref['source_file']} ({ref['relationship_type']})\n")
                f.write("\n")
        
        if results['non_compliant_filename']:
            f.write("## Non-Compliant Filenames\n\n")
            for item in results['non_compliant_filename']:
                if item not in results['non_compliant_id']:
                    f.write(f"### {item['file']}\n\n")
                    f.write(f"- **Current:** `{item['current_filename']}`\n")
                    f.write(f"- **Suggested:** `{item['suggested_filename']}`\n")
                    f.write(f"- **ID:** `{item['current_id']}`\n\n")
        
        if results['errors']:
            f.write("## Errors\n\n")
            for error in results['errors']:
                f.write(f"### {error['file']}\n\n")
                f.write(f"- **Error:** {error['error']}\n\n")

def main():
    """Main entry point."""
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent
    elements_dir = workspace_dir / 'elements'
    
    if not elements_dir.exists():
        print(f"Error: Elements directory not found: {elements_dir}")
        return
    
    print("Scanning element files...")
    results = audit_elements(elements_dir)
    
    # Print console report
    print_report(results)
    
    # Save detailed report
    report_file = workspace_dir / 'ID-COMPLIANCE-AUDIT.md'
    save_report(results, report_file)
    print(f"\nDetailed report saved to: {report_file}")

if __name__ == '__main__':
    main()
