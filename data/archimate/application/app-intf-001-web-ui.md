---
id: app-intf-001-web-ui
owner: Digital Experience Team
status: production
version: 3.2.0
last_updated: '2026-02-03'
review_cycle: annual
next_review: '2027-02-03'
relationships:
- type: serving
  target: bus-role-001-account-holder
  description: Provides interface for account holders
- type: serving
  target: bus-actr-003-retail-customer
  description: Provides interface for retail customers
- type: realization
  target: mot-reqt-003-mobile-first-banking-design
  description: Implements responsive design principles
name: Web User Interface
tags:
- web-interface
- customer-facing
- responsive-design
archimate:
  type: application-interface
  layer: application
  criticality: high
extensions:
  properties:
    technology: React, TypeScript
    lifecycle-phase: operate
---
# Web User Interface

Modern responsive web interface for the customer portal providing seamless banking services across devices.

## Description

The Web UI component delivers a responsive, accessible user interface built with modern web technologies. It provides Nordic banking customers with intuitive access to their accounts, transactions, payments, and banking services through any web browser.

## Key Features

- **Responsive Design**: Adapts seamlessly to desktop, tablet, and mobile viewports
- **Accessibility**: WCAG 2.1 AA compliant for inclusive banking
- **Progressive Web App**: Installable with offline capabilities
- **Multi-language**: Supports Swedish, Norwegian, Danish, Finnish, and English
- **Strong Authentication**: BankID and other Nordic e-ID integrations

## Technology Stack

- Frontend Framework: React 18
- Language: TypeScript
- State Management: Redux Toolkit
- UI Library: Material-UI (Nordic theme)
- Authentication: OAuth 2.0 / OpenID Connect
- API Communication: REST / GraphQL

## Compliance

- PSD2 Strong Customer Authentication
- GDPR data protection requirements
- National accessibility standards (Nordic countries)
