---
id: app-intf-001-web-ui
name: Web User Interface
type: application-interface
layer: application
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
properties:
  owner: Digital Experience Team
  status: production
  criticality: high
  technology: React, TypeScript
  version: "3.2.0"
  lifecycle-phase: operate
  last-updated: "2026-02-03"
tags:
- web-interface
- customer-facing
- responsive-design
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
