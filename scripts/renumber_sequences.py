#!/usr/bin/env python3
"""
Renumber element sequence numbers to ensure uniqueness within layer-type combinations.
"""

import os
import re
import yaml
import shutil
from pathlib import Path
from collections import defaultdict
from typing import Dict, List, Tuple

def parse_element_file(file_path: Path) -> Dict:
    """Parse element file and extract frontmatter."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        if content.startswith('---'):
            parts = content.split('---', 2)
            if len(parts) >= 3:
                frontmatter = yaml.safe_load(parts[1])
                return frontmatter
    except Exception as e:
        return {'error': str(e)}
    return {}

def update_element_id(file_path: Path, new_id: str) -> bool:
    """Update the ID in an element file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        if not content.startswith('---'):
            return False
        
        parts = content.split('---', 2)
        if len(parts) < 3:
            return False
        
        frontmatter_text = parts[1]
        body = parts[2]
        
        frontmatter = yaml.safe_load(frontmatter_text)
        old_id = frontmatter.get('id', '')
        frontmatter['id'] = new_id
        
        # Reconstruct file
        new_content = '---\n'
        new_content += yaml.dump(frontmatter, default_flow_style=False, sort_keys=False, allow_unicode=True)
        new_content += '---'
        new_content += body
        
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(new_content)
        
        return True
    except Exception as e:
        print(f"Error updating {file_path}: {e}")
        return False

def update_references_in_file(file_path: Path, id_mappings: Dict[str, str]) -> int:
    """Update all ID references in a file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        if not content.startswith('---'):
            return 0
        
        parts = content.split('---', 2)
        if len(parts) < 3:
            return 0
        
        frontmatter_text = parts[1]
        body = parts[2]
        
        frontmatter = yaml.safe_load(frontmatter_text)
        
        updates_made = 0
        
        if 'relationships' in frontmatter and isinstance(frontmatter['relationships'], list):
            for rel in frontmatter['relationships']:
                if 'target' in rel:
                    old_target = rel['target']
                    if old_target in id_mappings:
                        rel['target'] = id_mappings[old_target]
                        updates_made += 1
        
        if updates_made > 0:
            new_content = '---\n'
            new_content += yaml.dump(frontmatter, default_flow_style=False, sort_keys=False, allow_unicode=True)
            new_content += '---'
            new_content += body
            
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(new_content)
        
        return updates_made
    except Exception as e:
        print(f"Error updating references in {file_path}: {e}")
        return 0

def renumber_sequences(elements_dir: Path, dry_run: bool = False):
    """Renumber sequences to ensure uniqueness within layer-type combinations."""
    
    print("Scanning all element files...")
    
    # Group files by layer-type combination
    id_pattern = re.compile(r'^([a-z]{3})-([a-z]{4,5})-(\d{3})-(.+)$')
    layer_type_groups = defaultdict(list)
    
    for md_file in elements_dir.rglob('*.md'):
        frontmatter = parse_element_file(md_file)
        if 'error' in frontmatter:
            continue
        
        element_id = frontmatter.get('id', '')
        match = id_pattern.match(element_id)
        
        if match:
            layer_code = match.group(1)
            type_code = match.group(2)
            seq_num = match.group(3)
            desc_name = match.group(4)
            
            key = f"{layer_code}-{type_code}"
            layer_type_groups[key].append({
                'file': md_file,
                'id': element_id,
                'layer_code': layer_code,
                'type_code': type_code,
                'seq_num': int(seq_num),
                'desc_name': desc_name,
                'name': frontmatter.get('name', '')
            })
    
    # Find duplicates and create renumbering plan
    id_mappings = {}
    renaming_plan = []
    
    print("\nAnalyzing sequence number conflicts...")
    
    for key, elements in sorted(layer_type_groups.items()):
        # Sort by current sequence number, then by name for stability
        elements.sort(key=lambda x: (x['seq_num'], x['name']))
        
        # Check for duplicates
        seq_nums = [e['seq_num'] for e in elements]
        has_duplicates = len(seq_nums) != len(set(seq_nums))
        
        if has_duplicates or len(elements) > 1:
            print(f"\n{key}: {len(elements)} elements")
            
            # Assign new sequential numbers
            for i, element in enumerate(elements, start=1):
                new_seq = f"{i:03d}"
                old_id = element['id']
                new_id = f"{element['layer_code']}-{element['type_code']}-{new_seq}-{element['desc_name']}"
                
                if old_id != new_id:
                    print(f"  {old_id} -> {new_id}")
                    id_mappings[old_id] = new_id
                    
                    old_filename = element['file'].name
                    new_filename = f"{new_id}.md"
                    
                    renaming_plan.append({
                        'file': element['file'],
                        'old_id': old_id,
                        'new_id': new_id,
                        'old_filename': old_filename,
                        'new_filename': new_filename
                    })
    
    print(f"\n\nTotal ID changes needed: {len(id_mappings)}")
    
    if dry_run:
        print("\n=== DRY RUN MODE - No changes will be made ===")
        return
    
    if len(id_mappings) == 0:
        print("\nNo changes needed. All sequence numbers are unique!")
        return
    
    print("\n=== APPLYING CHANGES ===")
    
    # Step 1: Update IDs in files
    print("\nStep 1: Updating IDs in element files...")
    for item in renaming_plan:
        update_element_id(item['file'], item['new_id'])
        print(f"  Updated {item['file'].relative_to(elements_dir)}")
    
    # Step 2: Update all references
    print("\nStep 2: Updating references across all files...")
    total_refs = 0
    for md_file in elements_dir.rglob('*.md'):
        refs = update_references_in_file(md_file, id_mappings)
        if refs > 0:
            print(f"  Updated {refs} reference(s) in {md_file.relative_to(elements_dir)}")
            total_refs += refs
    
    print(f"\nTotal references updated: {total_refs}")
    
    # Step 3: Rename files
    print("\nStep 3: Renaming files to match new IDs...")
    for item in renaming_plan:
        old_path = item['file']
        new_path = old_path.parent / item['new_filename']
        
        if old_path != new_path and old_path.exists():
            shutil.move(str(old_path), str(new_path))
            print(f"  Renamed: {item['old_filename']} -> {item['new_filename']}")
    
    print("\n=== RENUMBERING COMPLETE ===")
    
    # Final verification
    print("\nVerifying uniqueness...")
    layer_type_groups_final = defaultdict(list)
    
    for md_file in elements_dir.rglob('*.md'):
        frontmatter = parse_element_file(md_file)
        element_id = frontmatter.get('id', '')
        match = id_pattern.match(element_id)
        
        if match:
            key = f"{match.group(1)}-{match.group(2)}"
            layer_type_groups_final[key].append(int(match.group(3)))
    
    conflicts = []
    for key, seq_nums in sorted(layer_type_groups_final.items()):
        if len(seq_nums) != len(set(seq_nums)):
            conflicts.append(key)
            print(f"  ⚠ {key}: Still has duplicates!")
    
    if not conflicts:
        print("  ✓ All sequence numbers are now unique within their layer-type groups!")

def main():
    """Main entry point."""
    import argparse
    
    parser = argparse.ArgumentParser(description='Renumber element sequences for uniqueness')
    parser.add_argument('--dry-run', action='store_true', help='Show what would be changed')
    args = parser.parse_args()
    
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent
    elements_dir = workspace_dir / 'elements'
    
    if not elements_dir.exists():
        print(f"Error: Elements directory not found: {elements_dir}")
        return
    
    renumber_sequences(elements_dir, dry_run=args.dry_run)

if __name__ == '__main__':
    main()
