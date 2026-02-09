---
id: mot-reqt-014-transaction-monitoring-and-screening
owner: Chief Compliance Officer
status: active
version: ''
last_updated: '2026-02-03'
review_cycle: annual
next_review: '2027-02-03'
relationships:
- type: realization
  target: mot-goal-002-regulatory-compliance
  description: Core requirement for AML/CT compliance
- type: realization
  target: mot-prin-006-amlct-compliance-excellence
  description: Realizes AML/CT compliance principle
name: Transaction Monitoring and Screening
tags:
- transaction-monitoring
- aml
- compliance
- sanctions-screening
- real-time-monitoring
- financial-crime
archimate:
  type: requirement
  layer: motivation
  criticality: critical
extensions:
  properties:
    legacy-id: mot-requirement-014-transaction-monitoring
---
# Transaction Monitoring and Screening

All customer transactions must be monitored for suspicious patterns, screened against sanctions lists in real-time, checked for fraudulent behavior, and compared against expected activity based on customer profiles and KYC declarations to detect and prevent money laundering, terrorism financing, sanctions violations, and fraud.

## Scope

- Payment transactions (domestic and international)
- Wire transfers and SWIFT messages
- Cash deposits and withdrawals
- Card transactions
- Securities trading
- Account opening and closure activities
- Changes in customer behavior or risk profile
- Fraud detection and prevention
- Validation against KYC-declared business purpose and activity
- Industry-specific transaction pattern verification

## Key Requirements

### Real-Time Sanctions Screening
- Screening all transactions against comprehensive sanctions lists:
  - EU sanctions lists
  - OFAC (Office of Foreign Assets Control) lists
  - UN sanctions
  - National sanctions (Sweden, Norway, Denmark)
- Screening of counterparties, beneficiaries, and intermediaries
- Fuzzy matching algorithms to handle name variations
- Immediate blocking of sanctions hits
- Escalation procedures for potential matches
- Manual review and resolution workflow
- Documentation of screening decisions

### Transaction Monitoring Scenarios

#### AML/CT Detection Scenarios
- **Structuring**: Multiple transactions below reporting threshold
- **Rapid Movement of Funds**: Quick in-and-out transactions
- **Unusual Transaction Patterns**: Inconsistent with customer profile
- **High-Risk Jurisdictions**: Transactions to/from high-risk countries
- **High-Value Transactions**: Transactions above thresholds
- **Round Amount Transactions**: Unusual round amounts
- **Velocity**: Sudden increase in transaction frequency or volume
- **Cross-Border Activity**: Unexpected international transactions
- **Cash Intensive**: Unusual cash deposit/withdrawal patterns
- **Third-Party Payments**: Frequent third-party involvement
- **Trade-Based Money Laundering**: Import/export anomalies
- **Cryptocurrency Transactions**: Transactions involving crypto exchanges

#### Fraud Detection Scenarios
- **Account Takeover**: Unusual login patterns, device changes, sudden beneficiary additions
- **Authorized Push Payment (APP) Fraud**: Social engineering, impersonation scams
- **Card Fraud**: Unusual merchant categories, geographic anomalies, velocity patterns
- **Identity Theft**: New account fraud, synthetic identities
- **Internal Fraud**: Employee transactions outside normal parameters
- **Payment Fraud**: Duplicate payments, altered payment instructions
- **Phishing Attacks**: Credential compromise indicators
- **Mule Account Activity**: Rapid in-out transactions, layering patterns

#### Customer Profile Deviation Scenarios
- **KYC Declaration Mismatch**: Activity inconsistent with stated business purpose
- **Industry Pattern Deviation**: Transactions atypical for customer's industry sector
- **Geographic Mismatch**: Transactions to/from countries not aligned with declared business
- **Counterparty Mismatch**: Payments to/from entities inconsistent with business model
- **Volume Deviation**: Transaction volumes significantly different from KYC expectations
- **Product Usage Mismatch**: Use of products/services not aligned with stated needs
- **Occupation-Based Anomalies**: Income/transaction patterns inconsistent with occupation
- **Business Model Inconsistency**: Activity that doesn't fit declared revenue model

#### Customer Behavior Baseline Scenarios
- **Behavioral Change**: Sudden deviation from customer's own historical patterns
- **Dormancy Followed by Activity**: Inactive accounts suddenly becoming active
- **Peer Group Deviation**: Activity significantly different from similar customers
- **Seasonal Pattern Breaks**: Deviation from established seasonal transaction patterns
- **Time-of-Day Anomalies**: Transactions at unusual times for this customer
- **Channel Switching**: Sudden changes in preferred banking channels

