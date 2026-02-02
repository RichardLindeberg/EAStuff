---
id: bus-proc-corporate-account-management-001
name: Corporate Account Management Process
type: business-process
layer: business
relationships:
  - type: realization
    target: bus-svc-corporate-banking-service-001
    description: Realizes corporate banking services
  - type: access
    target: bus-objt-business-account-001
    description: Manages corporate accounts
  - type: assignment
    target: bus-role-business-account-holder-001
    description: Performed by business account holders
properties:
  owner: Corporate Banking Division
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - process
  - corporate-banking
  - business-accounts
---

# Corporate Account Management Process

Process for managing corporate and business banking accounts.

## Description

The Corporate Account Management Process handles the specialized requirements of business customers including multi-user access, approval workflows, high transaction volumes, and integration with enterprise systems.

## Key Processes

### 1. Corporate Account Opening
- Business entity verification
- Beneficial ownership identification
- Corporate documentation
- Authorized signatories setup
- Multi-user access configuration
- Integration requirements gathering

### 2. User & Authorization Management
- User provisioning
- Role assignment
- Approval workflow configuration
- Signing authority limits
- Delegation rules
- Access reviews

### 3. Cash Management
- Liquidity monitoring
- Cash positioning
- Funds concentration
- Zero balance accounts
- Sweep arrangements
- Investment of excess funds

### 4. Payment & Collections
- Bulk payment processing
- Payroll execution
- Supplier payment automation
- Customer collections
- Direct debit management
- Virtual account management

### 5. Trade Finance
- Letter of credit issuance
- Bank guarantee processing
- Documentary collections
- Supply chain finance
- Export/import financing

### 6. Treasury Services
- FX trading
- Hedging instruments
- Interest rate management
- Liquidity facility management
- Debt instrument issuance

## Specialized Features

- Multi-entity consolidation
- Inter-company transactions
- Automated reconciliation
- ERP system integration
- SWIFT connectivity
- Host-to-host interfaces
- API access

## Performance Requirements

- Real-time balance information
- Same-day value payments
- 24/7 liquidity management
- Instant payment notifications
- Sub-second authorization
- 99.99% uptime

## Compliance

- Corporate governance requirements
- Financial reporting standards
- AML enhanced due diligence
- Sanctions screening
- Tax reporting (FATCA, CRS)
- Audit trail requirements
