[<AutoOpen>]
module Domain

open FSharp.Compiler.SyntaxTree
open Newtonsoft.Json.Linq

[<RequireQualifiedAccess>]
type EmptyDefinitionResolution =
    | Ignore
    | GenerateFreeForm

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

[<RequireQualifiedAccess>]
type FactoryFunction =
    | Create
    | None

type CodegenConfig = {
    schema: string
    output: string
    target: Target
    project : string
    asyncReturnType: AsyncReturnType
    synchronous: bool
    resolveReferences: bool
    emptyDefinitions: EmptyDefinitionResolution
    overrideSchema: JToken option
    filterTags: string list
    odataSchema: bool
}

type OperationParameter = {
    parameterName: string
    parameterIdent: string
    required: bool
    parameterType: SynType
    docs : string
    location: string
    style: string
    properties: string list
}