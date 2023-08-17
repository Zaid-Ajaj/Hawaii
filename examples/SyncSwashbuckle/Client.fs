namespace rec SyncSwashbuckle

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open SyncSwashbuckle.Types
open SyncSwashbuckle.Http

type SyncSwashbuckleClient(httpClient: HttpClient) =
    member this.GetTime(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/Time" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetTime.OK(Serializer.deserialize content)
        | HttpStatusCode.BadRequest -> GetTime.BadRequest(Serializer.deserialize content)
        | _ -> GetTime.Forbidden(Serializer.deserialize content)
