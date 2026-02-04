# Quick Reference: ArchiMate Exchange Generation

## TL;DR

```bash
# Generate the exchange file
python scripts/generate_archimate_exchange.py

# Validate it worked
python scripts/validate_archimate_exchange.py

# File is at: output/model-exchange.archimate
# Import it into your ArchiMate tool
```

## Generated Model

| Aspect | Count |
|--------|-------|
| **Total Elements** | 47 |
| **Total Relationships** | 91 |
| **Layers** | 5 (Strategy, Business, Application, Technology, Motivation) |
| **File Size** | ~57 KB |
| **Format** | XML (ArchiMate 3.1) |

## Element Breakdown

| Layer | Count | Types |
|-------|-------|-------|
| **Motivation** | 28 | Goals (6), Drivers (5), Principles (5), Requirements (12) |
| **Strategy** | 15 | Capabilities (9), Value Streams (3), Resources (2), Course of Action (1) |
| **Business** | 2 | Business Process (1), Business Object (1) |
| **Application** | 1 | Application Component (1) |
| **Technology** | 1 | Node (1) |

## Relationship Breakdown

| Type | Count | Meaning |
|------|-------|---------|
| **Realization** | 38 | Implements/fulfills relationship |
| **Influence** | 27 | Affects or modifies relationship |
| **Serving** | 13 | Provides functionality relationship |
| **Association** | 12 | Generic connection |
| **Assignment** | 1 | Allocates responsibility |

## relations.xml Relationship Codes

In [schemas/relations.xml](schemas/relations.xml), each `relations` attribute is a compact set of allowed relationship types between the source and target concepts. Each letter maps to a relationship type:

| Code | Relationship Type |
|------|-------------------|
| **a** | Access |
| **c** | Composition |
| **f** | Flow |
| **g** | Aggregation |
| **i** | Assignment |
| **n** | Influence |
| **o** | Association |
| **r** | Realization |
| **s** | Specialization |
| **t** | Triggering |
| **v** | Serving |

**How to read a cell:**
If a cell shows `relations="cgnos"`, it means the source element can connect to the target using Composition (c), Aggregation (g), Influence (n), Association (o), and Specialization (s).

## Key Concepts Mapped

**Strategic Vision:**
- Digital transformation to become customer-centric, digital-first bank
- Maintain trust and regulatory excellence
- Compete effectively with fintech

**Main Goals:**
- 🎯 Enhance Digital Banking Experience
- 🎯 Improve Operational Efficiency
- 🎯 Strengthen Regulatory Compliance
- 🎯 Build Customer Trust
- 🎯 Enable Open Banking Capabilities

**Main Drivers:**
- 📊 EU & Nordic Regulatory Requirements
- 💰 Cost Pressures and Margin Compression
- 🔄 Market Digital Transformation Trends
- 🌍 Nordic Sustainability Values
- 🛡️ DORA Digital Operational Resilience

**Core Capabilities:**
- Digital Banking Platform
- Cloud Infrastructure & Modern Architecture
- Data Analytics & AI
- Open Banking Ecosystem
- Process Automation & Operational Excellence
- Regulatory Compliance & Risk Management
- Digital Operational Resilience
- Customer Trust & Data Privacy
- Omnichannel Customer Engagement

**Key Requirements:**
- GDPR Data Protection Compliance
- MiFID II Investment Services Compliance
- DORA Digital Operational Resilience
- Modern API Platform
- Cloud Infrastructure Migration
- Process Automation
- Data Security & Privacy Protection
- Mobile-First Banking Design
- Seamless Omnichannel UX
- Strategic Ecosystem Partnerships

## Common Tasks

### Import into Archi
```
1. Download Archi from archimatetool.com
2. File → Import → Import model from file
3. Select output/model-exchange.archimate
4. Done!
```

### Import into Enterprise Architect
```
1. Open Enterprise Architect
2. File → Import → Import ArchiMate Model
3. Select output/model-exchange.archimate
4. Configure options
5. Done!
```

### Validate the File
```bash
python scripts/validate_archimate_exchange.py
# Shows: ✓ VALID with statistics
```

### Update After Changes
```bash
# Edit markdown files in elements/
# Then regenerate:
python scripts/generate_archimate_exchange.py

# This overwrites output/model-exchange.archimate
```

### View File Location
```bash
ls -lh output/model-exchange.archimate
# Shows file size and details
```

## File Structure

