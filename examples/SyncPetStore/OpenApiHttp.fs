namespace SyncPetStore.Http

open System
open System.Net.Http
open System.Globalization
open System.Collections.Generic
open System.Text
open Fable.Remoting.Json
open System.Threading


module Serializer =
    open Newtonsoft.Json
    let converter = FableJsonConverter() :> JsonConverter
    let settings = JsonSerializerSettings(Converters=[| converter |])
    settings.DateParseHandling <- DateParseHandling.None
    settings.NullValueHandling <- NullValueHandling.Ignore
    let serialize<'t> (value: 't) = JsonConvert.SerializeObject(value, settings)
    let deserialize<'t> (content: string) = JsonConvert.DeserializeObject<'t>(content, settings)

[<RequireQualifiedAccess>]
type OpenApiValue =
    | Int of int
    | Int64 of int64
    | String of string
    | Bool of bool
    | Double of double
    | Float of float32
    | List of OpenApiValue list

type MultiPartFormData =
    | Primitive of OpenApiValue
    | File of byte[]

type RequestPart =
    | Query of string * OpenApiValue
    | Path of string * OpenApiValue
    | Header of string * OpenApiValue
    | MultiPartFormData of string * MultiPartFormData
    | UrlEncodedFormData of string * OpenApiValue
    | JsonContent of string
    | BinaryContent of byte[]
    | Ignore

    static member query(key: string, value: int) = Query(key, OpenApiValue.Int value)
    static member query(key: string, values: int list) = Query(key, OpenApiValue.List [ for value in values -> OpenApiValue.Int value ])
    static member query(key: string, value: int64) = Query(key, OpenApiValue.Int64 value)
    static member query(key: string, value: string) = Query(key, OpenApiValue.String value)
    static member query(key: string, value: string option) = 
        match value with 
        | Some text -> Query(key, OpenApiValue.String text)
        | None -> Ignore
    static member query(key: string, value: int option) = 
        match value with 
        | Some number -> Query(key, OpenApiValue.Int number)
        | None -> Ignore
    static member query(key: string, value: bool option) = 
        match value with 
        | Some flag -> Query(key, OpenApiValue.Bool flag)
        | None -> Ignore
    static member query(key: string, value: double option) =
        match value with
        | Some number -> Query(key, OpenApiValue.Double number)
        | None -> Ignore
    static member inline query< ^a when ^a : (member Format: unit -> string)>(key: string, value: ^a) : RequestPart =
        let format = (^a: (member Format: unit -> string) (value))
        Query(key, OpenApiValue.String format)
    static member inline query< ^a when ^a : (member Format: unit -> string)>(key: string, value: ^a option) : RequestPart =
        match value with
        | None -> Ignore
        | Some instance ->
            let format = (^a: (member Format: unit -> string) (instance))
            Query(key, OpenApiValue.String format)
    static member query(key: string, value: int64 option) = 
        match value with 
        | Some number -> Query(key, OpenApiValue.Int64 number)
        | None -> Ignore

    static member query(key: string, values: string list) = Query(key, OpenApiValue.List [ for value in values -> OpenApiValue.String value ])
    static member query(key: string, values: Guid list) = Query(key, OpenApiValue.List [ for value in values -> OpenApiValue.String (value.ToString()) ])
    static member query(key: string, value: bool) = Query(key, OpenApiValue.Bool value)
    static member query(key: string, value: double) = Query(key, OpenApiValue.Double value)
    static member query(key: string, value: float32) = Query(key, OpenApiValue.Float value)
    static member query(key: string, value: Guid) = Query(key, OpenApiValue.String (value.ToString()))
    static member query(key: string, value: DateTimeOffset) = Query(key, OpenApiValue.String (value.ToString("O")))
    static member path(key: string, value: int) = Path(key, OpenApiValue.Int value)
    static member path(key: string, value: int64) = Path(key, OpenApiValue.Int64 value)
    static member path(key: string, value: string) = Path(key, OpenApiValue.String value)
    static member path(key: string, value: bool) = Path(key, OpenApiValue.Bool value)
    static member path(key: string, value: double) = Path(key, OpenApiValue.Double value)
    static member path(key: string, value: float32) = Path(key, OpenApiValue.Float value)
    static member path(key: string, value: Guid) = Path(key, OpenApiValue.String (value.ToString()))
    static member path(key: string, value: DateTimeOffset) = Path(key, OpenApiValue.String (value.ToString("O")))
    static member path(key: string, values: string list) = Path(key, OpenApiValue.List [ for value in values -> OpenApiValue.String value ])
    static member path(key: string, values: Guid list) = Path(key, OpenApiValue.List [ for value in values -> OpenApiValue.String (value.ToString()) ])
    static member path(key: string, values: int list) = Path(key, OpenApiValue.List [ for value in values -> OpenApiValue.Int value ])
    static member path(key: string, values: int64 list) = Path(key, OpenApiValue.List [ for value in values -> OpenApiValue.Int64 value ])
    static member inline path< ^a when ^a : (member Format: unit -> string)>(key: string, value: ^a) : RequestPart =
        let format = (^a: (member Format: unit -> string) (value))
        Path(key, OpenApiValue.String format)
    static member inline path< ^a when ^a : (member Format: unit -> string)>(key: string, value: ^a option) : RequestPart =
        match value with
        | None -> Ignore
        | Some instance ->
            let format = (^a: (member Format: unit -> string) (instance))
            Path(key, OpenApiValue.String format)
    static member multipartFormData(key: string, value: int) =
        MultiPartFormData(key, Primitive(OpenApiValue.Int value))
    static member multipartFormData(key: string, value: int64) =
        MultiPartFormData(key, Primitive(OpenApiValue.Int64 value))
    static member multipartFormData(key: string, value: string) =
        MultiPartFormData(key, Primitive(OpenApiValue.String value))
    static member multipartFormData(key: string, value: bool) =
        MultiPartFormData(key, Primitive(OpenApiValue.Bool value))
    static member multipartFormData(key: string, value: double) =
        MultiPartFormData(key, Primitive(OpenApiValue.Double value))
    static member multipartFormData(key: string, value: float32) =
        MultiPartFormData(key, Primitive(OpenApiValue.Float value))
    static member multipartFormData(key: string, value: Guid) =
        MultiPartFormData(key, Primitive(OpenApiValue.String (value.ToString())))
    static member multipartFormData(key: string, value: DateTimeOffset) =
        MultiPartFormData(key, Primitive(OpenApiValue.String (value.ToString("O"))))
    static member multipartFormData(key: string, value: byte[]) =
        MultiPartFormData(key, File value)
    static member multipartFormData(key: string, values: string list) = MultiPartFormData(key, Primitive(OpenApiValue.List [ for value in values -> OpenApiValue.String value ]))
    static member multipartFormData(key: string, values: Guid list) = MultiPartFormData(key, Primitive(OpenApiValue.List [ for value in values -> OpenApiValue.String (value.ToString()) ]))
    static member multipartFormData(key: string, values: int list) = MultiPartFormData(key, Primitive(OpenApiValue.List [ for value in values -> OpenApiValue.Int value ]))
    static member multipartFormData(key: string, values: int64 list) = MultiPartFormData(key, Primitive(OpenApiValue.List [ for value in values -> OpenApiValue.Int64 value ]))
    static member urlEncodedFormData(key: string, value: string) = UrlEncodedFormData(key, OpenApiValue.String value)
    static member urlEncodedFormData(key: string, value: bool) = UrlEncodedFormData(key, OpenApiValue.Bool value)
    static member urlEncodedFormData(key: string, value: double) = UrlEncodedFormData(key, OpenApiValue.Double value)
    static member urlEncodedFormData(key: string, value: float32) = UrlEncodedFormData(key, OpenApiValue.Float value)
    static member urlEncodedFormData(key: string, value: Guid) = UrlEncodedFormData(key, OpenApiValue.String (value.ToString()))
    static member urlEncodedFormData(key: string, value: DateTimeOffset) = UrlEncodedFormData(key, OpenApiValue.String (value.ToString("O")))
    static member header(key: string, value: int) = Header(key, OpenApiValue.Int value)
    static member header(key: string, value: int64) = Header(key, OpenApiValue.Int64 value)
    static member header(key: string, value: string) = Header(key, OpenApiValue.String value)
    static member header(key: string, value: bool) = Header(key, OpenApiValue.Bool value)
    static member header(key: string, value: double) = Header(key, OpenApiValue.Double value)
    static member header(key: string, value: float32) = Header(key, OpenApiValue.Float value)
    static member header(key: string, value: Guid) = Header(key, OpenApiValue.String (value.ToString()))
    static member header(key: string, value: DateTimeOffset) = Header(key, OpenApiValue.String (value.ToString("O")))
    static member jsonContent<'t>(content: 't) = JsonContent(Serializer.serialize content)
    static member binaryContent(content: byte[]) = BinaryContent(content)

