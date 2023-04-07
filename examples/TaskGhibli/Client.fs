namespace rec TaskGhibli

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open TaskGhibli.Types
open TaskGhibli.Http
open FSharp.Control.Tasks

///# Studio Ghibli API
///  The Studio Ghibli API catalogs the people, places, and things found in the worlds of Ghibli. It was created to help users discover resources, consume them via HTTP requests, and interact with them in whatever way makes sense. Navigation can be found on the left sidebar, and the right sidebar shows examples of returned objects for successful calls.
///  Users can raise an issue, ask for help, or find a contribution guide at the main repo: [https://github.com/janaipakos/ghibliapi](https://github.com/janaipakos/ghibliapi)
///# Getting Started
///  Requests can be made with `curl` or other helper libraries by following regular REST calls. For example, here is how to GET the resource for the film *My Neighbor Tororo*:
///  `curl https://ghibliapi.herokuapp.com/films/58611129-2dbc-4a81-a72f-77ddfc1b1b49`
///  Calling this resource will respond with the following object:
///  ```json
///  {
///  "id": "58611129-2dbc-4a81-a72f-77ddfc1b1b49",
///  "title": "My Neighbor Totoro",
///  "original_title": "となりのトトロ",
///  "original_title_romanised": "Tonari no Totoro",
///  "description": "Two sisters move to the country with their father in order to be closer to their hospitalized mother, and discover the surrounding trees are inhabited by Totoros, magical spirits of the forest. When the youngest runs away from home, the older sister seeks help from the spirits to find her.",
///  "director": "Hayao Miyazaki",
///  "producer": "Hayao Miyazaki",
///  "release_date": "1988",
///  "running_time": "86",
///  "rt_score": "93",
///  ...
///  }
///  ```
///# Base URL
///  Users must prepend all resource calls with this base URL:
///  `https://ghibliapi.herokuapp.com`
///# Authentication
///   There is no authentication necessary for the Studio Ghibli API.
///# Workflow
///  Endpoints can be used by themselves, or combined with one another to retrieve more specific information. An example workflow is listed below:
///  ### Goal: Get a list of people with the species classification as "spirit."
///    - Call the species endpoint with `/species?name=spirit`
///    - Call the people listed under this endpoint with `/people/&amp;lt;uuid&amp;gt;`
///    - Combine these results
///# Use Case
///  There are numerous ways for users to interact with the platform. For example, with the "people" API, users can get customized information about people, such as eye and hair color. Another example is using the "species"" API to find the different films each creature appears in. A more concrete case study is listed below:
///  ### A use case for finding information on all the cats of Studio Ghibli.
///  Using the Aeson library in Haskell, the user can parse the `people` array to return all of the cats, listed under `/species/603428ba-8a86-4b0b-a9f1-65df6abef3d3`
///  ```haskell
///  import qualified Data.ByteString.Lazy as L
///  import GHC.Generics
///  import Data.Aeson
///  main = do
///      fileData &amp;lt;- L.readFile "cats.json"
///      let ghibliResponse = decode fileData :: Maybe GhibliCatResponse
///      let ghibliResults = people &amp;lt;$&amp;gt; ghibliResponse
///      findCat ghibliResults
///  findCat :: Maybe [GhibliCatResult] -&amp;gt; IO ()
///  findCat Nothing = print "data not found"
///  findCat (Just people) = do
///      print $ T.pack "Studio Ghibli Cats:"
///      forM_ people $ \person -&amp;gt; do
///          let dataName = name person
///          let dataGender = gender person
///          let dataAge = age person
///          let dataHairColor = hairColor person
///          let dataEyeColor = eyeColor person
///          let dataFilms = films person
///          print $ T.concat [T.pack 'name: ', dataName
///                           ,T.pack ', gender: ', dataGender
///                           ,T.pack ', age: ', dataAge
///                           ,T.pack ', hair color: ', dataHairColor
///                           ,T.pack ', eye color: '', dataEyeColor
///                           ]
///  ```
///  The above code will return an IO Action of the requested cats.
///  ```
///  "Studio Ghibli Cats:"
///  "name: Jiji, gender: Male, hair color: Black, eye color: Black"
///  "name: Catbus, gender: Male, hair color: Brown, eye color: Yellow"
///  "name: Niya, gender: Male, hair color: Beige, eye color: White"
///  "name: Renaldo Moon aka Moon aka Muta, gender: Male, hair color: Beige, eye color: White"
///  "name: Cat King, gender: Male, hair color: Grey, eye color: Emerald"
///  "name: Yuki, gender: Female, hair color: White, eye color: Blue"
///  "name: Haru, gender: Female, hair color: Brown, eye color: Brown"
///  "name: Baron Humbert von Gikkingen, gender: Male, hair color: Yellow, eye color: Green"
///  "name: Natori, gender: Male, hair color: Grey, eye color: Blue"
///  ```
///# Helper Libraries
///  ## Elixir
///  - [ghibli](https://github.com/sotojuan/ghibli) by [Juan Soto](https://github.com/sotojuan)
type TaskGhibliClient(httpClient: HttpClient) =
    ///<summary>
    ///The Films endpoint returns information about all of the Studio Ghibli films.
    ///</summary>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="limit">amount of results (default 50) (maximum 250)</param>
    ///<param name="cancellationToken"></param>
    member this.GetFilms(?fields: string, ?limit: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/films" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetFilms.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetFilms.BadRequest
            else
                return GetFilms.NotFound
        }

    ///<summary>
    ///Returns a film based on a single ID
    ///</summary>
    ///<param name="id">film `id`</param>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.GetFilmsById(id: string, ?fields: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/films/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetFilmsById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetFilmsById.BadRequest
            else
                return GetFilmsById.NotFound
        }

    ///<summary>
    ///The People endpoint returns information about all of the Studio Ghibli people. This broadly includes all Ghibli characters, human and non-.
    ///</summary>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="limit">amount of results (default 50) (maximum 250)</param>
    ///<param name="cancellationToken"></param>
    member this.GetPeople(?fields: string, ?limit: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/people" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetPeople.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetPeople.BadRequest
            else
                return GetPeople.NotFound
        }

    ///<summary>
    ///Returns a person based on a single ID
    ///</summary>
    ///<param name="id">person `id`</param>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.GetPeopleById(id: string, ?fields: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/people/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetPeopleById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetPeopleById.BadRequest
            else
                return GetPeopleById.NotFound
        }

    ///<summary>
    ///The Locations endpoint returns information about all of the Studio Ghibli locations. This broadly includes lands, countries, and places.
    ///</summary>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="limit">amount of results (default 50) (maximum 250)</param>
    ///<param name="cancellationToken"></param>
    member this.GetLocations(?fields: string, ?limit: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/locations" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetLocations.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetLocations.BadRequest
            else
                return GetLocations.NotFound
        }

    ///<summary>
    ///Returns an individual location.
    ///</summary>
    ///<param name="id">location `id`</param>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.GetLocationsById(id: string, ?fields: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/locations/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetLocationsById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetLocationsById.BadRequest
            else
                return GetLocationsById.NotFound
        }

    ///<summary>
    ///The Species endpoint returns information about all of the Studio Ghibli species. This includes humans, animals, and spirits et al.
    ///</summary>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="limit">amount of results (default 50) (maximum 250)</param>
    ///<param name="cancellationToken"></param>
    member this.GetSpecies(?fields: string, ?limit: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/species" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSpecies.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetSpecies.BadRequest
            else
                return GetSpecies.NotFound
        }

    ///<summary>
    ///Returns an individual species
    ///</summary>
    ///<param name="id">film `id`</param>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.GetSpeciesById(id: string, ?fields: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/species/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSpeciesById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetSpeciesById.BadRequest
            else
                return GetSpeciesById.NotFound
        }

    ///<summary>
    ///The Vehicles endpoint returns information about all of the Studio Ghibli vechiles. This includes cars, ships, and planes.
    ///</summary>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="limit">amount of results (default 50) (maximum 250)</param>
    ///<param name="cancellationToken"></param>
    member this.GetVehicles(?fields: string, ?limit: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/vehicles" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetVehicles.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetVehicles.BadRequest
            else
                return GetVehicles.NotFound
        }

    ///<summary>
    ///An individual vehicle
    ///</summary>
    ///<param name="id">film `id`</param>
    ///<param name="fields">comma-separated list of fields to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.GetVehiclesById(id: string, ?fields: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/vehicles/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetVehiclesById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetVehiclesById.BadRequest
            else
                return GetVehiclesById.NotFound
        }
