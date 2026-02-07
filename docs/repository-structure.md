# Repository Structure

Updated: February 7, 2026

## Directory Layout

```
EA Stuff/
├── elements/                       # EA element definitions
│   ├── strategy/                   # Strategy layer elements
│   ├── business/                   # Business layer elements
│   ├── application/                # Application layer elements
│   ├── technology/                 # Technology layer elements
│   ├── physical/                   # Physical layer elements (optional)
│   ├── motivation/                 # Motivation layer elements
│   └── implementation/             # Implementation layer elements (optional)
│
├── schemas/                        # Validation schemas
│   └── archimate-3.2-schema.yaml   # ArchiMate 3.2 specification
│
├── docs/                           # Documentation
│   ├── quick-start.md
│   ├── mermaid-guide.md            # F# server UI guide
│   ├── element-types-reference.md
│   └── best-practices.md
│
├── fsharp-server/                  # F# web server
│   ├── Program.fs
│   ├── Handlers.fs
│   ├── Views.fs
│   └── EAArchive.fsproj
│
├── examples/                       # Example files
│   ├── invalid-missing-fields.md
│   └── invalid-wrong-layer.md
│
├── README.md                       # Main documentation
└── requirements.txt                # Legacy file (unused)

```

## Usage

Run the F# server from the fsharp-server directory:

```bash
dotnet build
dotnet run
```