---
id: bus-role-001-account-holder
name: Account Holder
type: business-role
layer: business
relationships:
- type: assignment
  target: bus-proc-001-account-management-process
  description: Manages accounts
- type: serving
  target: bus-srvc-001-account-service
  description: Uses account services
properties:
  owner: Retail Banking Division
  status: active
  criticality: high
  last-updated: '2026-02-02'
  legacy-id: bus-role-account-holder-001
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
