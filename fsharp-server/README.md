# F# ArchiMate Web Server with Giraffe and HTMX

A modern F# webserver that dynamically renders your ArchiMate Enterprise Architecture from markdown files, with interactive HTMX support.

## Features

- **Giraffe Web Framework**: Type-safe HTTP handler composition
- **HTMX Integration**: Interactive UI without complex JavaScript
- **Dynamic Content Loading**: Renders architecture elements from markdown files
- **Full Layer Support**: Strategy, Motivation, Business, Application, Technology, Physical, and Implementation layers
- **Relationship Mapping**: Visualizes incoming and outgoing relationships between elements
- **Tag System**: Browse and filter elements by tags
- **Responsive Design**: Modern, mobile-friendly interface

## Prerequisites

- .NET 8.0 SDK or later
- F# 8.0

## Project Structure

```
├── Domain.fs                 # Type definitions for elements and layers
├── ElementRegistry.fs        # Element loading and relationship tracking
├── Views.fs                  # HTMX-enabled HTML view generation
├── Handlers.fs              # Giraffe HTTP request handlers
├── Program.fs               # Application startup and configuration
└── EAArchive.fsproj        # Project configuration
```

## Building

```bash
cd fsharp-server
dotnet build
```

## Running

### Development

```bash
dotnet run
```

The server will start on `http://localhost:5000` by default.

### With Custom Elements Path

```bash
dotnet run -- /path/to/elements
```

## How It Works

### Element Loading

The application automatically discovers and loads all markdown files from the `elements/` directory. Each markdown file should contain YAML frontmatter with element metadata:

```yaml
---
id: bus-actr-001
name: Corporate Customer
type: Actor
layer: business
tags: customer, external, primary
relationships:
  - target: bus-objt-005
    type: interacts-with
    description: Engages with customer profile
---
Element content in markdown...
```

### Routes

- `/` or `/index.html` - Architecture overview with layer summaries
- `/{layer}.html` - View all elements in a specific layer (e.g., `/business.html`)
- `/elements/{id}.html` - Detailed view of a specific element with relationships
- `/tags.html` - Index of all tags
- `/tags/{tag}.html` - View all elements with a specific tag

### HTMX Integration

The interface includes HTMX for interactive features:

- Smooth page transitions
- Dynamic content swapping
- Relationship browsing without page reloads
- Tag filtering

Add `hx-boost="true"` to links to enable HTMX-powered navigation:

```html
<a href="/elements/bus-actr-001.html" hx-boost="true">
  View element
</a>
```

## Styling

The application includes a built-in CSS stylesheet with:

- Modern gradient header
- Card-based layout for elements
- Color-coded relationship types
- Responsive grid system
- Smooth animations and transitions

Customize styling by modifying the `stylesheet` variable in `Views.fs`.

## Extending

### Adding Custom Routes

In `Handlers.fs`, add routes to the `createHandlers` function:

```fsharp
let customHandler : HttpHandler =
    fun next ctx ->
        // Your handler logic
        htmlView someView next ctx

// In createHandlers:
choose [
    route "/custom" >=> customHandler
    // ... existing routes
]
```

### Custom Views

Create new view functions in `Views.fs` following the pattern:

```fsharp
let customPage (data: SomeType) : XmlNode =
    htmlPage "Title" "page-name" [
        div [_class "container"] [
            // Your content
        ]
    ]
```

## Performance Considerations

- Elements are loaded once at startup
- Registry is immutable and thread-safe
- HTML views are composed on-demand but are deterministic
- No database required - pure file-based architecture

## Deployment

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["dotnet", "EAArchive.dll"]
```

### IIS

Publish as a self-contained deployment:

```bash
dotnet publish -c Release -r win-x64 -p:SelfContained=true
```

Then configure IIS to use the output folder.

## Comparison with Python Generator

This F# implementation offers several advantages over the static Python generator:

| Feature | Python | F# |
|---------|--------|---|
| Dynamic rendering | ❌ Static generation | ✅ Real-time |
| Interactive UI | ❌ Basic HTML | ✅ HTMX-powered |
| Performance | One-time build | On-demand rendering |
| Memory | Disk-based output | Efficient registry |
| Type Safety | Dynamic | ✅ Full F# typing |
| Scalability | File-based | Web server |

## Dependencies

- `Giraffe` - HTTP handler composition and routing
- `Giraffe.ViewEngine` - Type-safe HTML generation
- `YamlDotNet` - YAML frontmatter parsing
- `FSharp.Data.Markdown` - Markdown parsing

## License

Same as parent project

## Contributing

1. Create a new branch
2. Make your changes
3. Test locally with `dotnet run`
4. Submit a pull request

## Troubleshooting

### Elements Not Loading

- Ensure the elements path is correct (default: `../elements`)
- Check that markdown files have valid YAML frontmatter with `id` field
- Check console output for parsing errors

### Port Already in Use

Change the port in `Program.fs` or set via environment:

```bash
set ASPNETCORE_URLS=http://localhost:5001
dotnet run
```

### HTMX Not Working

Ensure the HTMX script is loaded from the CDN in `Views.fs`. The default includes:

```html
<script src="https://unpkg.com/htmx.org@1.9.10"></script>
```
