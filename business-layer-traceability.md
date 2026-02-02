# Business Layer Traceability Matrix

This matrix shows how business elements trace to your motivation and strategy layers.

## Goal → Capability → Business Service Mapping

| Motivation Goal | Strategy Capability | Required Business Services |
|----------------|---------------------|---------------------------|
| **Digital Transformation** | Digital Banking Platform | • Digital Account Management<br>• Customer Onboarding<br>• Customer Support |
| **Enhance Digital Banking** | Omnichannel Engagement | • Digital Account Management<br>• Payment Processing<br>• Account Information |
| **Improve Operational Efficiency** | Process Automation | • Process Automation Workflow<br>• Transaction Monitoring<br>• Compliance Reporting |
| **Build Customer Trust** | Customer Trust & Privacy | • Consent Management<br>• KYC Verification<br>• Transaction Monitoring |
| **Enable Open Banking** | Open Banking Ecosystem | • Account Information Service (AIS)<br>• Payment Processing Service (PIS)<br>• Partner Onboarding |

## Requirement → Business Process Mapping

| Requirement | Required Business Process | Supporting Objects |
|------------|--------------------------|-------------------|
| **Mobile-First Design** | Customer Onboarding Process | Customer Profile, Account |
| **Seamless UX** | Customer Authentication Interaction | Customer Profile, API Token |
| **Process Automation** | Process Automation Workflow | Process Definition, Automation Rules |
| **API Platform** | Partner Onboarding Process | API Access Token, Third-Party Profile |
| **GDPR Compliance** | Consent Management Process | Customer Consent, Data Access Log |
| **DORA Compliance** | Incident Response Process | Incident Report, Recovery Plan |
| **Data Security** | KYC Verification Process | Customer Profile, Verification Records |

## Business Actor → Service Interaction

| Business Actor | Uses Services | Through Processes |
|---------------|---------------|-------------------|
| **Retail Customer** | • Digital Account Management<br>• Payment Processing<br>• Customer Support | • Customer Onboarding<br>• Payment Initiation<br>• Service Request |
| **Business Customer** | • Account Information<br>• Payment Processing<br>• Loan Origination | • Business Onboarding<br>• Payment Initiation<br>• Loan Application |
| **Third-Party Provider** | • Account Information (AIS)<br>• Payment Initiation (PIS) | • Partner Onboarding<br>• API Authentication<br>• Account Information Retrieval |
| **Customer Service Rep** | • Customer Support<br>• Account Management | • Customer Service Process<br>• Complaint Resolution |
| **Compliance Officer** | • Transaction Monitoring<br>• Compliance Reporting | • Compliance Review<br>• Incident Response |

## Coverage Analysis

### Well-Covered Areas ✓
- Digital transformation vision and goals
- Strategic capabilities defined
- Technology requirements documented
- Regulatory compliance requirements

### Gaps Requiring Business Elements ✗
1. **Customer Journey** - No business processes for end-to-end journeys
2. **Service Catalog** - No formal business services defined
3. **Actor Model** - No customer or user actors documented
4. **Event Model** - No business events for process orchestration
5. **Product Definitions** - No formal product structures

### Critical Dependencies

```
Strategy Layer (Capabilities)
        ↓
   [MISSING: Business Services]  ← YOU ARE HERE
        ↓
Application Layer (Components)
```

Without business services, you cannot trace:
- Which application components realize which strategic capabilities
- What processes need to be automated
- Who uses which applications and why
- What data objects support which business needs

## Architectural Debt

**Current State**: Strategy → Application (direct jump)
**Target State**: Strategy → Business → Application → Technology

**Risk**: Without the business layer, you may:
- Build applications that don't support actual business needs
- Automate the wrong processes
- Create APIs without clear business service definitions
- Struggle with impact analysis when requirements change

## Recommended Action Plan

### Week 1: Core Actors & Services
1. Create Retail Customer actor
2. Define 3 core business services:
   - Digital Account Management
   - Payment Processing  
   - Customer Support

### Week 2: Key Processes
3. Document Customer Onboarding Process
4. Document Payment Initiation Process
5. Link to existing Customer Service Process

### Week 3: Supporting Elements
6. Define business objects (Account, Payment Transaction)
7. Create business events (Customer Registration, Payment Initiated)
8. Add relationships to motivation/strategy elements

### Week 4: Review & Relationships
9. Update website generator and verify traceability
10. Create viewpoints showing goal → service → component flows
11. Validate with stakeholders

This will give you a complete, traceable architecture from strategic goals down to technical implementation.
