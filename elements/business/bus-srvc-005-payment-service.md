---
id: bus-srvc-005-payment-service
name: Payment Service
type: business-service
layer: business
relationships:
- type: realization
  target: bus-proc-008-payment-processing
  description: Realized by payment processing
- type: serving
  target: bus-role-001-account-holder
  description: Serves customers
properties:
  owner: Payment Operations
  status: active
  criticality: critical
  last-updated: '2026-02-02'
  legacy-id: bus-svc-payment-service-001
tags:
- service
- payments
- transactions
---

# Payment Service

Business service enabling customers to initiate and manage payment transactions.

## Description

The Payment Service provides customers with the capability to transfer funds, pay bills, and manage payment instructions across multiple payment types and channels.

## Service Capabilities

- Fund transfers (domestic/international)
- Bill payments
- Scheduled payments
- Recurring payments
- Beneficiary management
- Payment status tracking
- Payment limits management
- Instant payments

## Service Characteristics

- Real-time processing for instant payments
- Multi-currency support
- Bulk payment capabilities
- Payment templates
- Mobile payment integration
- PSD2 compliant

## Performance

- Instant payments: < 10 seconds
- Success rate: > 99%
- Availability: 99.95%
