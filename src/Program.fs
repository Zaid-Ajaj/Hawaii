﻿open System
open Microsoft.OpenApi.Readers
open System.Net.Http
open FsAst
open Fantomas
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open FSharp.Compiler.XmlDoc
open Microsoft.OpenApi.Models
open System.Linq
open System.IO
open System.Xml.Linq
open System.Net
open System.Collections.Generic
open Newtonsoft.Json.Linq
open System.Text
open Newtonsoft.Json

let logo = """

    _   _                     _ _
    | | | |                   (_|_)
    | |_| | __ ___      ____ _ _ _
    |  _  |/ _` \ \ /\ / / _` | | |
    | | | | (_| |\ V  V / (_| | | |
    \_| |_/\__,_| \_/\_/ \__,_|_|_|


    ❤️  Open source https://www.github.com/Zaid-Ajaj/Hawaii
    ⚖️  MIT LICENSE
"""

[<RequireQualifiedAccess>]
/// <summary>Describes the compilation target</summary>
type Target =
    | FSharp
    | Fable

/// <summary>Describes the async return type of the functions of the generated clients</summary>
[<RequireQualifiedAccess>]
type AsyncReturnType =
    | Async
    | Task

type CodegenConfig = {
    schema: string
    output: string
    target: Target
    project : string
    asyncReturnType: AsyncReturnType
    synchronous: bool
    resolveReferences: bool
}

let inline isNotNull (x: 't) = not (isNull x)

let resolveFile (path: string) =
    if Path.IsPathRooted path then
        path
    elif System.Diagnostics.Debugger.IsAttached then
        Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, path))
    else
        Path.GetFullPath (Path.Combine(Environment.CurrentDirectory, path))

let readConfig file =
    try
        if not (File.Exists file) then
            Error $"Hawaii configuration file {file} was not found"
        else
        let content = File.ReadAllText(file)
        let parts = JObject.Parse(content)
        if not (parts.ContainsKey "schema") then
            Error "Missing required configuration element 'schema'"
        elif isNotNull parts.["schema"] && parts.["schema"].Type <> JTokenType.String then
            Error "Configuration element 'schema' must be a string"
        elif not (parts.ContainsKey "output") then
            Error "Missing required configuration element 'output'"
        elif not (parts.ContainsKey "project") then
            Error "Missing required configuration element 'project'"
        elif isNotNull parts.["output"] && parts.["output"].Type <> JTokenType.String then
            Error "The 'output' configuration element must be a string"
        elif isNotNull parts.["target"] && parts.["target"].Type <> JTokenType.String then
            Error "The 'target' configuration element must be a string"
        elif isNotNull parts.["target"] && parts.["target"].ToObject<string>().ToLower().Trim() <> "fable" && parts.["target"].ToObject<string>().ToLower().Trim() <> "fsharp" then
            Error "The 'target' configuration element can only be 'fable' or 'fsharp'"
        elif isNotNull parts.["project"] && parts.["project"].Type <> JTokenType.String then
            Error "The 'project' configuration element must be a string"
        elif isNotNull parts.["project"] && String.IsNullOrWhiteSpace(parts.["project"].ToString().Trim()) then
            Error "The 'project' configuration element cannot be empty"
        elif isNotNull parts.["asyncReturnType"] && parts.["asyncReturnType"].Type <> JTokenType.String then
            Error "The 'asyncReturnType' configuration element must be a string"
        elif isNotNull parts.["asyncReturnType"] && parts.["asyncReturnType"].ToObject<string>().ToLower().Trim() <> "task" && parts.["asyncReturnType"].ToObject<string>().ToLower().Trim() <> "async" then
            Error "The 'asyncReturnType' configuration element can only be 'async' (default) or 'task'"
        elif isNotNull parts.["synchronous"] && parts.["synchronous"].Type <> JTokenType.Boolean then
            Error "The 'synchronous' configuration element must be a boolean"
        elif isNotNull parts.["resolveReferences"] && parts.["resolveReferences"].Type <> JTokenType.Boolean then
            Error "The 'resolveReferences' configuration element must be a boolean"
        else
            Ok {
                schema = parts.["schema"].ToObject<string>()
                output = resolveFile (parts.["output"].ToObject<string>())
                project = parts.["project"].ToString().Replace("(", "").Replace(")", "")
                target =
                    if isNotNull parts.["target"] && parts.["target"].ToString() = "fable"
                    then Target.Fable
                    else Target.FSharp
                asyncReturnType =
                    if isNotNull parts.["asyncReturnType"] && parts.["asyncReturnType"].ToString() = "task"
                    then AsyncReturnType.Task
                    else AsyncReturnType.Async
                synchronous =
                    if isNotNull parts.["synchronous"]
                    then parts.["synchronous"].ToObject<bool>()
                    else false
                resolveReferences =
                    if isNotNull parts.["resolveReferences"]
                    then parts.["resolveReferences"].ToObject<bool>()
                    else false
            }
    with
    | error ->
        Error $"Error ocurred while reading the configuration file: {error.Message}"

