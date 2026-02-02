#!/usr/bin/env python3
"""
Generate a proper ArchiMate 3.1 Model Exchange File (XML) from markdown elements.

This script reads all markdown elements from the elements/ directory and generates
a valid ArchiMate exchange file conforming to the Open Group XSD schema.

Usage:
    python generate_archimate_exchange.py [output_path]

Output:
    - If output_path is provided: saves to that path
    - Otherwise: saves to output/model-exchange.archimate
"""

import os
import glob
import json
import re
from datetime import datetime
from pathlib import Path
from xml.etree import ElementTree as ET
from xml.dom import minidom
import frontmatter


# Map markdown type names to ArchiMate XSD types
TYPE_MAPPING = {
    # Strategy layer
    'resource': 'Resource',
    'capability': 'Capability',
    'value-stream': 'ValueStream',
    'course-of-action': 'CourseOfAction',
    
    # Business layer
    'business-actor': 'BusinessActor',
    'business-role': 'BusinessRole',
    'business-collaboration': 'BusinessCollaboration',
    'business-interface': 'BusinessInterface',
    'business-process': 'BusinessProcess',
    'business-function': 'BusinessFunction',
    'business-interaction': 'BusinessInteraction',
    'business-event': 'BusinessEvent',
    'business-service': 'BusinessService',
    'business-object': 'BusinessObject',
    'contract': 'Contract',
    'representation': 'Representation',
    'product': 'Product',
    
    # Application layer
    'application-component': 'ApplicationComponent',
    'application-collaboration': 'ApplicationCollaboration',
    'application-interface': 'ApplicationInterface',
    'application-function': 'ApplicationFunction',
    'application-interaction': 'ApplicationInteraction',
    'application-process': 'ApplicationProcess',
    'application-event': 'ApplicationEvent',
    'application-service': 'ApplicationService',
    'data-object': 'DataObject',
    
    # Technology layer
    'node': 'Node',
    'device': 'Device',
    'system-software': 'SystemSoftware',
    'technology-collaboration': 'TechnologyCollaboration',
    'technology-interface': 'TechnologyInterface',
    'path': 'Path',
    'communication-network': 'CommunicationNetwork',
    'technology-function': 'TechnologyFunction',
    'technology-process': 'TechnologyProcess',
    'technology-interaction': 'TechnologyInteraction',
    'technology-event': 'TechnologyEvent',
    'technology-service': 'TechnologyService',
    'artifact': 'Artifact',
    
    # Physical layer
    'equipment': 'Equipment',
    'facility': 'Facility',
    'distribution-network': 'DistributionNetwork',
    'material': 'Material',
    
    # Motivation layer
    'stakeholder': 'Stakeholder',
    'driver': 'Driver',
    'assessment': 'Assessment',
    'goal': 'Goal',
    'outcome': 'Outcome',
    'principle': 'Principle',
    'requirement': 'Requirement',
    'constraint': 'Constraint',
    'meaning': 'Meaning',
    'value': 'Value',
    
    # Implementation layer
    'work-package': 'WorkPackage',
    'deliverable': 'Deliverable',
    'implementation-event': 'ImplementationEvent',
    'plateau': 'Plateau',
    'gap': 'Gap',
}

# Map relationship type names to ArchiMate XSD types
RELATIONSHIP_TYPE_MAPPING = {
    'composition': 'Composition',
    'aggregation': 'Aggregation',
    'assignment': 'Assignment',
    'realization': 'Realization',
    'serving': 'Serving',
    'access': 'Access',
    'influence': 'Influence',
    'triggering': 'Triggering',
    'flow': 'Flow',
    'specialization': 'Specialization',
    'association': 'Association',
    # Handle variations
    'influences': 'Influence',
    'supports': 'Serving',
}


