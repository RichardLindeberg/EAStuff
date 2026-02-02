---
id: bus-objt-product-subscription-001
name: Product Subscription
type: business-object
layer: business
relationships:
  - type: association
    target: bus-objt-customer-relationship-001
    description: Part of customer relationship
  - type: association
    target: bus-objt-customer-agreement-001
    description: Authorized by agreement
properties:
  owner: Product Management
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - business-object
  - product-subscription
  - product-holding
---

# Product Subscription

Business object representing a customer's active subscription/holding of a specific banking product.

## Description

Product Subscription represents the contractual right of a customer to use a specific product and service under the terms of a Customer Agreement. It tracks which products the customer has, the terms applied, and the status of each product.

## Attributes

### Subscription Identification
- Subscription ID (unique)
- Customer party reference
- Product reference
- Effective date (when customer started using product)
- Initiation date (when subscription was created)

### Product Details
- Product code
- Product name
- Product type (account, card, loan, investment, service)
- Product version
- Product offering date

### Subscription Terms
- Agreed interest rate(s)
- Fee structure
- Credit limit (if applicable)
- Debit limit (if applicable)
- Transaction limit
- Balance limit
- Geographic scope
- Channel access (mobile, online, branch, ATM, phone, API)

### Account/Facility Details
- Account number (if account product)
- Loan number (if lending product)
- Card number (if card product)
- Facility reference
- Primary vs. secondary indicator

### Status Information
- Subscription status (active, pending, suspended, closed, dormant)
- Status date
- Status change reason (if changed)
- Activation date
- Termination date (if applicable)
- Last activity date

### Financial Terms
- Opening balance
- Current balance
- Credit utilized
- Credit available
- Interest applied
- Fees applied

### Compliance Information
- Product terms accepted? (Yes/No)
- Risk disclosures provided? (Yes/No)
- Risk disclosures signed? (Yes/No)
- Product suitability assessment completed? (for investment products)
- Product suitability result
- AML checks completed? (Yes/No)
- Regulatory conditions met? (Yes/No)

### Usage Information
- Last transaction date
- Transaction frequency
- Monthly transaction volume
- Average transaction amount
- Usage pattern
- Unusual activity flag

## Product Types

### Deposit Products
- Checking/Current account
- Savings account
- Money market account
- Term deposit
- Youth account
- Senior account

### Credit Products
- Personal loan
- Mortgage loan
- Auto loan
- Line of credit
- Credit card
- Overdraft facility

### Payment Services
- Debit card
- Payment transfer authorization
- Direct debit authorization
- Standing order
- Mobile payment
- Contactless payment

### Investment Products
- Brokerage account
- Mutual fund holding
- Bond holding
- Stock holding
- Managed account
- Robo-advisor account

### Specialty Services
- Safe deposit box
- Notarization
- Financial planning
- Trust services
- Wealth management
- Corporate services

## Subscription Lifecycle

### 1. Initiation
- Customer applies for product
- Product eligibility verified
- Approval obtained
- Terms agreed
- Subscription created
- Confirmation sent

### 2. Activation
- Account/facility opened
- Cards/checks/credentials issued
- Initial funding/setup
- System provisioning complete
- Customer notified
- Product ready for use

### 3. Active Usage
- Customer uses product
- Transactions processed
- Interest/fees applied
- Activity monitored
- Statements generated
- Support available

### 4. Modification
- Customer requests change
- Terms/limits modified
- Documentation updated
- Systems updated
- Confirmation sent

### 5. Suspension
- Service temporarily stopped
- Customer notified
- Reason documented
- Restoration conditions specified
- Monitoring for restoration

### 6. Closure/Termination
- Customer requests closure
- Final transactions settled
- Final statement generated
- Account closed
- Retention per policy
- Archive completed

## Business Rules

- Each subscription must be linked to a Customer Agreement
- Product terms must comply with regulatory requirements
- Product eligibility must be verified at subscription
- Interest and fees must be calculated per terms
- Usage must stay within agreed limits
- Compliance status must be maintained
- Inactive subscriptions (no activity 12+ months) trigger dormancy review
- Dormant subscriptions may be closed if customer doesn't reactivate

## Monitoring & Alerts

- **Usage Alert**: Approaching limits
- **Inactivity Alert**: No recent transactions
- **Compliance Alert**: Status changes
- **Fraud Alert**: Unusual activity
- **Service Alert**: Service disruption
- **Renewal Alert**: Approaching renewal date
- **Fee Alert**: Upcoming fees
- **Interest Rate Alert**: Rate changes

## Reporting

- Product statement (monthly/quarterly/annual)
- Interest/fee summary
- Usage analysis
- Performance vs. terms
- Compliance status report
- Profitability analysis
