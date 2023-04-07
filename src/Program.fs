open System
open Microsoft.OpenApi.Readers
open System.Net.Http
open FsAst
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.XmlDoc
open Microsoft.OpenApi.Models
open System.IO
open System.Xml.Linq
open System.Net
open System.Collections.Generic
open Newtonsoft.Json.Linq
open System.Text
open Microsoft.OpenApi.OData
open Microsoft.OData.Edm.Csdl
open Microsoft.OpenApi.Writers
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

let resolveFile (path: string) =
    if Path.IsPathRooted path then
        path
    elif System.Diagnostics.Debugger.IsAttached then
        Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, path))
    else
        Path.GetFullPath (Path.Combine(Environment.CurrentDirectory, path))

let resolveRelativeFile current (path: string) =
    if Path.IsPathRooted path then
        path
    else
        Path.GetFullPath (Path.Combine(current, path))

let safeSeq (xs: seq<'a>) =
    if isNull xs
    then Seq.empty
    else xs

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
        elif isNotNull parts.["emptyDefinitions"] && parts.["emptyDefinitions"].ToObject<string>().ToLower().Trim() <> "ignore" && parts.["emptyDefinitions"].ToObject<string>().ToLower().Trim() <> "free-form" then
            Error "The 'emptyDefinitions' configuration element must either be 'ignore' or 'free-form'"
        else
            let configParent = Path.GetDirectoryName file
            Ok {
                schema = parts.["schema"].ToObject<string>()
                output = resolveRelativeFile configParent (parts.["output"].ToObject<string>())
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
                emptyDefinitions =
                    if isNotNull parts.["emptyDefinitions"] && parts.["emptyDefinitions"].ToString() = "free-form"
                    then EmptyDefinitionResolution.GenerateFreeForm
                    else EmptyDefinitionResolution.Ignore
                overrideSchema =
                    if isNotNull parts.["overrideSchema"]
                    then Some parts.["overrideSchema"]
                    else None
                filterTags =
                    if isNotNull parts.["filterTags"] && parts.["filterTags"].Type = JTokenType.Array
                    then [
                            for tag in unbox<JArray> parts.["filterTags"] do
                            if tag.Type = JTokenType.String then
                                tag.ToObject<string>() ]
                    else [ ]
                odataSchema = false
            }
    with
    | error ->
        Error $"Error ocurred while reading the configuration file: {error.Message}"

let escapeDocs (value: string) = value.Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;")

let xmlDocs (description: string) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map escapeDocs
        |> PreXmlDoc.Create

let xmlDocsWithParams (description: string) (parameters: (string * string) seq) =
    if String.IsNullOrWhiteSpace description then
        PreXmlDoc.Create [ ]
    else
        description.Split("\r\n")
        |> Seq.collect (fun line -> line.Split("\n"))
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map escapeDocs
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
                                |> Seq.map escapeDocs

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

let simplifyRedundantSchemaParts (schema: JObject) =
    let rec iterate (part: JObject) =
        let properties = List.ofSeq(part.Properties())
        for property in properties do
            if property.Name.StartsWith "application/vnd" && property.Name.EndsWith "+json" && property.Value.Type = JTokenType.Object && not (part.ContainsKey "application/json") then
                // rewrite JSON-like media types into application/json
                let mediaType = unbox<JObject> property.Value
                part.Add("application/json", mediaType)
                part.Remove(property.Name) |> ignore
            elif property.Name = "application/ld+json" && property.Value.Type = JTokenType.Object && not (part.ContainsKey "application/json") then
                // rewrite JSON-like media types into application/json
                let mediaType = unbox<JObject> property.Value
                part.Add("application/json", mediaType)
                part.Remove(property.Name) |> ignore
            elif property.Name = "anyOf" && property.Value.Type = JTokenType.Array then
                // simplify this shape
                // { anyOf: [ first ] }
                // into
                // { ...first }
                let anyOfArray = unbox<JArray> property.Value
                if anyOfArray.Count = 1 && anyOfArray.[0].Type = JTokenType.Object then
                    let innerObject = unbox<JObject> anyOfArray.[0]
                    for innerProp in innerObject.Properties() do
                        part.Add(innerProp)
                    part.Remove("anyOf") |> ignore
            elif property.Name = "oneOf" && property.Value.Type = JTokenType.Array then
                // simplify this shape
                // { oneOf: [ first ] }
                // into
                // { ...first }
                let oneOfArray = unbox<JArray> property.Value
                if oneOfArray.Count = 1 && oneOfArray.[0].Type = JTokenType.Object then
                    let innerObject = unbox<JObject> oneOfArray.[0]
                    for innerProp in innerObject.Properties() do
                        part.Add(innerProp)
                    part.Remove("oneOf") |> ignore
            elif property.Name = "allOf" && property.Value.Type = JTokenType.Array then
                // simplify this shape
                // { allOf: [ first, { "example": ... } ] }
                // into
                // { ...first }
                let allOfArray = unbox<JArray> property.Value
                if allOfArray.Count = 2 && allOfArray.[0].Type = JTokenType.Object && allOfArray.[1].Type = JTokenType.Object then
                    let firstObject = unbox<JObject> allOfArray.[0]
                    let secondObject = unbox<JObject> allOfArray.[1]
                    if secondObject.Count = 1 && secondObject.ContainsKey "example" then
                        for innerProp in firstObject.Properties() do
                            part.Add(innerProp)
                        part.Remove("allOf") |> ignore
                    else
                        for element in allOfArray do
                            if element.Type = JTokenType.Object then
                                iterate (unbox<JObject> element)
                else
                    for element in allOfArray do
                        if element.Type = JTokenType.Object then
                            iterate (unbox<JObject> element)
            elif property.Value.Type = JTokenType.Array then
                let elements = unbox<JArray> property.Value
                for element in elements do
                    if element.Type = JTokenType.Object then
                        iterate (unbox<JObject> element)
            if property.Value.Type = JTokenType.Object then
                iterate (unbox<JObject> property.Value)

    iterate schema
    schema

let readExternalODataSchema (schemaUrl: string) =
    let content =
        schemaUrl
        |> client.GetStringAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let odataModel = CsdlReader.Parse(XElement.Parse(content).CreateReader())
    let openApiModel = odataModel.ConvertToOpenApi();
    use stringTextWriter = new StringWriter()
    let writer = OpenApiJsonWriter(stringTextWriter)
    openApiModel.SerializeAsV3(writer);
    stringTextWriter.ToString()

let readLocalODataSchema (schemaUrl: string) =
    let content = File.ReadAllText schemaUrl
    let odataModel = CsdlReader.Parse(XElement.Parse(content).CreateReader())
    let openApiModel = odataModel.ConvertToOpenApi();
    use stringTextWriter = new StringWriter()
    let writer = OpenApiJsonWriter(stringTextWriter)
    openApiModel.SerializeAsV3(writer);
    stringTextWriter.ToString()

let getSchema(schema: string) (overrideSchema: JToken option) =
    let schemaContents =
        if File.Exists schema && schema.EndsWith ".json" then
            let content = File.ReadAllText schema
            JObject.Parse(content)
        elif File.Exists schema && schema.EndsWith ".xml" then
            Console.WriteLine "Detected local OData schema"
            let openApiJson = readLocalODataSchema schema
            JObject.Parse openApiJson
        elif schema.StartsWith "http" && schema.EndsWith "$metadata" then
            Console.WriteLine "Detected external OData schema"
            let openApiJson = readExternalODataSchema schema
            JObject.Parse openApiJson
        elif schema.StartsWith "http" then
            let content =
                client.GetStringAsync(schema)
                |> Async.AwaitTask
                |> Async.RunSynchronously
            JObject.Parse(content)
        else
            // assume the schema is coming in as a string
            // convert it into a memory stream
            // this is useful for unit tests
            JObject.Parse(schema)

    match overrideSchema with
    | None -> ()
    | Some miniSchema -> schemaContents.Merge(miniSchema)

    // Pre-process NSwag schemas and add { "produces": ["application/json"] } if missing for operations
    // we assume Hawaii is working with schemas that produce JSON
    if schemaContents.ContainsKey "x-generator" && schemaContents.["x-generator"].ToObject<string>().StartsWith "NSwag" then
        if schemaContents.ContainsKey "paths" && schemaContents.["paths"].Type = JTokenType.Object then
            let pathsObject = unbox<JObject> schemaContents.["paths"]
            for path in pathsObject.Properties() do
                if path.Value.Type = JTokenType.Object then
                    let operations = unbox<JObject> path.Value
                    for operation in operations.Properties() do
                        if operation.Value.Type = JTokenType.Object then
                            let operationAsObject = unbox<JObject> operation.Value
                            if not (operationAsObject.ContainsKey "produces") then
                                operationAsObject.Add(JProperty("produces", [| "application/json" |]))

    let simplified = simplifyRedundantSchemaParts schemaContents
    let simplifiedContents = simplified.ToString()
    if simplifiedContents.Contains "#/components/schemas/odata.error" then
        simplified.Add(JProperty("x-odata", true))
        let schemaBytes = System.Text.Encoding.UTF8.GetBytes(simplified.ToString())
        new MemoryStream(schemaBytes) :> Stream
    else
        let schemaBytes = System.Text.Encoding.UTF8.GetBytes simplifiedContents
        new MemoryStream(schemaBytes) :> Stream

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
    (schema.Type = "string" || schema.Type = "integer" || String.IsNullOrEmpty schema.Type)
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

        let containsDigitsOnly (case: string) =
            Seq.forall Char.IsDigit case

        let allDigitCases =
            Seq.forall containsDigitsOnly cases

        let containsQoutes =
            cases |> Seq.exists (fun case -> case.Contains "\"")

        let containsTimeZone =
            cases |> Seq.exists (fun case -> case.Contains "00Z")

        if not (Seq.isEmpty cases) && not allDigitCases && not containsQoutes && not containsTimeZone then
            Some (Seq.toList cases)
        else
            None
    else
        None

let rec cleanOperationName (operationName: string) =
    let operation = operationName.Replace("{", "").Replace("}", "")

    if operation.Contains "?" then
        match operation.Split '?' with
        | [| path; parameters |] ->
            let queryParams =
                parameters.Split([|'&'; '=' |], StringSplitOptions.RemoveEmptyEntries)
                |> Array.map (fun part -> part.Replace("{", "").Replace("}", ""))
                |> Array.distinctBy id
                |> Array.map capitalize
                |> String.concat "And"
            cleanOperationName (path + "By" + queryParams)
        | _ ->
            operation.Split([| '?'; '='; '&' |], StringSplitOptions.RemoveEmptyEntries)
            |> Array.map capitalize
            |> String.concat ""
            |> cleanOperationName
    else
        let invalidChars = [| '-'; '#'; '_'; '.'; '+'; '$'; '&'; '['; ']'; '/'; '\\'; '*'; '"'; '`' |]
        operation.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)
        |> Array.map capitalize
        |> String.concat ""

