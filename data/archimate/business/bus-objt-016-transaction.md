---
id: bus-objt-016-transaction
owner: Operations Division
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: composition
  target: bus-objt-003-customer-account
  description: Part of account history
- type: association
  target: bus-proc-008-payment-processing
  description: Created by payment processing
- type: association
  target: bus-proc-006-fraud-detection-process
  description: Monitored for fraud
name: Transaction
tags:
- business-object
- transaction
- payment
- core-data
archimate:
  type: business-object
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-objt-transaction-001
---
# Transaction

Business object representing a financial transaction.

## Description

A Transaction represents any financial movement in or out of an account including payments, transfers, deposits, withdrawals, and fees.

## Attributes

### Identification
- Transaction ID (unique)
- Reference number
- Original reference
- End-to-end ID
- Merchant reference

### Financial Details
- Amount
- Currency
- Exchange rate (if applicable)
- Fees
- Net amount
- Value date
- Booking date

### Parties
- Debtor account
- Debtor name
- Creditor account
- Creditor name
- Intermediary bank
- Correspondent bank

### Transaction Details
- Transaction type
- Transaction code
- Purpose/description
- Category
- Merchant category code
- Payment method

### Status & Control
- Status (pending, completed, failed, reversed)
- Authorization status
- Fraud score
- Hold indicator
- Reversal indicator
- Settlement status

### Metadata
- Transaction timestamp
- Channel (mobile, online, ATM, branch)
- Device ID
- IP address
- User agent
- Geographic location

## Transaction Types

### Debit Transactions
- Outgoing transfers
- Bill payments
- Card purchases
- ATM withdrawals
- Fees and charges
- Interest debits

### Credit Transactions
- Incoming transfers
- Deposits
- Salary credits
- Interest credits
- Refunds
- Reversals

### Internal Transactions
- Inter-account transfers
- Balance adjustments
- Fee applications
- Interest accruals

## Transaction States

1. **Initiated**: Customer submits transaction
2. **Authorized**: Approved by customer/bank
3. **Processing**: In execution
4. **Completed**: Successfully executed
5. **Failed**: Execution failed
6. **Reversed**: Transaction reversed
7. **Disputed**: Under investigation

## Business Rules

- Transaction limits validation
- Duplicate detection
- Fraud screening mandatory
- Balance check pre-execution
- Audit trail requirement
- Retention period: 7 years

## Regulatory Requirements

- AML transaction monitoring
- Suspicious activity reporting
- Large transaction reporting
- Cross-border reporting
- Tax reporting
- Audit trail maintenance
