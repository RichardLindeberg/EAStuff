---
id: bus-svc-customer-support-service-001
name: Customer Support Service
type: business-service
layer: business
relationships:
  - type: realization
    target: bus-proc-customer-service-001
    description: Realized by customer service process
  - type: assignment
    target: bus-role-support-agent-001
    description: Delivered by support agents
properties:
  owner: Customer Service Department
  status: active
  criticality: high
  last-updated: "2026-02-02"
tags:
  - service
  - customer-service
  - support
---

# Customer Support Service

Multi-channel customer support service for inquiries and issue resolution.

## Description

The Customer Support Service provides customers with assistance across all banking products and services through multiple channels including phone, chat, email, and in-person support.

## Service Functions

- Product information
- Issue resolution
- Transaction inquiries
- Account assistance
- Complaint handling
- Technical support
- Escalation management

## Support Channels

- Phone (24/7)
- Live chat
- Email
- Video banking
- Branch support
- Social media
- Self-service portal

## Service Levels

- Average response time: < 2 minutes (phone)
- First contact resolution: > 75%
- Customer satisfaction: > 4.5/5
- Availability: 24/7
