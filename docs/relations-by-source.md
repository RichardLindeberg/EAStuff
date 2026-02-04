# ArchiMate Relations (Source → Target)

This document lists allowed relationships from each source concept to each target concept, based on [schemas/relations.xml](../schemas/relations.xml).

## Relationship Code Mapping

| Code | Relationship |
|------|--------------|
| **a** | Access |
| **c** | Composition |
| **f** | Flow |
| **g** | Aggregation |
| **i** | Assignment |
| **n** | Influence |
| **o** | Association |
| **r** | Realization |
| **s** | Specialization |
| **t** | Triggering |
| **v** | Serving |

## Relations by Source Concept

### Application / Collaboration (ApplicationCollaboration)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Application / Component (ApplicationComponent)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Application / Event (ApplicationEvent)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Application / Function (ApplicationFunction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Application / Interaction (ApplicationInteraction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Application / Interface (ApplicationInterface)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Application / Process (ApplicationProcess)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Application / Service (ApplicationService)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Artifact (Artifact)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o), Realization (r) |
| Application / Component (ApplicationComponent) | Association (o), Realization (r) |
| Application / Event (ApplicationEvent) | Association (o), Realization (r) |
| Application / Function (ApplicationFunction) | Association (o), Realization (r) |
| Application / Interaction (ApplicationInteraction) | Association (o), Realization (r) |
| Application / Interface (ApplicationInterface) | Association (o), Realization (r) |
| Application / Process (ApplicationProcess) | Association (o), Realization (r) |
| Application / Service (ApplicationService) | Association (o), Realization (r) |
| Artifact (Artifact) | Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o), Realization (r) |
| Business / Function (BusinessFunction) | Association (o), Realization (r) |
| Business / Interaction (BusinessInteraction) | Association (o), Realization (r) |
| Business / Interface (BusinessInterface) | Association (o), Realization (r) |
| Business / Object (BusinessObject) | Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Association (o), Realization (r) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o), Realization (r) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o), Realization (r) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o), Realization (r) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o), Realization (r) |
| Technology / Function (TechnologyFunction) | Association (o), Realization (r) |
| Technology / Interaction (TechnologyInteraction) | Association (o), Realization (r) |
| Technology / Interface (TechnologyInterface) | Association (o), Realization (r) |
| Technology / Process (TechnologyProcess) | Association (o), Realization (r) |
| Technology / Service (TechnologyService) | Association (o), Realization (r) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Assessment (Assessment)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Business / Actor (BusinessActor)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Business / Collaboration (BusinessCollaboration)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Business / Event (BusinessEvent)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Business / Function (BusinessFunction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Business / Interaction (BusinessInteraction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Business / Interface (BusinessInterface)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Business / Object (BusinessObject)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Business / Process (BusinessProcess)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Business / Role (BusinessRole)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Business / Service (BusinessService)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Capability (Capability)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Work Package (WorkPackage) | Association (o) |

### Communication Network (CommunicationNetwork)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Constraint (Constraint)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Contract (Contract)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Course Of Action (CourseOfAction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Work Package (WorkPackage) | Association (o) |

### Data Object (DataObject)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Deliverable (Deliverable)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o), Realization (r) |
| Application / Component (ApplicationComponent) | Association (o), Realization (r) |
| Application / Event (ApplicationEvent) | Association (o), Realization (r) |
| Application / Function (ApplicationFunction) | Association (o), Realization (r) |
| Application / Interaction (ApplicationInteraction) | Association (o), Realization (r) |
| Application / Interface (ApplicationInterface) | Association (o), Realization (r) |
| Application / Process (ApplicationProcess) | Association (o), Realization (r) |
| Application / Service (ApplicationService) | Association (o), Realization (r) |
| Artifact (Artifact) | Association (o), Realization (r) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o), Realization (r) |
| Business / Collaboration (BusinessCollaboration) | Association (o), Realization (r) |
| Business / Event (BusinessEvent) | Association (o), Realization (r) |
| Business / Function (BusinessFunction) | Association (o), Realization (r) |
| Business / Interaction (BusinessInteraction) | Association (o), Realization (r) |
| Business / Interface (BusinessInterface) | Association (o), Realization (r) |
| Business / Object (BusinessObject) | Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Association (o), Realization (r) |
| Business / Role (BusinessRole) | Association (o), Realization (r) |
| Business / Service (BusinessService) | Association (o), Realization (r) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o), Realization (r) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o), Realization (r) |
| Deliverable (Deliverable) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Device (Device) | Association (o), Realization (r) |
| Distribution Network (DistributionNetwork) | Association (o), Realization (r) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o), Realization (r) |
| Facility (Facility) | Association (o), Realization (r) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o), Realization (r) |
| Material (Material) | Association (o), Realization (r) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o), Realization (r) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o), Realization (r) |
| Plateau (Plateau) | Association (o), Realization (r) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o), Realization (r) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o), Realization (r) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Association (o), Realization (r) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o), Realization (r) |
| Technology / Event (TechnologyEvent) | Association (o), Realization (r) |
| Technology / Function (TechnologyFunction) | Association (o), Realization (r) |
| Technology / Interaction (TechnologyInteraction) | Association (o), Realization (r) |
| Technology / Interface (TechnologyInterface) | Association (o), Realization (r) |
| Technology / Process (TechnologyProcess) | Association (o), Realization (r) |
| Technology / Service (TechnologyService) | Association (o), Realization (r) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Device (Device)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Distribution Network (DistributionNetwork)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Driver (Driver)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Equipment (Equipment)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Facility (Facility)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Gap (Gap)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Goal (Goal) | Association (o) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Goal (Goal)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Grouping (Grouping)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Composition (c), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s) |
| Assessment (Assessment) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Business / Actor (BusinessActor) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Business / Process (BusinessProcess) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Capability (Capability) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Communication Network (CommunicationNetwork) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Constraint (Constraint) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Contract (Contract) | Access (a), Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Course Of Action (CourseOfAction) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Data Object (DataObject) | Access (a), Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Deliverable (Deliverable) | Access (a), Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Device (Device) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Driver (Driver) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Equipment (Equipment) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Facility (Facility) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Gap (Gap) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Goal (Goal) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Specialization (s), Triggering (t) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Composition (c), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s) |
| Meaning (Meaning) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Node (Node) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Outcome (Outcome) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Path (Path) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Plateau (Plateau) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t) |
| Principle (Principle) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Product (Product) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Relationship (Relationship) | Composition (c), Aggregation (g), Association (o) |
| Representation (Representation) | Access (a), Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Requirement (Requirement) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Resource (Resource) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Stakeholder (Stakeholder) | Composition (c), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Value (Value) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Value Stream (ValueStream) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Work Package (WorkPackage) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Specialization (s), Triggering (t) |

