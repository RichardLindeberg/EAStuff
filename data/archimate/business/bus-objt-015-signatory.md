---
id: bus-objt-015-signatory
owner: Legal and Compliance
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
  - type: composition
    target: bus-objt-010-natural-person
    description: Is always a natural person
  - type: association
    target: bus-objt-014-representative
    description: May also be a representative
  - type: association
    target: bus-objt-004-customer-agreement
    description: Signs customer agreements
name: Signatory
tags:
  - business-object
  - legal-authority
  - agreement
  - contract-execution
archimate:
  type: business-object
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-objt-signatory-001
---
# Signatory

Business object representing natural persons who have legal authority to sign agreements and contracts on behalf of a party.

## Description

Signatories are natural persons with documented legal authority to execute and bind a party (individual or organization) to agreements and contracts. Signatory status is distinct from general representative authority and specifically relates to contract execution.

## Attributes

### Signatory Identification
- Signatory ID
- Natural Person reference
- Party represented (Legal Entity or individual)
- Signatory designation (who authorized this person)

### Signatory Authority Details
- Signatory title
- Scope of signing authority (all products, limited products)
- Contract types authorized (account agreements, loan agreements, etc.)
- Signing limits (if any)
- Counter-signature requirement?
- Can bind party to third parties?

### Legal Authority
- Authority source (company bylaws, board resolution, power of attorney, individual capacity)
- Authority documentation type
- Authority document reference
- Authority issue date
- Authority expiry date (if applicable)
- Authorized by (person or body)
- Board resolution number (if applicable)

### Signatory Documentation
- Specimen signature on file
- Signature verification completed? (Yes/No)
- Signature verification date
- Photo ID on file
- Identity verification completed? (Yes/No)
- Identity verification date
- Authority documentation on file
- Authority documentation location

### Contact Information
- Phone number
- Email address
- Office address
- Delivery address for notices

### Compliance & Verification
- Background check completed? (Yes/No)
- Background check result
- Identity documents verified? (Yes/No)
- Signatory authority verified? (Yes/No)
- Verification date
- Verifying officer
- Authority confirmation letter sent? (Yes/No)
- Authority confirmation acknowledged? (Yes/No)
- Authority confirmation date
- Sanctions screening completed? (Yes/No)
- PEP check completed? (Yes/No)
- Overall compliance status

### Status Information
- Signatory status (active, suspended, revoked)
- Status effective date
- Status change reason
- Change notification date
- Changed by (person and date)

### Signature & Binding Power
- Has execution authority for:
  - Customer agreements
  - Loan agreements
  - Card agreements
  - Payment service agreements
  - Investment agreements
  - Other agreements
- Cannot execute (if any restrictions)
- Requires counter-signature? (Yes/No)
- Counter-signatory required (if applicable)
- Can initiate but not finalize?

## Signatory Authority Levels

### Full Authority
- Can sign all contract types
- No monetary limit
- No counter-signature required
- Can bind party absolutely

### Limited Authority
- Specific contract types only
- Monetary limits per contract
- May require counter-signature
- Limited scope of binding

### Conditional Authority
- Authority subject to conditions
- Conditions specified in authorization
- May require additional approvals
- Authority may be revocable

## Signatory Classification

### By Role
- **Director/CEO**: Full signing authority
- **Senior Manager**: Department-level authority
- **Authorized Officer**: Specific signing authority
- **Limited Signatory**: Restricted products/amounts
- **Emergency Signatory**: Authority in emergencies only

### By Authority
- **Individual Capacity**: Person signing for themselves
- **Corporate Capacity**: Person signing for entity
- **Representative Capacity**: Person signing for another (power of attorney)
- **Trustee Capacity**: Person signing as trustee

## Business Rules

- Signatory must be natural person
- Signatory authority must be documented
- Signatory authority must be verified
- No conflict of interest
- Authority must be within delegation chain
- Authority modifications require authorization
- Suspended signatories cannot execute agreements
- All executed agreements must be signed by authorized signatory
- Specimen signature on file required

## Legal & Regulatory

- Signatory authority presumed binding on party
- Party responsible for signatory acts within authority
- Authority documentation retained (7+ years)
- Change of signatory authority reported to bank
- Bank not responsible for excess authority
- Signatory liable for fraud/unauthorized use
- Authentication may be required (notarization, etc.)

## Signature Verification

- Initial signature specimen collected
- Signature verification at account opening
- Periodic signature verification (annual review)
- High-value transaction signature verification
- Signature comparison in transaction authorization
- Fraud detection on signature inconsistencies
- Digital signature capabilities (if applicable)

## Agreement Execution Process

1. Prepare agreement with signature blocks
2. Identify required signatories
3. Verify signatory authority
4. Obtain signatures
5. Verify signatures match specimens
6. Confirm counter-signatures if required
7. Document execution date and parties
8. Retain signed agreement

## Account & System Access vs. Signatory

- **Representative/Officer**: May have transaction authority without signatory rights
- **Signatory**: Authority specifically for contract execution
- **Administrator**: System access without transaction authority
- **User**: Access to view information
- A person may hold multiple roles with different authorities
