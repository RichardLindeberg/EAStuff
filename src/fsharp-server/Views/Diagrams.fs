namespace EAArchive.Views

open EAArchive
open Giraffe.ViewEngine

module Diagrams =

    let cytoscapeDiagramPage (webConfig: WebUiConfig) (titleText: string) (dataJson: string) : XmlNode =
        html [_lang "en"] [
            head [] [
                meta [_charset "UTF-8"]
                meta [_name "viewport"; _content "width=device-width, initial-scale=1.0"]
                title [] [encodedText titleText]
                link [_rel "stylesheet"; _href webConfig.DiagramCssUrl]
                script [_src webConfig.CytoscapeScriptUrl] []
                script [_src webConfig.DagreScriptUrl] []
                script [_src webConfig.CytoscapeDagreScriptUrl] []
                script [_src webConfig.LodashScriptUrl] []
            ]
            body [] [
                div [_id "cy"] []
                div [_id "cy-context-menu"; _class "context-menu"; _style "display: none;"] [
                    button [_type "button"; _data "action" "expand"] [encodedText "Add linked elements"]
                    button [_type "button"; _data "action" "remove"] [encodedText "Remove element"]
                ]
                div [_class "controls"] [
                    button [_id "fitView"] [encodedText "Fit to View"]
                    button [_id "zoomIn"] [encodedText "Zoom In"]
                    button [_id "zoomOut"] [encodedText "Zoom Out"]
                    button [_id "resetLayout"] [encodedText "Reset Layout"]
                    button [_id "exportPNG"] [encodedText "Export PNG"]
                ]
                div [_class "legend"] [
                    h4 [] [encodedText "Layers"]
                    label [_class "legend-toggle"] [
                        input [_type "checkbox"; _id "toggle-architecture"; _checked]
                        span [_class "legend-swatch architecture"] []
                        span [] [encodedText "Architecture"]
                    ]
                    label [_class "legend-toggle"] [
                        input [_type "checkbox"; _id "toggle-governance"; _checked]
                        span [_class "legend-swatch governance"] []
                        span [] [encodedText "Governance"]
                    ]
                    h4 [] [encodedText "Relationships"]
                    div [_class "legend-item"] [
                        div [_class "legend-line"; _style "background: #0066cc;"] []
                        span [] [encodedText "Serving"]
                    ]
                    div [_class "legend-item"] [
                        div [_class "legend-line"; _style "background: #cc6600;"] []
                        span [] [encodedText "Realization"]
                    ]
                    div [_class "legend-item"] [
                        div [_class "legend-line"; _style "background: #333333;"] []
                        span [] [encodedText "Composition"]
                    ]
                    div [_class "legend-item"] [
                        div [_class "legend-line"; _style "background: #cc3366; border-top: 2px dashed;"] []
                        span [] [encodedText "Influence"]
                    ]
                    div [_class "legend-item"] [
                        div [_class "legend-line"; _style "background: #5a67d8; border-top: 2px dashed;"] []
                        span [] [encodedText "Governance link"]
                    ]
                ]
                script [] [rawText (sprintf "const graphData = %s;" dataJson)]
                script [_src webConfig.DiagramScriptUrl] []
            ]
        ]
