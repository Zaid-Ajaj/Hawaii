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
open Microsoft.OpenApi.Interfaces
open System.IO

let resolveFile (path: string) =
    if Path.IsPathRooted path
    then path
    else Path.GetFullPath (Path.Combine(Environment.CurrentDirectory, path))
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

    if not (visitedTypes.Contains (objectName + capitalize fieldName)) then
        objectName + capitalize fieldName
    elif not (visitedTypes.Contains nestedSelectionType) && selections.Length <= 3 && selections.Length > 1 then
        nestedSelectionType
    elif not (visitedTypes.Contains (capitalize fieldName + "From" + objectName)) then
        capitalize fieldName + "From" + objectName
    else
        nextTick (capitalize fieldName + "From" + objectName) visitedTypes

let isEnumType (schema: OpenApiSchema) =
    schema.Type = "string"
    && not (isNull schema.Enum)
    && schema.Enum.Count > 0
    && isNull schema.Reference

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
        | "string" when propertySchema.Format = "date-time" -> SynType.DateTimeOffset()
        | "string" when propertySchema.Format = "byte" ->
            // base64 encoded characters
            // use a byte array
            SynType.Array(1, SynType.Byte(), range0)
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

let createEnumType (enumName: string) (values: seq<string>) =
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
                Expr = SynExpr.Match(DebugPointForBinding.DebugPointAtBinding range0, SynExpr.Ident(Ident.Create "this"), matchClauses, range0)
            }
    ]

    SynModuleDecl.CreateSimpleType(info, simpleType, members)

let rec createRecordFromSchema (recordName: string) (schema: OpenApiSchema) (visitedTypes: ResizeArray<string>) : SynModuleDecl list =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create recordName ]
        XmlDoc = PreXmlDoc.Create [ if String.isNotNullOrEmpty schema.Description then schema.Description ]
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let nestedObjects = ResizeArray<SynModuleDecl>()
    let recordFields = ResizeArray<SynFieldRcd>()

    for property in schema.Properties do
        // todo: infer the types correctly
        let propertyName = property.Key
        let propertyType = property.Value
        let isEnum = isEnumType propertyType
        let required = schema.Required.Contains propertyName
        if (property.Value.Type <> "object" || not (isNull property.Value.Reference)) && not isEnum then
            let fieldType = createFieldType recordName required propertyName propertyType
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = PreXmlDoc.Create [ if String.isNotNullOrEmpty propertyType.Description then propertyType.Description ]
            recordFields.Add { field with XmlDoc = docs }
        else if isEnum then
            match property.Value with
            | StringEnum cases ->
                let enumTypeName = findNextTypeName propertyName recordName [ ] visitedTypes
                visitedTypes.Add enumTypeName
                let createdEnumType = createEnumType enumTypeName cases
                nestedObjects.Add createdEnumType
                let fieldType =
                    if required
                    then SynType.Create enumTypeName
                    else SynType.Option(SynType.Create enumTypeName)
                let field = SynFieldRcd.Create(propertyName, fieldType)
                let docs = PreXmlDoc.Create [ if String.isNotNullOrEmpty propertyType.Description then propertyType.Description ]
                recordFields.Add { field with XmlDoc = docs }
            | _ ->
                ()
        else
            let nestedPropertyNames =
                property.Value.Properties
                |> Seq.map (fun pair -> pair.Key)
                |> Seq.toList

            let nestedObjectTypeName = findNextTypeName property.Key recordName nestedPropertyNames visitedTypes
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName property.Value visitedTypes
            nestedObjects.AddRange nestedObject
            let fieldType =
                if required
                then SynType.Create nestedObjectTypeName
                else SynType.Option(SynType.Create nestedObjectTypeName)
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = PreXmlDoc.Create [ if String.isNotNullOrEmpty propertyType.Description then propertyType.Description ]
            recordFields.Add { field with XmlDoc = docs }


    let recordRepr = SynTypeDefnSimpleReprRecordRcd.Create (List.ofSeq recordFields)
    let simpleType = SynTypeDefnSimpleReprRcd.Record recordRepr

    [
        yield! nestedObjects
        SynModuleDecl.CreateSimpleType(info, simpleType)
    ]

let createGlobalTypesModule (projectName: string) (openApiDocument: OpenApiDocument) =
    let visitedTypes = ResizeArray<string>()

    let globalTypes = [
        for topLevelObject in openApiDocument.Components.Schemas do
            let typeName =
                if String.IsNullOrEmpty topLevelObject.Value.Title
                then topLevelObject.Key
                else topLevelObject.Value.Title

            visitedTypes.Add typeName

            if topLevelObject.Value.Type = "object"
            then yield! createRecordFromSchema typeName topLevelObject.Value visitedTypes
            elif topLevelObject.Value.Type = "string" then
                match topLevelObject.Value with
                | StringEnum cases -> createEnumType typeName cases
                | _ -> ()
            else
                ()
    ]

    let globalTypesModule = CodeGen.createNamespace [ projectName; "Types" ] globalTypes
    globalTypesModule

[<EntryPoint>]
let main argv =
    let schema = getSchema (resolveFile "./schemas/petstore.json")
    let reader = new OpenApiStreamReader()
    let (openApiDocument, diagnostics) =  reader.Read(schema)
    let globalTypesModule = createGlobalTypesModule "PetStore" openApiDocument
    let code = CodeGen.formatAst (CodeGen.createFile [ globalTypesModule ])
    System.Console.WriteLine code
    0 // return an integer exit code