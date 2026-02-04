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
    
    printfn "Starting EA Archive Server..."
    printfn "Loading elements from: %s" elementsPath
    printfn "Elements path exists: %b" (Directory.Exists elementsPath)
    
    let registry = ElementRegistry.create elementsPath
    printfn "Successfully loaded %d elements" (Map.count registry.elements)
    printfn "Elements by layer:"
    registry.elementsByLayer
    |> Map.iter (fun layer ids ->
        printfn "  %s: %d elements" layer (List.length ids)
    )
    
    Host
        .CreateDefaultBuilder(args)
        .ConfigureLogging(fun logging ->
            logging
                .ClearProviders()
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information) |> ignore
        )
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(fun app ->
                    let loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>()
                    app.UseStaticFiles() |> ignore
                    app.UseGiraffe(webApp registry loggerFactory)
                ) |> ignore
        )
        .Build()
        .Run()
    
    0
