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

        match int status with
        | 200 -> PostAuthToken.OK(Serializer.deserialize content)
        | 400 -> PostAuthToken.BadRequest(Serializer.deserialize content)
        | 401 -> PostAuthToken.Unauthorized(Serializer.deserialize content)
        | _ -> PostAuthToken.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostAuthRecoverPassword.OK(Serializer.deserialize content)
        | 400 -> PostAuthRecoverPassword.BadRequest(Serializer.deserialize content)
        | 401 -> PostAuthRecoverPassword.Unauthorized(Serializer.deserialize content)
        | _ -> PostAuthRecoverPassword.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostAuthResetByMailtoken.OK(Serializer.deserialize content)
        | 400 -> PostAuthResetByMailtoken.BadRequest(Serializer.deserialize content)
        | 401 -> PostAuthResetByMailtoken.Unauthorized(Serializer.deserialize content)
        | _ -> PostAuthResetByMailtoken.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostAuthRevokeToken.OK(Serializer.deserialize content)
        | 400 -> PostAuthRevokeToken.BadRequest(Serializer.deserialize content)
        | 401 -> PostAuthRevokeToken.Unauthorized(Serializer.deserialize content)
        | _ -> PostAuthRevokeToken.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostAuthChangePassword.OK(Serializer.deserialize content)
        | 400 -> PostAuthChangePassword.BadRequest(Serializer.deserialize content)
        | 401 -> PostAuthChangePassword.Unauthorized(Serializer.deserialize content)
        | _ -> PostAuthChangePassword.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Creates a new user taking token and new user data. Returns the new user data. An email is sent to the new user with instructions to log in.
    ///</summary>
    member this.PostUsers(xAccessToken: string, payload: PostUsersPayload, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/users" requestParts cancellationToken

        match int status with
        | 200 -> PostUsers.OK(Serializer.deserialize content)
        | 400 -> PostUsers.BadRequest(Serializer.deserialize content)
        | 401 -> PostUsers.Unauthorized(Serializer.deserialize content)
        | _ -> PostUsers.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetUsers.OK(Serializer.deserialize content)
        | 400 -> GetUsers.BadRequest(Serializer.deserialize content)
        | 401 -> GetUsers.Unauthorized(Serializer.deserialize content)
        | _ -> GetUsers.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostUsersbulk.OK(Serializer.deserialize content)
        | 400 -> PostUsersbulk.BadRequest(Serializer.deserialize content)
        | 401 -> PostUsersbulk.Unauthorized(Serializer.deserialize content)
        | _ -> PostUsersbulk.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetUsersMe.OK(Serializer.deserialize content)
        | 400 -> GetUsersMe.BadRequest(Serializer.deserialize content)
        | 401 -> GetUsersMe.Unauthorized(Serializer.deserialize content)
        | _ -> GetUsersMe.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///User accept terms and conditions.
    ///</summary>
    member this.PostUsersMeAcceptTc(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/users/me/accept-tc" requestParts cancellationToken

        match int status with
        | 201 -> PostUsersMeAcceptTc.Created
        | 400 -> PostUsersMeAcceptTc.BadRequest(Serializer.deserialize content)
        | 401 -> PostUsersMeAcceptTc.Unauthorized(Serializer.deserialize content)
        | _ -> PostUsersMeAcceptTc.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetUsersByUserId.OK(Serializer.deserialize content)
        | 400 -> GetUsersByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> GetUsersByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> GetUsersByUserId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutUsersByUserId.OK(Serializer.deserialize content)
        | 400 -> PutUsersByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> PutUsersByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> PutUsersByUserId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> DeleteUsersByUserId.OK(Serializer.deserialize content)
        | 400 -> DeleteUsersByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteUsersByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteUsersByUserId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutUsersChangePasswordByUserId.OK(Serializer.deserialize content)
        | 400 -> PutUsersChangePasswordByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> PutUsersChangePasswordByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> PutUsersChangePasswordByUserId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutUsersFavoritesByUserId.OK(Serializer.deserialize content)
        | 400 -> PutUsersFavoritesByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> PutUsersFavoritesByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> PutUsersFavoritesByUserId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutUsersCustomizationByUserId.OK(Serializer.deserialize content)
        | 400 -> PutUsersCustomizationByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> PutUsersCustomizationByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> PutUsersCustomizationByUserId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutUsersPermissionsByUserId.OK(Serializer.deserialize content)
        | 400 -> PutUsersPermissionsByUserId.BadRequest(Serializer.deserialize content)
        | 401 -> PutUsersPermissionsByUserId.Unauthorized(Serializer.deserialize content)
        | _ -> PutUsersPermissionsByUserId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all notifications of an account.
    ///</summary>
    member this.GetAccountsNotifications(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/notifications" requestParts cancellationToken

        match int status with
        | 200 -> GetAccountsNotifications.OK(Serializer.deserialize content)
        | 400 -> GetAccountsNotifications.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsNotifications.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsNotifications.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Set the notification which id has been given as readed.
    ///</summary>
    member this.PutAccountsNotificationsByNotificationId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/notifications/{notificationId}" requestParts cancellationToken

        match int status with
        | 200 -> PutAccountsNotificationsByNotificationId.OK
        | 400 -> PutAccountsNotificationsByNotificationId.BadRequest(Serializer.deserialize content)
        | 401 -> PutAccountsNotificationsByNotificationId.Unauthorized(Serializer.deserialize content)
        | _ -> PutAccountsNotificationsByNotificationId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete a notification
    ///</summary>
    member this.DeleteAccountsNotificationsByNotificationId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/accounts/notifications/{notificationId}" requestParts cancellationToken

        match int status with
        | 200 -> DeleteAccountsNotificationsByNotificationId.OK(Serializer.deserialize content)
        | 400 -> DeleteAccountsNotificationsByNotificationId.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteAccountsNotificationsByNotificationId.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteAccountsNotificationsByNotificationId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all accounts and subaccounts.
    ///</summary>
    member this.GetAccounts(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts" requestParts cancellationToken

        match int status with
        | 200 -> GetAccounts.OK(Serializer.deserialize content)
        | 400 -> GetAccounts.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccounts.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccounts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create a new child account in the system.
    ///</summary>
    member this.PostAccounts(account: PostAccountsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent account ]

        let (status, content) =
            OpenApiHttp.post httpClient "/accounts" requestParts cancellationToken

        match int status with
        | 200 -> PostAccounts.OK(Serializer.deserialize content)
        | 400 -> PostAccounts.BadRequest(Serializer.deserialize content)
        | 401 -> PostAccounts.Unauthorized(Serializer.deserialize content)
        | _ -> PostAccounts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all accounts and subaccounts.
    ///</summary>
    member this.PostAccountsbulk(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.post httpClient "/accountsbulk" requestParts cancellationToken

        match int status with
        | 200 -> PostAccountsbulk.OK(Serializer.deserialize content)
        | 400 -> PostAccountsbulk.BadRequest(Serializer.deserialize content)
        | 401 -> PostAccountsbulk.Unauthorized(Serializer.deserialize content)
        | _ -> PostAccountsbulk.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieve one account matching the id provided.
    ///</summary>
    member this.GetAccountsByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}" requestParts cancellationToken

        match int status with
        | 200 -> GetAccountsByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Modify an existing account with the new data setted into the parameters.
    ///</summary>
    member this.PutAccountsByAccountId(account: PutAccountsByAccountIdPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent account ]

        let (status, content) =
            OpenApiHttp.put httpClient "/accounts/{accountId}" requestParts cancellationToken

        match int status with
        | 200 -> PutAccountsByAccountId.OK(Serializer.deserialize content)
        | 400 -> PutAccountsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> PutAccountsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> PutAccountsByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> DeleteAccountsByAccountId.OK(Serializer.deserialize content)
        | 400 -> DeleteAccountsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteAccountsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteAccountsByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAccountsBrandingByAccountId.OK(Serializer.deserialize content)
        | 400 -> PutAccountsBrandingByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> PutAccountsBrandingByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> PutAccountsBrandingByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get Account Branding status.
    ///</summary>
    member this.GetAccountsBrandingVerifyByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/branding/verify" requestParts cancellationToken

        match int status with
        | 200 -> GetAccountsBrandingVerifyByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsBrandingVerifyByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsBrandingVerifyByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsBrandingVerifyByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Retrieve all existing roles.
    ///</summary>
    member this.GetAccountsRolesByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/roles" requestParts cancellationToken

        match int status with
        | 200 -> GetAccountsRolesByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsRolesByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsRolesByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsRolesByAccountId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all actions belonging a role.
    ///</summary>
    member this.GetAccountsRolesActionsByAccountIdAndRolename(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/roles/{rolename}/actions" requestParts cancellationToken

        match int status with
        | 200 -> GetAccountsRolesActionsByAccountIdAndRolename.OK(Serializer.deserialize content)
        | 400 -> GetAccountsRolesActionsByAccountIdAndRolename.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsRolesActionsByAccountIdAndRolename.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsRolesActionsByAccountIdAndRolename.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get all notifications of an account.
    ///</summary>
    member this.GetAccountsNotificationsByAccountId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/accounts/{accountId}/notifications" requestParts cancellationToken

        match int status with
        | 200 -> GetAccountsNotificationsByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsNotificationsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsNotificationsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsNotificationsByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAccountsNotificationsByAccountIdAndNotificationId.OK
        | 400 -> PutAccountsNotificationsByAccountIdAndNotificationId.BadRequest(Serializer.deserialize content)
        | 401 -> PutAccountsNotificationsByAccountIdAndNotificationId.Unauthorized(Serializer.deserialize content)
        | _ -> PutAccountsNotificationsByAccountIdAndNotificationId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> DeleteAccountsNotificationsByAccountIdAndNotificationId.OK
        | 400 -> DeleteAccountsNotificationsByAccountIdAndNotificationId.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteAccountsNotificationsByAccountIdAndNotificationId.Unauthorized(Serializer.deserialize content)
        | _ ->
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

        match int status with
        | 200 -> GetAccountsProductsByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsProductsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsProductsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsProductsByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAccountsProductsAlerts.OK
        | 400 -> PutAccountsProductsAlerts.BadRequest
        | 401 -> PutAccountsProductsAlerts.Unauthorized
        | _ -> PutAccountsProductsAlerts.InternalServerError

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

        match int status with
        | 200 -> PutAccountsTopupDirectByAccountId.OK(Serializer.deserialize content)
        | 400 -> PutAccountsTopupDirectByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> PutAccountsTopupDirectByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> PutAccountsTopupDirectByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetAccountsSecuritySettingsAvailableGapsByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsSecuritySettingsAvailableGapsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsSecuritySettingsAvailableGapsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsSecuritySettingsAvailableGapsByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetAccountsSecuritySettingsAvailableIpsByAccountId.OK(Serializer.deserialize content)
        | 400 -> GetAccountsSecuritySettingsAvailableIpsByAccountId.BadRequest(Serializer.deserialize content)
        | 401 -> GetAccountsSecuritySettingsAvailableIpsByAccountId.Unauthorized(Serializer.deserialize content)
        | _ -> GetAccountsSecuritySettingsAvailableIpsByAccountId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostMail.OK(Serializer.deserialize content)
        | 400 -> PostMail.BadRequest(Serializer.deserialize content)
        | 401 -> PostMail.Unauthorized(Serializer.deserialize content)
        | _ -> PostMail.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetProducts.OK(Serializer.deserialize content)
        | 400 -> GetProducts.BadRequest
        | 401 -> GetProducts.Unauthorized
        | 403 -> GetProducts.Forbidden
        | 500 -> GetProducts.InternalServerError
        | _ -> GetProducts.ServiceUnavailable

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

        match int status with
        | 200 -> PostProducts.OK(Serializer.deserialize content)
        | 400 -> PostProducts.BadRequest(Serializer.deserialize content)
        | 401 -> PostProducts.Unauthorized(Serializer.deserialize content)
        | _ -> PostProducts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get a product of an account
    ///</summary>
    member this.GetProductsByProductId(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/products/{productId}" requestParts cancellationToken

        match int status with
        | 200 -> GetProductsByProductId.OK(Serializer.deserialize content)
        | 400 -> GetProductsByProductId.BadRequest
        | 401 -> GetProductsByProductId.Unauthorized
        | 403 -> GetProductsByProductId.Forbidden
        | 500 -> GetProductsByProductId.InternalServerError
        | _ -> GetProductsByProductId.ServiceUnavailable

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

        match int status with
        | 200 -> PatchProductsByProductId.OK(Serializer.deserialize content)
        | 400 -> PatchProductsByProductId.BadRequest(Serializer.deserialize content)
        | 401 -> PatchProductsByProductId.Unauthorized(Serializer.deserialize content)
        | _ -> PatchProductsByProductId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete product by ID
    ///</summary>
    member this.DeleteProductsByProductId(xAccessToken: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/products/{productId}" requestParts cancellationToken

        match int status with
        | 200 -> DeleteProductsByProductId.OK(Serializer.deserialize content)
        | 400 -> DeleteProductsByProductId.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteProductsByProductId.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteProductsByProductId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetSchemaProduct.OK
        | 400 -> GetSchemaProduct.BadRequest(Serializer.deserialize content)
        | 401 -> GetSchemaProduct.Unauthorized(Serializer.deserialize content)
        | _ -> GetSchemaProduct.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostProductsTransferByProductId.OK(Serializer.deserialize content)
        | 400 -> PostProductsTransferByProductId.BadRequest
        | 401 -> PostProductsTransferByProductId.Unauthorized
        | 403 -> PostProductsTransferByProductId.Forbidden
        | 500 -> PostProductsTransferByProductId.InternalServerError
        | _ -> PostProductsTransferByProductId.ServiceUnavailable

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

        match int status with
        | 200 -> GetCdr.OK(Serializer.deserialize content)
        | 400 -> GetCdr.BadRequest(Serializer.deserialize content)
        | 401 -> GetCdr.Unauthorized(Serializer.deserialize content)
        | 404 -> GetCdr.NotFound(Serializer.deserialize content)
        | _ -> GetCdr.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetAssets.OK(Serializer.deserialize content)
        | 400 -> GetAssets.BadRequest(Serializer.deserialize content)
        | 401 -> GetAssets.Unauthorized(Serializer.deserialize content)
        | _ -> GetAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create a new Asset
    ///</summary>
    member this.PostAssets(payload: PostAssetsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets" requestParts cancellationToken

        match int status with
        | 200 -> PostAssets.OK(Serializer.deserialize content)
        | 400 -> PostAssets.BadRequest(Serializer.deserialize content)
        | 401 -> PostAssets.Unauthorized(Serializer.deserialize content)
        | _ -> PostAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Assets card Info (bulk)
    ///</summary>
    member this.PostAssetsbulk(payload: PostAssetsbulkPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assetsbulk" requestParts cancellationToken

        match int status with
        | 200 -> PostAssetsbulk.OK(Serializer.deserialize content)
        | 400 -> PostAssetsbulk.BadRequest(Serializer.deserialize content)
        | 401 -> PostAssetsbulk.Unauthorized(Serializer.deserialize content)
        | _ -> PostAssetsbulk.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Assets get Info
    ///</summary>
    member this.GetAssetsByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}" requestParts cancellationToken

        match int status with
        | 200 -> GetAssetsByIccid.OK(Serializer.deserialize content)
        | 400 -> GetAssetsByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> GetAssetsByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> GetAssetsByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Change the name of the asset for the specified accountId
    ///</summary>
    member this.PutAssetsByIccid(asset: PutAssetsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent asset ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}" requestParts cancellationToken

        match int status with
        | 200 -> PutAssetsByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Remove an Asset
    ///</summary>
    member this.DeleteAssetsByIccid(asset: DeleteAssetsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent asset ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/assets/{iccid}" requestParts cancellationToken

        match int status with
        | 200 -> DeleteAssetsByIccid.OK(Serializer.deserialize content)
        | 400 -> DeleteAssetsByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteAssetsByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteAssetsByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsGroupnameByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsGroupnameByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsGroupnameByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsGroupnameByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostAssetsTransferByIccid.OK(Serializer.deserialize content)
        | 400 -> PostAssetsTransferByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PostAssetsTransferByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PostAssetsTransferByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsSubscribeByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsSubscribeByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsSubscribeByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsSubscribeByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsUnsubscribeByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsUnsubscribeByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsUnsubscribeByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsUnsubscribeByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsResubscribeByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsResubscribeByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsResubscribeByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsResubscribeByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsSuspendByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsSuspendByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsSuspendByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsSuspendByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsUnsuspendByIccid.OK(Serializer.deserialize content)
        | 400 -> PutAssetsUnsuspendByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsUnsuspendByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsUnsuspendByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutAssetsAlertsByIccid(payload: PutAssetsAlertsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.put httpClient "/assets/{iccid}/alerts" requestParts cancellationToken

        match int status with
        | 200 -> PutAssetsAlertsByIccid.OK
        | 400 -> PutAssetsAlertsByIccid.BadRequest
        | 401 -> PutAssetsAlertsByIccid.Unauthorized
        | _ -> PutAssetsAlertsByIccid.InternalServerError

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostAssetsPurgeByIccid(payload: PostAssetsPurgeByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/purge" requestParts cancellationToken

        match int status with
        | 200 -> PostAssetsPurgeByIccid.OK
        | 400 -> PostAssetsPurgeByIccid.BadRequest
        | 401 -> PostAssetsPurgeByIccid.Unauthorized
        | _ -> PostAssetsPurgeByIccid.InternalServerError

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostAssetsSmsByIccid(message: PostAssetsSmsByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent message ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/sms" requestParts cancellationToken

        match int status with
        | 200 -> PostAssetsSmsByIccid.OK
        | 400 -> PostAssetsSmsByIccid.BadRequest
        | 401 -> PostAssetsSmsByIccid.Unauthorized
        | _ -> PostAssetsSmsByIccid.InternalServerError

    ///<summary>
    ///Set limits for a simcard. You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostAssetsLimitByIccid(message: PostAssetsLimitByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent message ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/limit" requestParts cancellationToken

        match int status with
        | 200 -> PostAssetsLimitByIccid.OK
        | 400 -> PostAssetsLimitByIccid.BadRequest
        | 401 -> PostAssetsLimitByIccid.Unauthorized
        | _ -> PostAssetsLimitByIccid.InternalServerError

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

        match int status with
        | 200 -> PostAssetsTagsByIccid.OK
        | 400 -> PostAssetsTagsByIccid.BadRequest
        | 401 -> PostAssetsTagsByIccid.Unauthorized
        | _ -> PostAssetsTagsByIccid.InternalServerError

    ///<summary>
    ///Checks the simcard status over the network, if the sim is correctly provisioned in the system, last connection, last data transmision, if it is online etc...
    ///</summary>
    member this.GetAssetsDiagnosticByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/diagnostic" requestParts cancellationToken

        match int status with
        | 200 -> GetAssetsDiagnosticByIccid.OK(Serializer.deserialize content)
        | 401 -> GetAssetsDiagnosticByIccid.Unauthorized
        | 500 -> GetAssetsDiagnosticByIccid.InternalServerError
        | _ -> GetAssetsDiagnosticByIccid.ServiceUnavailable

    ///<summary>
    ///Return assets location.
    ///</summary>
    member this.GetAssetsLocationByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/location" requestParts cancellationToken

        match int status with
        | 200 -> GetAssetsLocationByIccid.OK(Serializer.deserialize content)
        | 400 -> GetAssetsLocationByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> GetAssetsLocationByIccid.Unauthorized(Serializer.deserialize content)
        | 404 -> GetAssetsLocationByIccid.NotFound(Serializer.deserialize content)
        | _ -> GetAssetsLocationByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Return asset sessions.
    ///</summary>
    member this.GetAssetsSessionsByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/sessions" requestParts cancellationToken

        match int status with
        | 200 -> GetAssetsSessionsByIccid.OK(Serializer.deserialize content)
        | 400 -> GetAssetsSessionsByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> GetAssetsSessionsByIccid.Unauthorized(Serializer.deserialize content)
        | 404 -> GetAssetsSessionsByIccid.NotFound(Serializer.deserialize content)
        | _ -> GetAssetsSessionsByIccid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetReportsCustom.OK(Serializer.deserialize content)
        | 401 -> GetReportsCustom.Unauthorized(Serializer.deserialize content)
        | _ -> GetReportsCustom.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create a new custom report. &amp;lt;/br&amp;gt; Date filtering supports dates as `YYYY-MM-DDTHH:mm:SSZ`, for example `"2020-04-01T00:59:59Z"` &amp;lt;/br&amp;gt; The fileds requires are; accountId, dateFrom, dateTo
    ///</summary>
    member this.PostReportsCustom(report: FilterCustom, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent report ]

        let (status, content) =
            OpenApiHttp.post httpClient "/reports/custom" requestParts cancellationToken

        match int status with
        | 200 -> PostReportsCustom.OK
        | 401 -> PostReportsCustom.Unauthorized(Serializer.deserialize content)
        | 403 -> PostReportsCustom.Forbidden(Serializer.deserialize content)
        | 404 -> PostReportsCustom.NotFound(Serializer.deserialize content)
        | _ -> PostReportsCustom.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete all reports from an account
    ///</summary>
    member this.DeleteReportsCustom(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/reports/custom" requestParts cancellationToken

        match int status with
        | 204 -> DeleteReportsCustom.NoContent
        | 401 -> DeleteReportsCustom.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteReportsCustom.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get one reports from an account
    ///</summary>
    member this.GetReportsCustomByReportId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/reports/custom/{reportId}" requestParts cancellationToken

        match int status with
        | 200 -> GetReportsCustomByReportId.OK(Serializer.deserialize content)
        | 401 -> GetReportsCustomByReportId.Unauthorized(Serializer.deserialize content)
        | 404 -> GetReportsCustomByReportId.NotFound(Serializer.deserialize content)
        | _ -> GetReportsCustomByReportId.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Delete one report from an account
    ///</summary>
    member this.DeleteReportsCustomByReportId(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/reports/custom/{reportId}" requestParts cancellationToken

        match int status with
        | 204 -> DeleteReportsCustomByReportId.NoContent
        | 401 -> DeleteReportsCustomByReportId.Unauthorized(Serializer.deserialize content)
        | 404 -> DeleteReportsCustomByReportId.NotFound(Serializer.deserialize content)
        | _ -> DeleteReportsCustomByReportId.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetEvents.OK(Serializer.deserialize content)
        | 400 -> GetEvents.BadRequest(Serializer.deserialize content)
        | 401 -> GetEvents.Unauthorized(Serializer.deserialize content)
        | _ -> GetEvents.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 202 -> PostBulkAssetsSubscribe.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsSubscribe.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsSubscribe.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsSubscribe.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Transfer a Asset from a account to another in bulk
    ///</summary>
    member this.PostBulkAssetsTransfer(body: PostBulkAssetsTransferPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/transfer" requestParts cancellationToken

        match int status with
        | 202 -> PostBulkAssetsTransfer.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsTransfer.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsTransfer.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsTransfer.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Return a Asset from a account to parent account in bulk
    ///</summary>
    member this.PostBulkAssetsReturn(body: PostBulkAssetsReturnPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/return" requestParts cancellationToken

        match int status with
        | 202 -> PostBulkAssetsReturn.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsReturn.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsReturn.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsReturn.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Suspend assets in a massive way by using different filters
    ///</summary>
    member this.PutBulkAssetsSuspend(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/suspend" requestParts cancellationToken

        match int status with
        | 202 -> PutBulkAssetsSuspend.Accepted(Serializer.deserialize content)
        | 400 -> PutBulkAssetsSuspend.BadRequest(Serializer.deserialize content)
        | 401 -> PutBulkAssetsSuspend.Unauthorized(Serializer.deserialize content)
        | _ -> PutBulkAssetsSuspend.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///This endpoint change in a massive way the suspend status of the assets which match with the filter setted.
    ///</summary>
    member this.PutBulkAssetsUnsuspend(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/unsuspend" requestParts cancellationToken

        match int status with
        | 202 -> PutBulkAssetsUnsuspend.Accepted(Serializer.deserialize content)
        | 400 -> PutBulkAssetsUnsuspend.BadRequest(Serializer.deserialize content)
        | 401 -> PutBulkAssetsUnsuspend.Unauthorized(Serializer.deserialize content)
        | _ -> PutBulkAssetsUnsuspend.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 202 -> PostBulkAssetsResubscribe.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsResubscribe.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsResubscribe.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsResubscribe.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Create Assets in bulk mode
    ///</summary>
    member this.PostBulkAssets(body: PostBulkAssetsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets" requestParts cancellationToken

        match int status with
        | 202 -> PostBulkAssets.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssets.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssets.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Update Assets in bulk mode
    ///</summary>
    member this.PutBulkAssets(body: PutBulkAssetsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets" requestParts cancellationToken

        match int status with
        | 202 -> PutBulkAssets.Accepted(Serializer.deserialize content)
        | 400 -> PutBulkAssets.BadRequest(Serializer.deserialize content)
        | 401 -> PutBulkAssets.Unauthorized(Serializer.deserialize content)
        | _ -> PutBulkAssets.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Update group name of Assets in bulk mode
    ///</summary>
    member this.PutBulkAssetsGroupname(body: PutBulkAssetsGroupnamePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/groupname" requestParts cancellationToken

        match int status with
        | 202 -> PutBulkAssetsGroupname.Accepted(Serializer.deserialize content)
        | 400 -> PutBulkAssetsGroupname.BadRequest(Serializer.deserialize content)
        | 401 -> PutBulkAssetsGroupname.Unauthorized(Serializer.deserialize content)
        | _ -> PutBulkAssetsGroupname.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostBulkAssetsLimit(body: PostBulkAssetsLimitPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/limit" requestParts cancellationToken

        match int status with
        | 202 -> PostBulkAssetsLimit.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsLimit.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsLimit.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsLimit.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///- A maximum of 3 alerts per SIM can be set in order to notify the user when the consumption of a  promotion  reaches or exceeds a certain limit, Alerts can also be used to notify the user of usage over their predefined bundle amount.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set only.
    ///</summary>
    member this.PutBulkAssetsAlerts(body: PutBulkAssetsAlertsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/bulk/assets/alerts" requestParts cancellationToken

        match int status with
        | 202 -> PutBulkAssetsAlerts.Accepted(Serializer.deserialize content)
        | 400 -> PutBulkAssetsAlerts.BadRequest(Serializer.deserialize content)
        | 401 -> PutBulkAssetsAlerts.Unauthorized(Serializer.deserialize content)
        | _ -> PutBulkAssetsAlerts.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostBulkAssetsSms(body: PostBulkAssetsSmsPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/sms" requestParts cancellationToken

        match int status with
        | 202 -> PostBulkAssetsSms.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsSms.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsSms.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsSms.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostBulkAssetsPurge(body: PostBulkAssetsPurgePayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/bulk/assets/purge" requestParts cancellationToken

        match int status with
        | 202 -> PostBulkAssetsPurge.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsPurge.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsPurge.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsPurge.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 202 -> PostBulkAssetsReallocateIp.Accepted(Serializer.deserialize content)
        | 400 -> PostBulkAssetsReallocateIp.BadRequest(Serializer.deserialize content)
        | 401 -> PostBulkAssetsReallocateIp.Unauthorized(Serializer.deserialize content)
        | _ -> PostBulkAssetsReallocateIp.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostPaymentsTopupPaypal.OK(Serializer.deserialize content)
        | 400 -> PostPaymentsTopupPaypal.BadRequest(Serializer.deserialize content)
        | 401 -> PostPaymentsTopupPaypal.Unauthorized(Serializer.deserialize content)
        | _ -> PostPaymentsTopupPaypal.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostPaymentsConfirmTopupPaypal.OK(Serializer.deserialize content)
        | 400 -> PostPaymentsConfirmTopupPaypal.BadRequest(Serializer.deserialize content)
        | 401 -> PostPaymentsConfirmTopupPaypal.Unauthorized(Serializer.deserialize content)
        | _ -> PostPaymentsConfirmTopupPaypal.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetSecurityAlerts.OK(Serializer.deserialize content)
        | 400 -> GetSecurityAlerts.BadRequest
        | 401 -> GetSecurityAlerts.Unauthorized
        | 403 -> GetSecurityAlerts.Forbidden
        | 500 -> GetSecurityAlerts.InternalServerError
        | _ -> GetSecurityAlerts.ServiceUnavailable

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

        match int status with
        | 200 -> GetSecurityAlertsByAlertId.OK(Serializer.deserialize content)
        | 400 -> GetSecurityAlertsByAlertId.BadRequest
        | 401 -> GetSecurityAlertsByAlertId.Unauthorized
        | 403 -> GetSecurityAlertsByAlertId.Forbidden
        | 500 -> GetSecurityAlertsByAlertId.InternalServerError
        | _ -> GetSecurityAlertsByAlertId.ServiceUnavailable

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

        match int status with
        | 200 -> PutSecurityAlertsByAlertId.OK(Serializer.deserialize content)
        | 400 -> PutSecurityAlertsByAlertId.BadRequest
        | 401 -> PutSecurityAlertsByAlertId.Unauthorized
        | 403 -> PutSecurityAlertsByAlertId.Forbidden
        | 500 -> PutSecurityAlertsByAlertId.InternalServerError
        | _ -> PutSecurityAlertsByAlertId.ServiceUnavailable

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

        match int status with
        | 204 -> DeleteSecurityAlertsByAlertId.NoContent
        | 400 -> DeleteSecurityAlertsByAlertId.BadRequest
        | 401 -> DeleteSecurityAlertsByAlertId.Unauthorized
        | 403 -> DeleteSecurityAlertsByAlertId.Forbidden
        | 500 -> DeleteSecurityAlertsByAlertId.InternalServerError
        | _ -> DeleteSecurityAlertsByAlertId.ServiceUnavailable

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

        match int status with
        | 200 -> GetGraphSecurityTopthreaten.OK(Serializer.deserialize content)
        | 400 -> GetGraphSecurityTopthreaten.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraphSecurityTopthreaten.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraphSecurityTopthreaten.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraphSecurityByalarmtype.OK(Serializer.deserialize content)
        | 400 -> GetGraphSecurityByalarmtype.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraphSecurityByalarmtype.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraphSecurityByalarmtype.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostEsims.OK(Serializer.deserialize content)
        | 400 -> PostEsims.BadRequest(Serializer.deserialize content)
        | 401 -> PostEsims.Unauthorized(Serializer.deserialize content)
        | _ -> PostEsims.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostEsimsTransferByEid.OK(Serializer.deserialize content)
        | 400 -> PostEsimsTransferByEid.BadRequest(Serializer.deserialize content)
        | 401 -> PostEsimsTransferByEid.Unauthorized(Serializer.deserialize content)
        | _ -> PostEsimsTransferByEid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Return a ESim from a account to parent account
    ///</summary>
    member this.PostEsimsReturnByEid(payload: PostEsimsReturnByEidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/esims/{eid}/return" requestParts cancellationToken

        match int status with
        | 200 -> PostEsimsReturnByEid.OK(Serializer.deserialize content)
        | 400 -> PostEsimsReturnByEid.BadRequest(Serializer.deserialize content)
        | 401 -> PostEsimsReturnByEid.Unauthorized(Serializer.deserialize content)
        | _ -> PostEsimsReturnByEid.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph1.OK(Serializer.deserialize content)
        | 400 -> GetGraph1.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph1.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph1.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph2.OK(Serializer.deserialize content)
        | 400 -> GetGraph2.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph2.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph2.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph3.OK(Serializer.deserialize content)
        | 400 -> GetGraph3.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph3.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph3.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph4.OK(Serializer.deserialize content)
        | 400 -> GetGraph4.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph4.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph4.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph5.OK(Serializer.deserialize content)
        | 400 -> GetGraph5.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph5.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph5.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph6.OK(Serializer.deserialize content)
        | 400 -> GetGraph6.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph6.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph6.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph7.OK(Serializer.deserialize content)
        | 400 -> GetGraph7.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph7.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph7.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph8.OK(Serializer.deserialize content)
        | 400 -> GetGraph8.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph8.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph8.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph9.OK(Serializer.deserialize content)
        | 400 -> GetGraph9.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph9.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph9.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get stats of products by account for the current month. The period is from the first day of the current month to today. Type Table
    ///</summary>
    member this.GetGraph10(xAccessToken: string, accountId: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.header ("x-access-token", xAccessToken)
              RequestPart.query ("accountId", accountId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/graph/10" requestParts cancellationToken

        match int status with
        | 200 -> GetGraph10.OK(Serializer.deserialize content)
        | 400 -> GetGraph10.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph10.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph10.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph11.OK(Serializer.deserialize content)
        | 400 -> GetGraph11.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph11.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph11.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraph12.OK(Serializer.deserialize content)
        | 400 -> GetGraph12.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraph12.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraph12.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> GetGraphStatusesperday.OK(Serializer.deserialize content)
        | 400 -> GetGraphStatusesperday.BadRequest(Serializer.deserialize content)
        | 401 -> GetGraphStatusesperday.Unauthorized(Serializer.deserialize content)
        | _ -> GetGraphStatusesperday.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PostAssetsQuickDialByIccid.OK(Serializer.deserialize content)
        | 400 -> PostAssetsQuickDialByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PostAssetsQuickDialByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PostAssetsQuickDialByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Get a list of all quick dial entries for the SIM
    ///</summary>
    member this.GetAssetsQuickDialByIccid(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/quick-dial" requestParts cancellationToken

        match int status with
        | 200 -> GetAssetsQuickDialByIccid.OK(Serializer.deserialize content)
        | 400 -> GetAssetsQuickDialByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> GetAssetsQuickDialByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> GetAssetsQuickDialByIccid.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Show an individual quick dial entry in the SIM
    ///</summary>
    member this.GetAssetsQuickDialByIccidAndLocation(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/assets/{iccid}/quick-dial/{location}" requestParts cancellationToken

        match int status with
        | 200 -> GetAssetsQuickDialByIccidAndLocation.OK(Serializer.deserialize content)
        | 400 -> GetAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
        | 401 -> GetAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
        | _ -> GetAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)

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

        match int status with
        | 200 -> PutAssetsQuickDialByIccidAndLocation.OK(Serializer.deserialize content)
        | 400 -> PutAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
        | 401 -> PutAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
        | _ -> PutAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Remove a short dial entry. Use "all" as location param if all locations for an user need to be removed.
    ///</summary>
    member this.DeleteAssetsQuickDialByIccidAndLocation(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.delete httpClient "/assets/{iccid}/quick-dial/{location}" requestParts cancellationToken

        match int status with
        | 204 -> DeleteAssetsQuickDialByIccidAndLocation.NoContent
        | 400 -> DeleteAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
        | 401 -> DeleteAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
        | _ -> DeleteAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)

    ///<summary>
    ///Forces to make a MT voice call to a SIM. This function is used to make the client perform a task when it gets a call.
    ///</summary>
    member this.PostAssetsDialByIccid(payload: PostAssetsDialByIccidPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent payload ]

        let (status, content) =
            OpenApiHttp.post httpClient "/assets/{iccid}/dial" requestParts cancellationToken

        match int status with
        | 200 -> PostAssetsDialByIccid.OK(Serializer.deserialize content)
        | 400 -> PostAssetsDialByIccid.BadRequest(Serializer.deserialize content)
        | 401 -> PostAssetsDialByIccid.Unauthorized(Serializer.deserialize content)
        | _ -> PostAssetsDialByIccid.InternalServerError(Serializer.deserialize content)

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
