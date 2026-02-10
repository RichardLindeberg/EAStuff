---
id: mot-reqt-013-kyc-and-customer-identification
owner: Chief Compliance Officer
status: active
version: '1.0'
last_updated: '2026-02-03'
review_cycle: annual
next_review: '2027-02-03'
relationships:
  - type: realization
    target: mot-goal-002-regulatory-compliance
    description: Required by regulatory compliance and financial crime prevention
  - type: realization
    target: mot-prin-006-amlct-compliance-excellence
    description: Realizes AML/CT compliance principle
name: KYC and Customer Identification Requirements
tags:
  - kyc
  - customer-due-diligence
  - compliance
  - aml
  - regulatory
  - onboarding
archimate:
  type: requirement
  layer: motivation
  criticality: critical
extensions:
  properties:
    legacy-id: mot-requirement-013-kyc-compliance
---
# KYC and Customer Identification Requirements

All customers must be properly identified, verified, and risk-assessed through comprehensive Know Your Customer (KYC) procedures in compliance with AML/CT regulations.

## Scope

- Individual customer onboarding (retail and private banking)
- Corporate and business customer onboarding
- Beneficial ownership identification and verification
- Ongoing customer due diligence and profile updates
- Enhanced due diligence for high-risk customers
- Simplified due diligence for low-risk situations (where permitted)

## Key Requirements

### Customer Identification
- Full name, date of birth, and nationality
- Permanent address and contact information
- Official identification document (passport, national ID, driver's license)
- Tax identification number (TIN)
- Occupation and source of funds/wealth
- Purpose and nature of business relationship

### Verification Standards
- Original or certified copies of identification documents
- Independent verification through reliable sources
- Face-to-face verification or electronic identification (eID)
- Video identification where permitted by regulation
- Document validity and authenticity checks
- Sanctions and PEP screening at onboarding

### Beneficial Ownership
- Identification of natural persons owning >25% (or applicable threshold)
- Ultimate beneficial owner (UBO) documentation
- Corporate structure diagrams for complex entities
- Verification of beneficial owners to same standard as customers
- Public beneficial ownership register checks

### Risk Assessment
- Customer risk categorization (low, medium, high)
- Risk factors: geography, occupation, product usage, transaction patterns
- Enhanced due diligence triggers
- Risk rating methodology and governance
- Regular risk reassessment (at least annually or upon trigger events)

### Politically Exposed Persons (PEPs)
- Identification of PEPs, their family members, and close associates
- Enhanced due diligence requirements
- Source of wealth and source of funds verification
- Senior management approval for PEP relationships
- Enhanced ongoing monitoring

### Record Keeping
- Retention of KYC documents for 5 years after relationship ends (or longer per local requirements)
- Audit trail of all due diligence activities
- Documentation of risk assessment rationale
- Records readily accessible for regulatory inspections

## Compliance Measures

### Onboarding Process
- Structured KYC questionnaires and data collection
- Document upload and verification workflows
- Automated sanctions and PEP screening
- Risk scoring algorithms
- Compliance review and approval gates
- Clear audit trail of decisions

### Technology Systems
- Customer Relationship Management (CRM) integration
- Document management and optical character recognition (OCR)
- Electronic identity verification (eID) integration
- Sanctions and PEP screening databases
- Risk scoring engines
- Workflow and case management systems

### Ongoing Due Diligence
- Periodic KYC reviews based on risk (high-risk: annually; medium: every 2-3 years; low: every 3-5 years)
- Event-triggered reviews (unusual activity, media reports, regulatory alerts)
- Automated reminders for review cycles
- Customer outreach for information updates
- Documentation of review findings and decisions

### Quality Assurance
- Regular quality reviews of KYC files
- Sampling and testing of compliance controls
- Internal audit assessments
- Regulatory examination preparedness
- Remediation of deficiencies

## Constraints and Considerations

- **Customer Experience**: Balance thorough due diligence with smooth onboarding
- **Data Privacy**: GDPR compliance in collecting and storing personal data
- **Cross-Border**: Navigate different requirements across Nordic jurisdictions
- **Digital Channels**: Implement robust remote identification procedures
- **Resource Intensive**: Significant manual effort, particularly for complex entities
- **Information Quality**: Dependence on customer-provided information

## Success Criteria

- **Regulatory Compliance**: Zero significant KYC-related findings from regulators
- **Data Completeness**: >95% of required KYC fields populated
- **Review Timeliness**: 100% of periodic reviews completed on schedule
- **Onboarding Time**: Average time from application to account opening within target
- **Customer Satisfaction**: Minimize friction while maintaining compliance
- **Audit Quality**: High-quality KYC files that withstand scrutiny

## Related Business Processes

- Customer Onboarding Process (bus-process-011-customer-onboarding)
- Beneficial Ownership Identification Process (bus-process-002-beneficial-ownership-identification)
- Customer Service Process (bus-process-007-customer-service)

## Related Technology

- Customer Portal Application
- Core Banking System
- Document Management System
- Identity Verification Services
- Sanctions and PEP Screening Platform
