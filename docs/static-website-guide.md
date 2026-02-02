# Static Website Generation Guide

## Quick Start

Generate a complete static website with interactive diagrams and element pages:

```bash
python scripts/generators/generate_website.py
```

Then open `index.html` in your browser!

## What Gets Generated

### 1. Interactive Diagrams (`diagrams/`)

- `full-architecture-mermaid.html` - Complete architecture view
- `application-layer-mermaid.html` - Application layer view
- `business-layer-mermaid.html` - Business layer view
- `technology-layer-mermaid.html` - Technology layer view
- `strategy-layer-mermaid.html` - Strategy layer view
- `motivation-layer-mermaid.html` - Motivation layer view
- Element context diagrams (when generated individually)

### 2. Element Documentation (`elements/`)

HTML pages for each element in your architecture:
- `elements/application/*.html`
- `elements/business/*.html`
- `elements/technology/*.html`
- `elements/strategy/*.html`
- `elements/motivation/*.html`
- etc.

### 3. Index Page

`index.html` - Landing page with links to all diagrams

## Features

### Interactive Navigation

```
index.html
    ↓ Click on diagram card
diagram.html (Mermaid visualization)
    ↓ Click on element
element.html (Detailed documentation)
    ↓ Click on related element
another-element.html
    ↓ Click "Back to Architecture"
diagram.html
```

### Element Pages Include

- **Header**: Name, type, layer, ID, tags
- **Content**: Full markdown documentation with formatting
- **Properties**: Table of all element properties
- **Relationships**: Clickable links to related elements
- **Navigation**: Back to architecture diagrams

### Diagram Pages Include

- **Interactive Mermaid diagram**: Clickable elements
- **Color coding**: Elements colored by layer
- **Tooltips**: Relationship descriptions
- **Responsive**: Works on mobile and desktop

## Manual Generation

### Generate Specific Diagrams with Element Pages

```bash
# Full architecture with element HTML
python scripts/generators/generate_mermaid.py --html-elements

# Specific layer with element HTML
python scripts/generators/generate_mermaid.py --layer application --html-elements

# Element context diagram with element HTML
python scripts/generators/generate_mermaid.py --element app-comp-customer-portal-001 2 --html-elements
```

### Generate Only Diagrams (No Element HTML)

```bash
# Full architecture (links to .md files)
python scripts/generators/generate_mermaid.py

# Specific layer
python scripts/generators/generate_mermaid.py --layer application
```

### Generate Markdown Diagrams for GitHub

```bash
# Markdown output
python scripts/generators/generate_mermaid.py --md
```

## Deployment

The generated website is completely static and can be deployed anywhere:

### Local Viewing
Just open `index.html` in any browser!

### GitHub Pages
1. Push files to a GitHub repository
2. Enable GitHub Pages in repository settings
3. Set source to root or `/docs` folder
4. Access at `https://username.github.io/repository/`

### Web Server
Upload all files to any web server:
- No server-side processing required
- No database needed
- Works with any static hosting

### Zip and Share
Archive the entire directory and share:
- Recipients can unzip and open `index.html`
- Everything works offline
- No installation required

## Customization

### Update Element Styling

Edit `_convert_element_to_html()` in `scripts/generators/generate_mermaid.py`:

```python
# Change colors, fonts, spacing
.header {
    border-left: 4px solid {layer_color};
    padding-left: 20px;
}
```

### Update Diagram Styling

Edit `_wrap_in_html()` in `scripts/generators/generate_mermaid.py`:

```python
# Mermaid initialization options
mermaid.initialize({ 
    startOnLoad: true,
    theme: 'default',  # Try 'dark', 'forest', 'neutral'
    securityLevel: 'loose'
});
```

### Customize Index Page

Edit `index.html` directly to:
- Change colors and layout
- Add company branding
- Include additional navigation
- Add custom content

## Workflow

### Development Workflow

1. **Create/Update Elements**: Edit markdown files in `elements/`
2. **Validate**: `python scripts/validator/validate.py`
3. **Generate Website**: `python scripts/generators/generate_website.py`
4. **Preview**: Open `index.html` in browser
5. **Iterate**: Make changes and regenerate

### Continuous Updates

```bash
# Quick regeneration script
python scripts/generators/generate_website.py && start index.html
```

Or create a batch file `generate-and-open.bat`:
```batch
@echo off
python scripts/generators/generate_website.py
start index.html
```

## Troubleshooting

### Elements Not Clickable
- Make sure you used `--html-elements` flag
- Check that element HTML files were generated in `elements/`
- Verify links point to `.html` not `.md`

### Diagram Not Rendering
- Check browser console for errors
- Ensure internet connection (needs Mermaid.js CDN)
- Try different browser

### Broken Links
- Regenerate with `--html-elements` flag
- Check that referenced elements exist
- Validate elements first: `python scripts/validator/validate.py`

### Missing Diagrams
- Some layers may not have elements yet
- Check for errors in terminal output
- Verify layer names are correct

## Tips

### Best Practices

1. **Always validate first**: `python scripts/validator/validate.py`
2. **Use the website generator**: Easier than individual commands
3. **Commit HTML files to git**: Keep generated site in version control
4. **Regenerate after changes**: Website won't update automatically
5. **Test in browser**: Preview before sharing

### Performance

- Static site loads instantly
- No server processing overhead
- Works offline after initial load
- Scales to hundreds of elements

### Accessibility

- Semantic HTML structure
- Proper heading hierarchy
- Color contrast ratios met
- Keyboard navigation supported

## Examples

### Complete Documentation Site

```bash
# Generate everything
python scripts/generators/generate_website.py

# Upload to web server
rsync -av *.html diagrams/ elements/ user@server:/var/www/ea-docs/
```

### Presentation Mode

```bash
# Generate just full architecture
python scripts/generators/generate_mermaid.py --html-elements

# Open in browser fullscreen (F11)
start diagrams/full-architecture-mermaid.html
```

### GitHub Documentation

```bash
# Generate markdown versions for GitHub
python scripts/generators/generate_mermaid.py --md
python scripts/generators/generate_mermaid.py --layer application --md

# Commit to repository
git add diagrams/*.md
git commit -m "Update architecture diagrams"
git push
```

## Advanced Usage

### Custom Generation Script

Create your own generator combining options:

```python
from scripts.generators.generate_mermaid import MermaidGenerator
from pathlib import Path

workspace = Path('.')
generator = MermaidGenerator(
    str(workspace / 'elements'),
    output_format='html',
    generate_element_html=True
)

# Generate custom views
generator.load_elements()
generator.generate_full_diagram('custom-view.html', 'Custom Architecture View')
generator.generate_element_html_files(workspace)
```

### Integrate with CI/CD

```yaml
# .github/workflows/generate-docs.yml
name: Generate Architecture Docs
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Setup Python
        uses: actions/setup-python@v2
      - name: Install dependencies
        run: pip install -r requirements.txt
      - name: Generate website
        run: python scripts/generators/generate_website.py
      - name: Deploy to GitHub Pages
        uses: peaceiris/actions-gh-pages@v3
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_dir: .
```
