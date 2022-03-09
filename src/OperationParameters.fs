[<AutoOpen>]
module OperationParameters

open System
open FsAst
open FSharp.Compiler.SyntaxTree
open Microsoft.OpenApi.Models

let private paramReplace (parameter: string) (sep: char) : string =
    let parts = parameter.Split(sep)
    let firstPart = parts.[0]
    let otherParts = parts.[1..]
    let modified = [
        yield firstPart
        for part in otherParts do
            yield capitalize part
    ]

    modified
    |> List.map (fun part -> part.Replace("$", "").Replace("@", "").Replace(":", ""))
    |> String.concat ""
    |> camelCase

let rec private cleanParamIdent (parameter: string) (parameters: OperationParameter seq) : string =
    if String.IsNullOrWhiteSpace parameter then
        $"p{(parameters |> Seq.length) + 1}"
    else
        let parameter = String(Array.ofSeq (parameter |> Seq.skipWhile (Char.IsLetter >> not)))
        let cleanedParam =
            if parameter.Contains "-" then
                cleanParamIdent (paramReplace parameter '-') parameters
            elif parameter.Contains "_" then
                cleanParamIdent (paramReplace parameter '_') parameters
            elif parameter.Contains "." then
                cleanParamIdent (paramReplace parameter '.') parameters
            elif parameter.Contains "[" || parameter.Contains "]" then
                match parameter.Split([| "["; "]" |], StringSplitOptions.RemoveEmptyEntries) with
                | [| firstPart; secondPart |]  -> cleanParamIdent(camelCase firstPart + capitalize secondPart) parameters
                | _ -> cleanParamIdent(parameter.Replace("[", "").Replace("]", "")) parameters
            else
                camelCase (parameter.Replace("$", "").Replace("@", "").Replace(":", ""))

        if String.IsNullOrWhiteSpace cleanedParam then
            $"p{(parameters |> Seq.length) + 1}"
        else
            cleanedParam

let rec private readParamType (target: Target) (schema: OpenApiSchema) : SynType =
    if isNull schema then 
        if target = Target.FSharp
        then SynType.JToken()
        else SynType.Object()
    else
    match schema.Type with
    | "integer" when schema.Format = "int64" -> SynType.Int64()
    | "integer" -> SynType.Int()
    | "number" when schema.Format = "float" -> SynType.Float32()
    | "number" ->  SynType.Double()
    | "boolean" -> SynType.Bool()
    | "string" when schema.Format = "uuid" -> SynType.Guid()
    | "string" when schema.Format = "guid" -> SynType.Guid()
    | "string" when schema.Format = "date-time" -> SynType.DateTimeOffset()
    | "string" -> SynType.String()
    | "file" ->
        if target = Target.FSharp
        then SynType.ByteArray()
        else SynType.Create "File" // from Browser.Types

    | _ when not (isNull schema.Reference) ->
        // working with a reference type
        let typeName =
            if invalidTitle schema.Title
            then sanitizeTypeName schema.Reference.Id
            else sanitizeTypeName schema.Title
        SynType.Create typeName
    | "array" ->
        readParamType target schema.Items |> SynType.List
    | "object" ->
        if target = Target.FSharp
        then SynType.JObject()
        else SynType.Object()
    | _ ->
        SynType.String()

let private processOperationParameters
    (target: Target)
    (parameters: OperationParameter seq)
    (parameter: OpenApiParameter)
    : OperationParameter option =

    if isNull parameter ||
       parameter.Deprecated ||
       not parameter.In.HasValue ||
       isNull parameter.Schema
    then None
    else
        let shouldSpreadProperties =
            parameter.Style.HasValue
            && parameter.Style.Value = ParameterStyle.DeepObject
            && parameter.Explode
            && isNotNull parameter.Schema
            && parameter.Schema.Type = "object"

        let properties =
            if shouldSpreadProperties
            then List.ofSeq parameter.Schema.Properties.Keys
            else []

        let paramType =
            if parameter.Content.Count = 1 then
                let firstKey = Seq.head parameter.Content.Keys
                readParamType target parameter.Content.[firstKey].Schema
            elif parameter.In.Value = ParameterLocation.Header && isNotNull parameter.Schema && parameter.Schema.Type = "object" then
                // edge case for a weird schema
                SynType.String()
            else
                readParamType target parameter.Schema

        let nullable =
            if isNotNull parameter.Extensions && parameter.Extensions.ContainsKey "x-nullable" then
                match parameter.Extensions.["x-nullable"] with
                | :? Microsoft.OpenApi.Any.OpenApiBoolean as isNullable -> isNullable.Value
                | _ -> true
            else
                true

        let parameterIdentifier = cleanParamIdent parameter.Name parameters
        let identifierAlreadyAdded =
            parameters
            |> Seq.exists (fun opParam -> opParam.parameterIdent = parameterIdentifier)

        Some {
            parameterName = parameter.Name
            parameterIdent =
                if identifierAlreadyAdded
                then  $"{parameterIdentifier}In{capitalize(string parameter.In.Value)}"
                else parameterIdentifier
            required = parameter.Required || not nullable
            parameterType =  paramType
            docs = parameter.Description
            location = (string parameter.In.Value).ToLower()
            properties = properties
            style =
                if parameter.Style.HasValue
                then (string parameter.Style).ToLower()
                else "none"
        }

