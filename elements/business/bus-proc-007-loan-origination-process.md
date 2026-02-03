---
id: bus-proc-007-loan-origination-process
name: Loan Origination Process
type: business-process
layer: business
relationships:
- type: realization
  target: bus-srvc-004-lending-service
  description: Realizes lending services
- type: access
  target: bus-objt-009-loan-application
  description: Manages loan applications
- type: access
  target: bus-objt-005-customer-object
  description: Accesses customer data
- type: triggering
  target: bus-proc-004-credit-assessment-process
  description: Triggers credit evaluation
properties:
  owner: Lending Division
  status: active
  criticality: high
  last-updated: '2026-02-02'
  legacy-id: bus-proc-loan-origination-001
tags:
- process
- lending
- credit
- origination
---

# Loan Origination Process

End-to-end process for processing and approving loan applications.

## Description

The Loan Origination Process manages the complete lifecycle of loan applications from initial inquiry through to disbursement. This includes application intake, credit assessment, underwriting, approval, and fund disbursement.

## Process Stages

### 1. Pre-Qualification
- Customer inquiry
- Eligibility check
- Product recommendation
- Preliminary offer
- Application invitation

### 2. Application
- Application form completion
- Document collection
- Income verification
- Employment verification
- Identity verification

### 3. Credit Assessment
- Credit score retrieval
- Credit history analysis
- Debt-to-income calculation
- Affordability assessment
- Risk rating assignment

### 4. Underwriting
- Application review
- Document verification
- Collateral valuation (if secured)
- Terms and conditions determination
- Risk-based pricing
- Decision recommendation

### 5. Approval/Decline
- Credit committee review (if required)
- Final decision
- Offer letter generation
- Customer notification
- Appeals process (if declined)

### 6. Documentation
- Loan agreement preparation
- Terms acceptance
- Legal documentation
- Signatures collection
- Compliance verification

### 7. Disbursement
- Final checks
- Fund release
- Account setup
- Repayment schedule activation
- Customer confirmation

## Loan Types

- Personal loans
- Mortgage loans
- Auto loans
- Business loans
- Line of credit
- Student loans

## Performance Metrics

- Application to decision: < 24 hours (personal), < 5 days (mortgage)
- Approval rate: Industry benchmark
- Documentation completeness: > 95%
- Customer satisfaction: > 4.0/5
- Default rate: < 2%

## Compliance Requirements

- Responsible lending regulations
- Credit Act compliance
- Anti-discrimination laws
- Privacy and data protection
- Truth in lending disclosure
- Fair credit reporting
