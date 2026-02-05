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
    let elementsPathArg = if args.Length > 0 then Some args.[0] else None

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

                        let configuredPath = config.GetValue<string>("EAArchive:ElementsPath")
                        let defaultPath = AppContext.BaseDirectory
                        let elementsPath =
                            match elementsPathArg with
                            | Some argPath when not (String.IsNullOrWhiteSpace argPath) -> argPath
                            | _ when not (String.IsNullOrWhiteSpace configuredPath) -> configuredPath
                            | _ -> defaultPath

                        logger.LogInformation("Loading elements from: {elementsPath}", elementsPath)
                        logger.LogInformation("Elements path exists: {pathExists}", Directory.Exists(elementsPath))

                        ElementRegistry.createWithLogger elementsPath logger
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
