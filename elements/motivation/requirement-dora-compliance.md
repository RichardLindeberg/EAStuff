---
id: mot-requirement-011-dora-compliance
name: DORA Digital Operational Resilience Compliance
type: requirement
layer: motivation
relationships:
  - type: realizationOf
    target: mot-goal-001-regulatory-compliance
    description: Required by regulatory compliance goal
  - type: realizationOf
    target: mot-principle-002-regulatory-excellence
    description: Realizes regulatory excellence principle
  - type: influence
    target: mot-goal-003-operational-efficiency
    description: Requires operational resilience and business continuity
  - type: influence
    target: mot-requirement-007-data-security
    description: Complements data security requirements
  - type: influence
    target: mot-requirement-012-icт-incident-response
    description: Requires ICT incident response capabilities
properties:
  owner: Chief Information Security Officer
  status: in-progress
  criticality: critical
  last-updated: "2026-02-02"
tags:
  - dora
  - operational-resilience
  - compliance
  - regulatory
  - cybersecurity
---

# DORA Digital Operational Resilience Compliance

All financial and operational systems must comply with EU Digital Operational Resilience Act (DORA) requirements for incident detection, reporting, and operational resilience.

## Scope

- Information and communication technology (ICT) infrastructure
- ICT incident detection and monitoring
- Third-party ICT service providers and supply chain
- Business continuity and disaster recovery
- Digital operational resilience framework
- Board and executive governance

## Key Requirements

### ICT Risk Management
- Establish comprehensive ICT risk management framework
- Board oversight and executive accountability
- Regular risk assessments and scenario analysis
- ICT risk reporting to governance bodies
- Integration with enterprise risk management

### ICT Incident Reporting
- Incident detection and classification system
- Tiered reporting based on severity:
  - Significant incidents to authorities within 24 hours
  - Major incidents to authorities within 72 hours
  - All incidents to national regulators within 30 days
- Root cause analysis and remediation tracking
- Public disclosure for major incidents if required

### Business Continuity and Recovery
- Recovery time objective (RTO): <4 hours for critical systems
- Recovery point objective (RPO): <1 hour for critical data
- Documented business continuity plans
- Annual testing of recovery procedures
- Third-party service provider continuity assurance

### Third-Party Risk Management
- ICT service provider due diligence
- Ongoing monitoring and assessment
- Incident reporting requirements in contracts
- Tiered risk classification
- Exit strategy and data recovery procedures

### Digital Resilience Testing
- Threat-Led Penetration Testing (TLPT) annually
- Advanced cyber threat simulations
- Coordinated with regulators and industry
- Remediation of identified vulnerabilities

## Compliance Standards

- NIST Cybersecurity Framework
- ISO 27001/27002 information security standards
- EU NIS Directive provisions
- Industry-specific security standards
- DORA technical standards and guidelines

## Governance Requirements

- Board-level digital resilience committee
- Chief Information Security Officer (CISO) authority
- Chief Risk Officer (CRO) involvement
- Regular compliance reporting
- External audit and assessment

## Target Metrics

- DORA compliance: 100% by deadline
- ICT incidents detected: <1 hour average detection time
- Incident reporting compliance: 100% on-time filing
- Business continuity RTO/RPO: <4 hours/1 hour
- Third-party risk assessments: Annual completion
- TLPT completion: 100% annually
- Board reporting: Quarterly on digital resilience

## Implementation Phases

1. **Phase 1**: Risk assessment and gap analysis (Q1 2026)
2. **Phase 2**: Incident response and reporting systems (Q2-Q3 2026)
3. **Phase 3**: Business continuity and testing (Q3-Q4 2026)
4. **Phase 4**: Third-party risk management (Q4 2026-Q1 2027)
5. **Phase 5**: Continuous testing and optimization (Ongoing)
