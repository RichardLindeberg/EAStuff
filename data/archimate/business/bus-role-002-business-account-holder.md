---
id: bus-role-002-business-account-holder
owner: Corporate Banking Division
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
  - type: assignment
    target: bus-proc-003-corporate-account-management-process
    description: Manages corporate accounts
  - type: serving
    target: bus-srvc-002-corporate-banking-service
    description: Uses corporate banking services
name: Business Account Holder
tags:
  - role
  - corporate-role
  - business-banking
archimate:
  type: business-role
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-role-business-account-holder-001
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
