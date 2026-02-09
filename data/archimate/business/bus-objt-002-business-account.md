---
id: bus-objt-002-business-account
name: Business Account
type: business-object
layer: business
relationships:
- type: association
  target: bus-role-002-business-account-holder
  description: Accessed by business account holders
- type: association
  target: bus-proc-003-corporate-account-management-process
  description: Managed by corporate account management
- type: composition
  target: bus-objt-016-transaction
  description: Contains transactions
properties:
  owner: Corporate Banking Division
  status: active
  criticality: critical
  last-updated: '2026-02-02'
  legacy-id: bus-objt-business-account-001
tags:
- business-object
- corporate-account
- business-banking
---

# Business Account

Business object representing corporate and commercial bank accounts.

## Description

A Business Account represents the banking relationship with corporate customers, supporting complex requirements such as multi-user access, approval workflows, and high transaction volumes.

## Attributes

### Business Identification
- Account number
- Company registration number
- Tax identification
- Business type (LLC, Corp, Partnership, etc.)
- Industry classification
- Parent company (if applicable)

### Account Configuration
- Account structure (multi-currency, pooling)
- Authorized signatories
- Approval matrix
- Transaction limits per user/role
- Notification rules
- Integration endpoints

### Financial Information
- Multiple currency balances
- Consolidated position
- Credit facilities
- Overdraft arrangements
- Interest structures
- Fee schedules

### Operational Settings
- Cut-off times
- Value dating rules
- Statement frequency
- Reporting requirements
- API access credentials
- SWIFT/BIC details

## Account Types

- **Operating Account**: Daily business transactions
- **Payroll Account**: Salary disbursements
- **Savings Account**: Surplus fund management
- **Escrow Account**: Held funds for specific purposes
- **Trust Account**: Fiduciary arrangements
- **Virtual Accounts**: Automated reconciliation

## Authorization Structure

- Single signatory
- Dual authorization
- Multi-level approval
- Role-based access
- Delegation hierarchy
- Emergency override

## Business Rules

- Signing authority limits
- Transaction approval workflows
- Dual control requirements
- Segregation of duties
- Audit trail requirements
- Compliance monitoring

## Integration Capabilities

- ERP connectivity
- Treasury management systems
- Payment hubs
- Multi-bank platforms
- SWIFT network
- API access