let rec deriveOperationName (operationName: string) (path: string) (operationType: OperationType) (visitedTypes: ResizeArray<string>) =
    if not (String.IsNullOrWhiteSpace operationName) then
        if not (visitedTypes.Contains (cleanOperationName operationName)) then
           cleanOperationName operationName
        elif not (visitedTypes.Contains (string operationType + cleanOperationName operationName)) then
            string operationType + cleanOperationName operationName
        else
            deriveOperationName "" path operationType visitedTypes
    else
        let parts = path.Split("/")
        let parameters =
            parts
            |> Array.filter (fun part -> part.Contains "{" && part.Contains "}")
            |> Array.map (fun part -> part.Replace("{", "").Replace("}", ""))
            |> Array.map capitalize
            |> Array.map cleanOperationName
            |> String.concat "And"

        let segments =
            parts
            |> Array.filter (fun part -> not (part.Contains "{" && part.Contains "}"))
            |> Array.mapi (fun index part ->
                if index <> 0
                then cleanOperationName (capitalize part)
                else cleanOperationName part)
            |> String.concat ""

        if String.IsNullOrEmpty parameters then
            cleanOperationName (string operationType + segments)
        else
            cleanOperationName (string operationType + segments + "By" + parameters)

let deriveMemberName (operationName: string) (path: string) (operationType: OperationType) =
    if not (String.IsNullOrWhiteSpace operationName) then
        cleanOperationName operationName
    else
        let parts = path.Split("/")
        let parameters =
            parts
            |> Array.filter (fun part -> part.Contains "{" && part.Contains "}")
            |> Array.map (fun part -> part.Replace("{", "").Replace("}", ""))
            |> Array.map capitalize
            |> Array.map cleanOperationName
            |> String.concat "And"

        let segments =
            parts
            |> Array.filter (fun part -> not (part.Contains "{" && part.Contains "}"))
            |> Array.mapi (fun index part ->
                if index <> 0
                then cleanOperationName (capitalize part)
                else cleanOperationName part)
            |> String.concat ""

        if String.IsNullOrEmpty parameters then
            cleanOperationName (string operationType + segments)
        else
            cleanOperationName (string operationType + segments + "By" + parameters)

module MediaTypes =
    let [<Literal>] ApplicationJson = "application/json"
    let [<Literal>] OctetStream = "application/octet-stream"
    let [<Literal>] ApplicationPdf = "application/pdf"
    let [<Literal>] ApplicationZip = "application/zip"
    let [<Literal>] AppliationZipCompressed = "application/x-zip-compressed"
    let [<Literal>] ImagePng = "image/png"
    let [<Literal>] ImageJpg = "image/jpg"
    let [<Literal>] ImageJpeg = "image/jpeg"
    let [<Literal>] ImageGif = "image/gif"

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

let rec createFieldType recordName required (propertyName: string) (propertySchema: OpenApiSchema) (config: CodegenConfig) =
    if not required then
        let optionalType : SynType = createFieldType recordName true propertyName propertySchema config
        SynType.Option(optionalType)
    elif isNull propertySchema then
        if config.target = Target.FSharp
        then SynType.JToken()
        else SynType.Object()
    elif not (isNull propertySchema.Reference) then
        // working with a reference type
        let typeName =
            if invalidTitle propertySchema.Title
            then sanitizeTypeName propertySchema.Reference.Id
            else sanitizeTypeName propertySchema.Title
        SynType.Create typeName
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
            let arrayItemsType = createFieldType recordName required propertyName propertySchema.Items config
            SynType.List(arrayItemsType)
        | _ ->
            SynType.String()

let compiledName (name: string) = SynAttribute.CompiledName name

let createEnumType (enumName: string) (values: seq<string>) (docs: string option) =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [
            SynAttributeList.Create [
                SynAttribute.Create [ "Fable";"Core"; "StringEnum" ]
                SynAttribute.RequireQualifiedAccess()
            ]
        ]
        Id = [ Ident.Create enumName ]
        XmlDoc =
            match docs with
            | None -> PreXmlDoc.Empty
            | Some value -> xmlDocs value
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let cleanEnumValue (case: string) =
        if String.IsNullOrWhiteSpace case then
            "EmptyString"
        else
            let parts =
                case.Split([| '/'; '.'; ','; '['; '-'; ']';'('; ')'; ' ' |], StringSplitOptions.RemoveEmptyEntries)
            if parts.Length >= 1 then
                parts
                |> Array.map (fun part ->
                    let removedNumberPrefix =
                        if Char.IsDigit part.[0] && not (Seq.forall Char.IsDigit part) then
                            part
                            |> Seq.skipWhile (Char.IsLetter >> not)
                            |> Array.ofSeq
                            |> String
                        elif Seq.forall Char.IsDigit part then
                            $"Numeric_{part}"
                        elif part.[0] = '+' then
                            $"Plus{String(Array.ofSeq part.[1..])}"
                        elif part = "<" then
                            "LessThan"
                        elif part = "<=" then
                            "LessThanOrEqual"
                        elif part = ">" then
                            "GreaterThan"
                        elif part = ">=" then
                            "GreaterThanOrEqual"
                        elif part = "==" || part = "=" then
                            "Equal"
                        elif part = "!=" || part = "<>" then
                            "NotEqual"
                        elif part = "*" then
                            "Star"
                        else
                            part
                    capitalize removedNumberPrefix
                )
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



let statusCode = function
    | "200" -> Some (nameof HttpStatusCode.OK)
    | "201" -> Some (nameof HttpStatusCode.Created)
    | "202" -> Some (nameof HttpStatusCode.Accepted)
    | "204" -> Some (nameof HttpStatusCode.NoContent)
    | "206" -> Some (nameof HttpStatusCode.PartialContent)
    | "301" -> Some (nameof HttpStatusCode.MovedPermanently)
    | "302" -> Some (nameof HttpStatusCode.Found)
    | "400" -> Some (nameof HttpStatusCode.BadRequest)
    | "401" -> Some (nameof HttpStatusCode.Unauthorized)
    | "403" -> Some (nameof HttpStatusCode.Forbidden)
    | "404" -> Some (nameof HttpStatusCode.NotFound)
    | "405" -> Some (nameof HttpStatusCode.MethodNotAllowed)
    | "409" -> Some (nameof HttpStatusCode.Conflict)
    | "415" -> Some (nameof HttpStatusCode.UnsupportedMediaType)
    | "416" -> Some (nameof HttpStatusCode.RequestedRangeNotSatisfiable)
    | "500" -> Some (nameof HttpStatusCode.InternalServerError)
    | "503" -> Some (nameof HttpStatusCode.ServiceUnavailable)
    | "default" -> Some "DefaultResponse"
    | _ -> None

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



/// Creates a declaration: type {typeName} = {aliasedType}
///
/// This is used when there are global schema components that map to primitive types
let createTypeAbbreviation (abbreviation: string) (aliasedType: SynType) =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create (sanitizeTypeName abbreviation) ]
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

/// Creates a declaration: type {typeName} = {aliasedType}
///
/// This is used when there are global schema components that map to primitive types
let createTypeAbbreviationWithDocs (abbreviation: string) (aliasedType: SynType) (docs: string) =
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create (sanitizeTypeName abbreviation) ]
        XmlDoc = xmlDocs docs
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
    let typeName = sanitizeTypeName name
    let schemas =
        if isNotNull openApiDocument.Components
        then List.ofSeq (openApiDocument.Components.Schemas)
        else []

    let isGlobal =
        schemas
        |> List.exists (fun pair ->
            let pairName = sanitizeTypeName pair.Key
            let isRef = isNotNull pair.Value.Reference
            isRef && (
                typeName = pairName
                || typeName = sanitizeTypeName pair.Value.Reference.Id
            )
        )

    isGlobal

