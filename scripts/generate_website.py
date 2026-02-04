#!/usr/bin/env python3
"""
Generate a static website from ArchiMate elements.

This script reads all markdown elements from the elements/ directory and generates
a static website organized by layer, showing incoming and outgoing relationships.

Usage:
    python generate_website.py [output_dir]

Output:
    - If output_dir is provided: saves to that directory
    - Otherwise: saves to output/website/
"""

import os
import glob
from pathlib import Path
from collections import defaultdict
import frontmatter
import markdown
import re


# Layer order for display
LAYER_ORDER = [
    'strategy',
    'motivation',
    'business',
    'application',
    'technology',
    'physical',
    'implementation'
]

# Layer display names
LAYER_NAMES = {
    'strategy': 'Strategy Layer',
    'motivation': 'Motivation Layer',
    'business': 'Business Layer',
    'application': 'Application Layer',
    'technology': 'Technology Layer',
    'physical': 'Physical Layer',
    'implementation': 'Implementation & Migration Layer'
}


class ElementRegistry:
    """Registry to store all elements and their relationships."""
    
    def __init__(self):
        self.elements = {}  # id -> element data
        self.elements_by_layer = defaultdict(list)  # layer -> [elements]
        self.incoming_relations = defaultdict(list)  # target_id -> [(source_id, rel_type, description)]
        
    def add_element(self, element_data, file_path):
        """Add an element to the registry."""
        elem_id = element_data.get('id')
        if not elem_id:
            print(f"Warning: Element in {file_path} has no ID")
            return
            
        layer = element_data.get('layer', 'unknown')
        
        tags = element_data.get('tags', [])
        if isinstance(tags, str):
            tags = [t.strip() for t in tags.split(',') if t.strip()]
        elif tags is None:
            tags = []

        self.elements[elem_id] = {
            'id': elem_id,
            'name': element_data.get('name', 'Unnamed'),
            'type': element_data.get('type', 'unknown'),
            'layer': layer,
            'content': element_data.get('content', ''),
            'properties': element_data.get('properties', {}),
            'tags': tags,
            'relationships': element_data.get('relationships', []),
            'file_path': file_path
        }
        
        self.elements_by_layer[layer].append(elem_id)
        
        # Register outgoing relationships for incoming relationship tracking
        for rel in element_data.get('relationships', []):
            target_id = rel.get('target')
            if target_id:
                self.incoming_relations[target_id].append({
                    'source_id': elem_id,
                    'type': rel.get('type', 'unknown'),
                    'description': rel.get('description', '')
                })
    
    def get_element(self, elem_id):
        """Get element by ID."""
        return self.elements.get(elem_id)
    
    def get_incoming_relations(self, elem_id):
        """Get all incoming relations for an element."""
        return self.incoming_relations.get(elem_id, [])


def read_elements(elements_dir):
    """Read all element markdown files and build registry."""
    registry = ElementRegistry()
    
    # Find all .md files in elements directory
    pattern = os.path.join(elements_dir, '**', '*.md')
    for file_path in glob.glob(pattern, recursive=True):
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                post = frontmatter.load(f)
                
            element_data = dict(post.metadata)
            element_data['content'] = post.content
            
            registry.add_element(element_data, file_path)
            
        except Exception as e:
            print(f"Error reading {file_path}: {e}")
    
    return registry


def slugify_tag(tag):
    """Create a URL-safe slug for tag pages."""
    slug = re.sub(r'[^a-zA-Z0-9]+', '-', tag.strip().lower())
    return slug.strip('-') or 'tag'


def build_tag_index(registry):
    """Build mapping of tag -> list of element IDs."""
    tags_index = defaultdict(list)
    for elem_id, element in registry.elements.items():
        for tag in element.get('tags', []):
            if tag:
                tags_index[tag].append(elem_id)
    return tags_index


