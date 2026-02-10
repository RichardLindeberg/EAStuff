---
id: bus-objt-012-payment-card
owner: Card Services Division
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
  - type: association
    target: bus-objt-003-customer-account
    description: Linked to account
  - type: association
    target: bus-proc-008-payment-processing
    description: Used for payments
name: Payment Card
tags:
  - business-object
  - card
  - payment-instrument
archimate:
  type: business-object
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-objt-card-001
---
# Payment Card

Business object representing debit and credit cards.

## Description

A Payment Card is a physical or virtual card instrument enabling customers to make payments and access funds from their accounts.

## Attributes

### Card Identification
- Card number (PAN)
- Card type (debit, credit, prepaid)
- Card product
- Card brand (Visa, Mastercard)
- Embossed name
- Issue number
- Card token (digital wallet)

### Security
- CVV/CVC
- PIN
- Chip capability
- Contactless enabled
- 3D Secure enrollment
- Card verification status

### Status & Limits
- Card status (active, blocked, expired, replaced)
- Issue date
- Expiry date
- Daily ATM limit
- Daily POS limit
- Daily online limit
- International usage enabled

### Account Linking
- Primary account
- Card holder type (primary, additional)
- Linked customer ID
- Statement account

### Usage Controls
- Geographic restrictions
- Merchant category blocks
- Transaction type restrictions
- Velocity limits
- Risk-based controls

## Card Types

### Debit Cards
- Immediate account debit
- ATM access
- POS transactions
- Online purchases
- Contactless payments

### Credit Cards
- Revolving credit facility
- Credit limit
- Interest rate
- Minimum payment
- Rewards program
- Grace period

### Prepaid Cards
- Preloaded balance
- Reloadable
- Gift cards
- Travel cards

## Card Statuses

- **Active**: Normal use
- **Blocked**: Temporarily disabled
- **Lost**: Reported lost
- **Stolen**: Reported stolen
- **Damaged**: Physical damage
- **Expired**: Past expiry date
- **Replaced**: New card issued
- **Cancelled**: Permanently closed

## Digital Wallet Integration

- Apple Pay token
- Google Pay token
- Samsung Pay token
- Bank's digital wallet
- Token lifecycle management

## Security Features

- EMV chip
- Contactless (NFC)
- Dynamic CVV
- Biometric authentication
- Transaction alerts
- Fraud monitoring
