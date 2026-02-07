# Best Practices for EA Tool

Guidelines for creating and maintaining high-quality EA documentation.

## File Organization

### Directory Structure

Organize elements by ArchiMate layer:

```
elements/
  ├── strategy/          # Strategic capabilities and resources
  ├── business/          # Business architecture
  ├── application/       # Application architecture
  ├── technology/        # Technology infrastructure
  ├── physical/          # Physical elements
  ├── motivation/        # Goals, requirements, drivers
  └── implementation/    # Projects and deliverables
```

### Naming Conventions

**File Names:**
- Use lowercase with hyphens: `customer-portal.md`
- Be descriptive but concise
- Match the element name where possible

**Element IDs:**
- Follow the standardized ID format: `[layer-code]-[type-code]-[###]-[descriptive-name]`
- Examples: `app-comp-001-customer-portal`, `app-srvc-001-authentication`
- See [ID Naming Standard](id-naming-standard.md) for complete details
- Keep IDs unique across all elements
- Use meaningful prefixes for element categories

**Element Names:**
- Use title case: "Customer Portal"
- Be specific and descriptive
- Avoid abbreviations unless widely understood
- Keep names under 50 characters

## Documentation Quality

### YAML Frontmatter

**Required Fields:**
Always include these fields:
```yaml
id: app-comp-001-customer-portal
name: Element Name
type: element-type
layer: layer-name
```

**Recommended Fields:**
Include these for better documentation:
```yaml
properties:
  owner: Responsible team or person
  status: development|production|deprecated
  criticality: low|medium|high|critical
  last-updated: YYYY-MM-DD
```

**Relationships:**
Document connections to other elements:
```yaml
relationships:
  - type: relationship-type
    target: bus-proc-001-customer-onboarding
    description: Brief explanation of the relationship
```

### Markdown Content

**Structure:**
1. Main heading (H1) matching the element name
2. Brief one-sentence description
3. Detailed Description section (H2)
4. Additional relevant sections (H2)

**Sections to Consider:**
- Description / Overview
- Key Features / Capabilities
- Technical Details / Specifications
- Dependencies
- Quality Attributes / SLAs
- Risks / Issues
- Future Plans

**Writing Style:**
- Be concise and factual
- Use bullet points for lists
- Include specific metrics where relevant
- Avoid jargon or explain technical terms
- Keep audience in mind (technical vs business)

## Relationship Management

### Choosing Relationship Types

**Structural Relationships:**
- `composition`: When one element contains another (strong ownership)
- `aggregation`: When elements are grouped together (loose grouping)
- `assignment`: When allocating responsibility or resources
- `realization`: When implementing or fulfilling something

**Dependency Relationships:**
- `serving`: When providing functionality or services
- `access`: When reading or writing data
- `influence`: When affecting decisions or behavior
- `association`: For generic relationships

**Dynamic Relationships:**
- `triggering`: For temporal or causal sequences
- `flow`: For data or control flow between elements

### Bidirectional Relationships

Document relationships from both perspectives when relevant:
- A serves B → document on A
- B is served by A → document on B (optional but helpful)

## Property Management

### Standard Properties

**status:**
- `planning`: Being designed
- `development`: Under development
- `testing`: In testing phase
- `production`: Live and operational
- `maintenance`: In maintenance mode
- `deprecated`: No longer recommended
- `retired`: Decommissioned

**criticality:**
- `low`: Minimal business impact if unavailable
- `medium`: Moderate business impact
- `high`: Significant business impact
- `critical`: Business-critical, requires immediate attention

**lifecycle-phase:**
- `plan`: In planning
- `build`: Under construction
- `operate`: In operation
- `retire`: Being phased out

### Custom Properties

Add domain-specific properties as needed:
```yaml
properties:
  cost: "$10,000/year"
  compliance: "GDPR, SOX"
  sla: "99.9% uptime"
  capacity: "10,000 concurrent users"
  vendor: "Vendor Name"
```

## Version Control

### Tracking Changes

Update `last-updated` when making changes:
```yaml
properties:
  last-updated: "2026-01-30"
```

### Element Evolution

When replacing elements:
1. Mark old element as `deprecated`
2. Add relationship from new to old: `specialization`
3. Update relationships pointing to old element
4. Eventually retire the old element

## Validation and Quality Checks

### Regular Validation

Run the F# server and review validation warnings:
```bash
dotnet run
```

Check the console output and element pages for relationship or schema warnings.

### Quality Checklist

Before committing elements:
- ✅ All required fields present
- ✅ Valid element type for layer
- ✅ Relationships documented with targets
- ✅ Properties filled out (at minimum: owner, status)
- ✅ Markdown description is clear and complete
- ✅ No validation errors
- ✅ File named appropriately
- ✅ Placed in correct directory

## Team Collaboration

### Element Ownership

- Assign clear ownership in properties
- Keep contact information updated
- Review owned elements regularly (quarterly)

### Review Process

1. Create element in draft form
2. Validate using tool
3. Peer review for accuracy
4. Update status to production
5. Regular reviews and updates

### Documentation Standards

- Use consistent terminology across elements
- Reference official names from other systems
- Link to external documentation where helpful
- Keep descriptions up-to-date with reality

## Advanced Topics

### Modeling Principles

**Granularity:**
- Application Layer: Model significant systems, not every microservice
- Business Layer: Model key processes, not every task
- Strategy Layer: Model major capabilities, not skills

**Completeness vs. Usefulness:**
- Focus on elements that provide value
- Don't model everything, model what matters
- Quality over quantity

**Abstraction Levels:**
- Keep consistent abstraction within a layer
- Use different layers for different abstraction levels
- Don't mix detailed and high-level in same model

### Extending the Schema

To add custom element types:
1. Edit `schemas/archimate-3.2-schema.yaml`
2. Add to appropriate layer
3. Update documentation
4. Communicate to team

To add custom properties:
- Simply add to element frontmatter
- Update team standards documentation
- Consider adding to schema for validation

## Common Pitfalls

❌ **Don't:**
- Create elements without clear purpose
- Use vague or generic names
- Leave relationships undocumented
- Skip the markdown description
- Create duplicate elements
- Use inconsistent terminology
- Let documentation become stale

✅ **Do:**
- Model with intent and purpose
- Use specific, meaningful names
- Document all significant relationships
- Provide clear descriptions
- Maintain unique identifiers
- Establish and follow naming conventions
- Review and update regularly
