---
id: mot-reqt-012-ict-incident-detection-and
owner: Chief Information Security Officer
status: in-progress
version: '1.0'
last_updated: '2026-02-02'
review_cycle: annual
next_review: '2027-02-02'
relationships:
- type: association
  target: mot-reqt-011-dora-digital-operational-resilience
  description: Required by DORA compliance
- type: influence
  target: mot-reqt-007-data-security-and-privacy
  description: Complements data security controls
name: ICT Incident Detection and Response
tags:
- dora
- incident-response
- cybersecurity
- operational-resilience
archimate:
  type: requirement
  layer: motivation
  criticality: critical
extensions:
  properties:
    legacy-id: mot-requirement-012-ict-incident-response
---
# ICT Incident Detection and Response

Comprehensive capability to detect, classify, respond to, and report ICT incidents in compliance with DORA requirements.

## Incident Detection

- Real-time monitoring of all ICT systems and networks
- Automated anomaly detection and alerting
- Security Information and Event Management (SIEM) system
- Intrusion detection and prevention systems (IDS/IPS)
- Endpoint detection and response (EDR)
- Continuous vulnerability scanning and assessment

## Incident Classification

**Significant Incidents** (Report within 24 hours):
- Impact on multiple customers (>1000)
- Impact on critical services
- Unauthorized access to systems or data
- Large-scale data breach
- Sustained unavailability (>4 hours)

**Major Incidents** (Report within 72 hours):
- Impact on limited customer base (>100)
- Data exposure or loss
- Service degradation
- Security infrastructure compromise

**Other Incidents** (Report within 30 days):
- All other ICT incidents
- Minor security events
- Vulnerability disclosures

## Incident Response Process

1. **Detection**: Automated and manual detection of incidents
2. **Triage**: Severity classification and escalation
3. **Response**: Immediate containment and isolation
4. **Investigation**: Root cause analysis and forensics
5. **Remediation**: System recovery and restoration
6. **Reporting**: Notification to authorities and customers
7. **Post-Incident**: Lessons learned and process improvement

## Reporting Requirements

- Incident notification channels to competent authorities
- Tiered reporting based on severity
- On-time compliance (24-72 hours per classification)
- Detailed incident documentation
- Corrective action tracking
- Regulatory follow-up compliance

## Support Systems

- 24/7 Security Operations Center (SOC)
- Incident response team and playbooks
- Forensic analysis capabilities
- Communication protocols and channels
- Recovery and business continuity procedures

## Target Metrics

- Incident detection time: <1 hour average
- Response time: <30 minutes for critical incidents
- Reporting compliance: 100% on-time
- False positive rate: <10%
- MTTR (Mean Time to Repair): <4 hours for critical systems
- Annual incident simulations: 4+ completed
