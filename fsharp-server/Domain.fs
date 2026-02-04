namespace EAArchive

open System

/// Element relationship information
type Relationship = {
    target: string
    relationType: string
    description: string
}

/// ArchiMate element core data
type Element = {
    id: string
    name: string
    elementType: string
    layer: string
    content: string
    properties: Map<string, obj>
    tags: string list
    relationships: Relationship list
}

/// Element with computed relationship metadata
type ElementWithRelations = {
    element: Element
    incomingRelations: (Element * Relationship) list
    outgoingRelations: (Element * Relationship) list
}

/// Layer configuration and display information
type LayerInfo = {
    key: string
    displayName: string
    order: int
}

/// Application constants
module Config =
    let layerOrder = [
        { key = "strategy"; displayName = "Strategy Layer"; order = 0 }
        { key = "motivation"; displayName = "Motivation Layer"; order = 1 }
        { key = "business"; displayName = "Business Layer"; order = 2 }
        { key = "application"; displayName = "Application Layer"; order = 3 }
        { key = "technology"; displayName = "Technology Layer"; order = 4 }
        { key = "physical"; displayName = "Physical Layer"; order = 5 }
        { key = "implementation"; displayName = "Implementation & Migration Layer"; order = 6 }
    ]
    
    let elementsPath = "../elements"
    let baseUrl = "/"
