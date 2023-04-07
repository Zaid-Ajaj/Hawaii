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

            if status = 200 then
                return GetTime.OK(Serializer.deserialize content)
            else if status = 400 then
                return GetTime.BadRequest(Serializer.deserialize content)
            else
                return GetTime.Forbidden(Serializer.deserialize content)
        }
