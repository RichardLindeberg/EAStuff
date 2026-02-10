# How-to: Find Where Documents Live

Use this guide to choose the correct folder when adding or updating content.

## 1) Place architecture elements under `data/archimate`

Each element belongs to a layer folder.

```
data/archimate/
	├── strategy/
	├── business/
	├── application/
	├── technology/
	├── physical/
	├── motivation/
	└── implementation/
```

## 2) Place governance docs under `data/management-system`

Use these folders for policies, instructions, and manuals:

```
data/management-system/
	├── policies/
	├── instructions/
	└── manuals/
```

## 3) Find documentation in `docs/`

Start at [docs/index.md](../index.md) and follow the audience links.

## 4) Find validation schemas in `schemas/`

Schema files define ArchiMate types and relationship rules used by validation.