let rec createRecordFromSchema (recordName: string) (schema: OpenApiSchema) (visitedTypes: ResizeArray<string>) (config: CodegenConfig) (openApiDocument: OpenApiDocument) (factory: FactoryFunction) : SynModuleDecl list =
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

    let rec createPropertyType (propertyName: string) (propertyType: OpenApiSchema) =
        if isNull propertyType then
            None
        else
        let isEnum = isEnumType propertyType
        let required = schema.Required.Contains propertyName
        let isObjectArray =
            propertyType.Type = "array"
            && isNotNull propertyType.Items
            && propertyType.Items.Type = "object"
            && isNull propertyType.Items.Reference
            && isNotNull propertyType.Items.Properties
            && propertyType.Items.Properties.Count > 0

        let isAdditionalProperties =
            propertyType.AdditionalPropertiesAllowed
            && not (isNull propertyType.AdditionalProperties)
            && not (isNull propertyType.AdditionalProperties.Type && propertyType.AdditionalProperties.Properties.Count > 0)

        let isEnumArray =
            propertyType.Type = "array"
            && isNotNull propertyType.Items
            && isEnumType propertyType.Items

        let isEmptyObjectDefinition =
            propertyType.Type = "object"
            && propertyType.Properties.Count = 0
            && (isNull propertyType.AllOf || propertyType.AllOf.Count = 0)
            && (isNull propertyType.AnyOf || propertyType.AnyOf.Count = 0)

        let isEmptyDefinition = isNull propertyType.Type

        let isKeyValuePairObject =
            propertyType.Type = "object"
            && propertyType.Title = "KeyValuePair`2"
            && propertyType.Properties.Count = 2
            && propertyType.Properties.ContainsKey "Key"
            && propertyType.Properties.ContainsKey "Value"

        let isArrayOfKeyValuePairObject =
            propertyType.Type = "array"
            && isNotNull propertyType.Items
            && propertyType.Items.Type = "object"
            && propertyType.Items.Title = "KeyValuePair`2"
            && propertyType.Items.Properties.Count = 2
            && propertyType.Items.Properties.ContainsKey "Key"
            && propertyType.Items.Properties.ContainsKey "Value"

        let isArrayOfEmptyObject =
            propertyType.Type = "array"
            && isNotNull propertyType.Items
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
            not isEmptyDefinition
            not isKeyValuePairObject
            not isArrayOfKeyValuePairObject
            not isArrayOfEmptyObject
        ]

        if propertyType.Deprecated then
            // skip deprecated propertie
            None
        elif isAdditionalProperties then
            let fieldType = createFieldType recordName true propertyName propertyType.AdditionalProperties config
            match required, propertyType.AdditionalProperties.Nullable with
            | false, false -> SynType.Option(SynType.Map(SynType.String(), fieldType))
            | true, false -> SynType.Map(SynType.String(), fieldType)
            | false, true -> SynType.Option(SynType.Map(SynType.String(), SynType.Option(fieldType)))
            | true, true -> SynType.Map(SynType.String(), SynType.Option(fieldType))
            |> Some
        elif isPrimitve then
            let fieldType = createFieldType recordName required propertyName propertyType config
            Some fieldType
        else if isEnum && isNull propertyType.Reference then
            // nested enum -> not a reference to a global usable enum
            let enumPropertyName = sanitizeTypeName propertyName
            let enumTypeName = findNextEnumTypeName enumPropertyName recordName visitedTypes
            match propertyType with
            | StringEnum cases ->
                visitedTypes.Add enumTypeName
                let createdEnumType = createEnumType enumTypeName cases None
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
                if invalidTitle propertyType.Title
                then sanitizeTypeName propertyType.Reference.Id
                else sanitizeTypeName propertyType.Title
            let fieldType =
                if required
                then SynType.Create typeName
                else SynType.Option(SynType.Create typeName)
            Some fieldType
        else if isEmptyObjectDefinition then
            // empty object definition
            let fieldType =
                if required then
                    if config.target = Target.FSharp
                    then SynType.JObject()
                    else SynType.Object()
                else
                    if config.target = Target.FSharp
                    then SynType.Option(SynType.JObject())
                    else SynType.Option(SynType.Object())
            Some fieldType
        else if isEmptyDefinition then
            // empty object definition
            let fieldType =
                if required then
                    if config.target = Target.FSharp
                    then SynType.JToken()
                    else SynType.Object()
                else
                    if config.target = Target.FSharp
                    then SynType.Option(SynType.JToken())
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

            if isEmptySchema propertyType then
                let freeFormType =
                    if config.target = Target.FSharp
                    then SynType.JToken()
                    else SynType.Object()
                Some freeFormType
            else
                let objectPropertyName = (capitalize (sanitizeTypeName propertyName))
                let isGlobal = isGlobalRef objectPropertyName openApiDocument
                let nestedObjectTypeName = findNextTypeName objectPropertyName recordName nestedPropertyNames visitedTypes isGlobal
                visitedTypes.Add nestedObjectTypeName
                let nestedObject = createRecordFromSchema nestedObjectTypeName propertyType visitedTypes config openApiDocument factory
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

            let objectTypeName = capitalize (sanitizeTypeName propertyName)
            let isGlobal = isGlobalRef objectTypeName openApiDocument
            let nestedObjectTypeName = findNextTypeName objectTypeName recordName nestedPropertyNames visitedTypes isGlobal
            visitedTypes.Add nestedObjectTypeName
            let nestedObject = createRecordFromSchema nestedObjectTypeName arrayItemsType visitedTypes config openApiDocument factory
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
                    let createdEnumType = createEnumType enumTypeName cases None
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
                    if invalidTitle arrayItemsType.Title
                    then sanitizeTypeName arrayItemsType.Reference.Id
                    else sanitizeTypeName arrayItemsType.Title

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

    let alreadyContainsProperty (name: string) =
        addedFields
        |> Seq.exists (fun (fieldName, _, _) -> fieldName = name)

    let rec handleAllOf (currentSchema: OpenApiSchema) =
        if not (isNull currentSchema.AllOf) then
            for innerSchema in currentSchema.AllOf do
                if innerSchema.Type = "object" || isNotNull innerSchema.Properties then
                    for property in innerSchema.Properties do
                        match createPropertyType property.Key property.Value with
                        | None -> ()
                        | Some fieldType ->
                            let propertyName = property.Key
                            let propertyType = property.Value
                            let required = schema.Required.Contains propertyName
                            let field = SynFieldRcd.Create(propertyName, fieldType)
                            let docs = xmlDocs propertyType.Description
                            if not (alreadyContainsProperty propertyName) then
                                recordFields.Add { field with XmlDoc = docs }
                                addedFields.Add((propertyName, required, fieldType))

                if isNotNull innerSchema.AllOf && innerSchema.AllOf.Count > 0 then
                    // handle recursive allOf references
                    handleAllOf innerSchema

    handleAllOf schema

    for property in schema.Properties do
        match createPropertyType property.Key property.Value with
        | None -> ()
        | Some fieldType ->
            let propertyName = property.Key
            let propertyType = property.Value
            let required = schema.Required.Contains propertyName
            let field = SynFieldRcd.Create(propertyName, fieldType)
            let docs = xmlDocs propertyType.Description
            if not (alreadyContainsProperty propertyName) then
                recordFields.Add { field with XmlDoc = docs }
                addedFields.Add((propertyName, required, fieldType))

    let containsPreservedProperty =
        schema.Properties |> Seq.exists (fun prop -> prop.Key = "additionalProperties")

    let includeAdditionalProperties =
        schema.AdditionalPropertiesAllowed
        && not (isNull schema.AdditionalProperties)
        && not containsPreservedProperty
        && not (isNull schema.AdditionalProperties.Type && schema.Properties.Count > 0)

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
    elif recordFields.Count = 0 then
        // couldn't add any fields
        let valueType =
            if config.target = Target.FSharp
            then SynType.JToken()
            else SynType.Object()
        let dictionaryType = SynType.Map(SynType.String(), valueType)
        [ createTypeAbbreviation recordName dictionaryType ]
    else
        let odataTypeNameField = "ODataTypeName"
        if config.odataSchema && config.target = Target.FSharp && not (String.IsNullOrWhiteSpace schema.Title) then
            let required = false
            let fieldType = SynType.Option(SynType.String())
            let recordField = SynFieldRcd.Create(odataTypeNameField, fieldType)
            let attributes = SynAttributeList.Create [
                // create [<JsonProperty "@odata.type">]
                SynAttribute.Create([ Ident.Create "Newtonsoft"; Ident.Create "Json"; Ident.Create "JsonProperty" ], SynConst.CreateString "@odata.type")
            ]
            recordFields.Insert(0, { recordField with Attributes = [ attributes ] })
            addedFields.Insert(0, (odataTypeNameField, required, fieldType))
        elif config.odataSchema && not (String.IsNullOrWhiteSpace schema.Title) then
            // fable
            let propertyName = "@odata.type"
            let required = false
            let fieldType = SynType.Option(SynType.String())

            recordFields.Insert(0, SynFieldRcd.Create(propertyName, fieldType))
            addedFields.Insert(0, (propertyName, required, fieldType))

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
                                                        elif fieldName = "@odata.type" || (fieldName = "ODataTypeName" && config.odataSchema) then
                                                            ()
                                                        else
                                                            if required then yield SynPatRcd.Typed {
                                                                Type = fieldType
                                                                Pattern = SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString (camelCase fieldName), [])
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
                                    elif fieldName = "@odata.type" || (fieldName = "ODataTypeName" && config.odataSchema)
                                    then Some(SynExpr.CreatePartialApp([ "Some" ], [ SynExpr.CreateConstString $"#{schema.Title}" ]))
                                    elif required
                                    then Some(SynExpr.Ident(Ident.Create (camelCase fieldName)))
                                    else Some(SynExpr.Ident(Ident.Create "None"))
                                ((LongIdentWithDots.CreateFromLongIdent([ Ident.Create fieldName ]), false), expr)
                        ]
                }
        ]

        let anyFieldHasDots =
            addedFields
            |> Seq.exists (fun (fieldName, _, _) -> (fieldName.Contains "." || fieldName.Contains "/" || fieldName.Contains "@") && fieldName <> "@odata.type")

        // when fields have dots, they are not escaped for some reason
        // TODO: fix it later in fantomas
        // right now, we just won't generate the `Create` function
        let eventualMembers =
            if anyFieldHasDots || factory = FactoryFunction.None
            then []
            else members

        [
            yield! nestedObjects
            SynModuleDecl.CreateSimpleType(info, simpleRecordType, eventualMembers)
        ]

