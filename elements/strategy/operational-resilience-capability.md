---
id: str-capa-operational-resilience-001
name: Digital Operational Resilience Capability
type: capability
layer: strategy
relationships:
  - type: realization
    target: mot-goal-003-improve-operational-efficiency
    description: Realizes operational efficiency goal
  - type: realization
    target: mot-requirement-011-dora-compliance
    description: Realizes DORA compliance requirement
  - type: realization
    target: mot-requirement-012-ict-incident-response
    description: Realizes ICT incident response requirement
properties:
  owner: Chief Information Security Officer
  status: developing
  criticality: critical
  maturity-level: "2 - Repeatable"
  last-updated: "2026-02-02"
tags:
  - capability
  - resilience
  - cybersecurity
  - operational
  - strategic
---

# Digital Operational Resilience Capability

Strategic capability to maintain uninterrupted business operations, detect and respond to digital incidents, and ensure rapid recovery from disruptions.

## Description

This capability ensures the bank can withstand, recover from, and adapt to digital threats and operational disruptions. It includes incident detection, response, business continuity, and third-party resilience management.

## Capability Components

### People
- Security Operations Center (SOC) staff
- Incident response team
- Business continuity planners
- Infrastructure engineers
- Threat analysts
- Forensic specialists

### Process
- Incident detection and classification
- Escalation and response procedures
- Root cause analysis
- Recovery and restoration
- Business continuity planning
- Disaster recovery drills
- Third-party risk assessment

### Technology
- Security Information and Event Management (SIEM)
- Intrusion Detection/Prevention (IDS/IPS)
- Endpoint Detection and Response (EDR)
- Backup and recovery systems
- Redundant infrastructure
- Monitoring and alerting platforms
- Disaster recovery sites

### Information
- Incident logs and forensics
- Threat intelligence
- System performance metrics
- Recovery procedures
- Business impact assessments
- Third-party risk profiles

## Current State

- Manual incident detection
- Fragmented monitoring tools
- Basic backup procedures
- Maturity: 2 - Repeatable

## Target State

- Automated detection (<1 hour)
- Integrated SOC platform
- <4 hour RTO, <1 hour RPO
- 99.99% availability
- Full DORA compliance
- Maturity: 4 - Managed

## Strategic Value

- Operational continuity and uptime
- Regulatory compliance (DORA)
- Customer trust and confidence
- Competitive protection
- Risk mitigation
