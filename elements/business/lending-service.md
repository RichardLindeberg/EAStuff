---
id: bus-svc-lending-service-001
name: Lending Service
type: business-service
layer: business
relationships:
  - type: realization
    target: bus-proc-loan-origination-001
    description: Realized by loan origination process
  - type: serving
    target: bus-role-account-holder-001
    description: Serves customers
properties:
  owner: Lending Division
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - service
  - lending
  - credit
---

# Lending Service

Business service providing loan and credit products to customers.

## Description

The Lending Service offers customers access to various loan products including personal loans, mortgages, auto loans, and lines of credit with streamlined application and approval processes.

## Service Offerings

- Loan pre-qualification
- Online loan application
- Document upload
- Application status tracking
- Instant decision (eligible products)
- Digital loan agreement
- Fund disbursement
- Repayment management

## Product Types

- Personal loans
- Home mortgages
- Auto financing
- Lines of credit
- Business loans

## Service Levels

- Application to decision: < 24 hours
- Digital application completion: < 15 minutes
- Customer satisfaction: > 4.0/5
- Approval rate tracking
