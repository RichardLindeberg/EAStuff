---
id: mot-requirement-015-sar-reporting
name: Suspicious Activity Reporting (SAR)
type: requirement
layer: motivation
relationships:
  - type: realizationOf
    target: mot-goal-001-regulatory-compliance
    description: Legal obligation for AML/CT compliance
properties:
  owner: Chief Compliance Officer / MLRO
  status: active
  criticality: critical
  last-updated: "2026-02-03"
tags:
  - sar
  - suspicious-activity
  - aml
  - compliance
  - reporting
  - financial-crime
  - fiu
---

# Suspicious Activity Reporting (SAR)

All suspicious activities indicating potential money laundering, terrorism financing, or other financial crimes must be promptly identified, investigated, and reported to the relevant Financial Intelligence Unit (FIU).

## Scope

- Completed transactions flagged as suspicious
- Attempted transactions that were blocked or declined
- Customer behaviors that suggest criminal activity
- Activities inconsistent with customer profile
- Unusual patterns across multiple customers
- Internal fraud or employee suspicious activity
- Third-party relationships that raise concerns

## Key Requirements

### Legal Obligations
- **Mandatory Reporting**: Legal requirement to report suspicions
- **No Tipping Off**: Prohibition against informing customer of SAR filing
- **Liability Protection**: Safe harbor for good faith reporting
- **Timely Filing**: Specific deadlines per jurisdiction
  - Sweden (Finanspolisen): As soon as possible, ideally within 14 days
  - Norway (Økokrim): Immediately or as soon as possible
  - Denmark (Hvidvasksekretariatet): Immediately
- **Content Requirements**: Detailed information per regulatory templates
- **MLRO Authority**: Money Laundering Reporting Officer decides on filing

### Triggering Events
- Transaction monitoring alerts indicating suspicious patterns
- Customer due diligence red flags
- Unusual account activity or behavior changes
- Sanctions screening hits with unclear legitimacy
- News or adverse media reports about customer
- Law enforcement inquiries or subpoenas
- Employee observations or concerns
- Third-party notifications (correspondent banks, partners)
- Failed identity verification or fraudulent documents

### Investigation Standards
- Thorough investigation before filing decision
- Collection of all relevant facts and documentation
- Review of customer history and profile
- Analysis of transaction patterns
- Research using internal and external sources
- Consultation with subject matter experts as needed
- Documentation of investigation steps and findings
- Supervisor and MLRO review

### SAR Content Requirements
- **Subject Information**: Full customer/entity details
- **Suspicious Activity Description**: Clear narrative of what occurred
- **Transactions**: Detailed transaction information
- **Time Period**: Dates and duration of suspicious activity
- **Red Flags**: Specific indicators of suspicion
- **Products/Services**: Banking products involved
- **Prior SARs**: Reference to related previous filings
- **Supporting Documentation**: Attachments and evidence
- **Impact**: Financial amounts and potential harm
- **Investigation Summary**: Steps taken to investigate

### Workflow and Governance
- **Escalation Process**: Clear path from investigator to MLRO
- **Review Levels**: Multi-level review for quality
- **Decision Authority**: MLRO has final decision on filing
- **Documentation**: Comprehensive case files for all decisions
- **Filing Mechanism**: Secure submission to FIU (goAML or national systems)
- **Confirmation**: Receipt acknowledgment from FIU
- **Follow-Up**: Response to FIU inquiries and requests

### Record Keeping
- Retention of SAR files for at least 5 years (or longer per local requirements)
- Secure storage with restricted access
- Audit trail of all review and approval steps
- Documentation of rationale for non-filing decisions
- Regular archival and retention policy compliance

### Non-Filing Decisions
- Documentation of decision not to file
- Rationale and supporting analysis
- Supervisor and MLRO approval
- Periodic review of non-filed cases
- Consideration of cumulative patterns

## Compliance Measures

### Organization and Governance
- Dedicated SAR filing team or function
- Money Laundering Reporting Officer (MLRO) with authority
- Clear roles and responsibilities (RACI matrix)
- Board and management oversight
- Regular reporting on SAR metrics
- Independent audit and quality assurance

### Technology Systems
- Case management system for investigations
- Workflow for escalation and approvals
- Document repository for evidence
- Integration with transaction monitoring system
- Secure filing portal to FIU
- Analytics for quality assurance and trends

### Quality Assurance
- Pre-filing quality review process
- Post-filing quality assessments
- FIU feedback analysis
- Regulatory examination findings
- Continuous improvement initiatives
- Industry benchmarking

### Training and Awareness
- Mandatory training for all employees on SAR obligations
- Specialized training for investigators and compliance staff
- Updates on new typologies and red flags
- Case studies and lessons learned
- Testing and knowledge assessments
- Refresher training annually

### Confidentiality and Tipping Off
- Strict confidentiality of SAR filings
- Access controls and need-to-know basis
- No communication with customer about SAR
- Training on tipping off prohibitions
- Procedures for managing customer relationships post-SAR
- Disciplinary measures for breaches

### Defensive Filing Prevention
- Quality over quantity focus
- Thorough investigation before filing
- Clear articulation of suspicion
- Avoidance of purely checkbox filings
- Meaningful narrative and analysis

## Performance Metrics

### Volume Metrics
- Number of SARs filed (monthly, quarterly, annually)
- SAR filing rate (SARs per transaction or customer)
- Breakdown by product, geography, typology
- Trend analysis over time

### Quality Metrics
- FIU feedback and quality scores
- Regulatory examination findings
- Completeness and accuracy of filings
- Narrative quality assessments
- Follow-up information requests from FIU

### Efficiency Metrics
- Time from alert to SAR decision
- Time from decision to filing
- Investigation time per case
- Backlog and aging of cases
- Resource utilization

### Effectiveness Metrics
- Conversion rate (alerts to SARs)
- FIU action rate (SARs leading to investigations)
- Law enforcement outcomes (arrests, prosecutions)
- Asset seizures or freezes
- Regulatory satisfaction and ratings

## Constraints and Considerations

- **Confidentiality**: Extreme sensitivity of SAR information
- **Resource Intensive**: Significant time and expertise required
- **Customer Relationships**: Balance compliance with customer retention
- **Liability**: Legal and reputational risks of both filing and not filing
- **International Complexity**: Different requirements across jurisdictions
- **Data Privacy**: GDPR considerations in SAR processing

## Success Criteria

- Zero regulatory findings related to SAR deficiencies
- 100% of SARs filed within regulatory deadlines
- High-quality SARs with positive FIU feedback
- No tipping off incidents
- Comprehensive documentation for all SAR decisions
- Strong MLRO independence and authority
- Effective use of SAR intelligence for risk management

## Stakeholder Coordination

- **FIUs**: Swedish Finanspolisen, Norwegian Økokrim, Danish Hvidvasksekretariatet
- **Law Enforcement**: Local police and international agencies
- **Regulators**: Finansinspektionen, Finanstilsynet (NO), Finanstilsynet (DK)
- **Internal**: Board, Management, Legal, Risk, Business Units
- **External**: Correspondent banks, industry groups

## Related Business Processes

- Fraud Detection Process (bus-process-003-fraud-detection)
- Customer Service Process (bus-process-007-customer-service)
- Transaction Processing

## Related Technology Components

- Transaction Monitoring System
- Case Management System
- goAML or National FIU Filing System
- Document Management System
- Secure Communication Channels