let xmlDocs (description: string) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map (fun line -> line.Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;"))
        |> PreXmlDoc.Create

let xmlDocsWithParams (description: string) (parameters: (string * string) seq) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map (fun line -> line.Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;"))
        |> fun summary ->
            let containsParamDocs =
                parameters
                |> Seq.map snd
                |> Seq.exists (fun docs -> not (String.IsNullOrWhiteSpace docs))
            PreXmlDoc.Create [
                yield "<summary>"
                yield! summary
                yield "</summary>"
                if containsParamDocs then
                    for (param, paramDocs) in parameters do
                        if not (String.IsNullOrWhiteSpace paramDocs) then
                            let docs =
                                paramDocs.Split "\r\n"
                                |> Seq.collect (fun line -> line.Split("\n"))
                                |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
                                |> Seq.map (fun line -> line.Replace(">", "&gt;").Replace("<", "&lt;"))

                            if Seq.length docs = 1 then
                                yield $"<param name=\"{param}\">{Seq.head docs}</param>"
                            else
                                yield $"<param name=\"{param}\">"
                                yield! docs
                                yield "</param>"
                        else
                            yield $"<param name=\"{param}\"></param>"
            ]

let client = new HttpClient()
let getSchema(schema: string) =
    if File.Exists schema
    then new FileStream(schema, FileMode.Open) :> Stream
    elif schema.StartsWith "http"
    then
        client.GetStreamAsync(schema)
        |> Async.AwaitTask
        |> Async.RunSynchronously
    else
        // assume the schema is coming in as a string
        // convert it into a memory stream
        // this is useful for unit tests
        let schemaBytes = System.Text.Encoding.UTF8.GetBytes schema
        new MemoryStream(schemaBytes) :> Stream

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

let nextTick (name: string) (visited: ResizeArray<string>) =
    if not (visited.Contains name) then
        name
    else
    visited
    |> Seq.toList
    |> List.filter (fun visitedName -> visitedName.StartsWith name)
    |> List.map (fun visitedName -> visitedName.Replace(name, ""))
    |> List.choose(fun rest ->
        match Int32.TryParse rest with
        | true, n -> Some n
        | _ -> None)
    |> function
        | [ ] -> name + "1"
        | ns -> name + (string (List.max ns + 1))

let findNextTypeName fieldName objectName (selections: string list) (visitedTypes: ResizeArray<string>) (isGlobalRef: bool) =
    let nestedSelectionType =
        selections
        |> List.map capitalize
        |> String.concat "And"

    if not (visitedTypes.Contains objectName) && not isGlobalRef then
        objectName
    elif not (visitedTypes.Contains (capitalize fieldName)) && not isGlobalRef then
        capitalize fieldName
    elif not (visitedTypes.Contains (objectName + capitalize fieldName)) && not isGlobalRef then
        objectName + capitalize fieldName
    elif not (visitedTypes.Contains nestedSelectionType) && selections.Length <= 3 && selections.Length > 1 then
        nestedSelectionType
    elif not (visitedTypes.Contains (capitalize fieldName + "From" + objectName)) then
        capitalize fieldName + "From" + objectName
    else
        nextTick (capitalize fieldName + "From" + objectName) visitedTypes

let findNextEnumTypeName (fieldName: string) objectName (visitedTypes: ResizeArray<string>) =
    let fieldName = 
        if fieldName.Contains "." then
            fieldName.Split('.')
            |> Array.map capitalize
            |> String.concat ""
        elif fieldName.Contains " " then 
            fieldName.Split(' ')
            |> Array.map capitalize
            |> String.concat ""
        else 
            fieldName

    if not (visitedTypes.Contains (capitalize fieldName)) then
        capitalize fieldName
    elif not (visitedTypes.Contains (objectName + capitalize fieldName)) then
        objectName + capitalize fieldName
    elif not (visitedTypes.Contains (capitalize fieldName + "From" + objectName)) then
        capitalize fieldName + "From" + objectName
    else
        nextTick (capitalize fieldName + "From" + objectName) visitedTypes

let isEnumType (schema: OpenApiSchema) =
    (schema.Type = "string" || schema.Type = "integer")
    && not (isNull schema.Enum)
    && schema.Enum.Count > 0

let (|StringEnum|_|) (schema: OpenApiSchema) =
    if isEnumType schema then
        let cases =
            schema.Enum
            |> Seq.choose (fun enumCase ->
                match enumCase with
                | :? Microsoft.OpenApi.Any.OpenApiString as primitiveValue -> Some primitiveValue.Value
                | _ -> None)

        if not (Seq.isEmpty cases) then
            Some (Seq.toList cases)
        else
            None
    else
        None

let deriveOperationName (operationName: string) (path: string) (operationType: OperationType) =
    if not (String.IsNullOrWhiteSpace operationName) then
        if operationName.Contains "-" then
            operationName.Split('-', StringSplitOptions.RemoveEmptyEntries)
            |> Array.map capitalize
            |> String.concat ""
        else
            operationName
    else
        let parts = path.Split("/")
        let parameters =
            parts
            |> Array.filter (fun part -> part.StartsWith "{" && part.EndsWith "}")
            |> Array.map (fun part -> part.Replace("{", "").Replace("}", ""))
            |> String.concat "And"

        let segments =
            parts
            |> Array.filter (fun part -> not (part.StartsWith "{" && part.EndsWith "}"))
            |> Array.mapi (fun index part -> if index <> 0 then capitalize part else part)
            |> String.concat ""

        if String.IsNullOrEmpty parameters then
            string operationType + segments
        else
            string operationType + segments + "By" + parameters

let (|IntEnum|_|) (typeName: string) (schema: OpenApiSchema) =
    if isEnumType schema then
        let cases =
            schema.Enum
            |> Seq.choose (fun enumCase ->
                match enumCase with
                | :? Microsoft.OpenApi.Any.OpenApiInteger as primitiveValue -> Some primitiveValue.Value
                | _ -> None)

        let caseNames =
            if not (isNull schema.Extensions) && schema.Extensions.ContainsKey "x-enumNames" then
                let enumNames = schema.Extensions.["x-enumNames"]
                match enumNames with
                | :? Microsoft.OpenApi.Any.OpenApiArray as namesArray ->
                    namesArray
                    |> Seq.choose (function
                        | :? Microsoft.OpenApi.Any.OpenApiString as enumName -> Some enumName.Value
                        | _ -> None)
                    |> Seq.toList
                | _ ->
                    []
            else
                cases
                |> Seq.map (fun caseValue -> typeName + string caseValue)
                |> Seq.toList

        if not (Seq.isEmpty cases) && not (Seq.isEmpty caseNames) && Seq.length cases = Seq.length caseNames then
            cases
            |> Seq.zip caseNames
            |> Some
        else
            None
    else
        None

let rec createFieldType recordName required (propertyName: string) (propertySchema: OpenApiSchema) =
    if not required then
        let optionalType : SynType = createFieldType recordName true propertyName propertySchema
        SynType.Option(optionalType)
    else
        match propertySchema.Type with
        | "integer" when propertySchema.Format = "int64" -> SynType.Int64()
        | "integer" -> SynType.Int()
        | "number" when propertySchema.Format = "float" -> SynType.Float32()
        | "number" ->  SynType.Double()
        | "boolean" -> SynType.Bool()
        | "string" when propertySchema.Format = "uuid" -> SynType.Guid()
        | "string" when propertySchema.Format = "guid" -> SynType.Guid()
        | "string" when propertySchema.Format = "date-time" -> SynType.DateTimeOffset()
        | "string" when propertySchema.Format = "byte" ->
            // base64 encoded characters
            SynType.ByteArray()
        | "array" ->
            let arrayItemsType = createFieldType recordName required propertyName propertySchema.Items
            SynType.List(arrayItemsType)
        | _ when not (isNull propertySchema.Reference) ->
            // working with a reference type
            let typeName =
                if String.IsNullOrEmpty propertySchema.Title
                then propertySchema.Reference.Id
                else propertySchema.Title
            SynType.Create typeName
        | _ ->
            SynType.String()

let compiledName (name: string) = SynAttribute.CompiledName name

let createEnumType (enumName: string) (values: seq<string>) (config: CodegenConfig) =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [
            SynAttributeList.Create [
                SynAttribute.Create [ "Fable";"Core"; "StringEnum" ]
                SynAttribute.RequireQualifiedAccess()
            ]
        ]
        Id = [ Ident.Create enumName ]
        XmlDoc = PreXmlDoc.Empty
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let cleanEnumValue (case: string) = 
        if String.IsNullOrWhiteSpace case then 
            "EmptyString"
        else
        let parts = case.Split([| '/'; '.'; ','; '['; '-'; ']' |], StringSplitOptions.RemoveEmptyEntries)
        if parts.Length > 1 then 
            parts
            |> Array.map capitalize
            |> String.concat ""
        else
            capitalize case

    let distinctValues = 
        values
        |> Seq.distinctBy (fun value -> cleanEnumValue value)

    let enumRepresentation = SynTypeDefnSimpleReprUnionRcd.Create([
        for value in distinctValues ->
            let attrs = [ SynAttributeList.Create [| compiledName value  |] ]
            let docs = PreXmlDoc.Empty
            SynUnionCase.UnionCase(attrs, Ident.Create (cleanEnumValue value), SynUnionCaseType.UnionCaseFields [], docs, None, range0)
    ])

    let simpleType = SynTypeDefnSimpleReprRcd.Union(enumRepresentation)

    let members : SynMemberDefn list = [
        let unitConst : SynPatConstRcd = {
            Const = SynConst.Unit
            Range = range0
        }

        let matchClauses = [
            for value in distinctValues ->
                let id = LongIdentWithDots.CreateString (cleanEnumValue value)
                let matchedValue = SynPat.LongIdent(id, None, None, SynArgPats.Empty, None, range0)
                let result = SynExpr.CreateConstString value
                SynMatchClause.Clause(matchedValue, None, result, range0, DebugPointForTarget.No)
        ]

        SynMemberDefn.CreateMember
            { SynBindingRcd.Null with
                Pattern = SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString "this.Format", [SynPatRcd.Const unitConst])
                Expr = SynExpr.CreateMatch(SynExpr.Ident(Ident.Create "this"), matchClauses)
            }
    ]

    SynModuleDecl.CreateSimpleType(info, simpleType, members)

let rec getFieldType (schema: OpenApiSchema) =
    match schema.Type with
    | "integer" when schema.Format = "int64" -> SynType.Int64()
    | "integer" -> SynType.Int()
    | "number" when schema.Format = "float" -> SynType.Float32()
    | "number" ->  SynType.Double()
    | "boolean" -> SynType.Bool()
    | "string" when schema.Format = "uuid" -> SynType.Guid()
    | "string" when schema.Format = "guid" -> SynType.Guid()
    | "string" when schema.Format = "date-time" -> SynType.DateTimeOffset()
    | "string" when schema.Format = "byte" ->
        // base64 encoded characters
        SynType.ByteArray()
    | "file" ->
        SynType.ByteArray()
    | "array" ->
        let elementSchema = schema.Items
        let elementType = getFieldType elementSchema
        SynType.List elementType
    | _ when not (isNull schema.Reference) ->
        // working with a reference type
        let typeName =
            if String.IsNullOrEmpty schema.Title
            then schema.Reference.Id
            else schema.Title
        SynType.Create typeName
    | _ when schema.AdditionalPropertiesAllowed && not (isNull schema.AdditionalProperties) ->
        let valueType = getFieldType schema.AdditionalProperties
        let keyType = SynType.String()
        SynType.Map(keyType, valueType)
    | _ ->
        SynType.String()

let statusCode = function
    | "200" -> Some (nameof HttpStatusCode.OK)
    | "201" -> Some (nameof HttpStatusCode.Created)
    | "204" -> Some (nameof HttpStatusCode.NoContent)
    | "404" -> Some (nameof HttpStatusCode.NotFound)
    | "400" -> Some (nameof HttpStatusCode.BadRequest)
    | "401" -> Some (nameof HttpStatusCode.Unauthorized)
    | "403" -> Some (nameof HttpStatusCode.Forbidden)
    | "405" -> Some (nameof HttpStatusCode.MethodNotAllowed)
    | "500" -> Some (nameof HttpStatusCode.InternalServerError)
    | "default" -> Some "DefaultResponse"
    | _ -> None

let isEmptySchema (schema: OpenApiSchema) = 
    isNull schema.Type 
    && schema.Properties.Count = 0 
    && schema.AllOf.Count = 0
    && schema.AnyOf.Count = 0

let createResponseType (operation: OpenApiOperation) (path: string) (operationType: OperationType) =

    let typeName = deriveOperationName (capitalize operation.OperationId) path operationType
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [
            SynAttributeList.Create [
                SynAttribute.RequireQualifiedAccess()
            ]
        ]
        Id = [ Ident.Create typeName ]
        XmlDoc = PreXmlDoc.Empty
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let containsOkOrDefault =
        operation.Responses.ContainsKey "200"
        || operation.Responses.ContainsKey "201"
        || operation.Responses.ContainsKey "204"
        || operation.Responses.ContainsKey "202"
        || operation.Responses.ContainsKey "default"

    let enumRepresentation = SynTypeDefnSimpleReprUnionRcd.Create([
        for response in operation.Responses do
            match statusCode response.Key with
            | Some caseName ->
                let fieldTypes =
                    if response.Value.Content.ContainsKey "application/json" then
                        let responsePayloadType = response.Value.Content.["application/json"]
                        if not (isNull responsePayloadType.Schema) && not (isEmptySchema responsePayloadType.Schema) then
                            let fieldType = getFieldType responsePayloadType.Schema
                            [SynFieldRcd.Create("payload", fieldType).FromRcd]
                        else
                            []
                    elif response.Value.Content.ContainsKey "application/octet-stream" then
                        let fieldType = SynType.ByteArray()
                        [SynFieldRcd.Create("payload", fieldType).FromRcd]
                    else
                        []
                let docs = PreXmlDoc.Create response.Value.Description
                yield SynUnionCase.UnionCase([], Ident.Create (capitalize caseName), SynUnionCaseType.UnionCaseFields fieldTypes, docs, None, range0)
            | None ->
                ()

        if not containsOkOrDefault then
            let docs = PreXmlDoc.Empty
            yield SynUnionCase.UnionCase([], Ident.Create (capitalize "DefaultResponse"), SynUnionCaseType.UnionCaseFields [], docs, None, range0)
    ])

    let simpleType = SynTypeDefnSimpleReprRcd.Union(enumRepresentation)

    SynModuleDecl.CreateSimpleType(info, simpleType, [])

let createFlagsEnum (enumName: string) (values: seq<string * int>) =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [
            SynAttributeList.Create [
                SynAttribute.RequireQualifiedAccess()
            ]
        ]
        Id = [ Ident.Create enumName ]
        XmlDoc = PreXmlDoc.Empty
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let enumRepresentation = SynTypeDefnSimpleReprEnumRcd.Create([
        for (enumName, enumValue) in values ->
            let attrs = []
            let docs = PreXmlDoc.Empty
            SynEnumCaseRcd.Create(Ident.Create (capitalize enumName), SynConst.Int32 enumValue)
    ])

    let simpleType = SynTypeDefnSimpleReprRcd.Enum enumRepresentation

    SynModuleDecl.CreateSimpleType(info, simpleType)

let sanitizeTypeName (typeName: string) =
    if typeName.Contains "`" then
        match typeName.Split '`' with
        | [| name; typeArgArity |] -> name
        | _ -> typeName.Replace("`", "")
    else
        typeName

/// Creates a declaration: type {typeName} = {aliasedType}
///
/// This is used when there are global schema components that map to primitive types
let createTypeAbbreviation (abbreviation: string) (aliasedType: SynType) =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create abbreviation ]
        XmlDoc = PreXmlDoc.Empty
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let typeAbbrev = SynTypeDefnSimpleRepr.TypeAbbrev(ParserDetail.Ok, aliasedType, range0)
    let typeRepr = SynTypeDefnRepr.Simple(typeAbbrev, range0)
    let typeInfo = SynTypeDefn.TypeDefn(info.FromRcd, typeRepr, [], range0)
    SynModuleDecl.Types ([ typeInfo ], range0)

