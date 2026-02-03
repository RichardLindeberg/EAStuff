# Element ID Naming Standard

## Overview

This document defines the standard naming convention for ArchiMate element IDs to ensure consistency, scalability, and maintainability across the enterprise architecture repository.

## ID Format

All element IDs must follow this standardized format:

```
[layer-code]-[type-code]-[###]-[descriptive-name]
```

### Components

1. **Layer Code** (3 characters): Identifies the ArchiMate layer
2. **Type Code** (4 characters): Identifies the element type
3. **Sequential Number**: Three-digit number (001-999)
4. **Descriptive Name**: Short, meaningful identifier (lowercase, hyphen-separated)

## Layer Codes

| Layer | Code | Example |
|-------|------|---------|
| Strategy | `str` | str-cap-001-customer-engagement |
| Business | `bus` | bus-proc-001-order-fulfillment |
| Application | `app` | app-comp-001-customer-portal |
| Technology | `tec` | tec-node-001-web-server |
| Physical | `phy` | phy-equi-001-data-center-rack |
| Motivation | `mot` | mot-goal-001-increase-revenue |
| Implementation | `imp` | imp-work-001-cloud-migration |

## Type Codes

### Strategy Layer

| Element Type | Code | Example |
|--------------|------|---------|
| resource | `rsrc` | str-rsrc-001-financial-capital |
| capability | `capa` | str-capa-001-omnichannel |
| value-stream | `vstr` | str-vstr-001-customer-journey |
| course-of-action | `cact` | str-cact-001-digital-first |

### Business Layer

| Element Type | Code | Example |
|--------------|------|---------|
| business-actor | `actr` | bus-actr-001-customer |
| business-role | `role` | bus-role-001-account-manager |
| business-collaboration | `colab` | bus-colab-001-sales-team |
| business-interface | `intf` | bus-intf-001-service-desk |
| business-process | `proc` | bus-proc-001-order-processing |
| business-function | `func` | bus-func-001-credit-check |
| business-interaction | `intr` | bus-intr-001-customer-meeting |
| business-event | `evnt` | bus-evnt-001-order-received |
| business-service | `srvc` | bus-srvc-001-order-management |
| business-object | `objt` | bus-objt-001-customer-data |
| contract | `cntr` | bus-cntr-001-service-agreement |
| representation | `repr` | bus-repr-001-invoice-document |
| product | `prod` | bus-prod-001-insurance-policy |

### Application Layer

| Element Type | Code | Example |
|--------------|------|---------|
| application-component | `comp` | app-comp-001-crm-system |
| application-collaboration | `colab` | app-colab-001-integration-layer |
| application-interface | `intf` | app-intf-001-rest-api |
| application-function | `func` | app-func-001-validate-user |
| application-interaction | `intr` | app-intr-001-data-sync |
| application-process | `proc` | app-proc-001-batch-processing |
| application-event | `evnt` | app-evnt-001-payment-completed |
| application-service | `srvc` | app-srvc-001-authentication |
| data-object | `data` | app-data-001-customer-record |

### Technology Layer

| Element Type | Code | Example |
|--------------|------|---------|
| node | `node` | tec-node-001-app-server |
| device | `devc` | tec-devc-001-firewall |
| system-software | `sysw` | tec-sysw-001-operating-system |
| technology-collaboration | `colab` | tec-colab-001-cluster |
| technology-interface | `intf` | tec-intf-001-jdbc-connector |
| path | `path` | tec-path-001-fiber-optic |
| communication-network | `netw` | tec-netw-001-corporate-lan |
| technology-function | `func` | tec-func-001-data-encryption |
| technology-process | `proc` | tec-proc-001-backup-routine |
| technology-interaction | `intr` | tec-intr-001-replication |
| technology-event | `evnt` | tec-evnt-001-server-restart |
| technology-service | `srvc` | tec-srvc-001-hosting-service |
| artifact | `artf` | tec-artf-001-deployment-package |

### Physical Layer

| Element Type | Code | Example |
|--------------|------|---------|
| equipment | `equi` | phy-equi-001-server-rack |
| facility | `faci` | phy-faci-001-data-center |
| distribution-network | `dist` | phy-dist-001-power-grid |
| material | `matr` | phy-matr-001-raw-materials |

### Motivation Layer

| Element Type | Code | Example |
|--------------|------|---------|
| stakeholder | `stkh` | mot-stkh-001-ceo |
| driver | `drvr` | mot-drvr-001-market-competition |
| assessment | `asmt` | mot-asmt-001-swot-analysis |
| goal | `goal` | mot-goal-001-increase-revenue |
| outcome | `outc` | mot-outc-001-improved-satisfaction |
| principle | `prin` | mot-prin-001-security-first |
| requirement | `reqt` | mot-reqt-001-gdpr-compliance |
| constraint | `cnst` | mot-cnst-001-budget-limit |
| meaning | `mean` | mot-mean-001-brand-identity |
| value | `valu` | mot-valu-001-customer-trust |

### Implementation Layer

| Element Type | Code | Example |
|--------------|------|---------|
| work-package | `work` | imp-work-001-phase-1-migration |
| deliverable | `delv` | imp-delv-001-deployment-guide |
| implementation-event | `evnt` | imp-evnt-001-go-live |
| plateau | `plat` | imp-plat-001-target-architecture |
| gap | `gap_` | imp-gap_-001-capability-shortfall |

