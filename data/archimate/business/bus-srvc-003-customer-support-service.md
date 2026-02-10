---
id: bus-srvc-003-customer-support-service
owner: Customer Service Department
status: active
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: bus-proc-005-customer-service-process
  description: Realized by customer service process
- type: association
  target: bus-role-003-support-agent
  description: Delivered by support agents
name: Customer Support Service
tags:
- service
- customer-service
- support
archimate:
  type: business-service
  layer: business
  criticality: high
extensions:
  properties:
    legacy-id: bus-svc-customer-support-service-001
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
