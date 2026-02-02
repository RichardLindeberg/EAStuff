#!/usr/bin/env python3
"""
ID Migration Script

This script migrates ArchiMate element IDs from the old format to the new format:
- Old format: [layer-code]-[type-code]-[descriptive-name]-[###]
- New format: [layer-code]-[type-code]-[###]-[descriptive-name]

The script will:
1. Scan all element files in the elements/ directory
2. Extract and convert IDs from old format to new format
3. Update the ID in each element file
4. Update all references to that ID in other element files (relationships)
5. Create a mapping file for reference
6. Create a backup before making changes

Usage:
    python migrate_ids.py [--dry-run] [--backup-dir BACKUP_DIR]

Options:
    --dry-run       Preview changes without modifying files
    --backup-dir    Specify custom backup directory (default: backups/migration_TIMESTAMP)
"""

import os
import re
import yaml
import shutil
from pathlib import Path
from datetime import datetime
from typing import Dict, List, Tuple, Optional
import argparse


class IDMigrator:
    """Migrates ArchiMate element IDs from old format to new format."""
    
    # Regex pattern to match old format: layer-type-name-###
    OLD_FORMAT_PATTERN = r'^([a-z]{3})-([a-z_]{4})-(.+?)-(\d{3})$'
    
    def __init__(self, root_dir: str, dry_run: bool = False, backup_dir: Optional[str] = None):
        """
        Initialize the migrator.
        
        Args:
            root_dir: Root directory of the EA repository
            dry_run: If True, preview changes without modifying files
            backup_dir: Custom backup directory path
        """
        self.root_dir = Path(root_dir)
        self.elements_dir = self.root_dir / 'elements'
        self.dry_run = dry_run
        self.id_mapping: Dict[str, str] = {}  # old_id -> new_id
        self.file_mapping: Dict[str, Path] = {}  # id -> file_path
        self.errors: List[str] = []
        
        # Setup backup directory
        if backup_dir:
            self.backup_dir = Path(backup_dir)
        else:
            timestamp = datetime.now().strftime('%Y%m%d_%H%M%S')
            self.backup_dir = self.root_dir / 'backups' / f'migration_{timestamp}'
    
    def parse_old_id(self, old_id: str) -> Optional[Tuple[str, str, str, str]]:
        """
        Parse an ID in the old format.
        
        Args:
            old_id: ID in old format (e.g., 'app-comp-customer-portal-001')
        
        Returns:
            Tuple of (layer_code, type_code, descriptive_name, number) or None if invalid
        """
        match = re.match(self.OLD_FORMAT_PATTERN, old_id)
        if match:
            return match.groups()
        return None
    
    def convert_id(self, old_id: str) -> Optional[str]:
        """
        Convert an ID from old format to new format.
        
        Args:
            old_id: ID in old format (e.g., 'app-comp-customer-portal-001')
        
        Returns:
            ID in new format (e.g., 'app-comp-001-customer-portal') or None if invalid
        """
        parsed = self.parse_old_id(old_id)
        if not parsed:
            return None
        
        layer_code, type_code, descriptive_name, number = parsed
        new_id = f"{layer_code}-{type_code}-{number}-{descriptive_name}"
        return new_id
    
    def scan_element_files(self) -> List[Path]:
        """
        Scan all element markdown files in the elements directory.
        
        Returns:
            List of Path objects for all .md files found
        """
        element_files = []
        for root, dirs, files in os.walk(self.elements_dir):
            for file in files:
                if file.endswith('.md'):
                    element_files.append(Path(root) / file)
        return element_files
    
    def read_frontmatter(self, file_path: Path) -> Tuple[Optional[Dict], str]:
        """
        Read YAML frontmatter from a markdown file.
        
        Args:
            file_path: Path to the markdown file
        
        Returns:
            Tuple of (frontmatter_dict, remaining_content)
        """
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Check if file has frontmatter
        if not content.startswith('---'):
            return None, content
        
        # Extract frontmatter
        parts = content.split('---', 2)
        if len(parts) < 3:
            return None, content
        
        try:
            frontmatter = yaml.safe_load(parts[1])
            remaining_content = parts[2]
            return frontmatter, remaining_content
        except yaml.YAMLError as e:
            self.errors.append(f"Error parsing frontmatter in {file_path}: {e}")
            return None, content
    
    def write_frontmatter(self, file_path: Path, frontmatter: Dict, content: str):
        """
        Write YAML frontmatter and content to a markdown file.
        
        Args:
            file_path: Path to the markdown file
            frontmatter: Dictionary of frontmatter data
            content: Remaining markdown content
        """
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write('---\n')
            yaml.dump(frontmatter, f, default_flow_style=False, sort_keys=False, allow_unicode=True)
            f.write('---')
            f.write(content)
    
    def build_id_mapping(self, element_files: List[Path]):
        """
        Build a mapping of old IDs to new IDs by scanning all element files.
        
        Args:
            element_files: List of element file paths
        """
        print(f"\n📋 Scanning {len(element_files)} element files...")
        
        for file_path in element_files:
            frontmatter, _ = self.read_frontmatter(file_path)
            
            if not frontmatter or 'id' not in frontmatter:
                self.errors.append(f"Missing ID in {file_path}")
                continue
            
            old_id = frontmatter['id']
            
            # Check if ID is in old format
            if self.parse_old_id(old_id):
                new_id = self.convert_id(old_id)
                if new_id:
                    self.id_mapping[old_id] = new_id
                    self.file_mapping[old_id] = file_path
                    print(f"  ✓ {old_id} → {new_id}")
                else:
                    self.errors.append(f"Failed to convert ID {old_id} in {file_path}")
            else:
                # ID might already be in new format or invalid format
                self.file_mapping[old_id] = file_path
        
        print(f"\n✅ Found {len(self.id_mapping)} IDs to migrate")
    
    def create_backup(self):
        """Create a backup of the elements directory."""
        if self.dry_run:
            print(f"\n[DRY RUN] Would create backup at: {self.backup_dir}")
            return
        
        print(f"\n💾 Creating backup at: {self.backup_dir}")
        self.backup_dir.mkdir(parents=True, exist_ok=True)
        
        backup_elements = self.backup_dir / 'elements'
        shutil.copytree(self.elements_dir, backup_elements)
        
        # Save mapping file
        mapping_file = self.backup_dir / 'id_mapping.yaml'
        with open(mapping_file, 'w', encoding='utf-8') as f:
            yaml.dump(self.id_mapping, f, default_flow_style=False)
        
        print("✅ Backup created successfully")
    
    def update_element_id(self, file_path: Path, old_id: str, new_id: str):
        """
        Update an element's ID in its file.
        
        Args:
            file_path: Path to the element file
            old_id: Old ID to replace
            new_id: New ID to use
        """
        frontmatter, content = self.read_frontmatter(file_path)
        
        if not frontmatter:
            self.errors.append(f"Cannot update ID in {file_path} - no frontmatter")
            return
        
        # Update ID
        frontmatter['id'] = new_id
        
        # Add legacy-id to properties if not already present
        if 'properties' not in frontmatter:
            frontmatter['properties'] = {}
        
        if 'legacy-id' not in frontmatter['properties']:
            frontmatter['properties']['legacy-id'] = old_id
        
        if self.dry_run:
            print(f"  [DRY RUN] Would update {file_path}")
        else:
            self.write_frontmatter(file_path, frontmatter, content)
    
    def update_relationships(self, file_path: Path):
        """
        Update all relationship references in a file to use new IDs.
        
        Args:
            file_path: Path to the element file
        """
        frontmatter, content = self.read_frontmatter(file_path)
        
        if not frontmatter or 'relationships' not in frontmatter:
            return
        
        updated = False
        relationships = frontmatter['relationships']
        
        for rel in relationships:
            if 'target' in rel:
                old_target = rel['target']
                if old_target in self.id_mapping:
                    new_target = self.id_mapping[old_target]
                    rel['target'] = new_target
                    updated = True
        
        if updated:
            if self.dry_run:
                print(f"  [DRY RUN] Would update relationships in {file_path}")
            else:
                self.write_frontmatter(file_path, frontmatter, content)
    
    def migrate(self):
        """Execute the full migration process."""
        print("=" * 70)
        print("🔄 ArchiMate ID Migration Script")
        print("=" * 70)
        print(f"Root directory: {self.root_dir}")
        print(f"Mode: {'DRY RUN' if self.dry_run else 'LIVE'}")
        print("=" * 70)
        
        # Step 1: Scan files
        element_files = self.scan_element_files()
        if not element_files:
            print("❌ No element files found!")
            return
        
        # Step 2: Build ID mapping
        self.build_id_mapping(element_files)
        
        if not self.id_mapping:
            print("\n✅ No IDs need migration (all IDs are already in new format)")
            return
        
        # Step 3: Create backup
        self.create_backup()
        
        # Step 4: Update element IDs
        print(f"\n📝 Updating element IDs...")
        for old_id, new_id in self.id_mapping.items():
            file_path = self.file_mapping[old_id]
            self.update_element_id(file_path, old_id, new_id)
        
        # Step 5: Update relationship references
        print(f"\n🔗 Updating relationship references...")
        for file_path in element_files:
            self.update_relationships(file_path)
        
        # Step 6: Report results
        print("\n" + "=" * 70)
        print("📊 Migration Summary")
        print("=" * 70)
        print(f"Total files scanned: {len(element_files)}")
        print(f"IDs migrated: {len(self.id_mapping)}")
        print(f"Errors encountered: {len(self.errors)}")
        
        if self.errors:
            print("\n⚠️  Errors:")
            for error in self.errors:
                print(f"  - {error}")
        
        if self.dry_run:
            print("\n🔍 This was a DRY RUN - no files were modified")
            print("Run without --dry-run to apply changes")
        else:
            print(f"\n✅ Migration complete!")
            print(f"Backup saved to: {self.backup_dir}")
        
        print("=" * 70)


def main():
    """Main entry point for the migration script."""
    parser = argparse.ArgumentParser(
        description='Migrate ArchiMate element IDs from old format to new format',
        formatter_class=argparse.RawDescriptionHelpFormatter
    )
    parser.add_argument(
        '--dry-run',
        action='store_true',
        help='Preview changes without modifying files'
    )
    parser.add_argument(
        '--backup-dir',
        type=str,
        help='Custom backup directory path'
    )
    parser.add_argument(
        '--root-dir',
        type=str,
        default='.',
        help='Root directory of the EA repository (default: current directory)'
    )
    
    args = parser.parse_args()
    
    # Get absolute path to root directory
    root_dir = os.path.abspath(args.root_dir)
    
    # Verify elements directory exists
    elements_dir = Path(root_dir) / 'elements'
    if not elements_dir.exists():
        print(f"❌ Error: elements directory not found at {elements_dir}")
        print(f"Please run this script from the EA repository root or use --root-dir")
        return 1
    
    # Create and run migrator
    migrator = IDMigrator(
        root_dir=root_dir,
        dry_run=args.dry_run,
        backup_dir=args.backup_dir
    )
    
    migrator.migrate()
    
    return 0 if not migrator.errors else 1


if __name__ == '__main__':
    exit(main())