def generate_html_header(title, current_page='', base_path=''):
    """Generate HTML header with navigation."""
    nav_items = []
    for layer in LAYER_ORDER:
        layer_name = LAYER_NAMES.get(layer, layer.title())
        active = 'active' if current_page == layer else ''
        nav_items.append(f'<a href="{base_path}{layer}.html" class="{active}">{layer_name}</a>')

    tags_active = 'active' if current_page == 'tags' else ''
    nav_items.append(f'<a href="{base_path}tags.html" class="{tags_active}">Tags</a>')
    
    nav_html = '\n        '.join(nav_items)
    
    return f"""<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{title} - ArchiMate Architecture</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            line-height: 1.6;
            color: #333;
            background: #f5f5f5;
        }}
        
        header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 1.5rem 0;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        
        .header-content {{
            max-width: 1400px;
            margin: 0 auto;
            padding: 0 2rem;
        }}
        
        h1 {{
            font-size: 2rem;
            margin-bottom: 0.5rem;
        }}
        
        .tag {{
            margin-top: 1rem;
            display: flex;
            flex-wrap: wrap;
            gap: 0.5rem;
        }}
        
        nav a {{
            color: white;
            text-decoration: none;
            text-decoration: none;

        .tag:hover {{
            background: #e0e7ff;
        }}
            padding: 0.5rem 1rem;
            background: rgba(255,255,255,0.1);
            border-radius: 4px;
            transition: all 0.3s;
            font-size: 0.9rem;
        }}
        
        nav a:hover {{
            background: rgba(255,255,255,0.2);
            transform: translateY(-2px);
        }}
        
        nav a.active {{
            background: rgba(255,255,255,0.3);
            font-weight: 600;
        }}
        
        .container {{
            max-width: 1400px;
            margin: 2rem auto;
            padding: 0 2rem;
        }}
        
        .element-grid {{
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 1.5rem;
            margin-top: 2rem;
        }}
        
        .element-card {{
            background: white;
            border-radius: 8px;
            padding: 1.5rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            transition: all 0.3s;
            border-left: 4px solid #667eea;
        }}
        
        .element-card:hover {{
            transform: translateY(-4px);
            box-shadow: 0 4px 16px rgba(0,0,0,0.15);
        }}
        
        .element-card h3 {{
            color: #667eea;
            margin-bottom: 0.5rem;
            font-size: 1.2rem;
        }}
        
        .element-card h3 a {{
            color: inherit;
            text-decoration: none;
        }}
        
        .element-card h3 a:hover {{
            text-decoration: underline;
        }}
        
        .element-type {{
            display: inline-block;
            padding: 0.25rem 0.75rem;
            background: #e0e7ff;
            color: #4c51bf;
            border-radius: 12px;
            font-size: 0.85rem;
            font-weight: 500;
            margin-bottom: 0.5rem;
        }}
        
        .element-description {{
            color: #666;
            font-size: 0.95rem;
            margin-top: 0.5rem;
        }}
        
        .element-detail {{
            background: white;
            border-radius: 8px;
            padding: 2rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }}
        
        .element-detail h2 {{
            color: #667eea;
            margin-bottom: 1rem;
            font-size: 2rem;
        }}
        
        .metadata {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1rem;
            margin: 1.5rem 0;
            padding: 1rem;
            background: #f8f9fa;
            border-radius: 6px;
        }}
        
        .metadata-item {{
            display: flex;
            flex-direction: column;
        }}
        
        .metadata-label {{
            font-weight: 600;
            color: #4a5568;
            font-size: 0.85rem;
            text-transform: uppercase;
            margin-bottom: 0.25rem;
        }}
        
        .metadata-value {{
            color: #2d3748;
        }}
        
        .relations-section {{
            margin-top: 2rem;
        }}
        
        .relations-section h3 {{
            color: #4a5568;
            margin-bottom: 1rem;
            font-size: 1.3rem;
            border-bottom: 2px solid #e2e8f0;
            padding-bottom: 0.5rem;
        }}
        
        .relation-list {{
            list-style: none;
            margin-top: 1rem;
        }}
        
        .relation-item {{
            padding: 1rem;
            background: #f8f9fa;
            border-left: 3px solid #667eea;
            margin-bottom: 0.75rem;
            border-radius: 4px;
        }}
        
        .relation-item.incoming {{
            border-left-color: #48bb78;
        }}
        
        .relation-type {{
            display: inline-block;
            padding: 0.2rem 0.6rem;
            background: #667eea;
            color: white;
            border-radius: 10px;
            font-size: 0.8rem;
            font-weight: 500;
            margin-right: 0.5rem;
        }}
        
        .relation-item.incoming .relation-type {{
            background: #48bb78;
        }}
        
        .relation-item a {{
            color: #667eea;
            text-decoration: none;
            font-weight: 500;
        }}
        
        .relation-item a:hover {{
            text-decoration: underline;
        }}
        
        .relation-description {{
            color: #666;
            font-size: 0.9rem;
            margin-top: 0.25rem;
        }}
        
        .tags {{
            display: flex;
            flex-wrap: wrap;
            gap: 0.5rem;
            margin-top: 1rem;
        }}
        
        .tag {{
            padding: 0.25rem 0.75rem;
            background: #edf2f7;
            color: #4a5568;
            border-radius: 12px;
            font-size: 0.85rem;
        }}
        
        .content-section {{
            margin-top: 2rem;
            line-height: 1.8;
        }}
        
        .content-section h2,
        .content-section h3 {{
            color: #2d3748;
            margin-top: 1.5rem;
            margin-bottom: 0.75rem;
        }}
        
        .content-section ul {{
            margin-left: 2rem;
            margin-top: 0.5rem;
        }}
        
        .content-section code {{
            background: #f7fafc;
            padding: 0.2rem 0.4rem;
            border-radius: 3px;
            font-family: 'Courier New', monospace;
            font-size: 0.9em;
        }}
        
        .breadcrumb {{
            margin-bottom: 1.5rem;
            color: #666;
        }}
        
        .breadcrumb a {{
            color: #667eea;
            text-decoration: none;
        }}
        
        .breadcrumb a:hover {{
            text-decoration: underline;
        }}
        
        .layer-title {{
            font-size: 2.5rem;
            color: #2d3748;
            margin-bottom: 0.5rem;
        }}
        
        .layer-description {{
            color: #666;
            font-size: 1.1rem;
            margin-bottom: 1rem;
        }}
        
        .element-count {{
            color: #666;
            font-size: 1rem;
        }}

        .diagram-section {
            margin: 1.5rem 0 2rem;
            background: #fff;
            border-radius: 12px;
            padding: 1rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
        }

        .diagram-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 1rem;
            margin-bottom: 0.75rem;
        }

        .diagram-header h3 {
            margin: 0;
            font-size: 1.1rem;
            color: #2d3748;
        }

        .diagram-link {
            color: #667eea;
            text-decoration: none;
            font-size: 0.9rem;
        }

        .diagram-link:hover {
            text-decoration: underline;
        }

        .diagram-frame {
            width: 100%;
            height: 520px;
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            background: #f8fafc;
        }
    </style>
</head>
<body>
    <header>
        <div class="header-content">
            <h1>ArchiMate Architecture Repository</h1>
            <nav>
                <a href="{base_path}index.html" class="{'active' if current_page == 'index' else ''}">Home</a>
                {nav_html}
            </nav>
        </div>
    </header>
"""


