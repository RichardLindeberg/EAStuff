---
id: bus-srvc-006-customer-support-service
name: Customer Support Service
type: business-service
layer: business
relationships:
- type: serving
  target: bus-role-001-account-holder
  description: Provides support to account holders
- type: serving
  target: bus-actr-003-retail-customer
  description: Provides support to retail customers
- type: serving
  target: bus-actr-001-corporate-customer
  description: Provides support to corporate customers
- type: realization
  target: mot-reqt-008-transparent-customer-communication
  description: Enables transparent communication
properties:
  owner: Head of Customer Service
  status: active
  criticality: high
  service-level: "24/7 support (critical issues), business hours (general)"
  channels: "phone, email, chat, video, portal"
  last-updated: "2026-02-03"
tags:
- customer-service
- support
- business-service
---

# Customer Support Service

Comprehensive customer support service providing assistance, guidance, and issue resolution across multiple channels.

## Description

Customer Support Service delivers responsive, professional assistance to banking customers throughout the Nordic region. The service handles inquiries, resolves issues, provides guidance, and ensures customer satisfaction through multiple touchpoints including phone, email, secure messaging, chat, and video banking.

## Service Offerings

### Issue Resolution
- Account access and authentication problems
- Transaction disputes and corrections
- Technical assistance with digital banking
- Product and service troubleshooting

### Information & Guidance
- Product information and comparisons
- Fee and rate inquiries
- Regulatory and compliance questions
- Service availability and changes

### Proactive Support
- Fraud alerts and verification
- Service disruption notifications
- Product recommendations
- Usage tips and best practices

## Service Channels

### Phone Support
- Dedicated Swedish, Norwegian, Danish, Finnish numbers
- Priority lines for corporate customers
- Callback service during high volume
- Multi-language support (Nordic languages + English)

### Digital Channels
- Secure messaging in customer portal
- Live chat (AI-assisted with human escalation)
- Video banking for complex matters
- Social media monitoring and response

### Self-Service
- Comprehensive FAQ and knowledge base
- Interactive troubleshooting guides
- Video tutorials
- Community forums (moderated)

## Service Levels

- **Critical Issues**: 24/7 support, <15 min response
- **Urgent Matters**: Same-day resolution target
- **General Inquiries**: Response within 24 business hours
- **Corporate Customers**: Dedicated support teams with priority handling

## Nordic Banking Standards

Adheres to:
- National banking association service standards
- Consumer protection requirements
- Accessibility standards for customers with disabilities
- Language requirements (official languages in each market)