/// <summary>
/// Rewrites application/vnd.api+json into application/json to simplify the rest of the codegen pipeline
/// </summary>
/// <param name="operation">The operation to rewrite</param>
let rewriteOperationVendorJson  (operation: OpenApiOperation) =
    for response in operation.Responses do
        if response.Value.Content.ContainsKey "application/vnd.api+json" && not (response.Value.Content.ContainsKey "application/json") then
            let mediaType = response.Value.Content.["application/vnd.api+json"]
            response.Value.Content.Remove "application/vnd.api+json" |> ignore
            response.Value.Content.Add("application/json", mediaType)

    if isNotNull operation.RequestBody && isNotNull operation.RequestBody.Content then
        if operation.RequestBody.Content.ContainsKey "application/vnd.api+json" && not (operation.RequestBody.Content.ContainsKey "application/json") then
            let mediaType = operation.RequestBody.Content.["application/vnd.api+json"]
            operation.RequestBody.Content.Remove "application/vnd.api+json" |> ignore
            operation.RequestBody.Content.Add("application/json", mediaType)

let createResponseType (operation: OpenApiOperation) (path: string) (operationType: OperationType) (visitedTypes: ResizeArray<string>) (config: CodegenConfig) (document: OpenApiDocument) =
    // rewrite application/vnd.api+json into application/json
    rewriteOperationVendorJson operation
    let intermediateTypes = ResizeArray<SynModuleDecl>()
    let operationName = deriveOperationName (capitalize operation.OperationId) path operationType visitedTypes
    visitedTypes.Add operationName
    // hack: add it to the operation and retrieve it later
    operation.Extensions.Add("ResponseTypeName", new Microsoft.OpenApi.Any.OpenApiString(operationName))
    let rec getFieldType (schema: OpenApiSchema) (status: string) (wrapODataResponse: bool) =
        match schema.Type with
        | "integer" when schema.Format = "int64" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Int64())
            else SynType.Int64()
        | "integer" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Int())
            else SynType.Int()
        | "number" when schema.Format = "float" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Float32())
            else SynType.Float32()
        | "number" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Double())
            else SynType.Double()
        | "boolean" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Bool())
            else SynType.Bool()

        | "string" when schema.Format = "uuid" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Guid())
            else SynType.Guid()
        | "string" when schema.Format = "guid" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.Guid())
            else SynType.Guid()
        | "string" when schema.Format = "date-time" ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.DateTimeOffset())
            else SynType.DateTimeOffset()
        | "string" when schema.Format = "byte" ->
            // base64 encoded characters
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.ByteArray())
            else SynType.ByteArray()
        | "file" ->
            SynType.ByteArray()
        | "array" when isNotNull schema.Items ->
            let elementSchema = schema.Items
            let elementType = getFieldType elementSchema status false
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.List elementType)
            else SynType.List elementType
        | "array" ->
            // element type schema is null
            let elementType =
                if config.target = Target.FSharp
                then SynType.CreateLongIdent "Newtonsoft.Json.Linq.JArray"
                else SynType.ResizeArray(SynType.Create "obj")

            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(elementType)
            else elementType

        | _ when not (isNull schema.Reference) ->
            // working with a reference type
            let typeName =
                if invalidTitle schema.Title
                then sanitizeTypeName schema.Reference.Id
                else sanitizeTypeName schema.Title
            SynType.Create typeName
        | _ when schema.AdditionalPropertiesAllowed && not (isNull schema.AdditionalProperties) ->
            let valueType = getFieldType schema.AdditionalProperties status false
            let keyType = SynType.String()
            SynType.Map(keyType, valueType)
        | "object" ->
            let recordName = $"{operationName}_{status}"
            visitedTypes.Add recordName
            let factory = FactoryFunction.None
            for generatedType in createRecordFromSchema recordName schema visitedTypes config document factory do
                intermediateTypes.Add generatedType
            SynType.Create recordName
        | _ ->
            if config.odataSchema && wrapODataResponse
            then SynType.ODataResponse(SynType.String())
            else SynType.String()

    let hasLoosePayloadRequestBody =
        isNotNull operation.RequestBody
        && operation.RequestBody.Content.ContainsKey MediaTypes.ApplicationJson
        && isNotNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema
        && operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Type = "object"
        && isNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Reference
        && isNotNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Properties
        && operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Properties.Count > 0

    if hasLoosePayloadRequestBody then
        let schema = operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema
        let payloadTypeName = $"{operationName}Payload"
        visitedTypes.Add payloadTypeName
        let factory = FactoryFunction.Create
        for generatedType in createRecordFromSchema payloadTypeName schema visitedTypes config document factory do
            intermediateTypes.Add generatedType
        operation.Extensions.Add("RequestTypePayload", new Microsoft.OpenApi.Any.OpenApiString(payloadTypeName))

    let hasArrayOfLooseObjectsInRequestPayload =
        isNotNull operation.RequestBody
        && operation.RequestBody.Content.ContainsKey MediaTypes.ApplicationJson
        && isNotNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema
        && operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Type = "array"
        && isNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Reference
        && isNotNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Items
        && operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Items.Type = "object"
        && isNotNull operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Items.Properties
        && operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Items.Properties.Count > 0

    if hasArrayOfLooseObjectsInRequestPayload then
        let payloadTypeName = $"{operationName}Payload"
        let elementTypeName = $"{payloadTypeName}ArrayItem"
        let schema = operation.RequestBody.Content.[MediaTypes.ApplicationJson].Schema.Items
        visitedTypes.Add payloadTypeName
        visitedTypes.Add elementTypeName
        let factory = FactoryFunction.Create
        for generatedType in createRecordFromSchema elementTypeName schema visitedTypes config document factory do
            intermediateTypes.Add generatedType
        intermediateTypes.Add(createTypeAbbreviation payloadTypeName (SynType.List(SynType.Create elementTypeName)))
        operation.Extensions.Add("RequestTypePayload", new Microsoft.OpenApi.Any.OpenApiString(payloadTypeName))

    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [
            SynAttributeList.Create [
                SynAttribute.RequireQualifiedAccess()
            ]
        ]
        Id = [ Ident.Create operationName ]
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
                            let fieldType = getFieldType responsePayloadType.Schema caseName true
                            [SynFieldRcd.Create("payload", fieldType).FromRcd]
                        elif isNotNull responsePayloadType.Schema && isEmptySchema responsePayloadType.Schema then
                            if responsePayloadType.Schema.AdditionalPropertiesAllowed && isNotNull responsePayloadType.Schema.AdditionalProperties then
                                let valueType = getFieldType responsePayloadType.Schema.AdditionalProperties caseName true
                                let keyType = SynType.String()
                                let fieldType =  SynType.Map(keyType, valueType)
                                [SynFieldRcd.Create("payload", fieldType).FromRcd]
                            elif isNotNull responsePayloadType.Schema.Reference then
                                // reference to an empty schema
                                if config.emptyDefinitions = EmptyDefinitionResolution.GenerateFreeForm then
                                    let fieldType = getFieldType responsePayloadType.Schema caseName true
                                    [SynFieldRcd.Create("payload", fieldType).FromRcd]
                                else
                                    []
                            elif responsePayloadType.Schema.Type = "object" then
                                if config.target = Target.FSharp then
                                    let fieldType = SynType.JToken()
                                    [SynFieldRcd.Create("payload", fieldType).FromRcd]
                                else
                                    let fieldType = SynType.Object()
                                    [SynFieldRcd.Create("payload", fieldType).FromRcd]
                            else
                                []
                        else
                            []
                    elif response.Value.Content.ContainsKey "*/*" then
                        let responsePayloadType = response.Value.Content.["*/*"]
                        if not (isNull responsePayloadType.Schema) && not (isEmptySchema responsePayloadType.Schema) then
                            let fieldType = getFieldType responsePayloadType.Schema caseName false
                            [SynFieldRcd.Create("payload", fieldType).FromRcd]
                        else
                            []
                    elif response.Value.Content.ContainsKey "text/plain" && isNotNull response.Value.Content.["text/plain"].Schema then
                        let fieldType = SynType.String()
                        [SynFieldRcd.Create("text", fieldType).FromRcd]
                    elif response.Value.Content.ContainsKey "application/octet-stream" || response.Value.Content.ContainsKey "application/pdf" || response.Value.Content.ContainsKey "application/zip" then
                        let fieldType = SynType.ByteArray()
                        [SynFieldRcd.Create("payload", fieldType).FromRcd]
                    elif response.Value.Content.ContainsKey "image/png" && isNotNull response.Value.Content.["image/png"].Schema && response.Value.Content.["image/png"].Schema.Format = "binary" then
                        let fieldType = SynType.ByteArray()
                        [SynFieldRcd.Create("payload", fieldType).FromRcd]
                    elif response.Value.Content.ContainsKey "image/png" then
                        let fieldType = SynType.ByteArray()
                        [SynFieldRcd.Create("payload", fieldType).FromRcd]
                    else
                        []
                let docs = xmlDocs response.Value.Description
                yield SynUnionCase.UnionCase([], Ident.Create (capitalize caseName), SynUnionCaseType.UnionCaseFields fieldTypes, docs, None, range0)
            | None ->
                ()

        if not containsOkOrDefault then
            let docs = PreXmlDoc.Empty
            yield SynUnionCase.UnionCase([], Ident.Create (capitalize "DefaultResponse"), SynUnionCaseType.UnionCaseFields [], docs, None, range0)
    ])


    let simpleType = SynTypeDefnSimpleReprRcd.Union(enumRepresentation)

    [
        yield! intermediateTypes
        yield SynModuleDecl.CreateSimpleType(info, simpleType, [])
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

// type ODataResponse<'TValue> = { value: 'TValue }
let createODataResponse(config: CodegenConfig) =
    let valueTypeArg = SynTypar.Typar(Ident.Create "TValue", TyparStaticReq.NoStaticReq, false)

    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create "ODataResponse" ]
        XmlDoc = PreXmlDoc.Empty
        Parameters = [ SynTyparDecl.TyparDecl([], valueTypeArg) ]
        Constraints = [ ]
        PreferPostfix = true
        Range = range0
    }

    let attributes = SynAttributeList.Create [
        // [<JsonProperty "@odata.context">]
        SynAttribute.Create([ Ident.Create "Newtonsoft"; Ident.Create "Json"; Ident.Create "JsonProperty" ], SynConst.CreateString "@odata.context")
    ]

    let odataContextField = SynFieldRcd.Create("ODataContext", SynType.Option(SynType.String()))

    let recordRepr = SynTypeDefnSimpleReprRecordRcd.Create [
        if config.target = Target.FSharp then
            { odataContextField with Attributes = [ attributes ] }
        SynFieldRcd.Create("value", SynType.Var(valueTypeArg, range0))
    ]

    let simpleRecordType = SynTypeDefnSimpleReprRcd.Record recordRepr

    SynModuleDecl.CreateSimpleType(info, simpleRecordType)