let isGlobalRef (name: string) (openApiDocument: OpenApiDocument) = 
    let schemas = 
        if isNotNull openApiDocument.Components
        then List.ofSeq (openApiDocument.Components.Schemas)
        else []

    let isGlobal = 
        schemas
        |> List.exists (fun pair -> 
            let typeName = pair.Key
            let isRef = isNotNull pair.Value.Reference
            isRef && (name = typeName || name = pair.Value.Title || name = pair.Value.Reference.Id)
        )

    isGlobal

let rec createRecordFromSchema (recordName: string) (schema: OpenApiSchema) (visitedTypes: ResizeArray<string>) (config: CodegenConfig) (openApiDocument: OpenApiDocument) : SynModuleDecl list =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create (sanitizeTypeName recordName) ]
        XmlDoc = xmlDocs schema.Description
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let nestedObjects = ResizeArray<SynModuleDecl>()
    let recordFields = ResizeArray<SynFieldRcd>()
    let addedFields = ResizeArray<string * bool * SynType>()

    let rec createPropertyType (propertyName: string) (propertyType: OpenApiSchema) =
        let isEnum = isEnumType propertyType
        let required = propertyName = "additionalProperties" || schema.Required.Contains propertyName
        let isObjectArray =
            propertyType.Type = "array"
            && propertyType.Items.Type = "object"
            && isNull propertyType.Items.Reference
            && propertyType.Items.Properties.Count > 0

        let isAdditionalProperties =
            propertyType.AdditionalPropertiesAllowed
            && not (isNull propertyType.AdditionalProperties)

        let isEnumArray = propertyType.Type = "array" && isEnumType propertyType.Items

        let isEmptyObjectDefinition =
            propertyType.Type = "object"
            && propertyType.Properties.Count = 0
            && isNull propertyType.Reference
            && (isNull propertyType.AllOf || propertyType.AllOf.Count = 0)
            && (isNull propertyType.AnyOf || propertyType.AnyOf.Count = 0)

        let isKeyValuePairObject =
            propertyType.Type = "object"
            && propertyType.Title = "KeyValuePair`2"
            && propertyType.Properties.Count = 2
            && propertyType.Properties.ContainsKey "Key"
            && propertyType.Properties.ContainsKey "Value"

        let isArrayOfKeyValuePairObject =
            propertyType.Type = "array"
            && propertyType.Items.Type = "object"
            && propertyType.Items.Title = "KeyValuePair`2"
            && propertyType.Items.Properties.Count = 2
            && propertyType.Items.Properties.ContainsKey "Key"
            && propertyType.Items.Properties.ContainsKey "Value"

        let isArrayOfEmptyObject =
            propertyType.Type = "array"
            && propertyType.Items.Type = "object"
            && propertyType.Items.Properties.Count = 0
            && isNull propertyType.Items.Reference
            && (isNull propertyType.Items.AllOf || propertyType.Items.AllOf.Count = 0)
            && (isNull propertyType.Items.AnyOf || propertyType.Items.AnyOf.Count = 0)

        let isPrimitve = List.forall id [
            (propertyType.Type <> "object" || not (isNull propertyType.Reference))
            not isEnum
            not isObjectArray
            not isEnumArray
            not isEmptyObjectDefinition
            not isKeyValuePairObject
            not isArrayOfKeyValuePairObject
            not isArrayOfEmptyObject
        ]

        if propertyType.Deprecated then
            // skip deprecated propertie
            None
        elif isAdditionalProperties then
            let fieldType = createFieldType recordName true propertyName propertyType.AdditionalProperties
            if required
            then Some (SynType.Map(SynType.String(), fieldType))
            else Some (SynType.Option(SynType.Map(SynType.String(), fieldType)))
        elif isPrimitve then
            let fieldType = createFieldType recordName required propertyName propertyType
            Some fieldType
        else if isEnum && isNull propertyType.Reference then
            // nested enum -> not a reference to a global usable enum
            let enumTypeName = findNextEnumTypeName propertyName recordName visitedTypes
            match propertyType with
            | StringEnum cases ->
                visitedTypes.Add enumTypeName
                let createdEnumType = createEnumType enumTypeName cases config
                nestedObjects.Add createdEnumType
                let fieldType =
                    if required
                    then SynType.Create enumTypeName
                    else SynType.Option(SynType.Create enumTypeName)
                Some fieldType
            | IntEnum enumTypeName cases ->
                visitedTypes.Add enumTypeName
                let createdEnumType = createFlagsEnum enumTypeName cases
                nestedObjects.Add createdEnumType
                let fieldType =
                    if required
                    then SynType.Create enumTypeName
                    else SynType.Option(SynType.Create enumTypeName)
                Some fieldType
            | _ ->
                None
        else if isEnum && not (isNull propertyType.Reference) then
            // referenced enum
            let typeName =
                if String.IsNullOrEmpty propertyType.Title
                then propertyType.Reference.Id
                else propertyType.Title
            let fieldType =
                if required
                then SynType.Create typeName
                else SynType.Option(SynType.Create typeName)
            Some fieldType
        else if isEmptyObjectDefinition then
            // empty object definition
            let fieldType =
                if required
                then
                    if config.target = Target.FSharp
                    then SynType.JObject()
                    else SynType.Object()

                else
                    if config.target = Target.FSharp
                    then SynType.Option(SynType.JObject())
                    else SynType.Option(SynType.Object())
            Some fieldType
        else if isKeyValuePairObject then
            let keySchema = propertyType.Properties.["Key"]
            let valueSchema = propertyType.Properties.["Value"]
            match createPropertyType (propertyName + "Key") keySchema, createPropertyType (propertyName + "Value") valueSchema with
            | Some keyType, Some valueType ->
                let pairType = SynType.KeyValuePair(keyType, valueType)
                let fieldType =
                    if required
                    then pairType
                    else SynType.Option(pairType)
                Some fieldType
            | _ ->
                None
        else if propertyType.Type = "object" then
            // handle nested objects
            let nestedPropertyNames =
                propertyType.Properties
                |> Seq.map (fun pair -> pair.Key)
                |> Seq.toList

            let isGlobal = isGlobalRef (capitalize propertyName) openApiDocument
            let nestedObjectTypeName = findNextTypeName propertyName recordName nestedPropertyNames visitedTypes isGlobal
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName propertyType visitedTypes config openApiDocument
            nestedObjects.AddRange nestedObject
            let fieldType =
                if required
                then SynType.Create nestedObjectTypeName
                else SynType.Option(SynType.Create nestedObjectTypeName)
            Some fieldType
        else if isObjectArray then
             // handle arrays of nested objects
            let arrayItemsType = propertyType.Items
            let nestedPropertyNames =
                arrayItemsType.Properties
                |> Seq.map (fun pair -> pair.Key)
                |> Seq.toList

            let isGlobal = isGlobalRef (capitalize propertyName) openApiDocument
            let nestedObjectTypeName = findNextTypeName propertyName recordName nestedPropertyNames visitedTypes isGlobal
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName arrayItemsType visitedTypes config openApiDocument
            nestedObjects.AddRange nestedObject
            let fieldType =
                if required
                then SynType.List(SynType.Create nestedObjectTypeName)
                else SynType.Option(SynType.List(SynType.Create nestedObjectTypeName))
            Some fieldType
        else if isEnumArray then
            let arrayItemsType = propertyType.Items
            if isNull arrayItemsType.Reference then
                // nested enum type -> not a global reference
                let enumTypeName = findNextEnumTypeName propertyName recordName visitedTypes
                match arrayItemsType with
                | StringEnum cases ->
                    visitedTypes.Add enumTypeName
                    let createdEnumType = createEnumType enumTypeName cases config
                    nestedObjects.Add createdEnumType
                    let fieldType =
                        if required
                        then SynType.List(SynType.Create enumTypeName)
                        else SynType.Option(SynType.List(SynType.Create enumTypeName))
                    Some fieldType
                | IntEnum enumTypeName cases ->
                    visitedTypes.Add enumTypeName
                    let createdEnumType = createFlagsEnum enumTypeName cases
                    nestedObjects.Add createdEnumType
                    let fieldType =
                        if required
                        then SynType.List(SynType.Create enumTypeName)
                        else SynType.Option(SynType.List(SynType.Create enumTypeName))
                    Some fieldType
                | _ ->
                    None
            else
                // referenced enum type
                let typeName =
                    if String.IsNullOrEmpty propertyType.Title
                    then sanitizeTypeName propertyType.Reference.Id
                    else sanitizeTypeName propertyType.Title

                let fieldType =
                    if required
                    then SynType.List(SynType.Create typeName)
                    else SynType.Option(SynType.List(SynType.Create typeName))
                Some fieldType
        elif isArrayOfKeyValuePairObject then
            let keySchema = propertyType.Items.Properties.["Key"]
            let valueSchema = propertyType.Items.Properties.["Value"]
            match createPropertyType (propertyName + "Key") keySchema, createPropertyType (propertyName + "Value") valueSchema with
            | Some keyType, Some valueType ->
                let pairType = SynType.KeyValuePair(keyType, valueType)
                let fieldType =
                    if required
                    then SynType.List(pairType)
                    else SynType.Option(SynType.List(pairType))
                Some fieldType
            | _ ->
                None
        elif isArrayOfEmptyObject then
            let fieldType =
                if required
                then
                    if config.target = Target.FSharp
                    then SynType.CreateLongIdent "Newtonsoft.Json.Linq.JArray"
                    else SynType.ResizeArray(SynType.Create "obj")

                else
                    if config.target = Target.FSharp
                    then SynType.Option (SynType.CreateLongIdent "Newtonsoft.Json.Linq.JArray")
                    else SynType.Option(SynType.ResizeArray(SynType.Create "obj"))

            Some fieldType
        else
            None

    for property in schema.Properties do
        match createPropertyType property.Key property.Value with
        | None -> ()
        | Some fieldType ->
            let propertyName = property.Key
            let propertyType = property.Value
            let required = schema.Required.Contains propertyName
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = xmlDocs propertyType.Description
            recordFields.Add { field with XmlDoc = docs }
            addedFields.Add((propertyName, required, fieldType))

    let rec handleAllOf (currentSchema: OpenApiSchema) =
        if not (isNull currentSchema.AllOf) then
            for innerSchema in currentSchema.AllOf do
                if innerSchema.Type = "object" then
                    for property in innerSchema.Properties do
                        match createPropertyType property.Key property.Value with
                        | None -> ()
                        | Some fieldType ->
                            let propertyName = property.Key
                            let propertyType = property.Value
                            let required = schema.Required.Contains propertyName
                            let field = SynFieldRcd.Create(propertyName, fieldType)
                            let docs = xmlDocs propertyType.Description
                            recordFields.Add { field with XmlDoc = docs }
                            addedFields.Add((propertyName, required, fieldType))
                else if isNull innerSchema.Type && not (isNull innerSchema.AllOf) && innerSchema.AllOf.Count > 0 then
                    // handle recursice allOf references
                    handleAllOf innerSchema
                else
                    ()

    handleAllOf schema

    let containsPreservedProperty =
        schema.Properties.Any(fun prop -> prop.Key = "additionalProperties")

    let includeAdditionalProperties =
        schema.AdditionalPropertiesAllowed
        && not (isNull schema.AdditionalProperties)
        && not containsPreservedProperty

    if includeAdditionalProperties then
        // when there are additional properties
        // and fixed properties while targeting fable
        // then ignore the fixed properties and only get the dictionary type
        // when only additional properties are present
        // then create type abbreviation
        let isFreeForm = isNull schema.AdditionalProperties.Type
        match createPropertyType "additionalProperties" schema.AdditionalProperties with
        | None -> [ ]
        | Some additionalType ->
            let valueType =
                if isFreeForm && config.target = Target.FSharp
                then SynType.JToken()
                elif isFreeForm && config.target = Target.Fable
                then SynType.Object()
                else additionalType
            let dictionaryType = SynType.Map(SynType.String(), valueType)
            [ createTypeAbbreviation recordName dictionaryType ]
    else

        let recordRepr = SynTypeDefnSimpleReprRecordRcd.Create (List.ofSeq recordFields)
        let simpleRecordType = SynTypeDefnSimpleReprRcd.Record recordRepr

        let members : SynMemberDefn list = [
            SynMemberDefn.CreateStaticMember
                {
                    SynBindingRcd.Null with
                        XmlDoc = PreXmlDoc.Create $"Creates an instance of {recordName} with all optional fields initialized to None. The required fields are parameters of this function"
                        Pattern =
                            SynPatRcd.Typed {
                                Type = SynType.Create recordName
                                Range = range0
                                Pattern =
                                    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString "Create", [
                                        SynPatRcd.CreateParen(
                                            SynPatRcd.Tuple {
                                                Patterns = [
                                                    for (fieldName, required, fieldType) in addedFields do
                                                        if fieldName = "additionalProperties" && not containsPreservedProperty then
                                                            ()
                                                        else
                                                            if required then yield SynPatRcd.Typed {
                                                                Type = fieldType
                                                                Pattern = SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fieldName, [])
                                                                Range = range0
                                                            }
                                                ]
                                                Range  = range0
                                            }
                                        )
                                    ])
                            }

                        // create a record with the required fields
                        Expr = SynExpr.CreateRecord [
                            for (fieldName, required, fieldType) in addedFields do
                                let expr =
                                    if fieldName = "additionalProperties" && not containsPreservedProperty
                                    then Some(SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "Map.empty"))
                                    elif required
                                    then Some(SynExpr.Ident(Ident.Create fieldName))
                                    else Some(SynExpr.Ident(Ident.Create "None"))
                                ((LongIdentWithDots.CreateString fieldName, false), expr)
                        ]
                }
        ]

        let anyFieldHasDots = 
            addedFields
            |> Seq.exists (fun (fieldName, _, _) -> fieldName.Contains "." || fieldName.Contains "/")

        // when fields have dots, they are not escaped for some reason
        // TODO: fix it later in fantomas
        // right now, we just won't generate the `Create` function
        let eventualMembers = if anyFieldHasDots then [] else members

        [
            yield! nestedObjects
            SynModuleDecl.CreateSimpleType(info, simpleRecordType, eventualMembers)
        ]

