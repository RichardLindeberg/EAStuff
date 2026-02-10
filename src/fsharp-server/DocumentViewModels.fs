namespace EAArchive

/// Summary card for architecture elements.
type ArchimateCard = {
    id: string
    name: string
    elementTypeLabel: string
    description: string
    incomingCount: int
    outgoingCount: int
}

/// Relationship view model for architecture elements.
type ArchimateRelationView = {
    relatedId: string
    relatedName: string
    relationType: RelationType
    description: string
}

/// Governance relation view model for linking to governance docs.
type GovernanceRelationView = {
    docId: string
    slug: string
    title: string
    docType: GovernanceDocType
    relationType: RelationType
}

/// Detail view model for an ArchiMate element.
type ArchimateDetailView = {
    id: string
    name: string
    elementType: ElementType
    content: string
    tags: string list
    properties: (string * string) list
    incomingRelations: ArchimateRelationView list
    outgoingRelations: ArchimateRelationView list
    governanceOwners: GovernanceRelationView list
    governanceIncoming: GovernanceRelationView list
    governanceOutgoing: GovernanceRelationView list
}

/// Edit form model for an ArchiMate element.
type ArchimateEditView = {
    id: string
    name: string
    typeValue: string
    layerValue: string
    tags: string list
    properties: Map<string, string>
    relationships: Relationship list
    content: string
}

/// Summary card for governance documents.
type GovernanceCardView = {
    slug: string
    title: string
    docType: GovernanceDocType
    ownerLabel: string option
    ownerId: string option
}

/// Detail view model for governance documents.
type GovernanceDetailView = {
    slug: string
    title: string
    docType: GovernanceDocType
    metadataItems: (string * string) list
    governanceRelations: GovernanceRelationView list
    archimateRelations: ArchimateRelationView list
    archimateIncomingRelations: ArchimateRelationView list
    content: string
}
