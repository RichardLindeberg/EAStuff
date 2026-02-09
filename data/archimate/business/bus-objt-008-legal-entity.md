---
id: bus-objt-008-legal-entity
owner: Customer Data Management
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: composition
  target: bus-objt-011-party
  description: Is a type of party
- type: association
  target: bus-objt-001-beneficial-owner
  description: Has beneficial owners
- type: association
  target: bus-objt-014-representative
  description: Has representatives
- type: composition
  target: bus-objt-010-natural-person
  description: Composed of natural persons
- type: association
  target: bus-objt-004-customer-agreement
  description: Signs agreements through representatives
name: Legal Entity
tags:
- business-object
- party-model
- legal-entity
- corporate
archimate:
  type: business-object
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-objt-legal-entity-001
---
# Legal Entity

Business object representing organizations and corporate entities with legal status.

## Description

Legal Entity represents corporations, partnerships, associations, NGOs, government agencies, and other organizations that can enter into banking relationships. Legal entities act through authorized representatives and have beneficial owners.

## Attributes

### Entity Identification
- Entity ID (unique global)
- Legal name
- Trading name/DBA
- Entity type (corporation, partnership, LLC, NGO, government, etc.)
- Registration number/Company number
- Tax identification number
- Country of incorporation
- Jurisdiction

### Organizational Details
- Industry classification (NACE code)
- Business description
- Principal business activity
- Year established
- Number of employees
- Annual revenue (if shared)
- Public/Private status

### Contact Information
- Registered office address
- Operational address(es)
- Mailing address
- Phone number(s)
- Email address(es)
- Website
- Designated contact person

### Governance Structure
- Business structure (sole proprietor, partnership, corporation, etc.)
- Number of partners/shareholders
- Board composition
- Ultimate controlling person(s)
- Key decision makers

### Beneficial Ownership (Required)
- Beneficial owners list
- Ownership percentage per owner
- Control mechanisms
- Change of control notification
- UBO (Ultimate Beneficial Owner) identified
- Ownership verification date

### Authorized Representatives
- List of authorized signatories
- Signing authority limits
- Approval workflows
- Change of signatory procedures
- Dual control requirements

### Financial Information
- Annual turnover (last 3 years)
- Profitability indicators
- Banking relationships
- Credit exposure
- Payment track record

### Compliance Information
- Tax residency country
- FATCA status
- CRS reporting
- PEP flag (if applicable)
- Sanctions screening result
- Sanctions screening date
- AML risk rating
- High-risk jurisdiction flag

### KYC/AML - Enhanced Due Diligence (EDD)
- Entity KYC verification status
- Entity verification date
- Beneficial owner verification
- Beneficial owner verification date
- Sanctions list screening (entity & beneficial owners)
- OFAC/EU sanctions list checks
- Risk rating (entity level)
- Enhanced due diligence required?
- EDD completion status

### Legal & Regulatory Status
- Active/Dissolved status
- Regulatory licenses/permits
- Compliance certifications
- Court cases/litigation
- Regulatory actions or warnings
- Data Protection Officer appointed

### Account & Service Status
- Customer status (prospect, active, dormant, inactive)
- Service tier
- Approved products
- Restricted products
- Account restrictions
- Compliance restrictions

## Legal Entity Types

### By Structure
- **Limited Company**: Corporation with limited liability
- **Partnership**: General or limited partnership
- **Sole Proprietor**: Individual business owner
- **Limited Liability Company (LLC)**: Hybrid structure
- **Non-profit/NGO**: Non-profit organization
- **Government Agency**: Public sector entity
- **Trust/Estate**: Fiduciary arrangement

### By Business Size
- **Micro**: < 10 employees, < €2M revenue
- **Small**: < 50 employees, < €10M revenue
- **Medium**: < 250 employees, < €50M revenue
- **Large**: > 250 employees, > €50M revenue

### By Sector
- **Financial Services**: Banks, insurance, etc.
- **Manufacturing**: Production and industry
- **Retail/Hospitality**: Consumer-facing business
- **Technology**: Software and IT services
- **Professional Services**: Law, accounting, consulting
- **Real Estate**: Property and development
- **Non-profit**: Charitable organizations

## Beneficial Owner Structure

Each Legal Entity must have:
- **Ultimate Beneficial Owner (UBO)**: The natural person(s) ultimately owning/controlling entity
- **Ownership chain**: Documentation of ownership from entity to UBO
- **Control mechanisms**: How control is exercised (direct ownership, voting rights, trustee, etc.)
- **Transparency**: Full disclosure of all beneficial owners

## Signatory Authority

- **Authorized signatories**: Named natural persons who can sign on entity's behalf
- **Signing limits**: Transaction limits per signatory
- **Multi-signature requirements**: Dual authorization if applicable
- **Signing authority proof**: Certified board resolutions
- **Signatory changes**: Notification and documentation procedures

## Business Rules

- Legal entity status must be verified via company registry
- All beneficial owners must be identified and verified
- At least one authorized signatory required
- All signatories must be natural persons with identity verification
- Enhanced due diligence mandatory for high-risk entities
- Ownership structure must be documented and kept current
- Annual compliance reviews required

## Regulatory Requirements

- Corporate governance compliance
- Beneficial ownership transparency (EU 5AMLD, FinCEN)
- PEP/Sanctions screening mandatory
- Sanctions list checks (OFAC, EU, UN, national)
- AML/CFT compliance
- Tax compliance (FATCA, CRS, local)
- Data protection compliance (GDPR)
- Financial reporting (if applicable)

## Complex Structures

Special handling for:
- **Holding companies**: Multiple layers of entities
- **Trusts**: Trustee vs. beneficial owner
- **Partnerships**: Multiple general partners with joint liability
- **Syndications**: Multiple entities with shared ownership
- **Government entities**: Specific governance structures
