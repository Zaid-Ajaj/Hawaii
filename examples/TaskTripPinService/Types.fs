namespace rec TaskTripPinService.Types

#nowarn "1104"
type ODataResponse<'TValue> = { value: 'TValue }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type MicrosoftODataSampleServiceModelsTripPinPersonGender =
    | [<CompiledName "Male">] Male
    | [<CompiledName "Female">] Female
    | [<CompiledName "Unknown">] Unknown
    member this.Format() =
        match this with
        | Male -> "Male"
        | Female -> "Female"
        | Unknown -> "Unknown"

type GeoJSONposition = list<float>

type MicrosoftODataSampleServiceModelsTripPinCity =
    { ``@odata.type``: Option<string>
      CountryRegion: Option<string>
      Name: Option<string>
      Region: Option<string> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinCity with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinCity =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.City"
          CountryRegion = None
          Name = None
          Region = None }

type MicrosoftODataSampleServiceModelsTripPinLocation =
    { ``@odata.type``: Option<string>
      Address: Option<string>
      City: Option<MicrosoftODataSampleServiceModelsTripPinCity> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinLocation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinLocation =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Location"
          Address = None
          City = None }

type MicrosoftODataSampleServiceModelsTripPinEventLocation =
    { ``@odata.type``: Option<string>
      Address: Option<string>
      City: Option<MicrosoftODataSampleServiceModelsTripPinCity>
      BuildingInfo: Option<string> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinEventLocation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinEventLocation =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.EventLocation"
          Address = None
          City = None
          BuildingInfo = None }

type MicrosoftODataSampleServiceModelsTripPinAirportLocation =
    { ``@odata.type``: Option<string>
      Address: Option<string>
      City: Option<MicrosoftODataSampleServiceModelsTripPinCity>
      Loc: Option<EdmGeometryPoint> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinAirportLocation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinAirportLocation =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.AirportLocation"
          Address = None
          City = None
          Loc = None }

type MicrosoftODataSampleServiceModelsTripPinPhoto =
    { ``@odata.type``: Option<string>
      Id: Option<int64>
      Name: Option<string> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinPhoto with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinPhoto =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Photo"
          Id = None
          Name = None }

type MicrosoftODataSampleServiceModelsTripPinPerson =
    { ``@odata.type``: Option<string>
      UserName: Option<string>
      FirstName: Option<string>
      LastName: Option<string>
      Emails: Option<list<string>>
      AddressInfo: Option<list<MicrosoftODataSampleServiceModelsTripPinLocation>>
      Gender: Option<MicrosoftODataSampleServiceModelsTripPinPersonGender>
      Concurrency: Option<int64>
      Friends: Option<list<MicrosoftODataSampleServiceModelsTripPinPerson>>
      Trips: Option<list<MicrosoftODataSampleServiceModelsTripPinTrip>>
      Photo: Option<MicrosoftODataSampleServiceModelsTripPinPhoto> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinPerson with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinPerson =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Person"
          UserName = None
          FirstName = None
          LastName = None
          Emails = None
          AddressInfo = None
          Gender = None
          Concurrency = None
          Friends = None
          Trips = None
          Photo = None }

type MicrosoftODataSampleServiceModelsTripPinAirline =
    { ``@odata.type``: Option<string>
      AirlineCode: Option<string>
      Name: Option<string> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinAirline with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinAirline =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Airline"
          AirlineCode = None
          Name = None }

type MicrosoftODataSampleServiceModelsTripPinAirport =
    { ``@odata.type``: Option<string>
      IcaoCode: Option<string>
      Name: Option<string>
      IataCode: Option<string>
      Location: Option<obj> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinAirport with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinAirport =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Airport"
          IcaoCode = None
          Name = None
          IataCode = None
          Location = None }

type MicrosoftODataSampleServiceModelsTripPinPlanItem =
    { ``@odata.type``: Option<string>
      PlanItemId: Option<int>
      ConfirmationCode: Option<string>
      StartsAt: Option<System.DateTimeOffset>
      EndsAt: Option<System.DateTimeOffset>
      Duration: Option<string> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinPlanItem with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinPlanItem =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.PlanItem"
          PlanItemId = None
          ConfirmationCode = None
          StartsAt = None
          EndsAt = None
          Duration = None }

