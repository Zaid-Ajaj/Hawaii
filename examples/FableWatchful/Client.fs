namespace rec FableWatchful

open Browser.Types
open Fable.SimpleHttp
open FableWatchful.Types
open FableWatchful.Http

type FableWatchfulClient(url: string, headers: list<Header>) =
    new(url: string) = FableWatchfulClient(url, [])

    ///<summary>
    ///Returns a list of audits
    ///</summary>
    ///<param name="limit">Number of object to return (max 100, default 25)</param>
    ///<param name="limitstart">Start of the return (default 0)</param>
    ///<param name="order">ORDER by this field separete by comas. Add + / - after field for set ASC / DESC: type+,name-</param>
    member this.GetAudits(?limit: int64, ?limitstart: int64, ?order: string) =
        async {
            let requestParts =
                [ if limit.IsSome then
                      RequestPart.query ("limit", limit.Value)
                  if limitstart.IsSome then
                      RequestPart.query ("limitstart", limitstart.Value)
                  if order.IsSome then
                      RequestPart.query ("order", order.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/audits" headers requestParts

            if status = 200 then
                return GetAudits.OK(Serializer.deserialize content)
            else
                return GetAudits.Forbidden
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsAudits() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/audits/metadata" headers requestParts
            return GetFieldsAudits.OK content
        }

    ///<summary>
    ///Delete a specific audit
    ///</summary>
    ///<param name="id">ID of audit that needs to be deleted</param>
    member this.DeleteAuditById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/audits/{id}" headers requestParts

            if status = 200 then
                return DeleteAuditById.OK content
            else if status = 403 then
                return DeleteAuditById.Forbidden
            else
                return DeleteAuditById.NotFound
        }

    ///<summary>
    ///Returns a audit based on ID
    ///</summary>
    ///<param name="id">ID of audit that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    member this.GetAuditById(id: int64, ?fields: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/audits/{id}" headers requestParts

            if status = 200 then
                return GetAuditById.OK(Serializer.deserialize content)
            else if status = 400 then
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
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/extensions" headers requestParts

            if status = 200 then
                return GetExtensions.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetExtensions.Forbidden
            else
                return GetExtensions.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsExtensions() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/extensions/metadata" headers requestParts
            return GetFieldsExtensions.OK content
        }

    ///<summary>
    ///Set 'ignore updates' for a given extension / site_id
    ///</summary>
    ///<param name="id">ID of the extension</param>
    member this.IgnoreExtensionUpdate(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/extensions/{id}/ignore" headers requestParts

            if status = 200 then
                return IgnoreExtensionUpdate.OK content
            else
                return IgnoreExtensionUpdate.NotFound
        }

    ///<summary>
    ///Remove 'ignore updates' for a given extension
    ///</summary>
    ///<param name="id">ID of the extension</param>
    member this.UnignoreExtensionUpdate(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/extensions/{id}/unignore" headers requestParts

            if status = 200 then
                return UnignoreExtensionUpdate.OK content
            else
                return UnignoreExtensionUpdate.NotFound
        }

    ///<summary>
    ///Update the extension on the remote site
    ///</summary>
    ///<param name="id">ID of the extension</param>
    member this.UpdateExtension(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/extensions/{id}/update" headers requestParts

            if status = 200 then
                return UpdateExtension.OK content
            else
                return UpdateExtension.NotFound
        }

    ///<summary>
    ///Returns a list of feedbacks
    ///</summary>
    ///<param name="fields">Fields to return separate by comas (es. name,id)</param>
    member this.GetFeedbacks(?fields: string) =
        async {
            let requestParts =
                [ if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/feedbacks" headers requestParts

            if status = 200 then
                return GetFeedbacks.OK(Serializer.deserialize content)
            else
                return GetFeedbacks.Forbidden
        }

    ///<summary>
    ///Create a feedback
    ///</summary>
    member this.CreateFeedbacks(body: Feedback) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/feedbacks" headers requestParts

            if status = 200 then
                return CreateFeedbacks.OK(Serializer.deserialize content)
            else if status = 201 then
                return CreateFeedbacks.Created
            else if status = 400 then
                return CreateFeedbacks.BadRequest
            else if status = 403 then
                return CreateFeedbacks.Forbidden
            else
                return CreateFeedbacks.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsFeedbacks() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/feedbacks/metadata" headers requestParts
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
    member this.GetLogs
        (
            ?logType: string,
            ?logEntry: string,
            ?from: string,
            ?``to``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/logs" headers requestParts

            if status = 200 then
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
    member this.GetExportLogs
        (
            format: string,
            ?site: int64,
            ?filterType: string,
            ?search: string,
            ?startdate: string,
            ?enddate: string,
            ?limit: int64,
            ?startid: int64
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

            let! (status, content) = OpenApiHttp.getAsync url "/logs/export" headers requestParts

            if status = 200 then
                return GetExportLogs.OK
            else
                return GetExportLogs.Forbidden
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetFieldsLogs() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/logs/metadata" headers requestParts
            return GetFieldsLogs.OK content
        }

    ///<summary>
    ///Returns a list of log types
    ///</summary>
    member this.GetTypesLogs() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/logs/types" headers requestParts
            return GetTypesLogs.OK content
        }

    ///<summary>
    ///Delete a specific log
    ///</summary>
    ///<param name="id">ID of log that needs to be deleted</param>
    member this.DeleteLogById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/logs/{id}" headers requestParts

            if status = 200 then
                return DeleteLogById.OK content
            else if status = 403 then
                return DeleteLogById.Forbidden
            else
                return DeleteLogById.NotFound
        }

    member this.PostPackages() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/packages" headers requestParts
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
    member this.GetReportsSitesById
        (
            id: int64,
            ?from: string,
            ?``to``: string,
            ?reports: string,
            ?logType: string,
            ?compare: int
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

            let! (status, contentBinary) = OpenApiHttp.getBinaryAsync url "/reports/sites/{id}" headers requestParts

            if status = 200 then
                return GetReportsSitesById.OK contentBinary
            else if status = 403 then
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
    member this.GetReportsTagsById
        (
            id: int64,
            ?from: string,
            ?``to``: string,
            ?reports: string,
            ?logType: string,
            ?compare: int
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

            let! (status, contentBinary) = OpenApiHttp.getBinaryAsync url "/reports/tags/{id}" headers requestParts

            if status = 200 then
                return GetReportsTagsById.OK contentBinary
            else if status = 403 then
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
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/sites" headers requestParts

            if status = 200 then
                return GetSites.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSites.Forbidden
            else
                return GetSites.NotFound
        }

    ///<summary>
    ///Create a site
    ///</summary>
    member this.CreateSite(body: PostSite) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites" headers requestParts

            if status = 200 then
                return CreateSite.OK(Serializer.deserialize content)
            else if status = 201 then
                return CreateSite.Created
            else if status = 400 then
                return CreateSite.BadRequest
            else if status = 403 then
                return CreateSite.Forbidden
            else
                return CreateSite.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetSitesMetadata() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/sites/metadata" headers requestParts
            return GetSitesMetadata.OK content
        }

    ///<summary>
    ///Delete a specific Site
    ///</summary>
    ///<param name="id">ID of Site that needs to be deleted</param>
    member this.DeleteSitesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/sites/{id}" headers requestParts

            if status = 200 then
                return DeleteSitesById.OK content
            else if status = 403 then
                return DeleteSitesById.Forbidden
            else
                return DeleteSitesById.NotFound
        }

    ///<summary>
    ///Return a site based on ID
    ///</summary>
    ///<param name="id">ID that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    member this.GetSiteById(id: int64, ?fields: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}" headers requestParts

            if status = 200 then
                return GetSiteById.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSiteById.Forbidden
            else
                return GetSiteById.NotFound
        }

    ///<summary>
    ///Update a site
    ///</summary>
    ///<param name="id">ID of the website that needs to be update</param>
    ///<param name="body"></param>
    member this.PutSitesById(id: int64, body: PostSite) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync url "/sites/{id}" headers requestParts

            if status = 200 then
                return PutSitesById.OK(Serializer.deserialize content)
            else if status = 400 then
                return PutSitesById.BadRequest
            else if status = 403 then
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
    member this.GetSiteAudits(id: int64, ?fields: string, ?limit: int64, ?limitstart: int64, ?order: string) =
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

            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/audits" headers requestParts

            if status = 200 then
                return GetSiteAudits.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSiteAudits.Forbidden
            else
                return GetSiteAudits.NotFound
        }

    ///<summary>
    ///Create an audit for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.CreateAudits(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/audits" headers requestParts

            if status = 200 then
                return CreateAudits.OK(Serializer.deserialize content)
            else if status = 201 then
                return CreateAudits.Created
            else if status = 400 then
                return CreateAudits.BadRequest
            else if status = 403 then
                return CreateAudits.Forbidden
            else
                return CreateAudits.NotFound
        }

    ///<summary>
    ///Add the site to the backup queue
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.AddSiteToBackupQueue(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/backupnow" headers requestParts

            if status = 200 then
                return AddSiteToBackupQueue.OK(Serializer.deserialize content)
            else if status = 403 then
                return AddSiteToBackupQueue.Forbidden
            else
                return AddSiteToBackupQueue.NotFound
        }

    ///<summary>
    ///Return backup profile
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.GetBackupProfiles(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/backupprofiles" headers requestParts

            if status = 200 then
                return GetBackupProfiles.OK
            else if status = 403 then
                return GetBackupProfiles.Forbidden
            else
                return GetBackupProfiles.NotFound
        }

    ///<summary>
    ///List of latest backups
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.GetListBackups(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/backups" headers requestParts

            if status = 200 then
                return GetListBackups.OK
            else if status = 403 then
                return GetListBackups.Forbidden
            else
                return GetListBackups.NotFound
        }

    ///<summary>
    ///Start a remote backup for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.StartSiteBackup(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/backupstart" headers requestParts

            if status = 200 then
                return StartSiteBackup.OK(Serializer.deserialize content)
            else if status = 403 then
                return StartSiteBackup.Forbidden
            else
                return StartSiteBackup.NotFound
        }

    ///<summary>
    ///Step (continue) a remote backup for the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.StepSiteBackup(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/backupstep" headers requestParts

            if status = 200 then
                return StepSiteBackup.OK(Serializer.deserialize content)
            else if status = 403 then
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
    member this.GetSitesExtensionsById(id: int64, ?fields: string, ?limit: int64, ?limitstart: int64, ?order: string) =
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

            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/extensions" headers requestParts

            if status = 200 then
                return GetSitesExtensionsById.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSitesExtensionsById.Forbidden
            else
                return GetSitesExtensionsById.NotFound
        }

    ///<summary>
    ///Install extension
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="url">URL to install the extension from</param>
    member this.InstallExtension(id: int64, url: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.query ("url", url) ]

            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/extensions" headers requestParts

            if status = 200 then
                return InstallExtension.OK
            else if status = 403 then
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
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/logs" headers requestParts

            if status = 200 then
                return GetSitesLogsById.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSitesLogsById.Forbidden
            else
                return GetSitesLogsById.NotFound
        }

    ///<summary>
    ///Create a custom log for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="body"></param>
    member this.CreateLog(id: int64, body: PostLog) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/logs" headers requestParts

            if status = 200 then
                return CreateLog.OK(Serializer.deserialize content)
            else if status = 201 then
                return CreateLog.Created
            else if status = 400 then
                return CreateLog.BadRequest
            else if status = 403 then
                return CreateLog.Forbidden
            else
                return CreateLog.NotFound
        }

    ///<summary>
    ///Return boolean
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.DeleteMonitor(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/sites/{id}/monitor" headers requestParts

            if status = 200 then
                return DeleteMonitor.OK(Serializer.deserialize content)
            else if status = 403 then
                return DeleteMonitor.Forbidden
            else
                return DeleteMonitor.NotFound
        }

    ///<summary>
    ///Return boolean
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.PostMonitor(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/monitor" headers requestParts

            if status = 200 then
                return PostMonitor.OK(Serializer.deserialize content)
            else if status = 403 then
                return PostMonitor.Forbidden
            else
                return PostMonitor.NotFound
        }

    ///<summary>
    ///Scan the site for malware
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.Scanner(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/scanner" headers requestParts

            if status = 200 then
                return Scanner.OK content
            else if status = 403 then
                return Scanner.Forbidden
            else
                return Scanner.NotFound
        }

    ///<summary>
    ///SEO analyze for a page
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.SeoAnalyze(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/seo" headers requestParts

            if status = 200 then
                return SeoAnalyze.OK content
            else if status = 403 then
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
    member this.GetSitesTagsById
        (
            id: int64,
            ?name: string,
            ?``type``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/tags" headers requestParts

            if status = 200 then
                return GetSitesTagsById.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSitesTagsById.Forbidden
            else
                return GetSitesTagsById.NotFound
        }

    ///<summary>
    ///Add tags for a specific website
    ///</summary>
    ///<param name="id">ID of the website</param>
    ///<param name="body"></param>
    member this.PostTags(id: int64, body: Tag) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/tags" headers requestParts

            if status = 200 then
                return PostTags.OK(Serializer.deserialize content)
            else if status = 201 then
                return PostTags.Created
            else if status = 403 then
                return PostTags.Forbidden
            else
                return PostTags.NotFound
        }

    ///<summary>
    ///Update Joomla core on the remote site
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.UpdateJoomla(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.postAsync url "/sites/{id}/updatejoomla" headers requestParts

            if status = 200 then
                return UpdateJoomla.OK content
            else if status = 403 then
                return UpdateJoomla.Forbidden
            else
                return UpdateJoomla.NotFound
        }

    ///<summary>
    ///Return uptime data
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.GetUptime(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/uptime" headers requestParts

            if status = 200 then
                return GetUptime.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetUptime.Forbidden
            else
                return GetUptime.NotFound
        }

    ///<summary>
    ///validate the site
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.ValidateSite(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/validate" headers requestParts

            if status = 200 then
                return ValidateSite.OK(Serializer.deserialize content)
            else if status = 403 then
                return ValidateSite.Forbidden
            else
                return ValidateSite.NotFound
        }

    ///<summary>
    ///validate the site, return the debug information
    ///</summary>
    ///<param name="id">ID of the website</param>
    member this.ValidateDebugSite(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/sites/{id}/validatedebug" headers requestParts

            if status = 200 then
                return ValidateDebugSite.OK(Serializer.deserialize content)
            else if status = 403 then
                return ValidateDebugSite.Forbidden
            else
                return ValidateDebugSite.NotFound
        }

    ///<summary>
    ///Returns a list of SSO Users
    ///</summary>
    member this.GetSsoUsers() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/ssousers" headers requestParts

            if status = 200 then
                return GetSsoUsers.OK(Serializer.deserialize content)
            else
                return GetSsoUsers.Forbidden
        }

    ///<summary>
    ///Create a SSO User
    ///</summary>
    member this.CreateSsoUsers(body: SsoUsers) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/ssousers" headers requestParts

            if status = 200 then
                return CreateSsoUsers.OK(Serializer.deserialize content)
            else if status = 201 then
                return CreateSsoUsers.Created
            else if status = 400 then
                return CreateSsoUsers.BadRequest
            else if status = 403 then
                return CreateSsoUsers.Forbidden
            else
                return CreateSsoUsers.NotFound
        }

    ///<summary>
    ///Delete a specific SSO User
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be deleted</param>
    member this.DeleteSsoUserById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/ssousers/{id}" headers requestParts

            if status = 200 then
                return DeleteSsoUserById.OK content
            else if status = 403 then
                return DeleteSsoUserById.Forbidden
            else
                return DeleteSsoUserById.NotFound
        }

    ///<summary>
    ///Returns a SSO User based on ID
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    member this.GetSsoUsersById(id: int64, ?fields: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/ssousers/{id}" headers requestParts

            if status = 200 then
                return GetSsoUsersById.OK(Serializer.deserialize content)
            else if status = 400 then
                return GetSsoUsersById.BadRequest
            else
                return GetSsoUsersById.Forbidden
        }

    ///<summary>
    ///Update a SSO User
    ///</summary>
    ///<param name="id">ID of SSO User that needs to be updated</param>
    ///<param name="body"></param>
    member this.UpdateSsoUsers(id: int64, body: SsoUsers) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync url "/ssousers/{id}" headers requestParts

            if status = 200 then
                return UpdateSsoUsers.OK(Serializer.deserialize content)
            else if status = 201 then
                return UpdateSsoUsers.Created
            else if status = 400 then
                return UpdateSsoUsers.BadRequest
            else if status = 403 then
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
    member this.GetTags
        (
            ?name: string,
            ?``type``: string,
            ?fields: string,
            ?limit: int64,
            ?limitstart: int64,
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/tags" headers requestParts

            if status = 200 then
                return GetTags.OK(Serializer.deserialize content)
            else
                return GetTags.Forbidden
        }

    ///<summary>
    ///Create a tag
    ///</summary>
    member this.CreateTags(body: Tag) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/tags" headers requestParts

            if status = 200 then
                return CreateTags.OK(Serializer.deserialize content)
            else if status = 201 then
                return CreateTags.Created
            else if status = 400 then
                return CreateTags.BadRequest
            else if status = 403 then
                return CreateTags.Forbidden
            else
                return CreateTags.NotFound
        }

    ///<summary>
    ///Returns a list of fields
    ///</summary>
    member this.GetTagsMetadata() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/tags/metadata" headers requestParts
            return GetTagsMetadata.OK content
        }

    ///<summary>
    ///Delete a specific tag
    ///</summary>
    ///<param name="id">ID of tag that needs to be deleted</param>
    member this.DeleteTagsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/tags/{id}" headers requestParts

            if status = 200 then
                return DeleteTagsById.OK content
            else if status = 403 then
                return DeleteTagsById.Forbidden
            else
                return DeleteTagsById.NotFound
        }

    ///<summary>
    ///Returns a tag based on ID
    ///</summary>
    ///<param name="id">ID of tag that needs to be fetched</param>
    ///<param name="fields">Fields to return separate by comas: name,id</param>
    member this.GetTagById(id: int64, ?fields: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if fields.IsSome then
                      RequestPart.query ("fields", fields.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/tags/{id}" headers requestParts

            if status = 200 then
                return GetTagById.OK(Serializer.deserialize content)
            else if status = 400 then
                return GetTagById.BadRequest
            else
                return GetTagById.Forbidden
        }

    ///<summary>
    ///Update a tag
    ///</summary>
    ///<param name="id">ID of tag</param>
    ///<param name="body"></param>
    member this.UpdateTag(id: int64, body: Tag) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync url "/tags/{id}" headers requestParts

            if status = 200 then
                return UpdateTag.OK(Serializer.deserialize content)
            else if status = 400 then
                return UpdateTag.BadRequest
            else if status = 403 then
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
            ?order: string
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

            let! (status, content) = OpenApiHttp.getAsync url "/tags/{id}/sites" headers requestParts

            if status = 200 then
                return GetSitesByTags.OK(Serializer.deserialize content)
            else if status = 403 then
                return GetSitesByTags.Forbidden
            else
                return GetSitesByTags.NotFound
        }
