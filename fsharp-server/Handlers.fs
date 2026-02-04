namespace EAArchive

open System
open System.Collections.Generic
open Giraffe
open Giraffe.ViewEngine

module Handlers =
    
    /// Build tag index from registry
    let buildTagIndex (registry: ElementRegistry) : Map<string, string list> =
        registry.elements
        |> Map.fold (fun acc elemId elem ->
            elem.tags
            |> List.fold (fun tagMap tag ->
                match Map.tryFind tag tagMap with
                | Some ids -> Map.add tag (elemId :: ids) tagMap
                | None -> Map.add tag [elemId] tagMap
            ) acc
        ) Map.empty
    
    /// Index/home page handler
    let indexHandler (registry: ElementRegistry) : HttpHandler =
        fun next ctx ->
            let html = Views.indexPage registry
            htmlView html next ctx
    
    /// Layer page handler
    let layerHandler (layer: string) (registry: ElementRegistry) : HttpHandler =
        fun next ctx ->
            match Config.layerOrder |> List.tryFind (fun l -> l.key = layer) with
            | Some layerInfo ->
                let elements = ElementRegistry.getLayerElements layer registry
                let html = Views.layerPage layerInfo elements registry
                htmlView html next ctx
            | None -> 
                setStatusCode 404 >=> text "Layer not found" |> fun handler -> handler next ctx
    
    /// Element detail page handler
    let elementHandler (elemId: string) (registry: ElementRegistry) : HttpHandler =
        fun next ctx ->
            match ElementRegistry.getElement elemId registry with
            | Some elem ->
                let elemWithRels = ElementRegistry.withRelations elem registry
                let html = Views.elementPage elemWithRels
                htmlView html next ctx
            | None ->
                setStatusCode 404 >=> text "Element not found" |> fun handler -> handler next ctx
    
    /// Tags index handler
    let tagsIndexHandler (registry: ElementRegistry) : HttpHandler =
        fun next ctx ->
            let tagIndex = buildTagIndex registry
            let html = Views.tagsIndexPage tagIndex registry
            htmlView html next ctx
    
    /// Individual tag page handler
    let tagHandler (tag: string) (registry: ElementRegistry) : HttpHandler =
        fun next ctx ->
            let tagIndex = buildTagIndex registry
            match Map.tryFind tag tagIndex with
            | Some elemIds ->
                let elements =
                    elemIds
                    |> List.choose (fun id -> ElementRegistry.getElement id registry)
                    |> List.sortBy (fun e -> e.name)
                let html = Views.tagPage tag elements
                htmlView html next ctx
            | None ->
                setStatusCode 404 >=> text "Tag not found" |> fun handler -> handler next ctx
    
    /// Create route handlers
    let createHandlers (registry: ElementRegistry) : HttpHandler =
        choose [
            route "/" >=> indexHandler registry
            route "/index.html" >=> indexHandler registry
            routef "/elements/%s.html" (fun elemId -> elementHandler elemId registry)
            route "/tags.html" >=> tagsIndexHandler registry
            routef "/tags/%s.html" (fun tag -> tagHandler (Uri.UnescapeDataString tag) registry)
            routef "/%s.html" (fun layer -> layerHandler layer registry)
        ]
