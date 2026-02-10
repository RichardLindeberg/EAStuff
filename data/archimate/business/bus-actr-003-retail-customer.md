---
id: bus-actr-003-retail-customer
owner: Chief Customer Officer
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: assignment
  target: bus-role-001-account-holder
  description: Can be an account holder
- type: access
  target: bus-objt-003-customer-account
  description: Accesses their accounts
name: Retail Customer
tags:
- customer
- retail-banking
- business-actor
archimate:
  type: business-actor
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-actor-retail-customer-001
---
# Retail Customer

Individual customers using the bank's retail banking services.

## Description

Retail customers are individual consumers who use the bank's products and services for personal financial needs. They interact with the bank through multiple channels including mobile apps, web banking, branches, and ATMs.

## Characteristics

- Primary user base for digital banking platforms
- Multi-channel engagement preference
- Varying levels of digital literacy
- Diverse financial needs and goals

## Key Interactions

- Account management
- Payment transactions
- Loan applications
- Investment services
- Customer support

## Digital Expectations

- 24/7 access to services
- Mobile-first experience
- Instant notifications
- Personalized recommendations
- Seamless omnichannel experience
