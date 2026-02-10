---
id: bus-objt-017-customer-data
owner: Chief Data Officer
status: active
version: '1.0'
last_updated: '2026-02-03'
review_cycle: annual
next_review: '2027-02-03'
relationships:
  - type: composition
    target: bus-objt-006-customer-profile
    description: Contains customer profile information
  - type: composition
    target: bus-objt-003-customer-account
    description: Contains account data
  - type: composition
    target: bus-objt-016-transaction
    description: Contains transaction history
  - type: association
    target: bus-role-003-support-agent
    description: Accessed by support agents (restricted)
  - type: association
    target: bus-actr-002-customer-service-representative
    description: Accessed by customer service representatives
name: Customer Data
tags:
  - customer-data
  - business-object
  - pii
  - gdpr
archimate:
  type: business-object
  layer: business
  criticality: critical
extensions:
  properties:
    data-classification: confidential-pii
    retention-period: per GDPR and banking regulations
---
# Customer Data

Comprehensive collection of customer information used across banking operations, subject to strict data protection requirements.

## Description

Customer Data represents all information related to bank customers, including personal details, account information, transaction history, preferences, and interaction records. In the Nordic banking context, this data is subject to stringent GDPR requirements and must be handled with highest security standards.

## Data Components

### Personal Information
- Full name, date of birth, national identity number (personnummer/fødselsnummer)
- Contact details (address, phone, email)
- Citizenship and tax residency
- Language and communication preferences

### Financial Profile
- Account numbers and balances
- Credit history and scoring
- Income and employment information
- Investment portfolio details

### Transaction History
- Payment history
- Transfer records
- Card transactions
- Account statements

### Service Data
- Products and services held
- Agreements and contracts
- Support interactions and case history
- Marketing preferences and consents

### Compliance Data
- KYC/AML verification records
- Due diligence documentation
- Beneficial ownership information
- FATCA/CRS reporting data

## Data Protection Requirements

### GDPR Compliance
- **Lawful Basis**: Customer consent, contract performance, legal obligation
- **Data Minimization**: Only collect necessary information
- **Purpose Limitation**: Use data only for stated purposes
- **Accuracy**: Maintain current and correct information
- **Storage Limitation**: Delete when no longer needed
- **Integrity & Confidentiality**: Strong security controls

### Nordic-Specific Requirements
- **Swedish GDPR Implementation**: Extended rules for sensitive data
- **Norwegian Personal Data Act**: Additional notification requirements
- **Danish Data Protection Act**: Specific rules for data transfers
- **Finnish Data Protection Act**: Enhanced rights for data subjects

## Access Controls

### Role-Based Access
- Account holders: Full access to their own data
- Customer service: View access with audit logging
- Support agents: Limited access (need-to-know basis)
- Compliance officers: Full access for regulatory purposes
- Data protection officer: Access for oversight

### Security Measures
- End-to-end encryption for data at rest and in transit
- Multi-factor authentication for access
- Comprehensive audit logging
- Regular access reviews
- Data masking for non-production environments

## Data Quality

- Automated validation at point of entry
- Regular data quality assessments
- Customer self-service for updates
- Periodic verification campaigns
- Deduplication and consolidation processes

## Cross-Border Considerations

Nordic banks must manage:
- Cross-border data transfers within EEA
- GDPR requirements for non-EEA transfers
- Country-specific data localization requirements
- Tax information exchange (CRS)
