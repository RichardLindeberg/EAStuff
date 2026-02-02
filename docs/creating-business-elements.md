# Quick Start: Creating Business Layer Elements

## Using the Element Creation Script

The easiest way to create business elements is using the provided script:

```bash
source .venv/bin/activate
python3 scripts/create_element.py
```

The script will prompt you for:
1. Element type (choose from business layer types)
2. Element name
3. Element ID (or auto-generate)
4. Layer (will be 'business')
5. Relationships
6. Properties
7. Tags

## Business Layer Element Types

### Actors & Roles
- `business-actor` - External entity (customers, partners)
- `business-role` - Internal role (employee, department)
- `business-collaboration` - Group of actors working together

### Behavior Elements
- `business-process` - Sequence of activities achieving a business goal
- `business-function` - Business capability or responsibility
- `business-interaction` - Behavior between actors/roles
- `business-event` - Something that happens triggering behavior

### Service & Interface
- `business-service` - Externally visible business functionality
- `business-interface` - Point of access to business service

### Data & Information
- `business-object` - Business information or data
- `contract` - Formal agreement
- `representation` - Perceivable form of information

### Products
- `product` - Bundle of services and/or products offered

## Example 1: Creating a Business Actor

```yaml
---
id: bus-actor-retail-customer-001
name: Retail Customer
type: business-actor
layer: business
relationships:
  - type: assignment
    target: bus-proc-customer-onboarding-001
    description: Performs customer onboarding activities
  - type: serving
    target: bus-svc-digital-account-mgmt-001
    description: Uses digital account management service
properties:
  segment: retail-banking
  characteristics: digital-first, mobile-native
  priority: high
tags:
  - customer
  - retail
  - external-actor
---

# Retail Customer

Individual banking customer using digital banking services.

## Description

Retail customers are individuals who use the bank's consumer banking products including accounts, payments, loans, and investments. They primarily interact through digital channels (mobile, web) with occasional branch visits.

## Characteristics

- **Demographics**: Ages 18-75, digitally active
- **Channel Preference**: 80% mobile, 15% web, 5% branch
- **Expectations**: 24/7 access, instant responses, seamless experience
- **Digital Literacy**: Varies from basic to advanced

## Interaction Patterns

- Account opening and management
- Payment initiation and authorization
- Service requests and support
- Product browsing and purchase

## Related Goals

This actor is central to:
- Digital Transformation Goal
- Enhance Digital Banking Goal
- Build Customer Trust Goal
```

## Example 2: Creating a Business Service

```yaml
---
id: bus-svc-digital-account-mgmt-001
name: Digital Account Management Service
type: business-service
layer: business
relationships:
  - type: realization
    target: mot-goal-002-enhance-digital-banking
    description: Realizes digital banking enhancement goal
  - type: realization
    target: str-capa-digital-banking-001
    description: Supported by digital banking platform capability
  - type: serving
    target: bus-actor-retail-customer-001
    description: Serves retail customers
  - type: realization
    target: app-comp-customer-portal-001
    description: Realized by customer portal application
properties:
  owner: Digital Banking Department
  availability: 99.9%
  sla: 24x7 availability
  criticality: critical
tags:
  - digital
  - customer-facing
  - core-service
---

# Digital Account Management Service

Business service enabling customers to view and manage their banking accounts through digital channels.

## Description

This service provides customers with comprehensive self-service capabilities to manage their accounts including viewing balances, transactions, statements, and updating account settings.

## Service Capabilities

- View account balances and details
- Review transaction history
- Download statements
- Update account settings
- Set up alerts and notifications
- Link external accounts
- Manage beneficiaries

## Service Levels

- **Availability**: 99.9% uptime (SLA)
- **Response Time**: < 2 seconds for queries
- **Support Hours**: 24/7 digital, 8am-6pm phone
- **Channels**: Mobile app, web portal, phone banking

## Customer Benefits

- 24/7 account access
- Real-time balance updates
- Paperless statements
- Instant notifications
- Multi-account view

## Business Value

- Reduced branch traffic by 60%
- Lower operational costs
- Improved customer satisfaction
- Increased digital adoption
- Foundation for additional digital services
```

## Example 3: Creating a Business Process

