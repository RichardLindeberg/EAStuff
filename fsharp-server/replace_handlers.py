#!/usr/bin/env python3
import re
import sys

# Read the file
with open('Handlers.fs', 'r', encoding='utf-8') as f:
    content = f.read()

# Replace wrapCytoscapeHtml function
cyt_pattern = r'let private wrapCytoscapeHtml \(title: string\) \(data: string\) \(enableSave: bool\) : string =.*?sprintf """<!DOCTYPE html.*?</html>""" title data saveScript'
cyt_replacement = '''let private wrapCytoscapeHtml (title: string) (data: string) (enableSave: bool) : string =
        sprintf """<!DOCTYPE html
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>%s</title>
    <link rel="stylesheet" href="/css/cytoscape-diagram.css" />
    <script src="https://cdn.jsdelivr.net/npm/cytoscape@3.26.0/dist/cytoscape.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/dagre@0.8.5/dist/dagre.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/cytoscape-dagre@2.5.0/cytoscape-dagre.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/lodash@4.17.21/lodash.min.js"></script>
</head>
<body>
    <div id="cy"></div>
    
    <div class="controls">
        <button id="fitView">Fit to View</button>
        <button id="zoomIn">Zoom In</button>
        <button id="zoomOut">Zoom Out</button>
        <button id="resetLayout">Reset Layout</button>
        <button id="exportPNG">Export PNG</button>
    </div>
    
    <div class="legend">
        <h4>Relationships</h4>
        <div class="legend-item">
            <div class="legend-line" style="background: #0066cc;"></div>
            <span>Serving</span>
        </div>
        <div class="legend-item">
            <div class="legend-line" style="background: #cc6600;"></div>
            <span>Realization</span>
        </div>
        <div class="legend-item">
            <div class="legend-line" style="background: #333333;"></div>
            <span>Composition</span>
        </div>
        <div class="legend-item">
            <div class="legend-line" style="background: #cc3366; border-top: 2px dashed;"></div>
            <span>Influence</span>
        </div>
    </div>
    
    <script>
        const graphData = %s;
    </script>
    <script src="/js/cytoscape-diagram.js"></script>
</body>
</html>""" title data'''

content = re.sub(cyt_pattern, cyt_replacement, content, flags=re.DOTALL)

# Replace wrapMermaidHtml function
mer_pattern = r'let private wrapMermaidHtml \(title: string\) \(diagram: string\) : string =.*?sprintf """<!DOCTYPE html.*?</html>""" title title diagram'
mer_replacement = '''let private wrapMermaidHtml (title: string) (diagram: string) : string =
        sprintf """<!DOCTYPE html
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>%s</title>
    <link rel="stylesheet" href="/css/mermaid-diagram.css" />
    <script src="https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/svg-pan-zoom@3.6.1/dist/svg-pan-zoom.min.js"></script>
</head>
<body>
    <div class="container">
        <h1>%s</h1>
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
            <div id="diagram-container">
                <div class="mermaid">%s</div>
            </div>
        </div>
    </div>
    <script src="/js/mermaid-diagram.js"></script>
</body>
</html>""" title title diagram'''

content = re.sub(mer_pattern, mer_replacement, content, flags=re.DOTALL)

# Write back
with open('Handlers.fs', 'w', encoding='utf-8') as f:
    f.write(content)

print("Replacement complete")
