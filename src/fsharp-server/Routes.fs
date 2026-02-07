namespace EAArchive

open System
open EAArchive.DiagramGenerators
open Microsoft.Extensions.Logging
open Giraffe

module Routes =

    /// Create route handlers
    let createHandlers (registry: ElementRegistry) (governanceRegistry: GovernanceDocRegistry) (assets: DiagramAssetConfig) (webConfig: WebUiConfig) (loggerFactory: ILoggerFactory) : HttpHandler =
        let logger = loggerFactory.CreateLogger("Handlers")

        logger.LogInformation("Initializing route handlers")
        logger.LogInformation("Registry contains {elementCount} elements", Map.count registry.elements)

        choose [
            route "/" >=> Handlers.indexHandler registry governanceRegistry webConfig logger
            route "/index.html" >=> Handlers.indexHandler registry governanceRegistry webConfig logger
            route "/architecture" >=> Handlers.architectureIndexHandler registry governanceRegistry webConfig logger
            route "/governance" >=> Handlers.governanceIndexHandler governanceRegistry webConfig logger
            routef "/governance/%s" (fun slug -> Handlers.governanceDocHandler slug governanceRegistry registry webConfig logger)
            route "/management-system" >=> Handlers.governanceIndexHandler governanceRegistry webConfig logger
            routef "/management-system/%s" (fun slug -> Handlers.governanceDocHandler slug governanceRegistry registry webConfig logger)
            route "/elements/types" >=> Handlers.elementTypeOptionsHandler logger
            route "/elements/new" >=> Handlers.elementNewHandler registry webConfig logger
            route "/elements/new/download" >=> Handlers.elementNewDownloadHandler registry logger
            route "/elements/relations/types" >=> Handlers.relationTypeOptionsHandler registry logger
            route "/elements/relations/row" >=> Handlers.relationRowHandler webConfig logger
            routef "/elements/%s/edit" (fun elemId -> Handlers.elementEditHandler elemId registry webConfig logger)
            routef "/elements/%s/download" (fun elemId -> Handlers.elementDownloadHandler elemId registry logger)
            routef "/elements/%s" (fun elemId -> Handlers.elementHandler elemId registry webConfig logger)

            // Diagram routes
            routef "/diagrams/layer/%s" (fun layer -> Handlers.layerDiagramCytoscapeHandler layer registry assets webConfig logger)
            routef "/diagrams/context/%s" (fun elemId -> Handlers.contextDiagramCytoscapeHandler elemId registry assets webConfig logger)

            // Validation page and API endpoints
            route "/validation" >=> Handlers.validationPageHandler registry governanceRegistry webConfig logger
            route "/api/validation/errors" >=> Handlers.validationErrorsHandler registry governanceRegistry logger
            routef "/api/validation/file/%s" (fun filePath -> Handlers.fileValidationErrorsHandler filePath registry governanceRegistry logger)
            route "/api/validation/stats" >=> Handlers.validationStatsHandler registry governanceRegistry logger
            routef "/api/validation/revalidate/%s" (fun filePath -> Handlers.revalidateFileHandler filePath registry logger)

            route "/tags" >=> Handlers.tagsIndexHandler registry webConfig logger
            routef "/tags/%s" (fun tag -> Handlers.tagHandler (Uri.UnescapeDataString tag) registry webConfig logger)
            routef "/%s" (fun layer -> Handlers.layerHandler layer registry webConfig logger)
        ]
