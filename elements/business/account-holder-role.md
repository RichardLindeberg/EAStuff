---
id: bus-role-account-holder-001
name: Account Holder
type: business-role
layer: business
relationships:
  - type: assignment
    target: bus-proc-account-management-001
    description: Manages accounts
  - type: serving
    target: bus-svc-account-service-001
    description: Uses account services
properties:
  owner: Retail Banking Division
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - role
  - customer-role
  - account-management
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