### Implementation / Event (ImplementationEvent)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Access (a), Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Specialization (s), Triggering (t) |
| Implementation / Event (ImplementationEvent) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Flow (f), Association (o), Triggering (t) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Flow (f), Association (o), Triggering (t) |

### Junction (Junction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o), Realization (r) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Data Object (DataObject) | Access (a), Association (o), Realization (r) |
| Deliverable (Deliverable) | Access (a), Association (o), Realization (r) |
| Device (Device) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Flow (f), Assignment (i), Influence (n), Association (o), Realization (r), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Flow (f), Assignment (i), Association (o), Triggering (t) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o), Realization (r) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Plateau (Plateau) | Flow (f), Association (o), Realization (r), Triggering (t) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Relationship (Relationship) |  |
| Representation (Representation) | Access (a), Association (o), Realization (r) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Work Package (WorkPackage) | Flow (f), Assignment (i), Association (o), Triggering (t) |

### Location (Location)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Composition (c), Aggregation (g), Assignment (i), Association (o) |
| Assessment (Assessment) | Composition (c), Aggregation (g), Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Business / Process (BusinessProcess) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Course Of Action (CourseOfAction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Deliverable (Deliverable) | Composition (c), Aggregation (g), Association (o) |
| Device (Device) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Composition (c), Aggregation (g), Influence (n), Association (o) |
| Equipment (Equipment) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Gap (Gap) | Composition (c), Aggregation (g), Association (o) |
| Goal (Goal) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Composition (c), Aggregation (g), Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Composition (c), Aggregation (g), Assignment (i), Association (o) |
| Meaning (Meaning) | Composition (c), Aggregation (g), Influence (n), Association (o) |
| Node (Node) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Outcome (Outcome) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Path (Path) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Plateau (Plateau) | Composition (c), Aggregation (g), Association (o) |
| Principle (Principle) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Product (Product) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Composition (c), Aggregation (g), Association (o) |
| Representation (Representation) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Requirement (Requirement) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Composition (c), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Composition (c), Aggregation (g), Influence (n), Association (o) |
| Value Stream (ValueStream) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Work Package (WorkPackage) | Composition (c), Aggregation (g), Assignment (i), Association (o) |

### Material (Material)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o), Realization (r) |
| Application / Component (ApplicationComponent) | Association (o), Realization (r) |
| Application / Event (ApplicationEvent) | Association (o), Realization (r) |
| Application / Function (ApplicationFunction) | Association (o), Realization (r) |
| Application / Interaction (ApplicationInteraction) | Association (o), Realization (r) |
| Application / Interface (ApplicationInterface) | Association (o), Realization (r) |
| Application / Process (ApplicationProcess) | Association (o), Realization (r) |
| Application / Service (ApplicationService) | Association (o), Realization (r) |
| Artifact (Artifact) | Association (o), Realization (r) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o), Realization (r) |
| Business / Function (BusinessFunction) | Association (o), Realization (r) |
| Business / Interaction (BusinessInteraction) | Association (o), Realization (r) |
| Business / Interface (BusinessInterface) | Association (o), Realization (r) |
| Business / Object (BusinessObject) | Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Association (o), Realization (r) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o), Realization (r) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o), Realization (r) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o), Realization (r) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o), Realization (r) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Composition (c), Aggregation (g), Association (o), Realization (r), Specialization (s) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o), Realization (r) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o), Realization (r) |
| Technology / Function (TechnologyFunction) | Association (o), Realization (r) |
| Technology / Interaction (TechnologyInteraction) | Association (o), Realization (r) |
| Technology / Interface (TechnologyInterface) | Association (o), Realization (r) |
| Technology / Process (TechnologyProcess) | Association (o), Realization (r) |
| Technology / Service (TechnologyService) | Association (o), Realization (r) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Meaning (Meaning)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Node (Node)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Outcome (Outcome)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Path (Path)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Plateau (Plateau)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Component (ApplicationComponent) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Event (ApplicationEvent) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Function (ApplicationFunction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Interaction (ApplicationInteraction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Interface (ApplicationInterface) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Process (ApplicationProcess) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Application / Service (ApplicationService) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Artifact (Artifact) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Collaboration (BusinessCollaboration) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Event (BusinessEvent) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Function (BusinessFunction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Interaction (BusinessInteraction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Interface (BusinessInterface) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Object (BusinessObject) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Role (BusinessRole) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Business / Service (BusinessService) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Capability (Capability) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Constraint (Constraint) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Data Object (DataObject) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Deliverable (Deliverable) | Access (a), Association (o) |
| Device (Device) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Distribution Network (DistributionNetwork) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Facility (Facility) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t) |
| Implementation / Event (ImplementationEvent) | Flow (f), Association (o), Triggering (t) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Material (Material) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Outcome (Outcome) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Path (Path) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Plateau (Plateau) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Relationship (Relationship) | Composition (c), Aggregation (g), Association (o) |
| Representation (Representation) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Requirement (Requirement) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Collaboration (TechnologyCollaboration) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Event (TechnologyEvent) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Function (TechnologyFunction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Interaction (TechnologyInteraction) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Interface (TechnologyInterface) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Process (TechnologyProcess) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Technology / Service (TechnologyService) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Composition (c), Aggregation (g), Association (o), Realization (r) |
| Work Package (WorkPackage) | Flow (f), Association (o), Triggering (t) |

### Principle (Principle)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Product (Product)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Composition (c), Aggregation (g), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Composition (c), Flow (f), Aggregation (g), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Relationship (Relationship)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Association (o) |
| Grouping (Grouping) | Association (o) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) |  |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) |  |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Representation (Representation)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Composition (c), Aggregation (g), Association (o), Specialization (s) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Requirement (Requirement)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Resource (Resource)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Work Package (WorkPackage) | Association (o) |

### Stakeholder (Stakeholder)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### System Software (SystemSoftware)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Technology / Collaboration (TechnologyCollaboration)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Assignment (i), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Assignment (i), Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Assignment (i), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Aggregation (g), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Assignment (i), Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Assignment (i), Association (o) |

### Technology / Event (TechnologyEvent)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Technology / Function (TechnologyFunction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Technology / Interaction (TechnologyInteraction)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Technology / Interface (TechnologyInterface)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Assignment (i), Association (o), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Technology / Process (TechnologyProcess)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Composition (c), Flow (f), Aggregation (g), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Technology / Service (TechnologyService)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Component (ApplicationComponent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Event (ApplicationEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Function (ApplicationFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interaction (ApplicationInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Interface (ApplicationInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Process (ApplicationProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Application / Service (ApplicationService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Artifact (Artifact) | Access (a), Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Collaboration (BusinessCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Event (BusinessEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Function (BusinessFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interaction (BusinessInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Interface (BusinessInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Object (BusinessObject) | Access (a), Association (o) |
| Business / Process (BusinessProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Role (BusinessRole) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Business / Service (BusinessService) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Access (a), Association (o) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Access (a), Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Distribution Network (DistributionNetwork) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Facility (Facility) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Material (Material) | Access (a), Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Access (a), Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Collaboration (TechnologyCollaboration) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Event (TechnologyEvent) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Function (TechnologyFunction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interaction (TechnologyInteraction) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Interface (TechnologyInterface) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Process (TechnologyProcess) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Technology / Service (TechnologyService) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Association (o) |

### Value (Value)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Association (o) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Association (o) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o) |
| Grouping (Grouping) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o) |
| Resource (Resource) | Association (o) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Composition (c), Aggregation (g), Influence (n), Association (o), Specialization (s) |
| Value Stream (ValueStream) | Association (o) |
| Work Package (WorkPackage) | Association (o) |

### Value Stream (ValueStream)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o) |
| Application / Component (ApplicationComponent) | Association (o) |
| Application / Event (ApplicationEvent) | Association (o) |
| Application / Function (ApplicationFunction) | Association (o) |
| Application / Interaction (ApplicationInteraction) | Association (o) |
| Application / Interface (ApplicationInterface) | Association (o) |
| Application / Process (ApplicationProcess) | Association (o) |
| Application / Service (ApplicationService) | Association (o) |
| Artifact (Artifact) | Association (o) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o) |
| Business / Collaboration (BusinessCollaboration) | Association (o) |
| Business / Event (BusinessEvent) | Association (o) |
| Business / Function (BusinessFunction) | Association (o) |
| Business / Interaction (BusinessInteraction) | Association (o) |
| Business / Interface (BusinessInterface) | Association (o) |
| Business / Object (BusinessObject) | Association (o) |
| Business / Process (BusinessProcess) | Association (o) |
| Business / Role (BusinessRole) | Association (o) |
| Business / Service (BusinessService) | Association (o) |
| Capability (Capability) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Communication Network (CommunicationNetwork) | Association (o) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o) |
| Course Of Action (CourseOfAction) | Flow (f), Association (o), Realization (r), Triggering (t), Serving (v) |
| Data Object (DataObject) | Association (o) |
| Deliverable (Deliverable) | Association (o) |
| Device (Device) | Association (o) |
| Distribution Network (DistributionNetwork) | Association (o) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o) |
| Facility (Facility) | Association (o) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Implementation / Event (ImplementationEvent) | Association (o) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o) |
| Material (Material) | Association (o) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o) |
| Plateau (Plateau) | Association (o) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Flow (f), Association (o), Triggering (t), Serving (v) |
| Stakeholder (Stakeholder) | Influence (n), Association (o) |
| System Software (SystemSoftware) | Association (o) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o) |
| Technology / Event (TechnologyEvent) | Association (o) |
| Technology / Function (TechnologyFunction) | Association (o) |
| Technology / Interaction (TechnologyInteraction) | Association (o) |
| Technology / Interface (TechnologyInterface) | Association (o) |
| Technology / Process (TechnologyProcess) | Association (o) |
| Technology / Service (TechnologyService) | Association (o) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t), Serving (v) |
| Work Package (WorkPackage) | Association (o) |

