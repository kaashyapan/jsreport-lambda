namespace ReportMaker

module Stat =
    let getArgs =
        let dataStr =
            { number = "123"
              seller =
                  { name = "Next Step Webs, Inc."
                    road = "12345 Sunny Road"
                    country = "Sunnyville, TX 12345" }
              buyer =
                  { name = "Acme Corp."
                    road = "16 Johnson Road"
                    country = "Paris, France 8060" }
              items =
                  [ { name = "Website design"
                      price = 300 } ] }

        dataStr

module ReportMaker =
    [<EntryPoint>]
    let main argv =
        let fpath = Report.makeFromTemplate Stat.getArgs
        printfn "%A" fpath
        0 // return an integer exit code
