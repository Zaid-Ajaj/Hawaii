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

            if status = HttpStatusCode.OK then
                return GetTime.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetTime.BadRequest(Serializer.deserialize content)
            else
                return GetTime.Forbidden(Serializer.deserialize content)
        }
