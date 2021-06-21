module HttpLibrary

let content = """namespace {projectName}.Http

open {projectName}.Json
open System
open System.Net
open System.Net.Http

module Serializer =
    open Newtonsoft.Json
    let converter = OpenApiConverter() :> JsonConverter
    let serialize<'t> (value: 't) = JsonConvert.SerializeObject(value, [| converter |])

[<RequireQualifiedAccess>]
type OpenApiValue =
    | Int of int
    | Int64 of int64
    | String of string
    | Bool of bool
    | Double of double
    | Float of float32
    | List of OpenApiValue list

type RequestValue =
    | Query of string * OpenApiValue
    | Path of string * OpenApiValue
    | Header of string * OpenApiValue
    | FormData of string * OpenApiValue
    | File of string * byte[]
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
    static member formData(key: string, value: int) = FormData(key, OpenApiValue.Int value)
    static member formData(key: string, value: int64) = FormData(key, OpenApiValue.Int64 value)
    static member formData(key: string, value: string) = FormData(key, OpenApiValue.String value)
    static member formData(key: string, value: bool) = FormData(key, OpenApiValue.Bool value)
    static member formData(key: string, value: double) = FormData(key, OpenApiValue.Double value)
    static member formData(key: string, value: float32) = FormData(key, OpenApiValue.Float value)
    static member formData(key: string, value: Guid) = FormData(key, OpenApiValue.String (value.ToString()))
    static member formData(key: string, value: byte[]) = File(key, value)
    static member header(key: string, value: int) = Header(key, OpenApiValue.Int value)
    static member header(key: string, value: int64) = Header(key, OpenApiValue.Int64 value)
    static member header(key: string, value: string) = Header(key, OpenApiValue.String value)
    static member header(key: string, value: bool) = Header(key, OpenApiValue.Bool value)
    static member header(key: string, value: double) = Header(key, OpenApiValue.Double value)
    static member header(key: string, value: float32) = Header(key, OpenApiValue.Float value)
    static member header(key: string, value: Guid) = Header(key, OpenApiValue.String (value.ToString()))
    static member file(key: string, value: byte[]) = File(key, value)
    static member body<'t>(content: 't) = Body(Serializer.serialize content)

module OpenApiHttp =
    let getAsync (httpClient: HttpClient) (path: string) (parts: RequestValue list) : Async<HttpWebResponse> =
        failwith "To be implemented"
    let postAsync (httpClient: HttpClient) (path: string) (parts: RequestValue list) : Async<HttpWebResponse> =
        failwith "To be implemented"
    let deleteAsync (httpClient: HttpClient) (path: string) (parts: RequestValue list) : Async<HttpWebResponse> =
        failwith "To be implemented"
    let patchAsync (httpClient: HttpClient) (path: string) (parts: RequestValue list) : Async<HttpWebResponse> =
        failwith "To be implemented"
    let putAsync (httpClient: HttpClient) (path: string) (parts: RequestValue list) : Async<HttpWebResponse> =
        failwith "To be implemented"
"""