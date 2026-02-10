# Static Website Status

Static website generation has been removed. The F# server is now the only supported way to render and browse the architecture.

## Use the F# Server

```bash
cd src/fsharp-server
dotnet build
dotnet run
```

Open http://localhost:5000 to browse the architecture.

## Need Static Output?

If you require static exports, use external tools or generate diagrams manually and link them in docs. Static export tooling is not currently part of this repository.

For deployment guidance, see [deployment-guide.md](deployment-guide.md).