let rec isPrimitiveAllOf (schema: OpenApiSchema) =
    if isNotNull schema.AllOf && schema.AllOf.Count > 0 then
        schema.AllOf
        |> Seq.forall(fun innerSchema ->
            let isPrimitive =
                innerSchema.Type = "string"
                || innerSchema.Type = "boolean"
                || innerSchema.Type = "integer"
                || innerSchema.Type = "number"

            if isNotNull innerSchema.AllOf && innerSchema.AllOf.Count > 0 then
                isPrimitive && isPrimitiveAllOf innerSchema
            else
                isPrimitive
        )
    else
        false

let rec collectPrimitiveAllOf (schema: OpenApiSchema) =
    if isNotNull schema.AllOf then
        [
            for innerSchema in schema.AllOf do
                yield innerSchema.Type

                if isNotNull innerSchema.AllOf then
                    for nestedSchema in innerSchema.AllOf do
                        yield! collectPrimitiveAllOf nestedSchema
        ]
    else
        [

        ]

let includeOperation (operation: OpenApiOperation) (config: CodegenConfig) : bool =
    if config.filterTags.IsEmpty then
        true
    elif operation.Tags.Count = 0 && config.filterTags.Length > 0 then
        false
    else
        operation.Tags
        |> Seq.exists (fun tag ->
            config.filterTags
            |> List.exists (fun configTag -> tag.Name.StartsWith configTag)
        )

let createGlobalTypesModule (openApiDocument: OpenApiDocument) (config: CodegenConfig) =
    let visitedTypes = ResizeArray<string>()
    let moduleTypes = ResizeArray<SynModuleDecl>()

    if config.odataSchema then
        if config.target = Target.Fable then
            // Fable target will output @odata.type
            moduleTypes.Add (SynModuleDecl.CreateHashDirective("nowarn", [ "1104" ]))
        moduleTypes.Add (createODataResponse config)
        visitedTypes.Add "ODataResponse"

    if isNotNull openApiDocument.Components then

        // first add all global enum types
        for topLevelObject in openApiDocument.Components.Schemas do
            let typeName =
                if invalidTitle topLevelObject.Value.Title
                then sanitizeTypeName topLevelObject.Key
                else sanitizeTypeName topLevelObject.Value.Title

            if topLevelObject.Value.Type = "string" then
                match topLevelObject.Value with
                | StringEnum cases ->
                    // create global enum type
                    moduleTypes.Add (createEnumType typeName cases (Some topLevelObject.Value.Description))
                    visitedTypes.Add typeName
                | _ ->
                    // create abbreviated type
                    let abbreviatedType =
                        match topLevelObject.Value.Format with
                        | "guid" | "uuid" -> SynType.Guid()
                        | "date-time" -> SynType.DateTimeOffset()
                        | "byte" -> SynType.ByteArray()
                        | _ -> SynType.String()

                    moduleTypes.Add (createTypeAbbreviationWithDocs typeName abbreviatedType topLevelObject.Value.Description)
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
            elif isPrimitiveAllOf topLevelObject.Value then
                let collectedTypes = collectPrimitiveAllOf topLevelObject.Value
                let primitiveType =
                    collectedTypes
                    |> List.groupBy id
                    |> List.sortByDescending (fun (key, types) -> List.length types)
                    |> List.tryHead
                    |> Option.map (fun (key, types) -> key)

                match primitiveType with
                | Some "string" ->
                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.String()))
                    visitedTypes.Add typeName
                | Some "boolean" ->
                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.Bool()))
                    visitedTypes.Add typeName
                | Some "integer" ->
                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.Int()))
                    visitedTypes.Add typeName
                | Some "number" ->
                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.Double()))
                    visitedTypes.Add typeName
                | _ ->
                    ()
            elif topLevelObject.Value.Type = "array" then
                let elementType = topLevelObject.Value.Items
                if isNull elementType then
                    if config.target = Target.FSharp then
                        moduleTypes.Add (createTypeAbbreviation typeName (SynType.JArray()))
                        visitedTypes.Add typeName
                    else
                        moduleTypes.Add (createTypeAbbreviation typeName (SynType.List(SynType.Object())))
                        visitedTypes.Add typeName
                elif isNotNull elementType.Reference && isNotNull elementType.Reference.Id then
                    let referencedType = elementType.Reference.Id
                    moduleTypes.Add (createTypeAbbreviation typeName (SynType.List(SynType.Create referencedType)))
                    visitedTypes.Add typeName
                elif elementType.Type = "string" then
                    match elementType with
                    | StringEnum cases ->
                        // create global enum type
                        let enumTypeName = $"EnumFor{typeName}";
                        moduleTypes.Add (createEnumType enumTypeName cases None)
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

                        moduleTypes.Add (createTypeAbbreviationWithDocs typeName listOfAbbrev topLevelObject.Value.Description)
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
                elif elementType.Type = "object" && not (visitedTypes.Contains $"{typeName}ArrayItem") then
                    let elementTypeName = $"{typeName}ArrayItem"
                    visitedTypes.Add typeName
                    visitedTypes.Add elementTypeName
                    let factory = FactoryFunction.Create
                    for createdType in createRecordFromSchema elementTypeName elementType visitedTypes config openApiDocument factory do
                        moduleTypes.Add createdType
                    let abbreviatedType = SynType.List(SynType.Create elementTypeName)
                    moduleTypes.Add (createTypeAbbreviation typeName abbreviatedType)
            else
                ()

        // then handle the global objects
        for topLevelObject in openApiDocument.Components.Schemas do
            let canUseTitle =
                not (invalidTitle topLevelObject.Value.Title)
                && not (isGlobalRef topLevelObject.Value.Title openApiDocument)

            let typeName =
                if canUseTitle
                then sanitizeTypeName topLevelObject.Value.Title
                else sanitizeTypeName topLevelObject.Key

            if config.odataSchema then
                topLevelObject.Value.Title <- topLevelObject.Key

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

            if isEmptySchema topLevelObject.Value then
                match config.emptyDefinitions with
                | EmptyDefinitionResolution.Ignore -> ()
                | EmptyDefinitionResolution.GenerateFreeForm ->
                    let freeFormType =
                        if config.target = Target.FSharp
                        then SynType.JToken()
                        else SynType.Object()
                    moduleTypes.Add (createTypeAbbreviationWithDocs typeName freeFormType topLevelObject.Value.Description)
                    visitedTypes.Add typeName
            elif isPrimitiveAllOf topLevelObject.Value then
                // handled in primitive types case
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
                if not (visitedTypes.Contains typeName) then
                    visitedTypes.Add typeName
                    let factory = FactoryFunction.Create
                    for createdType in createRecordFromSchema typeName topLevelObject.Value visitedTypes config openApiDocument factory do
                        moduleTypes.Add createdType
            else
                ()

    for path in safeSeq openApiDocument.Paths do
        for operation in safeSeq path.Value.Operations do
            if includeOperation operation.Value config then
                let responseTypes = createResponseType operation.Value path.Key operation.Key visitedTypes config openApiDocument
                moduleTypes.AddRange responseTypes

    let globalTypesModule = CodeGen.createNamespace [ config.project; "Types" ] (Seq.toList moduleTypes)

    visitedTypes, globalTypesModule

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

