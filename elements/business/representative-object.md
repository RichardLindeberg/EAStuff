---
id: bus-objt-representative-001
name: Representative
type: business-object
layer: business
relationships:
  - type: composition
    target: bus-objt-natural-person-001
    description: Is always a natural person
  - type: association
    target: bus-objt-legal-entity-001
    description: Represents legal entity
  - type: association
    target: bus-objt-beneficial-owner-001
    description: Can represent beneficial owner
  - type: access
    target: bus-objt-customer-agreement-001
    description: May sign agreements on behalf of party
properties:
  owner: Operations and Compliance
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - business-object
  - representation
  - authorization
  - corporate-governance
---

# Representative

Business object representing natural persons authorized to act, sign, and conduct business on behalf of legal entities or other parties.

## Description

Representatives are natural persons who are authorized to represent and bind legal entities or other parties in banking relationships. They execute transactions, sign agreements, and manage accounts within their delegated authority.

## Attributes

### Representative Identification
- Representative ID
- Natural Person reference (link to Natural Person)
- Represented Party reference (Legal Entity or Beneficial Owner)
- Representation relationship ID
- Representation type

### Representation Authority
- Authority type (signatory, officer, power of attorney, proxy)
- Title/Position in organization
- Authority scope (all transactions, limited scope)
- Transaction limits (monetary and type)
- Approval authority limits
- Dual control requirement? (Yes/No)
- Can delegate authority? (Yes/No)

### Representation Scope
- Product access (which products can representative access)
- Channel access (mobile, online, branch, API)
- Geographic scope (any geography, limited)
- Time scope (ongoing, temporary, date-limited)
- Function scope (transaction, approval, reporting, maintenance)

### Signatory Authority
- Is authorized signatory? (Yes/No)
- Signatory title
- Signing limit (per transaction)
- Daily signing limit
- Monthly signing limit
- Requires counter-signature? (Yes/No)
- Counter-signatory required (if applicable)

### Authorization Documentation
- Authorization document type (board resolution, power of attorney, appointment letter, shareholder resolution)
- Document reference number
- Issue date
- Expiry date (if applicable)
- Scope documented in authorization
- Supporting documentation
- Document storage location
- Authorization verification date

### Contact Information
- Phone number
- Email address
- Office address
- Preferred contact method

### Compliance Information
- Background check completed? (Yes/No)
- Background check date
- Background check result (pass/concerns/fail)
- Identity verified? (Yes/No)
- Identity verification date
- Sanctions check completed? (Yes/No)
- Sanctions check date
- PEP status check completed? (Yes/No)
- PEP status (Yes/No)
- Compliance approval? (Yes/No)

### Status Information
- Representative status (active, suspended, terminated)
- Status effective date
- Reason for status change
- Suspension reason (if applicable)
- Termination date (if applicable)
- Termination reason (if applicable)

### Activity Tracking
- Last transaction date
- Last account access date
- Transaction frequency
- High-risk transaction flag
- Unusual activity alert

## Representative Types

### By Authorization Level
- **Authorized Signatory**: Can sign documents/contracts
- **Senior Officer**: Executive-level authority
- **Manager**: Department/function level authority
- **Limited Authority**: Restricted transaction authority
- **Restricted User**: Read-only or inquiry only
- **Administrator**: System administration access

### By Role
- **Managing Director/CEO**: Full authority
- **Finance Director/CFO**: Financial transactions
- **Operations Manager**: Daily operations
- **Accountant/Finance Officer**: Financial matters
- **HR Representative**: HR-related matters
- **Compliance Officer**: Compliance matters
- **Customer Service Representative**: Support matters

### By Authority Scope
- **General Authority**: All banking transactions
- **Limited Authority**: Specific transaction types
- **Amount-Limited**: Up to specific transaction amount
- **Time-Limited**: Authority for specific period
- **Conditional Authority**: Subject to conditions (dual control, etc.)

## Business Rules

- Representative must always be a natural person
- Representative must be verified and compliant
- Representative must have documented authorization
- Representative authority must be within policy limits
- Representative cannot exceed their delegated authority
- Changes in representative status must trigger account review
- Suspended/terminated representatives must have account access revoked
- Background checks required per policy
- PEP/Sanctions checks mandatory

## Authorization Management

- **Appointment**: New representative authorization
- **Modification**: Change scope, limits, or terms
- **Suspension**: Temporary revocation of authority
- **Termination**: Permanent revocation of authority
- **Re-activation**: Restoration after suspension
- **Delegation**: Authority delegated to another person
- **Renewal**: Periodic re-authorization (annual, etc.)

## Audit & Control

- All representative actions logged
- Unusual activity alerts
- Periodic authority reviews
- Compliance monitoring
- Exception reporting
- Transaction analysis
- Access control enforcement

## Regulatory Considerations

- Signatory authority verification required
- Officer identification for high-risk entities
- Authority documentation retention (7+ years)
- Change of signatory reporting
- Fraud prevention controls
- Dual control for sensitive transactions
- Segregation of duties enforcement
