module HttpLibrary

let content = """namespace {projectName}.Http

open System
open System.Net.Http
open System.Globalization
open System.Text
open Fable.Remoting.Json
{taskLibrary}

module Serializer =
    open Newtonsoft.Json
    let converter = FableJsonConverter() :> JsonConverter
    let settings = JsonSerializerSettings(Converters=[| converter |])
    settings.DateParseHandling <- DateParseHandling.None
    let serialize<'t> (value: 't) = JsonConvert.SerializeObject(value, settings)
    let deserialize<'t> (content: string) = JsonConvert.DeserializeObject<'t> content

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
    | Body of string

    static member query(key: string, value: int) = Query(key, OpenApiValue.Int value)
    static member query(key: string, values: int list) = Query(key, OpenApiValue.List [ for value in values -> OpenApiValue.Int value ])
    static member query(key: string, value: int64) = Query(key, OpenApiValue.Int64 value)
    static member query(key: string, value: string) = Query(key, OpenApiValue.String value)
    static member query(key: string, values: string list) = Query(key, OpenApiValue.List [ for value in values -> OpenApiValue.String value ])
    static member query(key: string, value: bool) = Query(key, OpenApiValue.Bool value)
    static member query(key: string, value: double) = Query(key, OpenApiValue.Double value)
    static member query(key: string, value: float32) = Query(key, OpenApiValue.Float value)
    static member query(key: string, value: Guid) = Query(key, OpenApiValue.String (value.ToString()))
    static member path(key: string, value: int) = Path(key, OpenApiValue.Int value)
    static member path(key: string, value: int64) = Path(key, OpenApiValue.Int64 value)
    static member path(key: string, value: string) = Path(key, OpenApiValue.String value)
    static member path(key: string, value: bool) = Path(key, OpenApiValue.Bool value)
    static member path(key: string, value: double) = Path(key, OpenApiValue.Double value)
    static member path(key: string, value: float32) = Path(key, OpenApiValue.Float value)
    static member path(key: string, value: Guid) = Path(key, OpenApiValue.String (value.ToString()))
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
    static member multipartFormData(key: string, value: byte[]) =
        MultiPartFormData(key, File value)
    static member urlEncodedFormData(key: string, value: string) = UrlEncodedFormData(key, OpenApiValue.String value)
    static member urlEncodedFormData(key: string, value: bool) = UrlEncodedFormData(key, OpenApiValue.Bool value)
    static member urlEncodedFormData(key: string, value: double) = UrlEncodedFormData(key, OpenApiValue.Double value)
    static member urlEncodedFormData(key: string, value: float32) = UrlEncodedFormData(key, OpenApiValue.Float value)
    static member urlEncodedFormData(key: string, value: Guid) = UrlEncodedFormData(key, OpenApiValue.String (value.ToString()))
    static member header(key: string, value: int) = Header(key, OpenApiValue.Int value)
    static member header(key: string, value: int64) = Header(key, OpenApiValue.Int64 value)
    static member header(key: string, value: string) = Header(key, OpenApiValue.String value)
    static member header(key: string, value: bool) = Header(key, OpenApiValue.Bool value)
    static member header(key: string, value: double) = Header(key, OpenApiValue.Double value)
    static member header(key: string, value: float32) = Header(key, OpenApiValue.Float value)
    static member header(key: string, value: Guid) = Header(key, OpenApiValue.String (value.ToString()))
    static member body<'t>(content: 't) = Body(Serializer.serialize content)

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
                |> List.map (fun (key, value) -> $"{key}={Uri.EscapeUriString(serializeValue value)}")
                |> String.concat "&"

            cleanedPath + "?" + combinedParamters

    let applyBodyContent (httpRequest: HttpRequestMessage) (parts: RequestPart list) =
        for part in parts do
            match part with
            | Body content ->
                httpRequest.Content <- new StringContent(content, Encoding.UTF8, "application/json")
            | _ -> ()

        httpRequest.Headers.Accept.ParseAdd "application/json"
        httpRequest

    let sendAsync (httpClient: HttpClient) (method: HttpMethod) (path: string) (parts: RequestPart list) =
        let modifiedPath = applyPathParts path parts
        let modifiedQueryParams = applyQueryStringParameters modifiedPath parts
        let requestUri = Uri(httpClient.BaseAddress, httpClient.BaseAddress.AbsolutePath + modifiedQueryParams)
        use request = new HttpRequestMessage(RequestUri=requestUri, Method=method)
        {asyncBuilder} {
            let requestWithBody = applyBodyContent request parts
            let! response = {getResponse}
            return response
        }

    let getAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        sendAsync httpClient HttpMethod.Get path parts

    let get (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        getAsync httpClient path parts
        {convertSync}

    let postAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        sendAsync httpClient HttpMethod.Post path parts

    let post (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        postAsync httpClient path parts
        {convertSync}

    let deleteAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        sendAsync httpClient HttpMethod.Delete path parts

    let delete (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        deleteAsync httpClient path parts
        {convertSync}

    let putAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        sendAsync httpClient HttpMethod.Put path parts

    let put (httpClient: HttpClient) (path: string) (parts: RequestPart list) =
        putAsync httpClient path parts
        {convertSync}
"""

let library isTask projectName =
    let convertSync =
        if isTask
        then "|> Async.AwaitTask |> Async.RunSynchronously"
        else "|> Async.RunSynchronously"

    let getResponse =
        if isTask
        then "httpClient.SendAsync requestWithBody"
        else "Async.AwaitTask(httpClient.SendAsync requestWithBody)"

    content
        .Replace("{projectName}", projectName)
        .Replace("{taskLibrary}", if isTask then "open FSharp.Control.Tasks" else "")
        .Replace("{asyncBuilder}", if isTask then "task" else "async")
        .Replace("{convertSync}", convertSync)
        .Replace("{getResponse}", getResponse)