type MicrosoftODataSampleServiceModelsTripPinPublicTransportation =
    { ``@odata.type``: Option<string>
      PlanItemId: Option<int>
      ConfirmationCode: Option<string>
      StartsAt: Option<System.DateTimeOffset>
      EndsAt: Option<System.DateTimeOffset>
      Duration: Option<string>
      SeatNumber: Option<string> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinPublicTransportation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinPublicTransportation =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.PublicTransportation"
          PlanItemId = None
          ConfirmationCode = None
          StartsAt = None
          EndsAt = None
          Duration = None
          SeatNumber = None }

type MicrosoftODataSampleServiceModelsTripPinFlight =
    { ``@odata.type``: Option<string>
      PlanItemId: Option<int>
      ConfirmationCode: Option<string>
      StartsAt: Option<System.DateTimeOffset>
      EndsAt: Option<System.DateTimeOffset>
      Duration: Option<string>
      SeatNumber: Option<string>
      FlightNumber: Option<string>
      From: Option<MicrosoftODataSampleServiceModelsTripPinAirport>
      To: Option<MicrosoftODataSampleServiceModelsTripPinAirport>
      Airline: Option<MicrosoftODataSampleServiceModelsTripPinAirline> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinFlight with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinFlight =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Flight"
          PlanItemId = None
          ConfirmationCode = None
          StartsAt = None
          EndsAt = None
          Duration = None
          SeatNumber = None
          FlightNumber = None
          From = None
          To = None
          Airline = None }

type MicrosoftODataSampleServiceModelsTripPinEvent =
    { ``@odata.type``: Option<string>
      PlanItemId: Option<int>
      ConfirmationCode: Option<string>
      StartsAt: Option<System.DateTimeOffset>
      EndsAt: Option<System.DateTimeOffset>
      Duration: Option<string>
      Description: Option<string>
      OccursAt: Option<obj> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinEvent with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinEvent =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Event"
          PlanItemId = None
          ConfirmationCode = None
          StartsAt = None
          EndsAt = None
          Duration = None
          Description = None
          OccursAt = None }

type MicrosoftODataSampleServiceModelsTripPinTrip =
    { ``@odata.type``: Option<string>
      TripId: Option<int>
      ShareId: Option<System.Guid>
      Description: Option<string>
      Name: Option<string>
      Budget: Option<obj>
      StartsAt: Option<System.DateTimeOffset>
      EndsAt: Option<System.DateTimeOffset>
      Tags: Option<list<string>>
      Photos: Option<list<MicrosoftODataSampleServiceModelsTripPinPhoto>>
      PlanItems: Option<list<MicrosoftODataSampleServiceModelsTripPinPlanItem>> }
    ///Creates an instance of MicrosoftODataSampleServiceModelsTripPinTrip with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MicrosoftODataSampleServiceModelsTripPinTrip =
        { ``@odata.type`` = Some "#Microsoft.OData.SampleService.Models.TripPin.Trip"
          TripId = None
          ShareId = None
          Description = None
          Name = None
          Budget = None
          StartsAt = None
          EndsAt = None
          Tags = None
          Photos = None
          PlanItems = None }

type EdmGeography = Map<string, obj>

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Type =
    | [<CompiledName "Point">] Point
    member this.Format() =
        match this with
        | Point -> "Point"

