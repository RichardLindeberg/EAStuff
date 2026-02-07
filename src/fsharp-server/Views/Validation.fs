namespace EAArchive.Views

open System
open System.IO
open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Validation =

    /// Validation errors page
    let validationPage (webConfig: WebUiConfig) (basePaths: string list) (errors: ValidationError list) =
        let toRelativePath (filePath: string) : string =
            let candidates =
                basePaths
                |> List.choose (fun basePath ->
                    try
                        let relative = Path.GetRelativePath(basePath, filePath)
                        if relative.StartsWith("..") then None else Some relative
                    with
                    | _ -> None
                )
            match candidates with
            | head :: _ -> head
            | [] -> filePath
        let errorsByFile =
            errors
            |> List.groupBy (fun e -> e.filePath)
            |> List.sortBy fst

        let fileCards =
            errorsByFile
            |> List.map (fun (filePath, fileErrors) ->
                let errorCount = List.length fileErrors
                let hasErrors = fileErrors |> List.exists (fun e -> e.severity = Severity.Error)
                let errorClass = if hasErrors then "has-errors" else "has-warnings"
                let severityBadges =
                    fileErrors
                    |> List.groupBy (fun e -> e.severity)
                    |> List.map (fun (severity, errs) ->
                        let badgeClass = if severity = Severity.Error then "badge-error" else "badge-warning"
                        let sevStr =
                            match severity with
                            | Severity.Error -> "Error"
                            | Severity.Warning -> "Warning"
                        span [_class $"badge {badgeClass}"] [
                            encodedText (sprintf "%s: %d" sevStr errs.Length)
                        ]
                    )

                let errorDetails =
                    fileErrors
                    |> List.map (fun err ->
                        let elemId = err.elementId |> Option.defaultValue "N/A"
                        let severityStr =
                            match err.severity with
                            | Severity.Error -> "error"
                            | Severity.Warning -> "warning"
                        let severityClass = $"severity-{severityStr}"
                        let errorTypeStr = ElementType.errorTypeToDisplayName err.errorType
                        div [_class $"error-detail {severityClass}"] [
                            div [_class "error-header"] [
                                span [_class "error-type"] [encodedText errorTypeStr]
                                span [_class "element-ref"] [encodedText $"ID: {elemId}"]
                            ]
                            p [_class "error-message"] [encodedText err.message]
                        ]
                    )

                let relativeFilePath = toRelativePath filePath

                div [_class $"file-card {errorClass}"] [
                    div [_class "file-header"] [
                        h3 [] [encodedText relativeFilePath]
                        div [_class "badges"] severityBadges
                    ]
                    div [_class "error-list"] errorDetails
                    button [
                        _type "button"
                        _class "btn-reload"
                        _onclick $"reloadFile('{relativeFilePath}')"
                    ] [
                        encodedText "Reload File"
                    ]
                ]
            )

        let stats =
            let totalFiles = List.length errorsByFile
            let totalErrors = errors |> List.filter (fun e -> e.severity = Severity.Error) |> List.length
            let totalWarnings = errors |> List.filter (fun e -> e.severity = Severity.Warning) |> List.length
            let errorClass = if totalErrors > 0 then "error" else ""
            let warningClass = if totalWarnings > 0 then "warning" else ""

            div [_class "validation-stats"] [
                div [_class "stat-item"] [
                    span [_class "stat-label"] [encodedText "Files with Issues:"]
                    span [_class "stat-value"] [encodedText (string totalFiles)]
                ]
                div [_class "stat-item"] [
                    span [_class "stat-label"] [encodedText "Errors:"]
                    span [_class $"stat-value {errorClass}"] [encodedText (string totalErrors)]
                ]
                div [_class "stat-item"] [
                    span [_class "stat-label"] [encodedText "Warnings:"]
                    span [_class $"stat-value {warningClass}"] [encodedText (string totalWarnings)]
                ]
            ]

        let content = [
            div [_class "container"] [
                h1 [_class "page-title"] [encodedText "Validation Errors Report"]
                stats
                div [_class "file-list"] fileCards
            ]
        ]

        htmlPage webConfig "Validation Report" "validation" content
