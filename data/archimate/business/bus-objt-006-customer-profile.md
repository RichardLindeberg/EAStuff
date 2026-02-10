---
id: bus-objt-006-customer-profile
owner: Customer Data Management
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
  - type: composition
    target: bus-objt-005-customer-object
    description: Contains customer master data
  - type: association
    target: bus-proc-001-account-management-process
    description: Updated by account management
name: Customer Profile
tags:
  - business-object
  - customer-data
  - master-data
archimate:
  type: business-object
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-objt-customer-profile-001
---
# Customer Profile

Business object representing customer master data and profile information.

## Description

The Customer Profile is the single source of truth for customer information including identity, contact details, preferences, and relationship data.

## Attributes

### Personal Information
- Customer ID (unique)
- Title
- First name
- Middle name
- Last name
- Date of birth
- Gender
- Nationality
- Marital status

### Identity Documentation
- ID type (passport, national ID, driver's license)
- ID number
- Issuing country
- Issue date
- Expiry date
- ID verification status

### Contact Information
- Primary address
- Mailing address
- Phone numbers (mobile, home, work)
- Email addresses
- Preferred contact method
- Language preference

### Employment Information
- Employment status
- Employer name
- Job title
- Industry
- Years employed
- Monthly income
- Income source

### Financial Information
- Annual income
- Net worth
- Source of wealth
- Credit rating
- Risk classification
- Product holdings

### Relationship Details
- Customer since date
- Customer segment
- Relationship manager
- Preferred branch
- Service tier
- Loyalty status

### Preferences
- Communication preferences
- Channel preferences
- Statement delivery (paper/electronic)
- Marketing consent
- Language preference
- Accessibility requirements

### Compliance Information
- KYC status
- KYC last updated
- AML risk rating
- PEP (Politically Exposed Person) status
- Sanctions screening status
- Tax residency
- FATCA status
- CRS reporting

## Profile Segments

- **Mass Market**: Standard customers
- **Mass Affluent**: Higher balance customers
- **Affluent**: High net worth individuals
- **Private Banking**: Ultra high net worth
- **Student**: Educational segment
- **Senior**: Retired customers
- **Business Owner**: Entrepreneur segment

## Data Quality Rules

- Mandatory fields validation
- Format validation (email, phone)
- Address verification
- Duplicate detection
- Consistency checks
- Regular data refresh
- Opt-out management

## Privacy & Security

- GDPR compliance
- Data minimization
- Purpose limitation
- Consent management
- Right to access
- Right to erasure
- Data portability
- Encryption at rest
