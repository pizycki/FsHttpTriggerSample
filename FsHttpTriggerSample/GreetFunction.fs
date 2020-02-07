namespace FsHttpTriggerSample

module GreetFunction =

    open Microsoft.AspNetCore.Mvc
    open Microsoft.Azure.WebJobs
    open Microsoft.AspNetCore.Http
    open Newtonsoft.Json
    open System.IO
    open Microsoft.Extensions.Logging
    open FSharp.Control.Tasks

    type User =
        { Name: string }

    [<FunctionName("Greet")>]
    let Run ([<HttpTrigger(Methods = [| "POST" |])>] req: HttpRequest)
            (log: ILogger) =
        task {
            log.LogInformation "Running Function"

            let! body = new StreamReader(req.Body)
                        |> (fun stream -> stream.ReadToEndAsync())

            let user = JsonConvert.DeserializeObject<User>(body)

            return OkObjectResult(sprintf "Hello %s" user.Name)
        }