### Work Package (WorkPackage)

| Target | Relationships |
|--------|---------------|
| Application / Collaboration (ApplicationCollaboration) | Association (o), Realization (r) |
| Application / Component (ApplicationComponent) | Association (o), Realization (r) |
| Application / Event (ApplicationEvent) | Association (o), Realization (r) |
| Application / Function (ApplicationFunction) | Association (o), Realization (r) |
| Application / Interaction (ApplicationInteraction) | Association (o), Realization (r) |
| Application / Interface (ApplicationInterface) | Association (o), Realization (r) |
| Application / Process (ApplicationProcess) | Association (o), Realization (r) |
| Application / Service (ApplicationService) | Association (o), Realization (r) |
| Artifact (Artifact) | Association (o), Realization (r) |
| Assessment (Assessment) | Influence (n), Association (o) |
| Business / Actor (BusinessActor) | Association (o), Realization (r) |
| Business / Collaboration (BusinessCollaboration) | Association (o), Realization (r) |
| Business / Event (BusinessEvent) | Association (o), Realization (r) |
| Business / Function (BusinessFunction) | Association (o), Realization (r) |
| Business / Interaction (BusinessInteraction) | Association (o), Realization (r) |
| Business / Interface (BusinessInterface) | Association (o), Realization (r) |
| Business / Object (BusinessObject) | Association (o), Realization (r) |
| Business / Process (BusinessProcess) | Association (o), Realization (r) |
| Business / Role (BusinessRole) | Association (o), Realization (r) |
| Business / Service (BusinessService) | Association (o), Realization (r) |
| Capability (Capability) | Association (o), Realization (r) |
| Communication Network (CommunicationNetwork) | Association (o), Realization (r) |
| Constraint (Constraint) | Influence (n), Association (o), Realization (r) |
| Contract (Contract) | Association (o), Realization (r) |
| Course Of Action (CourseOfAction) | Association (o), Realization (r) |
| Data Object (DataObject) | Association (o), Realization (r) |
| Deliverable (Deliverable) | Access (a), Association (o), Realization (r) |
| Device (Device) | Association (o), Realization (r) |
| Distribution Network (DistributionNetwork) | Association (o), Realization (r) |
| Driver (Driver) | Influence (n), Association (o) |
| Equipment (Equipment) | Association (o), Realization (r) |
| Facility (Facility) | Association (o), Realization (r) |
| Gap (Gap) | Association (o) |
| Goal (Goal) | Influence (n), Association (o), Realization (r) |
| Grouping (Grouping) | Access (a), Composition (c), Flow (f), Aggregation (g), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t) |
| Implementation / Event (ImplementationEvent) | Flow (f), Association (o), Triggering (t) |
| Junction (Junction) | Access (a), Composition (c), Flow (f), Aggregation (g), Assignment (i), Influence (n), Association (o), Realization (r), Specialization (s), Triggering (t), Serving (v) |
| Location (Location) | Association (o), Realization (r) |
| Material (Material) | Association (o), Realization (r) |
| Meaning (Meaning) | Influence (n), Association (o) |
| Node (Node) | Association (o), Realization (r) |
| Outcome (Outcome) | Influence (n), Association (o), Realization (r) |
| Path (Path) | Association (o), Realization (r) |
| Plateau (Plateau) | Flow (f), Association (o), Realization (r), Triggering (t) |
| Principle (Principle) | Influence (n), Association (o), Realization (r) |
| Product (Product) | Association (o), Realization (r) |
| Relationship (Relationship) | Association (o) |
| Representation (Representation) | Association (o), Realization (r) |
| Requirement (Requirement) | Influence (n), Association (o), Realization (r) |
| Resource (Resource) | Association (o), Realization (r) |
| Stakeholder (Stakeholder) | Influence (n), Association (o), Realization (r) |
| System Software (SystemSoftware) | Association (o), Realization (r) |
| Technology / Collaboration (TechnologyCollaboration) | Association (o), Realization (r) |
| Technology / Event (TechnologyEvent) | Association (o), Realization (r) |
| Technology / Function (TechnologyFunction) | Association (o), Realization (r) |
| Technology / Interaction (TechnologyInteraction) | Association (o), Realization (r) |
| Technology / Interface (TechnologyInterface) | Association (o), Realization (r) |
| Technology / Process (TechnologyProcess) | Association (o), Realization (r) |
| Technology / Service (TechnologyService) | Association (o), Realization (r) |
| Value (Value) | Influence (n), Association (o) |
| Value Stream (ValueStream) | Association (o), Realization (r) |
| Work Package (WorkPackage) | Composition (c), Flow (f), Aggregation (g), Association (o), Specialization (s), Triggering (t) |