def generate_html_footer():
    """Generate HTML footer."""
    return """
    <footer style="text-align: center; padding: 2rem; color: #666; font-size: 0.9rem;">
        <p>Generated from ArchiMate elements | © 2026</p>
    </footer>
</body>
</html>
"""


def generate_index_page(registry, output_dir):
    """Generate the main index page."""
    html = generate_html_header("Home", "index")
    
    html += """
    <div class="container">
        <h2 class="layer-title">Architecture Overview</h2>
        <p class="layer-description">
            This repository contains the enterprise architecture organized by ArchiMate layers.
            Each layer represents a different aspect of the architecture, from strategic goals to technical implementation.
        </p>
"""
    
    # Generate statistics by layer
    html += '<div class="element-grid">\n'
    
    for layer in LAYER_ORDER:
        layer_name = LAYER_NAMES.get(layer, layer.title())
        elements = registry.elements_by_layer.get(layer, [])
        count = len(elements)
        
        if count > 0:
            html += f"""
            <div class="element-card">
                <h3><a href="{layer}.html">{layer_name}</a></h3>
                <p class="element-count">{count} element{'s' if count != 1 else ''}</p>
                <p class="element-description">
                    View all {layer} layer elements and their relationships.
                </p>
            </div>
"""
    
    html += '        </div>\n'
    html += '    </div>\n'
    html += generate_html_footer()
    
    output_path = os.path.join(output_dir, 'index.html')
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(html)
    
    print(f"Generated: {output_path}")


