---
id: bus-proc-006-fraud-detection-process
owner: Fraud Prevention Department
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: serving
  target: bus-proc-008-payment-processing
  description: Protects payment processing
- type: serving
  target: bus-proc-001-account-management-process
  description: Protects account operations
- type: access
  target: bus-objt-016-transaction
  description: Analyzes transactions
name: Fraud Detection Process
tags:
- process
- security
- fraud-prevention
- risk-management
archimate:
  type: business-process
  layer: business
  criticality: critical
extensions:
  properties:
    legacy-id: bus-proc-fraud-detection-001
---
# Fraud Detection Process

Real-time process for detecting and preventing fraudulent transactions and activities.

## Description

The Fraud Detection Process continuously monitors transactions, account activities, and customer behaviors to identify and prevent fraudulent activities. It combines rule-based detection, machine learning models, and behavioral analytics to protect customers and the bank.

## Detection Methods

### 1. Real-Time Transaction Monitoring
- Amount threshold checks
- Velocity checks (frequency)
- Geographic anomaly detection
- Device fingerprinting
- IP address analysis
- Time-of-day patterns

### 2. Behavioral Analytics
- Customer spending patterns
- Transaction history analysis
- Channel usage patterns
- Beneficiary analysis
- Session behavior monitoring

### 3. Rule-Based Detection
- High-risk country transactions
- Blacklist matching
- Sanctions screening
- Duplicate transaction detection
- Merchant category restrictions

### 4. Machine Learning Models
- Anomaly detection
- Pattern recognition
- Predictive scoring
- Network analysis
- Entity resolution

## Process Flow

### 1. Transaction Capture
- Real-time transaction ingestion
- Data enrichment
- Context gathering

### 2. Risk Scoring
- Multi-factor risk assessment
- ML model scoring
- Rule evaluation
- Risk score calculation

### 3. Decision Making
- Low risk: Auto-approve
- Medium risk: Additional verification
- High risk: Block/challenge
- Critical risk: Immediate block + alert

### 4. Customer Verification
- SMS/email OTP
- Push notification approval
- Biometric verification
- Call-back verification

### 5. Case Management
- Alert generation
- Case assignment
- Investigation workflow
- Decision documentation
- Customer communication

### 6. Resolution
- Transaction approval/decline
- Account measures (freeze, limit reduction)
- Law enforcement notification
- Customer notification
- Feedback loop to models

## Fraud Types Detected

- Card fraud (CNP, lost/stolen)
- Account takeover
- Identity theft
- Payment fraud
- Application fraud
- Social engineering
- Money laundering

## Performance Targets

- Detection rate: > 95%
- False positive rate: < 5%
- Response time: < 100ms (inline)
- Alert resolution time: < 4 hours
- Customer impact: Minimal friction

## Compliance

- AML regulations
- KYC requirements
- PSD2 transaction monitoring
- GDPR data processing
- Fraud reporting obligations
