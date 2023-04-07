namespace rec Swashbuckle.Types

type ProblemDetails =
    { ``type``: Option<string>
      title: Option<string>
      status: Option<int>
      detail: Option<string>
      instance: Option<string> }
    ///Creates an instance of ProblemDetails with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProblemDetails =
        { ``type`` = None
          title = None
          status = None
          detail = None
          instance = None }

type ValidationProblemDetails =
    { ``type``: Option<string>
      title: Option<string>
      status: Option<int>
      detail: Option<string>
      instance: Option<string>
      errors: Option<Map<string, list<string>>> }
    ///Creates an instance of ValidationProblemDetails with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ValidationProblemDetails =
        { ``type`` = None
          title = None
          status = None
          detail = None
          instance = None
          errors = None }

[<RequireQualifiedAccess>]
type GetTime =
    ///Success
    | OK of payload: int
    ///Bad Request
    | BadRequest of payload: ValidationProblemDetails
    ///Forbidden
    | Forbidden of payload: ProblemDetails
