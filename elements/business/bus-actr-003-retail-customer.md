---
id: bus-actr-003-retail-customer
name: Retail Customer
type: business-actor
layer: business
relationships:
- type: assignment
  target: bus-role-001-account-holder
  description: Can be an account holder
- type: access
  target: bus-objt-003-customer-account
  description: Accesses their accounts
properties:
  owner: Chief Customer Officer
  status: active
  criticality: critical
  last-updated: '2026-02-02'
  legacy-id: bus-actor-retail-customer-001
tags:
- customer
- retail-banking
- business-actor
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
