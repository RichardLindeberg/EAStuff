#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
ArchiMate to draw.io Generator
Generates draw.io XML diagrams from markdown EA elements
"""

import os
import sys
import yaml
import re
import xml.etree.ElementTree as ET
from pathlib import Path
from typing import Dict, List, Set, Tuple
from collections import defaultdict
import uuid

# Fix Windows console encoding issues
if sys.platform == 'win32':
    import codecs
    sys.stdout = codecs.getwriter('utf-8')(sys.stdout.buffer, 'strict')
    sys.stderr = codecs.getwriter('utf-8')(sys.stderr.buffer, 'strict')

class DrawIOGenerator:
    def __init__(self, elements_dir: str, link_format: str = 'md', link_base: str = '..'):
        """Initialize generator with elements directory"""
        self.elements_dir = Path(elements_dir)
        self.elements = {}
        self.relationships = []
        self.link_format = link_format
        self.link_base = link_base.rstrip('/') if link_base else ''
        
        # Color scheme for different layers
        self.layer_colors = {
            'motivation': '#FFE6F0',  # Pink
            'strategy': '#FFF4E6',    # Orange
            'business': '#FFF9E6',    # Light yellow
            'application': '#E6F3FF', # Light blue
            'technology': '#E6FFE6',  # Light green
            'physical': '#F0E6FF',    # Light purple
            'implementation': '#FFE6E6', # Light red
        }
        
        # ArchiMate 3.2 shape styles for draw.io
        # Using draw.io's mxgraph.archimate3 shape library with simplified square/icon approach
        self.archimate_shapes = {
            # Strategy Layer
            'resource': 'shape=mxgraph.archimate3.application;appType=resource;archiType=square',
            'capability': 'shape=mxgraph.archimate3.application;appType=capability;archiType=square',
            'value-stream': 'shape=mxgraph.archimate3.application;appType=valueStream;archiType=rounded',
            'course-of-action': 'shape=mxgraph.archimate3.application;appType=course;archiType=square',
            
            # Motivation Layer
            'goal': 'shape=mxgraph.archimate3.application;appType=goal;archiType=rounded',
            'driver': 'shape=mxgraph.archimate3.application;appType=driver;archiType=square',
            'principle': 'shape=mxgraph.archimate3.application;appType=principle;archiType=square',
            'requirement': 'shape=mxgraph.archimate3.application;appType=requirement;archiType=square',
            'stakeholder': 'shape=mxgraph.archimate3.application;appType=stakeholder;archiType=square',
            'outcome': 'shape=mxgraph.archimate3.application;appType=goal;archiType=rounded',
            'constraint': 'shape=mxgraph.archimate3.application;appType=constraint;archiType=square',
            
            # Business Layer
            'business-actor': 'shape=mxgraph.archimate3.application;appType=actor;archiType=square',
            'business-role': 'shape=mxgraph.archimate3.application;appType=actor;archiType=square',
            'business-collaboration': 'shape=mxgraph.archimate3.application;appType=collab;archiType=rounded',
            'business-interface': 'shape=mxgraph.archimate3.application;appType=interface;archiType=square',
            'business-process': 'shape=mxgraph.archimate3.application;appType=proc;archiType=rounded',
            'business-function': 'shape=mxgraph.archimate3.application;appType=function;archiType=rounded',
            'business-interaction': 'shape=mxgraph.archimate3.application;appType=interaction;archiType=rounded',
            'business-event': 'shape=mxgraph.archimate3.application;appType=event;archiType=square',
            'business-service': 'shape=mxgraph.archimate3.application;appType=service;archiType=rounded',
            'business-object': 'shape=mxgraph.archimate3.application;appType=data;archiType=square',
            'contract': 'shape=mxgraph.archimate3.application;appType=interface;archiType=square',
            'product': 'shape=mxgraph.archimate3.application;appType=service;archiType=square',
            
            # Application Layer
            'application-component': 'shape=mxgraph.archimate3.application;appType=comp;archiType=square',
            'application-collaboration': 'shape=mxgraph.archimate3.application;appType=collab;archiType=rounded',
            'application-interface': 'shape=mxgraph.archimate3.application;appType=interface;archiType=square',
            'application-function': 'shape=mxgraph.archimate3.application;appType=function;archiType=rounded',
            'application-interaction': 'shape=mxgraph.archimate3.application;appType=interaction;archiType=rounded',
            'application-process': 'shape=mxgraph.archimate3.application;appType=proc;archiType=rounded',
            'application-event': 'shape=mxgraph.archimate3.application;appType=event;archiType=square',
            'application-service': 'shape=mxgraph.archimate3.application;appType=service;archiType=rounded',
            'data-object': 'shape=mxgraph.archimate3.application;appType=data;archiType=square',
            
            # Technology Layer
            'node': 'shape=mxgraph.archimate3.application;appType=node;archiType=square',
            'device': 'shape=mxgraph.archimate3.application;appType=device;archiType=square',
            'system-software': 'shape=mxgraph.archimate3.application;appType=comp;archiType=square',
            'technology-collaboration': 'shape=mxgraph.archimate3.application;appType=collab;archiType=rounded',
            'technology-interface': 'shape=mxgraph.archimate3.application;appType=interface;archiType=square',
            'path': 'shape=mxgraph.archimate3.application;appType=path;archiType=rounded',
            'communication-network': 'shape=mxgraph.archimate3.application;appType=network;archiType=rounded',
            'technology-function': 'shape=mxgraph.archimate3.application;appType=function;archiType=rounded',
            'technology-process': 'shape=mxgraph.archimate3.application;appType=proc;archiType=rounded',
            'technology-interaction': 'shape=mxgraph.archimate3.application;appType=interaction;archiType=rounded',
            'technology-event': 'shape=mxgraph.archimate3.application;appType=event;archiType=square',
            'technology-service': 'shape=mxgraph.archimate3.application;appType=service;archiType=rounded',
            'artifact': 'shape=mxgraph.archimate3.application;appType=artifact;archiType=square',
            
            # Physical Layer
            'equipment': 'shape=mxgraph.archimate3.application;appType=device;archiType=square',
            'facility': 'shape=mxgraph.archimate3.application;appType=facility;archiType=square',
            'distribution-network': 'shape=mxgraph.archimate3.application;appType=network;archiType=rounded',
            'material': 'shape=mxgraph.archimate3.application;appType=data;archiType=square',
            
            # Implementation Layer
            'work-package': 'shape=mxgraph.archimate3.application;appType=work;archiType=square',
            'deliverable': 'shape=mxgraph.archimate3.application;appType=artifact;archiType=square',
            'implementation-event': 'shape=mxgraph.archimate3.application;appType=event;archiType=square',
            'plateau': 'shape=mxgraph.archimate3.application;appType=function;archiType=rounded',
            'gap': 'shape=mxgraph.archimate3.application;appType=requirement;archiType=square',
        }
        
        # Element type display names
        self.element_type_names = {
            'goal': 'Goal',
            'driver': 'Driver',
            'principle': 'Principle',
            'requirement': 'Requirement',
            'resource': 'Resource',
            'capability': 'Capability',
            'value-stream': 'Value Stream',
            'course-of-action': 'Course of Action',
            'business-actor': 'Business Actor',
            'business-role': 'Business Role',
            'business-process': 'Business Process',
            'business-function': 'Business Function',
            'business-object': 'Business Object',
            'business-service': 'Business Service',
            'application-component': 'Application Component',
            'application-interface': 'Application Interface',
            'node': 'Technology Node',
            'artifact': 'Technology Artifact',
            'technology-service': 'Technology Service',
        }
    
    def load_elements(self):
        """Load all elements from markdown files"""
        layer_dirs = self.elements_dir.glob('*/')
        
        for layer_dir in sorted(layer_dirs):
            if not layer_dir.is_dir():
                continue
            
            layer = layer_dir.name
            md_files = sorted(layer_dir.glob('*.md'))
            
            for md_file in md_files:
                try:
                    # Parse markdown file
                    with open(md_file, 'r', encoding='utf-8') as f:
                        content = f.read()
                    
                    # Extract YAML front matter
                    if content.startswith('---'):
                        parts = content.split('---', 2)
                        if len(parts) >= 3:
                            try:
                                element = yaml.safe_load(parts[1])
                                # Extract relationships
                                if 'relationships' in element:
                                    for rel in element['relationships']:
                                        self.relationships.append({
                                            'source': element.get('id'),
                                            'target': rel.get('target'),
                                            'label': rel.get('label', ''),
                                            'type': rel.get('type', 'relates-to')
                                        })
                                
                                element['layer'] = layer
                                element['file'] = str(md_file)
                                self.elements[element.get('id')] = element
                            except yaml.YAMLError:
                                pass
                except Exception as e:
                    print(f"⚠️  Error loading {md_file}: {e}")
    
    def get_element_type_label(self, element_type: str) -> str:
        """Get display label for element type"""
        return self.element_type_names.get(element_type, element_type)
    
    def get_layer_color(self, layer: str) -> str:
        """Get color for layer"""
        return self.layer_colors.get(layer, '#FFFFFF')
    
    def get_archimate_style(self, element_type: str, color: str) -> str:
        """Get draw.io ArchiMate 3.2 style for element type"""
        shape_style = self.archimate_shapes.get(element_type, 'shape=mxgraph.archimate3.capability')
        # Combine ArchiMate shape with proper draw.io formatting
        return f"html=1;outlineConnect=0;whiteSpace=wrap;{shape_style};fillColor={color};strokeColor=#000000;strokeWidth=2;fontSize=11;fontStyle=1;"
    
    def get_relationship_arrow_style(self, relationship_type: str) -> str:
        """Get ArchiMate 3.2 relationship arrow style for draw.io"""
        # Map ArchiMate relationship types to draw.io arrow styles
        # Based on ArchiMate 3.2 relationship notation in draw.io
        arrow_styles = {
            # Structural relationships
            'composition': 'startArrow=diamondThin;startFill=1;endArrow=none;endFill=0;',  # Filled diamond
            'aggregation': 'startArrow=diamondThin;startFill=0;endArrow=none;endFill=0;',  # Unfilled diamond
            'assignment': 'startArrow=diamond;startFill=0;endArrow=none;endFill=0;',       # Hollow diamond
            'realization': 'startArrow=oval;startFill=1;endArrow=block;endFill=1;',        # Oval to block
            
            # Dependency relationships
            'serving': 'startArrow=none;endArrow=classic;endFill=1;',                      # Simple arrow
            'access': 'startArrow=none;endArrow=classic;endFill=1;dashed=1;',             # Dashed arrow
            'influence': 'startArrow=none;endArrow=classic;endFill=1;dashed=1;',          # Dashed arrow
            'association': 'startArrow=none;endArrow=classic;endFill=1;',                  # Simple arrow
            
            # Dynamic relationships
            'triggering': 'startArrow=none;endArrow=classic;endFill=1;',                   # Simple arrow
            'flow': 'startArrow=none;endArrow=classic;endFill=1;',                         # Simple arrow
            
            # Other relationships
            'specialization': 'startArrow=none;endArrow=classic;endFill=1;',              # Simple arrow
            'junction': 'startArrow=none;endArrow=classic;endFill=1;',                     # Simple arrow (logical operator)
            
            # Legacy/default
            'relates-to': 'startArrow=none;endArrow=classic;endFill=1;',
        }
        return arrow_styles.get(relationship_type, 'startArrow=none;endArrow=classic;endFill=1;')
    
    def generate_drawio_xml(self, diagram_name: str = "Enterprise Architecture") -> str:
        """Generate draw.io XML with all elements and relationships"""
        
        # Calculate grid layout
        elements_by_layer = defaultdict(list)
        for elem_id, elem in sorted(self.elements.items()):
            elements_by_layer[elem.get('layer')].append((elem_id, elem))
        
        # Start building XML
        xml_str = '''<?xml version="1.0" encoding="UTF-8"?>
<mxfile host="app.diagrams.net" modified="2026-02-02" agent="Enterprise Architecture Generator" version="1.0" type="device">
  <diagram id="ea-diagram" name="Enterprise Architecture">
    <mxGraphModel dx="1200" dy="800" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1169" pageHeight="827" background="#ffffff" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
'''
        
        # Add elements
        cell_id = 2
        cell_map = {}  # Map element IDs to cell IDs
        
        layers_ordered = ['motivation', 'strategy', 'business', 'application', 'technology']
        y_offset = 40
        
        for layer_idx, layer in enumerate(layers_ordered):
            if layer not in elements_by_layer:
                continue
            
            elements_list = elements_by_layer[layer]
            x_offset = 40
            
            # Layer header
            layer_title = layer.replace('-', ' ').title()
            xml_str += f'''        <mxCell id="layer-header-{layer_idx}" value="{layer_title} Layer" style="text;html=1;fontSize=14;fontStyle=1;fillColor=none;strokeColor=none;" vertex="1" parent="1">
          <mxGeometry x="{x_offset}" y="{y_offset - 30}" width="200" height="25" as="geometry" />
        </mxCell>
'''
            
            y_pos = y_offset
            for elem_idx, (elem_id, elem) in enumerate(elements_list):
                # Position elements in a grid
                x_pos = x_offset + (elem_idx % 3) * 350
                y_pos = y_offset + (elem_idx // 3) * 120
                
                color = self.get_layer_color(layer)
                elem_type = elem.get('type', 'unknown')
                elem_label = elem.get('name', elem_id)
                elem_desc = elem.get('description', '')
                
                # Get ArchiMate style
                style = self.get_archimate_style(elem_type, color)
                
                # Create cell with ArchiMate 3.2 styling
                xml_str += f'''        <mxCell id="cell-{cell_id}" value="{elem_label}" style="{style}" vertex="1" parent="1">
          <mxGeometry x="{x_pos}" y="{y_pos}" width="320" height="90" as="geometry" />
        </mxCell>
'''
                
                cell_map[elem_id] = f"cell-{cell_id}"
                cell_id += 1
            
            y_offset = y_pos + 150
        
        # Add relationships
        for rel in self.relationships:
            source_id = cell_map.get(rel['source'])
            target_id = cell_map.get(rel['target'])
            
            if source_id and target_id:
                rel_label = rel.get('label', '')
                rel_type = rel.get('type', 'relates-to')
                
                # Get ArchiMate 3.2 relationship arrow style
                arrow_style = self.get_relationship_arrow_style(rel_type)
                
                xml_str += f'''        <mxCell id="edge-{cell_id}" value="{rel_label}" style="html=1;{arrow_style}edgeStyle=elbowEdgeStyle;elbow=vertical;rounded=0;" edge="1" parent="1" source="{source_id}" target="{target_id}">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
'''
                cell_id += 1
        
        xml_str += '''      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
'''
        return xml_str
    
    def generate_drawio_layer(self, layer: str) -> str:
        """Generate draw.io XML for a specific layer"""
        
        # Filter elements for this layer
        layer_elements = {k: v for k, v in self.elements.items() 
                         if v.get('layer') == layer}
        layer_rels = [r for r in self.relationships 
                     if r['source'] in layer_elements and r['target'] in layer_elements]
        
        xml_str = '''<?xml version="1.0" encoding="UTF-8"?>
<mxfile host="app.diagrams.net" modified="2026-02-02" agent="Enterprise Architecture Generator" version="1.0" type="device">
  <diagram id="ea-diagram" name="Enterprise Architecture">
    <mxGraphModel dx="1200" dy="800" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1169" pageHeight="827" background="#ffffff" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
'''
        
        cell_id = 2
        cell_map = {}
        x_offset = 40
        y_offset = 40
        
        # Add elements
        for elem_idx, (elem_id, elem) in enumerate(sorted(layer_elements.items())):
            x_pos = x_offset + (elem_idx % 3) * 350
            y_pos = y_offset + (elem_idx // 3) * 120
            
            color = self.get_layer_color(layer)
            elem_type = elem.get('type', 'unknown')
            elem_label = elem.get('name', elem_id)
            
            # Get ArchiMate style
            style = self.get_archimate_style(elem_type, color)
            
            xml_str += f'''        <mxCell id="cell-{cell_id}" value="{elem_label}" style="{style}" vertex="1" parent="1">
          <mxGeometry x="{x_pos}" y="{y_pos}" width="320" height="90" as="geometry" />
        </mxCell>
'''
            
            cell_map[elem_id] = f"cell-{cell_id}"
            cell_id += 1
        
        # Add relationships
        for rel in layer_rels:
            source_id = cell_map.get(rel['source'])
            target_id = cell_map.get(rel['target'])
            
            if source_id and target_id:
                rel_label = rel.get('label', '')
                rel_type = rel.get('type', 'relates-to')
                
                # Get ArchiMate 3.2 relationship arrow style
                arrow_style = self.get_relationship_arrow_style(rel_type)
                
                xml_str += f'''        <mxCell id="edge-{cell_id}" value="{rel_label}" style="html=1;{arrow_style}edgeStyle=elbowEdgeStyle;elbow=vertical;rounded=0;" edge="1" parent="1" source="{source_id}" target="{target_id}">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
'''
                cell_id += 1
        
        xml_str += '''      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
'''
        return xml_str
    
    def save_drawio_files(self, output_dir: Path):
        """Save draw.io files to output directory"""
        output_dir.mkdir(parents=True, exist_ok=True)
        
        # Save full architecture diagram
        full_diagram = self.generate_drawio_xml("Enterprise Architecture")
        full_path = output_dir / 'full-architecture.drawio'
        with open(full_path, 'w', encoding='utf-8') as f:
            f.write(full_diagram)
        print(f"✅ Generated {full_path}")
        
        # Save layer-specific diagrams
        layers = set(elem.get('layer') for elem in self.elements.values())
        for layer in sorted(layers):
            if layer:
                layer_diagram = self.generate_drawio_layer(layer)
                layer_name = layer.replace('-', '_')
                layer_path = output_dir / f'{layer_name}-layer.drawio'
                with open(layer_path, 'w', encoding='utf-8') as f:
                    f.write(layer_diagram)
                print(f"✅ Generated {layer_path}")


def main():
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent.parent
    input_dir = workspace_dir / 'elements'
    output_dir = workspace_dir / 'output' / 'diagrams'

    args = sys.argv[1:]
    if '--input' in args:
        idx = args.index('--input')
        if idx + 1 < len(args):
            input_dir = Path(args[idx + 1])

    if '--output' in args:
        idx = args.index('--output')
        if idx + 1 < len(args):
            output_dir = Path(args[idx + 1])

    if not input_dir.exists():
        print(f"❌ Input directory not found: {input_dir}")
        return 1

    print(f"📂 Loading elements from: {input_dir}")
    generator = DrawIOGenerator(str(input_dir))
    generator.load_elements()
    
    print(f"📊 Loaded {len(generator.elements)} elements and {len(generator.relationships)} relationships")
    
    print(f"💾 Saving draw.io files to: {output_dir}")
    generator.save_drawio_files(output_dir)
    
    print("✅ Draw.io generation complete!")
    return 0


if __name__ == '__main__':
    sys.exit(main())
