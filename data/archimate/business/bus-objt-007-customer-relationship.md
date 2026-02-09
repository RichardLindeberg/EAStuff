---
id: bus-objt-007-customer-relationship
owner: Relationship Management
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: bus-objt-004-customer-agreement
  description: Established by agreement
- type: association
  target: bus-objt-011-party
  description: Is with customer party
- type: composition
  target: bus-objt-013-product-subscription
  description: Contains product subscriptions
name: Customer Relationship
tags:
- business-object
- customer-relationship
- customer-lifecycle
archimate:
  type: business-object
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-objt-customer-relationship-001
---
# Customer Relationship

Business object representing the active relationship between the bank and a customer, established through a Customer Agreement.

## Description

Customer Relationship is the operational view of the customer-bank engagement. It tracks the relationship status, products held, customer information, and lifecycle. While Customer Agreement is the legal contract, Customer Relationship is the operational reality of that relationship.

## Attributes

### Relationship Identification
- Relationship ID (unique)
- Customer party reference
- Primary agreement reference
- Relationship type (retail, corporate, SME, etc.)
- Relationship inception date
- Relationship manager (if applicable)

### Relationship Status
- Current status (prospect, active, dormant, at-risk, closed)
- Status date
- Previous status
- Status history
- Status change reason

### Customer Information (Operational)
- Customer name (as per agreement)
- Customer category (individual, business, etc.)
- Preferred contact method
- Contact frequency preference
- Communication language
- Designated contact person (corporate)

### Product Portfolio
- List of active products
- List of inactive products
- Primary product (main relationship driver)
- Product count
- Product revenue
- Cross-sell opportunities identified
- Next review date

### Financial Summary
- Total deposits (if applicable)
- Total credit exposure
- Credit utilization
- Net relationship profitability
- Customer lifetime value estimate
- Concentration risk

### Account Summary
- Primary account number
- Secondary accounts
- Account statuses
- Last transaction date (per account)
- Total transaction volume

### Relationship Health
- Payment status (on-time, late, missed)
- Days past due (if applicable)
- Collections status (if applicable)
- Complaint history
- Dispute history
- Compliance violations (if any)
- Risk indicators

### Customer Preferences
- Statement delivery method
- Account access channels preferred
- Service level expectations
- Notification preferences
- Service package (if applicable)

### Customer Value
- Annual revenue contribution
- Customer profitability score
- Customer lifetime value
- Segment assignment (platinum, gold, silver, standard)
- Relationship potential
- Attrition risk score

### Lifecycle Tracking
- Acquisition source
- Acquisition channel
- Acquisition date
- Onboarding completion date
- Time as customer
- Relationship maturity stage
- Projected closure date (if at-risk)

## Relationship Lifecycle Stages

### 1. Prospect
- Pre-customer inquiries
- Application under review
- Documentation pending
- Awaiting final approval

### 2. New Customer (0-3 months)
- Recently opened relationship
- Onboarding in progress
- Account setup
- Initial product adoption
- Welcome process

### 3. Growing (3-12 months)
- Additional products being added
- Relationship deepening
- Cross-sell opportunities
- Increasing transaction volume
- Operational integration

### 4. Established (1-3 years)
- Stable regular usage
- Multiple products/services
- Predictable behavior
- Lower churn risk
- Mature relationship

### 5. Mature (3+ years)
- Long-term relationship
- Diverse product portfolio
- High switching cost
- Low acquisition cost
- Profitable relationship

### 6. At-Risk
- Reduced transaction activity
- Product reduction
- Payment issues
- Competitor activity signals
- Churn indicators
- Requires intervention/retention

### 7. Dormant
- No transactions for extended period
- Accounts inactive
- Relationship in limbo
- May reactivate or close
- Monitoring for reactivation opportunity

### 8. Closed
- Customer departed
- Account closed
- Relationship terminated
- Reason for closure documented
- Archive for historical reference

## Business Rules

- Customer Relationship must be based on active Customer Agreement
- At least one active product required for active relationship
- Relationship status reflects transaction/product activity
- Dormancy determined by no transactions for 12+ months
- Relationship reviews required annually minimum
- Relationship health monitoring continuous
- At-risk relationships require intervention plan
- Closed relationships retained for historical purposes (7+ years)

## Operational Tracking

- **Primary Account**: Customer's main account
- **Relationship Manager**: Assigned if customer qualifies (corporate, VIP)
- **Service Level**: Service tier based on value/segment
- **Support Queue**: For service requests
- **Issue Tracking**: For complaints/issues
- **Marketing Eligibility**: Based on preferences/compliance
- **Product Recommendations**: Identified from profile

## Analytics & Reporting

- **RFM (Recency, Frequency, Monetary)**: Customer engagement analysis
- **Churn Probability**: Risk of departure
- **Lifetime Value**: Long-term profitability
- **Wallet Share**: Portion of customer's financial services
- **Cross-Sell Opportunity**: Products customer might need
- **Segment Analysis**: Compare to similar customers
- **Profitability**: Net profit contribution

## Compliance & Risk

- **Compliance Status**: Current regulatory standing
- **Sanctions Status**: Current screening result
- **PEP Status**: Current political exposure
- **AML Risk**: Current AML risk rating
- **Credit Risk**: Current credit standing
- **Reputational Risk**: Any concerns
- **Operational Risk**: System/process issues
