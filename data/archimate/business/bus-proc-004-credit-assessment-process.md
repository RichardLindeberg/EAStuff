---
id: bus-proc-004-credit-assessment-process
name: Credit Assessment Process
type: business-process
layer: business
relationships:
- type: serving
  target: bus-proc-007-loan-origination-process
  description: Supports loan decisions
- type: access
  target: bus-objt-005-customer-object
  description: Analyzes customer data
properties:
  owner: Credit Risk Department
  status: active
  criticality: high
  last-updated: '2026-02-02'
  legacy-id: bus-proc-credit-assessment-001
tags:
- process
- credit-risk
- underwriting
- risk-assessment
---

# Credit Assessment Process

Process for evaluating customer creditworthiness and determining lending terms.

## Description

The Credit Assessment Process evaluates applicants' ability and willingness to repay loans by analyzing financial information, credit history, and risk factors. This process determines approval decisions and appropriate loan terms.

## Assessment Components

### 1. Credit Bureau Data
- Credit score retrieval
- Payment history
- Credit utilization
- Account age and mix
- Recent inquiries
- Public records

### 2. Financial Analysis
- Income verification
- Employment stability
- Debt-to-income ratio
- Asset evaluation
- Expense analysis
- Net worth calculation

### 3. Risk Evaluation
- Default probability scoring
- Loss given default estimation
- Risk rating assignment
- Collateral assessment
- Guarantor evaluation

### 4. Affordability Assessment
- Monthly income vs. obligations
- Discretionary income calculation
- Stress testing (rate increases)
- Future income projections
- Expense validation

## Decision Framework

### Auto-Approval Criteria
- Credit score > 750
- DTI < 30%
- Stable employment (2+ years)
- No recent defaults
- Within policy limits

### Manual Underwriting
- Borderline scores (650-750)
- High DTI (30-40%)
- Recent credit events
- Self-employed applicants
- Large loan amounts

### Decline Criteria
- Credit score < 600
- DTI > 45%
- Recent bankruptcies
- Insufficient income
- Fraudulent information

## Risk-Based Pricing

- Prime: Best rates (low risk)
- Standard: Market rates (moderate risk)
- Sub-prime: Higher rates (higher risk)
- Secured vs. unsecured adjustments

## Performance Metrics

- Assessment accuracy: > 90%
- Processing time: < 30 minutes (automated)
- Manual review time: < 4 hours
- Appeal success rate: 15-20%
- Portfolio default rate: < 2%