def generate_layer_page(layer, registry, output_dir):
    """Generate a page for a specific layer."""
    layer_name = LAYER_NAMES.get(layer, layer.title())
    elements = registry.elements_by_layer.get(layer, [])
    
    if not elements:
        return
    
    html = generate_html_header(layer_name, layer)
    
    html += f"""
    <div class="container">
        <div class="breadcrumb">
            <a href="index.html">Home</a> / {layer_name}
        </div>
        
        <h2 class="layer-title">{layer_name}</h2>
        <p class="element-count">{len(elements)} element{'s' if len(elements) != 1 else ''}</p>

        <div class="diagram-section">
            <div class="diagram-header">
                <h3>{layer_name} Diagram</h3>
                <a class="diagram-link" href="../diagrams/{layer}-layer-mermaid.html" target="_blank" rel="noopener">Open diagram ↗</a>
            </div>
            <iframe class="diagram-frame" src="../diagrams/{layer}-layer-mermaid.html" loading="lazy" title="{layer_name} diagram"></iframe>
        </div>
        
        <div class="element-grid">
"""
    
    # Sort elements by name
    sorted_elements = sorted([registry.get_element(elem_id) for elem_id in elements],
                           key=lambda e: e['name'].lower())
    
    for element in sorted_elements:
        elem_id = element['id']
        name = element['name']
        elem_type = element['type']
        
        # Get first paragraph of content as description
        content = element.get('content', '')
        description = ''
        if content:
            lines = [line.strip() for line in content.split('\n') if line.strip() and not line.strip().startswith('#')]
            if lines:
                description = lines[0][:150]
                if len(lines[0]) > 150:
                    description += '...'
        
        # Count relationships
        outgoing = len(element.get('relationships', []))
        incoming = len(registry.get_incoming_relations(elem_id))
        
        html += f"""
            <div class="element-card">
                <span class="element-type">{elem_type}</span>
                <h3><a href="elements/{elem_id}.html">{name}</a></h3>
                <p class="element-description">{description}</p>
                <p style="margin-top: 0.75rem; font-size: 0.85rem; color: #888;">
                    {outgoing} outgoing, {incoming} incoming relation{'s' if incoming != 1 else ''}
                </p>
            </div>
"""
    
    html += """
        </div>
    </div>
"""
    html += generate_html_footer()
    
    output_path = os.path.join(output_dir, f'{layer}.html')
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(html)
    
    print(f"Generated: {output_path}")


def generate_tags_index_page(registry, output_dir):
    """Generate the tags overview page."""
    tags_index = build_tag_index(registry)
    if not tags_index:
        return

    html = generate_html_header("Tags", "tags")

    html += """
    <div class="container">
        <h2 class="layer-title">Tags</h2>
        <p class="layer-description">
            Browse elements grouped by tag.
        </p>
        <div class="element-grid">
"""

    for tag in sorted(tags_index.keys(), key=lambda t: t.lower()):
        count = len(tags_index[tag])
        tag_slug = slugify_tag(tag)
        html += f"""
            <div class="element-card">
                <span class="element-type">Tag</span>
                <h3><a href="tags/{tag_slug}.html">{tag}</a></h3>
                <p class="element-count">{count} element{'s' if count != 1 else ''}</p>
                <p class="element-description">View elements with the <strong>{tag}</strong> tag.</p>
            </div>
"""

    html += """
        </div>
    </div>
"""

    html += generate_html_footer()

    output_path = os.path.join(output_dir, 'tags.html')
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(html)

    print(f"Generated: {output_path}")


