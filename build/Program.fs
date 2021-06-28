module Program

open System
open System.IO
open System.Text
open System.Xml
open System.Xml.Linq
open System.Net
open System.Net.Http
open Fake.IO
open Fake.Core
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.Linq


let path xs = Path.Combine(Array.ofList xs)

let solutionRoot = Files.findParent __SOURCE_DIRECTORY__ "Hawaii.sln";

let src = path [ solutionRoot; "src" ]

let [<Literal>] TargetFramework = "net5.0"

let httpClient = new HttpClient()

type ApiGuruSchema = { schemaUrl: string; title: string }

let apiGuruList() = 
    let guruJson = 
        httpClient.GetStringAsync("https://api.apis.guru/v2/list.json")
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> JObject.Parse

    let schemas = ResizeArray<ApiGuruSchema>()

    for property in guruJson.Properties() do 
        let schemaJson = unbox<JObject> property.Value
        let versions = unbox<JObject> schemaJson.["versions"]
        let lastVersion = unbox<JObject> (versions.Properties().Last().Value)
        schemas.Add {
            schemaUrl = string lastVersion.["swaggerUrl"]
            title = string lastVersion.["info"].["title"]
        }

    schemas

let build() =
    if Shell.Exec(Tools.dotnet, "build --configuration Release", solutionRoot) <> 0
    then failwith "build failed"

let pack() =
    Shell.deleteDir (path [ "src"; "bin" ])
    Shell.deleteDir (path [ "src"; "obj" ])
    if Shell.Exec(Tools.dotnet, "pack --configuration Release", src) <> 0 then
        failwith "Pack failed"
    else
        let outputPath = path [ src; "bin"; "Release" ]
        if Shell.Exec(Tools.dotnet, sprintf "tool install -g hawaii --add-source %s" outputPath) <> 0
        then failwith "Local install failed"

let capitalize (input: string) =
    if String.IsNullOrWhiteSpace input
    then ""
    else input.First().ToString().ToUpper() + String.Join("", input.Skip(1))

let camelCase (input: string) =
    if String.IsNullOrWhiteSpace input
    then ""
    else input.First().ToString().ToLower() + String.Join("", input.Skip(1))

let publish() =
    Shell.deleteDir (path [ src; "bin" ])
    Shell.deleteDir (path [ src; "obj" ])

    if Shell.Exec(Tools.dotnet, "pack --configuration Release", src) <> 0 then
        failwith "Pack failed"
    else
        let nugetKey =
            match Environment.environVarOrNone "NUGET_KEY" with
            | Some nugetKey -> nugetKey
            | None -> failwith "The Nuget API key must be set in a NUGET_KEY environmental variable"

        let nugetPath =
            Directory.GetFiles(path [ src; "bin"; "Release" ])
            |> Seq.head
            |> Path.GetFullPath

        if Shell.Exec(Tools.dotnet, sprintf "nuget push %s -s nuget.org -k %s" nugetPath nugetKey, src) <> 0
        then failwith "Publish failed"

let generateAndBuild(schema: ApiGuruSchema) = 
    let integrationSchema = path [ src; "hawaii.json" ]
    let content = JObject()
    content.Add(JProperty("schema", schema.schemaUrl))
    content.Add(JProperty("project", schema.title))
    content.Add(JProperty("output", "./output"))
    File.WriteAllText(integrationSchema, content.ToString(Formatting.Indented))
    printfn $"Attempting to generate project {schema.title} from {schema.schemaUrl}"
    if Shell.Exec(Tools.dotnet, "run -- --no-logo", src) <> 0 then
        failwith $"Failed to generate project {schema.title}"
    else
        if Shell.Exec(Tools.dotnet, "build --configuration Release", path [ src; "output" ]) <> 0
        then failwith "build failed"

let integration() = 
    let schemas = apiGuruList()

    let normalize (name: string) = 
        name.Split ' '
        |> Array.map (fun part -> 
            let modified = capitalize (part.ToLower().Replace(":", "").Replace("-", ""))
            if modified.StartsWith "1" 
            then capitalize (String.Join("", modified.Skip(1)))
            else modified
        )
        |> String.concat ""

    printfn $"Found {schemas.Count} OpenAPI schemas from API Guru list"
    let n = 20; 
    printfn $"Generating and building the first {n} OpenAPI schemas from that list"

    schemas
    |> Seq.truncate n
    |> Seq.map (fun schema -> { schema with title = normalize schema.title })
    |> Seq.filter (fun schema -> schema.title <> "AdyenForPlatformsNotifications") // OpenApi 3.1 not supported
    |> Seq.iter generateAndBuild

[<EntryPoint>]
let main (args: string[]) =
    Console.InputEncoding <- Encoding.UTF8
    Console.OutputEncoding <- Encoding.UTF8
    try
        match args with
        | [| "build"   |] -> build()
        | [| "pack"    |] -> pack()
        | [| "publish" |] -> publish()
        | [| "integration" |] -> integration()

        | _ -> printfn "Unknown args %A" args
        0
    with ex ->
        printfn "%A" ex
        1
