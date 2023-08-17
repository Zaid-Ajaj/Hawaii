namespace rec TripPinService

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open TripPinService.Types
open TripPinService.Http

///This OData service is located at http://localhost
type TripPinServiceClient(httpClient: HttpClient) =
    ///<summary>
    ///Get entities from Airlines
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.AirlinesAirlineListAirline
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Airlines" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return AirlinesAirlineListAirline.OK(Serializer.deserialize content)
            | _ -> return AirlinesAirlineListAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Add new entity to Airlines
    ///</summary>
    member this.AirlinesAirlineCreateAirline
        (
            body: MicrosoftODataSampleServiceModelsTripPinAirline,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/Airlines" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return AirlinesAirlineCreateAirline.Created(Serializer.deserialize content)
            | _ -> return AirlinesAirlineCreateAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from Airlines by key
    ///</summary>
    ///<param name="airlineCode">key: AirlineCode of Airline</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.AirlinesAirlineGetAirline
        (
            airlineCode: string,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("AirlineCode", airlineCode)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/Airlines({AirlineCode})" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return AirlinesAirlineGetAirline.OK(Serializer.deserialize content)
            | _ -> return AirlinesAirlineGetAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in Airlines
    ///</summary>
    ///<param name="airlineCode">key: AirlineCode of Airline</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.AirlinesAirlineUpdateAirline
        (
            airlineCode: string,
            body: MicrosoftODataSampleServiceModelsTripPinAirline,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("AirlineCode", airlineCode)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync httpClient "/Airlines({AirlineCode})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return AirlinesAirlineUpdateAirline.NoContent
            | _ -> return AirlinesAirlineUpdateAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete entity from Airlines
    ///</summary>
    ///<param name="airlineCode">key: AirlineCode of Airline</param>
    ///<param name="ifMatch">ETag</param>
    ///<param name="cancellationToken"></param>
    member this.AirlinesAirlineDeleteAirline
        (
            airlineCode: string,
            ?ifMatch: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("AirlineCode", airlineCode)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/Airlines({AirlineCode})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return AirlinesAirlineDeleteAirline.NoContent
            | _ -> return AirlinesAirlineDeleteAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entities from Airports
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.AirportsAirportListAirport
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Airports" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return AirportsAirportListAirport.OK(Serializer.deserialize content)
            | _ -> return AirportsAirportListAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from Airports by key
    ///</summary>
    ///<param name="icaoCode">key: IcaoCode of Airport</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.AirportsAirportGetAirport
        (
            icaoCode: string,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("IcaoCode", icaoCode)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/Airports({IcaoCode})" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return AirportsAirportGetAirport.OK(Serializer.deserialize content)
            | _ -> return AirportsAirportGetAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in Airports
    ///</summary>
    ///<param name="icaoCode">key: IcaoCode of Airport</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.AirportsAirportUpdateAirport
        (
            icaoCode: string,
            body: MicrosoftODataSampleServiceModelsTripPinAirport,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("IcaoCode", icaoCode)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync httpClient "/Airports({IcaoCode})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return AirportsAirportUpdateAirport.NoContent
            | _ -> return AirportsAirportUpdateAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke functionImport GetNearestAirport
    ///</summary>
    member this.FunctionImportGetNearestAirport(lat: string, lon: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("lat", lat)
                  RequestPart.path ("lon", lon) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/GetNearestAirport(lat={lat},lon={lon})" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FunctionImportGetNearestAirport.OK(Serializer.deserialize content)
            | _ -> return FunctionImportGetNearestAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Me
    ///</summary>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MePersonGetPerson(?select: list<string>, ?expand: list<string>, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Me" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MePersonGetPerson.OK(Serializer.deserialize content)
            | _ -> return MePersonGetPerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update Me
    ///</summary>
    member this.MePersonUpdatePerson
        (
            body: MicrosoftODataSampleServiceModelsTripPinPerson,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.patchAsync httpClient "/Me" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MePersonUpdatePerson.NoContent
            | _ -> return MePersonUpdatePerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Friends from Me
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeListFriends
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Me/Friends" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeListFriends.OK(Serializer.deserialize content)
            | _ -> return MeListFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get ref of Friends from Me
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="cancellationToken"></param>
    member this.MeListRefFriends
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Me/Friends/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeListRefFriends.OK(Serializer.deserialize content)
            | _ -> return MeListRefFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property ref to Friends for Me
    ///</summary>
    member this.MeCreateRefFriends(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync httpClient "/Me/Friends/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return MeCreateRefFriends.Created(Serializer.deserialize content)
            | _ -> return MeCreateRefFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFavoriteAirline
    ///</summary>
    member this.MeGetFavoriteAirline(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/Me/Microsoft.OData.SampleService.Models.TripPin.GetFavoriteAirline()"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeGetFavoriteAirline.OK(Serializer.deserialize content)
            | _ -> return MeGetFavoriteAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFriendsTrips
    ///</summary>
    ///<param name="userName">Usage: userName={userName}</param>
    ///<param name="cancellationToken"></param>
    member this.MeGetFriendsTrips(userName: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("userName", userName) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/Me/Microsoft.OData.SampleService.Models.TripPin.GetFriendsTrips(userName={userName})"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeGetFriendsTrips.OK(Serializer.deserialize content)
            | _ -> return MeGetFriendsTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke action ShareTrip
    ///</summary>
    member this.MeShareTrip(body: MeShareTripPayload, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync
                    httpClient
                    "/Me/Microsoft.OData.SampleService.Models.TripPin.ShareTrip"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeShareTrip.NoContent
            | _ -> return MeShareTrip.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Photo from Me
    ///</summary>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeGetPhoto(?select: list<string>, ?expand: list<string>, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Me/Photo" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeGetPhoto.OK(Serializer.deserialize content)
            | _ -> return MeGetPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get ref of Photo from Me
    ///</summary>
    member this.MeGetRefPhoto(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/Me/Photo/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeGetRefPhoto.OK(Serializer.deserialize content)
            | _ -> return MeGetRefPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update the ref of navigation property Photo in Me
    ///</summary>
    member this.MeUpdateRefPhoto(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync httpClient "/Me/Photo/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeUpdateRefPhoto.NoContent
            | _ -> return MeUpdateRefPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete ref of navigation property Photo for Me
    ///</summary>
    ///<param name="ifMatch">ETag</param>
    ///<param name="cancellationToken"></param>
    member this.MeDeleteRefPhoto(?ifMatch: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/Me/Photo/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeDeleteRefPhoto.NoContent
            | _ -> return MeDeleteRefPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Trips from Me
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeListTrips
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Me/Trips" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeListTrips.OK(Serializer.deserialize content)
            | _ -> return MeListTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property to Trips for Me
    ///</summary>
    member this.MeCreateTrips
        (
            body: MicrosoftODataSampleServiceModelsTripPinTrip,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/Me/Trips" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return MeCreateTrips.Created(Serializer.deserialize content)
            | _ -> return MeCreateTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Trips from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeGetTrips
        (
            tripId: int,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/Me/Trips({TripId})" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeGetTrips.OK(Serializer.deserialize content)
            | _ -> return MeGetTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update the navigation property Trips in Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.MeUpdateTrips
        (
            tripId: int,
            body: MicrosoftODataSampleServiceModelsTripPinTrip,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync httpClient "/Me/Trips({TripId})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeUpdateTrips.NoContent
            | _ -> return MeUpdateTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete navigation property Trips for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="ifMatch">ETag</param>
    ///<param name="cancellationToken"></param>
    member this.MeDeleteTrips(tripId: int, ?ifMatch: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/Me/Trips({TripId})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeDeleteTrips.NoContent
            | _ -> return MeDeleteTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetInvolvedPeople
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsTripGetInvolvedPeople(tripId: int, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("TripId", tripId) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/Me/Trips({TripId})/Microsoft.OData.SampleService.Models.TripPin.GetInvolvedPeople()"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeTripsTripGetInvolvedPeople.OK(Serializer.deserialize content)
            | _ -> return MeTripsTripGetInvolvedPeople.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Photos from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsListPhotos
        (
            tripId: int,
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/Me/Trips({TripId})/Photos" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeTripsListPhotos.OK(Serializer.deserialize content)
            | _ -> return MeTripsListPhotos.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get ref of Photos from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsListRefPhotos
        (
            tripId: int,
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/Me/Trips({TripId})/Photos/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeTripsListRefPhotos.OK(Serializer.deserialize content)
            | _ -> return MeTripsListRefPhotos.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property ref to Photos for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsCreateRefPhotos(tripId: int, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("TripId", tripId) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/Me/Trips({TripId})/Photos/$ref" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return MeTripsCreateRefPhotos.Created(Serializer.deserialize content)
            | _ -> return MeTripsCreateRefPhotos.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get PlanItems from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsListPlanItems
        (
            tripId: int,
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/Me/Trips({TripId})/PlanItems" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeTripsListPlanItems.OK(Serializer.deserialize content)
            | _ -> return MeTripsListPlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property to PlanItems for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsCreatePlanItems
        (
            tripId: int,
            body: MicrosoftODataSampleServiceModelsTripPinPlanItem,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/Me/Trips({TripId})/PlanItems" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return MeTripsCreatePlanItems.Created(Serializer.deserialize content)
            | _ -> return MeTripsCreatePlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get PlanItems from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="planItemId">key: PlanItemId of PlanItem</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsGetPlanItems
        (
            tripId: int,
            planItemId: int,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.path ("PlanItemId", planItemId)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/Me/Trips({TripId})/PlanItems({PlanItemId})"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return MeTripsGetPlanItems.OK(Serializer.deserialize content)
            | _ -> return MeTripsGetPlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update the navigation property PlanItems in Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="planItemId">key: PlanItemId of PlanItem</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsUpdatePlanItems
        (
            tripId: int,
            planItemId: int,
            body: MicrosoftODataSampleServiceModelsTripPinPlanItem,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.path ("PlanItemId", planItemId)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync
                    httpClient
                    "/Me/Trips({TripId})/PlanItems({PlanItemId})"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeTripsUpdatePlanItems.NoContent
            | _ -> return MeTripsUpdatePlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete navigation property PlanItems for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="planItemId">key: PlanItemId of PlanItem</param>
    ///<param name="ifMatch">ETag</param>
    ///<param name="cancellationToken"></param>
    member this.MeTripsDeletePlanItems
        (
            tripId: int,
            planItemId: int,
            ?ifMatch: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.path ("PlanItemId", planItemId)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    httpClient
                    "/Me/Trips({TripId})/PlanItems({PlanItemId})"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return MeTripsDeletePlanItems.NoContent
            | _ -> return MeTripsDeletePlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entities from People
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonListPerson
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/People" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return PeoplePersonListPerson.OK(Serializer.deserialize content)
            | _ -> return PeoplePersonListPerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Add new entity to People
    ///</summary>
    member this.PeoplePersonCreatePerson
        (
            body: MicrosoftODataSampleServiceModelsTripPinPerson,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/People" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return PeoplePersonCreatePerson.Created(Serializer.deserialize content)
            | _ -> return PeoplePersonCreatePerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from People by key
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonGetPerson(userName: string, ?expand: list<string>, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/People({UserName})" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return PeoplePersonGetPerson.OK(Serializer.deserialize content)
            | _ -> return PeoplePersonGetPerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in People
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonUpdatePerson
        (
            userName: string,
            body: MicrosoftODataSampleServiceModelsTripPinPerson,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync httpClient "/People({UserName})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return PeoplePersonUpdatePerson.NoContent
            | _ -> return PeoplePersonUpdatePerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete entity from People
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="ifMatch">ETag</param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonDeletePerson(userName: string, ?ifMatch: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/People({UserName})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return PeoplePersonDeletePerson.NoContent
            | _ -> return PeoplePersonDeletePerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Friends from People
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.PeopleListFriends
        (
            userName: string,
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/People({UserName})/Friends" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return PeopleListFriends.OK(Serializer.deserialize content)
            | _ -> return PeopleListFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFavoriteAirline
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonGetFavoriteAirline(userName: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/People({UserName})/Microsoft.OData.SampleService.Models.TripPin.GetFavoriteAirline()"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return PeoplePersonGetFavoriteAirline.OK(Serializer.deserialize content)
            | _ -> return PeoplePersonGetFavoriteAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFriendsTrips
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="userNameInPath">Usage: userName={userName}</param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonGetFriendsTrips
        (
            userName: string,
            userNameInPath: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.path ("userName", userNameInPath) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/People({UserName})/Microsoft.OData.SampleService.Models.TripPin.GetFriendsTrips(userName={userName})"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return PeoplePersonGetFriendsTrips.OK(Serializer.deserialize content)
            | _ -> return PeoplePersonGetFriendsTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke action ShareTrip
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonShareTrip
        (
            userName: string,
            body: PeoplePersonShareTripPayload,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync
                    httpClient
                    "/People({UserName})/Microsoft.OData.SampleService.Models.TripPin.ShareTrip"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return PeoplePersonShareTrip.NoContent
            | _ -> return PeoplePersonShareTrip.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetInvolvedPeople
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="cancellationToken"></param>
    member this.PeoplePersonTripsTripGetInvolvedPeople
        (
            userName: string,
            tripId: int,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.path ("TripId", tripId) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    httpClient
                    "/People({UserName})/Trips({TripId})/Microsoft.OData.SampleService.Models.TripPin.GetInvolvedPeople()"
                    requestParts
                    cancellationToken

            match status with
            | HttpStatusCode.OK -> return PeoplePersonTripsTripGetInvolvedPeople.OK(Serializer.deserialize content)
            | _ -> return PeoplePersonTripsTripGetInvolvedPeople.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entities from Photos
    ///</summary>
    ///<param name="top">Show only the first n items</param>
    ///<param name="skip">Skip the first n items</param>
    ///<param name="search">Search items by search phrases</param>
    ///<param name="filter">Filter items by property values</param>
    ///<param name="count">Include count of items</param>
    ///<param name="orderby">Order items by property values</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.PhotosPhotoListPhoto
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if top.IsSome then
                      RequestPart.query ("$top", top.Value)
                  if skip.IsSome then
                      RequestPart.query ("$skip", skip.Value)
                  if search.IsSome then
                      RequestPart.query ("$search", search.Value)
                  if filter.IsSome then
                      RequestPart.query ("$filter", filter.Value)
                  if count.IsSome then
                      RequestPart.query ("$count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("$orderby", orderby.Value)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Photos" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return PhotosPhotoListPhoto.OK(Serializer.deserialize content)
            | _ -> return PhotosPhotoListPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Add new entity to Photos
    ///</summary>
    member this.PhotosPhotoCreatePhoto
        (
            body: MicrosoftODataSampleServiceModelsTripPinPhoto,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/Photos" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return PhotosPhotoCreatePhoto.Created(Serializer.deserialize content)
            | _ -> return PhotosPhotoCreatePhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from Photos by key
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    ///<param name="cancellationToken"></param>
    member this.PhotosPhotoGetPhoto
        (
            id: int64,
            ?select: list<string>,
            ?expand: list<string>,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/Photos({Id})" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return PhotosPhotoGetPhoto.OK(Serializer.deserialize content)
            | _ -> return PhotosPhotoGetPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PhotosPhotoUpdatePhoto
        (
            id: int64,
            body: MicrosoftODataSampleServiceModelsTripPinPhoto,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync httpClient "/Photos({Id})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return PhotosPhotoUpdatePhoto.NoContent
            | _ -> return PhotosPhotoUpdatePhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete entity from Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="ifMatch">ETag</param>
    ///<param name="cancellationToken"></param>
    member this.PhotosPhotoDeletePhoto(id: int64, ?ifMatch: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/Photos({Id})" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return PhotosPhotoDeletePhoto.NoContent
            | _ -> return PhotosPhotoDeletePhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get media content for Photo from Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="cancellationToken"></param>
    member this.PhotosPhotoGetContent(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("Id", id) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/Photos({Id})/$value" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return PhotosPhotoGetContent.OK contentBinary
            | _ ->
                let content = Encoding.UTF8.GetString contentBinary
                return PhotosPhotoGetContent.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update media content for Photo in Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="cancellationToken"></param>
    ///<param name="requestBody"></param>
    member this.PhotosPhotoUpdateContent(id: int64, ?cancellationToken: CancellationToken, ?requestBody: byte []) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) =
                OpenApiHttp.putAsync httpClient "/Photos({Id})/$value" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return PhotosPhotoUpdateContent.NoContent
            | _ -> return PhotosPhotoUpdateContent.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke actionImport ResetDataSource
    ///</summary>
    member this.ActionImportResetDataSource(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync httpClient "/ResetDataSource" requestParts cancellationToken

            match status with
            | HttpStatusCode.NoContent -> return ActionImportResetDataSource.NoContent
            | _ -> return ActionImportResetDataSource.DefaultResponse(Serializer.deserialize content)
        }