```yaml
---
id: bus-proc-customer-onboarding-001
name: Digital Customer Onboarding Process
type: business-process
layer: business
relationships:
  - type: realization
    target: bus-svc-customer-onboarding-001
    description: Realizes customer onboarding service
  - type: realization
    target: mot-requirement-003-mobile-first
    description: Supports mobile-first requirement
  - type: assignment
    target: bus-role-customer-service-rep-001
    description: Assigned to customer service reps for review
  - type: access
    target: bus-obj-customer-profile-001
    description: Creates and accesses customer profile
  - type: triggering
    target: bus-event-customer-registration-001
    description: Triggered by customer registration event
properties:
  owner: Customer Experience Department
  cycle-time: 15 minutes (target)
  automation-level: 85%
  status: active
  kpi-target: ">60% completion rate"
tags:
  - onboarding
  - digital
  - automated
  - critical-path
---

# Digital Customer Onboarding Process

End-to-end process for registering new customers and opening their first account digitally.

## Process Overview

This process enables customers to become fully onboarded bank customers within 15 minutes using only their mobile device, meeting regulatory KYC requirements while providing excellent user experience.

## Process Steps

### 1. Customer Registration (2 min)
- Enter personal details (name, address, DOB)
- Provide contact information (email, phone)
- Accept terms and conditions

### 2. Identity Verification (5 min)
- Capture government ID document
- Perform facial recognition match
- Verify against identity databases
- Check sanctions and PEP lists

### 3. Account Setup (3 min)
- Choose account type and features
- Set up security (PIN, biometrics)
- Configure initial preferences
- Review and confirm details

### 4. Initial Funding (3 min)
- Link external bank account, or
- Schedule branch visit, or
- Register payment card

### 5. Activation (2 min)
- Activate mobile banking
- Complete welcome tutorial
- Receive account details
- Access is granted

## KPIs

- **Completion Rate**: Currently 62% (target >60%)
- **Average Duration**: 14 minutes (target <15 min)
- **Drop-off Rate**: 38% (mainly at identity verification)
- **Customer Satisfaction**: 4.3/5

## Automation

- 85% automated (identity verification, account creation, activation)
- 15% manual review (high-risk cases, verification failures)
- AI-assisted identity document verification
- Automated KYC checks

## Compliance

- GDPR compliant consent management
- KYC/AML requirements met
- Identity verification per banking regulations
- Audit trail for all steps

## Integration Points

- Identity verification service (3rd party)
- Core banking system
- CRM system
- Compliance monitoring system
```

## Example 4: Creating a Business Object

```yaml
---
id: bus-obj-account-001
name: Account
type: business-object
layer: business
relationships:
  - type: access
    target: bus-svc-digital-account-mgmt-001
    description: Accessed by account management service
  - type: access
    target: bus-proc-payment-initiation-001
    description: Accessed by payment processes
  - type: realization
    target: app-data-account-001
    description: Realized by account data object in application layer
properties:
  data-classification: confidential
  retention-period: "7 years after closure"
  owner: Core Banking Department
tags:
  - core-data
  - customer-data
  - regulated
---

# Account

Business object representing a customer's banking account with associated balance, transactions, and settings.

## Description

The Account is a fundamental business object representing the relationship between a customer and the bank, holding funds and transaction history.

## Attributes

### Core Attributes
- Account Number (unique identifier)
- Account Type (checking, savings, investment)
- Status (active, frozen, closed)
- Opening Date
- Currency

### Financial Attributes
- Current Balance
- Available Balance
- Overdraft Limit
- Interest Rate
- Minimum Balance

### Customer Attributes
- Primary Account Holder
- Joint Account Holders
- Authorized Signatories
- Account Owner Type (individual, business)

### Settings
- Alert Preferences
- Statement Frequency
- Overdraft Settings
- Transaction Limits

## Lifecycle

1. **Opened** - Account created during onboarding
2. **Active** - Normal operational state
3. **Frozen** - Temporarily suspended (fraud, court order)
4. **Dormant** - No activity for extended period
5. **Closed** - Account terminated by customer or bank

## Business Rules

- Minimum age for account holder: 18 years
- Minimum opening balance: varies by account type
- Maximum daily transfer limit: €50,000
- Monthly maintenance fee: waived if balance > €5,000
- Negative balance allowed up to overdraft limit

## Related Objects

- Customer Profile
- Payment Transaction
- Statement
- Card
- Loan
```

## Relationship Types for Business Layer

Common relationship types from business to other layers:

### To Motivation Layer
- `realization` - Business element realizes a goal/requirement
- `influence` - Business element is influenced by a driver/principle

### To Strategy Layer
- `realization` - Business element realizes a capability
- `association` - Business element relates to a resource

### Within Business Layer
- `assignment` - Actor/role is assigned to behavior
- `serving` - Service serves an actor
- `access` - Process accesses an object
- `triggering` - Event triggers a process
- `composition` - Element is part of another
- `aggregation` - Element groups other elements
- `realization` - Behavior realizes a service
- `flow` - Process flow between elements

### To Application Layer
- `realization` - Application component/service realizes business service
- `serving` - Application service serves business process

## Tips for Good Business Elements

1. **Use business language** - Avoid technical jargon
2. **Focus on what, not how** - Describe business logic, not implementation
3. **Link to strategy** - Show how element supports goals/capabilities
4. **Define ownership** - Assign business owners, not IT
5. **Include metrics** - KPIs, SLAs, target values
6. **Document decisions** - Explain why element exists
7. **Trace to applications** - Show which apps realize the business element

## Validation Checklist

Before finalizing a business element, ensure:

- [ ] Clear business owner assigned
- [ ] Links to at least one motivation/strategy element
- [ ] Has description in business language
- [ ] Includes relevant properties (owner, status, criticality)
- [ ] Tagged appropriately for findability
- [ ] Relationships to dependent elements defined
- [ ] Metrics or success criteria included (where applicable)

## Next Steps

After creating business elements:

1. **Regenerate website**: `./generate-website.sh`
2. **Verify relationships**: Check incoming/outgoing relations in website
3. **Review traceability**: Ensure goals → capabilities → services trace correctly
4. **Validate with stakeholders**: Review with business owners
5. **Create viewpoints**: Generate diagrams showing specific aspects
