---
id: bus-role-support-agent-001
name: Support Agent
type: business-role
layer: business
relationships:
  - type: assignment
    target: bus-proc-customer-service-001
    description: Executes customer service
  - type: serving
    target: bus-svc-customer-support-service-001
    description: Provides support services
  - type: access
    target: bus-objt-customer-object-001
    description: Accesses customer data
properties:
  owner: Customer Service Department
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - role
  - internal-role
  - customer-service
---

# Support Agent

Internal role for employees providing customer support services.

## Description

Support Agents assist customers with inquiries, troubleshoot issues, and ensure positive customer experiences across all service channels.

## Permissions

- View customer information
- Access transaction history
- Process service requests
- Create and update tickets
- Execute approved account changes
- Access knowledge base
- Escalate to specialists

## Responsibilities

- Respond to customer inquiries promptly
- Resolve issues within authority level
- Document all interactions
- Follow service procedures
- Maintain customer confidentiality
- Meet service level agreements
- Provide accurate information

## Skills Required

- Product knowledge
- Communication skills
- Problem-solving ability
- System proficiency
- Empathy and patience
- Attention to detail
