# How-to: Keep Documents Valid and Consistent

Use this guide when editing existing documents to avoid validation errors.

## 1) Start from required fields

Architecture and governance documents require these shared fields:

```yaml
id:
owner:
status:
version:
last_updated:
review_cycle:
next_review:
```

Architecture documents also require `name` and an `archimate` section:

```yaml
archimate:
  type:
  layer:
```

Governance documents must include a `governance` section:

```yaml
governance:
  approved_by:
  effective_date:
```

`name` is recommended for governance documents even though it is not enforced.

## 2) Use valid IDs

- ArchiMate IDs follow `[layer-code]-[type-code]-[###]-[descriptive-name]`.
- Governance IDs follow `ms-(policy|instruction|manual)-[###]-[descriptive-name]`.
- Glossary IDs follow `glossary-[descriptive-name]`.

See [Element ID Naming Standard](../reference/id-naming-standard.md).

## 3) Keep ownership correct

- `owner` is required for all documents.
- Governance documents require `owner` to reference a `business-role` element.

## 4) Write relationships carefully

Each relationship requires:

```yaml
relationships:
  - type: association
    target: some-id
    description: Optional description
```

Targets must exist in the repository. Missing targets produce warnings.

## 5) Put custom data under `extensions`

Use `extensions` for additional metadata instead of a top-level `properties` key.

```yaml
extensions:
  properties:
    lifecycle-phase: operate
    legacy-id: app-comp-legacy-001
```

## 6) Re-validate after edits

Run the server and review validation output.

```bash
dotnet run
```

## Checklist

- Required fields present and not empty
- IDs match the expected pattern
- `archimate.type` and `archimate.layer` present for architecture docs
- `governance.approved_by` and `governance.effective_date` present for governance docs
- Relationship targets exist
- Establish and follow naming conventions
- Review and update regularly