### Real-Time Monitoring
- Immediate alert generation for high-risk scenarios
- Transaction blocking capability for critical risks
- Integration with payment processing systems
- Sub-second screening for customer experience
- Intelligent alert routing to appropriate investigators

### Batch Monitoring
- Daily, weekly, and monthly scenario runs
- Pattern detection across multiple transactions
- Peer group analysis and benchmarking
- Historical trend analysis
- Machine learning model predictions

### Alert Management
- Prioritization based on risk score
- Assignment to qualified investigators
- Defined investigation procedures and timeframes
- Documentation requirements for all alert dispositions
- Escalation paths for complex cases
- Quality assurance reviews

### Threshold Management
- Risk-based threshold configuration
- Regular calibration and optimization
- Jurisdiction-specific adjustments
- Customer segment differentiation
- Documentation of threshold decisions

### Customer Profile Integration
- **KYC Data Utilization**: Integration of all KYC information into monitoring rules
  - Stated business purpose and expected transaction types
  - Declared annual turnover and transaction volumes
  - Geographic scope of business operations
  - Nature of customers and suppliers
  - Product and service usage intentions
  - Source of funds and wealth information
  - Occupation and income levels (for retail customers)

- **Industry-Specific Rules**: Tailored monitoring based on business sector
  - Import/Export businesses: Trade finance patterns, shipping documentation
  - Retail businesses: Cash intensity, daily transaction patterns
  - Professional services: Fee structures, client payment patterns
  - Real estate: Large irregular transactions, deposit patterns
  - E-commerce: High transaction volumes, international payments
  - Non-profits: Donation patterns, expense types
  - Manufacturing: Supplier payment patterns, working capital cycles

- **Dynamic Risk Profiling**: Continuous adjustment of monitoring parameters
  - Automated risk score updates based on actual behavior
  - Trigger-based profile reviews when deviations detected
  - Integration with customer due diligence refresh cycles
  - Machine learning models for evolving customer baselines

- **Expected Activity Modeling**: Establish customer-specific expectations
  - Transaction volume ranges (daily, weekly, monthly)
  - Typical counterparty types and jurisdictions
  - Normal product usage patterns
  -omprehensive customer profile data integration:
  - KYC questionnaire responses
  - Business purpose and nature declarations
  - Industry classification codes (NACE, SIC)
  - Expected transaction patterns from onboarding
  - Beneficial ownership and control structures
  - Product holdings and usage history
  - Customer risk ratings and periodic review findings
- External data source integration (news, adverse media, industry benchmarks)
- Data lineage and audit trails
- Data quality monitoring and remediation
- Real-time data enrichment and validation

### Fraud Prevention Integration
- Coordination with fraud detection systems
- Shared intelligence on suspicious patterns
- Combined AML/fraud alert workflows where appropriate
- Cross-functional investigation for complex cases
- Unified view of customer risk (AML and fraud)
- Real-time fraud scoring integration
- Device and behavioral biometrics consider
## Compliance Measures

### System Capabilities
- Enterprise transaction monitoring platform with customer profile integration
- Integration with core banking, payment systems, and CRM
- Sanctions screening engine with multiple data sources
- Fraud detection system with real-time scoring
- Case management workflow system
- Reporting and analytics capabilities
- API connectivity for external data enrichment
- Machine learning and AI for pattern detection and behavioral modeling
- Customer 360-degree view with KYC data integration
- Industry benchmark and peer group analysis tools

### Scenario Development
- Typology research and scenario design
- Regular review and updates (at least annually)
- Regulatory guidance incorporation
- Industry best practice adoption
- Testing and validation before deployment
- Documentation of scenario logic and parameters
 differentiated by alert type:
  - AML/CT investigations
  - Fraud investigations
  - Customer profile deviation reviews
- Access to comprehensive customer information:
  - Complete transaction history
  - KYC documentation and declarations
  - Customer communications and interactions
  - Product applications and documentation
  - Industry benchmarks and peer comparisons
  - Historical investigation outcomes
- Research tools and external databases
- Customer contact procedures when clarification needed
- Documented investigation findings with clear rationale
- Decision-making criteria aligned with alert type
- Supervisor review and approval
- Escalation paths:
  - SAR filing for AML/CT concerns
  - Fraud case escalation for fraud indicators
  - Enhanced due diligence triggers for profile deviations
  - Account restrictions for immediate risks
