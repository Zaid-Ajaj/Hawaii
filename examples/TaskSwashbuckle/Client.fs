namespace rec TaskSwashbuckle

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open TaskSwashbuckle.Types
open TaskSwashbuckle.Http
open FSharp.Control.Tasks

type TaskSwashbuckleClient(httpClient: HttpClient) =
    member this.GetTime(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/Time" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return GetTime.OK(Serializer.deserialize content)
            | HttpStatusCode.BadRequest -> return GetTime.BadRequest(Serializer.deserialize content)
            | _ -> return GetTime.Forbidden(Serializer.deserialize content)
        }
