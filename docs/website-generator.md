# Website Generator

This script generates a static HTML website from your ArchiMate element markdown files.

## Features

- **Layer-based Organization**: Elements are grouped by ArchiMate layers (Strategy, Motivation, Business, Application, Technology, etc.)
- **Incoming & Outgoing Relations**: Each element page shows both outgoing and incoming relationships
- **Navigation**: Easy navigation between layers and elements
- **Responsive Design**: Modern, clean UI that works on desktop and mobile
- **Metadata Display**: Shows all element properties, tags, and descriptions
- **Markdown Support**: Element content is rendered from markdown

## Usage

```bash
# Activate virtual environment
source .venv/bin/activate

# Generate website to default location (output/website/)
python3 scripts/generate_website.py

# Or specify a custom output directory
python3 scripts/generate_website.py /path/to/output/dir
```

## Output Structure

```
output/website/
├── index.html              # Home page with layer overview
├── strategy.html           # Strategy layer elements
├── motivation.html         # Motivation layer elements
├── business.html           # Business layer elements
├── application.html        # Application layer elements
├── technology.html         # Technology layer elements
└── elements/               # Individual element pages
    ├── elem-id-001.html
    ├── elem-id-002.html
    └── ...
```

## Element Page Contents

Each element page includes:

1. **Header Navigation**: Quick links to all layers
2. **Breadcrumb**: Shows current location in site hierarchy
3. **Element Metadata**: ID, type, layer, and custom properties
4. **Tags**: All element tags
5. **Outgoing Relationships**: Relations defined in the element
6. **Incoming Relationships**: Relations from other elements to this one
7. **Content**: Full markdown content rendered as HTML

## Relationship Display

- **Outgoing Relations**: Purple badges, listed in the element's frontmatter
- **Incoming Relations**: Green badges, automatically discovered by scanning all elements
- Each relation shows:
  - Relationship type (realization, composition, influence, etc.)
  - Target/source element name (linked)
  - Layer of the target/source
  - Description (if available)

## Viewing the Website

Simply open the generated `index.html` file in your web browser:

```bash
# On Linux
xdg-open output/website/index.html

# On macOS
open output/website/index.html

# On Windows
start output/website/index.html
```

Or use any local web server:

```bash
# Python 3
cd output/website
python3 -m http.server 8000

# Then open http://localhost:8000 in your browser
```

## Dependencies

Required Python packages (installed automatically with the project):
- `python-frontmatter`: Parse markdown frontmatter
- `markdown`: Convert markdown to HTML

## Customization

The generator includes inline CSS in each page. To customize the styling:

1. Edit the `generate_html_header()` function in `scripts/generate_website.py`
2. Modify the CSS within the `<style>` tag
3. Re-run the generator

## Differences from ArchiMate Exchange Generator

This website generator is complementary to the `generate_archimate_exchange.py` script:

| Feature | Website Generator | Exchange Generator |
|---------|-------------------|-------------------|
| Output | HTML website | XML exchange file |
| Focus | Documentation & browsing | Tool interchange |
| Diagrams | Not included | Not included |
| Relations | Shows incoming & outgoing | Exports all relations |
| Format | Human-readable | Machine-readable |
| Use case | Team documentation | Import to ArchiMate tools |
