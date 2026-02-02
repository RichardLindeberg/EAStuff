# Interactive Mermaid Diagram Guide

## Overview

The Mermaid generator creates interactive, clickable diagrams from your ArchiMate elements with links to their markdown documentation.

## Features

### 🖱️ Clickable Elements
Every element in the diagram is clickable. When you click an element, it will open the corresponding markdown file with full documentation.

### 🎨 Color-Coded Layers
Elements are automatically color-coded by their ArchiMate layer:

- **Strategy Layer** - Light Orange (#FFF4E6)
- **Business Layer** - Light Yellow (#FFF9E6)
- **Application Layer** - Light Blue (#E6F3FF)
- **Technology Layer** - Light Green (#E6FFE6)
- **Physical Layer** - Light Gray (#F0F0F0)
- **Motivation Layer** - Light Pink (#FFE6F0)
- **Implementation Layer** - Light Purple (#F5E6FF)

### 📐 Element Shapes
Different ArchiMate element types are rendered with appropriate shapes:

- **Rectangles** `[]` - Application components
- **Rounded boxes** `()` - Business elements
- **Stadiums** `([])` - Technology elements
- **Hexagons** `{{}}` - Strategy elements
- **Circles** `(())` - Stakeholders
- **Trapezoids** `[//]` - Physical elements
- **Subroutines** `[[]]` - Implementation elements
- **Cylinders** `[()]` - Data objects

## Output Formats

### HTML Format (Default)
```bash
python scripts/generators/generate_mermaid.py
```

**Benefits:**
- Self-contained - works in any browser
- No installation required (uses CDN)
- Fully interactive and clickable
- Professional styling included

**Use cases:**
- Sharing with stakeholders
- Presentations
- Documentation websites
- Internal wikis

### Markdown Format
```bash
python scripts/generators/generate_mermaid.py --md
```

**Benefits:**
- Works on GitHub (automatic rendering)
- VS Code preview with Mermaid extension
- Portable and version-control friendly
- Easy to embed in documentation

**Use cases:**
- GitHub repositories
- README files
- GitBook/MkDocs documentation
- Collaborative editing

## Usage Examples

### Generate Full Architecture
```bash
python scripts/generators/generate_mermaid.py
# Output: diagrams/full-architecture-mermaid.html
```

Open the HTML file in your browser and click on any element to view its documentation.

### Generate Layer Diagram
```bash
python scripts/generators/generate_mermaid.py --layer application
# Output: diagrams/application-layer-mermaid.html
```

This creates a focused view of just the application layer with all connections.

### Generate Context Diagram
```bash
python scripts/generators/generate_mermaid.py --element app-comp-customer-portal-001 2
# Output: diagrams/app-comp-customer-portal-001-context-mermaid.html
```

The `2` parameter sets the depth - how many relationship hops to include. The focal element is highlighted in blue.

### Generate Markdown Version
```bash
python scripts/generators/generate_mermaid.py --element app-comp-customer-portal-001 --md
# Output: diagrams/app-comp-customer-portal-001-context-mermaid.md
```

Perfect for embedding in GitHub README or documentation.

## How Clickable Links Work

### In HTML Files
```mermaid
click element_id "../elements/application/customer-portal.md" "View details"
```

When clicked, the browser navigates to the relative path of the markdown file.

### In Markdown Files
```mermaid
click element_id "elements/application/customer-portal.md" "View details"
```

GitHub and other markdown renderers handle the links automatically.

## Relationship Styles

The generator uses different arrow styles for different relationship types:

- `-->` Solid arrow - Composition, serving, access, flow, triggering
- `-.->` Dotted arrow - Assignment, influence
- `==>` Thick arrow - Realization
- `---` Plain line - Association

## Integration with Existing Tools

The Mermaid generator works alongside the PlantUML generator:

| Feature | PlantUML | Mermaid |
|---------|----------|---------|
| Installation | Requires Java + PlantUML | No installation (CDN) |
| Clickable | Limited | Full support |
| Browser viewing | Needs export | Native HTML |
| GitHub rendering | No | Yes (markdown) |
| Styling | Extensive | Good |
| Complex layouts | Excellent | Good |

**Recommendation:** Use PlantUML for complex, publication-quality diagrams. Use Mermaid for interactive, shareable web documentation.

## Viewing the Diagrams

### HTML Files
1. Navigate to the `diagrams/` folder
2. Double-click any `*-mermaid.html` file
3. Your browser will open and render the diagram
4. Click on elements to navigate to their documentation

### Markdown Files
**On GitHub:**
- Just view the `.md` file - Mermaid renders automatically
- Elements are clickable

**In VS Code:**
1. Install "Markdown Preview Mermaid Support" extension
2. Open the `.md` file
3. Click the preview button (Ctrl+Shift+V)

**In Other Tools:**
- GitBook, MkDocs, Docusaurus all support Mermaid natively

## Tips and Best Practices

### For Presentations
- Generate HTML versions
- Open in browser full-screen (F11)
- Click through elements to show details
- Professional and interactive

### For Documentation
- Generate markdown versions
- Commit to Git repository
- Automatic rendering on GitHub
- Keep diagrams in sync with code

### For Exploration
- Use context diagrams with depth 2-3
- Navigate through the architecture
- Understand dependencies
- Find impact of changes

### For Stakeholders
- HTML files are self-contained
- No technical knowledge needed
- Just open and click
- Professional appearance

## Troubleshooting

### Links Not Working in HTML
- Ensure relative paths are correct
- Check that markdown files exist
- Verify file permissions

### Diagram Not Rendering
- Check internet connection (needs CDN)
- Verify Mermaid syntax with `--md` first
- Look for syntax errors in terminal output

### Elements Missing
- Run `python scripts/generators/generate_mermaid.py --list`
- Verify element has correct YAML frontmatter
- Check that element files are in `elements/` directory

## Advanced Usage

### Custom Colors
Edit the `_get_layer_colors()` method in `generate_mermaid.py` to customize layer colors.

### Custom Shapes
Modify `element_to_shape` dictionary to change element shapes by type.

### Custom Styling
For HTML output, edit the `_wrap_in_html()` method to add custom CSS.

### Integration
The generator returns file paths, so you can integrate it into build scripts:

```python
from generator.generate_mermaid import MermaidGenerator

gen = MermaidGenerator('elements', output_format='html')
output = gen.generate_full_diagram('output.html', 'My Architecture')
print(f"Generated: {output}")
```

## Next Steps

1. **Generate your first diagram**: `python scripts/generators/generate_mermaid.py`
2. **Open it in browser**: Double-click the HTML file
3. **Click around**: Explore your architecture interactively
4. **Share it**: Email the HTML file or commit markdown to Git
5. **Iterate**: Keep your diagrams in sync as elements change

## Comparison with PlantUML

Both generators read the same element files, so you can use both:

```bash
# Generate PlantUML for publication
python scripts/generators/generate_puml.py

# Generate Mermaid for interactive web
python scripts/generators/generate_mermaid.py

# Both are always in sync!
```

The choice depends on your use case:
- **PlantUML**: Better for printed documents, complex layouts, pixel-perfect control
- **Mermaid**: Better for web, GitHub, interactive exploration, ease of use
