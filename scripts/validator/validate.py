#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
ArchiMate 3.2 Markdown Validator
Validates EA element markdown files for ArchiMate 3.2 compliance
"""

import os
import sys
import yaml
import re
from pathlib import Path
from typing import Dict, List, Tuple

# Fix Windows console encoding issues
if sys.platform == 'win32':
    import codecs
    sys.stdout = codecs.getwriter('utf-8')(sys.stdout.buffer, 'strict')
    sys.stderr = codecs.getwriter('utf-8')(sys.stderr.buffer, 'strict')

class ArchiMateValidator:
    def __init__(self, schema_path: str, elements_dir: str = None):
        """Initialize validator with schema file"""
        with open(schema_path, 'r', encoding='utf-8') as f:
            self.schema = yaml.safe_load(f)
        
        self.errors = []
        self.warnings = []
        self.elements_dir = elements_dir
        self._all_element_ids = None
    
    def validate_file(self, file_path: str) -> Tuple[bool, List[str], List[str]]:
        """Validate a single markdown file"""
        self.errors = []
        self.warnings = []
        
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Extract YAML frontmatter
            frontmatter, markdown = self._extract_frontmatter(content)
            
            if not frontmatter:
                self.errors.append("Missing YAML frontmatter")
                return False, self.errors, self.warnings
            
            # Validate required fields
            self._validate_required_fields(frontmatter)
            
            # Validate element type
            self._validate_element_type(frontmatter)
            
            # Validate relationships
            self._validate_relationships(frontmatter)
            
            # Check for missing relationship targets
            if self.elements_dir:
                self._check_relationship_targets(frontmatter)
            
            # Check markdown content
            self._validate_markdown(markdown)
            
            # Additional warnings
            self._check_optional_fields(frontmatter)
            
        except Exception as e:
            self.errors.append(f"Error reading file: {str(e)}")
            return False, self.errors, self.warnings
        
        return len(self.errors) == 0, self.errors, self.warnings
    
    def _extract_frontmatter(self, content: str) -> Tuple[Dict, str]:
        """Extract YAML frontmatter from markdown content"""
        pattern = r'^---\s*\n(.*?)\n---\s*\n(.*)$'
        match = re.match(pattern, content, re.DOTALL)
        
        if not match:
            return None, content
        
        try:
            frontmatter = yaml.safe_load(match.group(1))
            markdown = match.group(2)
            return frontmatter, markdown
        except yaml.YAMLError as e:
            self.errors.append(f"Invalid YAML: {str(e)}")
            return None, ""
    
    def _validate_required_fields(self, frontmatter: Dict):
        """Check all required fields are present"""
        required = self.schema['required_fields']
        
        for field in required:
            if field not in frontmatter:
                self.errors.append(f"Missing required field: {field}")
    
    def _validate_element_type(self, frontmatter: Dict):
        """Validate element type belongs to correct layer"""
        if 'type' not in frontmatter or 'layer' not in frontmatter:
            return
        
        element_type = frontmatter['type']
        layer = frontmatter['layer']
        
        # Get valid types for this layer
        layer_types = self.schema['element_types'].get(layer, [])
        
        if not layer_types:
            self.errors.append(f"Invalid layer: {layer}")
            return
        
        if element_type not in layer_types:
            self.errors.append(
                f"Element type '{element_type}' is not valid for layer '{layer}'. "
                f"Valid types: {', '.join(layer_types)}"
            )
    
    def _validate_relationships(self, frontmatter: Dict):
        """Validate relationship types"""
        if 'relationships' not in frontmatter:
            return
        
        relationships = frontmatter['relationships']
        if not isinstance(relationships, list):
            self.errors.append("Relationships must be a list")
            return
        
        # Collect all valid relationship types
        valid_types = []
        for category in self.schema['relationships'].values():
            valid_types.extend(category)
        
        for i, rel in enumerate(relationships):
            if not isinstance(rel, dict):
                self.errors.append(f"Relationship {i+1} must be an object")
                continue
            
            if 'type' not in rel:
                self.errors.append(f"Relationship {i+1} missing 'type' field")
            elif rel['type'] not in valid_types:
                self.errors.append(
                    f"Invalid relationship type: {rel['type']}. "
                    f"Valid types: {', '.join(valid_types)}"
                )
            
            if 'target' not in rel:
                self.errors.append(f"Relationship {i+1} missing 'target' field")
    
    def _validate_markdown(self, markdown: str):
        """Validate markdown content"""
        if not markdown or not markdown.strip():
            self.warnings.append("No markdown description provided")
        
        # Check for heading
        if not re.search(r'^#\s+', markdown, re.MULTILINE):
            self.warnings.append("Consider adding a heading to the description")
    
    def _check_optional_fields(self, frontmatter: Dict):
        """Check for recommended optional fields"""
        optional = self.schema['optional_fields']
        missing = []
        
        for field in optional:
            if field == 'documentation':
                continue  # This is the markdown content
            if field not in frontmatter:
                missing.append(field)
        
        if missing and 'properties' in missing:
            self.warnings.append(
                f"Consider adding optional fields: {', '.join(missing)}"
            )
    
    def _load_all_element_ids(self):
        """Load all element IDs from the elements directory (cached)"""
        if self._all_element_ids is not None:
            return self._all_element_ids
        
        if not self.elements_dir or not os.path.exists(self.elements_dir):
            self._all_element_ids = set()
            return self._all_element_ids
        
        all_ids = set()
        for root, dirs, files in os.walk(self.elements_dir):
            for file in files:
                if file.endswith('.md'):
                    file_path = os.path.join(root, file)
                    try:
                        with open(file_path, 'r', encoding='utf-8') as f:
                            content = f.read()
                        frontmatter, _ = self._extract_frontmatter(content)
                        if frontmatter and 'id' in frontmatter:
                            all_ids.add(frontmatter['id'])
                    except:
                        pass  # Ignore errors in other files
        
        self._all_element_ids = all_ids
        return all_ids
    
    def _check_relationship_targets(self, frontmatter: Dict):
        """Check if relationship targets exist"""
        if 'relationships' not in frontmatter:
            return
        
        all_ids = self._load_all_element_ids()
        if not all_ids:
            return
        
        missing_targets = []
        for rel in frontmatter.get('relationships', []):
            if isinstance(rel, dict) and 'target' in rel:
                target = rel['target']
                if target not in all_ids:
                    missing_targets.append(target)
        
        if missing_targets:
            self.warnings.append(
                f"Relationship(s) reference missing elements: {', '.join(sorted(set(missing_targets)))}"
            )
    
    def validate_directory(self, directory: str) -> Dict[str, Tuple[bool, List, List]]:
        """Validate all markdown files in a directory"""
        results = {}
        
        # Validate all files
        for root, dirs, files in os.walk(directory):
            for file in files:
                if file.endswith('.md'):
                    file_path = os.path.join(root, file)
                    results[file_path] = self.validate_file(file_path)
        
        return results


def main():
    """Main validation function"""
    # Determine paths
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent.parent  # Go up two levels: validator -> scripts -> workspace
    schema_path = workspace_dir / 'schemas' / 'archimate-3.2-schema.yaml'
    elements_dir = workspace_dir / 'elements'
    
    if not schema_path.exists():
        print(f"❌ Schema file not found: {schema_path}")
        return 1
    
    validator = ArchiMateValidator(str(schema_path), str(elements_dir))
    
    # Check if specific file provided
    if len(sys.argv) > 1:
        file_path = sys.argv[1]
        if not os.path.exists(file_path):
            print(f"❌ File not found: {file_path}")
            return 1
        
        valid, errors, warnings = validator.validate_file(file_path)
        
        print(f"\n📄 Validating: {file_path}")
        
        if errors:
            print("\n❌ Errors:")
            for error in errors:
                print(f"  • {error}")
        
        if warnings:
            print("\n⚠️  Warnings:")
            for warning in warnings:
                print(f"  • {warning}")
        
        if valid:
            print("\n✅ File is valid!")
            return 0
        else:
            return 1
    
    # Validate entire directory
    if not elements_dir.exists():
        print(f"❌ Elements directory not found: {elements_dir}")
        return 1
    
    print(f"🔍 Validating all elements in: {elements_dir}\n")
    
    results = validator.validate_directory(str(elements_dir))
    
    if not results:
        print("⚠️  No markdown files found")
        return 0
    
    valid_no_warnings = 0
    valid_with_warnings = 0
    invalid_count = 0
    
    # First, print files with no warnings
    print("✅ Valid (no warnings):")
    has_valid_no_warnings = False
    for file_path, (valid, errors, warnings) in results.items():
        rel_path = os.path.relpath(file_path, workspace_dir)
        if valid and not warnings:
            print(f"  • {rel_path}")
            valid_no_warnings += 1
            has_valid_no_warnings = True
    
    if not has_valid_no_warnings:
        print("  (none)")
    
    # Second, print files with warnings
    print("\n⚠️  Valid with warnings:")
    has_valid_with_warnings = False
    for file_path, (valid, errors, warnings) in results.items():
        rel_path = os.path.relpath(file_path, workspace_dir)
        if valid and warnings:
            print(f"  • {rel_path}")
            for warning in warnings:
                print(f"      ⚠️  {warning}")
            valid_with_warnings += 1
            has_valid_with_warnings = True
    
    if not has_valid_with_warnings:
        print("  (none)")
    
    # Third, print invalid files
    print("\n❌ Invalid:")
    has_invalid = False
    for file_path, (valid, errors, warnings) in results.items():
        rel_path = os.path.relpath(file_path, workspace_dir)
        if not valid:
            print(f"  • {rel_path}")
            for error in errors:
                print(f"      • {error}")
            invalid_count += 1
            has_invalid = True
    
    if not has_invalid:
        print("  (none)")
    
    print(f"\n{'='*60}")
    total_valid = valid_no_warnings + valid_with_warnings
    print(f"Total: {len(results)} files")
    print(f"  ✅ Valid: {total_valid} ({valid_no_warnings} clean, {valid_with_warnings} with warnings)")
    print(f"  ❌ Invalid: {invalid_count}")
    print(f"{'='*60}\n")
    
    return 0 if invalid_count == 0 else 1


if __name__ == '__main__':
    sys.exit(main())
