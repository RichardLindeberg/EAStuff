---
id: bus-proc-001-account-management-process
owner: Retail Banking Operations
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
  - type: realization
    target: bus-srvc-001-account-service
    description: Realizes account service
  - type: access
    target: bus-objt-003-customer-account
    description: Manages account data
  - type: association
    target: bus-role-001-account-holder
    description: Performed by account holders
name: Account Management Process
tags:
  - process
  - account-management
  - retail-banking
archimate:
  type: business-process
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-proc-account-management-001
---
# Account Management Process

End-to-end process for managing customer accounts throughout their lifecycle.

## Description

The Account Management Process encompasses all activities related to creating, maintaining, modifying, and closing customer accounts. This includes onboarding, profile updates, service changes, and account closure.

## Process Steps

### 1. Account Opening
- Customer identification and verification
- KYC/AML checks
- Account type selection
- Terms and conditions acceptance
- Initial deposit
- Account activation

### 2. Account Maintenance
- Profile updates
- Service additions/removals
- Limit modifications
- Beneficiary management
- Preference configuration

### 3. Account Monitoring
- Transaction monitoring
- Balance tracking
- Limit compliance
- Fraud detection
- Dormancy management

### 4. Account Closure
- Closure request validation
- Outstanding balance settlement
- Final statement generation
- Data archival
- Regulatory reporting

## Key Performance Indicators

- Account opening time: < 15 minutes
- Profile update time: < 5 minutes
- Closure completion: < 2 business days
- Data accuracy: > 99.9%
- Customer satisfaction: > 4.5/5

## Regulatory Requirements

- KYC/AML compliance
- GDPR data protection
- Financial record retention
- Audit trail maintenance
