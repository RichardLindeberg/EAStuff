#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
ArchiMate to PlantUML Generator
Generates PlantUML diagrams from markdown EA elements
"""

import os
import sys
import yaml
import re
from pathlib import Path
from typing import Dict, List, Set
from collections import defaultdict

# Fix Windows console encoding issues
if sys.platform == 'win32':
    import codecs
    sys.stdout = codecs.getwriter('utf-8')(sys.stdout.buffer, 'strict')
    sys.stderr = codecs.getwriter('utf-8')(sys.stderr.buffer, 'strict')

class PlantUMLGenerator:
    def __init__(self, elements_dir: str):
        """Initialize generator with elements directory"""
        self.elements_dir = Path(elements_dir)
        self.elements = {}
        self.relationships = []
        
        # ArchiMate element to PlantUML mapping
        self.element_to_puml = {
            # Strategy
            'resource': 'Archimate_Resource',
            'capability': 'Archimate_Capability',
            'value-stream': 'Archimate_ValueStream',
            'course-of-action': 'Archimate_CourseOfAction',
            
            # Business
            'business-actor': 'Archimate_BusinessActor',
            'business-role': 'Archimate_BusinessRole',
            'business-collaboration': 'Archimate_BusinessCollaboration',
            'business-interface': 'Archimate_BusinessInterface',
            'business-process': 'Archimate_BusinessProcess',
            'business-function': 'Archimate_BusinessFunction',
            'business-interaction': 'Archimate_BusinessInteraction',
            'business-event': 'Archimate_BusinessEvent',
            'business-service': 'Archimate_BusinessService',
            'business-object': 'Archimate_BusinessObject',
            'contract': 'Archimate_Contract',
            'representation': 'Archimate_Representation',
            'product': 'Archimate_Product',
            
            # Application
            'application-component': 'Archimate_ApplicationComponent',
            'application-collaboration': 'Archimate_ApplicationCollaboration',
            'application-interface': 'Archimate_ApplicationInterface',
            'application-function': 'Archimate_ApplicationFunction',
            'application-interaction': 'Archimate_ApplicationInteraction',
            'application-process': 'Archimate_ApplicationProcess',
            'application-event': 'Archimate_ApplicationEvent',
            'application-service': 'Archimate_ApplicationService',
            'data-object': 'Archimate_DataObject',
            
            # Technology
            'node': 'Archimate_Node',
            'device': 'Archimate_Device',
            'system-software': 'Archimate_SystemSoftware',
            'technology-collaboration': 'Archimate_TechnologyCollaboration',
            'technology-interface': 'Archimate_TechnologyInterface',
            'path': 'Archimate_Path',
            'communication-network': 'Archimate_CommunicationNetwork',
            'technology-function': 'Archimate_TechnologyFunction',
            'technology-process': 'Archimate_TechnologyProcess',
            'technology-interaction': 'Archimate_TechnologyInteraction',
            'technology-event': 'Archimate_TechnologyEvent',
            'technology-service': 'Archimate_TechnologyService',
            'artifact': 'Archimate_Artifact',
            
            # Physical
            'equipment': 'Archimate_Equipment',
            'facility': 'Archimate_Facility',
            'distribution-network': 'Archimate_DistributionNetwork',
            'material': 'Archimate_Material',
            
            # Motivation
            'stakeholder': 'Archimate_Stakeholder',
            'driver': 'Archimate_Driver',
            'assessment': 'Archimate_Assessment',
            'goal': 'Archimate_Goal',
            'outcome': 'Archimate_Outcome',
            'principle': 'Archimate_Principle',
            'requirement': 'Archimate_Requirement',
            'constraint': 'Archimate_Constraint',
            'meaning': 'Archimate_Meaning',
            'value': 'Archimate_Value',
            
            # Implementation
            'work-package': 'Archimate_WorkPackage',
            'deliverable': 'Archimate_Deliverable',
            'implementation-event': 'Archimate_ImplementationEvent',
            'plateau': 'Archimate_Plateau',
            'gap': 'Archimate_Gap',
        }
        
        # Relationship type mapping
        self.relationship_style = {
            'composition': 'Composition',
            'aggregation': 'Aggregation',
            'assignment': 'Assignment',
            'realization': 'Realization',
            'serving': 'Serving',
            'access': 'Access',
            'influence': 'Influence',
            'association': 'Association',
            'triggering': 'Triggering',
            'flow': 'Flow',
            'specialization': 'Specialization',
        }
    
    def load_elements(self):
        """Load all markdown elements"""
        for root, dirs, files in os.walk(self.elements_dir):
            for file in files:
                if file.endswith('.md'):
                    file_path = os.path.join(root, file)
                    element = self._parse_element(file_path)
                    if element:
                        self.elements[element['id']] = element
                        
                        # Extract relationships
                        if 'relationships' in element:
                            for rel in element['relationships']:
                                self.relationships.append({
                                    'source': element['id'],
                                    'target': rel.get('target'),
                                    'type': rel.get('type'),
                                    'description': rel.get('description', '')
                                })
    
    def _parse_element(self, file_path: str) -> Dict:
        """Parse a single markdown file"""
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Extract YAML frontmatter
            pattern = r'^---\s*\n(.*?)\n---\s*\n(.*)$'
            match = re.match(pattern, content, re.DOTALL)
            
            if not match:
                return None
            
            frontmatter = yaml.safe_load(match.group(1))
            return frontmatter
            
        except Exception as e:
            print(f"Warning: Could not parse {file_path}: {e}")
            return None
    
    def generate_full_diagram(self, output_file: str, title: str = "Enterprise Architecture"):
        """Generate a complete architecture diagram"""
        self.load_elements()
        
        puml = []
        puml.append("@startuml")
        puml.append("")
        puml.append(f"title {title}")
        puml.append("")
        puml.append("' Styling")
        puml.append("skinparam componentStyle rectangle")
        puml.append("skinparam backgroundColor white")
        puml.append("skinparam shadowing false")
        puml.append("")
        puml.append("' Define elements")
        
        # Group elements by layer
        by_layer = defaultdict(list)
        for elem_id, elem in self.elements.items():
            layer = elem.get('layer', 'other')
            by_layer[layer].append(elem)
        
        # Generate elements grouped by layer
        for layer in ['strategy', 'business', 'application', 'technology', 'physical', 'motivation', 'implementation']:
            if layer in by_layer:
                puml.append(f"\n' {layer.title()} Layer")
                for elem in by_layer[layer]:
                    puml.append(self._element_to_puml_string(elem))
        
        # Add other elements not in standard layers
        if 'other' in by_layer:
            puml.append("\n' Other Elements")
            for elem in by_layer['other']:
                puml.append(self._element_to_puml_string(elem))
        
        # Generate relationships
        puml.append("\n' Relationships")
        missing_targets = set()
        for rel in self.relationships:
            if rel['source'] in self.elements and rel['target'] in self.elements:
                puml.append(self._relationship_to_puml_string(rel))
            elif rel['target'] not in self.elements:
                missing_targets.add(rel['target'])
        
        # Warn about missing targets
        if missing_targets:
            print(f"⚠️  Warning: Some relationships reference missing elements: {', '.join(sorted(missing_targets))}")
        
        puml.append("\n@enduml")
        
        # Write to file
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write('\n'.join(puml))
        
        print(f"✅ Generated: {output_file}")
        return output_file
    
    def generate_layer_diagram(self, layer: str, output_file: str):
        """Generate a diagram for a specific layer"""
        self.load_elements()
        
        layer_elements = {eid: e for eid, e in self.elements.items() if e.get('layer') == layer}
        
        if not layer_elements:
            print(f"⚠️  No elements found for layer: {layer}")
            return None
        
        puml = []
        puml.append("@startuml")
        puml.append("")
        puml.append(f"title {layer.title()} Layer Architecture")
        puml.append("")
        puml.append("' Styling")
        puml.append("skinparam componentStyle rectangle")
        puml.append("skinparam backgroundColor white")
        puml.append("skinparam shadowing false")
        puml.append("")
        
        # Add elements
        for elem_id, elem in layer_elements.items():
            puml.append(self._element_to_puml_string(elem))
        
        # Add relationships (only within this layer or to/from this layer)
        layer_elem_ids = set(layer_elements.keys())
        puml.append("\n' Relationships")
        for rel in self.relationships:
            if rel['source'] in layer_elem_ids or rel['target'] in layer_elem_ids:
                if rel['source'] in self.elements and rel['target'] in self.elements:
                    puml.append(self._relationship_to_puml_string(rel))
        
        puml.append("\n@enduml")
        
        # Write to file
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write('\n'.join(puml))
        
        print(f"✅ Generated: {output_file}")
        return output_file
    
    def generate_element_context_diagram(self, element_id: str, output_file: str, depth: int = 1):
        """Generate a context diagram for a specific element"""
        self.load_elements()
        
        if element_id not in self.elements:
            print(f"❌ Element not found: {element_id}")
            return None
        
        # Find connected elements
        connected = self._find_connected_elements(element_id, depth)
        connected.add(element_id)
        
        puml = []
        puml.append("@startuml")
        puml.append("")
        elem = self.elements[element_id]
        puml.append(f"title Context: {elem.get('name', element_id)}")
        puml.append("")
        puml.append("' Styling")
        puml.append("skinparam componentStyle rectangle")
        puml.append("skinparam backgroundColor white")
        puml.append("skinparam shadowing false")
        puml.append("")
        
        # Add elements
        for eid in connected:
            if eid in self.elements:
                elem_data = self.elements[eid]
                
                # Highlight the main element
                if eid == element_id:
                    elem_id = self._sanitize_id(eid)
                    elem_name = elem_data.get('name', eid)
                    puml.append(f"component \"{elem_name}\" as {elem_id} #lightblue")
                else:
                    puml.append(self._element_to_puml_string(elem_data))
        
        # Add relationships
        puml.append("\n' Relationships")
        for rel in self.relationships:
            if rel['source'] in connected and rel['target'] in connected:
                puml.append(self._relationship_to_puml_string(rel))
        
        puml.append("\n@enduml")
        
        # Write to file
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write('\n'.join(puml))
        
        print(f"✅ Generated: {output_file}")
        return output_file
    
    def _find_connected_elements(self, element_id: str, depth: int) -> Set[str]:
        """Find elements connected to the given element up to specified depth"""
        connected = set()
        current_level = {element_id}
        
        for _ in range(depth):
            next_level = set()
            for eid in current_level:
                for rel in self.relationships:
                    if rel['source'] == eid:
                        next_level.add(rel['target'])
                    elif rel['target'] == eid:
                        next_level.add(rel['source'])
            
            connected.update(next_level)
            current_level = next_level
        
        return connected
    
    def _element_to_puml_string(self, elem: Dict) -> str:
        """Convert element to PlantUML string"""
        elem_type = elem.get('type', 'component')
        layer = elem.get('layer', 'other')
        elem_id = self._sanitize_id(elem['id'])
        elem_name = elem.get('name', elem['id'])
        
        # Color coding by layer
        color_map = {
            'strategy': '#FFF4E6',
            'business': '#FFF9E6',
            'application': '#E6F3FF',
            'technology': '#E6FFE6',
            'physical': '#F0F0F0',
            'motivation': '#FFE6F0',
            'implementation': '#F5E6FF',
        }
        color = color_map.get(layer, '#FFFFFF')
        
        # Use standard PlantUML components with color
        return f"component \"{elem_name}\" as {elem_id} {color}"
    
    def _relationship_to_puml_string(self, rel: Dict) -> str:
        """Convert relationship to PlantUML string"""
        source = self._sanitize_id(rel['source'])
        target = self._sanitize_id(rel['target'])
        rel_type = rel['type']
        description = rel.get('description', '')
        
        # Map relationship types to PlantUML arrows
        arrow_map = {
            'composition': '++--',       # Strong aggregation
            'aggregation': 'o--',        # Weak aggregation
            'assignment': '..',          # Assignment/allocation
            'realization': '.|>',        # Realizes/implements
            'serving': '--',             # Provides service
            'access': '-->',             # Accesses data
            'influence': '..>',          # Influences
            'association': '--',         # Generic association
            'triggering': '->',          # Triggers
            'flow': '-->',               # Data/control flow
            'specialization': '--|>',    # Inheritance
        }
        
        arrow = arrow_map.get(rel_type, '--')
        
        if description:
            return f"{source} {arrow} {target} : {description}"
        else:
            return f"{source} {arrow} {target}"
    
    def _sanitize_id(self, elem_id: str) -> str:
        """Sanitize element ID for PlantUML"""
        # Replace hyphens and other special characters with underscores
        return elem_id.replace('-', '_').replace('.', '_')
    
    def list_elements(self):
        """List all available elements"""
        self.load_elements()
        
        by_layer = defaultdict(list)
        for elem_id, elem in self.elements.items():
            layer = elem.get('layer', 'other')
            by_layer[layer].append((elem_id, elem.get('name', elem_id)))
        
        print("\n📋 Available Elements:\n")
        for layer in sorted(by_layer.keys()):
            print(f"  {layer.upper()}:")
            for elem_id, name in sorted(by_layer[layer]):
                print(f"    • {elem_id}: {name}")
            print()


def main():
    """Main function"""
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent.parent  # Go up two levels: generators -> scripts -> workspace
    elements_dir = workspace_dir / 'elements'
    output_dir = workspace_dir / 'diagrams'
    
    # Create output directory
    output_dir.mkdir(exist_ok=True)
    
    generator = PlantUMLGenerator(str(elements_dir))
    
    if len(sys.argv) == 1:
        # Generate full diagram
        output_file = output_dir / 'full-architecture.puml'
        generator.generate_full_diagram(str(output_file))
        print(f"\n💡 Tip: Use 'python generate_puml.py --help' for more options")
        
    elif sys.argv[1] == '--list':
        generator.list_elements()
        
    elif sys.argv[1] == '--layer':
        if len(sys.argv) < 3:
            print("Usage: python generate_puml.py --layer <layer_name>")
            return 1
        layer = sys.argv[2]
        output_file = output_dir / f'{layer}-layer.puml'
        generator.generate_layer_diagram(layer, str(output_file))
        
    elif sys.argv[1] == '--element':
        if len(sys.argv) < 3:
            print("Usage: python generate_puml.py --element <element_id> [depth]")
            return 1
        element_id = sys.argv[2]
        depth = int(sys.argv[3]) if len(sys.argv) > 3 else 1
        output_file = output_dir / f'{element_id}-context.puml'
        generator.generate_element_context_diagram(element_id, str(output_file), depth)
        
    elif sys.argv[1] == '--help':
        print("""
ArchiMate to PlantUML Generator

Usage:
  python generate_puml.py                          Generate full architecture diagram
  python generate_puml.py --list                   List all elements
  python generate_puml.py --layer <name>           Generate diagram for specific layer
  python generate_puml.py --element <id> [depth]   Generate context diagram for element
  python generate_puml.py --help                   Show this help

Examples:
  python generate_puml.py --layer application
  python generate_puml.py --element cust-portal-001 2

Output files are saved to: diagrams/

To render PlantUML files:
  - Use PlantUML online editor: http://www.plantuml.com/plantuml/
  - Install PlantUML locally: https://plantuml.com/download
  - Use VS Code extension: "PlantUML" by jebbs
        """)
    else:
        print(f"Unknown option: {sys.argv[1]}")
        print("Use --help for usage information")
        return 1
    
    return 0


if __name__ == '__main__':
    sys.exit(main())