```
your-project/
├── elements/              # Source markdown files
│   ├── motivation/        # Goals, drivers, principles, requirements
│   ├── strategy/          # Capabilities, resources, value streams
│   ├── business/          # Processes, objects
│   ├── application/       # Components, services
│   └── technology/        # Infrastructure, systems
│
├── scripts/
│   ├── generate_archimate_exchange.py    # Main generator
│   └── validate_archimate_exchange.py    # Validation tool
│
├── output/
│   └── model-exchange.archimate          # Generated file ← Import this!
│
└── docs/
    ├── archimate-exchange-generation.md  # Detailed guide
    └── using-archimate-exchange.md       # Import instructions
```

## Supported Tools

✓ **Archi** (free, open-source) - Best for architects
✓ **Enterprise Architect** (commercial) - Full-featured
✓ **Ardoq** (cloud-based SaaS)
✓ **LeanIX** (cloud-based SaaS)
✓ **BiZZdesign** (enterprise platform)
✓ Any tool supporting ArchiMate 3.1 XSD format

## What's Included

### From Your Markdown:
- ✓ All element IDs, names, types
- ✓ All layer assignments
- ✓ All relationships and connections
- ✓ All properties (owner, status, criticality, etc.)
- ✓ All tags and metadata
- ✓ Documentation (first line of content)

### What's Generated:
- ✓ Valid XML structure
- ✓ Proper ArchiMate 3.1 namespace
- ✓ Element uniqueness validation
- ✓ Relationship integrity checks
- ✓ Type mappings to official ArchiMate types

### What's Not Included:
- ⚠️ Visual diagrams/layouts (created in tool)
- ⚠️ Color schemes (set in tool)
- ⚠️ View definitions (create in tool)
- ⚠️ Full documentation (link to markdown)

## Markdown Frontmatter Format

Required fields for each element:
```yaml
---
id: unique-element-id
name: Element Display Name
type: element-type
layer: motivation|strategy|business|application|technology
relationships:
  - type: relationship-type
    target: target-element-id
    description: Optional description
properties:
  owner: Owner Name
  status: active|planning|deprecated
  criticality: low|medium|high|critical
---
```

## Element Types

### Motivation Layer
goal, driver, principle, requirement, constraint, meaning, value, stakeholder, assessment, outcome

### Strategy Layer
capability, resource, value-stream, course-of-action

### Business Layer
business-actor, business-role, business-process, business-function, business-service, business-object, business-interface, contract, product

### Application Layer
application-component, application-service, application-interface, application-process, application-function, data-object

### Technology Layer
node, device, system-software, artifact, technology-service, technology-interface, path, communication-network

### Implementation Layer
work-package, deliverable, implementation-event, plateau, gap

## Relationship Types

Structural: composition, aggregation, assignment, realization
Dependency: serving, access, influence, association
Dynamic: triggering, flow
Other: specialization

## Statistics

```
Model: Enterprise Architecture Model
Version: 1.0
Format: ArchiMate 3.1 XML
Namespace: http://www.opengroup.org/xsd/archimate/3.1

Elements: 47
├── Motivation: 28
├── Strategy: 15
├── Business: 2
├── Application: 1
└── Technology: 1

Relationships: 91
├── Realization: 38
├── Influence: 27
├── Serving: 13
├── Association: 12
└── Assignment: 1

Layers: 5
Properties: Present
Documentation: Present
Validation: ✓ PASSED
```

## Useful Commands

```bash
# Generate (default location)
python scripts/generate_archimate_exchange.py

# Generate (custom location)
python scripts/generate_archimate_exchange.py path/to/output.archimate

# Validate
python scripts/validate_archimate_exchange.py

# Validate specific file
python scripts/validate_archimate_exchange.py path/to/file.archimate

# Show file info
file output/model-exchange.archimate
ls -lh output/model-exchange.archimate
wc -l output/model-exchange.archimate

# Check for XML errors
xmllint output/model-exchange.archimate
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| File not created | Check file paths, run from project root |
| Import fails | Run validation script, check element IDs |
| Missing relationships | Ensure target IDs exist in elements |
| Type errors | Check TYPE_MAPPING in generation script |
| XML errors | Run validate script to find issues |

## Next Steps

1. Regenerate after editing markdown files
2. Import into chosen ArchiMate tool
3. Create visual diagrams
4. Generate reports
5. Share with stakeholders
6. Keep markdown as source of truth

## For More Info

See:
- `docs/archimate-exchange-generation.md` - Generation details
- `docs/using-archimate-exchange.md` - Import instructions
- `docs/quick-start.md` - Element format guide
