namespace EAArchive.Views

open EAArchive
open EAArchive.ViewHelpers
open Giraffe.ViewEngine
open Common

module Index =

    /// Index page
    let indexPage (webConfig: WebUiConfig) (currentPage: string) =
        let _ = webConfig.BaseUrl

        let content = [
            div [_class "container"] [
                h2 [_class "layer-title"] [encodedText "Architecture Repository"]
                p [_class "layer-description"] [
                    encodedText "This repository brings governance, architecture, and validation together in one place. Use the navigation to explore governance documents, browse ArchiMate layers, and review validation status."
                ]
            ]
        ]

        htmlPage webConfig "Home" currentPage content
