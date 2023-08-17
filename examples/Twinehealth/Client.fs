namespace rec Twinehealth

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open Twinehealth.Types
open Twinehealth.Http

///# Overview
///The Fitbit Plus API is a RESTful API. The requests and responses are formated according to the
///[JSON API](http://jsonapi.org/format/1.0/) specification.
///In addition to this documentation, we also provide an
///[OpenAPI](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) "yaml" file describing the API:
///[Fitbit Plus API Specification](swagger.yaml).
///# Authentication
///Authentication for the Fitbit Plus API is based on the
///[OAuth 2.0 Authorization Framework](https://tools.ietf.org/html/rfc6749). Fitbit Plus currently supports grant
///types of **client_credentials** and **refresh_token**.
///See [POST /oauth/token](#operation/createToken) for details on the request and response formats.
///&amp;lt;!-- ReDoc-Inject: &amp;lt;security-definitions&amp;gt; --&amp;gt;
///## Building Integrations
///We will provide customers with unique client credentials for each application/integration they build, allowing us
///to enforce appropriate access controls and monitor API usage.
///The client credentials will be scoped to the organization, and allow full access to all patients and related data
///within that organization.
///These credentials are appropriate for creating an integration that does one of the following:
/// - background reporting/analysis
/// - synchronizing data with another system (such as an EMR)
///The API credentials and oauth flows we currently support are **not** well suited for creating a user-facing
///application that allows a user (patient, coach, or admin) to login and have access to data which is appropriate to
///that specific user. It is possible to build such an application, but it is not possible to use Fitbit Plus as a
///federated identity provider. You would need to have a separate means of verifying a user's identity. We do not
///currently support the required password-based oauth flow to make this possible.
///# Paging
///The Fitbit Plus API supports two different pagination strategies for GET collection endpoints.
///#### Skip-based paging
///Skip-based paging uses the query parameters `page[size]` and `page[number]` to specify the max number of resources returned and the page number. We default to skip-based paging if there are no page parameters. The response will include a `links` object containing links to the first, last, prev, and next pages of data.
///If the contents of the collection change while you are iterating through the collection, you will see duplicate or missing documents. For example, if you are iterating through the `calender_event` resource via `GET /pub/calendar_event?sort=start_at&amp;page[size]=50&amp;page[number]=1`, and a new `calendar_event` is created that has a `start_at` value before the first `calendar_event`, when you fetch the next page at `GET /pub/calendar_event?sort=start_at&amp;page[size]=50&amp;page[number]=2`, the first entry in the second response will be a duplicate of the last entry in the first response.
///#### Cursor-based paging
///Cursor-based paging uses the query parameters `page[limit]` and `page[after]` to specify the max number of entries returned and identify where to begin the next page. Add `page[limit]` to the parameters to use cursor-based paging. The response will include a `links` object containing a link to the next page of data, if the next page exists.
///Cursor-based paging is not subject to duplication if new resources are added to the collection. For example, if you are iterating through the `calender_event` resource via `GET /pub/calendar_event?sort=start_at&amp;page[limit]=50`, and a new `calendar_event` is created that has a `start_at` value before the first `calendar_event`, you will not see a duplicate entry when you fetch the next page at `GET /pub/calendar_event?sort=start_at&amp;page[limit]=50&amp;page[after]=&amp;lt;cursor&amp;gt;`.
///We encourage the use of cursor-based paging for performance reasons.
///In either form of paging, you can determine whether any resources were missed by comparing the number of fetched resources against `meta.count`. Set `page[size]` or `page[limit]` to 0 to get only the count.
///It is not valid to mix the two strategies.
type TwinehealthClient(httpClient: HttpClient) =
    ///<summary>
    ///Create a plan action
    ///</summary>
    member this.CreateAction(body: CreateActionRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/action" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return CreateAction.Created(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateAction.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateAction.Forbidden(Serializer.deserialize content)
            | _ -> return CreateAction.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a health action from a patient's plan.
    ///</summary>
    ///<param name="id">Action identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchAction(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/action/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchAction.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchAction.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchAction.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Update a health action from a patient's plan.
    ///</summary>
    ///<param name="id">Action identifier</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateAction(id: string, body: UpdateActionRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync httpClient "/action/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return UpdateAction.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return UpdateAction.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return UpdateAction.Forbidden(Serializer.deserialize content)
            | _ -> return UpdateAction.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a bundle in a patient's plan
    ///</summary>
    member this.CreateBundle(body: CreateBundleRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/bundle" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreateBundle.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateBundle.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateBundle.Forbidden(Serializer.deserialize content)
            | _ -> return CreateBundle.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a bundle from a patient's plan.
    ///</summary>
    ///<param name="id">Bundle identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchBundle(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/bundle/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchBundle.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchBundle.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchBundle.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Updte a bundle from a patient's plan.
    ///</summary>
    ///<param name="id">Bundle identifier</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateBundle(id: string, body: UpdateBundleRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync httpClient "/bundle/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return UpdateBundle.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return UpdateBundle.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return UpdateBundle.Forbidden(Serializer.deserialize content)
            | _ -> return UpdateBundle.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of calendar events
    ///</summary>
    ///<param name="filterPatient">Patient id to fetch calendar event. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, `filter[organization]`, or `filter[attendees]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, `filter[organization]`, or `filter[attendees]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, `filter[organization]`, or `filter[attendees]`.</param>
    ///<param name="filterAttendees">Comma-separated list of coach or patient ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, `filter[organization]`, or `filter[attendees]`.</param>
    ///<param name="filterType">Calendar event type</param>
    ///<param name="filterCompleted">If not specified, return all calendar events. If set to `true` return only events marked as completed, if set to `false`, return only events not marked as completed yet.</param>
    ///<param name="filterStartAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for events starting in November 2017 (America/New_York): `filter[start_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="filterEndAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for events ending in November 2017 (America/New_York): `filter[end_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="filterCompletedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for events completed in November 2017 (America/New_York): `filter[completed_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="filterCreatedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for events created in November 2017 (America/New_York): `filter[created_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="filterUpdatedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for events updated in November 2017 (America/New_York): `filter[updated_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="pageNumber">Page number</param>
    ///<param name="pageSize">Page size</param>
    ///<param name="pageLimit">Page limit</param>
    ///<param name="pageCursor">Page cursor</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchCalendarEvents
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?filterAttendees: string,
            ?filterType: string,
            ?filterCompleted: bool,
            ?filterStartAt: string,
            ?filterEndAt: string,
            ?filterCompletedAt: string,
            ?filterCreatedAt: string,
            ?filterUpdatedAt: string,
            ?pageNumber: int,
            ?pageSize: int,
            ?pageLimit: int,
            ?pageCursor: string,
            ?``include``: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if filterAttendees.IsSome then
                      RequestPart.query ("filter[attendees]", filterAttendees.Value)
                  if filterType.IsSome then
                      RequestPart.query ("filter[type]", filterType.Value)
                  if filterCompleted.IsSome then
                      RequestPart.query ("filter[completed]", filterCompleted.Value)
                  if filterStartAt.IsSome then
                      RequestPart.query ("filter[start_at]", filterStartAt.Value)
                  if filterEndAt.IsSome then
                      RequestPart.query ("filter[end_at]", filterEndAt.Value)
                  if filterCompletedAt.IsSome then
                      RequestPart.query ("filter[completed_at]", filterCompletedAt.Value)
                  if filterCreatedAt.IsSome then
                      RequestPart.query ("filter[created_at]", filterCreatedAt.Value)
                  if filterUpdatedAt.IsSome then
                      RequestPart.query ("filter[updated_at]", filterUpdatedAt.Value)
                  if pageNumber.IsSome then
                      RequestPart.query ("page[number]", pageNumber.Value)
                  if pageSize.IsSome then
                      RequestPart.query ("page[size]", pageSize.Value)
                  if pageLimit.IsSome then
                      RequestPart.query ("page[limit]", pageLimit.Value)
                  if pageCursor.IsSome then
                      RequestPart.query ("page[cursor]", pageCursor.Value)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/calendar_event" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchCalendarEvents.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchCalendarEvents.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchCalendarEvents.Forbidden(Serializer.deserialize content)
            | _ -> return FetchCalendarEvents.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a calendar event for a patient. Attribute `all_day` must be set to `true` and `end_at` cannot be set for `plan-check-in` event type.
    ///</summary>
    member this.CreateCalendarEvent(body: CreateCalendarEventRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/calendar_event" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return CreateCalendarEvent.Created(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateCalendarEvent.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateCalendarEvent.Forbidden(Serializer.deserialize content)
            | _ -> return CreateCalendarEvent.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete a calendar event by id
    ///</summary>
    ///<param name="id">Calendar event identifier</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteCalendarEvent(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/calendar_event/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return DeleteCalendarEvent.OK
            | HttpStatusCode.Unauthorized -> return DeleteCalendarEvent.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteCalendarEvent.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a calendar event by id
    ///</summary>
    ///<param name="id">Calendar event identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchCalendarEvent(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/calendar_event/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchCalendarEvent.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchCalendarEvent.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchCalendarEvent.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Update a calendar event for a patient. Attribute `all_day` must be true and `end_at` cannot be specified for `plan-check-in` event type. To mark a calendar event as 'completed', set `completed_at` and `completed_by` to desired values.  To mark a completed calendar event as 'not completed', set `completed_at` and `completed_by` to `null`. Attendees can be added or removed, but response status cannot be updated. Use the calendar event response api for response status updates instead.
    ///</summary>
    ///<param name="id">Calendar event identifier</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateCalendarEvent
        (
            id: string,
            body: UpdateCalendarEventRequest,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync httpClient "/calendar_event/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return UpdateCalendarEvent.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return UpdateCalendarEvent.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return UpdateCalendarEvent.Forbidden(Serializer.deserialize content)
            | _ -> return UpdateCalendarEvent.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a calendar event response for an attendee of a calendar event, the attendee can be a coach or patient.  Calendar event responses cannot be fetched, updated nor deleted.  Use calendar event api to fetch the response status for attendees.
    ///</summary>
    member this.CreateCalendarEventResponse
        (
            body: CreateCalendarEventResponseRequest,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/calendar_event_response" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return PostCreateCalendarEventResponse.Created(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return PostCreateCalendarEventResponse.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden ->
                return PostCreateCalendarEventResponse.Forbidden(Serializer.deserialize content)
            | _ -> return PostCreateCalendarEventResponse.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of coaches matching the specified filters.
    ///</summary>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[groups]`, `filter[organization]`.</param>
    ///<param name="cancellationToken"></param>
    member this.FetchCoaches
        (
            ?filterGroups: string,
            ?filterOrganization: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/coach" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchCoaches.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchCoaches.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchCoaches.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a coach record by id.
    ///</summary>
    ///<param name="id">Coach identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchCoach(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/coach/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchCoach.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchCoach.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchCoach.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of email histories
    ///</summary>
    ///<param name="filterReceiver">Fitbit Plus user id of email recipient. Required if filter[sender] is not defined.</param>
    ///<param name="filterSender">Fitbit Plus user id of email sender. Required if filter[receiver] is not defined.</param>
    ///<param name="filterEmailType">Type of email</param>
    ///<param name="sort">
    ///valid sorts:
    ///  * send_time - ascending by send_time
    ///  * -send_time - descending by send_time
    ///</param>
    ///<param name="cancellationToken"></param>
    member this.FetchEmailHistories
        (
            ?filterReceiver: string,
            ?filterSender: string,
            ?filterEmailType: string,
            ?sort: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterReceiver.IsSome then
                      RequestPart.query ("filter[receiver]", filterReceiver.Value)
                  if filterSender.IsSome then
                      RequestPart.query ("filter[sender]", filterSender.Value)
                  if filterEmailType.IsSome then
                      RequestPart.query ("filter[emailType]", filterEmailType.Value)
                  if sort.IsSome then
                      RequestPart.query ("sort", sort.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/email_history" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchEmailHistories.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchEmailHistories.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchEmailHistories.Forbidden(Serializer.deserialize content)
            | _ -> return FetchEmailHistories.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get an email history by id
    ///</summary>
    ///<param name="id">Email history identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchEmailHistory(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/email_history/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchEmailHistory.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchEmailHistory.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchEmailHistory.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of groups matching the specified filters.
    ///</summary>
    ///<param name="filterOrganization">Organization identifier</param>
    ///<param name="filterName">Group name</param>
    ///<param name="cancellationToken"></param>
    member this.FetchGroups(filterOrganization: string, ?filterName: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.query ("filter[organization]", filterOrganization)
                  if filterName.IsSome then
                      RequestPart.query ("filter[name]", filterName.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/group" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchGroups.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchGroups.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchGroups.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a group record.
    ///</summary>
    member this.CreateGroup(body: CreateGroupRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/group" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return CreateGroup.Created(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateGroup.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateGroup.Forbidden(Serializer.deserialize content)
            | _ -> return CreateGroup.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a group record by id.
    ///</summary>
    ///<param name="id">Group identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchGroup(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/group/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchGroup.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchGroup.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchGroup.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of health profiles
    ///</summary>
    ///<param name="filterPatient">Patient id to fetch health profile. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="pageNumber">Page number</param>
    ///<param name="pageSize">Page size</param>
    ///<param name="pageLimit">Page limit</param>
    ///<param name="pageCursor">Page cursor</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthProfiles
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?pageNumber: int,
            ?pageSize: int,
            ?pageLimit: int,
            ?pageCursor: string,
            ?``include``: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if pageNumber.IsSome then
                      RequestPart.query ("page[number]", pageNumber.Value)
                  if pageSize.IsSome then
                      RequestPart.query ("page[size]", pageSize.Value)
                  if pageLimit.IsSome then
                      RequestPart.query ("page[limit]", pageLimit.Value)
                  if pageCursor.IsSome then
                      RequestPart.query ("page[cursor]", pageCursor.Value)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/health_profile" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthProfiles.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchHealthProfiles.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchHealthProfiles.Forbidden(Serializer.deserialize content)
            | _ -> return FetchHealthProfiles.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a health profile by id
    ///</summary>
    ///<param name="id">Health profile identifier</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthProfile(id: string, ?``include``: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_profile/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthProfile.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchHealthProfile.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchHealthProfile.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of health profile answers
    ///</summary>
    ///<param name="filterPatient">Patient id to fetch healt profile answers. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="pageNumber">Page number</param>
    ///<param name="pageSize">Page size</param>
    ///<param name="pageLimit">Page limit</param>
    ///<param name="pageCursor">Page cursor</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthProfileAnswers
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?pageNumber: int,
            ?pageSize: int,
            ?pageLimit: int,
            ?pageCursor: string,
            ?``include``: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if pageNumber.IsSome then
                      RequestPart.query ("page[number]", pageNumber.Value)
                  if pageSize.IsSome then
                      RequestPart.query ("page[size]", pageSize.Value)
                  if pageLimit.IsSome then
                      RequestPart.query ("page[limit]", pageLimit.Value)
                  if pageCursor.IsSome then
                      RequestPart.query ("page[cursor]", pageCursor.Value)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_profile_answer" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthProfileAnswers.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchHealthProfileAnswers.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchHealthProfileAnswers.Forbidden(Serializer.deserialize content)
            | _ -> return FetchHealthProfileAnswers.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a health profile answer by id
    ///</summary>
    ///<param name="id">Health profile answer identifier</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthProfileAnswer(id: string, ?``include``: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_profile_answer/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthProfileAnswer.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchHealthProfileAnswer.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchHealthProfileAnswer.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of health profile questions
    ///</summary>
    ///<param name="filterPatient">Patient id to fetch healt profile questions. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[group]`, or `filter[organization]`.</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthProfileQuestions
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?``include``: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_profile_question" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthProfileQuestions.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchHealthProfileQuestions.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchHealthProfileQuestions.Forbidden(Serializer.deserialize content)
            | _ -> return FetchHealthProfileQuestions.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a health profile by id
    ///</summary>
    ///<param name="id">Health profile question identifier</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthProfileQuestion(id: string, ?``include``: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_profile_question/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthProfileQuestion.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchHealthProfileQuestion.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchHealthProfileQuestion.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of all health question definitions
    ///</summary>
    member this.FetchHealthQuestionDefinitions(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_question_definition" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthQuestionDefinitions.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchHealthQuestionDefinitions.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden ->
                return FetchHealthQuestionDefinitions.Forbidden(Serializer.deserialize content)
            | _ -> return FetchHealthQuestionDefinitions.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a health question definition by id
    ///</summary>
    ///<param name="id">Health question definition identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchHealthQuestionDefinition(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/health_question_definition/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchHealthQuestionDefinition.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchHealthQuestionDefinition.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchHealthQuestionDefinition.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create an OAuth 2.0 Bearer token. A valid bearer token is required for all other API requests.
    ///Be sure to set the header `Content-Type: "application/vnd.api+json"`. Otherwise, you will get an error
    ///403 Forbidden. Using `Content-Type: "application/json"` is permitted (to support older oauth clients) but when
    ///using `application/json` the body should have a body in the following format instead of nesting under
    ///`data.attributes`:
    ///```
    ///{
    ///  "grant_type": "client_credentials",
    ///  "client_id": "95c78ab2-167f-40b8-8bec-8398d4b87454",
    ///  "client_secret": "35d18dc9-a3dd-4948-b787-063a490b9354"
    ///}
    ///```
    ///</summary>
    ///<param name="body"></param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.CreateToken(body: CreateTokenRequest, ?``include``: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.jsonContent body
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/oauth/token" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return CreateToken.Created(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateToken.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateToken.Forbidden(Serializer.deserialize content)
            | _ -> return CreateToken.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the list of groups a token can be used to access.
    ///</summary>
    ///<param name="id">Token identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchTokenGroups(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/oauth/token/{id}/groups" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchTokenGroups.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchTokenGroups.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchTokenGroups.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the organization a token can be used to access.
    ///</summary>
    ///<param name="id">Token identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchTokenOrganization(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/oauth/token/{id}/organization" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchTokenOrganization.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchTokenOrganization.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchTokenOrganization.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get an organization record by id.
    ///</summary>
    ///<param name="id">Organization identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchOrganization(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/organization/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchOrganization.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchOrganization.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchOrganization.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of patients.
    ///</summary>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that either `filter[group]` or `filter[organization]` must be specified.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that either `filter[group]` or `filter[organization]` must be specified.</param>
    ///<param name="filteridentifiersystem">Identifier system (example: "MyEHR") - requires a "filter[identifier][value]" parameter</param>
    ///<param name="filteridentifiervalue">Identifier value (example: "12345") - requires a "filter[identifier][system]" parameter</param>
    ///<param name="filterArchived">If not specified, return all patients. If set to 'true' return only archived patients, if set to 'false', return only patients who are not archived.</param>
    ///<param name="filterCreatedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for patients created in November 2017 (America/New_York): `filter[created_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="filterUpdatedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for patients updated in November 2017 (America/New_York): `filter[updated_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="pageNumber">Page number</param>
    ///<param name="pageSize">Page size</param>
    ///<param name="pageLimit">Page limit</param>
    ///<param name="pageCursor">Page cursor</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatients
        (
            ?filterGroups: string,
            ?filterOrganization: string,
            ?filteridentifiersystem: string,
            ?filteridentifiervalue: string,
            ?filterArchived: bool,
            ?filterCreatedAt: string,
            ?filterUpdatedAt: string,
            ?pageNumber: int,
            ?pageSize: int,
            ?pageLimit: int,
            ?pageCursor: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if filteridentifiersystem.IsSome then
                      RequestPart.query ("filter[identifier][system]", filteridentifiersystem.Value)
                  if filteridentifiervalue.IsSome then
                      RequestPart.query ("filter[identifier][value]", filteridentifiervalue.Value)
                  if filterArchived.IsSome then
                      RequestPart.query ("filter[archived]", filterArchived.Value)
                  if filterCreatedAt.IsSome then
                      RequestPart.query ("filter[created_at]", filterCreatedAt.Value)
                  if filterUpdatedAt.IsSome then
                      RequestPart.query ("filter[updated_at]", filterUpdatedAt.Value)
                  if pageNumber.IsSome then
                      RequestPart.query ("page[number]", pageNumber.Value)
                  if pageSize.IsSome then
                      RequestPart.query ("page[size]", pageSize.Value)
                  if pageLimit.IsSome then
                      RequestPart.query ("page[limit]", pageLimit.Value)
                  if pageCursor.IsSome then
                      RequestPart.query ("page[cursor]", pageCursor.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/patient" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatients.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchPatients.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchPatients.Forbidden(Serializer.deserialize content)
            | _ -> return FetchPatients.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a patient record.
    ///Example for creating a patient with a group specified using `meta.query` instead of `id`:
    ///```JSON
    ///{
    ///  "data": {
    ///    "type": "patient",
    ///    "attributes": {
    ///      "first_name": "Andrew",
    ///      "last_name": "Smith"
    ///    },
    ///    "relationships": {
    ///      "groups": {
    ///        "data": [
    ///          {
    ///            "type": "group",
    ///            "meta": {
    ///              "query": {
    ///                "organization": "58c88de7c93eb96357a87033",
    ///                "name": "Patients Lead"
    ///              }
    ///            }
    ///          }
    ///        ]
    ///      }
    ///    }
    ///  }
    ///}
    ///```
    ///</summary>
    member this.CreatePatient(body: CreatePatientRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/patient" requestParts cancellationToken

            match status with
            | HttpStatusCode.Created -> return CreatePatient.Created(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreatePatient.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreatePatient.Forbidden(Serializer.deserialize content)
            | _ -> return CreatePatient.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a new patient or update an existing patient
    ///</summary>
    member this.UpsertPatient(body: UpsertPatientRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync httpClient "/patient" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return UpsertPatient.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return UpsertPatient.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return UpsertPatient.Forbidden(Serializer.deserialize content)
            | _ -> return UpsertPatient.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Gets a patient record by id.
    ///</summary>
    ///<param name="id">Patient identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatient(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/patient/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatient.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchPatient.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatient.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Update a patient record.
    ///</summary>
    ///<param name="id">Patient identifier</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdatePatient(id: string, body: UpdatePatientRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.patchAsync httpClient "/patient/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return UpdatePatient.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return UpdatePatient.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return UpdatePatient.Forbidden(Serializer.deserialize content)
            | _ -> return UpdatePatient.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the list of coaches for a patient.
    ///</summary>
    ///<param name="id">Patient identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientCoaches(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/patient/{id}/coaches" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientCoaches.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchPatientCoaches.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatientCoaches.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the list of groups for a patient.
    ///</summary>
    ///<param name="id">Patient identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientGroups(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/patient/{id}/groups" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientGroups.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchPatientGroups.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatientGroups.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of patient health metrics.
    ///</summary>
    ///<param name="filterPatient">Filter the patient health metrics for a specified patient. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="pageNumber">Page number</param>
    ///<param name="pageSize">Page size</param>
    ///<param name="pageLimit">Page limit</param>
    ///<param name="pageCursor">Page cursor</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientHealthMetrics
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?pageNumber: int,
            ?pageSize: int,
            ?pageLimit: int,
            ?pageCursor: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if pageNumber.IsSome then
                      RequestPart.query ("page[number]", pageNumber.Value)
                  if pageSize.IsSome then
                      RequestPart.query ("page[size]", pageSize.Value)
                  if pageLimit.IsSome then
                      RequestPart.query ("page[limit]", pageLimit.Value)
                  if pageCursor.IsSome then
                      RequestPart.query ("page[cursor]", pageCursor.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/patient_health_metric" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientHealthMetrics.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchPatientHealthMetrics.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchPatientHealthMetrics.Forbidden(Serializer.deserialize content)
            | _ -> return FetchPatientHealthMetrics.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Create one or more patient health metrics.
    ///Example for creating a patient health result with a patient specified using `meta.query` instead of `id`:
    ///```JSON
    ///  {
    ///    "data": {
    ///      "type": "patient_health_metric",
    ///       "attributes": {
    ///         "code": {
    ///           "system": "LOINC",
    ///           "value": "13457-7"
    ///         },
    ///         "type": "ldl_cholesterol",
    ///         "occurred_at": "2017-03-14T11:00:57.000Z",
    ///         "value": 121,
    ///         "unit": "mg/dl"
    ///      },
    ///      "relationships": {
    ///        "patient": {
    ///          "data": {
    ///            "type": "patient",
    ///            "meta": {
    ///              "query": {
    ///                "identifier": {
    ///                  "system": "medical-record-number",
    ///                  "value": "121212"
    ///                },
    ///                "organization": "58c4554710123c5c40dbab81"
    ///              }
    ///            }
    ///          }
    ///        }
    ///      }
    ///    }
    ///  }
    ///```
    ///</summary>
    member this.CreatePatientHealthMetric
        (
            body: CreatePatientHealthMetricRequest,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/patient_health_metric" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreatePatientHealthMetric.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return CreatePatientHealthMetric.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreatePatientHealthMetric.Forbidden(Serializer.deserialize content)
            | _ -> return CreatePatientHealthMetric.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the plan summary for a patient.
    ///</summary>
    ///<param name="id">Patient health metric identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientHealthMetric(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/patient_health_metric/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientHealthMetric.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchPatientHealthMetric.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatientHealthMetric.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of patient plan summaries
    ///</summary>
    ///<param name="filterPatient">Patient id to fetch plan summary for. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientPlanSummaries
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?``include``: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/patient_plan_summary" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientPlanSummaries.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchPatientPlanSummaries.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatientPlanSummaries.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the plan summary for a patient.
    ///</summary>
    ///<param name="id">Plan summary identifier</param>
    ///<param name="include">List of related resources to include in the response</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientPlanSummary(id: string, ?``include``: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if ``include``.IsSome then
                      RequestPart.query ("include", ``include``.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/patient_plan_summary/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientPlanSummary.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchPatientPlanSummary.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatientPlanSummary.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Update a plan summary record for a patient.
    ///</summary>
    ///<param name="id">Plan summary identifier</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdatePatientPlanSummary
        (
            id: string,
            body: UpdatePatientPlanSummaryRequest,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.patchAsync httpClient "/patient_plan_summary/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return UpdatePatientPlanSummary.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return UpdatePatientPlanSummary.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return UpdatePatientPlanSummary.Forbidden(Serializer.deserialize content)
            | _ -> return UpdatePatientPlanSummary.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of patient health results.
    ///</summary>
    ///<param name="filterPatient">Filter the patient health results for a specified patient</param>
    ///<param name="filterActions">A comma-separated list of action identifiers</param>
    ///<param name="filterStartAt">Filter results that occurred after the passed ISO date and time string</param>
    ///<param name="filterEndAt">Filter results that occurred before the passed ISO date and time string</param>
    ///<param name="filterThreads">A comma-separated list of thread identifiers</param>
    ///<param name="filterCreatedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for results created in November 2017 (America/New_York): `filter[created_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="filterUpdatedAt">The start (inclusive) and end (exclusive) dates are ISO date and time strings separated by `..`. Example for results updated in November 2017 (America/New_York): `filter[updated_at]=2017-11-01T00:00:00-04:00..2017-12-01T00:00:00-05:00`</param>
    ///<param name="pageNumber">Page number</param>
    ///<param name="pageSize">Page size</param>
    ///<param name="pageLimit">Page limit</param>
    ///<param name="pageAfter">Page cursor</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientHealthResults
        (
            filterPatient: string,
            ?filterActions: string,
            ?filterStartAt: string,
            ?filterEndAt: string,
            ?filterThreads: string,
            ?filterCreatedAt: string,
            ?filterUpdatedAt: string,
            ?pageNumber: int,
            ?pageSize: int,
            ?pageLimit: int,
            ?pageAfter: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.query ("filter[patient]", filterPatient)
                  if filterActions.IsSome then
                      RequestPart.query ("filter[actions]", filterActions.Value)
                  if filterStartAt.IsSome then
                      RequestPart.query ("filter[start_at]", filterStartAt.Value)
                  if filterEndAt.IsSome then
                      RequestPart.query ("filter[end_at]", filterEndAt.Value)
                  if filterThreads.IsSome then
                      RequestPart.query ("filter[threads]", filterThreads.Value)
                  if filterCreatedAt.IsSome then
                      RequestPart.query ("filter[created_at]", filterCreatedAt.Value)
                  if filterUpdatedAt.IsSome then
                      RequestPart.query ("filter[updated_at]", filterUpdatedAt.Value)
                  if pageNumber.IsSome then
                      RequestPart.query ("page[number]", pageNumber.Value)
                  if pageSize.IsSome then
                      RequestPart.query ("page[size]", pageSize.Value)
                  if pageLimit.IsSome then
                      RequestPart.query ("page[limit]", pageLimit.Value)
                  if pageAfter.IsSome then
                      RequestPart.query ("page[after]", pageAfter.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/result" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientHealthResults.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchPatientHealthResults.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return FetchPatientHealthResults.Forbidden(Serializer.deserialize content)
            | _ -> return FetchPatientHealthResults.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get patient health result by id.
    ///</summary>
    ///<param name="id">Patient health result identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchPatientHealthResult(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/result/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchPatientHealthResult.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchPatientHealthResult.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchPatientHealthResult.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of rewards matching the specified filters.
    ///</summary>
    ///<param name="filterPatient">Patient identifier. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterRewardProgramActivation">Reward program activation identifier</param>
    ///<param name="filterThread">Thread identifier</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewards
        (
            ?filterPatient: string,
            ?filterRewardProgramActivation: string,
            ?filterThread: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterRewardProgramActivation.IsSome then
                      RequestPart.query ("filter[reward_program_activation]", filterRewardProgramActivation.Value)
                  if filterThread.IsSome then
                      RequestPart.query ("filter[thread]", filterThread.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/reward" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewards.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchRewards.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewards.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a reward for a patient.
    ///</summary>
    member this.CreateReward(body: CreateRewardRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/reward" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreateReward.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateReward.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateReward.Forbidden(Serializer.deserialize content)
            | _ -> return CreateReward.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a reward record by id.
    ///</summary>
    ///<param name="id">Reward identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchReward(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/reward/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchReward.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchReward.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchReward.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of reward earnings matching the specified filters.
    ///</summary>
    ///<param name="filterGroups">Group identifiers</param>
    ///<param name="filterPatient">Patient identifier</param>
    ///<param name="filterReadyForFulfillment">If true, only returns those reward earnings for which ready_for_fulfillment is true and fulfilled_at is null. If false, only returns those reward earnings for which ready_for_fulfillment is false and fulfilled_at is null.</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardEarnings
        (
            filterGroups: string,
            filterPatient: string,
            ?filterReadyForFulfillment: bool,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ RequestPart.query ("filter[groups]", filterGroups)
                  RequestPart.query ("filter[patient]", filterPatient)
                  if filterReadyForFulfillment.IsSome then
                      RequestPart.query ("filter[ready_for_fulfillment]", filterReadyForFulfillment.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/reward_earning" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardEarnings.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchRewardEarnings.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardEarnings.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a reward earning for a reward. There can only be one earning for a reward. It is possilble to create multiple reward earnings simultaneously by providing and array of reward earnings in the data property.
    ///</summary>
    member this.CreateRewardEarning(body: CreateRewardEarningRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/reward_earning" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreateRewardEarning.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateRewardEarning.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateRewardEarning.Forbidden(Serializer.deserialize content)
            | _ -> return CreateRewardEarning.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a reward earning record by id.
    ///</summary>
    ///<param name="id">Reward earning identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardEarning(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_earning/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardEarning.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchRewardEarning.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardEarning.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of reward earning fulfillments matching the specified filters.
    ///</summary>
    ///<param name="filterPatient">Patient identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardEarningFulfillments(filterPatient: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.query ("filter[patient]", filterPatient) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_earning_fulfillment" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardEarningFulfillments.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchRewardEarningFulfillments.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardEarningFulfillments.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a reward earning fulfillment for a reward earning. There can only be one fulfillment for each earning.
    ///</summary>
    member this.CreateRewardEarningFulfillment
        (
            body: CreateRewardEarningFulfillmentRequest,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/reward_earning_fulfillment" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreateRewardEarningFulfillment.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return CreateRewardEarningFulfillment.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden ->
                return CreateRewardEarningFulfillment.Forbidden(Serializer.deserialize content)
            | _ -> return CreateRewardEarningFulfillment.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a reward earning fulfillment record by id.
    ///</summary>
    ///<param name="id">Reward earning fulfillment identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardEarningFulfillment(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_earning_fulfillment/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardEarningFulfillment.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchRewardEarningFulfillment.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardEarningFulfillment.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of reward programs matching the specified filters.
    ///</summary>
    ///<param name="filterGroups">Comma-separated list of group identifiers. Note that one of the following filters must be specified: `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[groups]`, `filter[organization]`.</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardPrograms
        (
            ?filterGroups: string,
            ?filterOrganization: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/reward_program" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardPrograms.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchRewardPrograms.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardPrograms.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a reward program for a group.
    ///</summary>
    member this.CreateRewardProgram(body: CreateRewardProgramRequest, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/reward_program" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreateRewardProgram.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return CreateRewardProgram.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateRewardProgram.Forbidden(Serializer.deserialize content)
            | _ -> return CreateRewardProgram.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a reward program record by id.
    ///</summary>
    ///<param name="id">Reward program identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardProgram(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_program/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardProgram.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchRewardProgram.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardProgram.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the group related to a reward program.
    ///</summary>
    ///<param name="id">Reward program identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardProgramGroup(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_program/{id}/group" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardProgramGroup.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized -> return FetchRewardProgramGroup.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardProgramGroup.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of reward program activations matching the specified filters.
    ///</summary>
    ///<param name="filterPatient">Patient identifier. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterGroups">Comma-separated list of group ids. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="filterOrganization">Fitbit Plus organization id. Note that one of the following filters must be specified: `filter[patient]`, `filter[groups]`, `filter[organization]`.</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardProgramActivations
        (
            ?filterPatient: string,
            ?filterGroups: string,
            ?filterOrganization: string,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts =
                [ if filterPatient.IsSome then
                      RequestPart.query ("filter[patient]", filterPatient.Value)
                  if filterGroups.IsSome then
                      RequestPart.query ("filter[groups]", filterGroups.Value)
                  if filterOrganization.IsSome then
                      RequestPart.query ("filter[organization]", filterOrganization.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_program_activation" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardProgramActivations.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchRewardProgramActivations.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardProgramActivations.Forbidden(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a reward program activation for a patient. There can only be one activation for a patient for a given reward program.
    ///</summary>
    member this.CreateRewardProgramActivation
        (
            body: CreateRewardProgramActivationRequest,
            ?cancellationToken: CancellationToken
        ) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/reward_program_activation" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return CreateRewardProgramActivation.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return CreateRewardProgramActivation.Unauthorized(Serializer.deserialize content)
            | HttpStatusCode.Forbidden -> return CreateRewardProgramActivation.Forbidden(Serializer.deserialize content)
            | _ -> return CreateRewardProgramActivation.Conflict(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a reward program activationrecord by id.
    ///</summary>
    ///<param name="id">Reward program activation identifier</param>
    ///<param name="cancellationToken"></param>
    member this.FetchRewardProgramActivation(id: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/reward_program_activation/{id}" requestParts cancellationToken

            match status with
            | HttpStatusCode.OK -> return FetchRewardProgramActivation.OK(Serializer.deserialize content)
            | HttpStatusCode.Unauthorized ->
                return FetchRewardProgramActivation.Unauthorized(Serializer.deserialize content)
            | _ -> return FetchRewardProgramActivation.Forbidden(Serializer.deserialize content)
        }
