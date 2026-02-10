namespace EAArchive

open System
open Giraffe
open Microsoft.AspNetCore.Http

module HttpContextHelpers =

    let getQueryStringValueOrEmpty (ctx: HttpContext) (key: string) : string =
        match ctx.GetQueryStringValue key with
        | Ok value -> value
        | Error _ -> ""

    let tryGetQueryStringValue (ctx: HttpContext) (key: string) : string option =
        match ctx.GetQueryStringValue key with
        | Ok value when not (String.IsNullOrWhiteSpace value) -> Some value
        | _ -> None

    let tryGetQueryStringValueLower (ctx: HttpContext) (key: string) : string option =
        tryGetQueryStringValue ctx key
        |> Option.map (fun value -> value.Trim().ToLowerInvariant())

    let tryGetQueryStringValueInt (ctx: HttpContext) (key: string) : int option =
        match ctx.GetQueryStringValue key with
        | Ok value ->
            match Int32.TryParse(value) with
            | true, parsed -> Some parsed
            | _ -> None
        | Error _ -> None
