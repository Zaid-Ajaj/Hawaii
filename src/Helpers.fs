[<AutoOpen>]
module Helpers

open System
open Fantomas
open Microsoft.OpenApi.Models
open System.Linq

let inline isNotNull (x: 't) = not (isNull x)

let capitalize (input: string) =
    if String.IsNullOrWhiteSpace input
    then ""
    else input.First().ToString().ToUpper() + String.Join("", input.Skip(1))

let camelCase (input: string) =
    if String.IsNullOrWhiteSpace input
    then ""
    else input.First().ToString().ToLower() + String.Join("", input.Skip(1))

let normalizeFullCaps (input: string) =
    let fullCaps =
        input |> Seq.forall Char.IsUpper

    if fullCaps
    then input.ToLower()
    else input

let sanitizeTypeName (typeName: string) =
    if String.IsNullOrWhiteSpace typeName then
        typeName
    elif typeName.Contains "`" then
        match typeName.Split '`' with
        | [| name; typeArgArity |] -> name
        | _ -> typeName.Replace("`", "")
    elif typeName.Contains "." then
        typeName.Split('.', StringSplitOptions.RemoveEmptyEntries)
        |> String.concat ""
    elif typeName.Contains "_" then
        typeName.Split('_', StringSplitOptions.RemoveEmptyEntries)
        |> String.concat ""
    elif typeName.Contains "[" && typeName.Contains "]" then
        typeName.Replace("[", "").Replace("]", "")
    else
        typeName

let invalidTitle (title: string) =
    String.IsNullOrWhiteSpace title
    || (title.Contains "Mediatype identifier" && title.Contains "application/")
    || (title.Split(' ').Length >= 1)

let isEmptySchema (schema: OpenApiSchema) =
    isNull schema
    || (
        (isNull schema.Type || schema.Type = "object")
        && schema.Properties.Count = 0
        && schema.AllOf.Count = 0
        && schema.AnyOf.Count = 0
    )