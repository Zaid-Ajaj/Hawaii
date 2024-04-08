module HttpLibrary

let content = """namespace {projectName}.Http

open System
open System.Net.Http
open System.Globalization
open System.Collections.Generic
open System.Text
open Fable.Remoting.Json
open System.Threading
{taskLibrary}

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
                    content.Headers.Add("Content-Disposition", $"form-data; name=\"{key}\"")
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

    let sendAsync (httpClient: HttpClient) (method: HttpMethod) (path: string) (parts: RequestPart list) {cancellationArgument} =
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

        {asyncBuilder} {
            let! response = {getResponse}
            let! content = {getContent}
            return (response.StatusCode, content)
        }

    let sendBinaryAsync (httpClient: HttpClient) (method: HttpMethod) (path: string) (parts: RequestPart list) {cancellationArgument} =
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

        {asyncBuilder} {
            let! response = {getResponse}
            let! content = {getBinaryContent}
            return (response.StatusCode, content)
        }

    let getAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendAsync httpClient HttpMethod.Get path parts {cancellationParameter}

    let get (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        getAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let getBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendBinaryAsync httpClient HttpMethod.Get path parts {cancellationParameter}

    let getBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        getBinaryAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let postAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendAsync httpClient HttpMethod.Post path parts {cancellationParameter}

    let post (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        postAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let postBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendBinaryAsync httpClient HttpMethod.Post path parts {cancellationParameter}

    let postBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} = 
        postBinaryAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let deleteAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendAsync httpClient HttpMethod.Delete path parts {cancellationParameter}

    let delete (httpClient: HttpClient) (path: string) (parts: RequestPart list)  {cancellationArgument} =
        deleteAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let deleteBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendBinaryAsync httpClient HttpMethod.Delete path parts {cancellationParameter}

    let deleteBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list)  {cancellationArgument} =
        deleteBinaryAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let putAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendAsync httpClient HttpMethod.Put path parts {cancellationParameter}

    let put (httpClient: HttpClient) (path: string) (parts: RequestPart list)  {cancellationArgument} =
        putAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let putBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendBinaryAsync httpClient HttpMethod.Put path parts {cancellationParameter}

    let putBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        putBinaryAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let patchAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendAsync httpClient (HttpMethod "PATCH") path parts {cancellationParameter}

    let patch (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        patchAsync httpClient path parts {cancellationParameter} 
        {convertSync}

    let patchBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendBinaryAsync httpClient (HttpMethod "PATCH") path parts {cancellationParameter}

    let patchBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        patchBinaryAsync httpClient path parts {cancellationParameter}
        {convertSync}
    
    let headAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendAsync httpClient (HttpMethod "HEAD") path parts {cancellationParameter}
         
    let head (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        headAsync httpClient path parts {cancellationParameter}
        {convertSync}

    let headBinaryAsync (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        sendBinaryAsync httpClient (HttpMethod "HEAD") path parts {cancellationParameter}

    let headBinary (httpClient: HttpClient) (path: string) (parts: RequestPart list) {cancellationArgument} =
        headBinaryAsync httpClient path parts {cancellationParameter}
        {convertSync}
"""

let library isTask projectName =
    let convertSync =
        if isTask
        then "|> Async.AwaitTask |> Async.RunSynchronously"
        else "|> Async.RunSynchronously"

    let getResponse =
        if isTask
        then "httpClient.SendAsync(populatedRequest, cancellationToken)"
        else "Async.AwaitTask(httpClient.SendAsync(populatedRequest, cancellationToken))"

    let getContent =
        if isTask
        then "response.Content.ReadAsStringAsync()"
        else "Async.AwaitTask(response.Content.ReadAsStringAsync())"

    let getBinaryContent = 
        if isTask
        then "response.Content.ReadAsByteArrayAsync()"
        else "Async.AwaitTask(response.Content.ReadAsByteArrayAsync())"

    content
        .Replace("{projectName}", projectName)
        .Replace("{taskLibrary}", if isTask then "open FSharp.Control.Tasks" else "")
        .Replace("{asyncBuilder}", if isTask then "task" else "async")
        .Replace("{cancellationArgument}", "(cancellationToken: CancellationToken option)")
        .Replace("{cancellationParameter}", "cancellationToken")
        .Replace("{convertSync}", convertSync)
        .Replace("{getResponse}", getResponse)
        .Replace("{getContent}", getContent)
        .Replace("{getBinaryContent}", getBinaryContent)

