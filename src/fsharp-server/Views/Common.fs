namespace EAArchive.Views

open EAArchive
open Giraffe.ViewEngine

module Common =

    /// HTML header with navigation
    let htmlHeader (webConfig: WebUiConfig) (title: string) (currentPage: string) =
        let baseUrl = webConfig.BaseUrl
        let governanceActive = if currentPage = "governance" || currentPage = "index" then "active" else ""
        let architectureActive = if currentPage = "architecture" then "active" else ""
        let validationActive = if currentPage = "validation" then "active" else ""

        let menuRow =
            div [_class "menu-row"] [
                a [_href $"{baseUrl}governance"; _class $"menu-link {governanceActive}"] [
                    encodedText "Governance"
                ]
                a [_href $"{baseUrl}architecture"; _class $"menu-link {architectureActive}"] [
                    encodedText "Architecture"
                ]
                a [_href $"{baseUrl}validation"; _class $"menu-link {validationActive}"] [
                    encodedText "Validation"
                ]
            ]

        let submenuRow =
            let submenuItems =
                if currentPage = "architecture" then
                    let overviewLink =
                        a [_href $"{baseUrl}architecture"; _class "submenu-link active"] [encodedText "Architecture Overview"]

                    let layerLinks =
                        Config.layerOrder
                        |> Map.toList
                        |> List.sortBy (fun (_, info) -> info.order)
                        |> List.map (fun (layerKey, info) ->
                            a [_href $"{baseUrl}architecture/layer/{layerKey}"; _class "submenu-link"] [
                                encodedText info.displayName
                            ]
                        )

                    overviewLink :: layerLinks
                elif currentPage = "governance" || currentPage = "index" then
                    [ a [_href $"{baseUrl}governance"; _class "submenu-link active"] [encodedText "Governance Overview"] ]
                elif currentPage = "validation" then
                    [ a [_href $"{baseUrl}validation"; _class "submenu-link active"] [encodedText "Validation Report"] ]
                else
                    []

            if List.isEmpty submenuItems then
                emptyText
            else
                div [_class "submenu-row"] submenuItems

        header [_class "header"] [
            div [_class "header-content"] [
                h1 [] [encodedText "ArchiMate Architecture Repository"]
                nav [] [
                    menuRow
                    submenuRow
                ]
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