type EdmGeographyPoint =
    { ``@odata.type``: Option<string>
      ``type``: Type
      coordinates: GeoJSONposition }
    ///Creates an instance of EdmGeographyPoint with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: Type, coordinates: GeoJSONposition): EdmGeographyPoint =
        { ``@odata.type`` = Some "#Edm.GeographyPoint"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeographyLineStringType =
    | [<CompiledName "LineString">] LineString
    member this.Format() =
        match this with
        | LineString -> "LineString"

type EdmGeographyLineString =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeographyLineStringType
      coordinates: list<GeoJSONposition> }
    ///Creates an instance of EdmGeographyLineString with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeographyLineStringType, coordinates: list<GeoJSONposition>): EdmGeographyLineString =
        { ``@odata.type`` = Some "#Edm.GeographyLineString"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeographyPolygonType =
    | [<CompiledName "Polygon">] Polygon
    member this.Format() =
        match this with
        | Polygon -> "Polygon"

type EdmGeographyPolygon =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeographyPolygonType
      coordinates: list<list<GeoJSONposition>> }
    ///Creates an instance of EdmGeographyPolygon with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeographyPolygonType, coordinates: list<list<GeoJSONposition>>): EdmGeographyPolygon =
        { ``@odata.type`` = Some "#Edm.GeographyPolygon"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeographyMultiPointType =
    | [<CompiledName "MultiPoint">] MultiPoint
    member this.Format() =
        match this with
        | MultiPoint -> "MultiPoint"

type EdmGeographyMultiPoint =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeographyMultiPointType
      coordinates: list<GeoJSONposition> }
    ///Creates an instance of EdmGeographyMultiPoint with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeographyMultiPointType, coordinates: list<GeoJSONposition>): EdmGeographyMultiPoint =
        { ``@odata.type`` = Some "#Edm.GeographyMultiPoint"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeographyMultiLineStringType =
    | [<CompiledName "MultiLineString">] MultiLineString
    member this.Format() =
        match this with
        | MultiLineString -> "MultiLineString"

type EdmGeographyMultiLineString =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeographyMultiLineStringType
      coordinates: list<list<GeoJSONposition>> }
    ///Creates an instance of EdmGeographyMultiLineString with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeographyMultiLineStringType, coordinates: list<list<GeoJSONposition>>): EdmGeographyMultiLineString =
        { ``@odata.type`` = Some "#Edm.GeographyMultiLineString"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeographyMultiPolygonType =
    | [<CompiledName "MultiPolygon">] MultiPolygon
    member this.Format() =
        match this with
        | MultiPolygon -> "MultiPolygon"

type EdmGeographyMultiPolygon =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeographyMultiPolygonType
      coordinates: list<list<list<GeoJSONposition>>> }
    ///Creates an instance of EdmGeographyMultiPolygon with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeographyMultiPolygonType, coordinates: list<list<list<GeoJSONposition>>>): EdmGeographyMultiPolygon =
        { ``@odata.type`` = Some "#Edm.GeographyMultiPolygon"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeographyCollectionType =
    | [<CompiledName "GeometryCollection">] GeometryCollection
    member this.Format() =
        match this with
        | GeometryCollection -> "GeometryCollection"

type EdmGeographyCollection =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeographyCollectionType
      coordinates: list<EdmGeometry> }
    ///Creates an instance of EdmGeographyCollection with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeographyCollectionType, coordinates: list<EdmGeometry>): EdmGeographyCollection =
        { ``@odata.type`` = Some "#Edm.GeographyCollection"
          ``type`` = ``type``
          coordinates = coordinates }

type EdmGeometry = Map<string, obj>

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryPointType =
    | [<CompiledName "Point">] Point
    member this.Format() =
        match this with
        | Point -> "Point"

type EdmGeometryPoint =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryPointType
      coordinates: GeoJSONposition }
    ///Creates an instance of EdmGeometryPoint with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryPointType, coordinates: GeoJSONposition): EdmGeometryPoint =
        { ``@odata.type`` = Some "#Edm.GeometryPoint"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryLineStringType =
    | [<CompiledName "LineString">] LineString
    member this.Format() =
        match this with
        | LineString -> "LineString"

type EdmGeometryLineString =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryLineStringType
      coordinates: list<GeoJSONposition> }
    ///Creates an instance of EdmGeometryLineString with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryLineStringType, coordinates: list<GeoJSONposition>): EdmGeometryLineString =
        { ``@odata.type`` = Some "#Edm.GeometryLineString"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryPolygonType =
    | [<CompiledName "Polygon">] Polygon
    member this.Format() =
        match this with
        | Polygon -> "Polygon"

type EdmGeometryPolygon =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryPolygonType
      coordinates: list<list<GeoJSONposition>> }
    ///Creates an instance of EdmGeometryPolygon with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryPolygonType, coordinates: list<list<GeoJSONposition>>): EdmGeometryPolygon =
        { ``@odata.type`` = Some "#Edm.GeometryPolygon"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryMultiPointType =
    | [<CompiledName "MultiPoint">] MultiPoint
    member this.Format() =
        match this with
        | MultiPoint -> "MultiPoint"

type EdmGeometryMultiPoint =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryMultiPointType
      coordinates: list<GeoJSONposition> }
    ///Creates an instance of EdmGeometryMultiPoint with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryMultiPointType, coordinates: list<GeoJSONposition>): EdmGeometryMultiPoint =
        { ``@odata.type`` = Some "#Edm.GeometryMultiPoint"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryMultiLineStringType =
    | [<CompiledName "MultiLineString">] MultiLineString
    member this.Format() =
        match this with
        | MultiLineString -> "MultiLineString"

type EdmGeometryMultiLineString =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryMultiLineStringType
      coordinates: list<list<GeoJSONposition>> }
    ///Creates an instance of EdmGeometryMultiLineString with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryMultiLineStringType, coordinates: list<list<GeoJSONposition>>): EdmGeometryMultiLineString =
        { ``@odata.type`` = Some "#Edm.GeometryMultiLineString"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryMultiPolygonType =
    | [<CompiledName "MultiPolygon">] MultiPolygon
    member this.Format() =
        match this with
        | MultiPolygon -> "MultiPolygon"

type EdmGeometryMultiPolygon =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryMultiPolygonType
      coordinates: list<list<list<GeoJSONposition>>> }
    ///Creates an instance of EdmGeometryMultiPolygon with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryMultiPolygonType, coordinates: list<list<list<GeoJSONposition>>>): EdmGeometryMultiPolygon =
        { ``@odata.type`` = Some "#Edm.GeometryMultiPolygon"
          ``type`` = ``type``
          coordinates = coordinates }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type EdmGeometryCollectionType =
    | [<CompiledName "GeometryCollection">] GeometryCollection
    member this.Format() =
        match this with
        | GeometryCollection -> "GeometryCollection"

