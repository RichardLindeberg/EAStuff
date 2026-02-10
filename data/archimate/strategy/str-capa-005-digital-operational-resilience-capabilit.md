---
id: str-capa-005-digital-operational-resilience-capabilit
owner: Chief Information Security Officer
status: developing
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: realization
  target: mot-goal-004-operational-efficiency
  description: Realizes operational efficiency goal
- type: realization
  target: mot-reqt-011-dora-digital-operational-resilience
  description: Realizes DORA compliance requirement
- type: realization
  target: mot-reqt-012-ict-incident-detection-and
  description: Realizes ICT incident response requirement
name: Digital Operational Resilience Capability
tags:
- capability
- resilience
- cybersecurity
- operational
- strategic
archimate:
  type: capability
  layer: strategy
  criticality: critical
extensions:
  properties:
    maturity-level: 2 - Repeatable
    legacy-id: str-capa-operational-resilience-001
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
