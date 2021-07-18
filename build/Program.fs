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
    schemaUrl: string; 
    title: string;
    synchronous: bool; 
    asyncReturnType: string
    target:string
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
        let compatibleVersion = 
            versions.Properties()
            |> Seq.filter (fun versionInfo ->
                let openApiVer = string versionInfo.Value.["openapiVer"]
                not (openApiVer.StartsWith "3.1"))
            |> Seq.tryLast
            |> Option.map (fun property -> unbox<JObject> property.Value)

        match compatibleVersion with 
        | Some lastVersion -> 
            schemas.Add {
                schemaUrl = string lastVersion.["swaggerUrl"]
                title = string lastVersion.["info"].["title"]
                synchronous = false
                asyncReturnType = "async"
                target = "fsharp"
            }
        | None -> ()

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
    content.Add(JProperty("asyncReturnType", schema.asyncReturnType))
    content.Add(JProperty("target", schema.target))
    content.Add(JProperty("synchronous", schema.synchronous))

    File.WriteAllText(integrationSchema, content.ToString(Formatting.Indented))
    printfn $"Attempting to generate project {schema.title} from {schema.schemaUrl}"
    if Shell.Exec(Tools.dotnet, "run -- --no-logo", src) <> 0 then
        failwith $"Failed to generate project {schema.title}"
    else
        if Shell.Exec(Tools.dotnet, "build --configuration Release", path [ src; "output" ]) <> 0
        then failwith "build failed"

let normalize (name: string) = 
    let modifiedName = 
        name.Split ' '
        |> Array.map (fun part -> 
            let modified = capitalize (part.ToLower().Replace(":", "").Replace("-", "").Replace(",", "").Replace("&","").Replace("(", "").Replace(")", "").Replace(".", "").Replace("[", "").Replace("]", "").Replace("|", "").Replace("/", "").Replace("\\", ""))
            if modified.StartsWith "1" 
            then capitalize (String.Join("", modified.Skip(1)))
            else modified
        )
        |> String.concat ""

    if String.IsNullOrWhiteSpace modifiedName then
        "EmptyProject"
    else 
        modifiedName

let integration() = 
    let schemas = apiGuruList()
    printfn $"Found {schemas.Count} OpenAPI schemas from API Guru list"
    let n = 10; 
    printfn $"Generating and building the first {n} OpenAPI schemas from that list"

    schemas
    |> List.ofSeq
    |> List.truncate n
    |> List.map (fun schema -> { schema with title = normalize schema.title })
    |> List.iter generateAndBuild