def generate_tag_page(tag, elem_ids, registry, output_dir):
    """Generate a page for a specific tag."""
    base_path = '../'
    html = generate_html_header(f"Tag: {tag}", "tags", base_path)

    html += f"""
    <div class="container">
        <div class="breadcrumb">
            <a href="{base_path}index.html">Home</a> /
            <a href="{base_path}tags.html">Tags</a> /
            {tag}
        </div>

        <h2 class="layer-title">Tag: {tag}</h2>
        <p class="element-count">{len(elem_ids)} element{'s' if len(elem_ids) != 1 else ''}</p>

        <div class="element-grid">
"""

    sorted_elements = sorted(
        [registry.get_element(elem_id) for elem_id in elem_ids],
        key=lambda e: e['name'].lower()
    )

    for element in sorted_elements:
        elem_id = element['id']
        name = element['name']
        elem_type = element['type']

        content = element.get('content', '')
        description = ''
        if content:
            lines = [line.strip() for line in content.split('\n') if line.strip() and not line.strip().startswith('#')]
            if lines:
                description = lines[0][:150]
                if len(lines[0]) > 150:
                    description += '...'

        outgoing = len(element.get('relationships', []))
        incoming = len(registry.get_incoming_relations(elem_id))

        html += f"""
            <div class="element-card">
                <span class="element-type">{elem_type}</span>
                <h3><a href="{base_path}elements/{elem_id}.html">{name}</a></h3>
                <p class="element-description">{description}</p>
                <p style="margin-top: 0.75rem; font-size: 0.85rem; color: #888;">
                    {outgoing} outgoing, {incoming} incoming relation{'s' if incoming != 1 else ''}
                </p>
            </div>
"""

    html += """
        </div>
    </div>
"""
    html += generate_html_footer()

    tags_dir = os.path.join(output_dir, 'tags')
    os.makedirs(tags_dir, exist_ok=True)
    output_path = os.path.join(tags_dir, f'{slugify_tag(tag)}.html')
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(html)

    print(f"Generated: {output_path}")


def generate_element_page(elem_id, registry, output_dir):
    """Generate a page for a specific element."""
    element = registry.get_element(elem_id)
    if not element:
        return
    
    name = element['name']
    elem_type = element['type']
    layer = element['layer']
    layer_name = LAYER_NAMES.get(layer, layer.title())
    content = element.get('content', '')
    properties = element.get('properties', {})
    tags = element.get('tags', [])
    relationships = element.get('relationships', [])
    incoming_relations = registry.get_incoming_relations(elem_id)
    
    base_path = '../'
    html = generate_html_header(name, '', base_path)
    
    html += f"""
    <div class="container">
        <div class="breadcrumb">
            <a href="{base_path}index.html">Home</a> / 
            <a href="{base_path}{layer}.html">{layer_name}</a> / 
            {name}
        </div>
        
        <div class="element-detail">
            <span class="element-type">{elem_type}</span>
            <h2>{name}</h2>
            
            <div class="metadata">
                <div class="metadata-item">
                    <span class="metadata-label">ID</span>
                    <span class="metadata-value">{elem_id}</span>
                </div>
                <div class="metadata-item">
                    <span class="metadata-label">Type</span>
                    <span class="metadata-value">{elem_type}</span>
                </div>
                <div class="metadata-item">
                    <span class="metadata-label">Layer</span>
                    <span class="metadata-value"><a href="{base_path}{layer}.html">{layer_name}</a></span>
                </div>
"""
    
    # Add custom properties
    for prop_key, prop_value in properties.items():
        html += f"""
                <div class="metadata-item">
                    <span class="metadata-label">{prop_key.replace('_', ' ').replace('-', ' ').title()}</span>
                    <span class="metadata-value">{prop_value}</span>
                </div>
"""
    
    html += '            </div>\n'
    
    # Add tags
    if tags:
        html += '            <div class="tags">\n'
        for tag in tags:
            tag_slug = slugify_tag(tag)
            html += f'                <a class="tag" href="{base_path}tags/{tag_slug}.html">{tag}</a>\n'
        html += '            </div>\n'
    
    # Add outgoing relationships
    if relationships:
        html += """
            <div class="relations-section">
                <h3>Outgoing Relationships</h3>
                <ul class="relation-list">
"""
        for rel in relationships:
            rel_type = rel.get('type', 'unknown')
            target_id = rel.get('target', '')
            description = rel.get('description', '')
            
            target_element = registry.get_element(target_id)
            if target_element:
                target_name = target_element['name']
                target_layer = target_element['layer']
                html += f"""
                    <li class="relation-item">
                        <span class="relation-type">{rel_type}</span>
                        <a href="{target_id}.html">{target_name}</a>
                        <span style="color: #888; font-size: 0.85rem;">({target_layer})</span>
"""
                if description:
                    html += f'                        <div class="relation-description">{description}</div>\n'
                html += '                    </li>\n'
            else:
                html += f"""
                    <li class="relation-item">
                        <span class="relation-type">{rel_type}</span>
                        <span>{target_id}</span>
"""
                if description:
                    html += f'                        <div class="relation-description">{description}</div>\n'
                html += '                    </li>\n'
        
        html += """
                </ul>
            </div>
"""
    
    # Add incoming relationships
    if incoming_relations:
        html += """
            <div class="relations-section">
                <h3>Incoming Relationships</h3>
                <ul class="relation-list">
"""
        for rel in incoming_relations:
            rel_type = rel.get('type', 'unknown')
            source_id = rel.get('source_id', '')
            description = rel.get('description', '')
            
            source_element = registry.get_element(source_id)
            if source_element:
                source_name = source_element['name']
                source_layer = source_element['layer']
                html += f"""
                    <li class="relation-item incoming">
                        <span class="relation-type">{rel_type}</span>
                        <a href="{source_id}.html">{source_name}</a>
                        <span style="color: #888; font-size: 0.85rem;">({source_layer})</span>
"""
                if description:
                    html += f'                        <div class="relation-description">{description}</div>\n'
                html += '                    </li>\n'
            else:
                html += f"""
                    <li class="relation-item incoming">
                        <span class="relation-type">{rel_type}</span>
                        <span>{source_id}</span>
"""
                if description:
                    html += f'                        <div class="relation-description">{description}</div>\n'
                html += '                    </li>\n'
        
        html += """
                </ul>
            </div>
"""
    
    # Add content
    if content:
        # Convert markdown to HTML
        md = markdown.Markdown(extensions=['extra', 'codehilite'])
        content_html = md.convert(content)
        
        html += f"""
            <div class="content-section">
                {content_html}
            </div>
"""
    
    html += """
        </div>
    </div>
"""
    html += generate_html_footer()
    
    # Create elements subdirectory if needed
    elements_dir = os.path.join(output_dir, 'elements')
    os.makedirs(elements_dir, exist_ok=True)
    
    output_path = os.path.join(elements_dir, f'{elem_id}.html')
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(html)


