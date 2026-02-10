---
id: bus-role-001-account-holder
owner: Retail Banking Division
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: assignment
  target: bus-proc-001-account-management-process
  description: Manages accounts
- type: serving
  target: bus-srvc-001-account-service
  description: Uses account services
name: Account Holder
tags:
- role
- customer-role
- account-management
archimate:
  type: business-role
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-role-account-holder-001
---
# Account Holder

Role representing customers who hold one or more accounts with the bank.

## Description

The Account Holder role encompasses all responsibilities and permissions related to owning and managing bank accounts. This includes viewing balances, making transactions, and maintaining account settings.

## Permissions

- View account balances and transactions
- Transfer funds between accounts
- Set up beneficiaries
- Configure alerts and notifications
- Update account preferences
- Request statements and reports

## Responsibilities

- Maintain accurate account information
- Report suspicious activities
- Comply with account terms and conditions
- Maintain sufficient funds for obligations
- Review statements regularly

## Typical Actors

- Retail customers
- Joint account holders
- Business representatives
