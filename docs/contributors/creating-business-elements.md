# How-to: Create Business Layer Elements

Use this guide to add business-layer elements that match current frontmatter requirements.

## 1) Choose the correct folder

Create a new file under `data/archimate/business/`.

## 2) Add required frontmatter

All ArchiMate elements share a common set of required fields plus an `archimate` section.

```yaml
---
id: bus-proc-001-customer-onboarding
name: Customer Onboarding Process
owner: bus-role-001-process-owner
status: draft
version: "0.1"
last_updated: "2026-02-10"
review_cycle: annual
next_review: "2027-02-10"
archimate:
  type: business-process
  layer: business
relationships:
  - type: realization
    target: bus-srvc-001-customer-onboarding
    description: Realizes onboarding service
tags:
  - onboarding
  - customer
extensions:
  properties:
    lifecycle-phase: build
---
```

## 3) Write the markdown body

```markdown
# Customer Onboarding Process

Short description of the process and its scope.
```

## 4) Pick a valid business element type

Common business-layer types:

- `business-actor`
- `business-role`
- `business-process`
- `business-function`
- `business-service`
- `business-object`
- `business-event`
- `business-collaboration`
- `business-interface`
- `business-interaction`
- `contract`
- `representation`
- `product`

See [Element Types Reference](../reference/element-types-reference.md) for the full list.

## 5) Validate in the server

Run the F# server and check validation warnings for required fields, invalid IDs, and missing targets.

```bash
dotnet run
```

## Notes

- `archimate.layer` must be `business` for business elements.
- `relationships` entries must include `type` and `target`.
- Use `extensions` for domain-specific fields instead of a top-level `properties` key.

## KPIs

- **Completion Rate**: Currently 62% (target >60%)
- **Average Duration**: 14 minutes (target <15 min)
- **Drop-off Rate**: 38% (mainly at identity verification)
- **Customer Satisfaction**: 4.3/5

## Automation

- 85% automated (identity verification, account creation, activation)
- 15% manual review (high-risk cases, verification failures)
- AI-assisted identity document verification
- Automated KYC checks

## Compliance

- GDPR compliant consent management
- KYC/AML requirements met
- Identity verification per banking regulations
- Audit trail for all steps

## Integration Points

- Identity verification service (3rd party)
- Core banking system
- CRM system
- Compliance monitoring system
```

## Example 4: Creating a Business Object

```yaml
---
id: bus-objt-001-account
name: Account
type: business-object
layer: business
relationships:
  - type: access
    target: bus-srvc-001-digital-account-mgmt
    description: Accessed by account management service
  - type: access
    target: bus-proc-001-payment-initiation
    description: Accessed by payment processes
  - type: realization
    target: app-data-001-account
    description: Realized by account data object in application layer
properties:
  data-classification: confidential
  retention-period: "7 years after closure"
  owner: Core Banking Department
tags:
  - core-data
  - customer-data
  - regulated
---

# Account

Business object representing a customer's banking account with associated balance, transactions, and settings.

## Description

The Account is a fundamental business object representing the relationship between a customer and the bank, holding funds and transaction history.

## Attributes

### Core Attributes
- Account Number (unique identifier)
- Account Type (checking, savings, investment)
- Status (active, frozen, closed)
- Opening Date
- Currency

### Financial Attributes
- Current Balance
- Available Balance
- Overdraft Limit
- Interest Rate
- Minimum Balance

### Customer Attributes
- Primary Account Holder
- Joint Account Holders
- Authorized Signatories
- Account Owner Type (individual, business)

### Settings
- Alert Preferences
- Statement Frequency
- Overdraft Settings
- Transaction Limits

## Lifecycle

1. **Opened** - Account created during onboarding
2. **Active** - Normal operational state
3. **Frozen** - Temporarily suspended (fraud, court order)
4. **Dormant** - No activity for extended period
5. **Closed** - Account terminated by customer or bank

## Business Rules

- Minimum age for account holder: 18 years
- Minimum opening balance: varies by account type
- Maximum daily transfer limit: €50,000
- Monthly maintenance fee: waived if balance > €5,000
- Negative balance allowed up to overdraft limit

## Related Objects

- Customer Profile
- Payment Transaction
- Statement
- Card
- Loan
```

## Relationship Types for Business Layer

Common relationship types from business to other layers:

### To Motivation Layer
- `realization` - Business element realizes a goal/requirement
- `influence` - Business element is influenced by a driver/principle

### To Strategy Layer
- `realization` - Business element realizes a capability
- `association` - Business element relates to a resource

### Within Business Layer
- `assignment` - Actor/role is assigned to behavior
- `serving` - Service serves an actor
- `access` - Process accesses an object
- `triggering` - Event triggers a process
- `composition` - Element is part of another
- `aggregation` - Element groups other elements
- `realization` - Behavior realizes a service
- `flow` - Process flow between elements

### To Application Layer
- `realization` - Application component/service realizes business service
- `serving` - Application service serves business process

## Tips for Good Business Elements

1. **Use business language** - Avoid technical jargon
2. **Focus on what, not how** - Describe business logic, not implementation
3. **Link to strategy** - Show how element supports goals/capabilities
4. **Define ownership** - Assign business owners, not IT
5. **Include metrics** - KPIs, SLAs, target values
6. **Document decisions** - Explain why element exists
7. **Trace to applications** - Show which apps realize the business element

## Validation Checklist

Before finalizing a business element, ensure:

- [ ] Clear business owner assigned
- [ ] Links to at least one motivation/strategy element
- [ ] Has description in business language
- [ ] Includes relevant properties (owner, status, criticality)
- [ ] Tagged appropriately for findability
- [ ] Relationships to dependent elements defined
- [ ] Metrics or success criteria included (where applicable)

## Next Steps

After creating business elements:

1. **Run the F# server**: `dotnet run`
2. **Verify relationships**: Review incoming and outgoing relations in the UI
3. **Review traceability**: Ensure goals → capabilities → services trace correctly
4. **Validate with stakeholders**: Review with business owners
5. **Create viewpoints**: Document specific aspects in dedicated views