let fableContent = """namespace {projectName}.Http

open System
open Fable.SimpleJson
open Fable.SimpleHttp
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open System.Runtime.CompilerServices

/// Utilities for working with binary data types in the browser
module Utilities =
    [<Emit("new FileReader()")>]
    /// Creates a new instance of a FileReader
    let createFileReader() : FileReader = jsNative
    [<Emit("new Uint8Array($0)")>]
    let createUInt8Array(x: 'a) : byte[]  = jsNative
    /// Creates a Blob from the given input string
    [<Emit("new Blob([$0.buffer], { type: $1 })")>]
    let createBlobFromBytesAndMimeType (value: byte[]) (mimeType: string) : Blob = jsNative
    [<Emit("window.URL.createObjectURL($0)")>]
    /// Creates an object URL (also known as data url) from a Blob
    let createObjectUrl (blob: Blob) : string = jsNative
    [<Emit "URL.revokeObjectURL($0)">]
    /// Releases an existing object URL which was previously created by calling createObjectURL(). Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
    let revokeObjectUrl (dataUrl: string) : unit = jsNative
    [<Emit "$0 instanceof Uint8Array">]
    /// Returns whether the input byte array is a typed array of type Uint8Array
    let isUInt8Array (data: byte[]) : bool = jsNative
    /// Creates a typed byte array of binary data if it not already typed
    let toUInt8Array(data: byte[]) : byte[] =
        if isUInt8Array data
        then data
        else createUInt8Array data

    /// Creates a Blob from the given input string
    [<Emit("new Blob([$0.buffer], { type: 'text/plain' })")>]
    let fromBinaryEncodedText (value: byte[]) : Blob = jsNative

    /// Asynchronously reads the blob data content as string
    let readBytesAsText (bytes: byte[]) : Async<string> =
        Async.FromContinuations <| fun (resolve, _, _) ->
            let reader = createFileReader()
            reader.onload <- fun _ ->
                if reader.readyState = FileReaderState.DONE
                then resolve (unbox reader.result)

            reader.readAsText(fromBinaryEncodedText bytes)



[<AutoOpenAttribute>]
module BrowserFileExtensions =

    type File with

        /// Asynchronously reads the File content as byte[]
        member instance.ReadAsByteArray() =
            Async.FromContinuations <| fun (resolve, _, _) ->
                let reader = Utilities.createFileReader()
                reader.onload <- fun _ ->
                    if reader.readyState = FileReaderState.DONE
                    then resolve(Utilities.createUInt8Array(reader.result))

                reader.readAsArrayBuffer(instance)

        /// Asynchronously reads the File content as a data url string
        member instance.ReadAsDataUrl() =
            Async.FromContinuations <| fun (resolve, _, _) ->
                let reader = Utilities.createFileReader()
                reader.onload <- fun _ ->
                    if reader.readyState = FileReaderState.DONE
                    then resolve(unbox<string> reader.result)

                reader.readAsDataURL(instance)

        /// <summary>Asynchronously reads the File contents as text</summary>
        member instance.ReadAsText() =
            Async.FromContinuations <| fun (resolve, _, _) ->
                let reader = Utilities.createFileReader()
                reader.onload <- fun _ ->
                    if reader.readyState = FileReaderState.DONE
                    then resolve(unbox<string> reader.result)

                reader.readAsText(instance)

[<Extension>]
type ByteArrayExtensions =
    [<Extension>]
    /// <summary>Saves the binary content as a file using the provided file name.</summary>
    static member SaveFileAs(content: byte[], fileName: string) =

        if String.IsNullOrWhiteSpace(fileName) then
            ()
        else
        let mimeType = "application/octet-stream"
        let binaryData = Utilities.toUInt8Array content
        let blob = Utilities.createBlobFromBytesAndMimeType binaryData mimeType
        let dataUrl = Utilities.createObjectUrl blob
        let anchor =  (Browser.Dom.document.createElement "a")
        anchor?style <- "display: none"
        anchor?href <- dataUrl
        anchor?download <- fileName
        anchor?rel <- "noopener"
        anchor.click()
        // clean up
        anchor.remove()
        // clean up the created object url because it is being kept in memory
        Browser.Dom.window.setTimeout(unbox(fun () -> Utilities.revokeObjectUrl(dataUrl)), 40 * 1000)
        |> ignore

    [<Extension>]
    /// Saves the binary content as a file using the provided file name.
    static member SaveFileAs(content: byte[], fileName: string, mimeType: string) =

        if String.IsNullOrWhiteSpace(fileName) then
            ()
        else
        let binaryData = Utilities.toUInt8Array content
        let blob = Utilities.createBlobFromBytesAndMimeType binaryData mimeType
        let dataUrl = Utilities.createObjectUrl blob
        let anchor =  Browser.Dom.document.createElement "a"
        anchor?style <- "display: none"
        anchor?href <- dataUrl
        anchor?download <- fileName
        anchor?rel <- "noopener"
        anchor.click()
        // clean up element
        anchor.remove()
        // clean up the created object url because it is being kept in memory
        Browser.Dom.window.setTimeout(unbox(fun () -> Utilities.revokeObjectUrl(dataUrl)), 40 * 1000)
        |> ignore

    [<Extension>]
    /// Converts the binary content into a data url by first converting it to a Blob of type "application/octet-stream" and reading it as a data url.
    static member AsDataUrl(content: byte[]) : string =
        let binaryData = Utilities.toUInt8Array content
        let blob = Utilities.createBlobFromBytesAndMimeType binaryData "application/octet-stream"
        let dataUrl = Utilities.createObjectUrl blob
        dataUrl

    [<Extension>]
    /// Converts the binary content into a data url by first converting it to a Blob of the provided mime-type and reading it as a data url.
    static member AsDataUrl(content: byte[], mimeType:string) : string =
        let binaryData = Utilities.toUInt8Array content
        let blob = Utilities.createBlobFromBytesAndMimeType binaryData mimeType
        let dataUrl = Utilities.createObjectUrl blob
        dataUrl

module Serializer =
    let inline serialize<'t> (value: 't) = Json.serialize value
    let inline deserialize<'t> (content: string) = Json.parseNativeAs<'t>(content)

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
    | File of File

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
    static member multipartFormData(key: string, value: File) =
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
    static member inline jsonContent<'t>(content: 't) = JsonContent(Serializer.serialize content)
    static member binaryContent(content: byte[]) = BinaryContent(content)

module OpenApiHttp =
    let rec serializeValue = function
        | OpenApiValue.String value -> value
        | OpenApiValue.Int value -> string value
        | OpenApiValue.Int64 value -> string value
        | OpenApiValue.Double value -> string value
        | OpenApiValue.Float value -> string value
        | OpenApiValue.Bool value -> string value
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

    [<Emit "encodeURIComponent($0)">]
    let encodeURIComponent(queryValue: string) = jsNative

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
                |> List.map (fun (key, value) -> $"{key}={encodeURIComponent(serializeValue value)}")
                |> String.concat "&"

            cleanedPath + "?" + combinedParamters

    let combineBasePath (basePath: string) (path: string) : string =
        basePath.TrimEnd '/' + "/" + path.TrimStart '/'

    let applyJsonRequestBody (parts: RequestPart list) (httpRequest: HttpRequest) =
        parts
        |> Seq.choose (function
            | JsonContent json ->
                httpRequest
                |> Http.header (Headers.contentType "application/json")
                |> Http.content (BodyContent.Text json)
                |> Some
            | _ -> None)
        |> Seq.tryHead
        |> Option.defaultValue httpRequest

    let applyMultipartFormData (parts: RequestPart list) (httpRequest: HttpRequest) =
        let formParts =
            parts
            |> List.choose(function
                | MultiPartFormData(key, value) -> Some(key, value)
                | _ -> None
            )

        if formParts |> List.isEmpty then
            httpRequest
        else
            let formData = FormData.create()
            for (key, value) in formParts do
                match value with
                | Primitive primitive -> formData.append(key, serializeValue primitive)
                | File file -> formData.append(key, file)

            httpRequest
            |> Http.content (BodyContent.Form formData)

    let applyUrlEncodedFormData (parts: RequestPart list) (httpRequest: HttpRequest) =
        parts
        |> List.choose(function
            | UrlEncodedFormData(key, value) ->
                Some $"{key}={encodeURIComponent(serializeValue value)}"
            | _ -> None)
        |> function
            | [] -> httpRequest
            | data ->
                httpRequest
                |> Http.header (Headers.contentType "application/x-www-form-urlencoded")
                |> Http.content (BodyContent.Text (data |> String.concat "&"))

    let applyHeaders (parts: RequestPart list) (httpRequest: HttpRequest) =
        parts
        |> List.choose (function 
            | RequestPart.Header (s, v) ->
                Fable.SimpleHttp.Header (s, serializeValue v)
                |> Some
            | _ -> None)
        |> function
            | [] -> httpRequest
            | headers ->
                httpRequest
                |> Http.headers headers

    let sendAsync (method: HttpMethod) (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) : Async<int * string> =
        async {
            let requestPath = applyPathParts path parts
            let requestPathWithQuery = applyQueryStringParameters requestPath parts
            let fullPath = combineBasePath basePath requestPathWithQuery
            let! response =
                Http.request fullPath
                |> Http.method method
                |> applyJsonRequestBody parts
                |> applyMultipartFormData parts
                |> applyUrlEncodedFormData parts
                |> applyHeaders parts
                |> Http.headers extraHeaders
                |> Http.withCredentials true
                |> Http.send

            let status = response.statusCode
            let content = response.responseText
            return status, content
        }

    let sendBinaryAsync (method: HttpMethod) (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) : Async<int * byte[]> =
        async {
            let requestPath = applyPathParts path parts
            let requestPathWithQuery = applyQueryStringParameters requestPath parts
            let fullPath = combineBasePath basePath requestPathWithQuery
            let! response =
                Http.request fullPath
                |> Http.method method
                |> applyJsonRequestBody parts
                |> applyMultipartFormData parts
                |> applyUrlEncodedFormData parts
                |> Http.headers extraHeaders
                |> Http.overrideResponseType ResponseTypes.ArrayBuffer
                |> Http.withCredentials true
                |> Http.send

            match response.content with
            | ResponseContent.ArrayBuffer arrayBuffer ->
                let status = response.statusCode
                let content = Utilities.createUInt8Array arrayBuffer
                return status, content
            | _ ->
                let status = response.statusCode
                return status, [||]
        }

    let getAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendAsync GET basePath path extraHeaders parts

    let getBinaryAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendBinaryAsync GET basePath path extraHeaders parts

    let postAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendAsync POST basePath path extraHeaders parts

    let postBinaryAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendBinaryAsync POST basePath path extraHeaders parts

    let deleteAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendAsync DELETE basePath path extraHeaders parts

    let deleteBinaryAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendBinaryAsync DELETE basePath path extraHeaders parts

    let putAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendAsync PUT basePath path extraHeaders parts

    let putBinaryAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendBinaryAsync PUT basePath path extraHeaders parts

    let patchAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendAsync PATCH basePath path extraHeaders parts

    let patchBinaryAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendBinaryAsync PATCH basePath path extraHeaders parts

    let headAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendAsync HEAD basePath path extraHeaders parts

    let headBinaryAsync (basePath: string) (path: string) (extraHeaders: Header list) (parts: RequestPart list) =
        sendBinaryAsync HEAD basePath path extraHeaders parts
"""

let fableLibrary (projectName: string) = fableContent.Replace("{projectName}", projectName)
