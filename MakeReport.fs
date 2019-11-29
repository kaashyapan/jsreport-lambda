namespace ReportMaker

open jsreport.Binary.Linux
open jsreport.Local
open jsreport.Types
open System
open System.IO

module Report =
    // makeFromTemplate
    let makeFromTemplate (argv: DataStruct) =
        let dfunc (cfg: Configuration) =
            cfg.AllowLocalFilesAccess <- true
            cfg.FileSystemStore() |> ignore
            cfg.BaseUrlAsWorkingDirectory() |> ignore
            cfg

        let rs =
            LocalReporting().RunInDirectory(Path.Combine(Directory.GetCurrentDirectory(), "jsreport"))
                .KillRunningJsReportProcesses().UseBinary(JsReportBinary.GetBinary()).Configure(fun cfg -> dfunc cfg)
                .AsUtility().Create()

        let renderStored =
            async {
                let! bytes = rs.RenderByNameAsync("Invoice", argv) |> Async.AwaitTask
                let! x = bytes.Content.CopyToAsync(File.OpenWrite("invoice.pdf")) |> Async.AwaitTask
                printfn "%A" "executed stored templates jsreports"
                let y = Path.Combine(Directory.GetCurrentDirectory(), "customReport.pdf")
                return y
            }

        let newFilePath = renderStored |> Async.RunSynchronously
        newFilePath // return an integer exit code

    // makeCustom
    let makeCustom (argv: DataMsg) =
        let rs =
            LocalReporting().RunInDirectory(Path.Combine(Directory.GetCurrentDirectory(), "jsreport"))
                .KillRunningJsReportProcesses().UseBinary(JsReportBinary.GetBinary()).AsUtility().Create()

        let template = Template()
        template.Recipe <- Some Recipe.ChromePdf |> Option.toNullable
        template.Engine <- Some Engine.Handlebars |> Option.toNullable
        template.Content <- "<table><tr><td>Hello {{message}}</td></td>"

        let request = RenderRequest()
        request.Template <- template
        request.Data <- argv

        let renderCustom =
            async {
                let! bytes = rs.RenderAsync(request) |> Async.AwaitTask
                let! x = bytes.Content.CopyToAsync(File.OpenWrite("customReport.pdf")) |> Async.AwaitTask
                printfn "%A" "executed jsreports"
                let y = Path.Combine(Directory.GetCurrentDirectory(), "customReport.pdf")
                return y
            }

        let newFilePath = renderCustom |> Async.RunSynchronously
        newFilePath // return an integer exit code
