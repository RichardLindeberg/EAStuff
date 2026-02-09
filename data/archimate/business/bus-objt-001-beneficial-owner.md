---
id: bus-objt-001-beneficial-owner
owner: Compliance and AML
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: composition
  target: bus-objt-010-natural-person
  description: Is always a natural person
- type: association
  target: bus-objt-008-legal-entity
  description: Owns/controls legal entity
- type: association
  target: bus-proc-002-beneficial-ownership-identification-proc
  description: Identified through BO identification process
name: Beneficial Owner
tags:
- business-object
- beneficial-ownership
- compliance
- aml-kyc
archimate:
  type: business-object
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-objt-beneficial-owner-001
---
# Beneficial Owner

Business object representing natural persons who ultimately own or control a legal entity.

## Description

Beneficial Owner is a natural person who directly or indirectly owns, controls, or has significant influence over a legal entity. This is critical for AML/KYC compliance and regulatory transparency. Every corporate customer must have identified beneficial owners.

## Attributes

### Beneficial Owner Identification
- Beneficial Owner ID
- Natural Person reference (link to Natural Person)
- Legal Entity reference (link to Legal Entity)
- Ownership type (direct, indirect, through trust, etc.)

### Ownership Details
- Ownership percentage
- Class of ownership (common stock, preferred, etc.)
- Voting rights percentage
- Control type (legal owner, controller, beneficiary)
- Date ownership acquired
- Date ownership verified

### Control Mechanisms
- **Direct Ownership**: Direct shareholding
- **Indirect Ownership**: Through intermediate entities
- **Trustee Arrangement**: Beneficial interest in trust
- **Voting Agreement**: Control via voting agreement
- **Management Control**: De facto control despite lower ownership
- **Settlement Arrangement**: Through settlement agreements

### Beneficial Ownership Details
- Ultimate Beneficial Owner (UBO) flag (Yes/No)
- Ownership chain description
- Cumulative ownership (all positions combined)
- Controlling interest flag (>25%, >50%, etc.)
- Joint control (if multiple beneficial owners)

### Verification Information
- Beneficial owner identification date
- Identification method (document review, registry check, company statement)
- Identification evidence (documents, confirmations)
- Verification status (verified, unverified, unable to verify)
- Verification date
- Next verification date
- Verifying officer name
- Verification notes

### Compliance Information
- PEP (Politically Exposed Person) status
- PEP check date
- PEP family member flag
- PEP close associate flag
- Sanctions list match
- Sanctions list check date
- AML risk rating
- Conflict of interest flag
- Source of wealth documentation

### Status Information
- Beneficial owner status (current, former, inactive)
- Status change date
- Reason for status change (if applicable)
- Notification of change date
- Updated by (person and date)

## Beneficial Owner Classifications

### By Ownership Structure
- **Direct Beneficial Owner**: Individual shareholder
- **Indirect Beneficial Owner**: Owns through other entity
- **Trust Beneficiary**: Beneficial interest through trust
- **Settlement Trustee**: Trustee with beneficial discretion
- **Foundation Member**: Member with control in foundation
- **Partnership Member**: Partner with ownership/control

### By Ownership Percentage
- **Significant Ownership**: > 25%
- **Majority Ownership**: > 50%
- **Total Control**: 100%
- **Joint Beneficial Owners**: Multiple UBOs with combined control

### By Control Type
- **Legal Owner**: Title holder
- **Controller**: De facto control
- **Beneficiary**: Beneficial interest holder
- **Trustee**: Trustee with discretion
- **Senior Manager**: Key management control

## Business Rules

- Every legal entity must have identified beneficial owners
- If no identifiable beneficial owner, company itself is treated as party
- Beneficial owner must be a natural person (unless structure exceeds policy)
- Beneficial ownership percentage must be documented
- UBO identification mandatory
- Beneficial owner information must be kept current (annual review minimum)
- Changes in beneficial ownership must trigger compliance review
- PEP status must be checked for all beneficial owners
- Source of wealth documentation for high-risk beneficial owners

## Regulatory Requirements (5AMLD & FinCEN)

- Identification of beneficial owners holding 25%+ ownership
- UBO identification for all corporate customers
- Verification of beneficial owner information
- Maintenance of beneficial ownership registers
- Suspicious ownership structures reporting
- Transparency for complex structures (trusts, shell companies)
- Update when beneficial ownership changes
- Investigation of obscured ownership structures

## Special Cases

### Difficult to Identify Scenarios
- Public company with diffuse shareholders
- Shell companies with nominee shareholders
- Trusts with discretionary beneficiaries
- Foreign ownership through offshore entities
- Government-controlled entities
- Employee ownership plans

### Reporting Obligations
- Suspicious beneficial ownership structures
- Changes in beneficial ownership
- PEP beneficial owners
- Beneficial owners from high-risk jurisdictions
- Unusual ownership arrangements
