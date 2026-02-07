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
open EAArchive.DiagramGenerators

let webApp (registry: ElementRegistry) (diagramAssets: DiagramAssetConfig) (webConfig: WebUiConfig) (loggerFactory: ILoggerFactory) : HttpHandler =
    Handlers.createHandlers registry diagramAssets webConfig loggerFactory

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

                    let contentRoot = context.HostingEnvironment.ContentRootPath
                    let resolvePath (pathValue: string) =
                        if Path.IsPathRooted(pathValue) then
                            Path.GetFullPath(pathValue)
                        else
                            Path.GetFullPath(Path.Combine(contentRoot, pathValue))

                    let normalizeBaseUrl (value: string) =
                        if String.IsNullOrWhiteSpace value then value
                        elif value.EndsWith("/") then value
                        else value + "/"

                    let getRequired (config: IConfiguration) (key: string) : string =
                        let value = config.GetValue<string>(key)
                        if String.IsNullOrWhiteSpace value then
                            failwith (sprintf "%s must be set in appsettings.json" key)
                        value

                    services.AddSingleton<WebUiConfig>(fun sp ->
                        let config = sp.GetRequiredService<IConfiguration>()

                        let baseUrl =
                            getRequired config "EAArchive:Web:BaseUrl"
                            |> normalizeBaseUrl

                        { BaseUrl = baseUrl
                          SiteCssUrl = getRequired config "EAArchive:Web:SiteCssUrl"
                          DiagramCssUrl = getRequired config "EAArchive:Web:DiagramCssUrl"
                          ValidationScriptUrl = getRequired config "EAArchive:Web:ValidationScriptUrl"
                          DiagramScriptUrl = getRequired config "EAArchive:Web:DiagramScriptUrl"
                          HtmxScriptUrl = getRequired config "EAArchive:Web:HtmxScriptUrl"
                          HtmxDebugScriptUrl = getRequired config "EAArchive:Web:HtmxDebugScriptUrl"
                          CytoscapeScriptUrl = getRequired config "EAArchive:Web:CytoscapeScriptUrl"
                          DagreScriptUrl = getRequired config "EAArchive:Web:DagreScriptUrl"
                          CytoscapeDagreScriptUrl = getRequired config "EAArchive:Web:CytoscapeDagreScriptUrl"
                          LodashScriptUrl = getRequired config "EAArchive:Web:LodashScriptUrl" }
                    )
                    |> ignore

                    services.AddSingleton<DiagramAssetConfig>(fun sp ->
                        let config = sp.GetRequiredService<IConfiguration>()

                        let configuredSymbolsPath = getRequired config "EAArchive:Assets:SymbolsPath"
                        let configuredIconsPath = getRequired config "EAArchive:Assets:IconsPath"
                        let configuredSymbolsBaseUrl = getRequired config "EAArchive:Assets:SymbolsBaseUrl"
                        let configuredIconsBaseUrl = getRequired config "EAArchive:Assets:IconsBaseUrl"

                        { SymbolsPath = resolvePath configuredSymbolsPath
                          IconsPath = resolvePath configuredIconsPath
                          SymbolsBaseUrl = normalizeBaseUrl configuredSymbolsBaseUrl
                          IconsBaseUrl = normalizeBaseUrl configuredIconsBaseUrl }
                    )
                    |> ignore

                    services.AddSingleton<ElementRegistry>(fun sp ->
                        let config = sp.GetRequiredService<IConfiguration>()
                        let logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("ElementRegistry")

                        let configuredElementsPath = getRequired config "EAArchive:ElementsPath"
                        let configuredRelationsPath = getRequired config "EAArchive:RelationsPath"

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
                    let diagramAssets = app.ApplicationServices.GetRequiredService<DiagramAssetConfig>()
                    let webConfig = app.ApplicationServices.GetRequiredService<WebUiConfig>()
                    let loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>()

                    app.UseStaticFiles() |> ignore
                    app.UseGiraffe(webApp registry diagramAssets webConfig loggerFactory)
                ) |> ignore
        )
        .Build()
        .Run()
    
    0