// type KeyValuePair<'TKey, 'TValue> = { Key: 'TKey, Value: 'TValue }
let createKeyValuePair() =
    let keyTypeArg = SynTypar.Typar(Ident.Create "TKey", TyparStaticReq.NoStaticReq, false)
    let valueTypeArg = SynTypar.Typar(Ident.Create "TValue", TyparStaticReq.NoStaticReq, false)

    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create "KeyValuePair" ]
        XmlDoc = PreXmlDoc.Empty
        Parameters = [
            SynTyparDecl.TyparDecl([], keyTypeArg)
            SynTyparDecl.TyparDecl([], valueTypeArg)
        ]
        Constraints = [ ]
        PreferPostfix = true
        Range = range0
    }

    let recordRepr = SynTypeDefnSimpleReprRecordRcd.Create [
        SynFieldRcd.Create("Key", SynType.Var(keyTypeArg, range0))
        SynFieldRcd.Create("Value", SynType.Var(valueTypeArg, range0))
    ]

    let simpleRecordType = SynTypeDefnSimpleReprRcd.Record recordRepr

    SynModuleDecl.CreateSimpleType(info, simpleRecordType)



let createGlobalTypesModule (openApiDocument: OpenApiDocument) (config: CodegenConfig) =
    let visitedTypes = ResizeArray<string>()
    let moduleTypes = ResizeArray<SynModuleDecl>()



    if isNotNull openApiDocument.Components then 

        // first add all global enum types
        for topLevelObject in openApiDocument.Components.Schemas do
            let typeName =
                if String.IsNullOrEmpty topLevelObject.Value.Title
                then sanitizeTypeName topLevelObject.Key
                else sanitizeTypeName topLevelObject.Value.Title

            if topLevelObject.Value.Type = "string" then
                match topLevelObject.Value with
                | StringEnum cases ->
                    // create global enum type
                    moduleTypes.Add (createEnumType typeName cases config)
                    visitedTypes.Add typeName
                | _ ->
                    // create abbreviated type
                    let abbreviatedType =
                        match topLevelObject.Value.Format with
                        | "guid" | "uuid" -> SynType.Guid()
                        | "date-time" -> SynType.DateTimeOffset()
                        | "byte" -> SynType.ByteArray()
                        | _ -> SynType.String()

                    moduleTypes.Add (createTypeAbbreviation typeName abbreviatedType)
                    visitedTypes.Add typeName
            elif topLevelObject.Value.Type = "integer" then
                match topLevelObject.Value with
                | IntEnum typeName cases ->
                    // create global enum type
                    moduleTypes.Add(createFlagsEnum typeName cases)
                    visitedTypes.Add typeName
                | _ ->
                    // create type abbreviation
                    let abbreviatedType =
                        match topLevelObject.Value.Format with
                        | "int64" -> SynType.Int64()
                        | _ -> SynType.Int()
                    moduleTypes.Add (createTypeAbbreviation typeName abbreviatedType)
                    visitedTypes.Add typeName
            elif topLevelObject.Value.Type = "number" then
                // create type abbreviation
                let abbreviatedType =
                    match topLevelObject.Value.Format with
                    | "float" -> SynType.Float32()
                    | _ -> SynType.Double()
                moduleTypes.Add (createTypeAbbreviation typeName abbreviatedType)
                visitedTypes.Add typeName
            elif topLevelObject.Value.Type = "boolean" then
                // create type abbreviation
                moduleTypes.Add (createTypeAbbreviation typeName (SynType.Bool()))
                visitedTypes.Add typeName
            elif topLevelObject.Value.Type = "array" then
                let elementType = topLevelObject.Value.Items
                if not (isNull elementType.Reference) then
                    let referencedType =
                        if String.IsNullOrEmpty elementType.Title
                        then elementType.Reference.Id
                        else elementType.Title

                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.List(SynType.Create referencedType)))
                    visitedTypes.Add typeName
                elif elementType.Type = "string" then
                    match elementType with
                    | StringEnum cases ->
                        // create global enum type
                        let enumTypeName = $"EnumFor{typeName}";
                        moduleTypes.Add (createEnumType enumTypeName cases config)
                        let arrayOfEnum = SynType.List(SynType.Create enumTypeName)
                        moduleTypes.Add (createTypeAbbreviation typeName arrayOfEnum)

                        visitedTypes.Add enumTypeName
                        visitedTypes.Add typeName
                    | _ ->
                        // create abbreviated type
                        let abbreviatedType =
                            match topLevelObject.Value.Format with
                            | "guid" | "uuid" -> SynType.Guid()
                            | "date-time" -> SynType.DateTimeOffset()
                            | "byte" -> SynType.ByteArray()
                            | _ -> SynType.String()

                        let listOfAbbrev = SynType.List abbreviatedType

                        moduleTypes.Add (createTypeAbbreviation typeName listOfAbbrev)
                        visitedTypes.Add typeName
                elif elementType.Type = "integer" then
                    match elementType with
                    | IntEnum typeName cases ->
                        // create global enum type
                        let enumTypeName = $"EnumFor{typeName}";
                        moduleTypes.Add(createFlagsEnum typeName cases)
                        let arrayOfEnum = SynType.List(SynType.Create enumTypeName)
                        moduleTypes.Add (createTypeAbbreviation typeName arrayOfEnum)
                        visitedTypes.Add typeName
                    | _ ->
                        // create type abbreviation
                        let abbreviatedType =
                            match elementType.Format with
                            | "int64" -> SynType.List(SynType.Int64())
                            | _ -> SynType.List(SynType.Int())
                        moduleTypes.Add (createTypeAbbreviation typeName abbreviatedType)
                        visitedTypes.Add typeName
                elif elementType.Type = "number" then
                    // create type abbreviation
                    let abbreviatedType =
                        match elementType.Format with
                        | "float" -> SynType.List(SynType.Float32())
                        | _ -> SynType.List(SynType.Double())
                    moduleTypes.Add (createTypeAbbreviation typeName abbreviatedType)
                    visitedTypes.Add typeName
                elif elementType.Type = "boolean" then
                    // create type abbreviation
                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.List(SynType.Bool())))
                    visitedTypes.Add typeName
            else
                ()

        // then handle the global objects
        for topLevelObject in openApiDocument.Components.Schemas do
            let canUseTitle = 
                not (String.IsNullOrEmpty topLevelObject.Value.Title)
                && not (isGlobalRef topLevelObject.Value.Title openApiDocument)

            let typeName =
                if canUseTitle
                then sanitizeTypeName topLevelObject.Value.Title
                else sanitizeTypeName topLevelObject.Key

            let isAllOf =
                isNull topLevelObject.Value.Type
                && not (isNull topLevelObject.Value.AllOf)
                && topLevelObject.Value.AllOf.Count > 0

            let isKeyValuePairObject =
                topLevelObject.Value.Type = "object"
                && topLevelObject.Value.Title = "KeyValuePair`2"
                && topLevelObject.Value.Properties.Count = 2
                && topLevelObject.Value.Properties.ContainsKey "Key"
                && topLevelObject.Value.Properties.ContainsKey "Value"

            if topLevelObject.Value.Deprecated then
                // skip deprecated global types
                ()
            elif isKeyValuePairObject && not (visitedTypes.Contains "KeyValuePair") then
                // create specialized key value pair when encountering auto generated type
                // from .NET backend services that encode System.Collections.Generic.KeyValuePair
                moduleTypes.Add(createKeyValuePair())
                visitedTypes.Add "KeyValuePair"
            elif isKeyValuePairObject then
                // skip generating more key value pair type
                ()
            elif topLevelObject.Value.Type = "object" || isAllOf || (isNull topLevelObject.Value.Type && topLevelObject.Value.Properties.Count > 0) then
                visitedTypes.Add typeName
                for createdType in createRecordFromSchema typeName topLevelObject.Value visitedTypes config openApiDocument do
                    moduleTypes.Add createdType
            else
                ()

    for path in openApiDocument.Paths do
        for operation in path.Value.Operations do
            let responseType = createResponseType operation.Value path.Key operation.Key
            moduleTypes.Add responseType

    let globalTypesModule = CodeGen.createNamespace [ config.project; "Types" ] (Seq.toList moduleTypes)

    visitedTypes, globalTypesModule

