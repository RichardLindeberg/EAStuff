# EAArchive F# Server

## Quick Start

1. **Build the project**
   ```bash
   dotnet build
   ```

2. **Run the server**
   ```bash
   dotnet run
   ```

3. **Open in browser**
   Navigate to `http://localhost:5000`

## Architecture

The F# server dynamically renders your ArchiMate elements from markdown files with:

- **Type-safe architecture**: Full F# domain model
- **Giraffe web framework**: Composable HTTP handlers  
- **HTMX integration**: Interactive UI for browsing relationships
- **Real-time rendering**: No static file generation needed

## Legacy Note

Static Python generators have been removed. Use the F# server for all browsing and validation workflows.

## Project Files

- **Domain.fs** - Core types: Element, Relationship, LayerInfo
- **ElementRegistry.fs** - Loads markdown files, parses YAML frontmatter, builds relationship index
- **Views.fs** - Generates HTML using Giraffe.ViewEngine and HTMX
- **Handlers.fs** - HTTP request handlers for all routes
- **Program.fs** - Application startup, loads elements into registry

## Routes

- `GET /` - Home page with layer overview
- `GET /{layer}.html` - Browse all elements in a layer
- `GET /elements/{id}.html` - View element with relationships
- `GET /tags.html` - Browse all tags
- `GET /tags/{tag}.html` - Filter elements by tag

## Adding HTMX Interactivity

The views already include HTMX script. To add interactive features:

```html
<!-- Swap relationship details on click -->
<button hx-get="/api/element/{id}/relations" 
        hx-target="#relations">
  Show Relations
</button>

<!-- Infinite scroll on element lists -->
<div hx-get="/api/layer/{layer}?page=2" 
     hx-trigger="revealed" 
     hx-swap="beforeend">
</div>
```

## Development

Edit any F# file and rebuild:

```bash
dotnet watch run
```

This will watch for changes and recompile automatically.

## Element Frontmatter Format

```yaml
---
id: bus-actr-001
name: Corporate Customer  
type: Actor
layer: business
tags: customer, stakeholder
relationships:
  - target: bus-objt-005
    type: interacts-with
    description: Engages with customer accounts
---
# Markdown content here
```

## Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Set to `Development` or `Production`
- `ASPNETCORE_URLS` - Override server URL (e.g., `http://localhost:5001`)

## Building for Production

```bash
dotnet publish -c Release -r win-x64
```

Output will be in `bin/Release/net8.0/win-x64/publish/`

The server expects external data only. Set `EAArchive:ElementsPath` in `appsettings.Production.json` to an absolute path containing the ArchiMate markdown files.
