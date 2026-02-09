---
id: bus-actr-001-corporate-customer
owner: Chief Business Officer
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
  - type: assignment
    target: bus-role-002-business-account-holder
    description: Operates business accounts
  - type: access
    target: bus-objt-002-business-account
    description: Accesses corporate accounts
name: Corporate Customer
tags:
  - customer
  - corporate-banking
  - business-actor
archimate:
  type: business-actor
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-actor-corporate-customer-001
---
# Corporate Customer

Business entities and organizations using the bank's corporate banking services.

## Description

Corporate customers are businesses, SMEs, and large enterprises that use the bank's commercial banking products and services. They require specialized solutions for cash management, trade finance, and treasury services.

## Characteristics

- Complex financial requirements
- Multiple authorized users
- High transaction volumes
- Regulatory compliance needs
- Integration requirements with accounting systems

## Key Services

- Cash management
- Payment processing
- Trade finance
- Treasury services
- Corporate lending
- FX services

## Digital Requirements

- API integration capabilities
- Multi-user access controls
- Advanced reporting
- Real-time liquidity management
- Automated reconciliation