type OperationParameter = {
    parameterName: string
    parameterIdent: string
    required: bool
    parameterType: SynType
    docs : string
    location: string
    style: string
}

let sanitizeParameterName (name: string) =
    if name.Contains "." then
        match name.Split "." with
        | [| ns; parameter |] -> parameter
        | _ -> name.Replace(".", "")
    else
        name

let paramReplace (parameter: string) (sep: char) = 
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

let cleanParamIdent (parameter: string) = 
    if parameter.Contains "-" then
        paramReplace parameter '-'
    elif parameter.Contains "_" then
        paramReplace parameter '-'
    elif parameter.Contains "." then 
        paramReplace parameter '.'
    else
        camelCase (parameter.Replace("$", "").Replace("@", "").Replace(":", ""))

let operationParameters (operation: OpenApiOperation) (visitedTypes: ResizeArray<string>) =
    let parameters = ResizeArray<OperationParameter>()
    let rec readParamType (schema: OpenApiSchema) =
        match schema.Type with
        | "integer" when schema.Format = "int64" -> SynType.Int64()
        | "integer" -> SynType.Int()
        | "number" when schema.Format = "float" -> SynType.Float32()
        | "number" ->  SynType.Double()
        | "boolean" -> SynType.Bool()
        | "string" when schema.Format = "uuid" -> SynType.Guid()
        | "string" when schema.Format = "guid" -> SynType.Guid()
        | "string" when schema.Format = "date-time" -> SynType.DateTimeOffset()
        | "string" when schema.Format = "byte" ->
            // base64 encoded characters
            SynType.ByteArray()
        | "file" ->
            SynType.ByteArray()
        | "array" ->
            let elementSchema = schema.Items
            let elementType = readParamType elementSchema
            SynType.List elementType
        | _ when not (isNull schema.Reference) ->
            // working with a reference type
            let typeName =
                if String.IsNullOrEmpty schema.Title
                then schema.Reference.Id
                else schema.Title
            SynType.Create typeName
        | _ ->
            SynType.String()

    for parameter in operation.Parameters do
        if not parameter.Deprecated && parameter.In.HasValue then
            let paramType = readParamType parameter.Schema
            parameters.Add {
                parameterName = sanitizeParameterName parameter.Name
                parameterIdent = cleanParamIdent (sanitizeParameterName parameter.Name)
                required = parameter.Required
                parameterType = paramType
                docs = parameter.Description
                location = (string parameter.In.Value).ToLower()
                style =
                    if parameter.Style.HasValue
                    then (string parameter.Style).ToLower()
                    else "none"
            }

    if not (isNull operation.RequestBody) then
        for pair in operation.RequestBody.Content do
            if pair.Key = "multipart/form-data" then
                for property in pair.Value.Schema.Properties do
                    parameters.Add {
                        parameterName = property.Key
                        parameterIdent = cleanParamIdent property.Key
                        required = pair.Value.Schema.Required.Contains property.Key
                        parameterType = readParamType property.Value
                        docs = property.Value.Description
                        location = "multipartFormData"
                        style = "formfield"
                    }

            if pair.Key = "application/json" && not (isEmptySchema pair.Value.Schema) then
                let schema = pair.Value.Schema
                let typeName = "body"
                let parameterName =
                    if operation.RequestBody.Extensions.ContainsKey "x-bodyName" then
                        match operation.RequestBody.Extensions.["x-bodyName"] with
                        | :? Microsoft.OpenApi.Any.OpenApiString as name -> name.Value
                        | _ -> camelCase (normalizeFullCaps typeName)
                    else
                        camelCase (normalizeFullCaps typeName)

                parameters.Add {
                    parameterName = parameterName
                    parameterIdent = cleanParamIdent parameterName
                    required = true
                    parameterType = readParamType schema
                    docs = schema.Description
                    location = "jsonContent"
                    style = "none"
                }

            let hasJsonContent = operation.RequestBody.Content.ContainsKey "application/json"

            if pair.Key = "application/x-www-form-urlencoded" then
                // make sure the form schema isn't the same as the JSON schema
                // use one or the other
                if hasJsonContent && pair.Value.Schema <> operation.RequestBody.Content.["application/json"].Schema && pair.Value.Schema.Type <> "object" then
                    for property in pair.Value.Schema.Properties do
                        parameters.Add {
                            parameterName = property.Key
                            parameterIdent = cleanParamIdent property.Key
                            required = pair.Value.Schema.Required.Contains property.Key
                            parameterType = readParamType property.Value
                            docs = property.Value.Description
                            location = "urlEncodedFormData"
                            style = "formfield"
                        }
            if pair.Key = "application/octet-stream" then
                parameters.Add {
                    parameterName = "requestBody"
                    parameterIdent = "requestBody"
                    required = false
                    parameterType = SynType.ByteArray()
                    docs = ""
                    location = "binaryContent"
                    style = "formfield"
                }

    parameters
    |> Seq.sortBy (fun param ->
        // required parameters come first
        if param.required
        then 0
        else 1
    )

