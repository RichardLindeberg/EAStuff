---
id: bus-objt-natural-person-001
name: Natural Person
type: business-object
layer: business
relationships:
  - type: composition
    target: bus-objt-party-001
    description: Is a type of party
  - type: association
    target: bus-objt-beneficial-owner-001
    description: Can be beneficial owner
  - type: association
    target: bus-objt-representative-001
    description: Can be representative
  - type: association
    target: bus-objt-signatory-001
    description: Can be signatory
  - type: access
    target: bus-objt-customer-agreement-001
    description: Can sign agreements
properties:
  owner: Customer Data Management
  status: active
  criticality: critical
  last-updated: "2026-02-02"
tags:
  - business-object
  - party-model
  - natural-person
  - consumer
---

# Natural Person

Business object representing an individual human being in banking relationships.

## Description

Natural Person represents individual customers, officers, beneficial owners, and representatives. Each natural person is uniquely identified and tracked for compliance and relationship management.

## Attributes

### Personal Identification
- Personal ID (unique)
- Full legal name
- Date of birth
- Gender
- Nationality/Nationalities
- Country of residence
- Country of domicile

### Identity Documentation
- Document type (passport, ID, driver's license)
- Document number
- Issuing country
- Issue date
- Expiry date
- Document verification status
- Face verification (if conducted)

### Contact Information
- Home address (complete with postal code)
- Mailing address (if different)
- Phone number(s)
- Email address(es)
- Preferred contact method

### Employment & Income
- Employment status
- Employer name and address
- Job title/Position
- Employment start date
- Annual income
- Income sources
- Source of wealth documentation

### Beneficial Ownership Information
- Is beneficial owner of entities? (Yes/No)
- Ownership percentage in each entity
- Control mechanisms
- UBO (Ultimate Beneficial Owner) flag

### Representation Authority
- Authorized to represent entities? (Yes/No)
- List of entities represented
- Scope of authority
- Authorization limits

### Compliance Information
- Tax residency (country)
- Tax ID/SSN
- FATCA status
- CRS status
- PEP (Politically Exposed Person) flag
- Sanctions screening result
- AML risk rating
- Compliance review date
- Adverse media hit flag

### KYC/AML
- KYC verification status
- KYC verification date
- Document verification level (basic, standard, enhanced)
- Sanctions list screening result
- Sanctions list screening date
- Risk rating

### Legal & Account Status
- Legal capacity status
- Mental competency status
- Guardianship/Power of attorney (if applicable)
- Bankruptcy status
- Court orders or restrictions

## Natural Person Classifications

### By Customer Type
- **Retail Individual**: Primary personal banking customer
- **High Net Worth**: Elevated wealth threshold
- **Business Owner**: Runs own business
- **Professional**: Salaried professional
- **Retiree**: Pension-income customer
- **Student**: Educational status

### By Role
- **Primary Customer**: Direct relationship with bank
- **Secondary Account Owner**: Joint account holder
- **Authorized User**: Card or account user
- **Representative**: Authorized agent for entity
- **Beneficial Owner**: Ownership interest in entity
- **Signatory**: Authority to sign on behalf of entity

## Business Rules

- Minimum age requirements (typically 18)
- Legal capacity verification required
- At least one form of government ID required
- Beneficial ownership disclosure mandatory if applicable
- Representation authority must be documented
- PEP/Sanctions screening mandatory
- AML risk assessment required
- CRS/FATCA self-certification required

## Data Security

- Personal data encrypted
- Access restricted by role
- Audit trail on all access
- GDPR compliance mandatory
- Right to access/erasure honored
- Data minimization practiced
