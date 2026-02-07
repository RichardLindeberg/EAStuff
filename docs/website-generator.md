# Website Generator Status

The static website generator has been removed. The current, supported experience is the F# server, which renders architecture pages dynamically.

## Use the F# Server

```bash
cd fsharp-server
dotnet build
dotnet run
```

Open http://localhost:5000 to browse the architecture.

## Need Static Output?

If you require static exports, use external tools or generate diagrams manually and link them in docs. Static export tooling is not currently part of this repository.