let createOpenApiClient
    (openApiDocument: OpenApiDocument)
    (visitedTypes: ResizeArray<string>)
    (config: CodegenConfig) =

    let extraTypes = ResizeArray<SynModuleDecl>()

    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create $"{config.project}Client" ]
        XmlDoc = xmlDocs openApiDocument.Info.Description
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let clientMembers = ResizeArray<SynMemberDefn>()

    let httpClient = SynSimplePat.CreateTyped(Ident.Create "httpClient", SynType.Create "HttpClient")

    clientMembers.Add(SynMemberDefn.CreateImplicitCtor [ httpClient ])

    for path in openApiDocument.Paths do
        let fullPath = path.Key
        let pathInfo = path.Value
        for operation in pathInfo.Operations do
            let operationInfo = operation.Value
            if not operationInfo.Deprecated then
                let parameters = operationParameters operationInfo visitedTypes
                let summary =
                    if String.IsNullOrWhiteSpace operationInfo.Description
                    then operationInfo.Summary
                    else operationInfo.Description

                let parameterDocs = [
                    for p in parameters -> (p.parameterIdent, p.docs)
                ]

                let memberName = deriveOperationName operationInfo.OperationId fullPath operation.Key
                let createIdent xs = SynExpr.CreateLongIdent(LongIdentWithDots.Create xs)
                let stringExpr value = SynExpr.CreateConstString value
                let createLetAssignment leftSide rightSide continuation =
                    let emptySynValData = SynValData.SynValData(None, SynValInfo.Empty, None)
                    let headPat = SynPat.Named(SynPat.Wild range0, leftSide, false, None, range0)
                    let binding = SynBinding.Binding(None, SynBindingKind.NormalBinding, false, false, [], PreXmlDoc.Empty, emptySynValData, headPat, None, rightSide, range0, DebugPointForBinding.DebugPointAtBinding range0 )
                    SynExpr.LetOrUse(false, false, [binding], continuation, range0)

                // for async calls
                // creates let! (status, content) = {body} in {continuation}
                let deconstructAsyncResponse body continuation =
                    let status = SynPat.Named(SynPat.Wild range0, Ident.Create "status", false, None, range0)
                    let content = SynPat.Named(SynPat.Wild range0, Ident.Create "content", false, None, range0)
                    let headPat = SynPat.Paren(SynPat.Tuple(false, [ status; content ], range0), range0)
                    SynExpr.LetOrUseBang(DebugPointForBinding.DebugPointAtBinding range0, false, false, headPat, body, [], continuation, range0)

                // for synchronous calls
                // creates let (status, content) = {body} in {continuation}
                let deconstructResponse body continuation =
                    let emptySynValData = SynValData.SynValData(None, SynValInfo.Empty, None)
                    let status = SynPat.Named(SynPat.Wild range0, Ident.Create "status", false, None, range0)
                    let content = SynPat.Named(SynPat.Wild range0, Ident.Create "content", false, None, range0)
                    let headPat = SynPat.Paren(SynPat.Tuple(false, [ status; content ], range0), range0)
                    let binding = SynBinding.Binding(None, SynBindingKind.NormalBinding, false, false, [], PreXmlDoc.Empty, emptySynValData, headPat, None, body, range0, DebugPointForBinding.DebugPointAtBinding range0 )
                    SynExpr.LetOrUse(false, false, [binding], continuation, range0)

                let requestValues = [
                    for parameter in parameters do
                        if parameter.required then
                            yield SynExpr.CreatePartialApp(["RequestPart"; parameter.location], [
                                if parameter.location <> "jsonContent" && parameter.location <> "binaryContent" then
                                    SynExpr.CreateParen(SynExpr.CreateTuple [
                                        stringExpr parameter.parameterName
                                        createIdent [ parameter.parameterIdent ]
                                    ])
                                else
                                    createIdent [ parameter.parameterIdent ]
                            ])
                        else
                            let condition = createIdent [ parameter.parameterIdent; "IsSome" ]
                            let value =
                                SynExpr.CreatePartialApp([ "RequestPart"; parameter.location ], [
                                    if parameter.location <> "jsonContent" && parameter.location <> "binaryContent" then
                                        SynExpr.CreateParen(SynExpr.CreateTuple [
                                            stringExpr parameter.parameterName
                                            createIdent [ parameter.parameterIdent; "Value" ]
                                        ])
                                    else
                                        createIdent [ parameter.parameterIdent; "Value" ]
                                ])

                            yield SynExpr.CreateIfThen(condition, value)
                ]

                let httpFunction = operation.Key.ToString().ToLower()
                let httpFunctionAsync = $"{httpFunction}Async"
                let requestParts = Ident.Create "requestParts"
                let httpCall httpFunc =
                    SynExpr.CreatePartialApp(["OpenApiHttp"; httpFunc], [
                        SynExpr.CreateIdent (Ident.Create "httpClient")
                        SynExpr.CreateConstString fullPath
                        SynExpr.Ident requestParts
                    ])

                let wrappedReturn expr =
                    if config.synchronous
                    then expr
                    else SynExpr.CreateReturn expr

                let equal left right =
                    let innerApp = SynExpr.App(ExprAtomicFlag.NonAtomic, true, (createIdent ["op_Equality"]), left, range0)
                    SynExpr.CreateApp(innerApp, right)

                let responses =
                    operation.Value.Responses
                    |> Seq.choose (fun pair ->
                        match statusCode pair.Key with
                        | Some status -> Some (status, pair.Value)
                        | _ -> None
                    )
                    |> Seq.toList
                    |> List.groupBy fst
                    |> List.collect (fun (key, group) ->
                        if group.Length = 2 && key = "OK" then
                            let (_, response0) = group.[0]
                            let (_, response1) = group.[1]
                            if (response0.Content.Count = 0 && response1.Content.Count >= 1)
                            then [ group.[1] ]
                            else [ group.[0] ]
                        else
                            group
                    )

                let containsOkOrDefault =
                    responses
                    |> List.exists (fun (status, response) ->
                        status = "OK"
                        || status = "Created"
                        || status = "Accepted"
                        || status = "NoContent"
                        || status = "DefaultResponse")

                let responses =
                    if not containsOkOrDefault then
                        [
                            yield! responses
                            yield ("DefaultResponse", new OpenApiResponse(Content = new Dictionary<_,_>()))
                        ]
                    else
                        responses

                let responseType = capitalize memberName
                let returnExpr =
                    let createOutput (status: string,response: OpenApiResponse) =
                        if response.Content.ContainsKey "application/json" && not (isNull (response.Content.["application/json"].Schema)) && not (isEmptySchema response.Content.["application/json"].Schema) then
                            SynExpr.CreatePartialApp([responseType; status], [
                                SynExpr.CreateParen(
                                    SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                        createIdent [ "content" ]
                                    ])
                                )
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/octet-stream" then
                            SynExpr.CreatePartialApp([responseType; status], [
                                SynExpr.CreateParen(
                                    SynExpr.CreatePartialApp(["System"; "Text"; "Encoding"; "UTF8"; "GetBytes"], [
                                        createIdent [ "content" ]
                                    ])
                                )
                            ])
                            |> wrappedReturn
                        else
                            createIdent [ responseType; status ]
                            |> wrappedReturn

                    let statusIsEqual status =
                        equal (createIdent [ "status" ]) (createIdent [ "HttpStatusCode"; status ])

                    if responses.Length = 1 then
                        createOutput responses.[0]
                    elif responses.Length = 2 then
                        let (status1, response1) = responses.[0]
                        let (status2, response2) = responses.[1]
                        SynExpr.CreateIfThenElse(statusIsEqual status1, createOutput responses.[0], createOutput responses.[1])
                    elif responses.Length = 3 then
                        let (status1, response1) = responses.[0]
                        let (status2, response2) = responses.[1]
                        let (status3, response3) = responses.[2]
                        SynExpr.CreateIfThenElse(
                            statusIsEqual status1,
                            createOutput responses.[0],
                            SynExpr.CreateIfThenElse(
                                statusIsEqual status2,
                                createOutput responses.[1],
                                createOutput responses.[2]
                            )
                        )
                    elif responses.Length = 4 then
                        let (status1, response1) = responses.[0]
                        let (status2, response2) = responses.[1]
                        let (status3, response3) = responses.[2]
                        let (status4, response4) = responses.[3]
                        SynExpr.CreateIfThenElse(
                            statusIsEqual status1,
                            createOutput responses.[0],
                            SynExpr.CreateIfThenElse(
                                statusIsEqual status2,
                                createOutput responses.[1],
                                SynExpr.CreateIfThenElse(
                                    statusIsEqual status3,
                                    createOutput responses.[2],
                                    createOutput responses.[3]
                                )
                            )
                        )
                    elif responses.Length = 5 then
                        let (status1, response1) = responses.[0]
                        let (status2, response2) = responses.[1]
                        let (status3, response3) = responses.[2]
                        let (status4, response4) = responses.[3]
                        let (status5, response5) = responses.[4]
                        SynExpr.CreateIfThenElse(
                            statusIsEqual status1,
                            createOutput responses.[0],
                            SynExpr.CreateIfThenElse(
                                statusIsEqual status2,
                                createOutput responses.[1],
                                SynExpr.CreateIfThenElse(
                                    statusIsEqual status3,
                                    createOutput responses.[2],
                                    SynExpr.CreateIfThenElse(
                                        statusIsEqual status4,
                                        createOutput responses.[3],
                                        createOutput responses.[4]
                                    )
                                )
                            )
                        )
                    else
                        let (status1, response1) = responses.[0]
                        let (status2, response2) = responses.[1]
                        let (status3, response3) = responses.[2]
                        let (status4, response4) = responses.[3]
                        let (status5, response5) = responses.[4]
                        let (status6, response6) = responses.[5]
                        SynExpr.CreateIfThenElse(
                            statusIsEqual status1,
                            createOutput responses.[0],
                            SynExpr.CreateIfThenElse(
                                statusIsEqual status2,
                                createOutput responses.[1],
                                SynExpr.CreateIfThenElse(
                                    statusIsEqual status3,
                                    createOutput responses.[2],
                                    SynExpr.CreateIfThenElse(
                                        statusIsEqual status4,
                                        createOutput responses.[3],
                                        SynExpr.CreateIfThenElse(
                                            statusIsEqual status5,
                                            createOutput responses.[4],
                                            createOutput responses.[5]
                                        )
                                    )
                                )
                            )
                        )

                let asyncBuilder expr =
                    if config.synchronous then
                        expr
                    else
                        match config.asyncReturnType with
                        | AsyncReturnType.Async -> SynExpr.CreateAsync expr
                        | AsyncReturnType.Task -> SynExpr.CreateTask expr

                let destructExpr httpFunc =
                    if config.synchronous
                    then deconstructResponse (httpCall httpFunc) returnExpr
                    else deconstructAsyncResponse (httpCall httpFunc) returnExpr

                let clientOperation httpFunc name = SynMemberDefn.CreateMember {
                    SynBindingRcd.Null with
                        XmlDoc = xmlDocsWithParams summary parameterDocs
                        Expr =
                            asyncBuilder (
                                createLetAssignment
                                    requestParts
                                    (SynExpr.CreateList requestValues)
                                    (destructExpr httpFunc)
                            )

                        Pattern =
                            SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString $"this.{name}", [
                                SynPatRcd.CreateParen(
                                    SynPatRcd.Tuple {
                                        Patterns = [
                                            for parameter in parameters do
                                                SynPatRcd.Typed {
                                                    Range = range0
                                                    Type = parameter.parameterType
                                                    Pattern =
                                                        if parameter.required
                                                        then SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString(parameter.parameterIdent), [])
                                                        else SynPatRcd.OptionalVal {
                                                            Range = range0
                                                            Id = Ident.Create parameter.parameterIdent
                                                        }
                                                }
                                        ]
                                        Range  = range0
                                    }
                                )
                            ])
                }

                if config.synchronous then
                    clientMembers.Add (clientOperation httpFunction memberName)
                else
                    clientMembers.Add (clientOperation httpFunctionAsync memberName)

    let clientType = SynModuleDecl.CreateType(info, Seq.toList clientMembers)

    let moduleContents = [
        yield SynModuleDecl.CreateOpen "System.Net"
        yield SynModuleDecl.CreateOpen "System.Net.Http"
        yield SynModuleDecl.CreateOpen $"{config.project}.Types"
        yield SynModuleDecl.CreateOpen $"{config.project}.Http"
        if config.asyncReturnType = AsyncReturnType.Task then
            // from the Ply package
            yield SynModuleDecl.CreateOpen "FSharp.Control.Tasks"
        // extra types generated from parameters
        for extraType in extraTypes do
            yield extraType
        // the main http client
        yield clientType
    ]

    let clientModule = CodeGen.createNamespace [ config.project ] moduleContents
    clientModule

