---
id: bus-role-business-account-holder-001
name: Business Account Holder
type: business-role
layer: business
relationships:
  - type: assignment
    target: bus-proc-corporate-account-management-001
    description: Manages corporate accounts
  - type: serving
    target: bus-svc-corporate-banking-service-001
    description: Uses corporate banking services
properties:
  owner: Corporate Banking Division
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - role
  - corporate-role
  - business-banking
---

# Business Account Holder

Role for authorized representatives managing corporate bank accounts.

## Description

Business Account Holders are authorized individuals within an organization who manage corporate bank accounts and execute financial transactions on behalf of their company.

## Permissions

- View corporate account details
- Initiate payments and transfers
- Manage payroll operations
- Access trade finance services
- Configure account parameters
- Authorize transactions per signing authority
- Generate financial reports

## Responsibilities

- Execute approved financial transactions
- Maintain proper authorization controls
- Ensure compliance with company policies
- Monitor account activity
- Report discrepancies or fraud
- Maintain audit trails

## Authorization Levels

- Single signatory
- Dual authorization
- Multi-level approval
- Role-based access control
