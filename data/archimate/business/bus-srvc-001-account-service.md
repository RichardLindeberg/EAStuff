---
id: bus-srvc-001-account-service
owner: Retail Banking Division
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: bus-proc-001-account-management-process
  description: Realized by account management process
- type: serving
  target: bus-role-001-account-holder
  description: Serves account holders
name: Account Service
tags:
- service
- retail-banking
- account-management
archimate:
  type: business-service
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-svc-account-service-001
---
# Account Service

Business service providing account management capabilities to customers.

## Description

The Account Service offers customers the ability to manage their bank accounts including viewing balances, updating information, and configuring preferences.

## Service Functions

- View account balances and details
- Review transaction history
- Update contact information
- Manage beneficiaries
- Configure alerts and notifications
- Request statements
- Download transaction data
- Set account preferences

## Service Levels

- Availability: 99.9%
- Response time: < 2 seconds
- 24/7 access
- Multi-channel delivery

## Channels

- Mobile banking app
- Online banking portal
- ATMs
- Branch terminals
- Phone banking
- API access
