---
id: bus-proc-002-beneficial-ownership-identification-proc
owner: Compliance and AML
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: bus-objt-001-beneficial-owner
  description: Identifies beneficial owners
- type: access
  target: bus-objt-008-legal-entity
  description: For legal entity customers
name: Beneficial Ownership Identification Process
tags:
- process
- beneficial-ownership
- compliance
- regulatory
archimate:
  type: business-process
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-proc-beneficial-ownership-identification-001
---
# Beneficial Ownership Identification Process

Process for identifying, verifying, and documenting beneficial owners of corporate customers in compliance with regulatory requirements (5AMLD, FinCEN, local regulations).

## Description

The Beneficial Ownership Identification Process ensures every corporate customer has properly identified and verified beneficial owners. This is critical for AML/KYC compliance and addresses regulatory transparency requirements across EU 5AMLD, US FinCEN, and equivalent local regulations.

## Process Steps

### 1. Beneficial Ownership Declaration
- Provide beneficial ownership declaration form
- Define "beneficial owner" per regulations (25%+ ownership/control)
- Request list of all beneficial owners
- Request ownership percentage per owner
- Request control mechanism documentation
- Request UBO identification

### 2. Initial Information Gathering
- Document review (articles of association, shareholder register)
- Corporate structure analysis
- Ownership chain mapping
- Intermediate entity analysis (if any)
- Trust/settlement documentation review (if applicable)
- Organizational chart review

### 3. Beneficial Owner Identification
- Identify each beneficial owner
- Determine ownership type (direct, indirect, through trust)
- Document ownership percentage
- Document control mechanisms
- Document UBO (Ultimate Beneficial Owner)
- Identify joint beneficial owners (if any)

### 4. Natural Person Verification
- Obtain identity documents
- Verify identity documents
- Verify natural person name against ownership documentation
- Document relationship to legal entity
- Obtain contact information
- Document verification date

### 5. Risk Assessment
- PEP (Politically Exposed Person) screening
- PEP family member identification
- PEP close associate identification
- Sanctions list screening
- AML risk rating
- High-risk jurisdiction check
- Conflict of interest assessment

### 6. Documentation & Evidence
- Gather supporting documentation:
  - Company registry extracts
  - Shareholder register
  - Board resolutions
  - Articles of incorporation/association
  - Trust documents (if applicable)
  - Organizational charts
  - Identity documents (beneficial owners)
  - Beneficial ownership declaration form (signed)
- Store documentation securely
- Create audit trail

### 7. Verification & Confirmation
- Confirm beneficial ownership through independent sources
- Verify ownership percentages
- Verify control mechanisms
- Confirm UBO identification
- Document verification source/method
- Obtain management certification (if required)

### 8. Compliance Review
- Verify all beneficial owners identified
- Verify beneficial owner verification completed
- Verify PEP/Sanctions screening completed
- Verify documentation complete
- Verify identification reliable
- Complete compliance sign-off

### 9. System Update
- Input beneficial owner information
- Flag high-risk beneficial owners
- Update customer risk profile
- Set review date
- Configure monitoring rules
- Send confirmation to customer

### 10. Customer Communication
- Provide beneficial ownership confirmation
- Confirm beneficial owner details captured correctly
- Notify of ongoing monitoring
- Explain ongoing obligations
- Provide change notification procedures
- Confirm annual review requirement

## Beneficial Ownership Types

### Direct Ownership
- Direct shareholding in corporate customer
- Clear ownership documentation
- Simple verification

### Indirect Ownership
- Ownership through intermediate entities
- Ownership chain documentation required
- Look-through to ultimate owner required
- May require verification from intermediate entities

### Beneficial Interest Through Trust
- Trustee owns shares
- Beneficiary has economic interest
- Trust documentation required
- Both trustee and beneficiary may require identification

### De Facto Control Without Ownership
- Control through voting agreements
- Control through management position
- Control through board representation
- Documentation of control mechanism required

### Joint Beneficial Owners
- Multiple beneficial owners with combined control
- Combined ownership > 25%
- Each beneficial owner individually identified
- Separate verification for each

## Difficult-to-Identify Scenarios

### Public Companies
- Diffuse shareholding
- Regulation/policy determines treatment
- Often treated as transparent (no specific UBO)
- Document basis for determination

### Shell Companies
- Nominee shareholders
- Lack of substantive business
- Higher risk classification
- Enhanced due diligence required
- May require refusal to onboard

### Employee Stock Ownership Plans (ESOPs)
- Employee ownership
- Trustee holds shares
- Individual employees as beneficial owners
- May use ESOP representative

### Offshore Entities
- Complex ownership structures
- Intermediate jurisdictions
- Transparency requirements strict
- May require additional documentation
- Enhanced due diligence likely

### Government-Controlled Entities
- Government as beneficial owner
- Special treatment per policy
- May be exempt from standard requirements
- Regulatory approval may be required

## Verification Methods

- **Company Registry**: Official corporate records
- **Stock Exchange Database**: Public company ownership
- **Trust Deeds**: Legal documentation of trust
- **Board Resolutions**: Corporate decisions
- **Power of Attorney**: Authority documentation
- **Certified Extracts**: From corporate records
- **Third-party Verification**: Specialist providers
- **Customer Declaration**: Customer attestation
- **Management Certification**: Management confirmation

## Annual Review & Update

- Annual beneficial ownership review required
- Request updated beneficial ownership declaration
- Verify no changes since last review
- Update verification dates
- Assess any changes in beneficial ownership
- Trigger investigations if changes detected
- Update risk profile if needed

## Change Management

When beneficial ownership changes:
- Customer must notify bank immediately
- New beneficial ownership declaration required
- New verification documentation required
- PEP/Sanctions screening of new beneficial owners
- Risk reassessment
- Compliance review
- Account review for potential impact
- Customer agreement amendment (if needed)

## Regulatory Reporting

- Report beneficial owner data to authorities (if required)
- Report complex/suspicious ownership structures
- Annual beneficial ownership updates
- Immediate reporting of beneficial owner changes
- Reporting of inability to identify beneficial owners
- Documentation retention per requirements

## Business Rules

- All corporate customers must have identified beneficial owners
- Every identified beneficial owner must be a natural person
- If no identifiable beneficial owner exists per policy, entity itself treated as party
- Beneficial ownership > 25% threshold per regulations
- PEP status must be checked for all beneficial owners
- Annual reviews mandatory
- Changes must trigger review process
- Documentation retained per regulatory requirements
- Audit trail maintained for all changes
