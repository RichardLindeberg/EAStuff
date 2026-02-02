---
id: bus-objt-card-001
name: Payment Card
type: business-object
layer: business
relationships:
  - type: association
    target: bus-objt-customer-account-001
    description: Linked to account
  - type: access
    target: bus-proc-payment-processing-001
    description: Used for payments
properties:
  owner: Card Services Division
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - business-object
  - card
  - payment-instrument
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
