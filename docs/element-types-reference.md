# ArchiMate 3.2 Element Type Reference

This document provides a quick reference for ArchiMate 3.2 element types organized by layer.

## Strategy Layer

Elements that describe strategic direction and capabilities.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `resource` | An asset owned by the organization | Human resources, financial assets |
| `capability` | An ability to perform a particular kind of work | Customer service capability |
| `value-stream` | A sequence of activities that create value | Order fulfillment value stream |
| `course-of-action` | An approach or plan for configuring capabilities | Digital transformation strategy |

## Business Layer

Elements that describe business services, processes, and organizational structure.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `business-actor` | An organizational entity capable of performing behavior | Department, organization unit |
| `business-role` | The responsibility for performing specific behavior | Customer service agent |
| `business-process` | A sequence of business behaviors | Order processing, customer onboarding |
| `business-function` | A collection of business behavior | Finance, HR, Sales |
| `business-service` | An explicitly defined behavior | Customer support service |
| `business-object` | A concept used within a business domain | Customer, Order, Invoice |
| `product` | A coherent collection of services | Banking product, Insurance policy |

## Application Layer

Elements that describe application structure and behavior.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `application-component` | A modular, deployable software component | Customer Portal, CRM System |
| `application-service` | An automated service | Authentication service, Payment processing |
| `application-function` | Automated behavior | Data validation, Report generation |
| `application-process` | A sequence of automated behaviors | Batch processing workflow |
| `data-object` | Data structured for automated processing | Customer record, Transaction log |

## Technology Layer

Elements that describe technology infrastructure and platforms.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `node` | A computational or physical resource | Server, Virtual machine |
| `device` | A physical IT resource | Router, Firewall, Workstation |
| `system-software` | Software environment for applications | Operating system, Database system |
| `technology-service` | Infrastructure service | Storage service, Messaging service |
| `artifact` | A physical piece of data | Configuration file, Library, Database |
| `communication-network` | A set of structures for data exchange | LAN, WAN, Internet |

## Physical Layer

Elements that describe physical resources and equipment.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `equipment` | Physical machine or equipment | Manufacturing machine, HVAC system |
| `facility` | A physical location or environment | Building, Data center, Warehouse |
| `distribution-network` | Physical network for distribution | Supply chain network, Power grid |
| `material` | Physical matter or energy | Raw materials, Products |

## Motivation Layer

Elements that describe motivations, drivers, and goals.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `stakeholder` | A person or organization with interests | CEO, Customer, Regulator |
| `driver` | Something that creates motivation | Market competition, Regulation |
| `goal` | A high-level statement of intent | Increase market share |
| `outcome` | An end result | Improved customer satisfaction |
| `principle` | A normative property or guideline | Security first, Customer-centric |
| `requirement` | A statement of need | System must support 1000 users |
| `constraint` | A restriction on the way outcome is realized | Budget limit, Time constraint |

## Implementation & Migration Layer

Elements that describe implementation and migration projects.

| Element Type | Description | Example |
|-------------|-------------|---------|
| `work-package` | A series of actions for a specific result | Infrastructure upgrade project |
| `deliverable` | A precisely-defined result of a work package | Technical specification, Software release |
| `implementation-event` | A state change in implementation | Go-live, Milestone achieved |
| `plateau` | A relatively stable state of the architecture | Current state, Target state |
| `gap` | A difference between plateaus | Missing capability, Process gap |

## Common Relationship Types

| Relationship | Description | Example |
|-------------|-------------|---------|
| `composition` | Consists of, contains | Application contains components |
| `aggregation` | Groups together | Role aggregates functions |
| `assignment` | Allocates responsibility | Role assigned to process |
| `realization` | Implements, fulfills | Component realizes service |
| `serving` | Provides functionality to | Service serves process |
| `access` | Provides access to data | Process accesses data object |
| `influence` | Affects, modifies | Goal influences requirement |
| `triggering` | Temporal/causal relationship | Event triggers process |
| `flow` | Transfer or exchange | Data flows between processes |
| `specialization` | Is a kind of | Mobile app specializes app |
