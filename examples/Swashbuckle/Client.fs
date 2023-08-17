namespace rec Swashbuckle

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open Swashbuckle.Types
open Swashbuckle.Http

type SwashbuckleClient(httpClient: HttpClient) =
    member this.GetTime(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/Time" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return GetTime.OK(Serializer.deserialize content)
            | HttpStatusCode.BadRequest -> return GetTime.BadRequest(Serializer.deserialize content)
            | _ -> return GetTime.Forbidden(Serializer.deserialize content)
        }
