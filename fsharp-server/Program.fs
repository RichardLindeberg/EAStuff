open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open EAArchive

let webApp (registry: ElementRegistry) (loggerFactory: ILoggerFactory) : HttpHandler =
    Handlers.createHandlers registry loggerFactory

[<EntryPoint>]
let main args =
    let elementsPath = 
        if args.Length > 0 then 
            args.[0]
        else
            // Elements are copied directly to the application directory with their layer structure preserved
            AppContext.BaseDirectory
    
    let configureLogging (logging: ILoggingBuilder) : unit =
        logging
            .ClearProviders()
            .AddConsole()
            .SetMinimumLevel(LogLevel.Debug)
        |> ignore

    let loggerFactory = LoggerFactory.Create(configureLogging)

    let logger = loggerFactory.CreateLogger("Startup")

    logger.LogInformation("Starting EA Archive Server...")
    logger.LogInformation("Loading elements from: {elementsPath}", elementsPath)
    logger.LogInformation("Elements path exists: {pathExists}", Directory.Exists(elementsPath))
    
    let registry = ElementRegistry.create elementsPath
    logger.LogInformation("Successfully loaded {elementCount} elements", Map.count registry.elements)
    logger.LogInformation("Elements by layer:")
    registry.elementsByLayer
    |> Map.iter (fun layer ids ->
        logger.LogInformation("  {layer}: {count} elements", layer, List.length ids)
    )
    
    Host
        .CreateDefaultBuilder(args)
        .ConfigureLogging(configureLogging)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .ConfigureServices(fun services ->
                    services.AddGiraffe() |> ignore
                )
                .Configure(fun app ->
                    let loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>()
                    app.UseStaticFiles() |> ignore
                    app.UseGiraffe(webApp registry loggerFactory)
                ) |> ignore
        )
        .Build()
        .Run()
    
    0
