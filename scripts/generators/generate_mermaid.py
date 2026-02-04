#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
ArchiMate to Mermaid Generator
Generates Mermaid diagrams from markdown EA elements with clickable links
"""

import os
import sys
import yaml
import re
import markdown
from pathlib import Path
from typing import Dict, List, Set
from collections import defaultdict

# Fix Windows console encoding issues
if sys.platform == 'win32':
    import codecs
    sys.stdout = codecs.getwriter('utf-8')(sys.stdout.buffer, 'strict')
    sys.stderr = codecs.getwriter('utf-8')(sys.stderr.buffer, 'strict')

class MermaidGenerator:
    def __init__(self, elements_dir: str, output_format: str = 'html', generate_element_html: bool = False, output_dir: Path = None):
        """Initialize generator with elements directory
        
        Args:
            elements_dir: Path to elements directory
            output_format: 'html' for web pages or 'md' for markdown files
            generate_element_html: If True, generate HTML files for all element markdown files
            output_dir: Base directory for all outputs (diagrams and element HTML)
        """
        self.elements_dir = Path(elements_dir)
        self.elements = {}
        self.relationships = []
        self.output_format = output_format
        self.generate_element_html = generate_element_html
        self.output_dir = output_dir
        
        # ArchiMate element to Mermaid shape mapping
        self.element_to_shape = {
            # Strategy - use hexagon
            'resource': 'hexagon',
            'capability': 'hexagon',
            'value-stream': 'hexagon',
            'course-of-action': 'hexagon',
            
            # Business - use rounded rectangles
            'business-actor': 'rounded',
            'business-role': 'rounded',
            'business-collaboration': 'rounded',
            'business-interface': 'rounded',
            'business-process': 'rounded',
            'business-function': 'rounded',
            'business-interaction': 'rounded',
            'business-event': 'rounded',
            'business-service': 'rounded',
            'business-object': 'cylinder',
            'contract': 'document',
            'representation': 'document',
            'product': 'rounded',
            
            # Application - use rectangles
            'application-component': 'rect',
            'application-collaboration': 'rect',
            'application-interface': 'rect',
            'application-function': 'rect',
            'application-interaction': 'rect',
            'application-process': 'rect',
            'application-event': 'rect',
            'application-service': 'rect',
            'data-object': 'cylinder',
            
            # Technology - use stadium
            'node': 'stadium',
            'device': 'stadium',
            'system-software': 'stadium',
            'technology-collaboration': 'stadium',
            'technology-interface': 'stadium',
            'path': 'stadium',
            'communication-network': 'stadium',
            'technology-function': 'stadium',
            'technology-process': 'stadium',
            'technology-interaction': 'stadium',
            'technology-event': 'stadium',
            'technology-service': 'stadium',
            'artifact': 'document',
            
            # Physical - use trapezoid
            'equipment': 'trapezoid',
            'facility': 'trapezoid',
            'distribution-network': 'trapezoid',
            'material': 'cylinder',
            
            # Motivation - use circle or ellipse
            'stakeholder': 'circle',
            'driver': 'rounded',
            'assessment': 'rounded',
            'goal': 'rounded',
            'outcome': 'rounded',
            'principle': 'rounded',
            'requirement': 'rounded',
            'constraint': 'rounded',
            'meaning': 'rounded',
            'value': 'rounded',
            
            # Implementation - use subroutine
            'work-package': 'subroutine',
            'deliverable': 'subroutine',
            'implementation-event': 'subroutine',
            'plateau': 'subroutine',
            'gap': 'subroutine',
        }
    
    def load_elements(self):
        """Load all markdown elements"""
        for root, dirs, files in os.walk(self.elements_dir):
            for file in files:
                if file.endswith('.md'):
                    file_path = os.path.join(root, file)
                    element = self._parse_element(file_path)
                    if element:
                        # Store the relative path to the markdown file
                        rel_path = os.path.relpath(file_path, self.elements_dir.parent)
                        element['_file_path'] = rel_path.replace('\\', '/')
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
        
        mmd = []
        if self.output_format == 'md':
            mmd.append("```mermaid")
        mmd.append("graph TD")
        mmd.append("")
        
        # Define elements
        mmd.append("%% Define elements")
        
        # Group elements by layer
        by_layer = defaultdict(list)
        for elem_id, elem in self.elements.items():
            layer = elem.get('layer', 'other')
            by_layer[layer].append(elem)
        
        # Generate elements grouped by layer
        for layer in ['strategy', 'business', 'application', 'technology', 'physical', 'motivation', 'implementation']:
            if layer in by_layer:
                mmd.append(f"\n%% {layer.title()} Layer")
                for elem in by_layer[layer]:
                    mmd.append(self._element_to_mermaid_string(elem))
        
        # Add other elements not in standard layers
        if 'other' in by_layer:
            mmd.append("\n%% Other Elements")
            for elem in by_layer['other']:
                mmd.append(self._element_to_mermaid_string(elem))
        
        # Generate relationships
        mmd.append("\n%% Relationships")
        missing_targets = set()
        rel_styles = []
        rel_index = 0
        for rel in self.relationships:
            if rel['source'] in self.elements and rel['target'] in self.elements:
                mmd.append(self._relationship_to_mermaid_string(rel))
                style = self._relationship_style(rel['type'])
                if style:
                    rel_styles.append(f"linkStyle {rel_index} {style}")
                rel_index += 1
            elif rel['target'] not in self.elements:
                missing_targets.add(rel['target'])
        
        # Warn about missing targets
        if missing_targets:
            print(f"⚠️  Warning: Some relationships reference missing elements: {', '.join(sorted(missing_targets))}")
        
        # Add relationship styling
        if rel_styles:
            mmd.append("\n%% Relationship Styling")
            mmd.extend(rel_styles)

        # Add styling
        mmd.append("\n%% Styling")
        for layer, color in self._get_layer_colors().items():
            layer_elems = [self._sanitize_id(e['id']) for e in by_layer.get(layer, [])]
            if layer_elems:
                mmd.append(f"classDef {layer}Style fill:{color},stroke:#333,stroke-width:2px")
                mmd.append(f"class {','.join(layer_elems)} {layer}Style")
        
        # Add clickable links
        mmd.append("\n%% Clickable links to element markdown files")
        for elem_id, elem in self.elements.items():
            link = self._get_element_link(elem)
            sanitized_id = self._sanitize_id(elem_id)
            mmd.append(f"click {sanitized_id} \"{link}\" \"View details\"")
        
        if self.output_format == 'md':
            mmd.append("```")
        
        # Write to file
        output_path = Path(output_file)
        with open(output_path, 'w', encoding='utf-8') as f:
            if self.output_format == 'html':
                f.write(self._wrap_in_html('\n'.join(mmd), title))
            else:
                f.write(f"# {title}\n\n")
                f.write('\n'.join(mmd))
        
        print(f"✅ Generated: {output_file}")
        return output_file
    
    def generate_layer_diagram(self, layer: str, output_file: str):
        """Generate a diagram for a specific layer"""
        self.load_elements()
        
        layer_elements = {eid: e for eid, e in self.elements.items() if e.get('layer') == layer}
        
        if not layer_elements:
            print(f"⚠️  No elements found for layer: {layer}")
            return None
        
        mmd = []
        if self.output_format == 'md':
            mmd.append("```mermaid")
        mmd.append("graph TD")
        mmd.append("")
        
        layer_elem_ids = set(layer_elements.keys())
        
        # Collect all element IDs that need nodes (layer elements + relationship targets)
        all_node_ids = set(layer_elem_ids)
        for rel in self.relationships:
            if rel['source'] in layer_elem_ids and rel['target'] in self.elements:
                all_node_ids.add(rel['target'])
            if rel['target'] in layer_elem_ids and rel['source'] in self.elements:
                all_node_ids.add(rel['source'])
        
        # Add all referenced elements as nodes
        for elem_id in all_node_ids:
            if elem_id in self.elements:
                mmd.append(self._element_to_mermaid_string(self.elements[elem_id]))
        
        # Add relationships (only within this layer or to/from this layer)
        mmd.append("\n%% Relationships")
        rel_styles = []
        rel_index = 0
        for rel in self.relationships:
            if rel['source'] in layer_elem_ids or rel['target'] in layer_elem_ids:
                if rel['source'] in self.elements and rel['target'] in self.elements:
                    mmd.append(self._relationship_to_mermaid_string(rel))
                    style = self._relationship_style(rel['type'])
                    if style:
                        rel_styles.append(f"linkStyle {rel_index} {style}")
                    rel_index += 1
        
        # Add relationship styling
        if rel_styles:
            mmd.append("\n%% Relationship Styling")
            mmd.extend(rel_styles)

        # Add styling
        mmd.append("\n%% Styling")
        color = self._get_layer_colors().get(layer, '#FFFFFF')
        layer_elems = [self._sanitize_id(eid) for eid in layer_elements.keys()]
        mmd.append(f"classDef {layer}Style fill:{color},stroke:#333,stroke-width:2px")
        mmd.append(f"class {','.join(layer_elems)} {layer}Style")
        
        # Add clickable links for all nodes in the diagram
        mmd.append("\n%% Clickable links")
        for elem_id in all_node_ids:
            if elem_id in self.elements:
                elem = self.elements[elem_id]
                link = self._get_element_link(elem)
                sanitized_id = self._sanitize_id(elem_id)
                mmd.append(f"click {sanitized_id} \"{link}\" \"View details\"")
        
        if self.output_format == 'md':
            mmd.append("```")
        
        # Write to file
        output_path = Path(output_file)
        title = f"{layer.title()} Layer Architecture"
        with open(output_path, 'w', encoding='utf-8') as f:
            if self.output_format == 'html':
                f.write(self._wrap_in_html('\n'.join(mmd), title))
            else:
                f.write(f"# {title}\n\n")
                f.write('\n'.join(mmd))
        
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
        
        mmd = []
        if self.output_format == 'md':
            mmd.append("```mermaid")
        mmd.append("graph TD")
        mmd.append("")
        
        # Add elements
        for eid in connected:
            if eid in self.elements:
                elem_data = self.elements[eid]
                mmd.append(self._element_to_mermaid_string(elem_data))
        
        # Add relationships
        mmd.append("\n%% Relationships")
        rel_styles = []
        rel_index = 0
        for rel in self.relationships:
            if rel['source'] in connected and rel['target'] in connected:
                mmd.append(self._relationship_to_mermaid_string(rel))
                style = self._relationship_style(rel['type'])
                if style:
                    rel_styles.append(f"linkStyle {rel_index} {style}")
                rel_index += 1
        
        # Add relationship styling
        if rel_styles:
            mmd.append("\n%% Relationship Styling")
            mmd.extend(rel_styles)

        # Add styling - highlight the main element
        mmd.append("\n%% Styling")
        main_elem_id = self._sanitize_id(element_id)
        mmd.append(f"classDef highlightStyle fill:#lightblue,stroke:#0066cc,stroke-width:3px")
        mmd.append(f"class {main_elem_id} highlightStyle")
        
        # Add layer colors for other elements
        by_layer = defaultdict(list)
        for eid in connected:
            if eid != element_id and eid in self.elements:
                layer = self.elements[eid].get('layer', 'other')
                by_layer[layer].append(self._sanitize_id(eid))
        
        for layer, elems in by_layer.items():
            if elems:
                color = self._get_layer_colors().get(layer, '#FFFFFF')
                mmd.append(f"classDef {layer}Style fill:{color},stroke:#333,stroke-width:2px")
                mmd.append(f"class {','.join(elems)} {layer}Style")
        
        # Add clickable links
        mmd.append("\n%% Clickable links")
        for eid in connected:
            if eid in self.elements:
                elem = self.elements[eid]
                link = self._get_element_link(elem)
                sanitized_id = self._sanitize_id(eid)
                mmd.append(f"click {sanitized_id} \"{link}\" \"View details\"")
        
        if self.output_format == 'md':
            mmd.append("```")
        
        # Write to file
        output_path = Path(output_file)
        elem = self.elements[element_id]
        title = f"Context: {elem.get('name', element_id)}"
        with open(output_path, 'w', encoding='utf-8') as f:
            if self.output_format == 'html':
                f.write(self._wrap_in_html('\n'.join(mmd), title))
            else:
                f.write(f"# {title}\n\n")
                f.write('\n'.join(mmd))
        
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
    
    def _element_to_mermaid_string(self, elem: Dict) -> str:
        """Convert element to Mermaid string with proper shape"""
        elem_type = elem.get('type', 'component')
        elem_id = self._sanitize_id(elem['id'])
        elem_name = elem.get('name', elem['id'])
        
        # Get shape for element type
        shape = self.element_to_shape.get(elem_type, 'rect')
        
        # Map shape names to Mermaid syntax
        if shape == 'rect':
            return f"    {elem_id}[\"{elem_name}\"]"
        elif shape == 'rounded':
            return f"    {elem_id}(\"{elem_name}\")"
        elif shape == 'stadium':
            return f"    {elem_id}([{elem_name}])"
        elif shape == 'cylinder':
            return f"    {elem_id}[(\"{elem_name}\")]"
        elif shape == 'circle':
            return f"    {elem_id}((\"{elem_name}\"))"
        elif shape == 'hexagon':
            return f"    {elem_id}{{{{\"{elem_name}\"}}}}"
        elif shape == 'trapezoid':
            return f"    {elem_id}[/{elem_name}/]"
        elif shape == 'document':
            return f"    {elem_id}[\"{elem_name}\"]"
        elif shape == 'subroutine':
            return f"    {elem_id}[[\"{elem_name}\"]]"
        else:
            return f"    {elem_id}[\"{elem_name}\"]"
    
    def _relationship_to_mermaid_string(self, rel: Dict) -> str:
        """Convert relationship to Mermaid string"""
        source = self._sanitize_id(rel['source'])
        target = self._sanitize_id(rel['target'])
        rel_type = rel['type']
        description = rel.get('description', '')
        
        # Map relationship types to Mermaid arrows
        arrow_map = {
            'composition': '-->',       # Strong aggregation
            'aggregation': '-->',       # Weak aggregation
            'assignment': '-.->',       # Assignment/allocation (dotted)
            'realization': '==>',       # Realizes/implements (thick)
            'serving': '-->',           # Provides service
            'access': '-->',            # Accesses data
            'influence': '-.->',        # Influences (dotted)
            'association': '---',       # Generic association (no arrow)
            'triggering': '-->',        # Triggers
            'flow': '-->',              # Data/control flow
            'specialization': '-->',    # Inheritance
        }
        
        arrow = arrow_map.get(rel_type, '-->')
        
        if description:
            return f"    {source} {arrow}|{description}| {target}"
        else:
            return f"    {source} {arrow} {target}"

    def _relationship_style(self, rel_type: str) -> str:
        """Return Mermaid linkStyle string for ArchiMate-like styling"""
        style_map = {
            'composition': 'stroke:#333,stroke-width:2.5px',
            'aggregation': 'stroke:#333,stroke-width:2px',
            'assignment': 'stroke:#333,stroke-dasharray:4 4',
            'realization': 'stroke:#333,stroke-width:2px,stroke-dasharray:6 4',
            'serving': 'stroke:#333,stroke-width:2px',
            'access': 'stroke:#333,stroke-width:2px',
            'influence': 'stroke:#666,stroke-dasharray:2 2',
            'association': 'stroke:#999,stroke-dasharray:5 5',
            'triggering': 'stroke:#333,stroke-width:2px',
            'flow': 'stroke:#333,stroke-width:2px',
            'specialization': 'stroke:#333,stroke-width:2px',
        }
        return style_map.get(rel_type, 'stroke:#333,stroke-width:2px')
    
    def _sanitize_id(self, elem_id: str) -> str:
        """Sanitize element ID for Mermaid"""
        # Replace hyphens and other special characters with underscores
        return elem_id.replace('-', '_').replace('.', '_')
    
    def _get_element_link(self, elem: Dict) -> str:
        """Get link to element markdown/html file"""
        file_path = elem.get('_file_path', '')
        if self.output_format == 'html' and self.generate_element_html:
            # Link to generated HTML file
            return f"../{file_path.replace('.md', '.html')}"
        elif self.output_format == 'html':
            # For HTML output without element HTML generation, link to markdown
            return f"../{file_path}"
        else:
            # For markdown output, use relative path
            return file_path
    
    def _get_layer_colors(self) -> Dict[str, str]:
        """Get color mapping for layers"""
        return {
            'strategy': '#FFF4E6',
            'business': '#FFF9E6',
            'application': '#E6F3FF',
            'technology': '#E6FFE6',
            'physical': '#F0F0F0',
            'motivation': '#FFE6F0',
            'implementation': '#F5E6FF',
        }
    
    def _wrap_in_html(self, mermaid_code: str, title: str) -> str:
        """Wrap Mermaid diagram in HTML with Mermaid.js"""
        return f"""<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{title}</title>
    <script type="module">
        import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
        mermaid.initialize({{ 
            startOnLoad: true,
            theme: 'default',
            securityLevel: 'loose',
            flowchart: {{ useMaxWidth: false, htmlLabels: true }}
        }});
    </script>
    <script src="https://cdn.jsdelivr.net/npm/svg-pan-zoom@3.6.1/dist/svg-pan-zoom.min.js"></script>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }}
        .container {{
            max-width: 1400px;
            margin: 0 auto;
            background: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        h1 {{
            color: #333;
            border-bottom: 2px solid #0066cc;
            padding-bottom: 10px;
            margin-bottom: 30px;
        }}
        .diagram-toolbar {{
            display: flex;
            gap: 8px;
            margin: 10px 0 20px;
        }}
        .diagram-toolbar button {{
            background: #0066cc;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 8px 12px;
            cursor: pointer;
            font-size: 14px;
        }}
        .diagram-toolbar button:hover {{
            background: #004c99;
        }}
        .diagram-wrapper {{
            width: 100%;
            height: 75vh;
            min-height: 420px;
            border: 1px solid #e0e0e0;
            border-radius: 6px;
            overflow: hidden;
            background: #fff;
        }}
        .mermaid {{
            width: 100%;
            height: 100%;
        }}
        .info {{
            background: #e8f4f8;
            padding: 15px;
            border-left: 4px solid #0066cc;
            margin: 20px 0;
            border-radius: 4px;
        }}
        .info p {{
            margin: 5px 0;
            color: #333;
        }}
    </style>
</head>
<body>
    <div class="container">
        <h1>{title}</h1>
        <div class="info">
            <p><strong>💡 Tip:</strong> Click on any element in the diagram to view its detailed documentation.</p>
            <p><strong>🧭 Navigation:</strong> Use the zoom controls below or scroll to zoom and drag to pan.</p>
        </div>
        <div class="diagram-toolbar">
            <button type="button" id="zoom-in">Zoom In</button>
            <button type="button" id="zoom-out">Zoom Out</button>
            <button type="button" id="zoom-reset">Reset</button>
        </div>
        <div class="diagram-wrapper">
            <div class="mermaid">
{mermaid_code}
            </div>
        </div>
    </div>
    <script>
        let panZoomInstance = null;
        const ensureViewBox = (svg) => {{
            if (!svg.getAttribute('viewBox')) {{
                const box = svg.getBBox();
                if (box.width > 0 && box.height > 0) {{
                    svg.setAttribute('viewBox', `${{box.x}} ${{box.y}} ${{box.width}} ${{box.height}}`);
                }}
            }}
        }};

        const initPanZoom = (attempt = 0) => {{
            const svg = document.querySelector('.mermaid svg');
            if (!svg || svg.__panzoom) return;
            ensureViewBox(svg);
            const box = svg.viewBox.baseVal;
            if (!box || !isFinite(box.width) || !isFinite(box.height) || box.width === 0 || box.height === 0) {{
                if (attempt < 10) {{
                    requestAnimationFrame(() => initPanZoom(attempt + 1));
                }}
                return;
            }}
            panZoomInstance = svgPanZoom(svg, {{
                zoomEnabled: true,
                controlIconsEnabled: false,
                fit: true,
                center: true,
                minZoom: 0.2,
                maxZoom: 10
            }});
            svg.__panzoom = true;
        }};

        const observer = new MutationObserver(() => initPanZoom());
        const mermaidRoot = document.querySelector('.mermaid');
        if (mermaidRoot) {{
            observer.observe(mermaidRoot, {{ childList: true, subtree: true }});
        }}

        window.addEventListener('load', () => initPanZoom());

        document.getElementById('zoom-in').addEventListener('click', () => panZoomInstance && panZoomInstance.zoomIn());
        document.getElementById('zoom-out').addEventListener('click', () => panZoomInstance && panZoomInstance.zoomOut());
        document.getElementById('zoom-reset').addEventListener('click', () => panZoomInstance && panZoomInstance.resetZoom());
    </script>
</body>
</html>
"""
    
    def _convert_element_to_html(self, elem: Dict, md_file_path: Path, workspace_dir: Path) -> str:
        """Convert element markdown to HTML page"""
        try:
            with open(md_file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Extract frontmatter and markdown
            pattern = r'^---\s*\n(.*?)\n---\s*\n(.*)$'
            match = re.match(pattern, content, re.DOTALL)
            
            if not match:
                return None
            
            frontmatter = yaml.safe_load(match.group(1))
            markdown_content = match.group(2)
            
            # Convert markdown to HTML
            md = markdown.Markdown(extensions=['extra', 'codehilite', 'tables', 'fenced_code'])
            html_content = md.convert(markdown_content)
            
            # Get element properties
            elem_id = frontmatter.get('id', 'unknown')
            elem_name = frontmatter.get('name', elem_id)
            elem_type = frontmatter.get('type', 'unknown')
            elem_layer = frontmatter.get('layer', 'unknown')
            
            # Get layer color
            layer_colors = self._get_layer_colors()
            layer_color = layer_colors.get(elem_layer, '#FFFFFF')
            
            # Build properties table
            properties_html = ""
            if 'properties' in frontmatter:
                props = frontmatter['properties']
                properties_html = "<h2>Properties</h2><table class='properties'>"
                for key, value in props.items():
                    properties_html += f"<tr><td><strong>{key.replace('-', ' ').title()}</strong></td><td>{value}</td></tr>"
                properties_html += "</table>"
            
            # Build relationships table
            relationships_html = ""
            if 'relationships' in frontmatter:
                rels = frontmatter['relationships']
                relationships_html = "<h2>Relationships</h2><table class='relationships'>"
                relationships_html += "<tr><th>Type</th><th>Target</th><th>Description</th></tr>"
                for rel in rels:
                    rel_type = rel.get('type', '').replace('-', ' ').title()
                    target = rel.get('target', '')
                    # Try to link to target element if it exists
                    if target in self.elements:
                        target_name = self.elements[target].get('name', target)
                        target_path = self.elements[target].get('_file_path', '').replace('.md', '.html')
                        target_link = f"<a href='../{target_path}'>{target_name}</a>"
                    else:
                        target_link = target
                    description = rel.get('description', '')
                    relationships_html += f"<tr><td>{rel_type}</td><td>{target_link}</td><td>{description}</td></tr>"
                relationships_html += "</table>"
            
            # Build tags
            tags_html = ""
            if 'tags' in frontmatter:
                tags = frontmatter['tags']
                tags_html = "<div class='tags'>"
                for tag in tags:
                    tags_html += f"<span class='tag'>{tag}</span>"
                tags_html += "</div>"
            
            # Generate HTML
            html = f"""<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{elem_name} - EA Tool</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
            line-height: 1.6;
        }}
        .container {{
            max-width: 1000px;
            margin: 0 auto;
            background: white;
            padding: 40px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        .header {{
            border-left: 4px solid {layer_color};
            padding-left: 20px;
            margin-bottom: 30px;
        }}
        .header h1 {{
            margin: 0 0 10px 0;
            color: #333;
        }}
        .metadata {{
            display: flex;
            gap: 20px;
            margin-bottom: 10px;
            color: #666;
            font-size: 0.9em;
        }}
        .metadata .item {{
            display: flex;
            align-items: center;
            gap: 5px;
        }}
        .badge {{
            display: inline-block;
            padding: 4px 12px;
            border-radius: 12px;
            font-size: 0.85em;
            font-weight: 500;
            background: {layer_color};
            color: #333;
        }}
        .tags {{
            margin: 20px 0;
        }}
        .tag {{
            display: inline-block;
            padding: 4px 12px;
            margin: 4px 4px 4px 0;
            border-radius: 4px;
            background: #e8f4f8;
            color: #0066cc;
            font-size: 0.85em;
        }}
        .content {{
            margin: 30px 0;
        }}
        .content h2 {{
            color: #333;
            border-bottom: 2px solid #f0f0f0;
            padding-bottom: 10px;
            margin-top: 30px;
        }}
        .content h3 {{
            color: #555;
            margin-top: 20px;
        }}
        .content ul {{
            padding-left: 20px;
        }}
        .content li {{
            margin: 8px 0;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }}
        table th {{
            background: #f8f9fa;
            padding: 12px;
            text-align: left;
            border-bottom: 2px solid #dee2e6;
            font-weight: 600;
        }}
        table td {{
            padding: 12px;
            border-bottom: 1px solid #dee2e6;
        }}
        table tr:hover {{
            background: #f8f9fa;
        }}
        .properties td:first-child {{
            width: 200px;
            color: #666;
        }}
        a {{
            color: #0066cc;
            text-decoration: none;
        }}
        a:hover {{
            text-decoration: underline;
        }}
        .nav {{
            margin-bottom: 20px;
        }}
        .nav a {{
            color: #666;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }}
        .nav a:hover {{
            color: #0066cc;
        }}
        code {{
            background: #f5f5f5;
            padding: 2px 6px;
            border-radius: 3px;
            font-family: 'Courier New', monospace;
            font-size: 0.9em;
        }}
        pre {{
            background: #f5f5f5;
            padding: 15px;
            border-radius: 5px;
            overflow-x: auto;
        }}
        pre code {{
            background: none;
            padding: 0;
        }}
    </style>
</head>
<body>
    <div class="container">
        <div class="nav">
            <a href="../diagrams/full-architecture-mermaid.html">← Back to Architecture</a>
        </div>
        
        <div class="header">
            <h1>{elem_name}</h1>
            <div class="metadata">
                <div class="item">
                    <span class="badge">{elem_layer.title()}</span>
                </div>
                <div class="item">
                    <strong>Type:</strong> {elem_type.replace('-', ' ').title()}
                </div>
                <div class="item">
                    <strong>ID:</strong> <code>{elem_id}</code>
                </div>
            </div>
            {tags_html}
        </div>
        
        <div class="content">
{html_content}
        </div>
        
        {properties_html}
        
        {relationships_html}
    </div>
</body>
</html>
"""
            return html
            
        except Exception as e:
            print(f"⚠️  Warning: Could not convert {md_file_path}: {e}")
            return None
    
    def generate_element_html_files(self, workspace_dir: Path):
        """Generate HTML files for all element markdown files"""
        if not self.generate_element_html:
            return
        
        count = 0
        for elem_id, elem in self.elements.items():
            file_path = elem.get('_file_path', '')
            if not file_path:
                continue
            
            md_file = workspace_dir / file_path
            
            # Output to output_dir if specified, otherwise to workspace
            if self.output_dir:
                html_file = self.output_dir / file_path.replace('.md', '.html')
            else:
                html_file = workspace_dir / file_path.replace('.md', '.html')
            
            if not md_file.exists():
                continue
            
            html_content = self._convert_element_to_html(elem, md_file, workspace_dir)
            if html_content:
                # Create directory if needed
                html_file.parent.mkdir(parents=True, exist_ok=True)
                
                with open(html_file, 'w', encoding='utf-8') as f:
                    f.write(html_content)
                count += 1
        
        if count > 0:
            print(f"✅ Generated {count} element HTML files")
    
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
    
    # Default to HTML output
    output_format = 'html'
    generate_element_html = False
    use_output_folder = False
    args = sys.argv[1:]
    
    # Check for output format flags
    if '--md' in args:
        output_format = 'md'
        args.remove('--md')
    
    if '--html-elements' in args:
        generate_element_html = True
        args.remove('--html-elements')
    
    if '--output' in args:
        use_output_folder = True
        args.remove('--output')
    
    # Determine output directory
    if use_output_folder or generate_element_html:
        output_base_dir = workspace_dir / 'output'
        output_base_dir.mkdir(exist_ok=True)
        output_dir = output_base_dir / 'diagrams'
    else:
        output_base_dir = None
        output_dir = workspace_dir / 'diagrams'
    
    # Create output directory
    output_dir.mkdir(exist_ok=True)
    
    generator = MermaidGenerator(str(elements_dir), output_format, generate_element_html, output_base_dir)
    
    if len(args) == 0:
        # Generate full diagram
        ext = 'html' if output_format == 'html' else 'md'
        output_file = output_dir / f'full-architecture-mermaid.{ext}'
        generator.generate_full_diagram(str(output_file))
        
        # Generate element HTML files if requested
        if generate_element_html:
            generator.generate_element_html_files(workspace_dir)
        
        if use_output_folder or generate_element_html:
            print(f"\n📁 Output directory: {output_base_dir}")
        print(f"\n💡 Tip: Use 'python generate_mermaid.py --help' for more options")
        
    elif args[0] == '--list':
        generator.list_elements()
        
    elif args[0] == '--layer':
        if len(args) < 2:
            print("Usage: python generate_mermaid.py --layer <layer_name>")
            return 1
        layer = args[1]
        ext = 'html' if output_format == 'html' else 'md'
        output_file = output_dir / f'{layer}-layer-mermaid.{ext}'
        generator.generate_layer_diagram(layer, str(output_file))
        
        # Generate element HTML files if requested
        if generate_element_html:
            generator.generate_element_html_files(workspace_dir)
        
        if use_output_folder or generate_element_html:
            print(f"\n📁 Output directory: {output_base_dir}")
        
    elif args[0] == '--element':
        if len(args) < 2:
            print("Usage: python generate_mermaid.py --element <element_id> [depth]")
            return 1
        element_id = args[1]
        depth = int(args[2]) if len(args) > 2 else 1
        ext = 'html' if output_format == 'html' else 'md'
        output_file = output_dir / f'{element_id}-context-mermaid.{ext}'
        generator.generate_element_context_diagram(element_id, str(output_file), depth)
        
        # Generate element HTML files if requested
        if generate_element_html:
            generator.generate_element_html_files(workspace_dir)
        
        if use_output_folder or generate_element_html:
            print(f"\n📁 Output directory: {output_base_dir}")
        
    elif args[0] == '--help':
        print("""
ArchiMate to Mermaid Generator

Usage:
  python generate_mermaid.py [--html-elements]                          Generate full architecture diagram
  python generate_mermaid.py --list                                     List all elements
  python generate_mermaid.py --layer <name> [--html-elements]           Generate diagram for specific layer
  python generate_mermaid.py --element <id> [depth] [--html-elements]   Generate context diagram for element
  python generate_mermaid.py --help                                     Show this help

Options:
  --md              Output diagram as markdown file instead of HTML (default: HTML)
  --html-elements   Generate HTML files for all element markdown files (creates static site)
  --output          Use 'output/' folder for all generated files (automatic with --html-elements)

Examples:
  python generate_mermaid.py
  python generate_mermaid.py --html-elements
  python generate_mermaid.py --layer application --html-elements
  python generate_mermaid.py --element cust-portal-001 2
  python generate_mermaid.py --layer business --md

Output files:
  Without --html-elements: diagrams/ and elements/ (in workspace)
  With --html-elements:    output/ folder (ready for deployment)
    output/diagrams/       - All diagram HTML files
    output/elements/       - All element HTML files

Features:
  ✓ Interactive clickable diagrams
  ✓ Links to element documentation
  ✓ Color-coded by layer
  ✓ HTML output with Mermaid.js (no install needed)
  ✓ Markdown output for GitHub/docs
  ✓ Generate static website with --html-elements
  ✓ Deploy-ready output folder

Static Website Generation:
  Use --html-elements to convert all element markdown files to HTML.
  All files are automatically placed in the 'output/' folder.
  This creates a fully browsable static website where:
  - Diagrams link to element HTML pages
  - Element pages link back to diagrams
  - Element pages link to related elements
  - All files are viewable in a browser without a web server

To view HTML files:
  - Open the .html file in any web browser
  - Elements are clickable - click to view markdown details

To view markdown files:
  - View on GitHub (automatic rendering)
  - Use VS Code with Markdown Preview Mermaid Support extension
  - Use any markdown viewer with Mermaid support
        """)
    else:
        print(f"Unknown option: {args[0]}")
        print("Use --help for usage information")
        return 1
    
    return 0


if __name__ == '__main__':
    sys.exit(main())
