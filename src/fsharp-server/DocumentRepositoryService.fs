namespace EAArchive

open System.IO
open Microsoft.Extensions.Logging

/// Service wrapper for the unified document repository and relationship rules.
type DocumentRepositoryService
    (
        elementsPath: string,
        managementSystemPath: string,
        glossaryPath: string,
        relationsPath: string,
        loggerFactory: ILoggerFactory
    ) =

    let logger = loggerFactory.CreateLogger("DocumentRepository")

    let relationshipRules =
        if File.Exists(relationsPath) then
            match ElementType.parseRelationshipRulesWithLogger relationsPath logger with
            | Ok rules ->
                logger.LogInformation("Loaded {ruleCount} relationship rules from {rulesPath}", Map.count rules, relationsPath)
                rules
            | Error errorMessage ->
                logger.LogWarning("Failed to parse relationship rules from {rulesPath}: {error}", relationsPath, errorMessage)
                Map.empty
        else
            logger.LogWarning("Relationship rules file not found: {rulesPath}", relationsPath)
            Map.empty

    let mutable repository = DocumentRepositoryLoader.loadRepository elementsPath managementSystemPath glossaryPath logger

    member _.Repository = repository

    member _.RelationshipRules = relationshipRules

    member _.ElementsPath = elementsPath

    member _.ManagementSystemPath = managementSystemPath

    member _.GlossaryPath = glossaryPath

    member _.BasePaths = [ elementsPath; managementSystemPath; glossaryPath ]

    member _.Reload() : unit =
        repository <- DocumentRepositoryLoader.loadRepository elementsPath managementSystemPath glossaryPath logger
