# Interactive Web UI Guide

This project now uses the F# server to render the architecture dynamically. The prior Mermaid generator has been removed.

## What You Get

- Layer browsing (strategy, business, application, technology, motivation, etc.)
- Element detail pages with relationships
- Tag browsing and filtering
- Relationship validation warnings based on schemas/relations.xml

## Run the Server

From the fsharp-server directory:

```bash
dotnet build
dotnet run
```

Open http://localhost:5000 in your browser.

## Key Routes

- / or /index.html - Architecture overview
- /{layer}.html - Layer view (example: /business.html)
- /elements/{id}.html - Element details
- /tags.html - All tags
- /tags/{tag}.html - Filter by tag

## Development Tips

- Use dotnet watch run for hot reload during development
- Pass a custom elements path: dotnet run -- /path/to/elements

## Troubleshooting

- Elements not loading: confirm YAML frontmatter includes id, name, type, layer
- Missing relationships: verify target IDs exist
- Port in use: set ASPNETCORE_URLS to a different port