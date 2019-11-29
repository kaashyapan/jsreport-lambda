namespace Lambada

open Amazon.Lambda.Core
open Amazon.Lambda.APIGatewayEvents


[<assembly:LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
do ()

open System.IO
open System.Text
open System.Net
open System.Collections.Generic
open Newtonsoft.Json

module Handler =
    type RecordType =
        { success: bool }

    let accept (request: APIGatewayProxyRequest, context: ILambdaContext): APIGatewayProxyResponse =
        printfn "Body was %A" request.Body
        printfn "Headers was %A" request.Headers
        ReportMaker.maker |> ignore
        let response = APIGatewayProxyResponse()
        let headers = Dictionary<string, string>()
        headers.Add("Content-Type", "application/json")
        headers.Add("Access-Control-Allow-Origin", "*")
        response.Headers <- headers
        let data: RecordType = { success = true }
        let body = JsonConvert.SerializeObject(data)
        response.Body <- body
        response.StatusCode <- (int) HttpStatusCode.OK
        response
