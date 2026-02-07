namespace EAArchive.Views

open EAArchive
open Giraffe.ViewEngine

module Common =

    /// HTML header with navigation
    let htmlHeader (webConfig: WebUiConfig) (title: string) (currentPage: string) =
        let baseUrl = webConfig.BaseUrl
        let navItems =
            Config.layerOrder
            |> Map.toList
            |> List.sortBy (fun (_, layerInfo) -> layerInfo.order)
            |> List.map (fun (layerKey, layerInfo) ->
                let layerKeyLower = Layer.toKey layerKey
                let isActive = currentPage = layerKeyLower
                let activeClass = if isActive then "active" else ""
                a [_href $"{baseUrl}{layerKeyLower}"; _class $"nav-link {activeClass}"] [
                    encodedText layerInfo.displayName
                ]
            )

        let governanceActive = if currentPage = "governance" then "active" else ""
        let tagsActive = if currentPage = "tags" then "active" else ""
        let validationActive = if currentPage = "validation" then "active" else ""
        let navLinks =
            navItems
            @ [
                a [_href $"{baseUrl}governance"; _class $"nav-link {governanceActive}"] [
                    encodedText "Governance"
                ]
                a [_href $"{baseUrl}tags"; _class $"nav-link {tagsActive}"] [
                    encodedText "Tags"
                ]
                a [_href $"{baseUrl}validation"; _class $"nav-link {validationActive}"] [
                    encodedText "Validation"
                ]
            ]

        header [_class "header"] [
            div [_class "header-content"] [
                h1 [] [encodedText "ArchiMate Architecture Repository"]
                nav [] navLinks
            ]
        ]

    /// HTML footer
    let htmlFooter () =
        footer [_class "footer"] [
            p [] [encodedText "Generated from ArchiMate elements | (c) 2026"]
        ]

    /// Full HTML page template
    let htmlPage (webConfig: WebUiConfig) (pageTitle: string) (currentPage: string) (content: XmlNode list) =
        html [_lang "en"] [
            head [] [
                meta [_charset "UTF-8"]
                meta [_name "viewport"; _content "width=device-width, initial-scale=1.0"]
                title [] [encodedText $"{pageTitle} - ArchiMate Architecture"]
                link [_rel "stylesheet"; _href webConfig.SiteCssUrl]
                link [_rel "stylesheet"; _href webConfig.DiagramCssUrl]
                script [_src webConfig.HtmxScriptUrl] []
                script [_src webConfig.ValidationScriptUrl] []
                script [_src webConfig.DiagramScriptUrl] []
#if DEBUG
                script [_src webConfig.HtmxDebugScriptUrl] []
#endif
            ]
            body [] ([htmlHeader webConfig pageTitle currentPage] @ content @ [htmlFooter ()])
        ]