class ArchiMateExchangeGenerator:
    """Generator for ArchiMate Model Exchange Files."""
    
    def __init__(self, elements_dir='elements', output_file=None):
        self.elements_dir = Path(elements_dir)
        self.output_file = output_file or Path('output/model-exchange.archimate')
        self.elements = {}
        self.relationships = []
        self.namespace = 'http://www.opengroup.org/xsd/archimate/3.0/'
        self.schema_location = 'http://www.opengroup.org/xsd/archimate/3.1/archimate3_Model.xsd'
        self.property_defs = {}  # Track property definitions
        self.property_counter = 0
        
    def load_elements(self):
        """Load all markdown elements from the elements directory."""
        md_files = sorted(self.elements_dir.glob('**/*.md'))
        print(f"Found {len(md_files)} markdown files")
        
        for md_file in md_files:
            try:
                with open(md_file, 'r', encoding='utf-8') as f:
                    post = frontmatter.load(f)
                    
                # Extract metadata from frontmatter
                metadata = post.metadata
                if not all(k in metadata for k in ['id', 'name', 'type', 'layer']):
                    print(f"  ⚠ Skipping {md_file.name}: missing required fields")
                    continue
                
                element_id = metadata['id']
                self.elements[element_id] = {
                    'id': element_id,
                    'name': metadata['name'],
                    'type': metadata['type'],
                    'layer': metadata['layer'],
                    'documentation': post.content,
                    'properties': metadata.get('properties', {}),
                    'relationships': metadata.get('relationships', []),
                    'tags': metadata.get('tags', []),
                }
                print(f"  ✓ Loaded {element_id}: {metadata['name']}")
                
            except Exception as e:
                print(f"  ✗ Error loading {md_file}: {e}")
        
        print(f"\nLoaded {len(self.elements)} elements\n")
        
        # Extract relationships
        self._extract_relationships()
    
    def _extract_relationships(self):
        """Extract relationships from element metadata."""
        for element_id, element in self.elements.items():
            for rel in element.get('relationships', []):
                if isinstance(rel, dict):
                    rel_type = rel.get('type', 'association')
                    target_id = rel.get('target')
                    
                    if target_id and target_id in self.elements:
                        # Normalize relationship type
                        archimate_type = RELATIONSHIP_TYPE_MAPPING.get(
                            rel_type.lower(), 'Association'
                        )
                        
                        self.relationships.append({
                            'id': f"rel_{len(self.relationships):04d}",
                            'source': element_id,
                            'target': target_id,
                            'type': archimate_type,
                            'description': rel.get('description', ''),
                        })
    
    def generate_xml(self):
        """Generate the ArchiMate Model Exchange XML."""
        # First pass: collect all property definitions
        self._collect_property_definitions()
        
        # Create root model element
        model = ET.Element('model', {
            'xmlns': self.namespace,
            'xmlns:xsi': 'http://www.w3.org/2001/XMLSchema-instance',
            'xsi:schemaLocation': f'{self.namespace} {self.schema_location}',
            'identifier': 'model-1',
            'version': '1.0',
        })
        
        # Add name
        name_elem = ET.SubElement(model, 'name')
        name_elem.text = 'Enterprise Architecture Model'
        
        # Add documentation
        doc_elem = ET.SubElement(model, 'documentation')
        doc_elem.text = 'Enterprise Architecture Model generated from markdown elements'
        
        # Add metadata
        metadata = ET.SubElement(model, 'metadata')
        schema_info = ET.SubElement(metadata, 'schemaInfo')
        schema_elem = ET.SubElement(schema_info, 'schema')
        schema_elem.text = self.namespace
        schema_version = ET.SubElement(schema_info, 'schemaversion')
        schema_version.text = '3.1'
        
        # Add elements
        elements_container = ET.SubElement(model, 'elements')
        for element_id, element in self.elements.items():
            self._add_element_to_xml(elements_container, element)
        
        # Add relationships
        if self.relationships:
            relationships_container = ET.SubElement(model, 'relationships')
            for rel in self.relationships:
                self._add_relationship_to_xml(relationships_container, rel)
        
        # Add property definitions at the end
        if self.property_defs:
            self._add_property_definitions_to_xml(model)
        
        return model
    
    def _collect_property_definitions(self):
        """Collect all unique properties used across all elements."""
        # Standard properties we'll always define
        standard_props = {
            'owner': 'string',
            'status': 'string',
            'criticality': 'string',
            'cost': 'currency',
            'complexity': 'string',
            'lifecycle-phase': 'string',
            'version': 'string',
            'last-updated': 'date',
            'source': 'string',
            'urgency': 'string',
            'trend': 'string',
            'maturity-level': 'string',
        }
        
        # Add custom properties from elements
        for element in self.elements.values():
            for prop_key, prop_value in element.get('properties', {}).items():
                if prop_key not in standard_props:
                    standard_props[prop_key] = 'string'
        
        # Create property definitions (normalize keys)
        seen_ids = set()
        for prop_name, prop_type in sorted(standard_props.items()):
            # Normalize to use hyphens consistently
            normalized_key = prop_name.lower().replace('_', '-')
            prop_id = f"propdef-{normalized_key.replace('-', '-')}"
            
            if prop_id not in seen_ids:
                self.property_defs[normalized_key] = {
                    'id': prop_id,
                    'type': prop_type,
                }
                seen_ids.add(prop_id)
    
    def _add_property_definitions_to_xml(self, model):
        """Add propertyDefinitions section to model."""
        if not self.property_defs:
            return
        
        prop_defs_elem = ET.SubElement(model, 'propertyDefinitions')
        
        for prop_name, prop_info in sorted(self.property_defs.items()):
            prop_def = ET.SubElement(prop_defs_elem, 'propertyDefinition', {
                'identifier': prop_info['id'],
                'type': prop_info['type'],
            })
            
            name_elem = ET.SubElement(prop_def, 'name')
            name_elem.text = prop_name

    def _add_element_to_xml(self, parent, element):
        """Add an individual element to the XML tree."""
        element_type = TYPE_MAPPING.get(element['type'], 'BusinessObject')
        
        elem = ET.SubElement(parent, 'element', {
            'identifier': element['id'],
            'xsi:type': element_type,
        })
        
        # Add name with language attribute
        name_elem = ET.SubElement(elem, 'name', {'xml:lang': 'en'})
        name_elem.text = element['name']
        
        # Add documentation if available
        if element.get('documentation'):
            doc_elem = ET.SubElement(elem, 'documentation')
            # Extract first paragraph as documentation
            doc_text = element['documentation'].split('\n')[0]
            doc_elem.text = doc_text[:500] if doc_text else element['name']
        
        # Add properties if available
        if element.get('properties'):
            self._add_properties_to_xml(elem, element['properties'])
    
    def _add_properties_to_xml(self, parent, properties):
        """Add properties to an element."""
        if not properties:
            return
        
        props_elem = ET.SubElement(parent, 'properties')
        
        for key, value in properties.items():
            # Skip empty values
            value_str = str(value) if value else ''
            if not value_str:
                continue
            
            # Normalize property key (use hyphens)
            prop_key_normalized = key.lower().replace('_', '-').replace(' ', '-')
            prop_id = f"propdef-{prop_key_normalized}"
            
            prop_elem = ET.SubElement(props_elem, 'property', {
                'propertyDefinitionRef': prop_id
            })
            
            val_elem = ET.SubElement(prop_elem, 'value')
            val_elem.text = value_str
    
    def _add_relationship_to_xml(self, parent, relationship):
        """Add a relationship to the XML tree."""
        rel_type = relationship['type']
        
        rel = ET.SubElement(parent, 'relationship', {
            'identifier': relationship['id'],
            'source': relationship['source'],
            'target': relationship['target'],
            'xsi:type': rel_type,
        })
        
        # Add optional description as name with language attribute
        if relationship.get('description'):
            name_elem = ET.SubElement(rel, 'name', {'xml:lang': 'en'})
            name_elem.text = relationship['description']
    
    def prettify_xml(self, elem):
        """Return a pretty-printed XML string."""
        rough_string = ET.tostring(elem, encoding='unicode')
        reparsed = minidom.parseString(rough_string)
        return reparsed.toprettyxml(indent="  ")
    
    def save(self):
        """Generate and save the ArchiMate exchange file."""
        print("Generating ArchiMate Model Exchange XML...")
        
        model = self.generate_xml()
        xml_str = self.prettify_xml(model)
        
        # Clean up the XML string
        xml_str = '\n'.join(line for line in xml_str.split('\n') 
                           if line.strip() or line.startswith('<?xml'))
        
        # Ensure output directory exists
        self.output_file.parent.mkdir(parents=True, exist_ok=True)
        
        # Save to file
        with open(self.output_file, 'w', encoding='utf-8') as f:
            f.write(xml_str)
        
        print(f"\n✓ Generated ArchiMate exchange file")
        print(f"  File: {self.output_file}")
        print(f"  Elements: {len(self.elements)}")
        print(f"  Relationships: {len(self.relationships)}")
        print(f"  Size: {self.output_file.stat().st_size:,} bytes")
        
        return self.output_file
    
    def generate_summary(self):
        """Print a summary of the generated model."""
        print("\n" + "="*60)
        print("ARCHIMATE MODEL SUMMARY")
        print("="*60)
        
        # Group elements by layer
        by_layer = {}
        for elem in self.elements.values():
            layer = elem['layer']
            if layer not in by_layer:
                by_layer[layer] = []
            by_layer[layer].append(elem)
        
        # Group elements by type
        by_type = {}
        for elem in self.elements.values():
            elem_type = elem['type']
            if elem_type not in by_type:
                by_type[elem_type] = []
            by_type[elem_type].append(elem)
        
        print("\nElements by Layer:")
        for layer in sorted(by_layer.keys()):
            print(f"  {layer}: {len(by_layer[layer])} elements")
        
        print("\nElements by Type:")
        for elem_type in sorted(by_type.keys()):
            print(f"  {elem_type}: {len(by_type[elem_type])}")
        
        print("\nRelationships by Type:")
        by_rel_type = {}
        for rel in self.relationships:
            rel_type = rel['type']
            if rel_type not in by_rel_type:
                by_rel_type[rel_type] = 0
            by_rel_type[rel_type] += 1
        
        for rel_type in sorted(by_rel_type.keys()):
            print(f"  {rel_type}: {by_rel_type[rel_type]}")
        
        print("\n" + "="*60)


def main():
    import sys
    
    elements_dir = Path(__file__).parent.parent / 'elements'
    output_file = Path(sys.argv[1]) if len(sys.argv) > 1 else None
    
    generator = ArchiMateExchangeGenerator(
        elements_dir=elements_dir,
        output_file=output_file,
    )
    
    generator.load_elements()
    generator.save()
    generator.generate_summary()


if __name__ == '__main__':
    main()