## Descriptive Name Guidelines

The descriptive name portion should be:

- **Concise**: 2-4 words maximum
- **Meaningful**: Clearly identifies the element
- **Lowercase**: All characters in lowercase
- **Hyphen-separated**: Use hyphens instead of spaces or underscores
- **Alphanumeric**: Only letters, numbers, and hyphens (no special characters)

### Good Examples
- `customer-portal`
- `order-processing`
- `crm-system`
- `web-server`
- `digital-transform`

### Bad Examples
- `CustomerPortal` (not lowercase)
- `customer_portal` (uses underscores)
- `the-main-customer-facing-web-portal` (too long)
- `portal` (too vague)
- `cust-prtl` (overly abbreviated)

## Sequential Numbers

- Start at `001` for the first instance
- Increment sequentially: `002`, `003`, etc.
- Always use three digits with leading zeros
- Numbers are unique within the combination of layer-type-name
- Maximum of 999 instances per unique identifier pattern

### Example Sequence
```
app-comp-001-web-portal
app-comp-002-web-portal
app-comp-003-web-portal
```

## Complete Examples

### Strategy Layer
```yaml
id: str-capa-001-omnichannel
name: Omnichannel Customer Engagement
type: capability
layer: strategy
```

### Business Layer
```yaml
id: bus-proc-001-customer-service
name: Customer Service Process
type: business-process
layer: business
```

### Application Layer
```yaml
id: app-comp-001-customer-portal
name: Customer Portal
type: application-component
layer: application
```

### Technology Layer
```yaml
id: tec-node-001-web-server
name: Web Application Server
type: node
layer: technology
```

### Motivation Layer
```yaml
id: mot-goal-001-digital-transform
name: Digital Transformation Initiative
type: goal
layer: motivation
```

## File Naming Standard

Element definition files should follow the same naming convention as the element IDs themselves for consistency and improved organization.

### File Name Format

```
[layer-code]-[type-code]-[###]-[descriptive-name].md
```

This matches the element ID format exactly, with the `.md` extension added.

### Examples

| Layer | File Name Example |
|-------|-------------------|
| Strategy | `str-capa-001-omnichannel.md` |
| Business | `bus-proc-001-customer-service.md` |
| Business | `bus-role-002-account-manager.md` |
| Application | `app-comp-001-customer-portal.md` |
| Technology | `tec-node-001-web-server.md` |
| Motivation | `mot-goal-001-digital-transform.md` |

### Benefits

- **Self-documenting**: Element layer and type visible at directory level
- **Natural sorting**: Files group by layer, then by type
- **Quick identification**: No need to open files to understand what they contain
- **Consistency**: File name matches the ID inside the file
- **Unique**: Prevents accidental file name collisions
- **Searchable**: Easy to find files using glob patterns or search

### Web Display

File names do NOT affect how elements are displayed on the generated website. The website uses the `name` field from within each element's YAML frontmatter for display purposes.

```yaml
---
id: bus-proc-001-customer-service
name: Customer Service Process    # ← This is used for web display
type: business-process
layer: business
---
```

This separation allows:
- Technical, organized file names for repository management
- Clean, human-friendly names for user-facing documentation
- Independence between storage format and presentation

## Benefits

This standardized approach provides:

✅ **Self-documenting**: Layer and type are immediately visible in the ID  
✅ **Sortable**: Elements naturally group by layer and type  
✅ **Searchable**: Easy to find related elements  
✅ **Collision-resistant**: Low risk of duplicate IDs across layers  
✅ **Scalable**: Supports up to 999 instances per element pattern  
✅ **Tool-friendly**: Consistent format enables automation  
✅ **Human-readable**: Clear structure without consulting documentation  

## Migration from Legacy IDs

For existing elements with non-standard IDs:

1. **Document legacy ID**: Add `legacy-id` property to preserve old identifier
2. **Update references**: Find and update all relationships pointing to the old ID
3. **Validate**: Run validation scripts to ensure no broken references
4. **Update diagrams**: Regenerate all diagrams with new IDs

Example migration:
```yaml
# Before (old standard)
id: app-comp-customer-portal-001

# After (new standard)
id: app-comp-001-customer-portal
properties:
  legacy-id: app-comp-customer-portal-001
```

## Tooling Support

The `scripts/create_element.py` script automatically generates IDs following this standard based on:
- Selected layer
- Selected element type
- User-provided element name
- Auto-incremented sequential number (scans existing elements)

## Validation

The validation script (`scripts/validator/validate.py`) enforces:
- Correct ID format structure
- Valid layer and type codes
- Proper character usage in descriptive names
- Unique IDs across the repository

## Best Practices

1. **Always use the creation script**: Ensures consistency and avoids manual errors
2. **Don't reuse IDs**: Even for deleted elements, skip that number in the sequence
3. **Keep names stable**: Once assigned, avoid changing the descriptive name portion
4. **Document exceptions**: If deviation is necessary, document the reason
5. **Review before commit**: Validate IDs before committing to version control

## Questions or Exceptions?

Contact the Enterprise Architecture team for guidance on:
- Complex naming scenarios
- Cross-cutting concerns
- Specialized element types
- Legacy system integration