let private processOperationRequestBody
    (config: CodegenConfig)
    (operation: OpenApiOperation)
    (parameters: OperationParameter seq)
    : OperationParameter list =

    if isNull operation.RequestBody then []
    else
        let content = operation.RequestBody.Content

        if content.ContainsKey "application/json" && not (isEmptySchema content.["application/json"].Schema) then
            let schema = content.["application/json"].Schema
            let typeName = "body"
            let parameterName =
                if operation.RequestBody.Extensions.ContainsKey "x-bodyName" then
                    match operation.RequestBody.Extensions.["x-bodyName"] with
                    | :? Microsoft.OpenApi.Any.OpenApiString as name -> name.Value
                    | _ -> camelCase (normalizeFullCaps typeName)
                else
                    camelCase (normalizeFullCaps typeName)

            let requestTypePayload =
                if operation.Extensions.ContainsKey "RequestTypePayload" then
                    match operation.Extensions.["RequestTypePayload"] with
                    | :? Microsoft.OpenApi.Any.OpenApiString as requestTypePayload ->
                        SynType.Create requestTypePayload.Value
                    | _ ->
                        readParamType config.target schema
                else
                    readParamType config.target schema

            [{
                parameterName = parameterName
                parameterIdent = cleanParamIdent parameterName parameters
                required = true
                parameterType = requestTypePayload
                docs = schema.Description
                location = "jsonContent"
                style = "none"
                properties = []
            }]

        elif content.ContainsKey "application/json" && isEmptySchema content.["application/json"].Schema && config.emptyDefinitions = EmptyDefinitionResolution.GenerateFreeForm then
            let schema = content.["application/json"].Schema
            let typeName = "body"
            let parameterName =
                if operation.RequestBody.Extensions.ContainsKey "x-bodyName" then
                    match operation.RequestBody.Extensions.["x-bodyName"] with
                    | :? Microsoft.OpenApi.Any.OpenApiString as name -> name.Value
                    | _ -> camelCase (normalizeFullCaps typeName)
                else
                    camelCase (normalizeFullCaps typeName)

            [{
                parameterName = parameterName
                parameterIdent = cleanParamIdent parameterName parameters
                required = true
                parameterType =
                    if config.target = Target.FSharp
                    then SynType.JToken()
                    else SynType.Object()
                docs =
                    if isNotNull schema
                    then schema.Description
                    else ""
                location = "jsonContent"
                style = "none"
                properties = []
            }]

        elif content.ContainsKey "multipart/form-data" && isNotNull content.["multipart/form-data"].Schema && content.["multipart/form-data"].Schema.Type = "object" then
            content.["multipart/form-data"].Schema.Properties
            |> Seq.map (fun property ->
                let parameterIdentifier = cleanParamIdent property.Key parameters
                let identifierAlreadyAdded =
                    parameters
                    |> Seq.exists (fun opParam -> opParam.parameterIdent = parameterIdentifier)

                {
                    parameterName = property.Key
                    parameterIdent =
                        if identifierAlreadyAdded
                        then $"{parameterIdentifier}InFormData"
                        else parameterIdentifier
                    required = content.["multipart/form-data"].Schema.Required.Contains property.Key
                    parameterType = readParamType config.target property.Value
                    docs = property.Value.Description
                    properties = []
                    location = "multipartFormData"
                    style = "formfield"
                })
            |> Seq.toList

        elif content.ContainsKey "multipart/form-data" && isNotNull content.["multipart/form-data"].Schema && content.["multipart/form-data"].Schema.Type = "file" then
            let parameterName =
                if operation.RequestBody.Extensions.ContainsKey "x-bodyName" then
                    match operation.RequestBody.Extensions.["x-bodyName"] with
                    | :? Microsoft.OpenApi.Any.OpenApiString as name -> name.Value
                    | _ -> "body"
                else
                    "body"

            [{
                parameterName = parameterName
                parameterIdent = cleanParamIdent parameterName parameters
                required = true
                parameterType =
                    if config.target = Target.FSharp
                    then SynType.ByteArray()
                    else SynType.Create "File" // from Browser.Types
                docs = ""
                properties = []
                location = "multipartFormData"
                style = "formfield"
            }]

        elif content.ContainsKey "application/x-www-form-urlencoded" then
            let schema = content.["application/x-www-form-urlencoded"].Schema
            if isNull schema || schema.Type = "object" then []
            else
                schema.Properties
                |> Seq.map (fun property ->
                    {
                        parameterName = property.Key
                        parameterIdent = cleanParamIdent property.Key parameters
                        required = schema.Required.Contains property.Key
                        parameterType = readParamType config.target property.Value
                        docs = property.Value.Description
                        properties = []
                        location = "urlEncodedFormData"
                        style = "formfield"
                    })
                |> Seq.toList

        elif content.ContainsKey "application/octet-stream" then
            [{
                parameterName = "requestBody"
                parameterIdent = "requestBody"
                required = false
                parameterType = SynType.ByteArray()
                docs = ""
                location = "binaryContent"
                style = "formfield"
                properties = []
            }]
        else []

let operationParameters
    (operation: OpenApiOperation)
    (pathInfoParameters: OpenApiParameter seq)
    (config: CodegenConfig)
    : OperationParameter list =

    let opParameters =
        (Seq.append pathInfoParameters operation.Parameters)
        |> Seq.fold (fun opParameters oaParameter ->
            match (processOperationParameters config.target opParameters oaParameter) with
            | None -> opParameters
            | Some opParameter -> opParameter :: opParameters) []
        |> List.rev
    
    let rqParameters =
        processOperationRequestBody config operation opParameters
        |> List.rev

    opParameters @ rqParameters
    |> List.sortBy (fun param ->
        // required parameters come first
        if param.required
        then 0
        else 1
    )