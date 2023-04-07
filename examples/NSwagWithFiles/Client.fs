namespace rec NSwagWithFiles

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open NSwagWithFiles.Types
open NSwagWithFiles.Http

///REST endpoints of NSwag
type NSwagWithFilesClient(httpClient: HttpClient) =
    ///<summary>
    ///Uploads an image property of a branding.
    ///</summary>
    ///<param name="brandingId">The ID of the branding to upload an image for</param>
    ///<param name="body"></param>
    ///<param name="property">Specifies which image property to upload to. Can be either "logo", "favicon" or "feature_image"</param>
    ///<param name="authorization">An authorization header with a bearer token of a SuperAdmin user.</param>
    ///<param name="cancellationToken"></param>
    member this.UploadBrandingImage
        (
            brandingId: int,
            body: byte [],
            ?property: string,
            ?authorization: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.query ("brandingId", brandingId)
                  RequestPart.multipartFormData ("body", body)
                  if property.IsSome then
                      RequestPart.query ("property", property.Value)
                  if authorization.IsSome then
                      RequestPart.header ("authorization", authorization.Value) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/api/Brandings/upload" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UploadBrandingImage.OK
            else if status = HttpStatusCode.BadRequest then
                return UploadBrandingImage.BadRequest(Serializer.deserialize content)
            else
                return UploadBrandingImage.Unauthorized(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrieves the data sources for a supplier (data measuring company) for the specified period.
    ///</summary>
    ///<param name="supplier">The name of the supplier (measuring company)</param>
    ///<param name="period">The period for which to retrieve the data</param>
    ///<param name="authorization">
    ///An authorization header in the format Basic {base64(username:password)}
    ///where username and password are the credentials of an admin user. For example
    ///if your username = 'admin' and password = 'admin' then the authorization header should be "Basic YWRtaW46YWRtaW4="
    ///</param>
    ///<param name="cancellationToken"></param>
    member this.GetDataSources
        (
            ?supplier: string,
            ?period: string,
            ?authorization: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if supplier.IsSome then
                      RequestPart.query ("supplier", supplier.Value)
                  if period.IsSome then
                      RequestPart.query ("period", period.Value)
                  if authorization.IsSome then
                      RequestPart.header ("authorization", authorization.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/api/Datahub/datasources" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetDataSources.OK(Serializer.deserialize content)
            else
                return GetDataSources.Unauthorized content
        }

    ///<summary>
    ///Expects the request body to be JSON formatted as follows:
    ///             [
    ///  {
    ///     "ic": {import code},
    ///     "values": [
    ///         { "v": {value}, "t": {unix timestamp} }
    ///     ]
    ///   }
    ///             ]
    ///</summary>
    ///<param name="timeshift">Whether or not the values should be timeshifted</param>
    ///<param name="authorization">
    ///An authorization header in the format Basic {base64(username:password)}
    ///where username and password are the credentials of an admin user. For example
    ///if your username = 'admin' and password = 'admin' then the authorization header should be "Basic YWRtaW46YWRtaW4="
    ///</param>
    ///<param name="cancellationToken"></param>
    member this.ImportData(timeshift: bool, ?authorization: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.query ("timeshift", timeshift)
                  if authorization.IsSome then
                      RequestPart.header ("authorization", authorization.Value) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/api/Datahub/import" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return ImportData.OK(Serializer.deserialize content)
            else
                return ImportData.Unauthorized content
        }

    member this.UploadFile(nodeId: int, ?path: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.query ("nodeId", nodeId)
                  if path.IsSome then
                      RequestPart.query ("path", path.Value) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/api/Documents/upload" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UploadFile.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return UploadFile.BadRequest(Serializer.deserialize content)
            else
                return UploadFile.Unauthorized(Serializer.deserialize content)
        }

    member this.DownloadFile(nodeId: int, ?path: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.query ("nodeId", nodeId)
                  if path.IsSome then
                      RequestPart.query ("path", path.Value) ]

            let! (status, contentBinary) =
                OpenApiHttp.postBinaryAsync httpClient "/api/Documents/download" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DownloadFile.OK contentBinary
            else if status = HttpStatusCode.BadRequest then
                let content = Encoding.UTF8.GetString contentBinary
                return DownloadFile.BadRequest(Serializer.deserialize content)
            else
                let content = Encoding.UTF8.GetString contentBinary
                return DownloadFile.Unauthorized(Serializer.deserialize content)
        }

    member this.HealthGet(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/api/Health" requestParts cancellationToken

            return HealthGet.OK contentBinary
        }

    member this.StandardProjectsUploadFile(projectId: int, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.query ("projectId", projectId) ]

            let! (status, contentBinary) =
                OpenApiHttp.postBinaryAsync httpClient "/api/standardProjects/upload" requestParts cancellationToken

            return StandardProjectsUploadFile.OK contentBinary
        }