- A/B testing of model improvements

### Data Quality
- Complete and accurate transaction data
- Customer profile data integration
- External data source integration (news, adverse media)
- Data lineage and audit trails
- Data quality monitoring and remediation, fraud cases, or meaningful findings)
- False positive rate (alerts dismissed after investigation)
- Time to detect suspicious activity or fraud
- Coverage of regulatory typologies
- SAR quality feedback from FIU
- Fraud detection and prevention rates
- Customer profile deviation detection accuracy
- KYC expectation vs. actual activity alignment
- Regulatory findings related to transaction monitoring
- Prevented fraud losses (financial impact)
- Documented investigation findings
- Clear decision-making criteria
- Supervisor review and approval
- Escalation to SAR filing when warranted

## Performance Metrics

### Effectiveness Metrics
- True positive rate (alerts that result in SARs or meaningful findings)
- False positive rate (alerts dismissed after investigation)
- Time to detect suspicious activity
- Coverage of regulatory typologies
- SAR quality feedback from FIU
- Regulatory findings related to transaction monitoring

### Efficiency Metrics
- Average investigation time per alert
- Alert backlog and aging
- Automation rate (auto-cleared vs. manual review)
- System availability and performance
- Staff productivity (alerts per investigator)

### Quality Metrics
- Investigation quality scores
- Documentation completeness
- Escalation accuracy
- Model performance (for ML scenarios)
- Scenario effectiveness scores

## Constraints and Considerations
; customers may question why legitimate transactions are flagged
- **Resource Intensive**: Significant investment in technology and staff
- **Evolving Threats**: Continuous adaptation to new money laundering and fraud typologies
- **Customer Experience**: Minimize friction from false positives while maintaining security
- **Data Completeness**: Monitoring effectiveness depends on quality of KYC data
- **Industry Variance**: Different industries require different baseline models
- **Customer Communication**: Balance between seeking clarification and tipping off concerns
- **Profile Maintenance**:for AML scenarios (industry benchmark varies)
- Fraud detection rate >70% of attempted fraud
- False positive rate reduction of 20% year-over-year through optimization
- 100% of high-priority alerts investigated within 24 hours
- Average alert investigation time <30 minutes for low-complexity alerts
- System availability >99.9%
- Real-time screening latency <200ms
- >90% of customers have complete and current KYC expectation profiles
- Customer profile deviation alerts lead to meaningful findings >25% of time
- Prevented fraud losses exceeding system investment costs
- Customer satisfaction maintained despite enhanced monitoring
## Success Criteria

- Zero regulatory findings related to transaction monitoring deficiencies
- True positive rate >15% (industry benchmark varies)
- False positive rate reduction of 20% year-over-year through optimization
- 100% of high-priority alerts investigated within 24 hours
- Average alert investigation time <30 minutes for low-complexity alerts
- System availability >99.9%
- Real-time screening latency <200ms

## Technology Requirements

- Scalable architecture supporting millions of transactions daily
- Low-latency real-time processing
- Advanced analytics and visualization
- Customer Onboarding Process (bus-process-011-customer-onboarding)
- Account Management Process (bus-process-001-account-management)
- Beneficial Ownership Identification (bus-process-002-beneficial-ownership-identification)

## Related Motivation Elements

- KYC and Customer Identification Requirements (mot-requirement-013-kyc-compliance)
- Enhanced Due Diligence Requirements (mot-requirement-016-enhanced-due-diligence)
- Suspicious Activity Reporting (mot-requirement-015-sar-reporting)
- AML/CT Compliance Excellence (mot-principle-006-aml-ct-compliance)
- Data Security and Privacy Protection (mot-requirement-007-data-security)

## Related Technology Components

- Transaction Monitoring System with customer profiling
- Sanctions Screening Engine
- Fraud Detection Platform
- Core Banking System
- CRM and Customer Data Platform
- Payment Processing Platform
- Data Analytics Platform
- Case Management System
- KYCd Detection Process (bus-process-003-fraud-detection)
- Customer Service Process (bus-process-007-customer-service)

## Related Technology Components

- Transaction Monitoring System
- Sanctions Screening Engine
- Core Banking System
- Payment Processing Platform
- Data Analytics Platform
- Case Management System
