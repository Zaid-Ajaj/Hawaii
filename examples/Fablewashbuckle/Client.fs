namespace rec Fablewashbuckle

open Browser.Types
open Fable.SimpleHttp
open Fablewashbuckle.Types
open Fablewashbuckle.Http

type FablewashbuckleClient(url: string, headers: list<Header>) =
    new(url: string) = FablewashbuckleClient(url, [])

    member this.GetTime() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/Time" headers requestParts

            match int status with
            | 200 -> return GetTime.OK(Serializer.deserialize content)
            | 400 -> return GetTime.BadRequest(Serializer.deserialize content)
            | _ -> return GetTime.Forbidden(Serializer.deserialize content)
        }
