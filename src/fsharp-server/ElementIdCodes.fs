namespace EAArchive

module ElementIdCodes =

    let layerNameToCode =
        Map.ofList [
            ("strategy", "str")
            ("business", "bus")
            ("application", "app")
            ("technology", "tec")
            ("physical", "phy")
            ("motivation", "mot")
            ("implementation", "imp")
        ]

    let layerCodeToName =
        Map.ofList [
            ("str", "strategy")
            ("bus", "business")
            ("app", "application")
            ("tec", "technology")
            ("phy", "physical")
            ("mot", "motivation")
            ("imp", "implementation")
        ]

    let typeNameToCode =
        Map.ofList [
            ("resource", "rsrc")
            ("capability", "capa")
            ("value-stream", "vstr")
            ("course-of-action", "cact")
            ("business-actor", "actr")
            ("business-role", "role")
            ("business-collaboration", "colab")
            ("business-interface", "intf")
            ("business-process", "proc")
            ("business-function", "func")
            ("business-interaction", "intr")
            ("business-event", "evnt")
            ("business-service", "srvc")
            ("business-object", "objt")
            ("contract", "cntr")
            ("representation", "repr")
            ("product", "prod")
            ("application-component", "comp")
            ("application-collaboration", "colab")
            ("application-interface", "intf")
            ("application-function", "func")
            ("application-interaction", "intr")
            ("application-process", "proc")
            ("application-event", "evnt")
            ("application-service", "srvc")
            ("data-object", "data")
            ("node", "node")
            ("device", "devc")
            ("system-software", "sysw")
            ("technology-collaboration", "colab")
            ("technology-interface", "intf")
            ("path", "path")
            ("communication-network", "netw")
            ("technology-function", "func")
            ("technology-process", "proc")
            ("technology-interaction", "intr")
            ("technology-event", "evnt")
            ("technology-service", "srvc")
            ("artifact", "artf")
            ("equipment", "equi")
            ("facility", "faci")
            ("distribution-network", "dist")
            ("material", "matr")
            ("stakeholder", "stkh")
            ("driver", "drvr")
            ("assessment", "asmt")
            ("goal", "goal")
            ("outcome", "outc")
            ("principle", "prin")
            ("requirement", "reqt")
            ("constraint", "cnst")
            ("meaning", "mean")
            ("value", "valu")
            ("work-package", "work")
            ("deliverable", "delv")
            ("implementation-event", "evnt")
            ("plateau", "plat")
            ("gap", "gap")
        ]

    let typeCodesByLayerCode =
        Map.ofList [
            ("str", [ "rsrc"; "capa"; "vstr"; "cact" ])
            ("bus", [ "actr"; "role"; "colab"; "intf"; "proc"; "func"; "intr"; "evnt"; "srvc"; "objt"; "cntr"; "repr"; "prod" ])
            ("app", [ "comp"; "colab"; "intf"; "func"; "intr"; "proc"; "evnt"; "srvc"; "data" ])
            ("tec", [ "node"; "devc"; "sysw"; "colab"; "intf"; "path"; "netw"; "func"; "proc"; "intr"; "evnt"; "srvc"; "artf" ])
            ("phy", [ "equi"; "faci"; "dist"; "matr" ])
            ("mot", [ "stkh"; "drvr"; "asmt"; "goal"; "outc"; "prin"; "reqt"; "cnst"; "mean"; "valu" ])
            ("imp", [ "work"; "delv"; "evnt"; "plat"; "gap" ])
        ]
