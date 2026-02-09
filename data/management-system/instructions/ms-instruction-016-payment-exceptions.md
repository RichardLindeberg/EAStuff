---
id: ms-instruction-016-payment-exceptions
owner: bus-role-011-payment-operations
status: Draft
version: '0.1'
last_updated: '2026-02-07'
review_cycle: Annual
next_review: '2027-02-07'
relationships:
- type: association
  target: bus-proc-008-payment-processing
- type: composition
  target: ms-policy-013-sanctions-screening
- type: association
  target: ms-manual-013-transaction-monitoring-sanctions
name: Payment Exceptions Handling Instruction
governance:
  approved_by: Head of Payments
  effective_date: '2026-02-07'
---
# Payment Exceptions Handling Instruction


**Purpose:**
Provide steps for handling payment exceptions, rejects, and recalls.

## Scope
- Domestic, international, and instant payments.

## Prerequisites
- Payment exception case record
- Transaction details and customer contact info

## Procedure
1. Validate exception reason and transaction status.
2. Notify customer and confirm corrective action.
3. Apply required fixes or re-initiate the payment.
4. Record resolution and any fees or adjustments.
5. Escalate systemic issues to operations management.

## Outputs and Records
- Exception resolution record
- Customer notification log


