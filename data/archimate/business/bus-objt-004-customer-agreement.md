---
id: bus-objt-004-customer-agreement
name: Customer Agreement
type: business-object
layer: business
relationships:
- type: association
  target: bus-objt-011-party
  description: Entered into by party
- type: association
  target: bus-objt-015-signatory
  description: Signed by authorized signatories
- type: realization
  target: bus-objt-007-customer-relationship
  description: Establishes customer relationship
- type: access
  target: bus-objt-013-product-subscription
  description: Grants access to products
- type: composition
  target: bus-objt-003-customer-account
  description: May establish account relationship
properties:
  owner: Legal and Product Management
  status: active
  criticality: critical
  last-updated: '2026-02-02'
  legacy-id: bus-objt-customer-agreement-001
tags:
- business-object
- legal-agreement
- contract
- customer-relationship
---

# Customer Agreement

Business object representing the contractual agreement that establishes and governs the banking relationship between the bank and a customer (party).

## Description

Customer Agreement is the legal foundation of all banking relationships. It documents the parties (customer and bank), products/services involved, terms and conditions, authorized signatories, and effective period. The agreement creates and defines the Customer Relationship.

## Attributes

### Agreement Identification
- Agreement ID (unique)
- Agreement type (personal banking, business banking, SME, corporate, etc.)
- Agreement number (contract number)
- Product agreement (single or umbrella agreement)
- Version number
- Language(s) of agreement

### Party Information
- Bank party (always the bank)
- Customer party (natural person or legal entity)
- Party contact information
- Authorized signatories (who can sign)
- Authorized representatives (who can act)

### Agreement Terms
- Agreement commencement date
- Agreement expiry date (if applicable)
- Renewal terms (automatic, requires action)
- Termination notice period
- Early termination clause? (Yes/No)
- Early termination conditions
- Amendment procedures

### Products & Services Included
- List of products authorized
- List of services authorized
- Transaction limits by product
- Channel access (mobile, online, branch, ATM, phone)
- Geographic scope
- Currency scope

### Signatory Authorization
- Primary signatory(ies) name(s)
- Primary signatory authority limits
- Secondary signatory(ies) if dual control required
- Signing authority scope (all terms, specific terms)
- Can delegate authority? (Yes/No)
- Signatory changes procedure
- Specimen signatures attached

### Financial Terms
- Interest rate(s) (if applicable)
- Fee structure
- Minimum balance (if applicable)
- Transaction limits
- Credit limit (if applicable)
- Overdraft terms (if applicable)

### Legal Clauses
- Governing law jurisdiction
- Dispute resolution process
- Arbitration clause? (Yes/No)
- Limitation of liability
- Confidentiality provisions
- Compliance representations
- Indemnification clause
- Force majeure provision

### Security & Compliance
- Regulatory disclosures completed? (Yes/No)
- Risk warnings provided? (Yes/No)
- Anti-money laundering verification? (Yes/No)
- Know-your-customer (KYC) completed? (Yes/No)
- Beneficial ownership declaration (if corporate)
- Tax compliance certifications
- Sanctions screening completed? (Yes/No)

### Documentation
- Original agreement location
- Backup/archive location
- Digital copy location
- Scan/OCR performed? (Yes/No)
- Electronic signature used? (Yes/No)
- All schedules/exhibits attached? (Yes/No)
- Amendment documents attached? (Yes/No)

### Execution Details
- Prepared by (bank officer/department)
- Prepared date
- Reviewed by (compliance/legal)
- Review date
- Approved by (manager/director)
- Approval date
- Signed by customer (date)
- Signed by bank representative (date)
- Witness signatures (if required)
- Notarized? (if required)
- Notary details

### Status Information
- Agreement status (active, pending, expired, terminated, suspended)
- Status effective date
- Status change reason (if changed)
- Next review date
- Last review date
- Annual review required? (Yes/No)

### Communication Details
- Communication method preferences
- Delivery address for statements/notices
- Email for electronic delivery
- Language of communication
- Consent to electronic communication? (Yes/No)
- Consent to marketing? (Yes/No)

## Agreement Types

### Personal Banking Agreements
- **Personal Account Agreement**: Basic checking/savings account
- **Personal Loan Agreement**: Consumer credit terms
- **Credit Card Agreement**: Card product terms
- **Investment Agreement**: Investment product terms
- **Deposit Agreement**: Term deposit/savings terms

### Corporate Banking Agreements
- **Master Account Agreement**: Umbrella corporate account terms
- **Cash Management Agreement**: Payment and liquidity services
- **Trade Finance Agreement**: Letter of credit and guarantees
- **Lending Agreement**: Corporate credit facility
- **Treasury Agreement**: FX and hedging services

### Multi-Product Agreements
- **Master Banking Agreement**: Covers multiple products
- **Relationship Agreement**: Overall customer relationship framework
- **Service Agreement**: Service delivery terms
- **Platform Agreement**: Digital banking platform access

## Agreement Lifecycle

### 1. Preparation
- Determine agreement type
- Prepare standard form
- Customize terms if needed
- Include all required disclosures
- Prepare for customer review

### 2. Customer Review & Negotiation
- Provide to customer
- Allow review period
- Address questions/concerns
- Negotiate non-standard terms (if applicable)
- Customer approval

### 3. Execution
- Obtain customer signature(s)
- Verify signatory authority
- Obtain bank representative signature
- Notarize if required
- Create executed copy

### 4. Documentation
- File original safely
- Create audit trail
- Store digitally
- Communicate terms to relevant systems
- Alert product teams

### 5. Active Management
- Monitor compliance with terms
- Track renewal dates
- Monitor for amendments needed
- Track authorization changes
- Annual review/refresh

### 6. Termination or Renewal
- At expiry: decide renewal or termination
- Notify customer of upcoming expiry
- Obtain renewal signature if continuing
- Close account if terminating
- Archive documentation

## Business Rules

- Every customer must have at least one executed agreement
- Agreement must be signed by authorized signatories
- Personal customers sign in individual capacity
- Corporate customers sign through authorized representatives
- All required disclosures must be included
- Agreement must reference all applicable products
- Signatory authority must be verified before execution
- Terms must comply with applicable regulations
- Original must be retained per regulatory requirements
- Amendments must be documented and executed
- Annual reviews required per policy

## Regulatory Requirements

- Disclosure of terms and conditions
- Risk warnings for investment products
- Interest rate or fee disclosure
- Limitations and liabilities
- Dispute resolution procedures
- Regulatory information (PSD2, MiFID, etc.)
- Tax compliance certifications
- AML/KYC verifications
- Beneficial ownership declaration
- Data protection notices
- Right to cancel clauses (if applicable)
- Complaint handling procedure
- Cooling-off period (if applicable)

## Special Cases

- **Joint Account Agreement**: Multiple signatories, separate provisions
- **Minor Account Agreement**: Parental/guardian consent required
- **Power of Attorney Agreement**: Authority delegation provisions
- **Corporate Restructuring**: Agreement updates for M&A, reorganization
- **Dormant Account Conversion**: Terms change for reactivation
