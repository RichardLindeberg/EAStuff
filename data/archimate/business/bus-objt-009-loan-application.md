---
id: bus-objt-009-loan-application
owner: Lending Division
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: bus-proc-007-loan-origination-process
  description: Managed by loan origination
- type: association
  target: bus-proc-004-credit-assessment-process
  description: Evaluated by credit assessment
- type: association
  target: bus-objt-005-customer-object
  description: Related to customer
name: Loan Application
tags:
- business-object
- lending
- application
archimate:
  type: business-object
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-objt-loan-application-001
---
# Loan Application

Business object representing a customer's loan application.

## Description

A Loan Application captures all information related to a customer's request for credit including personal details, financial information, loan requirements, and supporting documentation.

## Attributes

### Application Details
- Application ID (unique)
- Application date
- Product type
- Loan amount requested
- Loan term
- Purpose
- Priority (standard, expedited)

### Applicant Information
- Primary applicant
- Co-applicants
- Relationship to applicant
- Employment details
- Income information
- Asset details
- Liability information

### Loan Requirements
- Requested amount
- Preferred term
- Repayment frequency
- Preferred start date
- Collateral offered (if secured)
- Guarantors (if applicable)

### Assessment Results
- Credit score
- Risk rating
- DTI ratio
- Affordability score
- Decision recommendation
- Approved amount
- Approved terms
- Interest rate
- Fees

### Status Information
- Application status
- Current stage
- Assigned underwriter
- Decision date
- Approval/decline reason
- Appeals status

### Documentation
- ID documents
- Income proof
- Bank statements
- Tax returns
- Employment verification
- Asset documentation
- Collateral valuation
- Credit reports

## Application Statuses

1. **Draft**: Started but not submitted
2. **Submitted**: Awaiting review
3. **In Review**: Under assessment
4. **Additional Info Required**: Awaiting documents
5. **In Underwriting**: Manual review
6. **Approved**: Credit approved
7. **Declined**: Application rejected
8. **Withdrawn**: Customer withdrew
9. **Expired**: Time limit exceeded
10. **Disbursed**: Funds released

## Decision Outcomes

- **Approved**: Full approval as requested
- **Approved with Conditions**: Modified terms
- **Referred**: Requires senior review
- **Declined**: Application rejected
- **Counter Offer**: Alternative terms offered

## Business Rules

- Complete applications only
- Income verification mandatory
- Credit check authorization required
- Minimum age requirement (18)
- Maximum age at loan maturity
- Debt-to-income limits
- Minimum credit score thresholds