let rec deleteFilesAndFolders directory isRoot =
    for file in Directory.GetFiles directory
        do File.Delete file
    for subdirectory in Directory.GetDirectories directory do
        deleteFilesAndFolders subdirectory false
        if not isRoot then Directory.Delete subdirectory

let path xs = Path.Combine(Array.ofList xs)
let write content filePath = File.WriteAllText(path filePath, content)

let generateProjectDocument
    (packageReferences: XElement seq)
    (files: XElement seq)
    (copyLocalLockFileAssemblies: bool option)
    (contentItems: XElement seq)
    (projectReferences: XElement seq) =
    XDocument(
        XElement.ofStringName("Project",
            XAttribute.ofStringName("Sdk", "Microsoft.NET.Sdk"),
            seq {
            XElement.ofStringName("PropertyGroup",
                seq {
                    XElement.ofStringName("TargetFramework", "netstandard2.0")
                    XElement.ofStringName("LangVersion", "latest")
                    if copyLocalLockFileAssemblies.IsSome then
                        XElement.ofStringName("CopyLocalLockFileAssemblies",
                            if copyLocalLockFileAssemblies.Value
                            then "true"
                            else "false"
                        )
                })
            if not (files |> Seq.isEmpty) then
                XElement.ofStringName("ItemGroup", files)
            if not (contentItems |> Seq.isEmpty) then
                XElement.ofStringName("ItemGroup", contentItems)
            if not (packageReferences |> Seq.isEmpty) then
                XElement.ofStringName("ItemGroup", packageReferences)
            if not (projectReferences |> Seq.isEmpty) then
                XElement.ofStringName("ItemGroup", projectReferences)
        })
    )

