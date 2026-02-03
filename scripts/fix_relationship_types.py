#!/usr/bin/env python3
"""
Fix invalid relationship types in element files.
"""

import os
import re
from pathlib import Path

def fix_relationship_types(file_path: Path) -> int:
    """Fix invalid relationship types in a file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Fix: realizationOf -> realization
        content = re.sub(r'^(\s*-\s*type:\s*)realizationOf\s*$', r'\1realization', content, flags=re.MULTILINE)
        
        # Fix: influences -> influence
        content = re.sub(r'^(\s*-\s*type:\s*)influences\s*$', r'\1influence', content, flags=re.MULTILINE)
        
        # Fix: supports -> serving
        content = re.sub(r'^(\s*-\s*type:\s*)supports\s*$', r'\1serving', content, flags=re.MULTILINE)
        
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            return 1
        
        return 0
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return 0

def main():
    """Main entry point."""
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent
    elements_dir = workspace_dir / 'elements'
    
    if not elements_dir.exists():
        print(f"Error: Elements directory not found: {elements_dir}")
        return 1
    
    print("Fixing invalid relationship types...")
    print("  realizationOf -> realization")
    print("  influences -> influence")
    print("  supports -> serving\n")
    
    files_fixed = 0
    
    for md_file in elements_dir.rglob('*.md'):
        if fix_relationship_types(md_file):
            rel_path = md_file.relative_to(elements_dir)
            print(f"  Fixed: {rel_path}")
            files_fixed += 1
    
    print(f"\n✓ Fixed {files_fixed} file(s)")
    return 0

if __name__ == '__main__':
    exit(main())