def main():
    """Main function to generate the website."""
    import sys
    
    # Determine paths
    script_dir = Path(__file__).parent
    project_dir = script_dir.parent
    elements_dir = project_dir / 'elements'
    
    # Output directory
    if len(sys.argv) > 1:
        output_dir = Path(sys.argv[1])
    else:
        output_dir = project_dir / 'output' / 'website'
    
    # Create output directory
    os.makedirs(output_dir, exist_ok=True)
    
    print(f"Reading elements from: {elements_dir}")
    print(f"Output directory: {output_dir}")
    print()
    
    # Read all elements
    print("Reading elements...")
    registry = read_elements(elements_dir)
    
    total_elements = len(registry.elements)
    print(f"Found {total_elements} elements")
    print()
    
    # Generate pages
    print("Generating pages...")
    
    # Generate index
    generate_index_page(registry, output_dir)
    
    # Generate layer pages
    for layer in LAYER_ORDER:
        if registry.elements_by_layer.get(layer):
            generate_layer_page(layer, registry, output_dir)

    # Generate tag pages
    tags_index = build_tag_index(registry)
    if tags_index:
        generate_tags_index_page(registry, output_dir)
        for tag, elem_ids in sorted(tags_index.items(), key=lambda item: item[0].lower()):
            generate_tag_page(tag, elem_ids, registry, output_dir)
    
    # Generate individual element pages
    print(f"\nGenerating {total_elements} element pages...")
    for elem_id in registry.elements:
        generate_element_page(elem_id, registry, output_dir)
    
    print(f"\n✓ Website generated successfully in: {output_dir}")
    print(f"  Open {output_dir}/index.html in your browser")


if __name__ == '__main__':
    main()
