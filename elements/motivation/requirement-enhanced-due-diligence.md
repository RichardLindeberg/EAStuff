---
id: mot-requirement-016-enhanced-due-diligence
name: Enhanced Due Diligence (EDD) Requirements
type: requirement
layer: motivation
relationships:
  - type: realizationOf
    target: mot-goal-001-regulatory-compliance
    description: Risk-based requirement for high-risk customers and situations
  - type: realizationOf
    target: mot-principle-006-aml-ct-compliance
    description: Realizes AML/CT compliance principle
properties:
  owner: Chief Compliance Officer
  status: active
  criticality: high
  last-updated: "2026-02-03"
tags:
  - edd
  - enhanced-due-diligence
  - aml
  - compliance
  - high-risk
  - pep
  - kyc
---

# Enhanced Due Diligence (EDD) Requirements

High-risk customers, relationships, and transactions require enhanced due diligence measures beyond standard KYC to adequately assess and mitigate money laundering and terrorism financing risks.

## Scope

- Politically Exposed Persons (PEPs) and their close associates
- High-risk jurisdictions (FATF blacklist, greylist)
- Complex corporate structures with opaque ownership
- Cash-intensive businesses
- High-value customers or transactions
- Correspondent banking relationships
- Non-face-to-face customer relationships (where risk is elevated)
- Customers with adverse media or reputational concerns
- Trust and fiduciary service providers
- Money service businesses and cryptocurrency exchanges

## Key Requirements

### Risk Assessment Criteria
- **Geographic Risk**: Customer or transaction involves high-risk countries
- **Customer Type Risk**: PEPs, shell companies, cash-intensive businesses
- **Product/Service Risk**: Private banking, complex structured products
- **Transaction Risk**: Large, unusual, or complex transactions
- **Channel Risk**: Remote onboarding without face-to-face meeting
- **Behavior Risk**: Unusual activity, reluctance to provide information
- **Reputational Risk**: Adverse media, known associations

### EDD Triggers
- Customer risk rating of "High"
- PEP identification at onboarding or during relationship
- Transaction monitoring alerts requiring deeper investigation
- Significant changes in customer profile or behavior
- Adverse media hits or negative news
- Regulatory guidance or internal policy
- Correspondent bank due diligence requirements
- Red flags during standard due diligence

### Enhanced Identification and Verification
- Additional identity documents beyond standard requirements
- Independent verification of identity through multiple sources
- Face-to-face meeting or video identification even for digital channels
- In-person document verification where feasible
- Enhanced authentication procedures
- Verification of address through independent means (utility bills, property records)

### Source of Wealth (SOW) and Source of Funds (SOF)
- **Source of Wealth**: Documentation of how customer accumulated total wealth
  - Employment history and income
  - Business ownership and valuations
  - Inheritance documentation
  - Investment gains and property sales
  - Third-party verification where possible
- **Source of Funds**: Documentation of origin of specific funds in relationship
  - Pay slips, tax returns, audited financials
  - Sales agreements, loan documents
  - Investment statements, trust distributions
  - Ongoing verification for large transactions

### Beneficial Ownership Deep Dive
- Verification of ultimate beneficial owners (UBOs) beyond regulatory minimum
- Corporate structure diagrams and ownership charts
- Nominee shareholder arrangements and true ownership
- Control mechanisms beyond direct ownership
- Verification of UBO identity and source of wealth
- Legal opinions on complex structures

### Purpose and Nature of Relationship
- Detailed understanding of intended account usage
- Expected transaction volumes and patterns
- Business model and revenue sources
- Customer's customers and suppliers
- Legitimacy of business purpose
- Documentation of anticipated activity

### Ongoing Monitoring - Enhanced
- More frequent periodic reviews (e.g., annually for high-risk vs. every 3-5 years for low-risk)
- Real-time transaction monitoring with lower thresholds
- Proactive adverse media screening (daily or weekly)
- Relationship manager engagement and attestations
- Trigger-based reviews for any unusual activity
- Enhanced record keeping and documentation

