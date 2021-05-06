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

let xmlDocs (description: string) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> PreXmlDoc.Create

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

let isEnumType (schema: OpenApiSchema) =
    schema.Type = "string"
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
                Expr = SynExpr.CreateMatch(SynExpr.Ident(Ident.Create "this"), matchClauses)
            }
    ]

    SynModuleDecl.CreateSimpleType(info, simpleType, members)

let rec createRecordFromSchema (recordName: string) (schema: OpenApiSchema) (visitedTypes: ResizeArray<string>) : SynModuleDecl list =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create recordName ]
        XmlDoc = xmlDocs schema.Description
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let nestedObjects = ResizeArray<SynModuleDecl>()
    let recordFields = ResizeArray<SynFieldRcd>()
    let addedFields = ResizeArray<string * bool * SynType>()

    for property in schema.Properties do
        // todo: infer the types correctly
        let propertyName = property.Key
        let propertyType = property.Value
        let isEnum = isEnumType propertyType
        let required = schema.Required.Contains propertyName
        let isObjectArray =
            propertyType.Type = "array"
            && propertyType.Items.Type = "object"
            && isNull propertyType.Items.Reference
        let isEnumArray = propertyType.Type = "array" && isEnumType propertyType.Items
        let isPrimitve = List.forall id [
            (property.Value.Type <> "object" || not (isNull property.Value.Reference))
            not isEnum
            not isObjectArray
            not isEnumArray
        ]

        if isPrimitve then
            let fieldType = createFieldType recordName required propertyName propertyType
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = xmlDocs propertyType.Description
            recordFields.Add { field with XmlDoc = docs }
            addedFields.Add((propertyName, required, fieldType))
        else if isEnum && isNull propertyType.Reference then
            // nested enum -> not a reference to a global usable enum
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
                let docs = xmlDocs propertyType.Description
                recordFields.Add { field with XmlDoc = docs }
                addedFields.Add((propertyName, required, fieldType))
            | _ ->
                ()
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
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = xmlDocs propertyType.Description
            recordFields.Add { field with XmlDoc = docs }
            addedFields.Add((propertyName, required, fieldType))
        else if property.Value.Type = "object" then
            // handle nested objects
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
            let docs = xmlDocs propertyType.Description
            recordFields.Add { field with XmlDoc = docs }
            addedFields.Add((propertyName, required, fieldType))
        else if isObjectArray then
             // handle arrays of nested objects
            let arrayItemsType = propertyType.Items
            let nestedPropertyNames =
                arrayItemsType.Properties
                |> Seq.map (fun pair -> pair.Key)
                |> Seq.toList

            let nestedObjectTypeName = findNextTypeName property.Key recordName nestedPropertyNames visitedTypes
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName arrayItemsType visitedTypes
            nestedObjects.AddRange nestedObject
            let fieldType =
                if required
                then SynType.List(SynType.Create nestedObjectTypeName)
                else SynType.Option(SynType.List(SynType.Create nestedObjectTypeName))
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = xmlDocs propertyType.Description
            recordFields.Add { field with XmlDoc = docs }
            addedFields.Add((propertyName, required, fieldType))
        else if isEnumArray then
            let arrayItemsType = propertyType.Items
            if isNull arrayItemsType.Reference then
                // nested enum type -> not a global reference
                match arrayItemsType with
                | StringEnum cases ->
                    let enumTypeName = findNextTypeName propertyName recordName [ ] visitedTypes
                    visitedTypes.Add enumTypeName
                    let createdEnumType = createEnumType enumTypeName cases
                    nestedObjects.Add createdEnumType
                    let fieldType =
                        if required
                        then SynType.List(SynType.Create enumTypeName)
                        else SynType.Option(SynType.List(SynType.Create enumTypeName))
                    let field = SynFieldRcd.Create(propertyName, fieldType)
                    let docs = xmlDocs propertyType.Description
                    recordFields.Add { field with XmlDoc = docs }
                    addedFields.Add((propertyName, required, fieldType))
                | _ ->
                    ()
            else
                // referenced enum type
                let typeName =
                    if String.IsNullOrEmpty propertyType.Title
                    then propertyType.Reference.Id
                    else propertyType.Title
                let fieldType =
                    if required
                    then SynType.List(SynType.Create typeName)
                    else SynType.Option(SynType.List(SynType.Create typeName))
                let field = SynFieldRcd.Create(propertyName, fieldType)
                let docs = xmlDocs propertyType.Description
                recordFields.Add { field with XmlDoc = docs }
                addedFields.Add((propertyName, required, fieldType))
        else
            ()

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
                                if required
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
    if diagnostics.Errors.Count > 0 then
        for error in diagnostics.Errors do
            System.Console.WriteLine error.Message
        1
    else
        let globalTypesModule = createGlobalTypesModule "PetStore" openApiDocument
        let code = CodeGen.formatAst (CodeGen.createFile [ globalTypesModule ])
        System.Console.WriteLine code
        0 // return an integer exit code