let responseContainsBinaryOutput (response: OpenApiResponse) =
    if response.Content.ContainsKey MediaTypes.ApplicationJson then
        let jsonResponse = response.Content.[MediaTypes.ApplicationJson]
        let hasStringByteOutput =
            isNotNull jsonResponse.Schema
            && jsonResponse.Schema.Type = "string"
            && jsonResponse.Schema.Format = "byte"

        let hasFileOutput =
            isNotNull jsonResponse.Schema
            && jsonResponse.Schema.Type = "file"

        let hasBinaryOutput = hasStringByteOutput || hasFileOutput
        hasBinaryOutput
    else
        let hasBinaryOutput =
            response.Content.ContainsKey MediaTypes.OctetStream
            || response.Content.ContainsKey MediaTypes.ApplicationPdf
            || response.Content.ContainsKey MediaTypes.ApplicationZip
            || response.Content.ContainsKey MediaTypes.AppliationZipCompressed
            || response.Content.ContainsKey MediaTypes.ImagePng
            || response.Content.ContainsKey MediaTypes.ImageJpg
            || response.Content.ContainsKey MediaTypes.ImageJpeg
            || response.Content.ContainsKey MediaTypes.ImageGif
            || response.Content.ContainsKey MediaTypes.ImagePng
        hasBinaryOutput

let containsBinaryResponse (operation: OpenApiOperation) =
    operation.Responses
    |> Seq.exists (fun pair -> responseContainsBinaryOutput pair.Value)

let createIdent xs = SynExpr.CreateLongIdent(LongIdentWithDots.Create xs)
let stringExpr value = SynExpr.CreateConstString value
let createLetAssignment leftSide rightSide continuation =
    let emptySynValData = SynValData.SynValData(None, SynValInfo.Empty, None)
    let headPat = SynPat.Named(SynPat.Wild range0, leftSide, false, None, range0)
    let binding = SynBinding.Binding(None, SynBindingKind.NormalBinding, false, false, [], PreXmlDoc.Empty, emptySynValData, headPat, None, rightSide, range0, DebugPointForBinding.DebugPointAtBinding range0 )
    SynExpr.LetOrUse(false, false, [binding], continuation, range0)

let createLetBangAssignment leftSide body continuation =
    let emptySynValData = SynValData.SynValData(None, SynValInfo.Empty, None)
    let headPat = SynPat.Named(SynPat.Wild range0, leftSide, false, None, range0)
    SynExpr.LetOrUseBang(DebugPointForBinding.DebugPointAtBinding range0, false, false, headPat, body, [], continuation, range0)