### Senior Management Approval
- Requirement for senior management approval to establish relationship
- Re-approval for significant changes in relationship
- Documentation of approval rationale
- Clear accountability and decision-making authority
- Escalation procedures for concerns

### PEP-Specific Requirements
- Identification of PEP status (foreign, domestic, international organization)
- Identification of family members and close associates
- Senior management approval mandatory
- Enhanced ongoing monitoring (at least annually)
- Source of wealth mandatory
- Monitoring of political developments affecting PEP
- Exit strategy if PEP becomes sanctioned or implicated in scandal

## Compliance Measures

### Organization and Resources
- Specialized EDD team or function
- Training on red flags and EDD procedures
- Access to research tools and databases
- Clear escalation paths
- Subject matter expert support
- Quality assurance function

### Technology and Data Sources
- Enhanced due diligence workflow system
- Access to premium databases (Dow Jones, World-Check, LexisNexis)
- Adverse media screening tools
- Corporate registry and UBO databases
- Sanctions and PEP screening with lower match thresholds
- Document verification and fraud detection tools
- Integration with CRM and core banking systems

### Investigation Procedures
- Standardized EDD questionnaires and templates
- Investigation playbooks by risk type
- Open-source intelligence (OSINT) techniques
- Use of external investigators where needed
- Documentation requirements and quality standards
- Peer review and quality assurance
- Escalation for borderline cases

### Decision Framework
- Clear criteria for accepting/declining high-risk relationships
- Risk appetite statement from Board and senior management
- Decision-making authority matrix
- Documentation of risk/reward analysis
- Ongoing risk management plans
- Exit strategy for deteriorating relationships

### Record Keeping
- Comprehensive EDD files with all supporting documentation
- Investigation notes and findings
- Approval documentation and rationale
- Ongoing monitoring records
- Periodic review outcomes
- Retention for at least 5 years post-relationship

## Performance Metrics

### Coverage Metrics
- Percentage of high-risk customers with complete EDD
- Timeliness of EDD completion
- Periodic review completion rates
- Senior management approval rates

### Quality Metrics
- EDD file completeness scores
- Regulatory examination findings on EDD
- Internal audit findings
- SOW/SOF verification quality
- Documentation standards compliance

### Efficiency Metrics
- Average time to complete EDD
- Resource allocation (FTEs dedicated to EDD)
- Use of automation and technology
- Vendor spending on databases and tools

### Risk Metrics
- High-risk customer exit/decline rate
- SARs filed for high-risk customers (as percentage)
- Regulatory or reputational issues from high-risk relationships
- False positive rate on PEP/adverse media screening

## Constraints and Considerations

- **Customer Experience**: Extensive requirements may deter legitimate high-value customers
- **Complexity**: EDD is time-consuming and resource-intensive
- **Subjectivity**: Risk assessment and sufficiency of EDD involves judgment
- **Information Availability**: Difficulty obtaining documentation in some jurisdictions
- **Cost**: Premium databases and specialist resources are expensive
- **Privacy**: Balance invasive inquiries with data protection requirements

## Success Criteria

- Zero regulatory findings related to inadequate EDD
- 100% of high-risk customers have approved EDD before account opening
- Annual review completion rate >95%
- All PEPs identified and appropriately managed
- Senior management approval documented for all high-risk relationships
- Comprehensive SOW/SOF documentation for high-risk customers
- Clear audit trail demonstrating risk-based approach

## Relationship with Standard KYC

EDD builds upon standard KYC (mot-requirement-013-kyc-compliance) and is triggered by risk assessment. All standard KYC requirements apply, plus the enhanced measures described here.

## Related Business Processes

- Customer Onboarding Process (bus-process-011-customer-onboarding)
- Beneficial Ownership Identification (bus-process-002-beneficial-ownership-identification)
- Corporate Account Management (bus-process-006-corporate-account-management)

## Related Technology Components

- Customer Relationship Management (CRM) System
- KYC/AML Compliance Platform
- Document Management System
- Screening and Monitoring Systems
- Research and Intelligence Databases