let getJsonPart (url: string) : JObject option =
    match url.Split('#', StringSplitOptions.RemoveEmptyEntries) with
    | [| schemaUrl; path |] ->
        let schemaContent =
            client.GetStringAsync(schemaUrl)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let schemaJson = JObject.Parse(schemaContent)

        let jsonPath =
            path.Split('/', StringSplitOptions.RemoveEmptyEntries)
            |> String.concat "."

        let token = schemaJson.SelectToken(jsonPath)
        if token.Type = JTokenType.Object
        then Some (unbox<JObject> token)
        else None

    | [| schemaUrl |] ->
        let schemaContent =
            client.GetStringAsync(schemaUrl)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let schemaJson = JObject.Parse(schemaContent)
        Some schemaJson

    | _ ->
        None

let preprocessRelativeExternalReferences (schema: JObject) (url: string) =
    let rec iterate (part: JObject) =
        let properties = List.ofSeq(part.Properties())
        for property in properties do
            if property.Value.Type = JTokenType.Object then
                iterate (unbox<JObject> property.Value)
            elif property.Value.Type = JTokenType.Array then
                let elements = unbox<JArray> property.Value
                for element in elements do
                    if element.Type = JTokenType.Object then
                        iterate (unbox<JObject> element)
            elif property.Name = "$ref" && property.Value.Type = JTokenType.String then
                let refUrl = property.Value.ToObject<string>()
                // not absolute && not local -> relative
                if not (refUrl.StartsWith "http") && not (refUrl.StartsWith "#") then
                    // relative url
                    let modifiedUrl = Uri(Uri(url), refUrl)
                    match getJsonPart modifiedUrl.AbsoluteUri with
                    | Some resolvedObject ->
                        part.RemoveAll()
                        for resolvedProp in resolvedObject.Properties() do
                            part.Add(resolvedProp)
                    | None ->
                        property.Value <- JValue modifiedUrl.AbsoluteUri
                else
                    ()

    iterate schema
    schema

type ExternalResouceLoader(schema: string) =
    interface Interface.IStreamLoader with
        member self.Load(uri: Uri) =
            let absoluteUri =
                if not uri.IsAbsoluteUri then
                    Uri(Uri(schema), uri.OriginalString)
                else
                    uri

            client.GetStreamAsync(absoluteUri)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        member self.LoadAsync(uri: Uri) =
            let absoluteUri =
                if not uri.IsAbsoluteUri then
                    Uri(Uri(schema), uri.OriginalString)
                else
                    uri

            client.GetStreamAsync(absoluteUri)

let runConfig filePath =
    let config = resolveFile filePath
    match readConfig config with
    | Error errorMsg ->
        Console.WriteLine errorMsg
        1
    | Ok config ->
        let schema =
            if config.schema.StartsWith "http" && config.resolveReferences then
                let schemaContent =
                    config.schema
                    |> client.GetStringAsync
                    |> Async.AwaitTask
                    |> Async.RunSynchronously

                let schemaJson = JObject.Parse(schemaContent)
                let processedSchema = preprocessRelativeExternalReferences schemaJson config.schema
                getSchema(processedSchema.ToString())
            elif config.schema.StartsWith "http"
            then getSchema config.schema
            else getSchema (resolveFile config.schema)
        let settings = OpenApiReaderSettings()
        settings.ReferenceResolution <- ReferenceResolutionSetting.ResolveAllReferences
        if config.schema.StartsWith "http" then
            // customize how external references are resolved
            settings.CustomExternalLoader <- new ExternalResouceLoader(config.schema)
        let reader = new OpenApiStreamReader(settings)
        let openApi =
            reader.ReadAsync(schema)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let (openApiDocument, diagnostics) = openApi.OpenApiDocument, openApi.OpenApiDiagnostic
        if diagnostics.Errors.Count > 0 && isNull openApiDocument then
            for error in diagnostics.Errors do
                System.Console.WriteLine error.Message
            1
        else
            let outputDir = config.output
            // prepare output directory
            if Directory.Exists outputDir
            then deleteFilesAndFolders outputDir true
            else ignore(Directory.CreateDirectory outputDir)
            // generate global schema types
            let visitedTypes, globalTypesModule = createGlobalTypesModule openApiDocument config
            let code = CodeGen.formatAst (CodeGen.createFile [ globalTypesModule ])
            // generate HTTP client wrapper, pass visited types
            let clientModule = createOpenApiClient openApiDocument visitedTypes config
            let clientModuleCode = CodeGen.formatAst (CodeGen.createFile [ clientModule ])
            write code [ outputDir; "Types.fs" ]
            write clientModuleCode [ outputDir; "Client.fs" ]
            let projectFile =
                let packages = [
                    if config.target = Target.FSharp then
                        XElement.PackageReference("Fable.Remoting.Json", "2.17.0")
                        XElement.PackageReference("Newtonsoft.Json", "13.0.1")
                        if config.asyncReturnType = AsyncReturnType.Task
                        then XElement.PackageReference("Ply", "0.3.1")
                    else
                        XElement.PackageReference("Fable.SimpleJson", "3.21.0")
                        XElement.PackageReference("Fable.SimpleHttp", "3.0.0")
                ]

                let files = [
                    if config.target = Target.FSharp then
                        XElement.Compile "StringEnum.fs"
                        XElement.Compile "OpenApiHttp.fs"
                    XElement.Compile "Types.fs"
                    XElement.Compile "Client.fs"
                ]

                let copyLocalLockFileAssemblies = None
                let contentItems = [ ]
                let projectReferences = [ ]
                generateProjectDocument packages files copyLocalLockFileAssemblies contentItems projectReferences

            if config.target = Target.FSharp then
                let httpLibrary = HttpLibrary.library (config.asyncReturnType = AsyncReturnType.Task) config.project
                write httpLibrary [ outputDir; "OpenApiHttp.fs" ]
                write CodeGen.stringEnumAttr [ outputDir; "StringEnum.fs" ]

            write (projectFile.ToString()) [ outputDir; $"{config.project}.fsproj" ]
            printfn "Succesfully generated project %s" (path [outputDir; $"{config.project}.fsproj" ])
            0

[<EntryPoint>]
let main argv =
    Console.InputEncoding <- Encoding.UTF8
    Console.OutputEncoding <- Encoding.UTF8
    match argv with
    | [| "--version" |] ->
        printfn "0.9.0"
        0
    | [| |] ->
        Console.WriteLine(logo)
        runConfig "./hawaii.json"
    | [| "--no-logo" |] -> 
        runConfig "./hawaii.json"
    | [|"--config"; file|] ->
        Console.WriteLine(logo)
        runConfig file
    | [|"--config"; file; "--no-logo" |] ->
        runConfig file
    | arguments ->
        printfn "Unknown arguments [%s]" (String.concat ", " arguments)
        1