let createOpenApiClient
    (openApiDocument: OpenApiDocument)
    (visitedTypes: ResizeArray<string>)
    (config: CodegenConfig) =

    let extraTypes = ResizeArray<SynModuleDecl>()
    let clientTypeName = $"{config.project}Client"
    let info : SynComponentInfoRcd = {
        Access = None
        Attributes = [ ]
        Id = [ Ident.Create clientTypeName ]
        XmlDoc = xmlDocs openApiDocument.Info.Description
        Parameters = [ ]
        Constraints = [ ]
        PreferPostfix = false
        Range = range0
    }

    let clientMembers = ResizeArray<SynMemberDefn>()

    let httpClient = SynSimplePat.CreateTyped(Ident.Create "httpClient", SynType.Create "HttpClient")
    let urlContructorParam = SynSimplePat.CreateTyped(Ident.Create "url", SynType.String())
    let headersConstructorParam = SynSimplePat.CreateTyped(Ident.Create "headers", SynType.List(SynType.Create "Header"))

    if config.target = Target.FSharp then
        clientMembers.Add(SynMemberDefn.CreateImplicitCtor [ httpClient ])
    else
        clientMembers.Add(SynMemberDefn.CreateImplicitCtor [
            urlContructorParam
            headersConstructorParam
        ])

        let emptyHeadersList = SynExpr.CreateList [ ]
        let synValDataAsConstructor =
            match SynBindingRcd.Null.ValData with
            | SynValData(Some memberFlags, synValInfo, ident) ->
                let modifiedFlags = { memberFlags with MemberKind = MemberKind.Constructor }
                SynValData(Some modifiedFlags, synValInfo, ident)
            | _ ->
                SynBindingRcd.Null.ValData

        // generates new(url: string) = Client(url, [])
        // to initialize the client without extra headers
        let implicitConstructor = SynMemberDefn.CreateMember {
            SynBindingRcd.Null with
                XmlDoc = PreXmlDoc.Empty
                ValData = synValDataAsConstructor
                Expr = SynExpr.CreatePartialApp([ clientTypeName ], [ SynExpr.CreateParenedTuple [ createIdent ["url"]; emptyHeadersList ] ])
                Pattern = SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString "new", [
                    SynPatRcd.CreateParen(
                        SynPatRcd.Typed {
                            Range = range0
                            Type = SynType.String()
                            Pattern = SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString("url"), [])
                        })
                ])
        }

        clientMembers.Add(implicitConstructor)

    for path in safeSeq openApiDocument.Paths do
        let fullPath = path.Key
        let pathInfo = path.Value
        for operation in safeSeq pathInfo.Operations do
            let operationInfo = operation.Value
            if not operationInfo.Deprecated && includeOperation operationInfo config then

                if config.target = Target.FSharp then
                    operationInfo.Parameters.Add(OpenApiParameter(
                        Name = "cancellationToken",
                        In = ParameterLocation.Query,
                        Schema = OpenApiSchema(Reference = OpenApiReference(Id = "CancellationToken"))))

                let parameters = operationParameters operationInfo pathInfo.Parameters config

                let summary =
                    if String.IsNullOrWhiteSpace operationInfo.Description
                    then operationInfo.Summary
                    else operationInfo.Description

                let parameterDocs = [
                    for p in parameters -> (p.parameterIdent, p.docs)
                ]

                let hasBinaryResponse = containsBinaryResponse operation.Value
                let memberName = deriveMemberName operationInfo.OperationId fullPath operation.Key

                let contentIdent =
                    if hasBinaryResponse
                    then "contentBinary"
                    else "content"

                // for async calls
                // creates let! (status, content) = {body} in {continuation}
                let deconstructAsyncResponse body continuation =
                    let status = SynPat.Named(SynPat.Wild range0, Ident.Create "status", false, None, range0)
                    let content = SynPat.Named(SynPat.Wild range0, Ident.Create contentIdent, false, None, range0)
                    let headPat = SynPat.Paren(SynPat.Tuple(false, [ status; content ], range0), range0)
                    SynExpr.LetOrUseBang(DebugPointForBinding.DebugPointAtBinding range0, false, false, headPat, body, [], continuation, range0)

                // for synchronous calls
                // creates let (status, content) = {body} in {continuation}
                let deconstructResponse body continuation =
                    let emptySynValData = SynValData.SynValData(None, SynValInfo.Empty, None)
                    let status = SynPat.Named(SynPat.Wild range0, Ident.Create "status", false, None, range0)
                    let content = SynPat.Named(SynPat.Wild range0, Ident.Create contentIdent, false, None, range0)
                    let headPat = SynPat.Paren(SynPat.Tuple(false, [ status; content ], range0), range0)
                    let binding = SynBinding.Binding(None, SynBindingKind.NormalBinding, false, false, [], PreXmlDoc.Empty, emptySynValData, headPat, None, body, range0, DebugPointForBinding.DebugPointAtBinding range0 )
                    SynExpr.LetOrUse(false, false, [binding], continuation, range0)

                let requestValues = [
                    for parameter in parameters do
                        if parameter.required then
                            if parameter.properties.Length = 0 then
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
                                for property in parameter.properties do
                                yield SynExpr.CreatePartialApp(["RequestPart"; parameter.location], [
                                    SynExpr.CreateParen(SynExpr.CreateTuple [
                                        stringExpr ($"{parameter.parameterName}[{property}]")
                                        createIdent [ parameter.parameterIdent; property ]
                                    ])
                                ])
                        elif parameter.style <> "cancellation-token" then
                            let condition = createIdent [ parameter.parameterIdent; "IsSome" ]
                            let value =
                                if parameter.properties.Length = 0 then
                                    SynExpr.CreatePartialApp([ "RequestPart"; parameter.location ], [
                                        if parameter.location <> "jsonContent" && parameter.location <> "binaryContent" then
                                            SynExpr.CreateParen(SynExpr.CreateTuple [
                                                stringExpr parameter.parameterName
                                                createIdent [ parameter.parameterIdent; "Value" ]
                                            ])
                                        else
                                            createIdent [ parameter.parameterIdent; "Value" ]
                                    ])
                                else
                                    let parametersToYield = [
                                        for property in parameter.properties do
                                            SynExpr.CreatePartialApp([ "RequestPart"; parameter.location ], [
                                                SynExpr.CreateParen(SynExpr.CreateTuple [
                                                    stringExpr ($"{parameter.parameterName}[{property}]")
                                                    createIdent [ parameter.parameterIdent; "Value"; property ]
                                                ])
                                            ])
                                    ]

                                    SynExpr.CreateSequential(parametersToYield)

                            yield SynExpr.CreateIfThen(condition, value)
                ]

                let httpFunction =
                    if hasBinaryResponse
                    then $"{operation.Key.ToString().ToLower()}Binary"
                    else operation.Key.ToString().ToLower()

                let httpFunctionAsync = $"{httpFunction}Async"

                let requestParts = Ident.Create "requestParts"
                let httpCall httpFunc = SynExpr.CreatePartialApp(["OpenApiHttp"; httpFunc], [
                    if config.target = Target.FSharp then
                        // only use the HttpClient on F#/dotnet clients
                        SynExpr.CreateIdent (Ident.Create "httpClient")
                        SynExpr.CreateConstString fullPath
                        SynExpr.Ident requestParts
                        SynExpr.Ident <| Ident.Create "cancellationToken"
                    else
                        // apply the base path to the generated functions
                        SynExpr.CreateIdent (Ident.Create "url")
                        // the path the of the end point
                        SynExpr.CreateConstString fullPath
                        // the extra headers provided from the constructor
                        SynExpr.CreateIdent (Ident.Create "headers")
                        SynExpr.Ident requestParts
                ])

                let wrappedReturn expr =
                    match config.target with
                    | Target.FSharp when config.synchronous -> expr
                    | _ -> SynExpr.CreateReturn expr

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

                let responseType =
                    if operationInfo.Extensions.ContainsKey "ResponseTypeName" then
                        match operationInfo.Extensions.["ResponseTypeName"] with
                        | :? Microsoft.OpenApi.Any.OpenApiString as responseTypeName -> responseTypeName.Value
                        | _ -> capitalize memberName
                    else
                        capitalize memberName

                let returnExpr =
                    let createOutput (status: string,response: OpenApiResponse) =
                        if response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "string" && response.Content.["application/json"].Schema.Format = "byte" then
                            // Assume we have a binary response
                            SynExpr.CreatePartialApp([responseType; status], [
                                createIdent [ contentIdent ]
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "string" && (response.Content.["application/json"].Schema.Format = "uuid" || response.Content.["application/json"].Schema.Format = "guid") then
                            SynExpr.CreatePartialApp([responseType; status], [
                                SynExpr.CreateParen(
                                    SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                        createIdent [ "content" ]
                                    ])
                                )
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "file" then
                            // Assume we have a binary response
                            SynExpr.CreatePartialApp([responseType; status], [
                                createIdent [ contentIdent ]
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "string" then
                            if hasBinaryResponse && config.target = Target.FSharp then
                                let body = SynExpr.CreatePartialApp(["Encoding"; "UTF8"; "GetString"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        createIdent [ "content" ]
                                    ])
                                    |> wrappedReturn
                                )
                            elif hasBinaryResponse && config.target = Target.Fable then
                                let body = SynExpr.CreatePartialApp(["Utilities"; "readBytesAsText"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetBangAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        createIdent [ "content" ]
                                    ])
                                    |> wrappedReturn
                                )
                            else
                                if config.odataSchema then
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                else
                                    // when the media type is JSON but the return type is string
                                    // read the string as is without deserialization
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        createIdent [ "content" ]
                                    ])
                                    |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "integer" && config.odataSchema then
                            // OData Schema and integer response schema combo
                            SynExpr.CreatePartialApp([responseType; status], [
                                SynExpr.CreateParen(
                                    SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                        createIdent [ "content" ]
                                    ])
                                )
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "boolean" && config.odataSchema then
                            // OData Schema and boolean response schema combo
                            SynExpr.CreatePartialApp([responseType; status], [
                                SynExpr.CreateParen(
                                    SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                        createIdent [ "content" ]
                                    ])
                                )
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && response.Content.["application/json"].Schema.Type = "number" && config.odataSchema then
                            // OData Schema and number response schema combo
                            SynExpr.CreatePartialApp([responseType; status], [
                                SynExpr.CreateParen(
                                    SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                        createIdent [ "content" ]
                                    ])
                                )
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && not (isEmptySchema response.Content.["application/json"].Schema) then
                            if hasBinaryResponse && config.target = Target.FSharp then
                                let body = SynExpr.CreatePartialApp(["Encoding"; "UTF8"; "GetString"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                )
                            elif hasBinaryResponse && config.target = Target.Fable then
                                let body = SynExpr.CreatePartialApp(["Utilities"; "readBytesAsText"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetBangAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                )
                            else
                                SynExpr.CreatePartialApp([responseType; status], [
                                    SynExpr.CreateParen(
                                        SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                            createIdent [ "content" ]
                                        ])
                                    )
                                ])
                                |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && isEmptySchema response.Content.["application/json"].Schema && (isNotNull response.Content.["application/json"].Schema.AdditionalProperties || response.Content.["application/json"].Schema.Type = "object") then
                            if hasBinaryResponse && config.target = Target.FSharp then
                                let body = SynExpr.CreatePartialApp(["Encoding"; "UTF8"; "GetString"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                )
                            elif hasBinaryResponse && config.target = Target.Fable then
                                let body = SynExpr.CreatePartialApp(["Utilities"; "readBytesAsText"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetBangAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                )
                            else
                                SynExpr.CreatePartialApp([responseType; status], [
                                    SynExpr.CreateParen(
                                        SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                            createIdent [ "content" ]
                                        ])
                                    )
                                ])
                                |> wrappedReturn
                        elif response.Content.ContainsKey "application/json" && isNotNull response.Content.["application/json"].Schema && isEmptySchema response.Content.["application/json"].Schema then
                            // reference to an empty schema
                            if config.emptyDefinitions = EmptyDefinitionResolution.GenerateFreeForm then
                                if hasBinaryResponse && config.target = Target.FSharp then
                                    let body = SynExpr.CreatePartialApp(["Encoding"; "UTF8"; "GetString"], [
                                        createIdent [ "contentBinary" ]
                                    ])

                                    createLetAssignment (Ident.Create "content") body (
                                        // continuation
                                        SynExpr.CreatePartialApp([responseType; status], [
                                            SynExpr.CreateParen(
                                                SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                    createIdent [ "content" ]
                                                ])
                                            )
                                        ])
                                        |> wrappedReturn
                                    )
                                elif hasBinaryResponse && config.target = Target.Fable then
                                    let body = SynExpr.CreatePartialApp(["Utilities"; "readBytesAsText"], [
                                        createIdent [ "contentBinary" ]
                                    ])

                                    createLetBangAssignment (Ident.Create "content") body (
                                        // continuation
                                        SynExpr.CreatePartialApp([responseType; status], [
                                            SynExpr.CreateParen(
                                                SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                    createIdent [ "content" ]
                                                ])
                                            )
                                        ])
                                        |> wrappedReturn
                                    )
                                else
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                            else
                                // ignore
                                createIdent [ responseType; status ]
                                |> wrappedReturn

                        elif response.Content.ContainsKey "application/json" && isNull response.Content.["application/json"].Schema then
                            createIdent [ responseType; status ]
                            |> wrappedReturn
                        elif response.Content.ContainsKey "*/*" && isNotNull (response.Content.["*/*"].Schema) && not (isEmptySchema response.Content.["*/*"].Schema) then
                            if hasBinaryResponse && config.target = Target.FSharp then
                                let body = SynExpr.CreatePartialApp(["Encoding"; "UTF8"; "GetString"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                )
                            elif hasBinaryResponse && config.target = Target.Fable then
                                let body = SynExpr.CreatePartialApp(["Utilities"; "readBytesAsText"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetBangAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        SynExpr.CreateParen(
                                            SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                                createIdent [ "content" ]
                                            ])
                                        )
                                    ])
                                    |> wrappedReturn
                                )
                            else
                                SynExpr.CreatePartialApp([responseType; status], [
                                    SynExpr.CreateParen(
                                        SynExpr.CreatePartialApp(["Serializer"; "deserialize"], [
                                            createIdent [ "content" ]
                                        ])
                                    )
                                ])
                                |> wrappedReturn
                        elif response.Content.ContainsKey "text/plain" && isNotNull response.Content.["text/plain"].Schema then
                            if hasBinaryResponse && config.target = Target.FSharp then
                                let body = SynExpr.CreatePartialApp(["Encoding"; "UTF8"; "GetString"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        createIdent [ "content" ]
                                    ])
                                    |> wrappedReturn
                                )
                            elif hasBinaryResponse && config.target = Target.Fable then
                                let body = SynExpr.CreatePartialApp(["Utilities"; "readBytesAsText"], [
                                    createIdent [ "contentBinary" ]
                                ])

                                createLetBangAssignment (Ident.Create "content") body (
                                    // continuation
                                    SynExpr.CreatePartialApp([responseType; status], [
                                        createIdent [ "content" ]
                                    ])
                                    |> wrappedReturn
                                )
                            else
                                SynExpr.CreatePartialApp([responseType; status], [
                                    createIdent [ "content" ]
                                ])
                                |> wrappedReturn
                        elif response.Content.ContainsKey "application/octet-stream" || response.Content.ContainsKey "application/pdf" || response.Content.ContainsKey "application/zip" then
                            SynExpr.CreatePartialApp([responseType; status], [
                                createIdent [ contentIdent ]
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "image/png" && isNotNull response.Content.["image/png"].Schema && response.Content.["image/png"].Schema.Format = "binary" then
                            SynExpr.CreatePartialApp([responseType; status], [
                                createIdent [ contentIdent ]
                            ])
                            |> wrappedReturn
                        elif response.Content.ContainsKey "image/png" then
                            SynExpr.CreatePartialApp([responseType; status], [
                                createIdent [ contentIdent ]
                            ])
                            |> wrappedReturn
                        else
                            createIdent [ responseType; status ]
                            |> wrappedReturn

                    let statusIsEqual status =
                        let statusCode =
                            match status with
                            | nameof HttpStatusCode.OK -> 200
                            | nameof HttpStatusCode.Created -> 201
                            | nameof HttpStatusCode.Accepted -> 202
                            | nameof HttpStatusCode.NoContent -> 204
                            | nameof HttpStatusCode.PartialContent -> 206
                            | nameof HttpStatusCode.MovedPermanently -> 301
                            | nameof HttpStatusCode.Moved -> 301
                            | nameof HttpStatusCode.Found -> 302
                            | nameof HttpStatusCode.BadRequest -> 400
                            | nameof HttpStatusCode.Unauthorized -> 401
                            | nameof HttpStatusCode.PaymentRequired -> 402
                            | nameof HttpStatusCode.Forbidden -> 403
                            | nameof HttpStatusCode.NotFound -> 404
                            | nameof HttpStatusCode.MethodNotAllowed -> 405
                            | nameof HttpStatusCode.Conflict -> 409
                            | nameof HttpStatusCode.UnsupportedMediaType -> 415
                            | nameof HttpStatusCode.RequestedRangeNotSatisfiable -> 416
                            | nameof HttpStatusCode.InternalServerError -> 500
                            | nameof HttpStatusCode.NotImplemented -> 501
                            | nameof HttpStatusCode.BadGateway -> 502
                            | nameof HttpStatusCode.ServiceUnavailable -> 503
                            | nameof HttpStatusCode.GatewayTimeout -> 504
                            | _ -> 0

                        if config.target = Target.FSharp then
                            equal (createIdent [ "status" ]) (createIdent [ "HttpStatusCode"; status ])
                        else
                            equal (createIdent [ "status" ]) (SynExpr.CreateConst(SynConst.Int32 statusCode))

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
                    if config.target = Target.Fable then
                        SynExpr.CreateAsync expr
                    else
                        if config.synchronous then
                            expr
                        else
                            match config.asyncReturnType with
                            | AsyncReturnType.Async -> SynExpr.CreateAsync expr
                            | AsyncReturnType.Task -> SynExpr.CreateTask expr

                let destructExpr httpFunc =
                    match config.target with
                    | Target.FSharp when config.synchronous -> deconstructResponse (httpCall httpFunc) returnExpr
                    | _ -> deconstructAsyncResponse (httpCall httpFunc) returnExpr

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

                match config.target with
                | Target.FSharp when config.synchronous ->
                    clientMembers.Add (clientOperation httpFunction memberName)
                | _ ->
                    clientMembers.Add (clientOperation httpFunctionAsync memberName)

    let clientType = SynModuleDecl.CreateType(info, Seq.toList clientMembers)

    let moduleContents = [
        if config.target = Target.FSharp then
            yield SynModuleDecl.CreateOpen "System.Net"
            yield SynModuleDecl.CreateOpen "System.Net.Http"
            yield SynModuleDecl.CreateOpen "System.Text"
            yield SynModuleDecl.CreateOpen "System.Threading"
        else
            yield SynModuleDecl.CreateOpen "Browser.Types"
            yield SynModuleDecl.CreateOpen "Fable.SimpleHttp"

        yield SynModuleDecl.CreateOpen $"{config.project}.Types"
        yield SynModuleDecl.CreateOpen $"{config.project}.Http"

        if config.asyncReturnType = AsyncReturnType.Task then
            // from the Ply package
            yield SynModuleDecl.CreateOpen "FSharp.Control.Tasks"
        // extra types generated from parameters
        for extraType in extraTypes do
            yield extraType
        // the main http client
        if Seq.length (safeSeq openApiDocument.Paths) > 0 then
            // only when we actually have members
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
                getSchema (processedSchema.ToString()) config.overrideSchema

            elif config.schema.StartsWith "http" && config.schema.EndsWith ".json" then
                getSchema config.schema config.overrideSchema
            elif config.schema.StartsWith "http" && config.schema.EndsWith ".yaml" then
                let schemaContent =
                    config.schema
                    |> client.GetStringAsync
                    |> Async.AwaitTask
                    |> Async.RunSynchronously
                let schemaBytes = Encoding.UTF8.GetBytes(schemaContent)
                new MemoryStream(schemaBytes) :> Stream
            elif config.schema.StartsWith "http" then
                getSchema config.schema config.overrideSchema
            else
                getSchema (resolveFile config.schema) config.overrideSchema
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
            let config = { config with odataSchema = openApiDocument.Extensions.ContainsKey "x-odata" }
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
                        XElement.PackageReference("Fable.Remoting.Json", "2.18.0")
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
            else
                let httpLibrary = HttpLibrary.fableLibrary config.project
                write httpLibrary [ outputDir; "OpenApiHttp.fs" ]

            write (projectFile.ToString()) [ outputDir; $"{config.project}.fsproj" ]
            printfn "Succesfully generated project %s" (path [outputDir; $"{config.project}.fsproj" ])
            0

let showTags filePath =
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
                getSchema (processedSchema.ToString()) config.overrideSchema

            elif config.schema.StartsWith "http" && config.schema.EndsWith ".json" then
                getSchema config.schema config.overrideSchema
            elif config.schema.StartsWith "http" && config.schema.EndsWith ".yaml" then
                let schemaContent =
                    config.schema
                    |> client.GetStringAsync
                    |> Async.AwaitTask
                    |> Async.RunSynchronously
                let schemaBytes = Encoding.UTF8.GetBytes(schemaContent)
                new MemoryStream(schemaBytes) :> Stream
            elif config.schema.StartsWith "http" then
                getSchema config.schema config.overrideSchema
            else
                getSchema (resolveFile config.schema) config.overrideSchema
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
                Console.WriteLine error.Message
            1
        elif isNull openApiDocument then
            Console.WriteLine "Could not parse the OpenAPI schema"
            1
        else
            let tags = [
                for path in safeSeq openApiDocument.Paths do
                for operation in safeSeq path.Value.Operations do
                if isNotNull operation.Value.OperationId then
                    for tag in operation.Value.Tags do tag.Name, operation.Value.OperationId
            ]

            let content =
                tags
                |> List.groupBy fst
                |> List.sortByDescending (fun (tag, operations) -> operations.Length)
                |> List.map (fun (tag, operations) -> $"Tag {tag} has {operations.Length} operations(s)")
                |> String.concat "\n"

            File.WriteAllText("tags.txt", content)
            printfn "Tags information saved to tags.txt"
            0

[<EntryPoint>]
let main argv =
    Console.InputEncoding <- Encoding.UTF8
    Console.OutputEncoding <- Encoding.UTF8
    match argv with
    | [| "--version" |] ->
        printfn "0.64.0"
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
    | [| "--from-odata-schema"; schema; "--output"; output |] ->
        printfn "Generating OpenAPI specs from OData schema at %s" schema
        if schema.StartsWith "http" then
            let schemaWithMetadata =
                if schema.EndsWith "$metadata"
                then schema
                else $"{schema.TrimEnd '/'}/$metadata"
            let openApiSchema = readExternalODataSchema schemaWithMetadata
            let simplified = simplifyRedundantSchemaParts (JObject.Parse openApiSchema)
            File.WriteAllText(resolveFile output, simplified.ToString(Formatting.Indented))
            printfn "Generated OpenAPI specs saved as %s" (resolveFile output)
            0
        elif schema.EndsWith ".xml" && File.Exists (resolveFile schema) then
            let openApiSchema = readLocalODataSchema schema
            let simplified = simplifyRedundantSchemaParts (JObject.Parse openApiSchema)
            File.WriteAllText(resolveFile output, simplified.ToString(Formatting.Indented))
            printfn "Generated OpenAPI specs saved as %s" (resolveFile output)
            0
        else
            printfn "Invalid OData schema"
            printfn "Schema %s" schema
            1

    | [| "--show-tags"; "--config"; filePath |] ->
        printfn "Extracting OpenAPI tags schema at %s" filePath
        showTags filePath
    | [| "--config"; filePath; "--show-tags" |] ->
        printfn "Extracting OpenAPI tags schema at %s" filePath
        showTags filePath
    | [| "--show-tags" |] ->
        showTags "./hawaii.json"
    | arguments ->
        printfn "Unknown arguments [%s]" (String.concat ", " arguments)
        1