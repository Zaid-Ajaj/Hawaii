open System
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
    target: Target
    projectName : string
    asyncReturnType: AsyncReturnType
    synchornousMethods: bool
}

let xmlDocs (description: string) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map (fun line -> line.Replace(">", "&gt;").Replace("<", "&lt;"))
        |> PreXmlDoc.Create

let xmlDocsWithParams (description: string) (parameters: (string * string) seq) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map (fun line -> line.Replace(">", "&gt;").Replace("<", "&lt;"))
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

let resolveFile (path: string) =
    if Path.IsPathRooted path then
        path
    elif System.Diagnostics.Debugger.IsAttached then
        Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, path))
    else
        Path.GetFullPath (Path.Combine(Environment.CurrentDirectory, path))

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

let findNextTypeName fieldName objectName (selections: string list) (visitedTypes: ResizeArray<string>) =
    let nestedSelectionType =
        selections
        |> List.map capitalize
        |> String.concat "And"

    if not (visitedTypes.Contains objectName) then
        objectName
    elif not (visitedTypes.Contains (capitalize fieldName)) then
        capitalize fieldName
    elif not (visitedTypes.Contains (objectName + capitalize fieldName)) then
        objectName + capitalize fieldName
    elif not (visitedTypes.Contains nestedSelectionType) && selections.Length <= 3 && selections.Length > 1 then
        nestedSelectionType
    elif not (visitedTypes.Contains (capitalize fieldName + "From" + objectName)) then
        capitalize fieldName + "From" + objectName
    else
        nextTick (capitalize fieldName + "From" + objectName) visitedTypes

