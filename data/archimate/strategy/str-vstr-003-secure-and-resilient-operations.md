---
id: str-vstr-003-secure-and-resilient-operations
owner: Chief Information Security Officer
status: developing
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: str-capa-005-digital-operational-resilience-capabilit
  description: Realized through resilience capability
- type: association
  target: str-capa-002-customer-trust-and-data
  description: Builds on customer trust capability
- type: association
  target: mot-goal-002-regulatory-compliance
  description: Supports regulatory compliance goal
- type: association
  target: mot-goal-005-customer-trust
  description: Supports customer trust goal
name: Secure and Resilient Operations Value Stream
tags:
- value-stream
- security
- resilience
- compliance
- strategic
archimate:
  type: value-stream
  layer: strategy
  criticality: critical
extensions:
  properties:
    legacy-id: str-vstr-secure-resilient-operations-001
---
# Secure and Resilient Operations Value Stream

Complete value stream for protecting banking operations, detecting and responding to threats, and maintaining continuous service availability.

## Value Stream Description

This value stream ensures secure, reliable banking operations through threat detection, rapid response, and recovery capabilities. It protects customer data, prevents fraud, and maintains service continuity.

## Core Activities

### 1. Threat Prevention
- Network perimeter protection (firewalls, WAF)
- Endpoint protection (antivirus, EDR)
- Access control and authentication
- Data encryption and protection
- Vulnerability management
- Security awareness training

**Value Created**: Prevents threats before they impact operations

### 2. Threat Detection
- Security monitoring 24/7 (SOC)
- Real-time anomaly detection
- Log analysis and correlation (SIEM)
- Threat intelligence integration
- Vulnerability scanning

**Value Created**: Early detection of security incidents (<1 hour)

### 3. Incident Response
- Rapid incident classification
- Containment and isolation
- Investigation and forensics
- Remediation and recovery
- Regulatory notification

**Value Created**: Minimal impact from incidents, compliance with DORA

### 4. Business Continuity
- Backup and data protection
- Disaster recovery procedures
- System redundancy and failover
- Testing and drills
- Recovery automation

**Value Created**: Continuous service availability (99.99%)

### 5. Third-Party Risk Management
- Vendor security assessments
- Ongoing compliance monitoring
- Incident escalation procedures
- SLA enforcement
- Regular audits

**Value Created**: Supply chain security and resilience

### 6. Regulatory Compliance
- DORA incident reporting
- GDPR breach notification
- Regulatory audit support
- Compliance documentation
- Management reporting

**Value Created**: Regulatory compliance and trust

## Security Posture

### Current State
- Basic perimeter security
- Manual threat monitoring
- Reactive incident response
- Limited backup/recovery
- Fragmented compliance

### Target State
- Multi-layered defense
- 24/7 automated monitoring
- <30 minute response time
- 99.99% availability
- Proactive threat hunting
- Full DORA compliance

## Customer Value

- **Protection**: Customer data and assets protected
- **Privacy**: Private information kept confidential
- **Reliability**: Services always available
- **Confidence**: Trust in the bank's security
- **Compliance**: Assured regulatory compliance

## Business Value

- **Risk Mitigation**: Reduced security and operational risk
- **Compliance**: Meet DORA and other regulatory requirements
- **Reputation**: Maintained customer confidence
- **Availability**: Continuous profitable operations
- **Efficiency**: Automated threat response
- **Competitiveness**: Security as differentiator

## Key Performance Indicators

- Incident detection time: <1 hour
- Incident response time: <30 minutes (critical)
- Mean time to repair: <4 hours
- Service availability: 99.99%
- Security incident prevention rate: >98%
- Regulatory incident reporting compliance: 100%
- Third-party risk assessments: 100% annual completion
- Staff security training completion: 100%
