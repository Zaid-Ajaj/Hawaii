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

        if status = HttpStatusCode.OK then
            GetTime.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetTime.BadRequest(Serializer.deserialize content)
        else
            GetTime.Forbidden(Serializer.deserialize content)
