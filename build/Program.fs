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

type ApiGuruSchema = {
    schemaUrl: string
    title: string
}

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
    // TODO - generate project and build
    ()

let integration() = 
    let schemas = apiGuruList()

    schemas
    |> Seq.truncate 50
    |> Seq.iter (fun schema -> printfn $"{schema.title} - {schema.schemaUrl}")

[<EntryPoint>]
let main (args: string[]) =
    Console.InputEncoding <- Encoding.UTF8
    Console.OutputEncoding <- Encoding.UTF8
    Console.WriteLine(Swag.logo)
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