type EdmGeometryCollection =
    { ``@odata.type``: Option<string>
      ``type``: EdmGeometryCollectionType
      coordinates: list<EdmGeometry> }
    ///Creates an instance of EdmGeometryCollection with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: EdmGeometryCollectionType, coordinates: list<EdmGeometry>): EdmGeometryCollection =
        { ``@odata.type`` = Some "#Edm.GeometryCollection"
          ``type`` = ``type``
          coordinates = coordinates }

type odataerror =
    { ``@odata.type``: Option<string>
      error: odataerrormain }
    ///Creates an instance of odataerror with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (error: odataerrormain): odataerror =
        { ``@odata.type`` = Some "#odata.error"
          error = error }

type odataerrormain =
    { ``@odata.type``: Option<string>
      code: string
      message: string
      target: Option<string>
      details: Option<list<odataerrordetail>>
      ///The structure of this object is service-specific
      innererror: Option<obj> }
    ///Creates an instance of odataerrormain with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (code: string, message: string): odataerrormain =
        { ``@odata.type`` = Some "#odata.error.main"
          code = code
          message = message
          target = None
          details = None
          innererror = None }

type odataerrordetail =
    { ``@odata.type``: Option<string>
      code: string
      message: string
      target: Option<string> }
    ///Creates an instance of odataerrordetail with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (code: string, message: string): odataerrordetail =
        { ``@odata.type`` = Some "#odata.error.detail"
          code = code
          message = message
          target = None }

type AirlinesAirlineListAirline_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinAirline>> }

[<RequireQualifiedAccess>]
type AirlinesAirlineListAirline =
    ///Retrieved entities
    | OK of payload: AirlinesAirlineListAirline_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type AirlinesAirlineCreateAirline =
    ///Created entity
    | Created of payload: MicrosoftODataSampleServiceModelsTripPinAirline
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type AirlinesAirlineGetAirline =
    ///Retrieved entity
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinAirline
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type AirlinesAirlineUpdateAirline =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type AirlinesAirlineDeleteAirline =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

type AirportsAirportListAirport_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinAirport>> }

[<RequireQualifiedAccess>]
type AirportsAirportListAirport =
    ///Retrieved entities
    | OK of payload: AirportsAirportListAirport_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type AirportsAirportGetAirport =
    ///Retrieved entity
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinAirport
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type AirportsAirportUpdateAirport =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type FunctionImportGetNearestAirport =
    ///Success
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinAirport
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MePersonGetPerson =
    ///Retrieved entity
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinPerson
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MePersonUpdatePerson =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

type MeListFriends_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinPerson>> }

[<RequireQualifiedAccess>]
type MeListFriends =
    ///Retrieved navigation property
    | OK of payload: MeListFriends_OK
    ///error
    | DefaultResponse of payload: odataerror

type MeListRefFriends_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<string>> }

[<RequireQualifiedAccess>]
type MeListRefFriends =
    ///Retrieved navigation property links
    | OK of payload: MeListRefFriends_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeCreateRefFriends =
    ///Created navigation property link.
    | Created of payload: obj
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeGetFavoriteAirline =
    ///Success
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinAirline
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeGetFriendsTrips =
    ///Success
    | OK of payload: ODataResponse<list<MicrosoftODataSampleServiceModelsTripPinTrip>>
    ///error
    | DefaultResponse of payload: odataerror

type MeShareTripPayload =
    { userName: Option<string>
      tripId: Option<int> }
    ///Creates an instance of MeShareTripPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MeShareTripPayload = { userName = None; tripId = None }

[<RequireQualifiedAccess>]
type MeShareTrip =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeGetPhoto =
    ///Retrieved navigation property
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinPhoto
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeGetRefPhoto =
    ///Retrieved navigation property link
    | OK of payload: ODataResponse<string>
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeUpdateRefPhoto =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeDeleteRefPhoto =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

