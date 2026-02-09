---
id: bus-actr-002-customer-service-representative
owner: Customer Service Department
status: active
version: ''
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: assignment
  target: bus-role-003-support-agent
  description: Performs support role
- type: assignment
  target: bus-proc-005-customer-service-process
  description: Executes customer service process
- type: access
  target: bus-objt-005-customer-object
  description: Views customer information
name: Customer Service Representative
tags:
- employee
- customer-service
- business-actor
archimate:
  type: business-actor
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-actor-csr-001
---
# Customer Service Representative

Front-line employees who assist customers with inquiries, issues, and service requests.

## Description

Customer service representatives are the primary point of contact for customer support. They handle inquiries through phone, chat, email, and in-person interactions, providing assistance and resolving issues.

## Responsibilities

- Handle customer inquiries
- Resolve account issues
- Process service requests
- Escalate complex cases
- Document interactions
- Provide product information

## Tools and Systems

- Customer relationship management (CRM) system
- Core banking system
- Knowledge base
- Ticketing system
- Communication tools

## Performance Metrics

- Average handling time
- First contact resolution rate
- Customer satisfaction score
- Quality assurance rating
