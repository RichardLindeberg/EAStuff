---
id: bus-objt-011-party
owner: Customer Data Management
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: bus-objt-010-natural-person
  description: Can be a natural person
- type: association
  target: bus-objt-008-legal-entity
  description: Can be a legal entity
name: Party
tags:
- business-object
- party-model
- master-data
archimate:
  type: business-object
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-objt-party-001
---
# Party

Abstract business object representing any entity that can enter into banking relationships - either a natural person (individual) or legal entity (organization).

## Description

Party is the foundational concept for the banking customer model. Every entity (natural or legal) that interacts with the bank is represented as a Party. This allows unified treatment of diverse customer types while maintaining clear distinctions between individuals and organizations.

## Attributes

### Party Identification
- Party ID (unique global identifier)
- Party type (natural-person, legal-entity)
- Party name (individual or organization)
- External party identifiers (customer number, reference)
- Creation date
- Last update date

### Party Classification
- Customer status (prospect, active, dormant, closed)
- Party segment (retail, corporate, SME, etc.)
- Risk classification
- Regulatory classification
- PEP/Sanctions status

### Party Metadata
- Primary country
- Language preference
- Communication preference
- Data sharing consent
- Compliance flags

## Party Types

### Natural Person
- Individuals with legal capacity
- Minimum age and legal residency requirements
- Can be: primary customers, representatives, beneficial owners, signatories

### Legal Entity
- Organizations with legal status (corporation, partnership, NGO, etc.)
- Have beneficial owners and representatives
- Can enter contracts through authorized signatories
- Subject to enhanced compliance requirements

## Party Relationships

A Party can have multiple roles:
- **Customer**: Primary relationship via Customer Agreement
- **Representative**: Acting on behalf of another party
- **Beneficial Owner**: Ultimate beneficial owner of a legal entity
- **Signatory**: Authorized to sign agreements on behalf of party

## Business Rules

- Each Party must be uniquely identified
- Party type determines data requirements
- Party status governs available products/services
- Party compliance status must be current
- Party relationships must be documented and authorized
- Audit trail required for all party changes

## Regulatory Context

- GDPR: Individual natural person rights
- CRS/FATCA: Tax residency determination
- KYC/AML: Enhanced due diligence
- Beneficial Ownership: Transparency requirements
- Data Protection: Privacy by design
