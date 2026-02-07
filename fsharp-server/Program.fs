open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open Giraffe
open EAArchive

let webApp (registry: ElementRegistry) (loggerFactory: ILoggerFactory) : HttpHandler =
    Handlers.createHandlers registry loggerFactory

[<EntryPoint>]
let main args =

    Host
        .CreateDefaultBuilder(args)
        .ConfigureLogging(fun context logging ->
            logging
                .ClearProviders()
                .AddConfiguration(context.Configuration.GetSection("Logging"))
                .AddConsole()
            |> ignore
        )
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .ConfigureServices(fun context services ->
                    services.AddGiraffe() |> ignore

                    services.AddSingleton<ElementRegistry>(fun sp ->
                        let config = sp.GetRequiredService<IConfiguration>()
                        let logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("ElementRegistry")

                        let configuredElementsPath = config.GetValue<string>("EAArchive:ElementsPath")
                        let configuredRelationsPath = config.GetValue<string>("EAArchive:RelationsPath")

                        if String.IsNullOrWhiteSpace configuredElementsPath then
                            failwith "EAArchive:ElementsPath must be set in appsettings.json"
                        if String.IsNullOrWhiteSpace configuredRelationsPath then
                            failwith "EAArchive:RelationsPath must be set in appsettings.json"

                        let contentRoot = context.HostingEnvironment.ContentRootPath
                        let resolvePath (pathValue: string) =
                            if Path.IsPathRooted(pathValue) then
                                Path.GetFullPath(pathValue)
                            else
                                Path.GetFullPath(Path.Combine(contentRoot, pathValue))

                        let elementsPath = resolvePath configuredElementsPath
                        let relationsPath = resolvePath configuredRelationsPath

                        logger.LogInformation("Loading elements from: {elementsPath}", elementsPath)
                        logger.LogInformation("Elements path exists: {pathExists}", Directory.Exists(elementsPath))
                        logger.LogInformation("Relationship rules path: {relationsPath}", relationsPath)
                        logger.LogInformation("Relations file exists: {pathExists}", File.Exists(relationsPath))

                        ElementRegistry.createWithLogger elementsPath relationsPath logger
                    )
                    |> ignore
                )
                .Configure(fun app ->
                    let registry = app.ApplicationServices.GetRequiredService<ElementRegistry>()
                    let loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>()

                    app.UseStaticFiles() |> ignore
                    app.UseGiraffe(webApp registry loggerFactory)
                ) |> ignore
        )
        .Build()
        .Run()
    
    0
