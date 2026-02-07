# Repository Structure

Updated: February 7, 2026

## Directory Layout

```
EA Stuff/
├── data/                           # Operational content
│   ├── archimate/                  # EA element definitions
│   │   ├── strategy/               # Strategy layer elements
│   │   ├── business/               # Business layer elements
│   │   ├── application/            # Application layer elements
│   │   ├── technology/             # Technology layer elements
│   │   ├── physical/               # Physical layer elements (optional)
│   │   ├── motivation/             # Motivation layer elements
│   │   └── implementation/         # Implementation layer elements (optional)
│   └── management-system/          # Policies, instructions, manuals
│       ├── policies/
│       ├── instructions/
│       └── manuals/
│
├── schemas/                        # Validation schemas
│   └── archimate-3.2-schema.yaml   # ArchiMate 3.2 specification
│
├── docs/                           # Documentation
│   ├── quick-start.md
│   ├── mermaid-guide.md            # F# server UI guide
│   ├── element-types-reference.md
│   ├── best-practices.md
│   └── management-system/          # Management system docs (overview)
│       └── index.md
│
├── src/
│   ├── fsharp-server/              # F# web server
│   │   ├── Program.fs
│   │   ├── Handlers.fs
│   │   ├── Views.fs
│   │   └── EAArchive.fsproj
│   └── fsharp-server.Tests/        # F# server tests
│
├── README.md                       # Main documentation
└── requirements.txt                # Legacy file (unused)

```

## Usage

Run the F# server from the src/fsharp-server directory:

```bash
dotnet build
dotnet run
```