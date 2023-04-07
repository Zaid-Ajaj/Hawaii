namespace rec Ghibli.Types

type Films =
    { ///Unique identifier representing a specific film
      id: Option<string>
      ///Title of the film
      title: Option<string>
      ///Original title of the film
      original_title: Option<string>
      ///Orignal title in romanised form
      original_title_romanised: Option<string>
      ///Description of the film
      description: Option<string>
      ///Director of the film
      director: Option<string>
      ///Producer of the film
      producer: Option<string>
      ///Release year of film
      release_date: Option<string>
      ///Running time of the film in minutes
      running_time: Option<string>
      ///Rotten Tomato score of film
      rt_score: Option<string>
      ///People found in film
      people: Option<list<string>>
      ///Species found in film
      species: Option<list<string>>
      ///Locations found in film
      locations: Option<list<string>>
      ///Vehicles found in film
      vehicles: Option<list<string>>
      ///URL of film
      url: Option<string> }
    ///Creates an instance of Films with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Films =
        { id = None
          title = None
          original_title = None
          original_title_romanised = None
          description = None
          director = None
          producer = None
          release_date = None
          running_time = None
          rt_score = None
          people = None
          species = None
          locations = None
          vehicles = None
          url = None }

type People =
    { ///Unique identifier representing a specific person
      id: Option<string>
      ///Name of the person
      name: Option<string>
      ///Gender of the person
      gender: Option<string>
      ///Age, if known, of the person
      age: Option<string>
      ///Eye color of the person
      eye_color: Option<string>
      ///Hair color of the person
      hair_color: Option<string>
      ///Array of films the person appears in
      films: Option<list<string>>
      ///Species the person belongs to
      species: Option<string>
      ///Unique url of the person
      url: Option<string> }
    ///Creates an instance of People with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): People =
        { id = None
          name = None
          gender = None
          age = None
          eye_color = None
          hair_color = None
          films = None
          species = None
          url = None }

type Locations =
    { ///Unique identifier representing a specific location
      id: Option<string>
      ///Name of location
      name: Option<string>
      ///Climate of location
      climate: Option<string>
      ///Terrain type of location
      terrain: Option<string>
      ///Percent of location covered in water
      surface_water: Option<string>
      ///Array of residents in location
      residents: Option<list<string>>
      ///Array of films the location appears in
      films: Option<list<string>>
      ///Individual URL of the location
      url: Option<string> }
    ///Creates an instance of Locations with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Locations =
        { id = None
          name = None
          climate = None
          terrain = None
          surface_water = None
          residents = None
          films = None
          url = None }

type Species =
    { ///Unique identifier representing a specific species
      id: Option<string>
      ///Name of the species
      name: Option<string>
      ///Classification of the species
      classification: Option<string>
      ///Eye color of the species
      eye_color: Option<string>
      ///Hair color of the species
      hair_color: Option<string>
      ///People belonging to the species
      people: Option<list<string>>
      ///Array of films the species appears in
      films: Option<list<string>>
      ///Unique url of the species
      url: Option<string> }
    ///Creates an instance of Species with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Species =
        { id = None
          name = None
          classification = None
          eye_color = None
          hair_color = None
          people = None
          films = None
          url = None }

type Vehicles =
    { ///Unique identifier representing a specific vehicle
      id: Option<string>
      ///Name of the vehicles
      name: Option<string>
      ///Description of the vehicle
      description: Option<string>
      ///Class of the vehicle
      vehicle_class: Option<string>
      ///Length of the vehicle in feet
      length: Option<string>
      ///Pilot of the vehicle
      pilot: Option<string>
      ///Array of films the vehicle appears in
      films: Option<list<string>>
      ///Unique URL of the vehicle
      url: Option<string> }
    ///Creates an instance of Vehicles with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Vehicles =
        { id = None
          name = None
          description = None
          vehicle_class = None
          length = None
          pilot = None
          films = None
          url = None }

type Error =
    { code: Option<int>
      message: Option<string>
      fields: Option<string> }
    ///Creates an instance of Error with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Error =
        { code = None
          message = None
          fields = None }

[<RequireQualifiedAccess>]
type GetFilms =
    ///An array of films
    | OK of payload: list<Films>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetFilmsById =
    ///A single film is returned
    | OK of payload: list<Films>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetPeople =
    ///An array of people
    | OK of payload: list<People>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetPeopleById =
    ///A single person is returned
    | OK of payload: list<People>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetLocations =
    ///An array of locations
    | OK of payload: list<Locations>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetLocationsById =
    ///A single location is returned
    | OK of payload: Newtonsoft.Json.Linq.JToken
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetSpecies =
    ///An array of species
    | OK of payload: list<Species>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetSpeciesById =
    ///A single species is returned
    | OK of payload: list<Species>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetVehicles =
    ///An array of vehicles
    | OK of payload: list<Vehicles>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound

[<RequireQualifiedAccess>]
type GetVehiclesById =
    ///A single vehicle is returned
    | OK of payload: list<Vehicles>
    ///Bad request
    | BadRequest
    ///Not found
    | NotFound
