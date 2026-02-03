---
id: bus-objt-003-customer-account
name: Customer Account
type: business-object
layer: business
relationships:
- type: access
  target: bus-role-001-account-holder
  description: Accessed by account holders
- type: access
  target: bus-proc-001-account-management-process
  description: Managed by account management process
- type: composition
  target: bus-objt-016-transaction
  description: Contains transactions
properties:
  owner: Retail Banking Division
  status: active
  criticality: critical
  last-updated: '2026-02-02'
  legacy-id: bus-objt-customer-account-001
tags:
- business-object
- account
- core-data
---

# Customer Account

Core business object representing a customer's bank account.

## Description

A Customer Account represents the contractual relationship between the bank and customer for holding and managing funds. Each account has a unique identifier, holds a balance, and maintains a transaction history.

## Attributes

### Identification
- Account number (unique)
- Account type (savings, checking, etc.)
- IBAN
- Branch code
- Account name

### Financial
- Current balance
- Available balance
- Currency
- Interest rate
- Fee structure
- Credit/debit limits

### Status & Control
- Account status (active, frozen, closed)
- Opening date
- Last activity date
- Overdraft limit
- Hold amounts
- Restrictions

### Customer Details
- Primary account holder
- Joint account holders
- Beneficial owners
- Authorized signatories
- Contact preferences

## Account Types

- **Checking Account**: Daily transaction account
- **Savings Account**: Interest-bearing savings
- **Money Market**: Higher interest with restrictions
- **Term Deposit**: Fixed-term investment
- **Youth Account**: For minors with restrictions
- **Senior Account**: For seniors with benefits

## Business Rules

- Minimum balance requirements
- Fee schedule application
- Interest calculation method
- Overdraft policies
- Dormancy rules
- Closure procedures

## Lifecycle States

1. **Pending**: Application submitted
2. **Active**: Operational account
3. **Frozen**: Temporarily blocked
4. **Dormant**: No activity for extended period
5. **Closed**: Permanently closed

## Regulatory Compliance

- KYC/AML verification required
- Beneficial ownership disclosure
- GDPR data protection
- Financial record retention
- Reporting obligations