let findNextEnumTypeName fieldName objectName (visitedTypes: ResizeArray<string>) =
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

    let enumRepresentation = SynTypeDefnSimpleReprUnionRcd.Create([
        for value in values ->
            let attrs = [ SynAttributeList.Create [| compiledName value  |] ]
            let docs = PreXmlDoc.Empty
            SynUnionCase.UnionCase(attrs, Ident.Create (capitalize value), SynUnionCaseType.UnionCaseFields [], docs, None, range0)
    ])

    let simpleType = SynTypeDefnSimpleReprRcd.Union(enumRepresentation)

    let members : SynMemberDefn list = [
        let unitConst : SynPatConstRcd = {
            Const = SynConst.Unit
            Range = range0
        }

        let matchClauses = [
            for value in values ->
                let id = LongIdentWithDots.CreateString (capitalize value)
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

let rec createRecordFromSchema (recordName: string) (schema: OpenApiSchema) (visitedTypes: ResizeArray<string>) (config: CodegenConfig) : SynModuleDecl list =
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

        let isReference = not (isNull propertyType.Reference)

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

            let nestedObjectTypeName = findNextTypeName propertyName recordName nestedPropertyNames visitedTypes
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName propertyType visitedTypes config
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

            let nestedObjectTypeName = findNextTypeName propertyName recordName nestedPropertyNames visitedTypes
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName arrayItemsType visitedTypes config
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

        [
            yield! nestedObjects
            SynModuleDecl.CreateSimpleType(info, simpleRecordType, members)
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
        let typeName =
            if String.IsNullOrEmpty topLevelObject.Value.Title
            then sanitizeTypeName topLevelObject.Key
            else sanitizeTypeName topLevelObject.Value.Title

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
        elif topLevelObject.Value.Type = "object" || isAllOf  then
            visitedTypes.Add typeName
            for createdType in createRecordFromSchema typeName topLevelObject.Value visitedTypes config do
                moduleTypes.Add createdType
        else
            ()

    let globalTypesModule = CodeGen.createNamespace [ config.projectName; "Types" ] (Seq.toList moduleTypes)
    visitedTypes, globalTypesModule

let deriveOperationName (operationName: string) (path: string) (operationType: OperationType) =
    if not (String.IsNullOrWhiteSpace operationName) then
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

type OperationParameter = {
    parameterName: string
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
                        required = pair.Value.Schema.Required.Contains property.Key
                        parameterType = readParamType property.Value
                        docs = property.Value.Description
                        location = "multipartFormData"
                        style = "formfield"
                    }

            if pair.Key = "application/json" then
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
                    required = true
                    parameterType = readParamType schema
                    docs = schema.Description
                    location = "body"
                    style = "none"
                }

            if pair.Key = "application/x-www-form-urlencoded" then
                for property in pair.Value.Schema.Properties do
                    parameters.Add {
                        parameterName = property.Key
                        required = pair.Value.Schema.Required.Contains property.Key
                        parameterType = readParamType property.Value
                        docs = property.Value.Description
                        location = "urlEncodedFormData"
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
        Id = [ Ident.Create $"{config.projectName}Client" ]
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
                    for p in parameters -> (p.parameterName, p.docs)
                ]

                let memberName = deriveOperationName operationInfo.OperationId fullPath operation.Key
                let memberNameAsync = $"{memberName}Async"
                let createIdent xs = SynExpr.CreateLongIdent(LongIdentWithDots.Create xs)
                let stringExpr value = SynExpr.CreateConstString value
                let createLetAssignment leftSide rightSide continuation =
                    let emptySynValData = SynValData.SynValData(None, SynValInfo.Empty, None)
                    let headPat = SynPat.Named(SynPat.Wild range0, leftSide, false, None, range0)
                    let binding = SynBinding.Binding(None, SynBindingKind.NormalBinding, false, false, [], PreXmlDoc.Empty, emptySynValData, headPat, None, rightSide, range0, DebugPointForBinding.DebugPointAtBinding range0 )
                    SynExpr.LetOrUse(false, false, [binding], continuation, range0)

                let requestValues = [
                    for parameter in parameters do
                        if parameter.required then
                            yield SynExpr.CreateApp(
                                createIdent [ "RequestPart"; parameter.location ],
                                SynExpr.CreateParen(SynExpr.CreateTuple [
                                    if parameter.location <> "body"
                                    then stringExpr parameter.parameterName
                                    createIdent [ parameter.parameterName ]
                                ])
                            )
                        else
                            let ifExpr = createIdent [ parameter.parameterName; "IsSome" ]
                            let thenExpr =
                                SynExpr.CreateApp(
                                    createIdent [ "RequestPart"; parameter.location ],
                                    SynExpr.CreateParen(SynExpr.CreateTuple [
                                        if parameter.location <> "body"
                                        then stringExpr parameter.parameterName
                                        createIdent [ parameter.parameterName; "Value" ]
                                    ])
                                )
                            yield SynExpr.IfThenElse(ifExpr, thenExpr, None, DebugPointForBinding.DebugPointAtBinding(range0), false, range0, range0)
                ]

                let httpFunction = operation.Key.ToString().ToLower()
                let httpFunctionAsync = $"{httpFunction}Async"
                let requestParts = Ident.Create "requestParts"
                let clientOperation httpFunc name = SynMemberDefn.CreateMember {
                    SynBindingRcd.Null with
                        XmlDoc = xmlDocsWithParams summary parameterDocs
                        Expr =
                            createLetAssignment
                                requestParts
                                (SynExpr.ArrayOrList(false, requestValues, range0))
                                (SynExpr.CreateApp(
                                    SynExpr.CreateApp(
                                        SynExpr.CreateApp(
                                            SynExpr.CreateLongIdent(LongIdentWithDots.Create [ "OpenApiHttp"; httpFunc ]),
                                            SynExpr.CreateIdent (Ident.Create "httpClient")
                                        ),
                                        SynExpr.CreateConstString fullPath),
                                        SynExpr.Ident requestParts)
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
                                                        then SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString(parameter.parameterName), [])
                                                        else SynPatRcd.OptionalVal {
                                                            Range = range0
                                                            Id = Ident.Create parameter.parameterName
                                                        }
                                                }
                                        ]
                                        Range  = range0
                                    }
                                )
                            ])
                }

                if config.synchornousMethods then
                    clientMembers.Add (clientOperation httpFunctionAsync memberNameAsync)
                    clientMembers.Add (clientOperation httpFunction memberName)
                else
                    clientMembers.Add (clientOperation httpFunctionAsync memberName)

    let clientType = SynModuleDecl.CreateType(info, Seq.toList clientMembers)

    let moduleContents = [
        yield SynModuleDecl.CreateOpen "System.Net.Http"
        yield SynModuleDecl.CreateOpen $"{config.projectName}.Types"
        yield SynModuleDecl.CreateOpen $"{config.projectName}.Http"
        // extra types generated from parameters
        for extraType in extraTypes do
            yield extraType
        // the main http client
        yield clientType
    ]

    let clientModule = CodeGen.createNamespace [ config.projectName ] moduleContents
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

[<EntryPoint>]
let main argv =
    try
        let localScheme = resolveFile "./schemas/petstore-modified.json"
        let remoteSchema = "https://petstore.swagger.io/v2/swagger.json"
        let schema = getSchema remoteSchema
        let reader = new OpenApiStreamReader()
        let (openApiDocument, diagnostics) =  reader.Read(schema)
        if diagnostics.Errors.Count > 0 && isNull openApiDocument then
            for error in diagnostics.Errors do
                System.Console.WriteLine error.Message
            1
        else
            let outputDir = resolveFile "./output"

            let config = {
                target = Target.FSharp
                projectName = "PetStore"
                asyncReturnType = AsyncReturnType.Async
                synchornousMethods = false
            }

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
                        XElement.PackageReference("Fable.SimpleJson", "3.19.0")
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
                let httpLibrary = HttpLibrary.library (config.asyncReturnType = AsyncReturnType.Task) config.projectName
                write httpLibrary [ outputDir; "OpenApiHttp.fs" ]
                write CodeGen.stringEnumAttr [ outputDir; "StringEnum.fs" ]

            write (projectFile.ToString()) [ outputDir; $"{config.projectName}.fsproj" ]
            0 // return an integer exit code
    with
    | error ->
        printfn "%s" error.Message
        1