let integrationKnownSchemas() = 
    let defaultPetStore = {
        schemaUrl = "https://petstore3.swagger.io/api/v3/openapi.json"
        title = "DefaultPetStore"
        synchronous = false
        asyncReturnType = "async"
        target = "fsharp"
    }

    let synchronousPetStore = {
        schemaUrl = "https://petstore3.swagger.io/api/v3/openapi.json"
        title = "SyncPetStore"
        synchronous = true
        asyncReturnType = "async"
        target = "fsharp"
    }

    let taskPetStore = {
        schemaUrl = "https://petstore3.swagger.io/api/v3/openapi.json"
        title = "TaskPetStore"
        synchronous = false
        asyncReturnType = "task"
        target = "fsharp"
    }

    let fablePetStore = {
        schemaUrl = "https://petstore3.swagger.io/api/v3/openapi.json"
        title = "FablePetStore"
        synchronous = false
        asyncReturnType = "async"
        target = "fable"
    }

    let yamlPetStore = {
        schemaUrl = "https://petstore3.swagger.io/api/v3/openapi.yaml"
        title = "YamlPetStore"
        synchronous = false
        asyncReturnType = "async"
        target = "fsharp"
    }

    let defaultTripPin = {
        schemaUrl = "https://services.odata.org/V4/(S(s3lb035ptje4a1j0bvkmqqa0))/TripPinServiceRW/$metadata"
        title = "TripPinService"
        synchronous = false
        asyncReturnType = "async"
        target = "fsharp"
    }

    let taskTripPin = {
        schemaUrl = "https://services.odata.org/V4/(S(s3lb035ptje4a1j0bvkmqqa0))/TripPinServiceRW/$metadata"
        title = "TaskTripPinService"
        synchronous = false
        asyncReturnType = "task"
        target = "fsharp"
    }

    let syncTripPin = {
        schemaUrl = "https://services.odata.org/V4/(S(s3lb035ptje4a1j0bvkmqqa0))/TripPinServiceRW/$metadata"
        title = "TaskTripPinService"
        synchronous = true
        asyncReturnType = "async"
        target = "fsharp"
    }

    let fableTripPin = {
        schemaUrl = "https://services.odata.org/V4/(S(s3lb035ptje4a1j0bvkmqqa0))/TripPinServiceRW/$metadata"
        title = "TaskTripPinService"
        synchronous = false
        asyncReturnType = "async"
        target = "fable"
    }

    let defaultGhibli = {
        schemaUrl = "./schemas/ghibli.json"
        title = "Ghibli"
        synchronous = false
        asyncReturnType = "async"
        target = "fsharp"
    }

    let syncGhibli = {
        schemaUrl = "./schemas/ghibli.json"
        title = "SyncGhibli"
        synchronous = true
        asyncReturnType = "async"
        target = "fsharp"
    }

    let taskGhibli = {
        schemaUrl = "./schemas/ghibli.json"
        title = "TaskGhibli"
        synchronous = false
        asyncReturnType = "task"
        target = "fsharp"
    }

    let fableGhibli = {
        schemaUrl = "./schemas/ghibli.json"
        title = "FableGhibli"
        synchronous = false
        asyncReturnType = "async"
        target = "fable"
    }

    let defaultNSwag = {
        schemaUrl = "./schemas/nswag-with-files.json"
        title = "NSwagWithFiles"
        synchronous = false
        asyncReturnType = "async"
        target = "fsharp"
    }

    let syncNSwag = {
        schemaUrl = "./schemas/nswag-with-files.json"
        title = "SyncNSwag"
        synchronous = true
        asyncReturnType = "async"
        target = "fsharp"
    }

    let taskNSwag = {
        schemaUrl = "./schemas/nswag-with-files.json"
        title = "TaskNSwag"
        synchronous = true
        asyncReturnType = "task"
        target = "fsharp"
    }

    let fableNSwag = {
        schemaUrl = "./schemas/nswag-with-files.json"
        title = "FableNSwag"
        synchronous = false
        asyncReturnType = "async"
        target = "fable"
    }

    let schemas = [
        defaultPetStore
        synchronousPetStore
        taskPetStore
        fablePetStore
        yamlPetStore
        defaultTripPin
        taskTripPin
        syncTripPin
        fableTripPin
        defaultGhibli
        syncGhibli
        taskGhibli
        fableGhibli
        defaultNSwag
        syncNSwag
        taskNSwag
        fableNSwag
    ]

    for schema in schemas do generateAndBuild(schema)

let successRate(n: int) = 
    let schemas = apiGuruList()
    printfn $"Found {schemas.Count} OpenAPI schemas from API Guru list"
    printfn $"Generating and building the first {n} OpenAPI schemas from that list"

    let mutable totalSuccess = 0
    let mutable totalFailed = 0;

    let results = 
        schemas
        |> List.ofSeq
        |> List.rev
        |> List.truncate n
        |> List.map (fun schema -> { schema with title = normalize schema.title })
        |> List.mapi (fun index schema -> 
            let progress = Math.Round((float (index + 1) / float n) * 100.0)
            try 
                generateAndBuild schema
                totalSuccess <- totalSuccess + 1
                printfn $"Progress {int progress}%% -> success({totalSuccess} / {n}) failed({totalFailed} / {n})"
                Ok schema
            with error -> 
                totalFailed <- totalFailed + 1
                printfn $"Progress {int progress} -> success({totalSuccess} / {n}) failed({totalFailed} / {n})"
                Error $"Failed to generate or build schema {schema.title} from {schema.schemaUrl}"
        )

    let success = 
        results 
        |> Seq.choose (function | Ok schema -> Some schema | _ -> None)
        |> Seq.toList

    let failed = 
        results 
        |> Seq.choose (function | Error message -> Some message | _ -> None)
        |> Seq.toList

    printfn $"Hawaii was able to generate and build {success.Length} / {n} APIs"
    if failed.Length > 0 then
        printfn "Error while generating the following schemas:"
        for errorMessage in failed do printfn "%s" errorMessage

let (|Int|_|) (input: string) = 
    try (Some (int input))
    with error -> None

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
        | [| "generate-and-build" |] -> integrationKnownSchemas()
        | [| "rate"; Int n |] -> successRate n
        | [| "rate" |] -> successRate 100
        | _ -> printfn "Unknown args %A" args
        0
    with ex ->
        printfn "%A" ex
        1
