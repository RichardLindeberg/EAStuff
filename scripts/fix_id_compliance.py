#!/usr/bin/env python3
"""
Fix element IDs and filenames to comply with naming standard.
Updates all references across files.
"""

import os
import re
import yaml
import shutil
from pathlib import Path
from typing import Dict, List, Tuple
from collections import defaultdict

# Import the audit functions
import sys
script_dir = Path(__file__).parent
sys.path.insert(0, str(script_dir))

from audit_id_compliance import (
    audit_elements,
    parse_element_file,
    validate_id,
    validate_filename,
    generate_correct_id
)

def update_element_id(file_path: Path, old_id: str, new_id: str) -> bool:
    """Update the ID in an element file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Extract frontmatter
        if not content.startswith('---'):
            return False
        
        parts = content.split('---', 2)
        if len(parts) < 3:
            return False
        
        frontmatter_text = parts[1]
        body = parts[2]
        
        # Parse and update frontmatter
        frontmatter = yaml.safe_load(frontmatter_text)
        frontmatter['id'] = new_id
        
        # Add legacy ID if changing
        if old_id != new_id and old_id:
            if 'properties' not in frontmatter:
                frontmatter['properties'] = {}
            if not isinstance(frontmatter['properties'], dict):
                frontmatter['properties'] = {}
            frontmatter['properties']['legacy-id'] = old_id
        
        # Reconstruct file
        new_content = '---\n'
        new_content += yaml.dump(frontmatter, default_flow_style=False, sort_keys=False, allow_unicode=True)
        new_content += '---'
        new_content += body
        
        # Write back
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
        
        # Extract frontmatter
        if not content.startswith('---'):
            return 0
        
        parts = content.split('---', 2)
        if len(parts) < 3:
            return 0
        
        frontmatter_text = parts[1]
        body = parts[2]
        
        # Parse frontmatter
        frontmatter = yaml.safe_load(frontmatter_text)
        
        updates_made = 0
        
        # Update relationships
        if 'relationships' in frontmatter and isinstance(frontmatter['relationships'], list):
            for rel in frontmatter['relationships']:
                if 'target' in rel:
                    old_target = rel['target']
                    if old_target in id_mappings:
                        rel['target'] = id_mappings[old_target]
                        updates_made += 1
        
        if updates_made > 0:
            # Reconstruct file
            new_content = '---\n'
            new_content += yaml.dump(frontmatter, default_flow_style=False, sort_keys=False, allow_unicode=True)
            new_content += '---'
            new_content += body
            
            # Write back
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(new_content)
        
        return updates_made
    except Exception as e:
        print(f"Error updating references in {file_path}: {e}")
        return 0

def rename_file(old_path: Path, new_path: Path) -> bool:
    """Rename a file safely."""
    try:
        # Create parent directory if needed
        new_path.parent.mkdir(parents=True, exist_ok=True)
        
        # Move file
        shutil.move(str(old_path), str(new_path))
        return True
    except Exception as e:
        print(f"Error renaming {old_path} to {new_path}: {e}")
        return False

def fix_all_elements(elements_dir: Path, dry_run: bool = False):
    """Fix all non-compliant elements."""
    print("Running audit...")
    results = audit_elements(elements_dir)
    
    print(f"\nFound {len(results['non_compliant_id'])} elements with non-compliant IDs")
    print(f"Found {len(results['non_compliant_filename'])} elements with non-compliant filenames")
    
    if dry_run:
        print("\n=== DRY RUN MODE - No changes will be made ===\n")
    else:
        print("\n=== APPLYING FIXES ===\n")
    
    # Build ID mapping
    id_mappings = {}
    file_renames = []
    
    # Collect all ID changes
    for item in results['non_compliant_id']:
        old_id = item['current_id']
        new_id = item['suggested_id']
        if old_id and new_id and old_id != new_id:
            id_mappings[old_id] = new_id
            print(f"ID Change: {old_id} -> {new_id}")
    
    print(f"\nTotal ID changes: {len(id_mappings)}")
    
    if not dry_run:
        # Step 1: Update all IDs in files
        print("\nStep 1: Updating IDs in element files...")
        for item in results['non_compliant_id']:
            file_path = elements_dir / item['file']
            old_id = item['current_id']
            new_id = item['suggested_id']
            
            if old_id != new_id:
                print(f"  Updating {item['file']}")
                update_element_id(file_path, old_id, new_id)
        
        # Step 2: Update all references
        print("\nStep 2: Updating references across all files...")
        total_refs_updated = 0
        for md_file in elements_dir.rglob('*.md'):
            refs_updated = update_references_in_file(md_file, id_mappings)
            if refs_updated > 0:
                relative_path = md_file.relative_to(elements_dir)
                print(f"  Updated {refs_updated} reference(s) in {relative_path}")
                total_refs_updated += refs_updated
        
        print(f"\nTotal references updated: {total_refs_updated}")
        
        # Step 3: Rename files
        print("\nStep 3: Renaming files to match IDs...")
        
        # Need to re-audit to get updated IDs
        results = audit_elements(elements_dir)
        
        for item in results['non_compliant_filename']:
            old_file = elements_dir / item['file']
            
            # Get the directory part
            file_dir = old_file.parent
            
            # New filename should match the ID
            new_filename = item['suggested_filename']
            new_file = file_dir / new_filename
            
            if old_file != new_file and old_file.exists():
                print(f"  Renaming: {item['current_filename']} -> {new_filename}")
                rename_file(old_file, new_file)
        
        print("\n=== FIXES COMPLETE ===")
        print("\nRunning final audit...")
        
        # Final audit
        final_results = audit_elements(elements_dir)
        print(f"\nFinal compliance status:")
        print(f"✓ Compliant: {len(final_results['compliant'])}")
        print(f"✗ Non-compliant IDs: {len(final_results['non_compliant_id'])}")
        print(f"✗ Non-compliant filenames: {len(final_results['non_compliant_filename'])}")
    else:
        print("\nDry run complete. Run without --dry-run to apply changes.")

def main():
    """Main entry point."""
    import argparse
    
    parser = argparse.ArgumentParser(description='Fix element ID and filename compliance')
    parser.add_argument('--dry-run', action='store_true', help='Show what would be changed without making changes')
    args = parser.parse_args()
    
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent
    elements_dir = workspace_dir / 'elements'
    
    if not elements_dir.exists():
        print(f"Error: Elements directory not found: {elements_dir}")
        return
    
    fix_all_elements(elements_dir, dry_run=args.dry_run)

if __name__ == '__main__':
    main()
