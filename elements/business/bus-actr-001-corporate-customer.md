---
id: bus-actr-001-corporate-customer
name: Corporate Customer
type: business-actor
layer: business
relationships:
- type: assignment
  target: bus-role-002-business-account-holder
  description: Operates business accounts
- type: access
  target: bus-objt-002-business-account
  description: Accesses corporate accounts
properties:
  owner: Chief Business Officer
  status: active
  criticality: high
  last-updated: '2026-02-02'
  legacy-id: bus-actor-corporate-customer-001
tags:
- customer
- corporate-banking
- business-actor
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
