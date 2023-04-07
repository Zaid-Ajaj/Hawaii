namespace rec TaskTripPinService

open Browser.Types
open Fable.SimpleHttp
open TaskTripPinService.Types
open TaskTripPinService.Http

///This OData service is located at http://localhost
type TaskTripPinServiceClient(url: string, headers: list<Header>) =
    new(url: string) = TaskTripPinServiceClient(url, [])

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
    member this.AirlinesAirlineListAirline
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Airlines" headers requestParts

            if status = 200 then
                return AirlinesAirlineListAirline.OK(Serializer.deserialize content)
            else
                return AirlinesAirlineListAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Add new entity to Airlines
    ///</summary>
    member this.AirlinesAirlineCreateAirline(body: MicrosoftODataSampleServiceModelsTripPinAirline) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/Airlines" headers requestParts

            if status = 201 then
                return AirlinesAirlineCreateAirline.Created(Serializer.deserialize content)
            else
                return AirlinesAirlineCreateAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from Airlines by key
    ///</summary>
    ///<param name="airlineCode">key: AirlineCode of Airline</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.AirlinesAirlineGetAirline(airlineCode: string, ?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ RequestPart.path ("AirlineCode", airlineCode)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/Airlines({AirlineCode})" headers requestParts

            if status = 200 then
                return AirlinesAirlineGetAirline.OK(Serializer.deserialize content)
            else
                return AirlinesAirlineGetAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in Airlines
    ///</summary>
    ///<param name="airlineCode">key: AirlineCode of Airline</param>
    ///<param name="body"></param>
    member this.AirlinesAirlineUpdateAirline
        (
            airlineCode: string,
            body: MicrosoftODataSampleServiceModelsTripPinAirline
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("AirlineCode", airlineCode)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync url "/Airlines({AirlineCode})" headers requestParts

            if status = 204 then
                return AirlinesAirlineUpdateAirline.NoContent
            else
                return AirlinesAirlineUpdateAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete entity from Airlines
    ///</summary>
    ///<param name="airlineCode">key: AirlineCode of Airline</param>
    ///<param name="ifMatch">ETag</param>
    member this.AirlinesAirlineDeleteAirline(airlineCode: string, ?ifMatch: string) =
        async {
            let requestParts =
                [ RequestPart.path ("AirlineCode", airlineCode)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/Airlines({AirlineCode})" headers requestParts

            if status = 204 then
                return AirlinesAirlineDeleteAirline.NoContent
            else
                return AirlinesAirlineDeleteAirline.DefaultResponse(Serializer.deserialize content)
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
    member this.AirportsAirportListAirport
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Airports" headers requestParts

            if status = 200 then
                return AirportsAirportListAirport.OK(Serializer.deserialize content)
            else
                return AirportsAirportListAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from Airports by key
    ///</summary>
    ///<param name="icaoCode">key: IcaoCode of Airport</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.AirportsAirportGetAirport(icaoCode: string, ?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ RequestPart.path ("IcaoCode", icaoCode)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/Airports({IcaoCode})" headers requestParts

            if status = 200 then
                return AirportsAirportGetAirport.OK(Serializer.deserialize content)
            else
                return AirportsAirportGetAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in Airports
    ///</summary>
    ///<param name="icaoCode">key: IcaoCode of Airport</param>
    ///<param name="body"></param>
    member this.AirportsAirportUpdateAirport(icaoCode: string, body: MicrosoftODataSampleServiceModelsTripPinAirport) =
        async {
            let requestParts =
                [ RequestPart.path ("IcaoCode", icaoCode)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync url "/Airports({IcaoCode})" headers requestParts

            if status = 204 then
                return AirportsAirportUpdateAirport.NoContent
            else
                return AirportsAirportUpdateAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke functionImport GetNearestAirport
    ///</summary>
    member this.FunctionImportGetNearestAirport(lat: string, lon: string) =
        async {
            let requestParts =
                [ RequestPart.path ("lat", lat)
                  RequestPart.path ("lon", lon) ]

            let! (status, content) =
                OpenApiHttp.getAsync url "/GetNearestAirport(lat={lat},lon={lon})" headers requestParts

            if status = 200 then
                return FunctionImportGetNearestAirport.OK(Serializer.deserialize content)
            else
                return FunctionImportGetNearestAirport.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Me
    ///</summary>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.MePersonGetPerson(?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/Me" headers requestParts

            if status = 200 then
                return MePersonGetPerson.OK(Serializer.deserialize content)
            else
                return MePersonGetPerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update Me
    ///</summary>
    member this.MePersonUpdatePerson(body: MicrosoftODataSampleServiceModelsTripPinPerson) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.patchAsync url "/Me" headers requestParts

            if status = 204 then
                return MePersonUpdatePerson.NoContent
            else
                return MePersonUpdatePerson.DefaultResponse(Serializer.deserialize content)
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
    member this.MeListFriends
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Friends" headers requestParts

            if status = 200 then
                return MeListFriends.OK(Serializer.deserialize content)
            else
                return MeListFriends.DefaultResponse(Serializer.deserialize content)
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
    member this.MeListRefFriends
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Friends/$ref" headers requestParts

            if status = 200 then
                return MeListRefFriends.OK(Serializer.deserialize content)
            else
                return MeListRefFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property ref to Friends for Me
    ///</summary>
    member this.MeCreateRefFriends() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/Me/Friends/$ref" headers requestParts

            if status = 201 then
                return MeCreateRefFriends.Created(Serializer.deserialize content)
            else
                return MeCreateRefFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFavoriteAirline
    ///</summary>
    member this.MeGetFavoriteAirline() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync
                    url
                    "/Me/Microsoft.OData.SampleService.Models.TripPin.GetFavoriteAirline()"
                    headers
                    requestParts

            if status = 200 then
                return MeGetFavoriteAirline.OK(Serializer.deserialize content)
            else
                return MeGetFavoriteAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFriendsTrips
    ///</summary>
    ///<param name="userName">Usage: userName={userName}</param>
    member this.MeGetFriendsTrips(userName: string) =
        async {
            let requestParts =
                [ RequestPart.path ("userName", userName) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    url
                    "/Me/Microsoft.OData.SampleService.Models.TripPin.GetFriendsTrips(userName={userName})"
                    headers
                    requestParts

            if status = 200 then
                return MeGetFriendsTrips.OK(Serializer.deserialize content)
            else
                return MeGetFriendsTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke action ShareTrip
    ///</summary>
    member this.MeShareTrip(body: MeShareTripPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync
                    url
                    "/Me/Microsoft.OData.SampleService.Models.TripPin.ShareTrip"
                    headers
                    requestParts

            if status = 204 then
                return MeShareTrip.NoContent
            else
                return MeShareTrip.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Photo from Me
    ///</summary>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.MeGetPhoto(?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Photo" headers requestParts

            if status = 200 then
                return MeGetPhoto.OK(Serializer.deserialize content)
            else
                return MeGetPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get ref of Photo from Me
    ///</summary>
    member this.MeGetRefPhoto() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/Me/Photo/$ref" headers requestParts

            if status = 200 then
                return MeGetRefPhoto.OK(Serializer.deserialize content)
            else
                return MeGetRefPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update the ref of navigation property Photo in Me
    ///</summary>
    member this.MeUpdateRefPhoto() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync url "/Me/Photo/$ref" headers requestParts

            if status = 204 then
                return MeUpdateRefPhoto.NoContent
            else
                return MeUpdateRefPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete ref of navigation property Photo for Me
    ///</summary>
    ///<param name="ifMatch">ETag</param>
    member this.MeDeleteRefPhoto(?ifMatch: string) =
        async {
            let requestParts =
                [ if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/Me/Photo/$ref" headers requestParts

            if status = 204 then
                return MeDeleteRefPhoto.NoContent
            else
                return MeDeleteRefPhoto.DefaultResponse(Serializer.deserialize content)
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
    member this.MeListTrips
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Trips" headers requestParts

            if status = 200 then
                return MeListTrips.OK(Serializer.deserialize content)
            else
                return MeListTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property to Trips for Me
    ///</summary>
    member this.MeCreateTrips(body: MicrosoftODataSampleServiceModelsTripPinTrip) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/Me/Trips" headers requestParts

            if status = 201 then
                return MeCreateTrips.Created(Serializer.deserialize content)
            else
                return MeCreateTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Trips from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.MeGetTrips(tripId: int, ?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Trips({TripId})" headers requestParts

            if status = 200 then
                return MeGetTrips.OK(Serializer.deserialize content)
            else
                return MeGetTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update the navigation property Trips in Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="body"></param>
    member this.MeUpdateTrips(tripId: int, body: MicrosoftODataSampleServiceModelsTripPinTrip) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync url "/Me/Trips({TripId})" headers requestParts

            if status = 204 then
                return MeUpdateTrips.NoContent
            else
                return MeUpdateTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete navigation property Trips for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="ifMatch">ETag</param>
    member this.MeDeleteTrips(tripId: int, ?ifMatch: string) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/Me/Trips({TripId})" headers requestParts

            if status = 204 then
                return MeDeleteTrips.NoContent
            else
                return MeDeleteTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetInvolvedPeople
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    member this.MeTripsTripGetInvolvedPeople(tripId: int) =
        async {
            let requestParts = [ RequestPart.path ("TripId", tripId) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    url
                    "/Me/Trips({TripId})/Microsoft.OData.SampleService.Models.TripPin.GetInvolvedPeople()"
                    headers
                    requestParts

            if status = 200 then
                return MeTripsTripGetInvolvedPeople.OK(Serializer.deserialize content)
            else
                return MeTripsTripGetInvolvedPeople.DefaultResponse(Serializer.deserialize content)
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
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Trips({TripId})/Photos" headers requestParts

            if status = 200 then
                return MeTripsListPhotos.OK(Serializer.deserialize content)
            else
                return MeTripsListPhotos.DefaultResponse(Serializer.deserialize content)
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
    member this.MeTripsListRefPhotos
        (
            tripId: int,
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Trips({TripId})/Photos/$ref" headers requestParts

            if status = 200 then
                return MeTripsListRefPhotos.OK(Serializer.deserialize content)
            else
                return MeTripsListRefPhotos.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property ref to Photos for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    member this.MeTripsCreateRefPhotos(tripId: int) =
        async {
            let requestParts = [ RequestPart.path ("TripId", tripId) ]
            let! (status, content) = OpenApiHttp.postAsync url "/Me/Trips({TripId})/Photos/$ref" headers requestParts

            if status = 201 then
                return MeTripsCreateRefPhotos.Created(Serializer.deserialize content)
            else
                return MeTripsCreateRefPhotos.DefaultResponse(Serializer.deserialize content)
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
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Me/Trips({TripId})/PlanItems" headers requestParts

            if status = 200 then
                return MeTripsListPlanItems.OK(Serializer.deserialize content)
            else
                return MeTripsListPlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Create new navigation property to PlanItems for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="body"></param>
    member this.MeTripsCreatePlanItems(tripId: int, body: MicrosoftODataSampleServiceModelsTripPinPlanItem) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.postAsync url "/Me/Trips({TripId})/PlanItems" headers requestParts

            if status = 201 then
                return MeTripsCreatePlanItems.Created(Serializer.deserialize content)
            else
                return MeTripsCreatePlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get PlanItems from Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="planItemId">key: PlanItemId of PlanItem</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.MeTripsGetPlanItems(tripId: int, planItemId: int, ?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.path ("PlanItemId", planItemId)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync url "/Me/Trips({TripId})/PlanItems({PlanItemId})" headers requestParts

            if status = 200 then
                return MeTripsGetPlanItems.OK(Serializer.deserialize content)
            else
                return MeTripsGetPlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update the navigation property PlanItems in Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="planItemId">key: PlanItemId of PlanItem</param>
    ///<param name="body"></param>
    member this.MeTripsUpdatePlanItems
        (
            tripId: int,
            planItemId: int,
            body: MicrosoftODataSampleServiceModelsTripPinPlanItem
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.path ("PlanItemId", planItemId)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync url "/Me/Trips({TripId})/PlanItems({PlanItemId})" headers requestParts

            if status = 204 then
                return MeTripsUpdatePlanItems.NoContent
            else
                return MeTripsUpdatePlanItems.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete navigation property PlanItems for Me
    ///</summary>
    ///<param name="tripId">key: TripId of Trip</param>
    ///<param name="planItemId">key: PlanItemId of PlanItem</param>
    ///<param name="ifMatch">ETag</param>
    member this.MeTripsDeletePlanItems(tripId: int, planItemId: int, ?ifMatch: string) =
        async {
            let requestParts =
                [ RequestPart.path ("TripId", tripId)
                  RequestPart.path ("PlanItemId", planItemId)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/Me/Trips({TripId})/PlanItems({PlanItemId})" headers requestParts

            if status = 204 then
                return MeTripsDeletePlanItems.NoContent
            else
                return MeTripsDeletePlanItems.DefaultResponse(Serializer.deserialize content)
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
    member this.PeoplePersonListPerson
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/People" headers requestParts

            if status = 200 then
                return PeoplePersonListPerson.OK(Serializer.deserialize content)
            else
                return PeoplePersonListPerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Add new entity to People
    ///</summary>
    member this.PeoplePersonCreatePerson(body: MicrosoftODataSampleServiceModelsTripPinPerson) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/People" headers requestParts

            if status = 201 then
                return PeoplePersonCreatePerson.Created(Serializer.deserialize content)
            else
                return PeoplePersonCreatePerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from People by key
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="expand">Expand related entities</param>
    member this.PeoplePersonGetPerson(userName: string, ?expand: list<string>) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/People({UserName})" headers requestParts

            if status = 200 then
                return PeoplePersonGetPerson.OK(Serializer.deserialize content)
            else
                return PeoplePersonGetPerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in People
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="body"></param>
    member this.PeoplePersonUpdatePerson(userName: string, body: MicrosoftODataSampleServiceModelsTripPinPerson) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync url "/People({UserName})" headers requestParts

            if status = 204 then
                return PeoplePersonUpdatePerson.NoContent
            else
                return PeoplePersonUpdatePerson.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete entity from People
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="ifMatch">ETag</param>
    member this.PeoplePersonDeletePerson(userName: string, ?ifMatch: string) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/People({UserName})" headers requestParts

            if status = 204 then
                return PeoplePersonDeletePerson.NoContent
            else
                return PeoplePersonDeletePerson.DefaultResponse(Serializer.deserialize content)
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
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/People({UserName})/Friends" headers requestParts

            if status = 200 then
                return PeopleListFriends.OK(Serializer.deserialize content)
            else
                return PeopleListFriends.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFavoriteAirline
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    member this.PeoplePersonGetFavoriteAirline(userName: string) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    url
                    "/People({UserName})/Microsoft.OData.SampleService.Models.TripPin.GetFavoriteAirline()"
                    headers
                    requestParts

            if status = 200 then
                return PeoplePersonGetFavoriteAirline.OK(Serializer.deserialize content)
            else
                return PeoplePersonGetFavoriteAirline.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetFriendsTrips
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="userNameInPath">Usage: userName={userName}</param>
    member this.PeoplePersonGetFriendsTrips(userName: string, userNameInPath: string) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.path ("userName", userNameInPath) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    url
                    "/People({UserName})/Microsoft.OData.SampleService.Models.TripPin.GetFriendsTrips(userName={userName})"
                    headers
                    requestParts

            if status = 200 then
                return PeoplePersonGetFriendsTrips.OK(Serializer.deserialize content)
            else
                return PeoplePersonGetFriendsTrips.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke action ShareTrip
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="body"></param>
    member this.PeoplePersonShareTrip(userName: string, body: PeoplePersonShareTripPayload) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync
                    url
                    "/People({UserName})/Microsoft.OData.SampleService.Models.TripPin.ShareTrip"
                    headers
                    requestParts

            if status = 204 then
                return PeoplePersonShareTrip.NoContent
            else
                return PeoplePersonShareTrip.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke function GetInvolvedPeople
    ///</summary>
    ///<param name="userName">key: UserName of Person</param>
    ///<param name="tripId">key: TripId of Trip</param>
    member this.PeoplePersonTripsTripGetInvolvedPeople(userName: string, tripId: int) =
        async {
            let requestParts =
                [ RequestPart.path ("UserName", userName)
                  RequestPart.path ("TripId", tripId) ]

            let! (status, content) =
                OpenApiHttp.getAsync
                    url
                    "/People({UserName})/Trips({TripId})/Microsoft.OData.SampleService.Models.TripPin.GetInvolvedPeople()"
                    headers
                    requestParts

            if status = 200 then
                return PeoplePersonTripsTripGetInvolvedPeople.OK(Serializer.deserialize content)
            else
                return PeoplePersonTripsTripGetInvolvedPeople.DefaultResponse(Serializer.deserialize content)
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
    member this.PhotosPhotoListPhoto
        (
            ?top: int,
            ?skip: int,
            ?search: string,
            ?filter: string,
            ?count: bool,
            ?orderby: list<string>,
            ?select: list<string>,
            ?expand: list<string>
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

            let! (status, content) = OpenApiHttp.getAsync url "/Photos" headers requestParts

            if status = 200 then
                return PhotosPhotoListPhoto.OK(Serializer.deserialize content)
            else
                return PhotosPhotoListPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Add new entity to Photos
    ///</summary>
    member this.PhotosPhotoCreatePhoto(body: MicrosoftODataSampleServiceModelsTripPinPhoto) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/Photos" headers requestParts

            if status = 201 then
                return PhotosPhotoCreatePhoto.Created(Serializer.deserialize content)
            else
                return PhotosPhotoCreatePhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get entity from Photos by key
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="select">Select properties to be returned</param>
    ///<param name="expand">Expand related entities</param>
    member this.PhotosPhotoGetPhoto(id: int64, ?select: list<string>, ?expand: list<string>) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  if select.IsSome then
                      RequestPart.query ("$select", select.Value)
                  if expand.IsSome then
                      RequestPart.query ("$expand", expand.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/Photos({Id})" headers requestParts

            if status = 200 then
                return PhotosPhotoGetPhoto.OK(Serializer.deserialize content)
            else
                return PhotosPhotoGetPhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update entity in Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="body"></param>
    member this.PhotosPhotoUpdatePhoto(id: int64, body: MicrosoftODataSampleServiceModelsTripPinPhoto) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync url "/Photos({Id})" headers requestParts

            if status = 204 then
                return PhotosPhotoUpdatePhoto.NoContent
            else
                return PhotosPhotoUpdatePhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete entity from Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="ifMatch">ETag</param>
    member this.PhotosPhotoDeletePhoto(id: int64, ?ifMatch: string) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  if ifMatch.IsSome then
                      RequestPart.header ("If-Match", ifMatch.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/Photos({Id})" headers requestParts

            if status = 204 then
                return PhotosPhotoDeletePhoto.NoContent
            else
                return PhotosPhotoDeletePhoto.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Get media content for Photo from Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    member this.PhotosPhotoGetContent(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("Id", id) ]
            let! (status, contentBinary) = OpenApiHttp.getBinaryAsync url "/Photos({Id})/$value" headers requestParts

            if status = 200 then
                return PhotosPhotoGetContent.OK contentBinary
            else
                let! content = Utilities.readBytesAsText contentBinary
                return PhotosPhotoGetContent.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Update media content for Photo in Photos
    ///</summary>
    ///<param name="id">key: Id of Photo</param>
    ///<param name="requestBody"></param>
    member this.PhotosPhotoUpdateContent(id: int64, ?requestBody: byte []) =
        async {
            let requestParts =
                [ RequestPart.path ("Id", id)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) = OpenApiHttp.putAsync url "/Photos({Id})/$value" headers requestParts

            if status = 204 then
                return PhotosPhotoUpdateContent.NoContent
            else
                return PhotosPhotoUpdateContent.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Invoke actionImport ResetDataSource
    ///</summary>
    member this.ActionImportResetDataSource() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/ResetDataSource" headers requestParts

            if status = 204 then
                return ActionImportResetDataSource.NoContent
            else
                return ActionImportResetDataSource.DefaultResponse(Serializer.deserialize content)
        }
