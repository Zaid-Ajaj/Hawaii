namespace rec TaskNSwag.Types

type Error =
    { error: Option<string>
      details: Option<string> }
    ///Creates an instance of Error with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Error = { error = None; details = None }

type DataHubDataSource =
    { credentials: Option<string>
      meters: Option<list<DataHubMeter>> }
    ///Creates an instance of DataHubDataSource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): DataHubDataSource = { credentials = None; meters = None }

type DataHubMeter =
    { meterId: int
      importCode: Option<string>
      readFrequency: int
      cumulative: bool
      fromDate: int64
      toDate: int64
      fromDateFormatted: Option<string>
      toDateFormatted: Option<string> }
    ///Creates an instance of DataHubMeter with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (meterId: int, readFrequency: int, cumulative: bool, fromDate: int64, toDate: int64): DataHubMeter =
        { meterId = meterId
          importCode = None
          readFrequency = readFrequency
          cumulative = cumulative
          fromDate = fromDate
          toDate = toDate
          fromDateFormatted = None
          toDateFormatted = None }

type ImportOutput =
    { success: Option<list<ImportSuccessMeter>>
      failed: Option<list<ImportFailedMeter>> }
    ///Creates an instance of ImportOutput with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ImportOutput = { success = None; failed = None }

type ImportSuccessMeter =
    { importCode: Option<string>
      records: int }
    ///Creates an instance of ImportSuccessMeter with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (records: int): ImportSuccessMeter = { importCode = None; records = records }

type ImportFailedMeter =
    { importCode: Option<string>
      reason: Option<string> }
    ///Creates an instance of ImportFailedMeter with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ImportFailedMeter = { importCode = None; reason = None }

type UploadOutput =
    { results: Option<list<UploadFileResult>> }
    ///Creates an instance of UploadOutput with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UploadOutput = { results = None }

type UploadFileResult =
    { fileName: Option<string>
      result: Option<string> }
    ///Creates an instance of UploadFileResult with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UploadFileResult = { fileName = None; result = None }

type DocumentsError =
    { error: Option<string> }
    ///Creates an instance of DocumentsError with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): DocumentsError = { error = None }

[<RequireQualifiedAccess>]
type UploadBrandingImage =
    ///OK, the image was uploaded successfully
    | OK
    ///Bad request. Missing authorization header or invalid property parameter or the uploaded file is not a valid image
    | BadRequest of payload: Error
    ///Unauthorized. Only Super admins are allowed to upload branding images
    | Unauthorized of payload: Error

[<RequireQualifiedAccess>]
type GetDataSources =
    | OK of payload: list<DataHubDataSource>
    | Unauthorized of payload: string

[<RequireQualifiedAccess>]
type ImportData =
    ///The import output: for which meters data was imported and which meters failed
    | OK of payload: ImportOutput
    ///Authorization error
    | Unauthorized of payload: string

[<RequireQualifiedAccess>]
type UploadFile =
    | OK of payload: UploadOutput
    | BadRequest of payload: DocumentsError
    | Unauthorized of payload: DocumentsError

[<RequireQualifiedAccess>]
type DownloadFile =
    | OK of payload: byte []
    | BadRequest of payload: DocumentsError
    | Unauthorized of payload: DocumentsError

[<RequireQualifiedAccess>]
type HealthGet = OK of payload: byte []

[<RequireQualifiedAccess>]
type StandardProjectsUploadFile = OK of payload: byte []
