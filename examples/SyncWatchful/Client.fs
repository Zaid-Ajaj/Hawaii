namespace rec SyncWatchful

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open SyncWatchful.Types
open SyncWatchful.Http

type SyncWatchfulClient(httpClient: HttpClient) =
    ///<summary>
    ///Returns a list of audits
    ///</summary>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    ///<param name="cancellationToken"></param>
    member this.GetAudits(?limit: int64, ?limitstart: int64, ?order: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if limitstart.IsSome then
                  RequestPart.query ("limitstart", limitstart.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/audits" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetAudits.OK(Serializer.deserialize content)
        | _ -> GetAudits.Forbidden

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsAudits(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/audits/metadata" requestParts cancellationToken

        GetFieldsAudits.OK content

    ///<summary>
    ///Delete a specific audit
    ///</summary>
    ///<param name="id">ID of audit that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteAuditById(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/audits/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> DeleteAuditById.OK content
        | HttpStatusCode.Forbidden -> DeleteAuditById.Forbidden
        | _ -> DeleteAuditById.NotFound

    ///<summary>
    ///Returns a audit based on ID
    ///</summary>
    ///<param name="id">ID of audit that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetAuditById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              if fields.IsSome then
                  RequestPart.query ("fields", fields.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/audits/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetAuditById.OK(Serializer.deserialize content)
        | HttpStatusCode.BadRequest -> GetAuditById.BadRequest
        | _ -> GetAuditById.Forbidden

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

        let (status, content) =
            OpenApiHttp.get httpClient "/extensions" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetExtensions.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetExtensions.Forbidden
        | _ -> GetExtensions.NotFound

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsExtensions(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/extensions/metadata" requestParts cancellationToken

        GetFieldsExtensions.OK content

    ///<summary>
    ///Set 'ignore updates' for a given extension / site_id
    ///</summary>
    ///<param name="id">ID of the extension</param>
    ///<param name="cancellationToken"></param>
    member this.IgnoreExtensionUpdate(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/extensions/{id}/ignore" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> IgnoreExtensionUpdate.OK content
        | _ -> IgnoreExtensionUpdate.NotFound

    ///<summary>
    ///Remove 'ignore updates' for a given extension
    ///</summary>
    ///<param name="id">ID of the extension</param>
    ///<param name="cancellationToken"></param>
    member this.UnignoreExtensionUpdate(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/extensions/{id}/unignore" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> UnignoreExtensionUpdate.OK content
        | _ -> UnignoreExtensionUpdate.NotFound

    ///<summary>
    ///Update the extension on the remote site
    ///</summary>
    ///<param name="id">ID of the extension</param>
    ///<param name="cancellationToken"></param>
    member this.UpdateExtension(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/extensions/{id}/update" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> UpdateExtension.OK content
        | _ -> UpdateExtension.NotFound

    ///<summary>
    ///Returns a list of feedbacks
    ///</summary>
    ///<param name="fields">Fields to return separate by comas (es. name,id)</param>
    ///<param name="cancellationToken"></param>
    member this.GetFeedbacks(?fields: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if fields.IsSome then
                  RequestPart.query ("fields", fields.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/feedbacks" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetFeedbacks.OK(Serializer.deserialize content)
        | _ -> GetFeedbacks.Forbidden

    ///<summary>
    ///Create a feedback
    ///</summary>
    member this.CreateFeedbacks(body: Feedback, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/feedbacks" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> CreateFeedbacks.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> CreateFeedbacks.Created
        | HttpStatusCode.BadRequest -> CreateFeedbacks.BadRequest
        | HttpStatusCode.Forbidden -> CreateFeedbacks.Forbidden
        | _ -> CreateFeedbacks.NotFound

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsFeedbacks(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/feedbacks/metadata" requestParts cancellationToken

        GetFieldsFeedbacks.OK content

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

        let (status, content) =
            OpenApiHttp.get httpClient "/logs" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetLogs.OK(Serializer.deserialize content)
        | _ -> GetLogs.Forbidden

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

        let (status, content) =
            OpenApiHttp.get httpClient "/logs/export" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetExportLogs.OK
        | _ -> GetExportLogs.Forbidden

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsLogs(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/logs/metadata" requestParts cancellationToken

        GetFieldsLogs.OK content

    ///<summary>
    ///Returns a list of log types
    ///</summary>
    member this.GetTypesLogs(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/logs/types" requestParts cancellationToken

        GetTypesLogs.OK content

    ///<summary>
    ///Delete a specific log
    ///</summary>
    ///<param name="id">ID of log that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteLogById(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/logs/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> DeleteLogById.OK content
        | HttpStatusCode.Forbidden -> DeleteLogById.Forbidden
        | _ -> DeleteLogById.NotFound

    member this.PostPackages(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/packages" requestParts cancellationToken

        PostPackages.DefaultResponse

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

        let (status, contentBinary) =
            OpenApiHttp.getBinary httpClient "/reports/sites/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetReportsSitesById.OK contentBinary
        | HttpStatusCode.Forbidden -> GetReportsSitesById.Forbidden contentBinary
        | _ -> GetReportsSitesById.NotFound contentBinary

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

        let (status, contentBinary) =
            OpenApiHttp.getBinary httpClient "/reports/tags/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetReportsTagsById.OK contentBinary
        | HttpStatusCode.Forbidden -> GetReportsTagsById.Forbidden contentBinary
        | _ -> GetReportsTagsById.NotFound contentBinary

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

        let (status, content) =
            OpenApiHttp.get httpClient "/sites" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSites.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSites.Forbidden
        | _ -> GetSites.NotFound

    ///<summary>
    ///Create a site
    ///</summary>
    member this.CreateSite(body: PostSite, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> CreateSite.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> CreateSite.Created
        | HttpStatusCode.BadRequest -> CreateSite.BadRequest
        | HttpStatusCode.Forbidden -> CreateSite.Forbidden
        | _ -> CreateSite.NotFound

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetSitesMetadata(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/metadata" requestParts cancellationToken

        GetSitesMetadata.OK content

    ///<summary>
    ///Delete a specific Site
    ///</summary>
    ///<param name="id">ID of Site that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSitesById(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/sites/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> DeleteSitesById.OK content
        | HttpStatusCode.Forbidden -> DeleteSitesById.Forbidden
        | _ -> DeleteSitesById.NotFound

    ///<summary>
    ///Return a site based on ID
    ///</summary>
    ///<param name="id">ID that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetSiteById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              if fields.IsSome then
                  RequestPart.query ("fields", fields.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSiteById.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSiteById.Forbidden
        | _ -> GetSiteById.NotFound

    ///<summary>
    ///Update a site
    ///</summary>
    ///<param name="id">ID of the website that needs to be update</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PutSitesById(id: int64, body: PostSite, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/sites/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> PutSitesById.OK(Serializer.deserialize content)
        | HttpStatusCode.BadRequest -> PutSitesById.BadRequest
        | HttpStatusCode.Forbidden -> PutSitesById.Forbidden
        | _ -> PutSitesById.NotFound

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

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/audits" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSiteAudits.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSiteAudits.Forbidden
        | _ -> GetSiteAudits.NotFound

    ///<summary>
    ///Create an audit for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.CreateAudits(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/audits" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> CreateAudits.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> CreateAudits.Created
        | HttpStatusCode.BadRequest -> CreateAudits.BadRequest
        | HttpStatusCode.Forbidden -> CreateAudits.Forbidden
        | _ -> CreateAudits.NotFound

    ///<summary>
    ///Add the site to the backup queue
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.AddSiteToBackupQueue(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/backupnow" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> AddSiteToBackupQueue.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> AddSiteToBackupQueue.Forbidden
        | _ -> AddSiteToBackupQueue.NotFound

    ///<summary>
    ///Return backup profile
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.GetBackupProfiles(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/backupprofiles" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetBackupProfiles.OK
        | HttpStatusCode.Forbidden -> GetBackupProfiles.Forbidden
        | _ -> GetBackupProfiles.NotFound

    ///<summary>
    ///List of latest backups
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.GetListBackups(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/backups" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetListBackups.OK
        | HttpStatusCode.Forbidden -> GetListBackups.Forbidden
        | _ -> GetListBackups.NotFound

    ///<summary>
    ///Start a remote backup for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.StartSiteBackup(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/backupstart" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> StartSiteBackup.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> StartSiteBackup.Forbidden
        | _ -> StartSiteBackup.NotFound

    ///<summary>
    ///Step (continue) a remote backup for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.StepSiteBackup(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/backupstep" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> StepSiteBackup.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> StepSiteBackup.Forbidden
        | _ -> StepSiteBackup.NotFound

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

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/extensions" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSitesExtensionsById.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSitesExtensionsById.Forbidden
        | _ -> GetSitesExtensionsById.NotFound

    ///<summary>
    ///Install extension
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="url">URL to install the extension from</param>
    ///<param name="cancellationToken"></param>
    member this.InstallExtension(id: int64, url: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              RequestPart.query ("url", url) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/extensions" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> InstallExtension.OK
        | HttpStatusCode.Forbidden -> InstallExtension.Forbidden
        | _ -> InstallExtension.NotFound

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

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/logs" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSitesLogsById.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSitesLogsById.Forbidden
        | _ -> GetSitesLogsById.NotFound

    ///<summary>
    ///Create a custom log for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.CreateLog(id: int64, body: PostLog, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/logs" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> CreateLog.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> CreateLog.Created
        | HttpStatusCode.BadRequest -> CreateLog.BadRequest
        | HttpStatusCode.Forbidden -> CreateLog.Forbidden
        | _ -> CreateLog.NotFound

    ///<summary>
    ///Return boolean
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteMonitor(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/sites/{id}/monitor" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> DeleteMonitor.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> DeleteMonitor.Forbidden
        | _ -> DeleteMonitor.NotFound

    ///<summary>
    ///Return boolean
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.PostMonitor(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/monitor" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> PostMonitor.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> PostMonitor.Forbidden
        | _ -> PostMonitor.NotFound

    ///<summary>
    ///Scan the site for malware
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.Scanner(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/scanner" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> Scanner.OK content
        | HttpStatusCode.Forbidden -> Scanner.Forbidden
        | _ -> Scanner.NotFound

    ///<summary>
    ///SEO analyze for a page
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.SeoAnalyze(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/seo" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> SeoAnalyze.OK content
        | HttpStatusCode.Forbidden -> SeoAnalyze.Forbidden
        | _ -> SeoAnalyze.NotFound

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

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/tags" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSitesTagsById.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSitesTagsById.Forbidden
        | _ -> GetSitesTagsById.NotFound

    ///<summary>
    ///Add tags for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PostTags(id: int64, body: Tag, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/tags" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> PostTags.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> PostTags.Created
        | HttpStatusCode.Forbidden -> PostTags.Forbidden
        | _ -> PostTags.NotFound

    ///<summary>
    ///Update Joomla core on the remote site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.UpdateJoomla(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/sites/{id}/updatejoomla" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> UpdateJoomla.OK content
        | HttpStatusCode.Forbidden -> UpdateJoomla.Forbidden
        | _ -> UpdateJoomla.NotFound

    ///<summary>
    ///Return uptime data
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.GetUptime(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/uptime" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetUptime.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetUptime.Forbidden
        | _ -> GetUptime.NotFound

    ///<summary>
    ///validate the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.ValidateSite(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/validate" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> ValidateSite.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> ValidateSite.Forbidden
        | _ -> ValidateSite.NotFound

    ///<summary>
    ///validate the site, return the debug information
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="cancellationToken"></param>
    member this.ValidateDebugSite(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/sites/{id}/validatedebug" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> ValidateDebugSite.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> ValidateDebugSite.Forbidden
        | _ -> ValidateDebugSite.NotFound

    ///<summary>
    ///Returns a list of SSO Users
    ///</summary>
    member this.GetSsoUsers(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/ssousers" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSsoUsers.OK(Serializer.deserialize content)
        | _ -> GetSsoUsers.Forbidden

    ///<summary>
    ///Create a SSO User
    ///</summary>
    member this.CreateSsoUsers(body: SsoUsers, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/ssousers" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> CreateSsoUsers.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> CreateSsoUsers.Created
        | HttpStatusCode.BadRequest -> CreateSsoUsers.BadRequest
        | HttpStatusCode.Forbidden -> CreateSsoUsers.Forbidden
        | _ -> CreateSsoUsers.NotFound

    ///<summary>
    ///Delete a specific SSO User
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSsoUserById(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/ssousers/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> DeleteSsoUserById.OK content
        | HttpStatusCode.Forbidden -> DeleteSsoUserById.Forbidden
        | _ -> DeleteSsoUserById.NotFound

    ///<summary>
    ///Returns a SSO User based on ID
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetSsoUsersById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              if fields.IsSome then
                  RequestPart.query ("fields", fields.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/ssousers/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSsoUsersById.OK(Serializer.deserialize content)
        | HttpStatusCode.BadRequest -> GetSsoUsersById.BadRequest
        | _ -> GetSsoUsersById.Forbidden

    ///<summary>
    ///Update a SSO User
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be updated</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateSsoUsers(id: int64, body: SsoUsers, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/ssousers/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> UpdateSsoUsers.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> UpdateSsoUsers.Created
        | HttpStatusCode.BadRequest -> UpdateSsoUsers.BadRequest
        | HttpStatusCode.Forbidden -> UpdateSsoUsers.Forbidden
        | _ -> UpdateSsoUsers.NotFound

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

        let (status, content) =
            OpenApiHttp.get httpClient "/tags" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetTags.OK(Serializer.deserialize content)
        | _ -> GetTags.Forbidden

    ///<summary>
    ///Create a tag
    ///</summary>
    member this.CreateTags(body: Tag, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/tags" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> CreateTags.OK(Serializer.deserialize content)
        | HttpStatusCode.Created -> CreateTags.Created
        | HttpStatusCode.BadRequest -> CreateTags.BadRequest
        | HttpStatusCode.Forbidden -> CreateTags.Forbidden
        | _ -> CreateTags.NotFound

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetTagsMetadata(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/tags/metadata" requestParts cancellationToken

        GetTagsMetadata.OK content

    ///<summary>
    ///Delete a specific tag
    ///</summary>
    ///<param name="id">ID of tag that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteTagsById(id: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("id", id) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/tags/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> DeleteTagsById.OK content
        | HttpStatusCode.Forbidden -> DeleteTagsById.Forbidden
        | _ -> DeleteTagsById.NotFound

    ///<summary>
    ///Returns a tag based on ID
    ///</summary>
    ///<param name="id">ID of tag that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    ///<param name="cancellationToken"></param>
    member this.GetTagById(id: int64, ?fields: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              if fields.IsSome then
                  RequestPart.query ("fields", fields.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/tags/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetTagById.OK(Serializer.deserialize content)
        | HttpStatusCode.BadRequest -> GetTagById.BadRequest
        | _ -> GetTagById.Forbidden

    ///<summary>
    ///Update a tag
    ///</summary>
    ///<param name="id">ID of tag</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateTag(id: int64, body: Tag, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("id", id)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/tags/{id}" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> UpdateTag.OK(Serializer.deserialize content)
        | HttpStatusCode.BadRequest -> UpdateTag.BadRequest
        | HttpStatusCode.Forbidden -> UpdateTag.Forbidden
        | _ -> UpdateTag.NotFound

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

        let (status, content) =
            OpenApiHttp.get httpClient "/tags/{id}/sites" requestParts cancellationToken

        match status with
        | HttpStatusCode.OK -> GetSitesByTags.OK(Serializer.deserialize content)
        | HttpStatusCode.Forbidden -> GetSitesByTags.Forbidden
        | _ -> GetSitesByTags.NotFound
