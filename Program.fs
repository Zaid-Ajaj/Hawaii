// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Microsoft.OpenApi.Readers
open System.Net.Http
open FsAst
open Fantomas
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open FSharp.Compiler.XmlDoc
open Microsoft.OpenApi.Models

[<AutoOpen>]
module Extensions =
    type SynFieldRcd with
        static member Create(name: string, fieldType: SynType) =
            {
                Access = None
                Attributes = [ ]
                Id = Some (Ident.Create name)
                IsMutable = false
                IsStatic = false
                Range = range0
                Type = fieldType
                XmlDoc= PreXmlDoc.Empty
            }

        static member Create(name: string, fieldType: string) =
            {
                Access = None
                Attributes = [ ]
                Id = Some (Ident.Create name)
                IsMutable = false
                IsStatic = false
                Range = range0
                Type = SynType.Create fieldType
                XmlDoc= PreXmlDoc.Empty
            }

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let schema = "https://petstore.swagger.io/v2/swagger.json"
let project = "PetStore"

let createNamespace (names: seq<string>) declarations =
    let nameParts =
        names
        |> Seq.collect (fun name ->
            if name.Contains "."
            then name.Split('.')
            else [| name |]
        )

    let xmlDoc = PreXmlDoc.Create [ ]
    SynModuleOrNamespace.SynModuleOrNamespace([ for name in nameParts -> Ident.Create name ], true, SynModuleOrNamespaceKind.DeclaredNamespace,declarations,  xmlDoc, [ ], None, range.Zero)

let createQualifiedModule (idens: seq<string>) declarations =
    let nameParts =
        idens
        |> Seq.collect (fun name ->
            if name.Contains "."
            then name.Split('.')
            else [| name |]
        )

    let xmlDoc = PreXmlDoc.Create [ ]
    SynModuleOrNamespace.SynModuleOrNamespace([ for ident in nameParts -> Ident.Create ident ], true, SynModuleOrNamespaceKind.NamedModule,declarations,  xmlDoc, [ SynAttributeList.Create [ SynAttribute.RequireQualifiedAccess()  ]  ], None, range.Zero)

let createFile modules =
    let qualfiedNameOfFile = QualifiedNameOfFile.QualifiedNameOfFile(Ident.Create "IrrelevantFileName")
    ParsedImplFileInput.ParsedImplFileInput("IrrelevantFileName", false, qualfiedNameOfFile, [], [], modules, (false, false))

let formatAstInternal ast =
    let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true } // do not format comments
    CodeFormatter.FormatASTAsync(ast, "temp.fsx", [], None, cfg)

let formatAst file =
    formatAstInternal (ParsedInput.ImplFile file)
    |> Async.RunSynchronously

let client = new HttpClient()

[<EntryPoint>]
let main argv =
    let response =
        client.GetStreamAsync(schema)
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let reader = new OpenApiStreamReader()
    let (openApiDocument, diagnostics) =  reader.Read(response)

    let createRecordFromSchema (recordName: string) (schema: OpenApiSchema) =
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

        let rec createFSharpType required (propertyName: string) (schema: OpenApiSchema) =
            match schema.Type with
            | "integer" when schema.Format = "int64" ->
                if required
                then SynType.Int64()
                else SynType.Option(SynType.Int64())
            | "integer" when schema.Format = "int32" ->
                if required
                then SynType.Int()
                else SynType.Option(SynType.Int())
            | "array" ->
                let arrayItemsType = createFSharpType required propertyName schema.Items
                if required
                then SynType.List(arrayItemsType)
                else SynType.Option(SynType.List(arrayItemsType))
            | _ ->
                SynType.String()

        let recordRepresentation =  SynTypeDefnSimpleReprRecordRcd.Create [
            for property in schema.Properties do
                // todo: infer the types correctly
                let propertyName = property.Key
                let propertyType = property.Value
                let required = schema.Required.Contains propertyName
                SynFieldRcd.Create(propertyName, createFSharpType required propertyName propertyType)
        ]

        let simpleType = SynTypeDefnSimpleReprRcd.Record recordRepresentation
        SynModuleDecl.CreateSimpleType(info, simpleType)

    let globalTypes = [
        for schema in openApiDocument.Components.Schemas do
            createRecordFromSchema schema.Key schema.Value
    ]


    let globalTypesModule = createNamespace [ project; "Types" ] globalTypes

    let code = formatAst (createFile [ globalTypesModule ])

    System.Console.WriteLine code
    0 // return an integer exit code