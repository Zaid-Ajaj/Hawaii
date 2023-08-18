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

            match int status with
            | 200 -> return GetTime.OK(Serializer.deserialize content)
            | 400 -> return GetTime.BadRequest(Serializer.deserialize content)
            | _ -> return GetTime.Forbidden(Serializer.deserialize content)
        }