module OpenApiHttp =
    let rec serializeValue = function
        | OpenApiValue.String value -> value
        | OpenApiValue.Int value -> value.ToString(CultureInfo.InvariantCulture)
        | OpenApiValue.Int64 value -> value.ToString(CultureInfo.InvariantCulture)
        | OpenApiValue.Double value -> value.ToString(CultureInfo.InvariantCulture)
        | OpenApiValue.Float value -> value.ToString(CultureInfo.InvariantCulture)
        | OpenApiValue.Bool value -> value.ToString().ToLower()
        | OpenApiValue.List values ->
            values
            |> List.map serializeValue
            |> String.concat ","

    let applyPathParts (path: string) (parts: RequestPart list) =
        let applyPart (currentPath: string) (part: RequestPart) : string =
            match part with
            | Path(key, value) -> currentPath.Replace("{" + key + "}", serializeValue value)
            | _ -> currentPath

        parts |> List.fold applyPart path

    let applyQueryStringParameters (currentPath: string) (parts: RequestPart list) =
        let cleanedPath = currentPath.TrimEnd '/'
        let queryParams =
            parts
            |> List.choose (function
                | Query(key, value) -> Some(key, value)
                | _ -> None)

        if List.isEmpty queryParams then
            cleanedPath
        else
            let combinedParamters =
                queryParams
                |> List.map (fun (key, value) -> $"{key}={Uri.EscapeDataString(serializeValue value)}")
                |> String.concat "&"

            cleanedPath + "?" + combinedParamters

    let applyJsonContent (parts: RequestPart list) (httpRequest: HttpRequestMessage) =
        for part in parts do
            match part with
            | JsonContent content ->
                httpRequest.Content <- new StringContent(content, Encoding.UTF8, "application/json")
            | _ -> ()

        httpRequest

    let applyBinaryContent (parts: RequestPart list) (httpRequest: HttpRequestMessage) =
        for part in parts do
            match part with
            | BinaryContent content ->
                httpRequest.Content <- new ByteArrayContent(content)
            | _ -> ()

        httpRequest

    let applyHeaders (parts: RequestPart list) (httpRequest: HttpRequestMessage) =
        for part in parts do
            match part with
            | Header(key, value) ->
                httpRequest.Headers.Add(key, serializeValue value)
            | _ ->
                ()

        httpRequest

    let applyAcceptHeader (httpRequest: HttpRequestMessage) =
        httpRequest.Headers.Accept.ParseAdd "application/json"
        httpRequest

    let applyMultiPartFormData (parts: RequestPart list) (httpRequest: HttpRequestMessage) =
        let formParts =
            parts
            |> List.choose (function
                | MultiPartFormData (key, value) -> Some(key, value)
                | _ -> None
            )

        if formParts.IsEmpty then
            httpRequest
        else
            let multipartFormData = new MultipartFormDataContent()
            for (key, part) in formParts do
                match part with
                | Primitive value ->
                    let content = new StringContent(serializeValue value, Encoding.UTF8)
                    multipartFormData.Add(content, key)
                | File file ->
                    let content = new ByteArrayContent(file)
                    multipartFormData.Add(content, key)

            httpRequest.Content <- multipartFormData
            httpRequest

    let applyUrlEncodedFormData (parts: RequestPart list) (httpRequest: HttpRequestMessage) =
        let formParts =
            parts
            |> List.choose (function
                | UrlEncodedFormData(key, value) -> Some(key, value)
                | _ -> None
            )

        if formParts.IsEmpty then
            httpRequest
        else
            let contents = [ for (key, value) in formParts -> KeyValuePair(key, serializeValue value) ]
            httpRequest.Content <- new FormUrlEncodedContent(contents)
            httpRequest

    let sendAsync (httpClient: HttpClient) (method: HttpMethod) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        let cancellationToken = Option.defaultValue CancellationToken.None cancellationToken
        let modifiedPath = applyPathParts path parts
        let modifiedQueryParams = applyQueryStringParameters modifiedPath parts
        let requestUri = Uri(httpClient.BaseAddress.OriginalString.TrimEnd '/' + modifiedQueryParams)
        let request = new HttpRequestMessage(RequestUri=requestUri, Method=method)
        let populatedRequest =
            request
            |> applyJsonContent parts
            |> applyBinaryContent parts
            |> applyUrlEncodedFormData parts
            |> applyMultiPartFormData parts
            |> applyHeaders parts

        async {
            let! response = Async.AwaitTask(httpClient.SendAsync(populatedRequest, cancellationToken))
            let! content = Async.AwaitTask(response.Content.ReadAsStringAsync())
            return (response.StatusCode, content)
        }

    let sendBinaryAsync (httpClient: HttpClient) (method: HttpMethod) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        let cancellationToken = Option.defaultValue CancellationToken.None cancellationToken
        let modifiedPath = applyPathParts path parts
        let modifiedQueryParams = applyQueryStringParameters modifiedPath parts
        let requestUri = Uri(httpClient.BaseAddress.OriginalString.TrimEnd '/' + modifiedQueryParams)
        let request = new HttpRequestMessage(RequestUri=requestUri, Method=method)
        let populatedRequest =
            request
            |> applyJsonContent parts
            |> applyBinaryContent parts
            |> applyUrlEncodedFormData parts
            |> applyMultiPartFormData parts
            |> applyHeaders parts

        async {
            let! response = Async.AwaitTask(httpClient.SendAsync(populatedRequest, cancellationToken))
            let! content = Async.AwaitTask(response.Content.ReadAsByteArrayAsync())
            return (response.StatusCode, content)
        }

    let getAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendAsync httpClient HttpMethod.Get path parts cancellationToken

    let get (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        getAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let getBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendBinaryAsync httpClient HttpMethod.Get path parts cancellationToken

    let getBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        getBinaryAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let postAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendAsync httpClient HttpMethod.Post path parts cancellationToken

    let post (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        postAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let postBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendBinaryAsync httpClient HttpMethod.Post path parts cancellationToken

    let postBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) = 
        postBinaryAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let deleteAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendAsync httpClient HttpMethod.Delete path parts cancellationToken

    let delete (httpClient: HttpClient) (path: string) (parts: RequestPart list)  (cancellationToken: CancellationToken option) =
        deleteAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let deleteBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendBinaryAsync httpClient HttpMethod.Delete path parts cancellationToken

    let deleteBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list)  (cancellationToken: CancellationToken option) =
        deleteBinaryAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let putAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendAsync httpClient HttpMethod.Put path parts cancellationToken

    let put (httpClient: HttpClient) (path: string) (parts: RequestPart list)  (cancellationToken: CancellationToken option) =
        putAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let putBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendBinaryAsync httpClient HttpMethod.Put path parts cancellationToken

    let putBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        putBinaryAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let patchAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendAsync httpClient (HttpMethod "PATCH") path parts cancellationToken

    let patch (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        patchAsync httpClient path parts cancellationToken 
        |> Async.RunSynchronously

    let patchBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendBinaryAsync httpClient (HttpMethod "PATCH") path parts cancellationToken

    let patchBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        patchBinaryAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously
    
    let headAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendAsync httpClient (HttpMethod "HEAD") path parts cancellationToken
         
    let head (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        headAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously

    let headBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        sendBinaryAsync httpClient (HttpMethod "HEAD") path parts cancellationToken

    let headBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) (cancellationToken: CancellationToken option) =
        headBinaryAsync httpClient path parts cancellationToken
        |> Async.RunSynchronously
