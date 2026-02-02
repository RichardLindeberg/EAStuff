---
id: bus-actor-csr-001
name: Customer Service Representative
type: business-actor
layer: business
relationships:
  - type: assignment
    target: bus-role-support-agent-001
    description: Performs support role
  - type: assignment
    target: bus-proc-customer-service-001
    description: Executes customer service process
  - type: access
    target: bus-objt-customer-object-001
    description: Views customer information
properties:
  owner: Customer Service Department
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - employee
  - customer-service
  - business-actor
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
