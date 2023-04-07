namespace rec SyncPodiosuite

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open SyncPodiosuite.Types
open SyncPodiosuite.Http

type SyncPodiosuiteClient(httpClient: HttpClient) =
    ///<summary>
    ///Takes username and password and if it finds match in back-end it returns an object with the user data and token used to authorize the login.
    ///</summary>
    member this.PostAuthToken(payload: PostAuthTokenPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/auth/token" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAuthToken.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAuthToken.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAuthToken.Unauthorized(Serializer.deserialize content)
        else
            PostAuthToken.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Takes username or email as values and sends an email to the user with instructions to reset the password, including a tokenized URL that directs the user to the password reset form.
    ///</summary>
    member this.PostAuthRecoverPassword
        (
            payload: PostAuthRecoverPasswordPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/auth/recover-password" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAuthRecoverPassword.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAuthRecoverPassword.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAuthRecoverPassword.Unauthorized(Serializer.deserialize content)
        else
            PostAuthRecoverPassword.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Takes token session sent in email and resets the password for the active user.
    ///</summary>
    ///<param name="mailtoken">A valid token recived via email.</param>
    ///<param name="payload"></param>
    ///<param name="cancellationToken"></param>
    member this.PostAuthResetByMailtoken
        (
            mailtoken: string,
            payload: PostAuthResetByMailtokenPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.path ("mailtoken", mailtoken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/auth/reset/{mailtoken}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAuthResetByMailtoken.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAuthResetByMailtoken.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAuthResetByMailtoken.Unauthorized(Serializer.deserialize content)
        else
            PostAuthResetByMailtoken.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Revokes the active session for the user by disabling the token. This disables the different sessions opened on differents computers and browsers.&amp;lt;br /&amp;gt; The user will login again to use the App, and a new token will be generate.
    ///</summary>
    member this.PostAuthRevokeToken
        (
            xAccessToken: string,
            payload: PostAuthRevokeTokenPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/auth/revoke-token" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAuthRevokeToken.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAuthRevokeToken.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAuthRevokeToken.Unauthorized(Serializer.deserialize content)
        else
            PostAuthRevokeToken.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Changes the user’s password and resets the session token.
    ///</summary>
    member this.PostAuthChangePassword
        (
            xAccessToken: string,
            payload: PostAuthChangePasswordPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/auth/change-password" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAuthChangePassword.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAuthChangePassword.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAuthChangePassword.Unauthorized(Serializer.deserialize content)
        else
            PostAuthChangePassword.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Creates a new user taking token and new user data. Returns the new user data. An email is sent to the new user with instructions to log in.
    ///</summary>
    member this.PostUsers(xAccessToken: string, payload: PostUsersPayload, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/users" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostUsers.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostUsers.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostUsers.Unauthorized(Serializer.deserialize content)
        else
            PostUsers.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Takes token and account ID as mandatory parameters and returns an array of users. If nothing is found returns an empty array.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    ///<param name="username"></param>
    ///<param name="email"></param>
    ///<param name="favorites"></param>
    ///<param name="status"></param>
    ///<param name="created"></param>
    ///<param name="lastAccess">Filter Accounts by lastAccess. To match an exact datetime use format YYYY-MM-DD HH:mm:SS. To match a range place two datetimes separated by comma. Range filtering support partial dates as YYYY, YYYY-MM, YYYY-MM:DD HH, etc.</param>
    ///<param name="id">Filter by User ID. Multiple comma-separated values are allowed but 400 items maximum.</param>
    ///<param name="limit">Sets the number of results to return per page. The default limit is `10`.</param>
    ///<param name="page">Returns the content of the results page. The default page is `1`.</param>
    ///<param name="sort">It sorts the results by any of the User attributes. Default is unsorted.</param>
    ///<param name="order">It sorts ascendant or descendant. Defaults to ascendant.</param>
    ///<param name="cancellationToken"></param>
    member this.GetUsers
        (
            xAccessToken: string,
            accountId: string,
            ?username: string,
            ?email: string,
            ?favorites: bool,
            ?status: string,
            ?created: string,
            ?lastAccess: string,
            ?id: string,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if username.IsSome then
                  RequestPart.query ("username", username.Value)
              if email.IsSome then
                  RequestPart.query ("email", email.Value)
              if favorites.IsSome then
                  RequestPart.query ("favorites", favorites.Value)
              if status.IsSome then
                  RequestPart.query ("status", status.Value)
              if created.IsSome then
                  RequestPart.query ("created", created.Value)
              if lastAccess.IsSome then
                  RequestPart.query ("lastAccess", lastAccess.Value)
              if id.IsSome then
                  RequestPart.query ("_id", id.Value)
              if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if page.IsSome then
                  RequestPart.query ("page", page.Value)
              if sort.IsSome then
                  RequestPart.query ("sort", sort.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/users" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetUsers.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetUsers.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetUsers.Unauthorized(Serializer.deserialize content)
        else
            GetUsers.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Takes token, account ID and and bulk search parameters to returns a list of users belonging to a specific account.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    ///<param name="payload"></param>
    ///<param name="username"></param>
    ///<param name="email"></param>
    ///<param name="favorites"></param>
    ///<param name="status"></param>
    ///<param name="created"></param>
    ///<param name="lastAccess">Filter Accounts by lastAccess. To match an exact datetime use format YYYY-MM-DD HH:mm:SS. To match a range place two datetimes separated by comma. Range filtering support partial dates as YYYY, YYYY-MM, YYYY-MM:DD HH, etc.</param>
    ///<param name="id">Filter by User ID. Multiple comma-separated values are allowed but 400 items maximum.</param>
    ///<param name="limit">Sets the number of results to return per page. The default limit is `10`.</param>
    ///<param name="page">Returns the content of the results page. The default page is `1`.</param>
    ///<param name="sort">It sorts the results by any of the User attributes. Default is unsorted.</param>
    ///<param name="order">It sorts ascendant or descendant. Defaults to ascendant.</param>
    ///<param name="cancellationToken"></param>
    member this.PostUsersbulk
        (
            xAccessToken: string,
            accountId: string,
            payload: PostUsersbulkPayload,
            ?username: string,
            ?email: string,
            ?favorites: bool,
            ?status: string,
            ?created: string,
            ?lastAccess: string,
            ?id: string,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              RequestPart.jsonContent payload
              if username.IsSome then
                  RequestPart.query ("username", username.Value)
              if email.IsSome then
                  RequestPart.query ("email", email.Value)
              if favorites.IsSome then
                  RequestPart.query ("favorites", favorites.Value)
              if status.IsSome then
                  RequestPart.query ("status", status.Value)
              if created.IsSome then
                  RequestPart.query ("created", created.Value)
              if lastAccess.IsSome then
                  RequestPart.query ("lastAccess", lastAccess.Value)
              if id.IsSome then
                  RequestPart.query ("_id", id.Value)
              if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if page.IsSome then
                  RequestPart.query ("page", page.Value)
              if sort.IsSome then
                  RequestPart.query ("sort", sort.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/usersbulk" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostUsersbulk.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostUsersbulk.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostUsersbulk.Unauthorized(Serializer.deserialize content)
        else
            PostUsersbulk.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Takes token and returns user data.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    ///<param name="cancellationToken"></param>
    member this.GetUsersMe(xAccessToken: string, accountId: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/users/me" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetUsersMe.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetUsersMe.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetUsersMe.Unauthorized(Serializer.deserialize content)
        else
            GetUsersMe.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///User accept terms and conditions.
    ///</summary>
    member this.PostUsersMeAcceptTc(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/users/me/accept-tc" requestParts cancellationToken

        if status = HttpStatusCode.Created then
            PostUsersMeAcceptTc.Created
        else if status = HttpStatusCode.BadRequest then
            PostUsersMeAcceptTc.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostUsersMeAcceptTc.Unauthorized(Serializer.deserialize content)
        else
            PostUsersMeAcceptTc.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Gets a user by ID, using token, userID and accountID as parameters, and returns a JSON with the users data.
    ///</summary>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="userId">user ID.</param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    ///<param name="cancellationToken"></param>
    member this.GetUsersByUserId
        (
            xAccessToken: string,
            userId: string,
            accountId: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.path ("userId", userId)
              RequestPart.query ("accountId", accountId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/users/{userId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetUsersByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetUsersByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetUsersByUserId.Unauthorized(Serializer.deserialize content)
        else
            GetUsersByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Modify a user using the parameters available.
    ///</summary>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="userId">user ID.</param>
    ///<param name="payload"></param>
    ///<param name="cancellationToken"></param>
    member this.PutUsersByUserId
        (
            xAccessToken: string,
            userId: string,
            payload: PutUsersByUserIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.path ("userId", userId)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/users/{userId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutUsersByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutUsersByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutUsersByUserId.Unauthorized(Serializer.deserialize content)
        else
            PutUsersByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Takes token and user ID, then removes user.
    ///</summary>
    ///<param name="userId">User ID</param>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="payload"></param>
    ///<param name="cancellationToken"></param>
    member this.DeleteUsersByUserId
        (
            userId: string,
            xAccessToken: string,
            payload: DeleteUsersByUserIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.path ("userId", userId)
              RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/users/{userId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            DeleteUsersByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            DeleteUsersByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteUsersByUserId.Unauthorized(Serializer.deserialize content)
        else
            DeleteUsersByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Change a user password using the parameters available.
    ///</summary>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="payload"></param>
    ///<param name="cancellationToken"></param>
    member this.PutUsersChangePasswordByUserId
        (
            xAccessToken: string,
            payload: PutUsersChangePasswordByUserIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/users/{userId}/change-password" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutUsersChangePasswordByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutUsersChangePasswordByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutUsersChangePasswordByUserId.Unauthorized(Serializer.deserialize content)
        else
            PutUsersChangePasswordByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Modify the user options for favorites using the parameters available.
    ///</summary>
    member this.PutUsersFavoritesByUserId
        (
            xAccessToken: string,
            userId: string,
            payload: PutUsersFavoritesByUserIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.path ("userId", userId)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/users/{userId}/favorites" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutUsersFavoritesByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutUsersFavoritesByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutUsersFavoritesByUserId.Unauthorized(Serializer.deserialize content)
        else
            PutUsersFavoritesByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Modify the user customization options. Modify the setup of the columns as displayed in tables in the user interface and the sort order.
    ///</summary>
    member this.PutUsersCustomizationByUserId
        (
            xAccessToken: string,
            userId: string,
            payload: PutUsersCustomizationByUserIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.path ("userId", userId)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/users/{userId}/customization" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutUsersCustomizationByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutUsersCustomizationByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutUsersCustomizationByUserId.Unauthorized(Serializer.deserialize content)
        else
            PutUsersCustomizationByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Gell all users actions for an account.
    ///</summary>
    member this.PutUsersPermissionsByUserId
        (
            xAccessToken: string,
            payload: PutUsersPermissionsByUserIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/users/{userId}/permissions" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutUsersPermissionsByUserId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutUsersPermissionsByUserId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutUsersPermissionsByUserId.Unauthorized(Serializer.deserialize content)
        else
            PutUsersPermissionsByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all notifications of an account.
    ///</summary>
    member this.GetAccountsNotifications(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/notifications" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsNotifications.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsNotifications.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsNotifications.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsNotifications.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Set the notification which id has been given as readed.
    ///</summary>
    member this.PutAccountsNotificationsByNotificationId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/notifications/{notificationId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAccountsNotificationsByNotificationId.OK
        else if status = HttpStatusCode.BadRequest then
            PutAccountsNotificationsByNotificationId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAccountsNotificationsByNotificationId.Unauthorized(Serializer.deserialize content)
        else
            PutAccountsNotificationsByNotificationId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete a notification
    ///</summary>
    member this.DeleteAccountsNotificationsByNotificationId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/accounts/notifications/{notificationId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            DeleteAccountsNotificationsByNotificationId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            DeleteAccountsNotificationsByNotificationId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteAccountsNotificationsByNotificationId.Unauthorized(Serializer.deserialize content)
        else
            DeleteAccountsNotificationsByNotificationId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all accounts and subaccounts.
    ///</summary>
    member this.GetAccounts(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccounts.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccounts.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccounts.Unauthorized(Serializer.deserialize content)
        else
            GetAccounts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create a new child account in the system.
    ///</summary>
    member this.PostAccounts(account: PostAccountsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent account ]

        let (status, content) =
            OpenApiHttp.post httpClient "/accounts" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAccounts.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAccounts.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAccounts.Unauthorized(Serializer.deserialize content)
        else
            PostAccounts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all accounts and subaccounts.
    ///</summary>
    member this.PostAccountsbulk(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/accountsbulk" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAccountsbulk.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAccountsbulk.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAccountsbulk.Unauthorized(Serializer.deserialize content)
        else
            PostAccountsbulk.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieve one account matching the id provided.
    ///</summary>
    member this.GetAccountsByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Modify an existing account with the new data setted into the parameters.
    ///</summary>
    member this.PutAccountsByAccountId(account: PutAccountsByAccountIdPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent account ]

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/{accountId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAccountsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAccountsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAccountsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            PutAccountsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Remove an existing account from the database.
    ///</summary>
    member this.DeleteAccountsByAccountId
        (
            payload: DeleteAccountsByAccountIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/accounts/{accountId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            DeleteAccountsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            DeleteAccountsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteAccountsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            DeleteAccountsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Modify the branding information of an account.
    ///</summary>
    member this.PutAccountsBrandingByAccountId
        (
            account: PutAccountsBrandingByAccountIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent account ]

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/{accountId}/branding" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAccountsBrandingByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAccountsBrandingByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAccountsBrandingByAccountId.Unauthorized(Serializer.deserialize content)
        else
            PutAccountsBrandingByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get Account Branding status.
    ///</summary>
    member this.GetAccountsBrandingVerifyByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/branding/verify" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsBrandingVerifyByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsBrandingVerifyByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsBrandingVerifyByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsBrandingVerifyByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieve all existing roles.
    ///</summary>
    member this.GetAccountsRolesByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/roles" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsRolesByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsRolesByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsRolesByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsRolesByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all actions belonging a role.
    ///</summary>
    member this.GetAccountsRolesActionsByAccountIdAndRolename(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/roles/{rolename}/actions" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsRolesActionsByAccountIdAndRolename.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsRolesActionsByAccountIdAndRolename.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsRolesActionsByAccountIdAndRolename.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsRolesActionsByAccountIdAndRolename.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all notifications of an account.
    ///</summary>
    member this.GetAccountsNotificationsByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/notifications" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsNotificationsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsNotificationsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsNotificationsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsNotificationsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///The notification with the id provided will be check as readed.
    ///</summary>
    member this.PutAccountsNotificationsByAccountIdAndNotificationId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put
                httpClient
                "/accounts/{accountId}/notifications/{notificationId}"
                requestParts
                cancellationToken

        if status = HttpStatusCode.OK then
            PutAccountsNotificationsByAccountIdAndNotificationId.OK
        else if status = HttpStatusCode.BadRequest then
            PutAccountsNotificationsByAccountIdAndNotificationId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAccountsNotificationsByAccountIdAndNotificationId.Unauthorized(Serializer.deserialize content)
        else
            PutAccountsNotificationsByAccountIdAndNotificationId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete a notification
    ///</summary>
    member this.DeleteAccountsNotificationsByAccountIdAndNotificationId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete
                httpClient
                "/accounts/{accountId}/notifications/{notificationId}"
                requestParts
                cancellationToken

        if status = HttpStatusCode.OK then
            DeleteAccountsNotificationsByAccountIdAndNotificationId.OK
        else if status = HttpStatusCode.BadRequest then
            DeleteAccountsNotificationsByAccountIdAndNotificationId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteAccountsNotificationsByAccountIdAndNotificationId.Unauthorized(Serializer.deserialize content)
        else
            DeleteAccountsNotificationsByAccountIdAndNotificationId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrive all related products
    ///</summary>
    member this.GetAccountsProductsByAccountId(?filter: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if filter.IsSome then
                  RequestPart.query ("filter", filter.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/products" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsProductsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsProductsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsProductsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsProductsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///- A maximum of 3 alerts per SIM can be set in order to notify the user when the consumption of a  promotion  reaches or exceeds a certain limit, Alerts can also be used to notify the user of usage over their predefined bundle amount.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutAccountsProductsAlerts
        (
            payload: PutAccountsProductsAlertsPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/products/alerts" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAccountsProductsAlerts.OK
        else if status = HttpStatusCode.BadRequest then
            PutAccountsProductsAlerts.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PutAccountsProductsAlerts.Unauthorized
        else
            PutAccountsProductsAlerts.InternalServerError

    ///<summary>
    ///Amount in which the balance increases.
    ///</summary>
    member this.PutAccountsTopupDirectByAccountId
        (
            payload: PutAccountsTopupDirectByAccountIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/{accountId}/topup-direct" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAccountsTopupDirectByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAccountsTopupDirectByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAccountsTopupDirectByAccountId.Unauthorized(Serializer.deserialize content)
        else
            PutAccountsTopupDirectByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Add a Security Service Setting inside an account
    ///</summary>
    member this.PostAccountsSecuritySettingsByAccountId
        (
            payload: AccountSecuritySetting,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/accounts/{accountId}/security-settings" requestParts cancellationToken

        PostAccountsSecuritySettingsByAccountId.OK

    ///<summary>
    ///Edit a Security Service Setting inside an account
    ///</summary>
    member this.PutAccountsSecuritySettingsByAccountIdAndSecuritySettingId
        (
            payload: AccountSecuritySetting,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put
                httpClient
                "/accounts/{accountId}/security-settings/{securitySettingId}"
                requestParts
                cancellationToken

        PutAccountsSecuritySettingsByAccountIdAndSecuritySettingId.OK

    ///<summary>
    ///Edit a Security Service Setting inside an account
    ///</summary>
    member this.DeleteAccountsSecuritySettingsByAccountIdAndSecuritySettingId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete
                httpClient
                "/accounts/{accountId}/security-settings/{securitySettingId}"
                requestParts
                cancellationToken

        DeleteAccountsSecuritySettingsByAccountIdAndSecuritySettingId.OK

    ///<summary>
    ///Retrieve one account matching the id provided.
    ///</summary>
    member this.GetAccountsSecuritySettingsAvailableGapsByAccountId
        (
            accountId: string,
            accountIdInQuery: string,
            carrier: string,
            cidrBlockSize: float,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.path ("accountId", accountId)
              RequestPart.query ("accountId", accountIdInQuery)
              RequestPart.query ("carrier", carrier)
              RequestPart.query ("cidrBlockSize", cidrBlockSize) ]

        let (status, content) =
            OpenApiHttp.get
                httpClient
                "/accounts/{accountId}/security-settings/available-gaps"
                requestParts
                cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsSecuritySettingsAvailableGapsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsSecuritySettingsAvailableGapsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsSecuritySettingsAvailableGapsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsSecuritySettingsAvailableGapsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieve available ips for particular pool.
    ///</summary>
    member this.GetAccountsSecuritySettingsAvailableIpsByAccountId
        (
            accountId: string,
            accountIdInQuery: string,
            poolId: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.path ("accountId", accountId)
              RequestPart.query ("accountId", accountIdInQuery)
              RequestPart.query ("poolId", poolId) ]

        let (status, content) =
            OpenApiHttp.get
                httpClient
                "/accounts/{accountId}/security-settings/available-ips"
                requestParts
                cancellationToken

        if status = HttpStatusCode.OK then
            GetAccountsSecuritySettingsAvailableIpsByAccountId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAccountsSecuritySettingsAvailableIpsByAccountId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAccountsSecuritySettingsAvailableIpsByAccountId.Unauthorized(Serializer.deserialize content)
        else
            GetAccountsSecuritySettingsAvailableIpsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Endpoint to asure the App is up and working. If is not, nginx will return a 502 Bad Gateway exception.
    ///</summary>
    member this.GetPing(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/ping" requestParts cancellationToken

        GetPing.OK(Serializer.deserialize content)

    ///<summary>
    ///General endpoint to send an email to an account
    ///</summary>
    member this.PostMail(xAccessToken: string, payload: PostMailPayload, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/mail" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostMail.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostMail.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostMail.Unauthorized(Serializer.deserialize content)
        else
            PostMail.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///General endpoint to send files
    ///</summary>
    member this.PostUpload(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/upload" requestParts cancellationToken

        PostUpload.OK

    ///<summary>
    ///General endpoint to send files
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="fileId">The file to get</param>
    ///<param name="cancellationToken"></param>
    member this.GetFileByFileId(xAccessToken: string, ?fileId: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              if fileId.IsSome then
                  RequestPart.path ("fileId", fileId.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/file/{fileId}" requestParts cancellationToken

        GetFileByFileId.OK

    ///<summary>
    ///Return the branding of a customURL
    ///</summary>
    member this.GetLogincustomization(customURL: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.query ("customURL", customURL) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/logincustomization" requestParts cancellationToken

        GetLogincustomization.OK(Serializer.deserialize content)

    ///<summary>
    ///Create a Fake CDR
    ///</summary>
    member this.PostSimulatecdr
        (
            xAccessToken: string,
            payload: PostSimulatecdrPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/simulatecdr" requestParts cancellationToken

        PostSimulatecdr.OK(Serializer.deserialize content)

    ///<summary>
    ///General endpoint to send a webhook notification
    ///</summary>
    member this.PostWebhook(xAccessToken: string, payload: PostWebhookPayload, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/webhook" requestParts cancellationToken

        PostWebhook.NoContent

    ///<summary>
    ///Retrieves the information stored about Products for an account and below.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="iccid">Shows only the products available for an iccid</param>
    ///<param name="name">Name to filter</param>
    ///<param name="ownerAccountId">Account Owner ID is used for filter products by customer, can differ from your accountId in order to filter by yourself or a child account directly</param>
    ///<param name="accountTransferId">Account Transfer ID mandatory to filter them by customers.</param>
    ///<param name="isTransferred">If the Account Transfer ID exists</param>
    ///<param name="carriers">filter by carriers, divide by comma. By default returns any carrier of the list. If imsisType is 'multiimsis' return only assets with all these carriers</param>
    ///<param name="imsisType">Imsi Type to filter, separated by comma</param>
    ///<param name="type">The Product Type</param>
    ///<param name="currency">Currency of the product.</param>
    ///<param name="cycle"></param>
    ///<param name="cycleUnits">Units used for the cycle</param>
    ///<param name="contractLength"></param>
    ///<param name="renewOnExpiry">null</param>
    ///<param name="renewOnDepletion">null</param>
    ///<param name="id">Filter by Product ID. Multiple comma-separated values are allowed but 400 items maximum.</param>
    ///<param name="limit">Sets the number of results to return per page. The default limit is `10`.</param>
    ///<param name="page">Returns the content of the results page. The default page is `1`.</param>
    ///<param name="sort">It sorts the results by any of the Account attributes. Default is unsorted.</param>
    ///<param name="order">It sorts ascendant or descendant. Defaults to ascendant.</param>
    ///<param name="format"></param>
    ///<param name="cancellationToken"></param>
    member this.GetProducts
        (
            xAccessToken: string,
            accountId: string,
            ?iccid: string,
            ?name: string,
            ?ownerAccountId: string,
            ?accountTransferId: string,
            ?isTransferred: bool,
            ?carriers: string,
            ?imsisType: string,
            ?``type``: string,
            ?currency: string,
            ?cycle: float,
            ?cycleUnits: string,
            ?contractLength: float,
            ?renewOnExpiry: bool,
            ?renewOnDepletion: bool,
            ?id: string,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?format: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if iccid.IsSome then
                  RequestPart.query ("iccid", iccid.Value)
              if name.IsSome then
                  RequestPart.query ("name", name.Value)
              if ownerAccountId.IsSome then
                  RequestPart.query ("ownerAccountId", ownerAccountId.Value)
              if accountTransferId.IsSome then
                  RequestPart.query ("accountTransferId", accountTransferId.Value)
              if isTransferred.IsSome then
                  RequestPart.query ("isTransferred", isTransferred.Value)
              if carriers.IsSome then
                  RequestPart.query ("carriers", carriers.Value)
              if imsisType.IsSome then
                  RequestPart.query ("imsisType", imsisType.Value)
              if ``type``.IsSome then
                  RequestPart.query ("type", ``type``.Value)
              if currency.IsSome then
                  RequestPart.query ("currency", currency.Value)
              if cycle.IsSome then
                  RequestPart.query ("cycle", cycle.Value)
              if cycleUnits.IsSome then
                  RequestPart.query ("cycleUnits", cycleUnits.Value)
              if contractLength.IsSome then
                  RequestPart.query ("contractLength", contractLength.Value)
              if renewOnExpiry.IsSome then
                  RequestPart.query ("renewOnExpiry", renewOnExpiry.Value)
              if renewOnDepletion.IsSome then
                  RequestPart.query ("renewOnDepletion", renewOnDepletion.Value)
              if id.IsSome then
                  RequestPart.query ("_id", id.Value)
              if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if page.IsSome then
                  RequestPart.query ("page", page.Value)
              if sort.IsSome then
                  RequestPart.query ("sort", sort.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value)
              if format.IsSome then
                  RequestPart.query ("format", format.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/products" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetProducts.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetProducts.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            GetProducts.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            GetProducts.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            GetProducts.InternalServerError
        else
            GetProducts.ServiceUnavailable

    ///<summary>
    ///Creates a new product taking a token, an accountId and the new product data. Returns the new product data.
    ///</summary>
    member this.PostProducts
        (
            xAccessToken: string,
            payload: PostProductsPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/products" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostProducts.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostProducts.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostProducts.Unauthorized(Serializer.deserialize content)
        else
            PostProducts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get a product of an account
    ///</summary>
    member this.GetProductsByProductId(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/products/{productId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetProductsByProductId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetProductsByProductId.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            GetProductsByProductId.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            GetProductsByProductId.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            GetProductsByProductId.InternalServerError
        else
            GetProductsByProductId.ServiceUnavailable

    ///<summary>
    ///Updates a product taking a token, an accountId and the updated product data. Returns the updated product data.
    ///</summary>
    member this.PatchProductsByProductId
        (
            xAccessToken: string,
            payload: ProductNoBundleId,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.patch httpClient "/products/{productId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PatchProductsByProductId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PatchProductsByProductId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PatchProductsByProductId.Unauthorized(Serializer.deserialize content)
        else
            PatchProductsByProductId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete product by ID
    ///</summary>
    member this.DeleteProductsByProductId(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/products/{productId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            DeleteProductsByProductId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            DeleteProductsByProductId.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteProductsByProductId.Unauthorized(Serializer.deserialize content)
        else
            DeleteProductsByProductId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all the availables schemas for creating a product.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    ///<param name="cancellationToken"></param>
    member this.GetSchemaProduct(xAccessToken: string, accountId: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/schema/product" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetSchemaProduct.OK
        else if status = HttpStatusCode.BadRequest then
            GetSchemaProduct.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetSchemaProduct.Unauthorized(Serializer.deserialize content)
        else
            GetSchemaProduct.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Transfer products from an account to another
    ///</summary>
    member this.PostProductsTransferByProductId
        (
            xAccessToken: string,
            payload: PostProductsTransferByProductIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/products/{productId}/transfer" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostProductsTransferByProductId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostProductsTransferByProductId.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PostProductsTransferByProductId.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            PostProductsTransferByProductId.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            PostProductsTransferByProductId.InternalServerError
        else
            PostProductsTransferByProductId.ServiceUnavailable

    ///<summary>
    ///Get cdrs on application. If param type is not set to sms, always returns data cdrs
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="type">Filter SIM cards by type. &amp;lt;br&amp;gt; - If param iccid is not set and param type is not set or set to `data`, it filters only type data cdrs. If param type is set to `sms` only filters type sms cdrs. &amp;lt;br&amp;gt; - If param iccid is set and param type is not set or set to `data`, it returns data cdrs of that iccid. If param type is set to `sms`, it returns sms cdrs for that iccid..</param>
    ///<param name="limit">Sets the number of results to return per page. The default limit is `10`.</param>
    ///<param name="page">Returns the content of the results page. The default page is `1`.</param>
    ///<param name="sort">It sorts the results by any of the Account attributes. Default is unsorted.</param>
    ///<param name="order">It sorts ascendant or descendant. Defaults to ascendant.</param>
    ///<param name="format"></param>
    ///<param name="cancellationToken"></param>
    member this.GetCdr
        (
            xAccessToken: string,
            accountId: string,
            ?``type``: string,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?format: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if ``type``.IsSome then
                  RequestPart.query ("type", ``type``.Value)
              if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if page.IsSome then
                  RequestPart.query ("page", page.Value)
              if sort.IsSome then
                  RequestPart.query ("sort", sort.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value)
              if format.IsSome then
                  RequestPart.query ("format", format.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/cdr" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetCdr.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetCdr.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetCdr.Unauthorized(Serializer.deserialize content)
        else if status = HttpStatusCode.NotFound then
            GetCdr.NotFound(Serializer.deserialize content)
        else
            GetCdr.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all cdrs on application. If type is not set to `sms` it returns data cdrs.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="type">Filter SIM cards by type. &amp;lt;br&amp;gt; - If param iccid is not set and param type is not set or set to `data`, it filters only type data cdrs. If param type is set to `sms` only filters type sms cdrs. &amp;lt;br&amp;gt; - If param iccid is set and param type is not set or set to `data`, it returns data cdrs of that iccid. If param type is set to `sms`, it returns sms cdrs for that iccid..</param>
    ///<param name="cancellationToken"></param>
    member this.GetCdrStats
        (
            xAccessToken: string,
            accountId: string,
            ?``type``: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if ``type``.IsSome then
                  RequestPart.query ("type", ``type``.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/cdr/stats" requestParts cancellationToken

        GetCdrStats.OK(Serializer.deserialize content)

    ///<summary>
    ///Assets card Info (bulk)
    ///</summary>
    member this.GetAssets(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssets.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAssets.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssets.Unauthorized(Serializer.deserialize content)
        else
            GetAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create a new Asset
    ///</summary>
    member this.PostAssets(payload: PostAssetsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssets.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAssets.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAssets.Unauthorized(Serializer.deserialize content)
        else
            PostAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Assets card Info (bulk)
    ///</summary>
    member this.PostAssetsbulk(payload: PostAssetsbulkPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assetsbulk" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsbulk.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAssetsbulk.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsbulk.Unauthorized(Serializer.deserialize content)
        else
            PostAssetsbulk.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Assets get Info
    ///</summary>
    member this.GetAssetsByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssetsByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAssetsByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssetsByIccid.Unauthorized(Serializer.deserialize content)
        else
            GetAssetsByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Change the name of the asset for the specified accountId
    ///</summary>
    member this.PutAssetsByIccid(asset: PutAssetsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent asset ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Remove an Asset
    ///</summary>
    member this.DeleteAssetsByIccid(asset: DeleteAssetsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent asset ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/assets/{iccid}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            DeleteAssetsByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            DeleteAssetsByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteAssetsByIccid.Unauthorized(Serializer.deserialize content)
        else
            DeleteAssetsByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Update group name
    ///</summary>
    member this.PutAssetsGroupnameByIccid
        (
            asset: PutAssetsGroupnameByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent asset ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/groupname" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsGroupnameByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsGroupnameByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsGroupnameByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsGroupnameByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Transfer an Asset from an account to another
    ///</summary>
    member this.PostAssetsTransferByIccid
        (
            asset: PostAssetsTransferByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent asset ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/transfer" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsTransferByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAssetsTransferByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsTransferByIccid.Unauthorized(Serializer.deserialize content)
        else
            PostAssetsTransferByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Activate an asset and subscribe the asset to the assigned product.
    ///</summary>
    member this.PutAssetsSubscribeByIccid
        (
            payload: PutAssetsSubscribeByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/subscribe" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsSubscribeByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsSubscribeByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsSubscribeByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsSubscribeByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Unsubscribe the asset to the assigned product
    ///</summary>
    member this.PutAssetsUnsubscribeByIccid
        (
            payload: PutAssetsUnsubscribeByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/unsubscribe" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsUnsubscribeByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsUnsubscribeByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsUnsubscribeByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsUnsubscribeByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Remove subscription and create a new subscribe the asset to the assigned product.
    ///</summary>
    member this.PutAssetsResubscribeByIccid
        (
            payload: PutAssetsResubscribeByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/resubscribe" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsResubscribeByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsResubscribeByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsResubscribeByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsResubscribeByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Suspend an assets
    ///</summary>
    member this.PutAssetsSuspendByIccid
        (
            payload: PutAssetsSuspendByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/suspend" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsSuspendByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsSuspendByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsSuspendByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsSuspendByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Reactive an assets. The assets should be suspended
    ///</summary>
    member this.PutAssetsUnsuspendByIccid
        (
            payload: PutAssetsUnsuspendByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/unsuspend" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsUnsuspendByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsUnsuspendByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsUnsuspendByIccid.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsUnsuspendByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutAssetsAlertsByIccid(payload: PutAssetsAlertsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/alerts" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsAlertsByIccid.OK
        else if status = HttpStatusCode.BadRequest then
            PutAssetsAlertsByIccid.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsAlertsByIccid.Unauthorized
        else
            PutAssetsAlertsByIccid.InternalServerError

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostAssetsPurgeByIccid(payload: PostAssetsPurgeByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/purge" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsPurgeByIccid.OK
        else if status = HttpStatusCode.BadRequest then
            PostAssetsPurgeByIccid.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsPurgeByIccid.Unauthorized
        else
            PostAssetsPurgeByIccid.InternalServerError

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostAssetsSmsByIccid(message: PostAssetsSmsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent message ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/sms" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsSmsByIccid.OK
        else if status = HttpStatusCode.BadRequest then
            PostAssetsSmsByIccid.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsSmsByIccid.Unauthorized
        else
            PostAssetsSmsByIccid.InternalServerError

    ///<summary>
    ///Set limits for a simcard. You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostAssetsLimitByIccid(message: PostAssetsLimitByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent message ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/limit" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsLimitByIccid.OK
        else if status = HttpStatusCode.BadRequest then
            PostAssetsLimitByIccid.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsLimitByIccid.Unauthorized
        else
            PostAssetsLimitByIccid.InternalServerError

    ///<summary>
    ///Set tags for a simcard. Each tag have an index  related with the order of the custom field.,  You need to add in each query as many values as tags have the simcard.   Example if you create three custom Fields  in this order  CF1, CF2 and CF3, you will need to add in the body 3 sections with the values that you need
    ///```
    ///"tags":[
    ///  {"key":"1","value":""},
    ///  {"key":"2","value":""},
    ///  {"key":"3","value":""}
    ///]
    ///```
    ///If you want to change a value for only one of them you still need to add the rest of the values
    ///```
    ///"tags":[
    ///  {"key":"1","value":"Value1"},
    ///  {"key":"2","value":"NEWVALUE"},
    ///  {"key":"3","value":"Value3"}
    ///]
    ///```
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="iccid">ICCID number.</param>
    ///<param name="message"></param>
    ///<param name="cancellationToken"></param>
    member this.PostAssetsTagsByIccid
        (
            xAccessToken: string,
            iccid: string,
            message: PostAssetsTagsByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.path ("iccid", iccid)
              RequestPart.jsonContent message ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/tags" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsTagsByIccid.OK
        else if status = HttpStatusCode.BadRequest then
            PostAssetsTagsByIccid.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsTagsByIccid.Unauthorized
        else
            PostAssetsTagsByIccid.InternalServerError

    ///<summary>
    ///Checks the simcard status over the network, if the sim is correctly provisioned in the system, last connection, last data transmision, if it is online etc...
    ///</summary>
    member this.GetAssetsDiagnosticByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/diagnostic" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssetsDiagnosticByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssetsDiagnosticByIccid.Unauthorized
        else if status = HttpStatusCode.InternalServerError then
            GetAssetsDiagnosticByIccid.InternalServerError
        else
            GetAssetsDiagnosticByIccid.ServiceUnavailable

    ///<summary>
    ///Return assets location.
    ///</summary>
    member this.GetAssetsLocationByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/location" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssetsLocationByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAssetsLocationByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssetsLocationByIccid.Unauthorized(Serializer.deserialize content)
        else if status = HttpStatusCode.NotFound then
            GetAssetsLocationByIccid.NotFound(Serializer.deserialize content)
        else
            GetAssetsLocationByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Return asset sessions.
    ///</summary>
    member this.GetAssetsSessionsByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/sessions" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssetsSessionsByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAssetsSessionsByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssetsSessionsByIccid.Unauthorized(Serializer.deserialize content)
        else if status = HttpStatusCode.NotFound then
            GetAssetsSessionsByIccid.NotFound(Serializer.deserialize content)
        else
            GetAssetsSessionsByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Reassign a new IP to a single asset or a bulk of assets. You can choose to update the IP and assign a new Fixed IP from a security service or a dynamic IP.
    ///</summary>
    member this.PostAssetsReallocateIpByIccid
        (
            payload: PostAssetsReallocateIpByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/reallocate-ip" requestParts cancellationToken

        PostAssetsReallocateIpByIccid.OK

    ///<summary>
    ///Get all reports from an account
    ///</summary>
    member this.GetReportsCustom(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/reports/custom" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetReportsCustom.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetReportsCustom.Unauthorized(Serializer.deserialize content)
        else
            GetReportsCustom.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create a new custom report. &amp;lt;/br&amp;gt; Date filtering supports dates as `YYYY-MM-DDTHH:mm:SSZ`, for example `"2020-04-01T00:59:59Z"` &amp;lt;/br&amp;gt; The fileds requires are; accountId, dateFrom, dateTo
    ///</summary>
    member this.PostReportsCustom(report: FilterCustom, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent report ]

        let (status, content) =
            OpenApiHttp.post httpClient "/reports/custom" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostReportsCustom.OK
        else if status = HttpStatusCode.Unauthorized then
            PostReportsCustom.Unauthorized(Serializer.deserialize content)
        else if status = HttpStatusCode.Forbidden then
            PostReportsCustom.Forbidden(Serializer.deserialize content)
        else if status = HttpStatusCode.NotFound then
            PostReportsCustom.NotFound(Serializer.deserialize content)
        else
            PostReportsCustom.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete all reports from an account
    ///</summary>
    member this.DeleteReportsCustom(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/reports/custom" requestParts cancellationToken

        if status = HttpStatusCode.NoContent then
            DeleteReportsCustom.NoContent
        else if status = HttpStatusCode.Unauthorized then
            DeleteReportsCustom.Unauthorized(Serializer.deserialize content)
        else
            DeleteReportsCustom.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get one reports from an account
    ///</summary>
    member this.GetReportsCustomByReportId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/reports/custom/{reportId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetReportsCustomByReportId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetReportsCustomByReportId.Unauthorized(Serializer.deserialize content)
        else if status = HttpStatusCode.NotFound then
            GetReportsCustomByReportId.NotFound(Serializer.deserialize content)
        else
            GetReportsCustomByReportId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete one report from an account
    ///</summary>
    member this.DeleteReportsCustomByReportId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/reports/custom/{reportId}" requestParts cancellationToken

        if status = HttpStatusCode.NoContent then
            DeleteReportsCustomByReportId.NoContent
        else if status = HttpStatusCode.Unauthorized then
            DeleteReportsCustomByReportId.Unauthorized(Serializer.deserialize content)
        else if status = HttpStatusCode.NotFound then
            DeleteReportsCustomByReportId.NotFound(Serializer.deserialize content)
        else
            DeleteReportsCustomByReportId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Assets card Info (bulk)
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter them by customers.</param>
    ///<param name="userId">Id of the user that generates the action.</param>
    ///<param name="iccid">Asset iccid(s) to filter. If it's more than one, separated by comma</param>
    ///<param name="eid">eSIM eid(s) to filter. If it's more than one, separated by comma</param>
    ///<param name="action">Action(s) to filter. If it's more than one, separated by comma</param>
    ///<param name="created">To match a range place two datetimes separated by comma. Range filtering support partial dates as YYYY, YYYY-MM, YYYY-MM:DD HH, etc.</param>
    ///<param name="showSubaccounts">Show all the events for your account and subaccounts. By default is false</param>
    ///<param name="limit">Sets the number of results to return per page. The default limit is `10`.</param>
    ///<param name="page">Returns the content of the results page. The default page is `1`.</param>
    ///<param name="sort">It sorts the results by any of the SIM card attributes. Default is unsorted.</param>
    ///<param name="order">It sorts ascendant or descendant. Defaults to ascendant.</param>
    ///<param name="format"></param>
    ///<param name="cancellationToken"></param>
    member this.GetEvents
        (
            xAccessToken: string,
            accountId: string,
            ?userId: string,
            ?iccid: string,
            ?eid: string,
            ?action: string,
            ?created: string,
            ?showSubaccounts: string,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?format: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if userId.IsSome then
                  RequestPart.query ("userId", userId.Value)
              if iccid.IsSome then
                  RequestPart.query ("iccid", iccid.Value)
              if eid.IsSome then
                  RequestPart.query ("eid", eid.Value)
              if action.IsSome then
                  RequestPart.query ("action", action.Value)
              if created.IsSome then
                  RequestPart.query ("created", created.Value)
              if showSubaccounts.IsSome then
                  RequestPart.query ("showSubaccounts", showSubaccounts.Value)
              if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if page.IsSome then
                  RequestPart.query ("page", page.Value)
              if sort.IsSome then
                  RequestPart.query ("sort", sort.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value)
              if format.IsSome then
                  RequestPart.query ("format", format.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/events" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetEvents.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetEvents.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetEvents.Unauthorized(Serializer.deserialize content)
        else
            GetEvents.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all zones schemes
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="carrier">filter by carrier</param>
    ///<param name="cancellationToken"></param>
    member this.GetZonesSchemes
        (
            xAccessToken: string,
            accountId: string,
            ?carrier: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if carrier.IsSome then
                  RequestPart.query ("carrier", carrier.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/zones-schemes" requestParts cancellationToken

        GetZonesSchemes.OK(Serializer.deserialize content)

    ///<summary>
    ///Get a zone schemes
    ///</summary>
    member this.GetZonesSchemesByZoneSchemeId
        (
            xAccessToken: string,
            accountId: string,
            zoneSchemeId: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              RequestPart.path ("zoneSchemeId", zoneSchemeId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/zones-schemes/{zoneSchemeId}" requestParts cancellationToken

        GetZonesSchemesByZoneSchemeId.OK(Serializer.deserialize content)

    ///<summary>
    ///Activate a bulk of assets and subscribe the assets to the assigned product.
    ///</summary>
    member this.PostBulkAssetsSubscribe(body: PostBulkAssetsSubscribePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/subscribe" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsSubscribe.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsSubscribe.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsSubscribe.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsSubscribe.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Transfer a Asset from a account to another in bulk
    ///</summary>
    member this.PostBulkAssetsTransfer(body: PostBulkAssetsTransferPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/transfer" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsTransfer.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsTransfer.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsTransfer.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsTransfer.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Return a Asset from a account to parent account in bulk
    ///</summary>
    member this.PostBulkAssetsReturn(body: PostBulkAssetsReturnPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/return" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsReturn.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsReturn.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsReturn.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsReturn.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Suspend assets in a massive way by using different filters
    ///</summary>
    member this.PutBulkAssetsSuspend(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/suspend" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PutBulkAssetsSuspend.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutBulkAssetsSuspend.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutBulkAssetsSuspend.Unauthorized(Serializer.deserialize content)
        else
            PutBulkAssetsSuspend.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///This endpoint change in a massive way the suspend status of the assets which match with the filter setted.
    ///</summary>
    member this.PutBulkAssetsUnsuspend(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/unsuspend" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PutBulkAssetsUnsuspend.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutBulkAssetsUnsuspend.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutBulkAssetsUnsuspend.Unauthorized(Serializer.deserialize content)
        else
            PutBulkAssetsUnsuspend.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Resubscribe a bulk of assets to the assigned product.
    ///</summary>
    member this.PostBulkAssetsResubscribe
        (
            body: PostBulkAssetsResubscribePayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/resubscribe" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsResubscribe.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsResubscribe.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsResubscribe.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsResubscribe.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create Assets in bulk mode
    ///</summary>
    member this.PostBulkAssets(body: PostBulkAssetsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssets.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssets.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssets.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Update Assets in bulk mode
    ///</summary>
    member this.PutBulkAssets(body: PutBulkAssetsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PutBulkAssets.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutBulkAssets.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutBulkAssets.Unauthorized(Serializer.deserialize content)
        else
            PutBulkAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Update group name of Assets in bulk mode
    ///</summary>
    member this.PutBulkAssetsGroupname(body: PutBulkAssetsGroupnamePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/groupname" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PutBulkAssetsGroupname.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutBulkAssetsGroupname.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutBulkAssetsGroupname.Unauthorized(Serializer.deserialize content)
        else
            PutBulkAssetsGroupname.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostBulkAssetsLimit(body: PostBulkAssetsLimitPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/limit" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsLimit.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsLimit.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsLimit.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsLimit.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///- A maximum of 3 alerts per SIM can be set in order to notify the user when the consumption of a  promotion  reaches or exceeds a certain limit, Alerts can also be used to notify the user of usage over their predefined bundle amount.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set only.
    ///</summary>
    member this.PutBulkAssetsAlerts(body: PutBulkAssetsAlertsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/alerts" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PutBulkAssetsAlerts.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutBulkAssetsAlerts.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutBulkAssetsAlerts.Unauthorized(Serializer.deserialize content)
        else
            PutBulkAssetsAlerts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostBulkAssetsSms(body: PostBulkAssetsSmsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/sms" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsSms.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsSms.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsSms.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsSms.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostBulkAssetsPurge(body: PostBulkAssetsPurgePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/purge" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsPurge.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsPurge.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsPurge.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsPurge.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Download template for bulk update.
    ///</summary>
    ///<param name="blankTemplate">true if you want only the headers</param>
    ///<param name="cancellationToken"></param>
    member this.PostBulkAssetsUpdateTemplate(?blankTemplate: bool, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if blankTemplate.IsSome then
                  RequestPart.query ("blankTemplate", blankTemplate.Value) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/update/template" requestParts cancellationToken

        PostBulkAssetsUpdateTemplate.OK

    ///<summary>
    ///Bulk updates of assets for name, group and custom attributes uploading a CSV file.
    ///</summary>
    member this.PostBulkAssetsUpdateProcess(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/update/process" requestParts cancellationToken

        PostBulkAssetsUpdateProcess.Accepted(Serializer.deserialize content)

    ///<summary>
    ///Reassign a new IP in a bulk of assets selecting to assign a new dynamic IP or a security service.
    ///</summary>
    member this.PostBulkAssetsReallocateIp
        (
            payload: PostBulkAssetsReallocateIpPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/reallocate-ip" requestParts cancellationToken

        if status = HttpStatusCode.Accepted then
            PostBulkAssetsReallocateIp.Accepted(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostBulkAssetsReallocateIp.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostBulkAssetsReallocateIp.Unauthorized(Serializer.deserialize content)
        else
            PostBulkAssetsReallocateIp.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieve the URL and params for the redirection.
    ///```
    ///  var xmlhttp = new XMLHttpRequest();
    ///  var orderId;
    ///  var ACCESSTOKEN = '[ACCESSTOKEN]'
    ///  var BASE_URL = 'https://hummingbird-dev.podgroup.com/v3';
    ///  paypal.Button.render({
    ///    env: 'sandbox', // sandbox | production
    ///    // Show the buyer a 'Pay Now' button in the checkout flow
    ///    // commit: true,
    ///    style: {
    ///      label: 'paypal',
    ///      size:  'responsive',    // small | medium | large | responsive
    ///      shape: 'rect',     // pill | rect
    ///      color: 'blue',     // gold | blue | silver | black
    ///      tagline: false
    ///    },
    ///    // payment() is called when the button is clicked
    ///    payment: function() {
    ///      // Set up a url on your server to create the payment
    ///      var CREATE_URL = BASE_URL + '/payments/topupPaypal';
    ///      // Set up the data
    ///      var data = {
    ///        "accountId": "[AccountId]",
    ///        "amount": 9,
    ///        "currency": "EUR",
    ///        "returnUrl": "http://www.example.com/returnUrl",
    ///        "cancelUrl": "http://www.example.com/cancelUrl"
    ///      }
    ///      // Make a call to your server to set up the payment
    ///      return axios.post(CREATE_URL, data, {headers: {"x-access-token": ACCESSTOKEN}})
    ///        .then(function(res) {
    ///          orderId = res.data.orderId;
    ///          return res.data.providerId;
    ///        });
    ///      },
    ///      // onAuthorize() is called when the buyer approves the payment
    ///      onAuthorize: function(data, actions) {
    ///        // Set up a url on your server to execute the payment
    ///        var EXECUTE_URL = BASE_URL + '/payments/confirmTopupPaypal';
    ///        // Set up the data you need to pass to your server
    ///        var data = {
    ///          orderId: orderId,
    ///          accountId: "[AccountId]",
    ///          status: true
    ///        };
    ///        // Make a call to your server to execute the payment
    ///        return axios.post(EXECUTE_URL, data, {headers: {"x-access-token": ACCESSTOKEN}})
    ///          .then(function (res) {
    ///            window.alert('Payment Complete!');
    ///        });
    ///    }
    ///    }, '#paypal-button-container');
    ///```
    ///</summary>
    member this.PostPaymentsTopupPaypal
        (
            xAccessToken: string,
            payload: PostPaymentsTopupPaypalPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/payments/topupPaypal" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostPaymentsTopupPaypal.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostPaymentsTopupPaypal.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostPaymentsTopupPaypal.Unauthorized(Serializer.deserialize content)
        else
            PostPaymentsTopupPaypal.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Confirms a Paypal transaction
    ///</summary>
    member this.PostPaymentsConfirmTopupPaypal
        (
            xAccessToken: string,
            payload: PostPaymentsConfirmTopupPaypalPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/payments/confirmTopupPaypal" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostPaymentsConfirmTopupPaypal.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostPaymentsConfirmTopupPaypal.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostPaymentsConfirmTopupPaypal.Unauthorized(Serializer.deserialize content)
        else
            PostPaymentsConfirmTopupPaypal.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieves security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alarmLink">Alert alarm link</param>
    ///<param name="archived">Set to `true` to get archived Alerts. Default value is `false`</param>
    ///<param name="description">To filter by exact description</param>
    ///<param name="destinationEthernet">To filter by Destination Ethernet</param>
    ///<param name="destinationIP">To filter by Destination IP</param>
    ///<param name="destinationPort">To filter by Destination port</param>
    ///<param name="iccid">To filter by ICCID</param>
    ///<param name="id">To filter Alert by id</param>
    ///<param name="protocol">To filter by Protocol</param>
    ///<param name="sourceEthernet">To filter by Source Ethernet</param>
    ///<param name="sourceIP">To filter by Source IP</param>
    ///<param name="sourcePort">To filter by Source port</param>
    ///<param name="time">To filter Alert by time</param>
    ///<param name="timeStamp">To filter Alert by exact time stamp</param>
    ///<param name="type">To filter by Alert type</param>
    ///<param name="visible">To filter by Alert visibility</param>
    ///<param name="limit">Sets the number of results to return per page. The default limit is `10`.</param>
    ///<param name="page">Returns the content of the results page. The default page is `1`.</param>
    ///<param name="sort">It sorts the results by any of the Alert attributes. Default is `time`.</param>
    ///<param name="order">It sorts ascendant or descendant. Defaults to descendant.</param>
    ///<param name="cancellationToken"></param>
    member this.GetSecurityAlerts
        (
            xAccessToken: string,
            accountId: string,
            ?alarmLink: string,
            ?archived: bool,
            ?description: string,
            ?destinationEthernet: string,
            ?destinationIP: string,
            ?destinationPort: string,
            ?iccid: string,
            ?id: float,
            ?protocol: string,
            ?sourceEthernet: string,
            ?sourceIP: string,
            ?sourcePort: string,
            ?time: System.DateTimeOffset,
            ?timeStamp: System.DateTimeOffset,
            ?``type``: string,
            ?visible: bool,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if alarmLink.IsSome then
                  RequestPart.query ("alarmLink", alarmLink.Value)
              if archived.IsSome then
                  RequestPart.query ("archived", archived.Value)
              if description.IsSome then
                  RequestPart.query ("description", description.Value)
              if destinationEthernet.IsSome then
                  RequestPart.query ("destinationEthernet", destinationEthernet.Value)
              if destinationIP.IsSome then
                  RequestPart.query ("destinationIP", destinationIP.Value)
              if destinationPort.IsSome then
                  RequestPart.query ("destinationPort", destinationPort.Value)
              if iccid.IsSome then
                  RequestPart.query ("iccid", iccid.Value)
              if id.IsSome then
                  RequestPart.query ("id", id.Value)
              if protocol.IsSome then
                  RequestPart.query ("protocol", protocol.Value)
              if sourceEthernet.IsSome then
                  RequestPart.query ("sourceEthernet", sourceEthernet.Value)
              if sourceIP.IsSome then
                  RequestPart.query ("sourceIP", sourceIP.Value)
              if sourcePort.IsSome then
                  RequestPart.query ("sourcePort", sourcePort.Value)
              if time.IsSome then
                  RequestPart.query ("time", time.Value)
              if timeStamp.IsSome then
                  RequestPart.query ("timeStamp", timeStamp.Value)
              if ``type``.IsSome then
                  RequestPart.query ("type", ``type``.Value)
              if visible.IsSome then
                  RequestPart.query ("visible", visible.Value)
              if limit.IsSome then
                  RequestPart.query ("limit", limit.Value)
              if page.IsSome then
                  RequestPart.query ("page", page.Value)
              if sort.IsSome then
                  RequestPart.query ("sort", sort.Value)
              if order.IsSome then
                  RequestPart.query ("order", order.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/security/alerts" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetSecurityAlerts.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetSecurityAlerts.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            GetSecurityAlerts.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            GetSecurityAlerts.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            GetSecurityAlerts.InternalServerError
        else
            GetSecurityAlerts.ServiceUnavailable

    ///<summary>
    ///Fetches security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alertId">To know what alert to find</param>
    ///<param name="cancellationToken"></param>
    member this.GetSecurityAlertsByAlertId
        (
            xAccessToken: string,
            accountId: string,
            alertId: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              RequestPart.path ("alertId", alertId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/security/alerts/{alertId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetSecurityAlertsByAlertId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetSecurityAlertsByAlertId.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            GetSecurityAlertsByAlertId.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            GetSecurityAlertsByAlertId.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            GetSecurityAlertsByAlertId.InternalServerError
        else
            GetSecurityAlertsByAlertId.ServiceUnavailable

    ///<summary>
    ///Edits security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alertId">To know what alert to edit</param>
    ///<param name="payload"></param>
    ///<param name="cancellationToken"></param>
    member this.PutSecurityAlertsByAlertId
        (
            xAccessToken: string,
            accountId: string,
            alertId: string,
            payload: PutSecurityAlertsByAlertIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              RequestPart.path ("alertId", alertId)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/security/alerts/{alertId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutSecurityAlertsByAlertId.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutSecurityAlertsByAlertId.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            PutSecurityAlertsByAlertId.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            PutSecurityAlertsByAlertId.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            PutSecurityAlertsByAlertId.InternalServerError
        else
            PutSecurityAlertsByAlertId.ServiceUnavailable

    ///<summary>
    ///Deletes security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alertId">To know what alert to delete</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSecurityAlertsByAlertId
        (
            xAccessToken: string,
            accountId: string,
            alertId: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              RequestPart.path ("alertId", alertId) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/security/alerts/{alertId}" requestParts cancellationToken

        if status = HttpStatusCode.NoContent then
            DeleteSecurityAlertsByAlertId.NoContent
        else if status = HttpStatusCode.BadRequest then
            DeleteSecurityAlertsByAlertId.BadRequest
        else if status = HttpStatusCode.Unauthorized then
            DeleteSecurityAlertsByAlertId.Unauthorized
        else if status = HttpStatusCode.Forbidden then
            DeleteSecurityAlertsByAlertId.Forbidden
        else if status = HttpStatusCode.InternalServerError then
            DeleteSecurityAlertsByAlertId.InternalServerError
        else
            DeleteSecurityAlertsByAlertId.ServiceUnavailable

    ///<summary>
    ///Get Top 10 subscribers threatened. Type Table
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="isThreat">Type of alert you want to aggregate. True is Threaten, false is other type. By default is True</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraphSecurityTopthreaten
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?isThreat: bool,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value)
              if isThreat.IsSome then
                  RequestPart.query ("isThreat", isThreat.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/security/topthreaten" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraphSecurityTopthreaten.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraphSecurityTopthreaten.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraphSecurityTopthreaten.Unauthorized(Serializer.deserialize content)
        else
            GetGraphSecurityTopthreaten.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get Counter of alert grouped by type of security alert. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="isThreat">Type of alert you want to aggregate. True is Threaten, false is other type. By default is True</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraphSecurityByalarmtype
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?isThreat: bool,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value)
              if isThreat.IsSome then
                  RequestPart.query ("isThreat", isThreat.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/security/byalarmtype" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraphSecurityByalarmtype.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraphSecurityByalarmtype.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraphSecurityByalarmtype.Unauthorized(Serializer.deserialize content)
        else
            GetGraphSecurityByalarmtype.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrive the information stored about eSIMs for an account and below.
    ///</summary>
    member this.GetEsims(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/esims" requestParts cancellationToken

        GetEsims.OK(Serializer.deserialize content)

    ///<summary>
    ///Create a new eSIM. Profiles must already exist, belong to same accountId and comply with all restrictions for eSIMs
    ///- One bootstrap profile per eSIM
    ///- One enabled profile per eSIM
    ///- Profiles not enabled, must be in disabled state
    ///- All profiles must belong to the same account that the eSIM
    ///</summary>
    member this.PostEsims(payload: PostEsimsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostEsims.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostEsims.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostEsims.Unauthorized(Serializer.deserialize content)
        else
            PostEsims.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrive the information stored about eSIMs for an account and below.
    ///</summary>
    member this.PostEsimsbulk(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/esimsbulk" requestParts cancellationToken

        PostEsimsbulk.OK(Serializer.deserialize content)

    ///<summary>
    ///get eSIM info by EID
    ///</summary>
    member this.GetEsimsByEid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/esims/{eid}" requestParts cancellationToken

        GetEsimsByEid.OK(Serializer.deserialize content)

    ///<summary>
    ///Change eSimName and/or eSimGroupName of the eSIM for the specified accountId
    ///</summary>
    member this.PutEsimsByEid(eSim: PutEsimsByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent eSim ]

        let (status, content) =
            OpenApiHttp.put httpClient "/esims/{eid}" requestParts cancellationToken

        PutEsimsByEid.OK

    ///<summary>
    ///Transfer a ESim from a account to another
    ///</summary>
    member this.PostEsimsTransferByEid(payload: PostEsimsTransferByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/transfer" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostEsimsTransferByEid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostEsimsTransferByEid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostEsimsTransferByEid.Unauthorized(Serializer.deserialize content)
        else
            PostEsimsTransferByEid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Return a ESim from a account to parent account
    ///</summary>
    member this.PostEsimsReturnByEid(payload: PostEsimsReturnByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/return" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostEsimsReturnByEid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostEsimsReturnByEid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostEsimsReturnByEid.Unauthorized(Serializer.deserialize content)
        else
            PostEsimsReturnByEid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Activate the enabled profile and subscribe it to the assigned product.
    ///</summary>
    member this.PutEsimsSubscribeByEid(payload: PutEsimsSubscribeByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/esims/{eid}/subscribe" requestParts cancellationToken

        PutEsimsSubscribeByEid.OK

    ///<summary>
    ///Suspend enabled profile.
    ///</summary>
    member this.PutEsimsSuspendByEid(payload: PutEsimsSuspendByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/esims/{eid}/suspend" requestParts cancellationToken

        PutEsimsSuspendByEid.OK

    ///<summary>
    ///Reactivate enabled profile.
    ///</summary>
    member this.PutEsimsUnsuspendByEid(payload: PutEsimsUnsuspendByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/esims/{eid}/unsuspend" requestParts cancellationToken

        PutEsimsUnsuspendByEid.OK

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutEsimsAlertsByEid(payload: PutEsimsAlertsByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/esims/{eid}/alerts" requestParts cancellationToken

        PutEsimsAlertsByEid.OK

    ///<summary>
    ///Reactivate enabled profile.
    ///</summary>
    member this.PostEsimsPurgeByEid(payload: PostEsimsPurgeByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/purge" requestParts cancellationToken

        PostEsimsPurgeByEid.OK

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostEsimsSmsByEid(message: PostEsimsSmsByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent message ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/sms" requestParts cancellationToken

        PostEsimsSmsByEid.OK

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostEsimsLimitByEid(payload: PostEsimsLimitByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/limit" requestParts cancellationToken

        PostEsimsLimitByEid.OK

    ///<summary>
    ///DownloadProfile (ES2) creates a new Issuer Security Domain - Profile (ISD-P) on an eSIM, and then downloads and installs the specified profile onto the card.
    ///Optionally, the installed profile can also be enabled as part of this process. Otherwise, the new profile is installed in the Disabled state.
    ///</summary>
    member this.PostEsimsDownloadProfileByEid
        (
            payload: PostEsimsDownloadProfileByEidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/download-profile" requestParts cancellationToken

        PostEsimsDownloadProfileByEid.OK

    ///<summary>
    ///Moves a profile that is installed on an eSIM from the Disabled state to the Enabled state.
    ///To enable the target profile, the profile that is currently active on the card must also be disabled.
    ///</summary>
    member this.PostEsimsEnableProfileByEid
        (
            payload: PostEsimsEnableProfileByEidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/enable-profile" requestParts cancellationToken

        PostEsimsEnableProfileByEid.OK

    ///<summary>
    ///Moves a profile that is installed on an eSIM from the Enabled state to the Disabled state.
    ///To ensure that card connectivity is maintained, disabling the active profile automatically enables the Bootstrap Profile.
    ///</summary>
    member this.PostEsimsDisableProfileByEid
        (
            payload: PostEsimsDisableProfileByEidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/disable-profile" requestParts cancellationToken

        PostEsimsDisableProfileByEid.OK

    ///<summary>
    ///Removes a profile from an eSIM.
    ///If the profile to be deleted is in the Enabled state, it is first moved to the Disabled state.
    ///Then, to ensure that card connectivity is maintained, the Bootstrap Profile is enabled.
    ///The Bootstrap Profiles (BS) have the fallback attribute, hence cannot be deleted.
    ///</summary>
    member this.PostEsimsDeleteProfileByEid
        (
            payload: PostEsimsDeleteProfileByEidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/delete-profile" requestParts cancellationToken

        PostEsimsDeleteProfileByEid.OK

    ///<summary>
    ///Audit an eSIM and all its downloaded profiles.
    ///</summary>
    member this.PostEsimsAuditByEid(payload: PostEsimsAuditByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/audit" requestParts cancellationToken

        PostEsimsAuditByEid.OK

    ///<summary>
    ///Change eSimName and/or eSimGroupName of the eSIMs for the specified accountId
    ///</summary>
    member this.PutBulkEsims(body: PutBulkEsimsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/esims" requestParts cancellationToken

        PutBulkEsims.Accepted

    ///<summary>
    ///Transfer an eSIM and its profiles from an account to another in bulk
    ///</summary>
    member this.PostBulkEsimsTransfer(body: PostBulkEsimsTransferPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/esims/transfer" requestParts cancellationToken

        PostBulkEsimsTransfer.Accepted

    ///<summary>
    ///Return a ESim from a account to parent account
    ///</summary>
    member this.PostBulkEsimsReturn(body: PostBulkEsimsReturnPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/esims/return" requestParts cancellationToken

        PostBulkEsimsReturn.Accepted

    ///<summary>
    ///Activate a bulk of enabled profiles and subscribe the assets to the assigned product.
    ///</summary>
    member this.PostBulkEsimsSubscribe(body: PostBulkEsimsSubscribePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/esims/subscribe" requestParts cancellationToken

        PostBulkEsimsSubscribe.Accepted

    ///<summary>
    ///Suspend a bulk of enabled profiles
    ///</summary>
    member this.PutBulkEsimsSuspend(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/esims/suspend" requestParts cancellationToken

        PutBulkEsimsSuspend.Accepted

    ///<summary>
    ///Reactivate a bulk of enabled profiles
    ///</summary>
    member this.PutBulkEsimsUnsuspend(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/esims/unsuspend" requestParts cancellationToken

        PutBulkEsimsUnsuspend.Accepted

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutBulkEsimsAlerts(body: PutBulkEsimsAlertsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/esims/alerts" requestParts cancellationToken

        PutBulkEsimsAlerts.Accepted

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostBulkEsimsPurge(body: PostBulkEsimsPurgePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/esims/purge" requestParts cancellationToken

        PostBulkEsimsPurge.Accepted

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostBulkEsimsSms(body: PostBulkEsimsSmsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/esims/sms" requestParts cancellationToken

        PostBulkEsimsSms.Accepted

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostBulkEsimsLimit(body: PostBulkEsimsLimitPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/esims/limit" requestParts cancellationToken

        PostBulkEsimsLimit.Accepted

    ///<summary>
    ///Get bytes consumed by an account.Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph1
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/1" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph1.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph1.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph1.Unauthorized(Serializer.deserialize content)
        else
            GetGraph1.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get amount of cdrs by an account.Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph2
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/2" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph2.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph2.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph2.Unauthorized(Serializer.deserialize content)
        else
            GetGraph2.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get amount of cdrs aggregated by the mcc in an account. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph3
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/3" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph3.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph3.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph3.Unauthorized(Serializer.deserialize content)
        else
            GetGraph3.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get amount of cdrs aggregated by the mcc and the mnc in an account. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph4
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/4" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph4.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph4.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph4.Unauthorized(Serializer.deserialize content)
        else
            GetGraph4.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get CDRs aggregated by Products in an account. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph5
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/5" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph5.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph5.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph5.Unauthorized(Serializer.deserialize content)
        else
            GetGraph5.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get bytes consumed and aggregated by ICCID in an account. Type Table
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph6
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/6" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph6.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph6.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph6.Unauthorized(Serializer.deserialize content)
        else
            GetGraph6.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get CDRs aggregated by ICCID in an account. Type Table
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph7
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/7" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph7.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph7.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph7.Unauthorized(Serializer.deserialize content)
        else
            GetGraph7.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get bytes consumed and aggregated by account for the current month and the previous month. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph8
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/8" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph8.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph8.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph8.Unauthorized(Serializer.deserialize content)
        else
            GetGraph8.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get CDRs aggregated by account for the current month and the previous month. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph9
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/9" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph9.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph9.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph9.Unauthorized(Serializer.deserialize content)
        else
            GetGraph9.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get stats of products by account for the current month. The period is from the first day of the current month to today. Type Table
    ///</summary>
    member this.GetGraph10(xAccessToken: string, accountId: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/10" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph10.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph10.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph10.Unauthorized(Serializer.deserialize content)
        else
            GetGraph10.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get status of simcards by account. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph11
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/11" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph11.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph11.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph11.Unauthorized(Serializer.deserialize content)
        else
            GetGraph11.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get cost consumed by an account. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraph12
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/12" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraph12.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraph12.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraph12.Unauthorized(Serializer.deserialize content)
        else
            GetGraph12.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get the simcard status per day by an account. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="cancellationToken"></param>
    member this.GetGraphStatusesperday
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId)
              if unit.IsSome then
                  RequestPart.query ("unit", unit.Value)
              if quantity.IsSome then
                  RequestPart.query ("quantity", quantity.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/statusesperday" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetGraphStatusesperday.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetGraphStatusesperday.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetGraphStatusesperday.Unauthorized(Serializer.deserialize content)
        else
            GetGraphStatusesperday.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get components that are not fully operational and are being used in this account
    ///</summary>
    member this.GetStatusComponents(xAccessToken: string, accountId: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/status/components" requestParts cancellationToken

        GetStatusComponents.OK(Serializer.deserialize content)

    ///<summary>
    ///Add a short dial entry
    ///</summary>
    member this.PostAssetsQuickDialByIccid
        (
            payload: PostAssetsQuickDialByIccidPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/quick-dial" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsQuickDialByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAssetsQuickDialByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsQuickDialByIccid.Unauthorized(Serializer.deserialize content)
        else
            PostAssetsQuickDialByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get a list of all quick dial entries for the SIM
    ///</summary>
    member this.GetAssetsQuickDialByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/quick-dial" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssetsQuickDialByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAssetsQuickDialByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssetsQuickDialByIccid.Unauthorized(Serializer.deserialize content)
        else
            GetAssetsQuickDialByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Show an individual quick dial entry in the SIM
    ///</summary>
    member this.GetAssetsQuickDialByIccidAndLocation(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/quick-dial/{location}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetAssetsQuickDialByIccidAndLocation.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            GetAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
        else
            GetAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Update a short dial entry
    ///</summary>
    member this.PutAssetsQuickDialByIccidAndLocation
        (
            payload: PutAssetsQuickDialByIccidAndLocationPayload,
            ?cancellationToken: CancellationToken
        ) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/quick-dial/{location}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PutAssetsQuickDialByIccidAndLocation.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PutAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PutAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
        else
            PutAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Remove a short dial entry. Use "all" as location param if all locations for an user need to be removed.
    ///</summary>
    member this.DeleteAssetsQuickDialByIccidAndLocation(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/assets/{iccid}/quick-dial/{location}" requestParts cancellationToken

        if status = HttpStatusCode.NoContent then
            DeleteAssetsQuickDialByIccidAndLocation.NoContent
        else if status = HttpStatusCode.BadRequest then
            DeleteAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            DeleteAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
        else
            DeleteAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Forces to make a MT voice call to a SIM. This function is used to make the client perform a task when it gets a call.
    ///</summary>
    member this.PostAssetsDialByIccid(payload: PostAssetsDialByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/dial" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PostAssetsDialByIccid.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            PostAssetsDialByIccid.BadRequest(Serializer.deserialize content)
        else if status = HttpStatusCode.Unauthorized then
            PostAssetsDialByIccid.Unauthorized(Serializer.deserialize content)
        else
            PostAssetsDialByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrive the information stored about all steering lists for an account.
    ///</summary>
    member this.GetSteeringlists(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/steeringlists" requestParts cancellationToken

        GetSteeringlists.OK(Serializer.deserialize content)

    ///<summary>
    ///Create a new steering list
    ///</summary>
    member this.PostSteeringlists(payload: PostSteeringlistsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/steeringlists" requestParts cancellationToken

        PostSteeringlists.OK(Serializer.deserialize content)

    ///<summary>
    ///Delete a steering list
    ///</summary>
    member this.DeleteSteeringlistsBySteeringListId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/steeringlists/{steeringListId}" requestParts cancellationToken

        DeleteSteeringlistsBySteeringListId.OK(Serializer.deserialize content)

    ///<summary>
    ///Retrive the information stored about campaigns for an account.
    ///</summary>
    member this.GetCampaigns(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/campaigns" requestParts cancellationToken

        GetCampaigns.OK(Serializer.deserialize content)

    ///<summary>
    ///Create a campaign
    ///</summary>
    member this.PostCampaigns(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/campaigns" requestParts cancellationToken

        PostCampaigns.OK(Serializer.deserialize content)

    ///<summary>
    ///Edit a campaign
    ///</summary>
    member this.PutCampaigns(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/campaigns" requestParts cancellationToken

        PutCampaigns.OK(Serializer.deserialize content)

    ///<summary>
    ///Delete a campaign
    ///</summary>
    member this.DeleteCampaigns(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/campaigns" requestParts cancellationToken

        DeleteCampaigns.OK(Serializer.deserialize content)

    ///<summary>
    ///Get a campaign
    ///</summary>
    member this.GetCampaignsByCampaignId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/campaigns/{campaignId}" requestParts cancellationToken

        GetCampaignsByCampaignId.OK(Serializer.deserialize content)

    ///<summary>
    ///Get campaign completion forecast
    ///</summary>
    member this.PostCampaignsCompletionForecast(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/campaigns/completion-forecast" requestParts cancellationToken

        PostCampaignsCompletionForecast.OK(Serializer.deserialize content)

    ///<summary>
    ///Get items of a campaign
    ///</summary>
    member this.GetCampaignsItemsByCampaignId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/campaigns/{campaignId}/items" requestParts cancellationToken

        GetCampaignsItemsByCampaignId.OK(Serializer.deserialize content)
