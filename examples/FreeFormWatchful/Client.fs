namespace rec FreeFormWatchful

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open FreeFormWatchful.Types
open FreeFormWatchful.Http

type FreeFormWatchfulClient(httpClient: HttpClient) =
    ///<summary>
    ///Returns a list of audits
    ///</summary>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetAudits(?limit: int64, ?limitstart: int64, ?order: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/audits" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetAudits.OK(Serializer.deserialize content)
            else
                return GetAudits.Forbidden
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsAudits(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/audits/metadata" requestParts cancellationToken
            return GetFieldsAudits.OK content
        }

    ///<summary>
    ///Delete a specific audit
    ///</summary>
    ///<param name="id">ID of audit that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteAuditById(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/audits/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DeleteAuditById.OK content
            else if status = HttpStatusCode.Forbidden then
                return DeleteAuditById.Forbidden
            else
                return DeleteAuditById.NotFound
        }

    ///<summary>
    ///Returns a audit based on ID
    ///</summary>
    ///<param name="id">ID of audit that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetAuditById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/audits/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetAuditById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetAuditById.BadRequest
            else
                return GetAuditById.Forbidden
        }

    ///<summary>
    ///Returns a list Extensions
    ///</summary>
    ///<param name="extName">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="siteids">List of sites id separated by comma</param>
    ///<param name="extPrefix">Do a 'LIKE' search, you can also use '%'. technical name of the extension com_xxxx</param>
    ///<param name="version">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="vUpdate">update available for this extension</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetExtensions
        (
            ?extName: string,
            ?siteids: string,
            ?extPrefix: string,
            ?version: string,
            ?vUpdate: int,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if extName.IsSome then
                      RequestPart.query ("ext_name", extName.Value)
                  if siteids.IsSome then
                      RequestPart.query ("siteids", siteids.Value)
                  if extPrefix.IsSome then
                      RequestPart.query ("ext_prefix", extPrefix.Value)
                  if version.IsSome then
                      RequestPart.query ("version", version.Value)
                  if vUpdate.IsSome then
                      RequestPart.query ("vUpdate", vUpdate.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/extensions" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetExtensions.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetExtensions.Forbidden
            else
                return GetExtensions.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsExtensions(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/extensions/metadata" requestParts cancellationToken

            return GetFieldsExtensions.OK content
        }

    ///<summary>
    ///Set 'ignore updates' for a given extension / site_id
    ///</summary>
    ///<param name="id">ID of the extension</param>
    ///<param name="cancellationToken"></param>
    member this.IgnoreExtensionUpdate(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/extensions/{id}/ignore" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return IgnoreExtensionUpdate.OK content
            else
                return IgnoreExtensionUpdate.NotFound
        }

    ///<summary>
    ///Remove 'ignore updates' for a given extension
    ///</summary>
    ///<param name="id">ID of the extension</param>
    ///<param name="cancellationToken"></param>
    member this.UnignoreExtensionUpdate(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/extensions/{id}/unignore" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UnignoreExtensionUpdate.OK content
            else
                return UnignoreExtensionUpdate.NotFound
        }

    ///<summary>
    ///Update the extension on the remote site
    ///</summary>
    ///<param name="id">ID of the extension</param>
    ///<param name="cancellationToken"></param>
    member this.UpdateExtension(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/extensions/{id}/update" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UpdateExtension.OK content
            else
                return UpdateExtension.NotFound
        }

    ///<summary>
    ///Returns a list of feedbacks
    ///</summary>
    ///<param name="fields">Fields to return separate by comas (es. name,id)</param>
    ///<param name="cancellationToken"></param>
    member this.GetFeedbacks(?fields: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/feedbacks" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetFeedbacks.OK(Serializer.deserialize content)
            else
                return GetFeedbacks.Forbidden
        }

    ///<summary>
    ///Create a feedback
    ///</summary>
    member this.CreateFeedbacks(body: Feedback, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/feedbacks" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateFeedbacks.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return CreateFeedbacks.Created
            else if status = HttpStatusCode.BadRequest then
                return CreateFeedbacks.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return CreateFeedbacks.Forbidden
            else
                return CreateFeedbacks.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsFeedbacks(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/feedbacks/metadata" requestParts cancellationToken

            return GetFieldsFeedbacks.OK content
        }

    ///<summary>
    ///Returns a list of logs
    ///</summary>
    ///<param name="logType">Type of the log</param>
    ///<param name="logEntry">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="from">Logs after this date, format YYYY-MM-DD HH:MM:SS</param>
    ///<param name="to">Logs before this date, format YYYY-MM-DD HH:MM:SS</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetLogs
        (
            ?logType: string,
            ?logEntry: string,
            ?from: string,
            ?``to``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if logType.IsSome then
                      RequestPart.query ("log_type", logType.Value)
                  if logEntry.IsSome then
                      RequestPart.query ("log_entry", logEntry.Value)
                  if from.IsSome then
                      RequestPart.query ("from", from.Value)
                  if ``to``.IsSome then
                      RequestPart.query ("to", ``to``.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/logs" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetLogs.OK(Serializer.deserialize content)
            else
                return GetLogs.Forbidden
        }

    ///<summary>
    ///Returns a file contain the list of logs
    ///</summary>
    ///<param name="format">Format of exported file (PDF or CSV)</param>
    ///<param name="site">Site id of the log</param>
    ///<param name="filterType">Type of the log</param>
    ///<param name="search">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="startdate">Logs after this date, format YYYY-MM-DD HH:MM:SS</param>
    ///<param name="enddate">Logs before this date, format YYYY-MM-DD HH:MM:SS</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="startid">Start of the return (default 0)</param>
    ///<param name="cancellationToken"></param>
    member this.GetExportLogs
        (
            format: string,
            ?site: int64,
            ?filterType: string,
            ?search: string,
            ?startdate: string,
            ?enddate: string,
            ?limit: int64,
            ?startid: int64,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.query ("format", format)
                  if site.IsSome then
                      RequestPart.query ("site", site.Value)
                  if filterType.IsSome then
                      RequestPart.query ("filter_type", filterType.Value)
                  if search.IsSome then
                      RequestPart.query ("search", search.Value)
                  if startdate.IsSome then
                      RequestPart.query ("startdate", startdate.Value)
                  if enddate.IsSome then
                      RequestPart.query ("enddate", enddate.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if startid.IsSome then
                      RequestPart.query ("startid", startid.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/logs/export" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetExportLogs.OK
            else
                return GetExportLogs.Forbidden
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsLogs(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/logs/metadata" requestParts cancellationToken
            return GetFieldsLogs.OK content
        }

    ///<summary>
    ///Returns a list of log types
    ///</summary>
    member this.GetTypesLogs(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/logs/types" requestParts cancellationToken
            return GetTypesLogs.OK content
        }

    ///<summary>
    ///Delete a specific log
    ///</summary>
    ///<param name="id">ID of log that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteLogById(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/logs/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DeleteLogById.OK content
            else if status = HttpStatusCode.Forbidden then
                return DeleteLogById.Forbidden
            else
                return DeleteLogById.NotFound
        }

    member this.PostPackages(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync httpClient "/packages" requestParts cancellationToken
            return PostPackages.DefaultResponse
        }

    ///<summary>
    ///Returns a PDF report based on a site ID
    ///</summary>
    ///<param name="id">ID that needs to be fetched</param>
    ///<param name="from">Start of the report, format YYYY-MM-DD, default today-30day </param>
    ///<param name="to">End of the report, format YYYY-MM-DD, default today</param>
    ///<param name="reports">Type of reports separate by comas: Ga,Logs,Uptime</param>
    ///<param name="logType">Type of the log to show in the report</param>
    ///<param name="compare">Define if you want show previous values in Google Analytics graph</param>
    ///<param name="cancellationToken"></param>
    member this.GetReportsSitesById
        (
            id: int64,
            ?from: string,
            ?``to``: string,
            ?reports: string,
            ?logType: string,
            ?compare: int,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if from.IsSome then
                      RequestPart.query ("from", from.Value)
                  if ``to``.IsSome then
                      RequestPart.query ("to", ``to``.Value)
                  if reports.IsSome then
                      RequestPart.query ("reports", reports.Value)
                  if logType.IsSome then
                      RequestPart.query ("log_type", logType.Value)
                  if compare.IsSome then
                      RequestPart.query ("compare", compare.Value) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/reports/sites/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetReportsSitesById.OK contentBinary
            else if status = HttpStatusCode.Forbidden then
                return GetReportsSitesById.Forbidden contentBinary
            else
                return GetReportsSitesById.NotFound contentBinary
        }

    ///<summary>
    ///Returns a report based on a site ID
    ///</summary>
    ///<param name="id">ID that needs to be fetched</param>
    ///<param name="from">Start of the report, format YYYY-MM-DD, default today-30day </param>
    ///<param name="to">End of the report, format YYYY-MM-DD, default today</param>
    ///<param name="reports">Type of reports separate by comas: Ga,Logs,Uptime</param>
    ///<param name="logType">Type of the log to show in the report</param>
    ///<param name="compare">Define if you want show previous values in Google Analytics graph</param>
    ///<param name="cancellationToken"></param>
    member this.GetReportsTagsById
        (
            id: int64,
            ?from: string,
            ?``to``: string,
            ?reports: string,
            ?logType: string,
            ?compare: int,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if from.IsSome then
                      RequestPart.query ("from", from.Value)
                  if ``to``.IsSome then
                      RequestPart.query ("to", ``to``.Value)
                  if reports.IsSome then
                      RequestPart.query ("reports", reports.Value)
                  if logType.IsSome then
                      RequestPart.query ("log_type", logType.Value)
                  if compare.IsSome then
                      RequestPart.query ("compare", compare.Value) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/reports/tags/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetReportsTagsById.OK contentBinary
            else if status = HttpStatusCode.Forbidden then
                return GetReportsTagsById.Forbidden contentBinary
            else
                return GetReportsTagsById.NotFound contentBinary
        }

    ///<summary>
    ///Returns a list of Sites
    ///</summary>
    ///<param name="siteids">List of sites id separated by comma</param>
    ///<param name="name">Site name. Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="accessUrl">Access URL. Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="jVersion">Joomla version. Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="ip">Ip address. Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="jUpdate">Joomla core update status (1: update required, 0: update not required)</param>
    ///<param name="canUpdate">canUpdate</param>
    ///<param name="published">Is published</param>
    ///<param name="error">Has errors</param>
    ///<param name="nbUpdates"></param>
    ///<param name="up">Is online</param>
    ///<param name="fields">Fields to return separated by commas (e.g. name,id)</param>
    ///<param name="limit">Number of objects to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetSites
        (
            ?siteids: string,
            ?name: string,
            ?accessUrl: string,
            ?jVersion: string,
            ?ip: string,
            ?jUpdate: int,
            ?canUpdate: int,
            ?published: int,
            ?error: string,
            ?nbUpdates: string,
            ?up: int,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if siteids.IsSome then
                      RequestPart.query ("siteids", siteids.Value)
                  if name.IsSome then
                      RequestPart.query ("name", name.Value)
                  if accessUrl.IsSome then
                      RequestPart.query ("access_url", accessUrl.Value)
                  if jVersion.IsSome then
                      RequestPart.query ("j_version", jVersion.Value)
                  if ip.IsSome then
                      RequestPart.query ("ip", ip.Value)
                  if jUpdate.IsSome then
                      RequestPart.query ("jUpdate", jUpdate.Value)
                  if canUpdate.IsSome then
                      RequestPart.query ("canUpdate", canUpdate.Value)
                  if published.IsSome then
                      RequestPart.query ("published", published.Value)
                  if error.IsSome then
                      RequestPart.query ("error", error.Value)
                  if nbUpdates.IsSome then
                      RequestPart.query ("nbUpdates", nbUpdates.Value)
                  if up.IsSome then
                      RequestPart.query ("up", up.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSites.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSites.Forbidden
            else
                return GetSites.NotFound
        }

    ///<summary>
    ///Create a site
    ///</summary>
    member this.CreateSite(body: PostSite, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/sites" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateSite.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return CreateSite.Created
            else if status = HttpStatusCode.BadRequest then
                return CreateSite.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return CreateSite.Forbidden
            else
                return CreateSite.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetSitesMetadata(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/metadata" requestParts cancellationToken
            return GetSitesMetadata.OK content
        }

    ///<summary>
    ///Delete a specific Site
    ///</summary>
    ///<param name="id">ID of Site that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSitesById(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/sites/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DeleteSitesById.OK content
            else if status = HttpStatusCode.Forbidden then
                return DeleteSitesById.Forbidden
            else
                return DeleteSitesById.NotFound
        }

    ///<summary>
    ///Return a site based on ID
    ///</summary>
    ///<param name="id">ID that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetSiteById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSiteById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSiteById.Forbidden
            else
                return GetSiteById.NotFound
        }

    ///<summary>
    ///Update a site
    ///</summary>
    ///<param name="id">ID of the website that needs to be update</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PutSitesById(id: int64, body: PostSite, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync httpClient "/sites/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PutSitesById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return PutSitesById.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return PutSitesById.Forbidden
            else
                return PutSitesById.NotFound
        }

    ///<summary>
    ///Return audits for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field</param>
    ///<param name="cancellationToken"></param>
    member this.GetSiteAudits
        (
            id: int64,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/{id}/audits" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSiteAudits.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSiteAudits.Forbidden
            else
                return GetSiteAudits.NotFound
        }

    ///<summary>
    ///Create an audit for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.CreateAudits(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/audits" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateAudits.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return CreateAudits.Created
            else if status = HttpStatusCode.BadRequest then
                return CreateAudits.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return CreateAudits.Forbidden
            else
                return CreateAudits.NotFound
        }

    ///<summary>
    ///Add the site to the backup queue
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.AddSiteToBackupQueue(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/backupnow" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return AddSiteToBackupQueue.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return AddSiteToBackupQueue.Forbidden
            else
                return AddSiteToBackupQueue.NotFound
        }

    ///<summary>
    ///Return backup profile
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.GetBackupProfiles(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/sites/{id}/backupprofiles" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetBackupProfiles.OK
            else if status = HttpStatusCode.Forbidden then
                return GetBackupProfiles.Forbidden
            else
                return GetBackupProfiles.NotFound
        }

    ///<summary>
    ///List of latest backups
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.GetListBackups(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/sites/{id}/backups" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetListBackups.OK
            else if status = HttpStatusCode.Forbidden then
                return GetListBackups.Forbidden
            else
                return GetListBackups.NotFound
        }

    ///<summary>
    ///Start a remote backup for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.StartSiteBackup(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/backupstart" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return StartSiteBackup.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return StartSiteBackup.Forbidden
            else
                return StartSiteBackup.NotFound
        }

    ///<summary>
    ///Step (continue) a remote backup for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.StepSiteBackup(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/backupstep" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return StepSiteBackup.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return StepSiteBackup.Forbidden
            else
                return StepSiteBackup.NotFound
        }

    ///<summary>
    ///Get extensions for a site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field</param>
    ///<param name="cancellationToken"></param>
    member this.GetSitesExtensionsById
        (
            id: int64,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/sites/{id}/extensions" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSitesExtensionsById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSitesExtensionsById.Forbidden
            else
                return GetSitesExtensionsById.NotFound
        }

    ///<summary>
    ///Install extension
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="url">URL to install the extension from</param>
    ///<param name="cancellationToken"></param>
    member this.InstallExtension(id: int64, url: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.query ("url", url) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/extensions" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return InstallExtension.OK
            else if status = HttpStatusCode.Forbidden then
                return InstallExtension.Forbidden
            else
                return InstallExtension.NotFound
        }

    ///<summary>
    ///Return logs for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="logEntry">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="logType">Type of the log</param>
    ///<param name="from">Logs after this date, format YYYY-MM-DD HH:MM:SS</param>
    ///<param name="to">Logs before this date, format YYYY-MM-DD HH:MM:SS</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetSitesLogsById
        (
            id: int64,
            ?logEntry: string,
            ?logType: string,
            ?from: string,
            ?``to``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if logEntry.IsSome then
                      RequestPart.query ("log_entry", logEntry.Value)
                  if logType.IsSome then
                      RequestPart.query ("log_type", logType.Value)
                  if from.IsSome then
                      RequestPart.query ("from", from.Value)
                  if ``to``.IsSome then
                      RequestPart.query ("to", ``to``.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/{id}/logs" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSitesLogsById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSitesLogsById.Forbidden
            else
                return GetSitesLogsById.NotFound
        }

    ///<summary>
    ///Create a custom log for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.CreateLog(id: int64, body: PostLog, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/sites/{id}/logs" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateLog.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return CreateLog.Created
            else if status = HttpStatusCode.BadRequest then
                return CreateLog.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return CreateLog.Forbidden
            else
                return CreateLog.NotFound
        }

    ///<summary>
    ///Return boolean
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteMonitor(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/sites/{id}/monitor" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DeleteMonitor.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return DeleteMonitor.Forbidden
            else
                return DeleteMonitor.NotFound
        }

    ///<summary>
    ///Return boolean
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.PostMonitor(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/monitor" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PostMonitor.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return PostMonitor.Forbidden
            else
                return PostMonitor.NotFound
        }

    ///<summary>
    ///Scan the site for malware
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.Scanner(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/sites/{id}/scanner" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return Scanner.OK content
            else if status = HttpStatusCode.Forbidden then
                return Scanner.Forbidden
            else
                return Scanner.NotFound
        }

    ///<summary>
    ///SEO analyze for a page
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.SeoAnalyze(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/{id}/seo" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return SeoAnalyze.OK content
            else if status = HttpStatusCode.Forbidden then
                return SeoAnalyze.Forbidden
            else
                return SeoAnalyze.NotFound
        }

    ///<summary>
    ///Return tags for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="name">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="type">Bootstrap color of the tag</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field</param>
    ///<param name="cancellationToken"></param>
    member this.GetSitesTagsById
        (
            id: int64,
            ?name: string,
            ?``type``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if name.IsSome then
                      RequestPart.query ("name", name.Value)
                  if ``type``.IsSome then
                      RequestPart.query ("type", ``type``.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/{id}/tags" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSitesTagsById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSitesTagsById.Forbidden
            else
                return GetSitesTagsById.NotFound
        }

    ///<summary>
    ///Add tags for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PostTags(id: int64, body: Tag, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/sites/{id}/tags" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PostTags.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return PostTags.Created
            else if status = HttpStatusCode.Forbidden then
                return PostTags.Forbidden
            else
                return PostTags.NotFound
        }

    ///<summary>
    ///Update Joomla core on the remote site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.UpdateJoomla(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/sites/{id}/updatejoomla" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UpdateJoomla.OK content
            else if status = HttpStatusCode.Forbidden then
                return UpdateJoomla.Forbidden
            else
                return UpdateJoomla.NotFound
        }

    ///<summary>
    ///Return uptime data
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.GetUptime(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/sites/{id}/uptime" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetUptime.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetUptime.Forbidden
            else
                return GetUptime.NotFound
        }

    ///<summary>
    ///validate the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.ValidateSite(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/sites/{id}/validate" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return ValidateSite.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return ValidateSite.Forbidden
            else
                return ValidateSite.NotFound
        }

    ///<summary>
    ///validate the site, return the debug information
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.ValidateDebugSite(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/sites/{id}/validatedebug" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return ValidateDebugSite.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return ValidateDebugSite.Forbidden
            else
                return ValidateDebugSite.NotFound
        }

    ///<summary>
    ///Returns a list of SSO Users
    ///</summary>
    member this.GetSsoUsers(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/ssousers" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSsoUsers.OK(Serializer.deserialize content)
            else
                return GetSsoUsers.Forbidden
        }

    ///<summary>
    ///Create a SSO User
    ///</summary>
    member this.CreateSsoUsers(body: SsoUsers, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/ssousers" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateSsoUsers.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return CreateSsoUsers.Created
            else if status = HttpStatusCode.BadRequest then
                return CreateSsoUsers.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return CreateSsoUsers.Forbidden
            else
                return CreateSsoUsers.NotFound
        }

    ///<summary>
    ///Delete a specific SSO User
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSsoUserById(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/ssousers/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DeleteSsoUserById.OK content
            else if status = HttpStatusCode.Forbidden then
                return DeleteSsoUserById.Forbidden
            else
                return DeleteSsoUserById.NotFound
        }

    ///<summary>
    ///Returns a SSO User based on ID
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetSsoUsersById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/ssousers/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSsoUsersById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetSsoUsersById.BadRequest
            else
                return GetSsoUsersById.Forbidden
        }

    ///<summary>
    ///Update a SSO User
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be updated</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateSsoUsers(id: int64, body: SsoUsers, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync httpClient "/ssousers/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UpdateSsoUsers.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return UpdateSsoUsers.Created
            else if status = HttpStatusCode.BadRequest then
                return UpdateSsoUsers.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return UpdateSsoUsers.Forbidden
            else
                return UpdateSsoUsers.NotFound
        }

    ///<summary>
    ///Returns a list of tags
    ///</summary>
    ///<param name="name">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="type">Bootstrap color of the tag</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetTags
        (
            ?name: string,
            ?``type``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if name.IsSome then
                      RequestPart.query ("name", name.Value)
                  if ``type``.IsSome then
                      RequestPart.query ("type", ``type``.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/tags" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetTags.OK(Serializer.deserialize content)
            else
                return GetTags.Forbidden
        }

    ///<summary>
    ///Create a tag
    ///</summary>
    member this.CreateTags(body: Tag, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/tags" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateTags.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return CreateTags.Created
            else if status = HttpStatusCode.BadRequest then
                return CreateTags.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return CreateTags.Forbidden
            else
                return CreateTags.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetTagsMetadata(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/tags/metadata" requestParts cancellationToken
            return GetTagsMetadata.OK content
        }

    ///<summary>
    ///Delete a specific tag
    ///</summary>
    ///<param name="id">ID of tag that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteTagsById(id: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/tags/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return DeleteTagsById.OK content
            else if status = HttpStatusCode.Forbidden then
                return DeleteTagsById.Forbidden
            else
                return DeleteTagsById.NotFound
        }

    ///<summary>
    ///Returns a tag based on ID
    ///</summary>
    ///<param name="id">ID of tag that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetTagById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/tags/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetTagById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetTagById.BadRequest
            else
                return GetTagById.Forbidden
        }

    ///<summary>
    ///Update a tag
    ///</summary>
    ///<param name="id">ID of tag</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateTag(id: int64, body: Tag, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync httpClient "/tags/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UpdateTag.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return UpdateTag.BadRequest
            else if status = HttpStatusCode.Forbidden then
                return UpdateTag.Forbidden
            else
                return UpdateTag.NotFound
        }

    ///<summary>
    ///Returns a list of sites based with a specific tag id
    ///</summary>
    ///<param name="id">ID of tag that needs to be fetched</param>
    ///<param name="name">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="accessUrl">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="jVersion">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="ip">Do a 'LIKE' search, you can also use '%'</param>
    ///<param name="jUpdate">Joomla core update</param>
    ///<param name="published">is published</param>
    ///<param name="error">have errors</param>
    ///<param name="nbUpdates"></param>
    ///<param name="up">is the website online</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetSitesByTags
        (
            id: int64,
            ?name: string,
            ?accessUrl: string,
            ?jVersion: string,
            ?ip: string,
            ?jUpdate: int,
            ?published: int,
            ?error: string,
            ?nbUpdates: string,
            ?up: int,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if name.IsSome then
                      RequestPart.query ("name", name.Value)
                  if accessUrl.IsSome then
                      RequestPart.query ("access_url", accessUrl.Value)
                  if jVersion.IsSome then
                      RequestPart.query ("j_version", jVersion.Value)
                  if ip.IsSome then
                      RequestPart.query ("ip", ip.Value)
                  if jUpdate.IsSome then
                      RequestPart.query ("jUpdate", jUpdate.Value)
                  if published.IsSome then
                      RequestPart.query ("published", published.Value)
                  if error.IsSome then
                      RequestPart.query ("error", error.Value)
                  if nbUpdates.IsSome then
                      RequestPart.query ("nbUpdates", nbUpdates.Value)
                  if up.IsSome then
                      RequestPart.query ("up", up.Value)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value)
                  if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/tags/{id}/sites" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetSitesByTags.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Forbidden then
                return GetSitesByTags.Forbidden
            else
                return GetSitesByTags.NotFound
        }
