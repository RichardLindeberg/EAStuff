namespace EAArchive

open System
open EAArchive.DiagramGenerators
open Microsoft.Extensions.Logging
open Giraffe

module Routes =

    /// Create route handlers
    let createHandlers (repoService: DocumentRepositoryService) (assets: DiagramAssetConfig) (webConfig: WebUiConfig) (loggerFactory: ILoggerFactory) : HttpHandler =
        let logger = loggerFactory.CreateLogger("Handlers")

        logger.LogInformation("Initializing route handlers")
        logger.LogInformation("Repository contains {documentCount} documents", Map.count repoService.Repository.documents)

        choose [
            route "/" >=> Handlers.indexHandler repoService webConfig logger
            route "/index.html" >=> Handlers.indexHandler repoService webConfig logger
            route "/architecture" >=> Handlers.architectureIndexHandler repoService webConfig logger
            route "/governance" >=> Handlers.governanceIndexHandler repoService webConfig logger
            routef "/governance/%s" (fun slug -> Handlers.governanceDocHandler slug repoService webConfig logger)
            route "/management-system" >=> Handlers.governanceIndexHandler repoService webConfig logger
            routef "/management-system/%s" (fun slug -> Handlers.governanceDocHandler slug repoService webConfig logger)
            route "/elements/types" >=> Handlers.elementTypeOptionsHandler logger
            route "/elements/new" >=> Handlers.elementNewHandler repoService webConfig logger
            route "/elements/new/download" >=> Handlers.elementNewDownloadHandler repoService logger
            route "/elements/relations/types" >=> Handlers.relationTypeOptionsHandler repoService logger
            route "/elements/relations/row" >=> Handlers.relationRowHandler webConfig logger
            routef "/elements/%s/edit" (fun elemId -> Handlers.elementEditHandler elemId repoService webConfig logger)
            routef "/elements/%s/download" (fun elemId -> Handlers.elementDownloadHandler elemId repoService logger)
            routef "/elements/%s" (fun elemId -> Handlers.elementHandler elemId repoService webConfig logger)

            // Diagram routes
            routef "/diagrams/type/%s/%s" (fun (layer, typeValue) -> Handlers.elementTypeDiagramCytoscapeHandler layer typeValue repoService assets webConfig logger)
            routef "/diagrams/context/%s" (fun elemId -> Handlers.contextDiagramCytoscapeHandler elemId repoService assets webConfig logger)
            routef "/diagrams/governance/%s" (fun slug -> Handlers.governanceDiagramCytoscapeHandler slug repoService assets webConfig logger)

            // Validation page and API endpoints
            route "/validation" >=> Handlers.validationPageHandler repoService webConfig logger
            route "/api/validation/errors" >=> Handlers.validationErrorsHandler repoService logger
            routef "/api/validation/file/%s" (fun filePath -> Handlers.fileValidationErrorsHandler filePath repoService logger)
            route "/api/validation/stats" >=> Handlers.validationStatsHandler repoService logger
            routef "/api/validation/revalidate/%s" (fun filePath -> Handlers.revalidateFileHandler filePath repoService logger)

            route "/tags" >=> Handlers.tagsIndexHandler repoService webConfig logger
            routef "/tags/%s" (fun tag -> Handlers.tagHandler (Uri.UnescapeDataString tag) repoService webConfig logger)
            routef "/elements/type/%s/%s" (fun (layer, typeValue) -> Handlers.elementTypeHandler layer typeValue repoService webConfig logger)
        ]
