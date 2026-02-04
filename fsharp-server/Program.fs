open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Giraffe
open EAArchive

let webApp (registry: ElementRegistry) : HttpHandler =
    Handlers.createHandlers registry

[<EntryPoint>]
let main args =
    let elementsPath = 
        if args.Length > 0 then args.[0]
        else Path.Combine(AppContext.BaseDirectory, Config.elementsPath)
    
    printfn "Loading elements from: %s" elementsPath
    let registry = ElementRegistry.create elementsPath
    printfn "Loaded %d elements" (Map.count registry.elements)
    
    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(fun app ->
                    app.UseGiraffe(webApp registry)
                ) |> ignore
        )
        .Build()
        .Run()
    
    0