type MeListTrips_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinTrip>> }

[<RequireQualifiedAccess>]
type MeListTrips =
    ///Retrieved navigation property
    | OK of payload: MeListTrips_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeCreateTrips =
    ///Created navigation property.
    | Created of payload: MicrosoftODataSampleServiceModelsTripPinTrip
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeGetTrips =
    ///Retrieved navigation property
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinTrip
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeUpdateTrips =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeDeleteTrips =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeTripsTripGetInvolvedPeople =
    ///Success
    | OK of payload: ODataResponse<list<MicrosoftODataSampleServiceModelsTripPinPerson>>
    ///error
    | DefaultResponse of payload: odataerror

type MeTripsListPhotos_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinPhoto>> }

[<RequireQualifiedAccess>]
type MeTripsListPhotos =
    ///Retrieved navigation property
    | OK of payload: MeTripsListPhotos_OK
    ///error
    | DefaultResponse of payload: odataerror

type MeTripsListRefPhotos_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<string>> }

[<RequireQualifiedAccess>]
type MeTripsListRefPhotos =
    ///Retrieved navigation property links
    | OK of payload: MeTripsListRefPhotos_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeTripsCreateRefPhotos =
    ///Created navigation property link.
    | Created of payload: obj
    ///error
    | DefaultResponse of payload: odataerror

type MeTripsListPlanItems_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinPlanItem>> }

[<RequireQualifiedAccess>]
type MeTripsListPlanItems =
    ///Retrieved navigation property
    | OK of payload: MeTripsListPlanItems_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeTripsCreatePlanItems =
    ///Created navigation property.
    | Created of payload: MicrosoftODataSampleServiceModelsTripPinPlanItem
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeTripsGetPlanItems =
    ///Retrieved navigation property
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinPlanItem
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeTripsUpdatePlanItems =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type MeTripsDeletePlanItems =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

type PeoplePersonListPerson_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinPerson>> }

[<RequireQualifiedAccess>]
type PeoplePersonListPerson =
    ///Retrieved entities
    | OK of payload: PeoplePersonListPerson_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonCreatePerson =
    ///Created entity
    | Created of payload: MicrosoftODataSampleServiceModelsTripPinPerson
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonGetPerson =
    ///Retrieved entity
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinPerson
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonUpdatePerson =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonDeletePerson =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

type PeopleListFriends_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinPerson>> }

[<RequireQualifiedAccess>]
type PeopleListFriends =
    ///Retrieved navigation property
    | OK of payload: PeopleListFriends_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonGetFavoriteAirline =
    ///Success
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinAirline
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonGetFriendsTrips =
    ///Success
    | OK of payload: ODataResponse<list<MicrosoftODataSampleServiceModelsTripPinTrip>>
    ///error
    | DefaultResponse of payload: odataerror

type PeoplePersonShareTripPayload =
    { userName: Option<string>
      tripId: Option<int> }
    ///Creates an instance of PeoplePersonShareTripPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PeoplePersonShareTripPayload = { userName = None; tripId = None }

[<RequireQualifiedAccess>]
type PeoplePersonShareTrip =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PeoplePersonTripsTripGetInvolvedPeople =
    ///Success
    | OK of payload: ODataResponse<list<MicrosoftODataSampleServiceModelsTripPinPerson>>
    ///error
    | DefaultResponse of payload: odataerror

type PhotosPhotoListPhoto_OK =
    { ``@odata.type``: Option<string>
      value: Option<list<MicrosoftODataSampleServiceModelsTripPinPhoto>> }

[<RequireQualifiedAccess>]
type PhotosPhotoListPhoto =
    ///Retrieved entities
    | OK of payload: PhotosPhotoListPhoto_OK
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PhotosPhotoCreatePhoto =
    ///Created entity
    | Created of payload: MicrosoftODataSampleServiceModelsTripPinPhoto
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PhotosPhotoGetPhoto =
    ///Retrieved entity
    | OK of payload: MicrosoftODataSampleServiceModelsTripPinPhoto
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PhotosPhotoUpdatePhoto =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PhotosPhotoDeletePhoto =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PhotosPhotoGetContent =
    ///Retrieved media content
    | OK of payload: byte []
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type PhotosPhotoUpdateContent =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror

[<RequireQualifiedAccess>]
type ActionImportResetDataSource =
    ///Success
    | NoContent
    ///error
    | DefaultResponse of payload: odataerror
