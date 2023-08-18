namespace rec FablePodiosuite

open Browser.Types
open Fable.SimpleHttp
open FablePodiosuite.Types
open FablePodiosuite.Http

type FablePodiosuiteClient(url: string, headers: list<Header>) =
    new(url: string) = FablePodiosuiteClient(url, [])

    ///<summary>
    ///Takes username and password and if it finds match in back-end it returns an object with the user data and token used to authorize the login.
    ///</summary>
    member this.PostAuthToken(payload: PostAuthTokenPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/auth/token" headers requestParts

            match int status with
            | 200 -> return PostAuthToken.OK(Serializer.deserialize content)
            | 400 -> return PostAuthToken.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAuthToken.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAuthToken.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Takes username or email as values and sends an email to the user with instructions to reset the password, including a tokenized URL that directs the user to the password reset form.
    ///</summary>
    member this.PostAuthRecoverPassword(payload: PostAuthRecoverPasswordPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/auth/recover-password" headers requestParts

            match int status with
            | 200 -> return PostAuthRecoverPassword.OK(Serializer.deserialize content)
            | 400 -> return PostAuthRecoverPassword.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAuthRecoverPassword.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAuthRecoverPassword.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Takes token session sent in email and resets the password for the active user.
    ///</summary>
    ///<param name="mailtoken">A valid token recived via email.</param>
    ///<param name="payload"></param>
    member this.PostAuthResetByMailtoken(mailtoken: string, payload: PostAuthResetByMailtokenPayload) =
        async {
            let requestParts =
                [ RequestPart.path ("mailtoken", mailtoken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/auth/reset/{mailtoken}" headers requestParts

            match int status with
            | 200 -> return PostAuthResetByMailtoken.OK(Serializer.deserialize content)
            | 400 -> return PostAuthResetByMailtoken.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAuthResetByMailtoken.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAuthResetByMailtoken.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Revokes the active session for the user by disabling the token. This disables the different sessions opened on differents computers and browsers.&amp;lt;br /&amp;gt; The user will login again to use the App, and a new token will be generate.
    ///</summary>
    member this.PostAuthRevokeToken(xAccessToken: string, payload: PostAuthRevokeTokenPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/auth/revoke-token" headers requestParts

            match int status with
            | 200 -> return PostAuthRevokeToken.OK(Serializer.deserialize content)
            | 400 -> return PostAuthRevokeToken.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAuthRevokeToken.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAuthRevokeToken.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Changes the user’s password and resets the session token.
    ///</summary>
    member this.PostAuthChangePassword(xAccessToken: string, payload: PostAuthChangePasswordPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/auth/change-password" headers requestParts

            match int status with
            | 200 -> return PostAuthChangePassword.OK(Serializer.deserialize content)
            | 400 -> return PostAuthChangePassword.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAuthChangePassword.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAuthChangePassword.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Creates a new user taking token and new user data. Returns the new user data. An email is sent to the new user with instructions to log in.
    ///</summary>
    member this.PostUsers(xAccessToken: string, payload: PostUsersPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/users" headers requestParts

            match int status with
            | 200 -> return PostUsers.OK(Serializer.deserialize content)
            | 400 -> return PostUsers.BadRequest(Serializer.deserialize content)
            | 401 -> return PostUsers.Unauthorized(Serializer.deserialize content)
            | _ -> return PostUsers.InternalServerError(Serializer.deserialize content)
        }

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
            ?order: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/users" headers requestParts

            match int status with
            | 200 -> return GetUsers.OK(Serializer.deserialize content)
            | 400 -> return GetUsers.BadRequest(Serializer.deserialize content)
            | 401 -> return GetUsers.Unauthorized(Serializer.deserialize content)
            | _ -> return GetUsers.InternalServerError(Serializer.deserialize content)
        }

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
            ?order: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.postAsync url "/usersbulk" headers requestParts

            match int status with
            | 200 -> return PostUsersbulk.OK(Serializer.deserialize content)
            | 400 -> return PostUsersbulk.BadRequest(Serializer.deserialize content)
            | 401 -> return PostUsersbulk.Unauthorized(Serializer.deserialize content)
            | _ -> return PostUsersbulk.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Takes token and returns user data.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    member this.GetUsersMe(xAccessToken: string, accountId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/users/me" headers requestParts

            match int status with
            | 200 -> return GetUsersMe.OK(Serializer.deserialize content)
            | 400 -> return GetUsersMe.BadRequest(Serializer.deserialize content)
            | 401 -> return GetUsersMe.Unauthorized(Serializer.deserialize content)
            | _ -> return GetUsersMe.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///User accept terms and conditions.
    ///</summary>
    member this.PostUsersMeAcceptTc(xAccessToken: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken) ]

            let! (status, content) = OpenApiHttp.postAsync url "/users/me/accept-tc" headers requestParts

            match int status with
            | 201 -> return PostUsersMeAcceptTc.Created
            | 400 -> return PostUsersMeAcceptTc.BadRequest(Serializer.deserialize content)
            | 401 -> return PostUsersMeAcceptTc.Unauthorized(Serializer.deserialize content)
            | _ -> return PostUsersMeAcceptTc.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Gets a user by ID, using token, userID and accountID as parameters, and returns a JSON with the users data.
    ///</summary>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="userId">user ID.</param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    member this.GetUsersByUserId(xAccessToken: string, userId: string, accountId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.path ("userId", userId)
                  RequestPart.query ("accountId", accountId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/users/{userId}" headers requestParts

            match int status with
            | 200 -> return GetUsersByUserId.OK(Serializer.deserialize content)
            | 400 -> return GetUsersByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return GetUsersByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return GetUsersByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Modify a user using the parameters available.
    ///</summary>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="userId">user ID.</param>
    ///<param name="payload"></param>
    member this.PutUsersByUserId(xAccessToken: string, userId: string, payload: PutUsersByUserIdPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.path ("userId", userId)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.putAsync url "/users/{userId}" headers requestParts

            match int status with
            | 200 -> return PutUsersByUserId.OK(Serializer.deserialize content)
            | 400 -> return PutUsersByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutUsersByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutUsersByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Takes token and user ID, then removes user.
    ///</summary>
    ///<param name="userId">User ID</param>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="payload"></param>
    member this.DeleteUsersByUserId(userId: string, xAccessToken: string, payload: DeleteUsersByUserIdPayload) =
        async {
            let requestParts =
                [ RequestPart.path ("userId", userId)
                  RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/users/{userId}" headers requestParts

            match int status with
            | 200 -> return DeleteUsersByUserId.OK(Serializer.deserialize content)
            | 400 -> return DeleteUsersByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return DeleteUsersByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteUsersByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Change a user password using the parameters available.
    ///</summary>
    ///<param name="xAccessToken">Active session token</param>
    ///<param name="payload"></param>
    member this.PutUsersChangePasswordByUserId(xAccessToken: string, payload: PutUsersChangePasswordByUserIdPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.putAsync url "/users/{userId}/change-password" headers requestParts

            match int status with
            | 200 -> return PutUsersChangePasswordByUserId.OK(Serializer.deserialize content)
            | 400 -> return PutUsersChangePasswordByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutUsersChangePasswordByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutUsersChangePasswordByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Modify the user options for favorites using the parameters available.
    ///</summary>
    member this.PutUsersFavoritesByUserId
        (
            xAccessToken: string,
            userId: string,
            payload: PutUsersFavoritesByUserIdPayload
        ) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.path ("userId", userId)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.putAsync url "/users/{userId}/favorites" headers requestParts

            match int status with
            | 200 -> return PutUsersFavoritesByUserId.OK(Serializer.deserialize content)
            | 400 -> return PutUsersFavoritesByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutUsersFavoritesByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutUsersFavoritesByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Modify the user customization options. Modify the setup of the columns as displayed in tables in the user interface and the sort order.
    ///</summary>
    member this.PutUsersCustomizationByUserId
        (
            xAccessToken: string,
            userId: string,
            payload: PutUsersCustomizationByUserIdPayload
        ) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.path ("userId", userId)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.putAsync url "/users/{userId}/customization" headers requestParts

            match int status with
            | 200 -> return PutUsersCustomizationByUserId.OK(Serializer.deserialize content)
            | 400 -> return PutUsersCustomizationByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutUsersCustomizationByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutUsersCustomizationByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Gell all users actions for an account.
    ///</summary>
    member this.PutUsersPermissionsByUserId(xAccessToken: string, payload: PutUsersPermissionsByUserIdPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.putAsync url "/users/{userId}/permissions" headers requestParts

            match int status with
            | 200 -> return PutUsersPermissionsByUserId.OK(Serializer.deserialize content)
            | 400 -> return PutUsersPermissionsByUserId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutUsersPermissionsByUserId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutUsersPermissionsByUserId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all notifications of an account.
    ///</summary>
    member this.GetAccountsNotifications() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/accounts/notifications" headers requestParts

            match int status with
            | 200 -> return GetAccountsNotifications.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsNotifications.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsNotifications.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccountsNotifications.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Set the notification which id has been given as readed.
    ///</summary>
    member this.PutAccountsNotificationsByNotificationId() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.putAsync url "/accounts/notifications/{notificationId}" headers requestParts

            match int status with
            | 200 -> return PutAccountsNotificationsByNotificationId.OK
            | 400 -> return PutAccountsNotificationsByNotificationId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAccountsNotificationsByNotificationId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAccountsNotificationsByNotificationId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete a notification
    ///</summary>
    member this.DeleteAccountsNotificationsByNotificationId() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/accounts/notifications/{notificationId}" headers requestParts

            match int status with
            | 200 -> return DeleteAccountsNotificationsByNotificationId.OK(Serializer.deserialize content)
            | 400 -> return DeleteAccountsNotificationsByNotificationId.BadRequest(Serializer.deserialize content)
            | 401 -> return DeleteAccountsNotificationsByNotificationId.Unauthorized(Serializer.deserialize content)
            | _ ->
                return DeleteAccountsNotificationsByNotificationId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all accounts and subaccounts.
    ///</summary>
    member this.GetAccounts() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/accounts" headers requestParts

            match int status with
            | 200 -> return GetAccounts.OK(Serializer.deserialize content)
            | 400 -> return GetAccounts.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccounts.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccounts.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a new child account in the system.
    ///</summary>
    member this.PostAccounts(account: PostAccountsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent account ]
            let! (status, content) = OpenApiHttp.postAsync url "/accounts" headers requestParts

            match int status with
            | 200 -> return PostAccounts.OK(Serializer.deserialize content)
            | 400 -> return PostAccounts.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAccounts.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAccounts.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all accounts and subaccounts.
    ///</summary>
    member this.PostAccountsbulk() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/accountsbulk" headers requestParts

            match int status with
            | 200 -> return PostAccountsbulk.OK(Serializer.deserialize content)
            | 400 -> return PostAccountsbulk.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAccountsbulk.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAccountsbulk.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrieve one account matching the id provided.
    ///</summary>
    member this.GetAccountsByAccountId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/accounts/{accountId}" headers requestParts

            match int status with
            | 200 -> return GetAccountsByAccountId.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccountsByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Modify an existing account with the new data setted into the parameters.
    ///</summary>
    member this.PutAccountsByAccountId(account: PutAccountsByAccountIdPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent account ]
            let! (status, content) = OpenApiHttp.putAsync url "/accounts/{accountId}" headers requestParts

            match int status with
            | 200 -> return PutAccountsByAccountId.OK(Serializer.deserialize content)
            | 400 -> return PutAccountsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAccountsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAccountsByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Remove an existing account from the database.
    ///</summary>
    member this.DeleteAccountsByAccountId(payload: DeleteAccountsByAccountIdPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/accounts/{accountId}" headers requestParts

            match int status with
            | 200 -> return DeleteAccountsByAccountId.OK(Serializer.deserialize content)
            | 400 -> return DeleteAccountsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return DeleteAccountsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteAccountsByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Modify the branding information of an account.
    ///</summary>
    member this.PutAccountsBrandingByAccountId(account: PutAccountsBrandingByAccountIdPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent account ]
            let! (status, content) = OpenApiHttp.putAsync url "/accounts/{accountId}/branding" headers requestParts

            match int status with
            | 200 -> return PutAccountsBrandingByAccountId.OK(Serializer.deserialize content)
            | 400 -> return PutAccountsBrandingByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAccountsBrandingByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAccountsBrandingByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Account Branding status.
    ///</summary>
    member this.GetAccountsBrandingVerifyByAccountId() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync url "/accounts/{accountId}/branding/verify" headers requestParts

            match int status with
            | 200 -> return GetAccountsBrandingVerifyByAccountId.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsBrandingVerifyByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsBrandingVerifyByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccountsBrandingVerifyByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrieve all existing roles.
    ///</summary>
    member this.GetAccountsRolesByAccountId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/accounts/{accountId}/roles" headers requestParts

            match int status with
            | 200 -> return GetAccountsRolesByAccountId.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsRolesByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsRolesByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccountsRolesByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all actions belonging a role.
    ///</summary>
    member this.GetAccountsRolesActionsByAccountIdAndRolename() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync url "/accounts/{accountId}/roles/{rolename}/actions" headers requestParts

            match int status with
            | 200 -> return GetAccountsRolesActionsByAccountIdAndRolename.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsRolesActionsByAccountIdAndRolename.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsRolesActionsByAccountIdAndRolename.Unauthorized(Serializer.deserialize content)
            | _ ->
                return GetAccountsRolesActionsByAccountIdAndRolename.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all notifications of an account.
    ///</summary>
    member this.GetAccountsNotificationsByAccountId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/accounts/{accountId}/notifications" headers requestParts

            match int status with
            | 200 -> return GetAccountsNotificationsByAccountId.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsNotificationsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsNotificationsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccountsNotificationsByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///The notification with the id provided will be check as readed.
    ///</summary>
    member this.PutAccountsNotificationsByAccountIdAndNotificationId() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.putAsync url "/accounts/{accountId}/notifications/{notificationId}" headers requestParts

            match int status with
            | 200 -> return PutAccountsNotificationsByAccountIdAndNotificationId.OK
            | 400 ->
                return PutAccountsNotificationsByAccountIdAndNotificationId.BadRequest(Serializer.deserialize content)
            | 401 ->
                return PutAccountsNotificationsByAccountIdAndNotificationId.Unauthorized(Serializer.deserialize content)
            | _ ->
                return
                    PutAccountsNotificationsByAccountIdAndNotificationId.InternalServerError(
                        Serializer.deserialize content
                    )
        }

    ///<summary>
    ///Delete a notification
    ///</summary>
    member this.DeleteAccountsNotificationsByAccountIdAndNotificationId() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/accounts/{accountId}/notifications/{notificationId}" headers requestParts

            match int status with
            | 200 -> return DeleteAccountsNotificationsByAccountIdAndNotificationId.OK
            | 400 ->
                return
                    DeleteAccountsNotificationsByAccountIdAndNotificationId.BadRequest(Serializer.deserialize content)
            | 401 ->
                return
                    DeleteAccountsNotificationsByAccountIdAndNotificationId.Unauthorized(Serializer.deserialize content)
            | _ ->
                return
                    DeleteAccountsNotificationsByAccountIdAndNotificationId.InternalServerError(
                        Serializer.deserialize content
                    )
        }

    ///<summary>
    ///Retrive all related products
    ///</summary>
    member this.GetAccountsProductsByAccountId(?filter: string) =
        async {
            let requestParts =
                [ if filter.IsSome then
                      RequestPart.query ("filter", filter.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/accounts/{accountId}/products" headers requestParts

            match int status with
            | 200 -> return GetAccountsProductsByAccountId.OK(Serializer.deserialize content)
            | 400 -> return GetAccountsProductsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAccountsProductsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAccountsProductsByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///- A maximum of 3 alerts per SIM can be set in order to notify the user when the consumption of a  promotion  reaches or exceeds a certain limit, Alerts can also be used to notify the user of usage over their predefined bundle amount.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutAccountsProductsAlerts(payload: PutAccountsProductsAlertsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/accounts/products/alerts" headers requestParts

            match int status with
            | 200 -> return PutAccountsProductsAlerts.OK
            | 400 -> return PutAccountsProductsAlerts.BadRequest
            | 401 -> return PutAccountsProductsAlerts.Unauthorized
            | _ -> return PutAccountsProductsAlerts.InternalServerError
        }

    ///<summary>
    ///Amount in which the balance increases.
    ///</summary>
    member this.PutAccountsTopupDirectByAccountId(payload: PutAccountsTopupDirectByAccountIdPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/accounts/{accountId}/topup-direct" headers requestParts

            match int status with
            | 200 -> return PutAccountsTopupDirectByAccountId.OK(Serializer.deserialize content)
            | 400 -> return PutAccountsTopupDirectByAccountId.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAccountsTopupDirectByAccountId.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAccountsTopupDirectByAccountId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Add a Security Service Setting inside an account
    ///</summary>
    member this.PostAccountsSecuritySettingsByAccountId(payload: AccountSecuritySetting) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]

            let! (status, content) =
                OpenApiHttp.postAsync url "/accounts/{accountId}/security-settings" headers requestParts

            return PostAccountsSecuritySettingsByAccountId.OK
        }

    ///<summary>
    ///Edit a Security Service Setting inside an account
    ///</summary>
    member this.PutAccountsSecuritySettingsByAccountIdAndSecuritySettingId(payload: AccountSecuritySetting) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]

            let! (status, content) =
                OpenApiHttp.putAsync
                    url
                    "/accounts/{accountId}/security-settings/{securitySettingId}"
                    headers
                    requestParts

            return PutAccountsSecuritySettingsByAccountIdAndSecuritySettingId.OK
        }

    ///<summary>
    ///Edit a Security Service Setting inside an account
    ///</summary>
    member this.DeleteAccountsSecuritySettingsByAccountIdAndSecuritySettingId() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    url
                    "/accounts/{accountId}/security-settings/{securitySettingId}"
                    headers
                    requestParts

            return DeleteAccountsSecuritySettingsByAccountIdAndSecuritySettingId.OK
        }

    ///<summary>
    ///Retrieve one account matching the id provided.
    ///</summary>
    member this.GetAccountsSecuritySettingsAvailableGapsByAccountId
        (
            accountId: string,
            accountIdInQuery: string,
            carrier: string,
            cidrBlockSize: float
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("accountId", accountId)
                  RequestPart.query ("accountId", accountIdInQuery)
                  RequestPart.query ("carrier", carrier)
                  RequestPart.query ("cidrBlockSize", cidrBlockSize) ]

            let! (status, content) =
                OpenApiHttp.getAsync url "/accounts/{accountId}/security-settings/available-gaps" headers requestParts

            match int status with
            | 200 -> return GetAccountsSecuritySettingsAvailableGapsByAccountId.OK(Serializer.deserialize content)
            | 400 ->
                return GetAccountsSecuritySettingsAvailableGapsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 ->
                return GetAccountsSecuritySettingsAvailableGapsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ ->
                return
                    GetAccountsSecuritySettingsAvailableGapsByAccountId.InternalServerError(
                        Serializer.deserialize content
                    )
        }

    ///<summary>
    ///Retrieve available ips for particular pool.
    ///</summary>
    member this.GetAccountsSecuritySettingsAvailableIpsByAccountId
        (
            accountId: string,
            accountIdInQuery: string,
            poolId: string
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("accountId", accountId)
                  RequestPart.query ("accountId", accountIdInQuery)
                  RequestPart.query ("poolId", poolId) ]

            let! (status, content) =
                OpenApiHttp.getAsync url "/accounts/{accountId}/security-settings/available-ips" headers requestParts

            match int status with
            | 200 -> return GetAccountsSecuritySettingsAvailableIpsByAccountId.OK(Serializer.deserialize content)
            | 400 ->
                return GetAccountsSecuritySettingsAvailableIpsByAccountId.BadRequest(Serializer.deserialize content)
            | 401 ->
                return GetAccountsSecuritySettingsAvailableIpsByAccountId.Unauthorized(Serializer.deserialize content)
            | _ ->
                return
                    GetAccountsSecuritySettingsAvailableIpsByAccountId.InternalServerError(
                        Serializer.deserialize content
                    )
        }

    ///<summary>
    ///Endpoint to asure the App is up and working. If is not, nginx will return a 502 Bad Gateway exception.
    ///</summary>
    member this.GetPing() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/ping" headers requestParts
            return GetPing.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///General endpoint to send an email to an account
    ///</summary>
    member this.PostMail(xAccessToken: string, payload: PostMailPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/mail" headers requestParts

            match int status with
            | 200 -> return PostMail.OK(Serializer.deserialize content)
            | 400 -> return PostMail.BadRequest(Serializer.deserialize content)
            | 401 -> return PostMail.Unauthorized(Serializer.deserialize content)
            | _ -> return PostMail.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///General endpoint to send files
    ///</summary>
    member this.PostUpload(xAccessToken: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken) ]

            let! (status, content) = OpenApiHttp.postAsync url "/upload" headers requestParts
            return PostUpload.OK
        }

    ///<summary>
    ///General endpoint to send files
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="fileId">The file to get</param>
    member this.GetFileByFileId(xAccessToken: string, ?fileId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  if fileId.IsSome then
                      RequestPart.path ("fileId", fileId.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/file/{fileId}" headers requestParts
            return GetFileByFileId.OK
        }

    ///<summary>
    ///Return the branding of a customURL
    ///</summary>
    member this.GetLogincustomization(customURL: string) =
        async {
            let requestParts =
                [ RequestPart.query ("customURL", customURL) ]

            let! (status, content) = OpenApiHttp.getAsync url "/logincustomization" headers requestParts
            return GetLogincustomization.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a Fake CDR
    ///</summary>
    member this.PostSimulatecdr(xAccessToken: string, payload: PostSimulatecdrPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/simulatecdr" headers requestParts
            return PostSimulatecdr.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///General endpoint to send a webhook notification
    ///</summary>
    member this.PostWebhook(xAccessToken: string, payload: PostWebhookPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/webhook" headers requestParts
            return PostWebhook.NoContent
        }

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
            ?format: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/products" headers requestParts

            match int status with
            | 200 -> return GetProducts.OK(Serializer.deserialize content)
            | 400 -> return GetProducts.BadRequest
            | 401 -> return GetProducts.Unauthorized
            | 403 -> return GetProducts.Forbidden
            | 500 -> return GetProducts.InternalServerError
            | _ -> return GetProducts.ServiceUnavailable
        }

    ///<summary>
    ///Creates a new product taking a token, an accountId and the new product data. Returns the new product data.
    ///</summary>
    member this.PostProducts(xAccessToken: string, payload: PostProductsPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/products" headers requestParts

            match int status with
            | 200 -> return PostProducts.OK(Serializer.deserialize content)
            | 400 -> return PostProducts.BadRequest(Serializer.deserialize content)
            | 401 -> return PostProducts.Unauthorized(Serializer.deserialize content)
            | _ -> return PostProducts.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a product of an account
    ///</summary>
    member this.GetProductsByProductId(xAccessToken: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken) ]

            let! (status, content) = OpenApiHttp.getAsync url "/products/{productId}" headers requestParts

            match int status with
            | 200 -> return GetProductsByProductId.OK(Serializer.deserialize content)
            | 400 -> return GetProductsByProductId.BadRequest
            | 401 -> return GetProductsByProductId.Unauthorized
            | 403 -> return GetProductsByProductId.Forbidden
            | 500 -> return GetProductsByProductId.InternalServerError
            | _ -> return GetProductsByProductId.ServiceUnavailable
        }

    ///<summary>
    ///Updates a product taking a token, an accountId and the updated product data. Returns the updated product data.
    ///</summary>
    member this.PatchProductsByProductId(xAccessToken: string, payload: ProductNoBundleId) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.patchAsync url "/products/{productId}" headers requestParts

            match int status with
            | 200 -> return PatchProductsByProductId.OK(Serializer.deserialize content)
            | 400 -> return PatchProductsByProductId.BadRequest(Serializer.deserialize content)
            | 401 -> return PatchProductsByProductId.Unauthorized(Serializer.deserialize content)
            | _ -> return PatchProductsByProductId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete product by ID
    ///</summary>
    member this.DeleteProductsByProductId(xAccessToken: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/products/{productId}" headers requestParts

            match int status with
            | 200 -> return DeleteProductsByProductId.OK(Serializer.deserialize content)
            | 400 -> return DeleteProductsByProductId.BadRequest(Serializer.deserialize content)
            | 401 -> return DeleteProductsByProductId.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteProductsByProductId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all the availables schemas for creating a product.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID mandatory to filter by account.</param>
    member this.GetSchemaProduct(xAccessToken: string, accountId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/schema/product" headers requestParts

            match int status with
            | 200 -> return GetSchemaProduct.OK
            | 400 -> return GetSchemaProduct.BadRequest(Serializer.deserialize content)
            | 401 -> return GetSchemaProduct.Unauthorized(Serializer.deserialize content)
            | _ -> return GetSchemaProduct.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Transfer products from an account to another
    ///</summary>
    member this.PostProductsTransferByProductId(xAccessToken: string, payload: PostProductsTransferByProductIdPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/products/{productId}/transfer" headers requestParts

            match int status with
            | 200 -> return PostProductsTransferByProductId.OK(Serializer.deserialize content)
            | 400 -> return PostProductsTransferByProductId.BadRequest
            | 401 -> return PostProductsTransferByProductId.Unauthorized
            | 403 -> return PostProductsTransferByProductId.Forbidden
            | 500 -> return PostProductsTransferByProductId.InternalServerError
            | _ -> return PostProductsTransferByProductId.ServiceUnavailable
        }

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
    member this.GetCdr
        (
            xAccessToken: string,
            accountId: string,
            ?``type``: string,
            ?limit: string,
            ?page: float,
            ?sort: string,
            ?order: string,
            ?format: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/cdr" headers requestParts

            match int status with
            | 200 -> return GetCdr.OK(Serializer.deserialize content)
            | 400 -> return GetCdr.BadRequest(Serializer.deserialize content)
            | 401 -> return GetCdr.Unauthorized(Serializer.deserialize content)
            | 404 -> return GetCdr.NotFound(Serializer.deserialize content)
            | _ -> return GetCdr.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all cdrs on application. If type is not set to `sms` it returns data cdrs.
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="type">Filter SIM cards by type. &amp;lt;br&amp;gt; - If param iccid is not set and param type is not set or set to `data`, it filters only type data cdrs. If param type is set to `sms` only filters type sms cdrs. &amp;lt;br&amp;gt; - If param iccid is set and param type is not set or set to `data`, it returns data cdrs of that iccid. If param type is set to `sms`, it returns sms cdrs for that iccid..</param>
    member this.GetCdrStats(xAccessToken: string, accountId: string, ?``type``: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if ``type``.IsSome then
                      RequestPart.query ("type", ``type``.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/cdr/stats" headers requestParts
            return GetCdrStats.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Assets card Info (bulk)
    ///</summary>
    member this.GetAssets() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/assets" headers requestParts

            match int status with
            | 200 -> return GetAssets.OK(Serializer.deserialize content)
            | 400 -> return GetAssets.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAssets.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAssets.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a new Asset
    ///</summary>
    member this.PostAssets(payload: PostAssetsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets" headers requestParts

            match int status with
            | 200 -> return PostAssets.OK(Serializer.deserialize content)
            | 400 -> return PostAssets.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAssets.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAssets.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Assets card Info (bulk)
    ///</summary>
    member this.PostAssetsbulk(payload: PostAssetsbulkPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/assetsbulk" headers requestParts

            match int status with
            | 200 -> return PostAssetsbulk.OK(Serializer.deserialize content)
            | 400 -> return PostAssetsbulk.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAssetsbulk.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAssetsbulk.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Assets get Info
    ///</summary>
    member this.GetAssetsByIccid() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/assets/{iccid}" headers requestParts

            match int status with
            | 200 -> return GetAssetsByIccid.OK(Serializer.deserialize content)
            | 400 -> return GetAssetsByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAssetsByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAssetsByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Change the name of the asset for the specified accountId
    ///</summary>
    member this.PutAssetsByIccid(asset: PutAssetsByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent asset ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}" headers requestParts

            match int status with
            | 200 -> return PutAssetsByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Remove an Asset
    ///</summary>
    member this.DeleteAssetsByIccid(asset: DeleteAssetsByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent asset ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/assets/{iccid}" headers requestParts

            match int status with
            | 200 -> return DeleteAssetsByIccid.OK(Serializer.deserialize content)
            | 400 -> return DeleteAssetsByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return DeleteAssetsByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteAssetsByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Update group name
    ///</summary>
    member this.PutAssetsGroupnameByIccid(asset: PutAssetsGroupnameByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent asset ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/groupname" headers requestParts

            match int status with
            | 200 -> return PutAssetsGroupnameByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsGroupnameByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsGroupnameByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsGroupnameByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Transfer an Asset from an account to another
    ///</summary>
    member this.PostAssetsTransferByIccid(asset: PostAssetsTransferByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent asset ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/transfer" headers requestParts

            match int status with
            | 200 -> return PostAssetsTransferByIccid.OK(Serializer.deserialize content)
            | 400 -> return PostAssetsTransferByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAssetsTransferByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAssetsTransferByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Activate an asset and subscribe the asset to the assigned product.
    ///</summary>
    member this.PutAssetsSubscribeByIccid(payload: PutAssetsSubscribeByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/subscribe" headers requestParts

            match int status with
            | 200 -> return PutAssetsSubscribeByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsSubscribeByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsSubscribeByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsSubscribeByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Unsubscribe the asset to the assigned product
    ///</summary>
    member this.PutAssetsUnsubscribeByIccid(payload: PutAssetsUnsubscribeByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/unsubscribe" headers requestParts

            match int status with
            | 200 -> return PutAssetsUnsubscribeByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsUnsubscribeByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsUnsubscribeByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsUnsubscribeByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Remove subscription and create a new subscribe the asset to the assigned product.
    ///</summary>
    member this.PutAssetsResubscribeByIccid(payload: PutAssetsResubscribeByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/resubscribe" headers requestParts

            match int status with
            | 200 -> return PutAssetsResubscribeByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsResubscribeByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsResubscribeByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsResubscribeByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Suspend an assets
    ///</summary>
    member this.PutAssetsSuspendByIccid(payload: PutAssetsSuspendByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/suspend" headers requestParts

            match int status with
            | 200 -> return PutAssetsSuspendByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsSuspendByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsSuspendByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsSuspendByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Reactive an assets. The assets should be suspended
    ///</summary>
    member this.PutAssetsUnsuspendByIccid(payload: PutAssetsUnsuspendByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/unsuspend" headers requestParts

            match int status with
            | 200 -> return PutAssetsUnsuspendByIccid.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsUnsuspendByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsUnsuspendByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsUnsuspendByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutAssetsAlertsByIccid(payload: PutAssetsAlertsByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/assets/{iccid}/alerts" headers requestParts

            match int status with
            | 200 -> return PutAssetsAlertsByIccid.OK
            | 400 -> return PutAssetsAlertsByIccid.BadRequest
            | 401 -> return PutAssetsAlertsByIccid.Unauthorized
            | _ -> return PutAssetsAlertsByIccid.InternalServerError
        }

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostAssetsPurgeByIccid(payload: PostAssetsPurgeByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/purge" headers requestParts

            match int status with
            | 200 -> return PostAssetsPurgeByIccid.OK
            | 400 -> return PostAssetsPurgeByIccid.BadRequest
            | 401 -> return PostAssetsPurgeByIccid.Unauthorized
            | _ -> return PostAssetsPurgeByIccid.InternalServerError
        }

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostAssetsSmsByIccid(message: PostAssetsSmsByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent message ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/sms" headers requestParts

            match int status with
            | 200 -> return PostAssetsSmsByIccid.OK
            | 400 -> return PostAssetsSmsByIccid.BadRequest
            | 401 -> return PostAssetsSmsByIccid.Unauthorized
            | _ -> return PostAssetsSmsByIccid.InternalServerError
        }

    ///<summary>
    ///Set limits for a simcard. You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostAssetsLimitByIccid(message: PostAssetsLimitByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent message ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/limit" headers requestParts

            match int status with
            | 200 -> return PostAssetsLimitByIccid.OK
            | 400 -> return PostAssetsLimitByIccid.BadRequest
            | 401 -> return PostAssetsLimitByIccid.Unauthorized
            | _ -> return PostAssetsLimitByIccid.InternalServerError
        }

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
    member this.PostAssetsTagsByIccid(xAccessToken: string, iccid: string, message: PostAssetsTagsByIccidPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.path ("iccid", iccid)
                  RequestPart.jsonContent message ]

            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/tags" headers requestParts

            match int status with
            | 200 -> return PostAssetsTagsByIccid.OK
            | 400 -> return PostAssetsTagsByIccid.BadRequest
            | 401 -> return PostAssetsTagsByIccid.Unauthorized
            | _ -> return PostAssetsTagsByIccid.InternalServerError
        }

    ///<summary>
    ///Checks the simcard status over the network, if the sim is correctly provisioned in the system, last connection, last data transmision, if it is online etc...
    ///</summary>
    member this.GetAssetsDiagnosticByIccid() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/assets/{iccid}/diagnostic" headers requestParts

            match int status with
            | 200 -> return GetAssetsDiagnosticByIccid.OK(Serializer.deserialize content)
            | 401 -> return GetAssetsDiagnosticByIccid.Unauthorized
            | 500 -> return GetAssetsDiagnosticByIccid.InternalServerError
            | _ -> return GetAssetsDiagnosticByIccid.ServiceUnavailable
        }

    ///<summary>
    ///Return assets location.
    ///</summary>
    member this.GetAssetsLocationByIccid() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/assets/{iccid}/location" headers requestParts

            match int status with
            | 200 -> return GetAssetsLocationByIccid.OK(Serializer.deserialize content)
            | 400 -> return GetAssetsLocationByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAssetsLocationByIccid.Unauthorized(Serializer.deserialize content)
            | 404 -> return GetAssetsLocationByIccid.NotFound(Serializer.deserialize content)
            | _ -> return GetAssetsLocationByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Return asset sessions.
    ///</summary>
    member this.GetAssetsSessionsByIccid() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/assets/{iccid}/sessions" headers requestParts

            match int status with
            | 200 -> return GetAssetsSessionsByIccid.OK(Serializer.deserialize content)
            | 400 -> return GetAssetsSessionsByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAssetsSessionsByIccid.Unauthorized(Serializer.deserialize content)
            | 404 -> return GetAssetsSessionsByIccid.NotFound(Serializer.deserialize content)
            | _ -> return GetAssetsSessionsByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Reassign a new IP to a single asset or a bulk of assets. You can choose to update the IP and assign a new Fixed IP from a security service or a dynamic IP.
    ///</summary>
    member this.PostAssetsReallocateIpByIccid(payload: PostAssetsReallocateIpByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/reallocate-ip" headers requestParts
            return PostAssetsReallocateIpByIccid.OK
        }

    ///<summary>
    ///Get all reports from an account
    ///</summary>
    member this.GetReportsCustom() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/reports/custom" headers requestParts

            match int status with
            | 200 -> return GetReportsCustom.OK(Serializer.deserialize content)
            | 401 -> return GetReportsCustom.Unauthorized(Serializer.deserialize content)
            | _ -> return GetReportsCustom.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a new custom report. &amp;lt;/br&amp;gt; Date filtering supports dates as `YYYY-MM-DDTHH:mm:SSZ`, for example `"2020-04-01T00:59:59Z"` &amp;lt;/br&amp;gt; The fileds requires are; accountId, dateFrom, dateTo
    ///</summary>
    member this.PostReportsCustom(report: FilterCustom) =
        async {
            let requestParts = [ RequestPart.jsonContent report ]
            let! (status, content) = OpenApiHttp.postAsync url "/reports/custom" headers requestParts

            match int status with
            | 200 -> return PostReportsCustom.OK
            | 401 -> return PostReportsCustom.Unauthorized(Serializer.deserialize content)
            | 403 -> return PostReportsCustom.Forbidden(Serializer.deserialize content)
            | 404 -> return PostReportsCustom.NotFound(Serializer.deserialize content)
            | _ -> return PostReportsCustom.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete all reports from an account
    ///</summary>
    member this.DeleteReportsCustom() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.deleteAsync url "/reports/custom" headers requestParts

            match int status with
            | 204 -> return DeleteReportsCustom.NoContent
            | 401 -> return DeleteReportsCustom.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteReportsCustom.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get one reports from an account
    ///</summary>
    member this.GetReportsCustomByReportId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/reports/custom/{reportId}" headers requestParts

            match int status with
            | 200 -> return GetReportsCustomByReportId.OK(Serializer.deserialize content)
            | 401 -> return GetReportsCustomByReportId.Unauthorized(Serializer.deserialize content)
            | 404 -> return GetReportsCustomByReportId.NotFound(Serializer.deserialize content)
            | _ -> return GetReportsCustomByReportId.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete one report from an account
    ///</summary>
    member this.DeleteReportsCustomByReportId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.deleteAsync url "/reports/custom/{reportId}" headers requestParts

            match int status with
            | 204 -> return DeleteReportsCustomByReportId.NoContent
            | 401 -> return DeleteReportsCustomByReportId.Unauthorized(Serializer.deserialize content)
            | 404 -> return DeleteReportsCustomByReportId.NotFound(Serializer.deserialize content)
            | _ -> return DeleteReportsCustomByReportId.InternalServerError(Serializer.deserialize content)
        }

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
            ?format: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/events" headers requestParts

            match int status with
            | 200 -> return GetEvents.OK(Serializer.deserialize content)
            | 400 -> return GetEvents.BadRequest(Serializer.deserialize content)
            | 401 -> return GetEvents.Unauthorized(Serializer.deserialize content)
            | _ -> return GetEvents.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get all zones schemes
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="carrier">filter by carrier</param>
    member this.GetZonesSchemes(xAccessToken: string, accountId: string, ?carrier: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if carrier.IsSome then
                      RequestPart.query ("carrier", carrier.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/zones-schemes" headers requestParts
            return GetZonesSchemes.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a zone schemes
    ///</summary>
    member this.GetZonesSchemesByZoneSchemeId(xAccessToken: string, accountId: string, zoneSchemeId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  RequestPart.path ("zoneSchemeId", zoneSchemeId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/zones-schemes/{zoneSchemeId}" headers requestParts
            return GetZonesSchemesByZoneSchemeId.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Activate a bulk of assets and subscribe the assets to the assigned product.
    ///</summary>
    member this.PostBulkAssetsSubscribe(body: PostBulkAssetsSubscribePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/subscribe" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsSubscribe.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsSubscribe.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsSubscribe.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsSubscribe.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Transfer a Asset from a account to another in bulk
    ///</summary>
    member this.PostBulkAssetsTransfer(body: PostBulkAssetsTransferPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/transfer" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsTransfer.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsTransfer.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsTransfer.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsTransfer.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Return a Asset from a account to parent account in bulk
    ///</summary>
    member this.PostBulkAssetsReturn(body: PostBulkAssetsReturnPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/return" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsReturn.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsReturn.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsReturn.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsReturn.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Suspend assets in a massive way by using different filters
    ///</summary>
    member this.PutBulkAssetsSuspend() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/assets/suspend" headers requestParts

            match int status with
            | 202 -> return PutBulkAssetsSuspend.Accepted(Serializer.deserialize content)
            | 400 -> return PutBulkAssetsSuspend.BadRequest(Serializer.deserialize content)
            | 401 -> return PutBulkAssetsSuspend.Unauthorized(Serializer.deserialize content)
            | _ -> return PutBulkAssetsSuspend.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///This endpoint change in a massive way the suspend status of the assets which match with the filter setted.
    ///</summary>
    member this.PutBulkAssetsUnsuspend() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/assets/unsuspend" headers requestParts

            match int status with
            | 202 -> return PutBulkAssetsUnsuspend.Accepted(Serializer.deserialize content)
            | 400 -> return PutBulkAssetsUnsuspend.BadRequest(Serializer.deserialize content)
            | 401 -> return PutBulkAssetsUnsuspend.Unauthorized(Serializer.deserialize content)
            | _ -> return PutBulkAssetsUnsuspend.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Resubscribe a bulk of assets to the assigned product.
    ///</summary>
    member this.PostBulkAssetsResubscribe(body: PostBulkAssetsResubscribePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/resubscribe" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsResubscribe.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsResubscribe.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsResubscribe.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsResubscribe.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Create Assets in bulk mode
    ///</summary>
    member this.PostBulkAssets(body: PostBulkAssetsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets" headers requestParts

            match int status with
            | 202 -> return PostBulkAssets.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssets.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssets.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssets.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Update Assets in bulk mode
    ///</summary>
    member this.PutBulkAssets(body: PutBulkAssetsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/assets" headers requestParts

            match int status with
            | 202 -> return PutBulkAssets.Accepted(Serializer.deserialize content)
            | 400 -> return PutBulkAssets.BadRequest(Serializer.deserialize content)
            | 401 -> return PutBulkAssets.Unauthorized(Serializer.deserialize content)
            | _ -> return PutBulkAssets.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Update group name of Assets in bulk mode
    ///</summary>
    member this.PutBulkAssetsGroupname(body: PutBulkAssetsGroupnamePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/assets/groupname" headers requestParts

            match int status with
            | 202 -> return PutBulkAssetsGroupname.Accepted(Serializer.deserialize content)
            | 400 -> return PutBulkAssetsGroupname.BadRequest(Serializer.deserialize content)
            | 401 -> return PutBulkAssetsGroupname.Unauthorized(Serializer.deserialize content)
            | _ -> return PutBulkAssetsGroupname.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostBulkAssetsLimit(body: PostBulkAssetsLimitPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/limit" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsLimit.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsLimit.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsLimit.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsLimit.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///- A maximum of 3 alerts per SIM can be set in order to notify the user when the consumption of a  promotion  reaches or exceeds a certain limit, Alerts can also be used to notify the user of usage over their predefined bundle amount.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set only.
    ///</summary>
    member this.PutBulkAssetsAlerts(body: PutBulkAssetsAlertsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/assets/alerts" headers requestParts

            match int status with
            | 202 -> return PutBulkAssetsAlerts.Accepted(Serializer.deserialize content)
            | 400 -> return PutBulkAssetsAlerts.BadRequest(Serializer.deserialize content)
            | 401 -> return PutBulkAssetsAlerts.Unauthorized(Serializer.deserialize content)
            | _ -> return PutBulkAssetsAlerts.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostBulkAssetsSms(body: PostBulkAssetsSmsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/sms" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsSms.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsSms.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsSms.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsSms.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostBulkAssetsPurge(body: PostBulkAssetsPurgePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/purge" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsPurge.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsPurge.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsPurge.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsPurge.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Download template for bulk update.
    ///</summary>
    ///<param name="blankTemplate">true if you want only the headers</param>
    member this.PostBulkAssetsUpdateTemplate(?blankTemplate: bool) =
        async {
            let requestParts =
                [ if blankTemplate.IsSome then
                      RequestPart.query ("blankTemplate", blankTemplate.Value) ]

            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/update/template" headers requestParts
            return PostBulkAssetsUpdateTemplate.OK
        }

    ///<summary>
    ///Bulk updates of assets for name, group and custom attributes uploading a CSV file.
    ///</summary>
    member this.PostBulkAssetsUpdateProcess() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/update/process" headers requestParts
            return PostBulkAssetsUpdateProcess.Accepted(Serializer.deserialize content)
        }

    ///<summary>
    ///Reassign a new IP in a bulk of assets selecting to assign a new dynamic IP or a security service.
    ///</summary>
    member this.PostBulkAssetsReallocateIp(payload: PostBulkAssetsReallocateIpPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/assets/reallocate-ip" headers requestParts

            match int status with
            | 202 -> return PostBulkAssetsReallocateIp.Accepted(Serializer.deserialize content)
            | 400 -> return PostBulkAssetsReallocateIp.BadRequest(Serializer.deserialize content)
            | 401 -> return PostBulkAssetsReallocateIp.Unauthorized(Serializer.deserialize content)
            | _ -> return PostBulkAssetsReallocateIp.InternalServerError(Serializer.deserialize content)
        }

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
    member this.PostPaymentsTopupPaypal(xAccessToken: string, payload: PostPaymentsTopupPaypalPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/payments/topupPaypal" headers requestParts

            match int status with
            | 200 -> return PostPaymentsTopupPaypal.OK(Serializer.deserialize content)
            | 400 -> return PostPaymentsTopupPaypal.BadRequest(Serializer.deserialize content)
            | 401 -> return PostPaymentsTopupPaypal.Unauthorized(Serializer.deserialize content)
            | _ -> return PostPaymentsTopupPaypal.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Confirms a Paypal transaction
    ///</summary>
    member this.PostPaymentsConfirmTopupPaypal(xAccessToken: string, payload: PostPaymentsConfirmTopupPaypalPayload) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.postAsync url "/payments/confirmTopupPaypal" headers requestParts

            match int status with
            | 200 -> return PostPaymentsConfirmTopupPaypal.OK(Serializer.deserialize content)
            | 400 -> return PostPaymentsConfirmTopupPaypal.BadRequest(Serializer.deserialize content)
            | 401 -> return PostPaymentsConfirmTopupPaypal.Unauthorized(Serializer.deserialize content)
            | _ -> return PostPaymentsConfirmTopupPaypal.InternalServerError(Serializer.deserialize content)
        }

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
            ?order: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/security/alerts" headers requestParts

            match int status with
            | 200 -> return GetSecurityAlerts.OK(Serializer.deserialize content)
            | 400 -> return GetSecurityAlerts.BadRequest
            | 401 -> return GetSecurityAlerts.Unauthorized
            | 403 -> return GetSecurityAlerts.Forbidden
            | 500 -> return GetSecurityAlerts.InternalServerError
            | _ -> return GetSecurityAlerts.ServiceUnavailable
        }

    ///<summary>
    ///Fetches security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alertId">To know what alert to find</param>
    member this.GetSecurityAlertsByAlertId(xAccessToken: string, accountId: string, alertId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  RequestPart.path ("alertId", alertId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/security/alerts/{alertId}" headers requestParts

            match int status with
            | 200 -> return GetSecurityAlertsByAlertId.OK(Serializer.deserialize content)
            | 400 -> return GetSecurityAlertsByAlertId.BadRequest
            | 401 -> return GetSecurityAlertsByAlertId.Unauthorized
            | 403 -> return GetSecurityAlertsByAlertId.Forbidden
            | 500 -> return GetSecurityAlertsByAlertId.InternalServerError
            | _ -> return GetSecurityAlertsByAlertId.ServiceUnavailable
        }

    ///<summary>
    ///Edits security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alertId">To know what alert to edit</param>
    ///<param name="payload"></param>
    member this.PutSecurityAlertsByAlertId
        (
            xAccessToken: string,
            accountId: string,
            alertId: string,
            payload: PutSecurityAlertsByAlertIdPayload
        ) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  RequestPart.path ("alertId", alertId)
                  RequestPart.jsonContent payload ]

            let! (status, content) = OpenApiHttp.putAsync url "/security/alerts/{alertId}" headers requestParts

            match int status with
            | 200 -> return PutSecurityAlertsByAlertId.OK(Serializer.deserialize content)
            | 400 -> return PutSecurityAlertsByAlertId.BadRequest
            | 401 -> return PutSecurityAlertsByAlertId.Unauthorized
            | 403 -> return PutSecurityAlertsByAlertId.Forbidden
            | 500 -> return PutSecurityAlertsByAlertId.InternalServerError
            | _ -> return PutSecurityAlertsByAlertId.ServiceUnavailable
        }

    ///<summary>
    ///Deletes security alerts for an account
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId">Account ID is mandatory in order to know which account are you operating from</param>
    ///<param name="alertId">To know what alert to delete</param>
    member this.DeleteSecurityAlertsByAlertId(xAccessToken: string, accountId: string, alertId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  RequestPart.path ("alertId", alertId) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/security/alerts/{alertId}" headers requestParts

            match int status with
            | 204 -> return DeleteSecurityAlertsByAlertId.NoContent
            | 400 -> return DeleteSecurityAlertsByAlertId.BadRequest
            | 401 -> return DeleteSecurityAlertsByAlertId.Unauthorized
            | 403 -> return DeleteSecurityAlertsByAlertId.Forbidden
            | 500 -> return DeleteSecurityAlertsByAlertId.InternalServerError
            | _ -> return DeleteSecurityAlertsByAlertId.ServiceUnavailable
        }

    ///<summary>
    ///Get Top 10 subscribers threatened. Type Table
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="isThreat">Type of alert you want to aggregate. True is Threaten, false is other type. By default is True</param>
    member this.GetGraphSecurityTopthreaten
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?isThreat: bool
        ) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value)
                  if isThreat.IsSome then
                      RequestPart.query ("isThreat", isThreat.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/security/topthreaten" headers requestParts

            match int status with
            | 200 -> return GetGraphSecurityTopthreaten.OK(Serializer.deserialize content)
            | 400 -> return GetGraphSecurityTopthreaten.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraphSecurityTopthreaten.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraphSecurityTopthreaten.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get Counter of alert grouped by type of security alert. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    ///<param name="isThreat">Type of alert you want to aggregate. True is Threaten, false is other type. By default is True</param>
    member this.GetGraphSecurityByalarmtype
        (
            xAccessToken: string,
            accountId: string,
            ?unit: string,
            ?quantity: string,
            ?isThreat: bool
        ) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value)
                  if isThreat.IsSome then
                      RequestPart.query ("isThreat", isThreat.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/security/byalarmtype" headers requestParts

            match int status with
            | 200 -> return GetGraphSecurityByalarmtype.OK(Serializer.deserialize content)
            | 400 -> return GetGraphSecurityByalarmtype.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraphSecurityByalarmtype.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraphSecurityByalarmtype.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrive the information stored about eSIMs for an account and below.
    ///</summary>
    member this.GetEsims() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/esims" headers requestParts
            return GetEsims.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a new eSIM. Profiles must already exist, belong to same accountId and comply with all restrictions for eSIMs
    ///- One bootstrap profile per eSIM
    ///- One enabled profile per eSIM
    ///- Profiles not enabled, must be in disabled state
    ///- All profiles must belong to the same account that the eSIM
    ///</summary>
    member this.PostEsims(payload: PostEsimsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims" headers requestParts

            match int status with
            | 200 -> return PostEsims.OK(Serializer.deserialize content)
            | 400 -> return PostEsims.BadRequest(Serializer.deserialize content)
            | 401 -> return PostEsims.Unauthorized(Serializer.deserialize content)
            | _ -> return PostEsims.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrive the information stored about eSIMs for an account and below.
    ///</summary>
    member this.PostEsimsbulk() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/esimsbulk" headers requestParts
            return PostEsimsbulk.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///get eSIM info by EID
    ///</summary>
    member this.GetEsimsByEid() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/esims/{eid}" headers requestParts
            return GetEsimsByEid.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Change eSimName and/or eSimGroupName of the eSIM for the specified accountId
    ///</summary>
    member this.PutEsimsByEid(eSim: PutEsimsByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent eSim ]
            let! (status, content) = OpenApiHttp.putAsync url "/esims/{eid}" headers requestParts
            return PutEsimsByEid.OK
        }

    ///<summary>
    ///Transfer a ESim from a account to another
    ///</summary>
    member this.PostEsimsTransferByEid(payload: PostEsimsTransferByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/transfer" headers requestParts

            match int status with
            | 200 -> return PostEsimsTransferByEid.OK(Serializer.deserialize content)
            | 400 -> return PostEsimsTransferByEid.BadRequest(Serializer.deserialize content)
            | 401 -> return PostEsimsTransferByEid.Unauthorized(Serializer.deserialize content)
            | _ -> return PostEsimsTransferByEid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Return a ESim from a account to parent account
    ///</summary>
    member this.PostEsimsReturnByEid(payload: PostEsimsReturnByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/return" headers requestParts

            match int status with
            | 200 -> return PostEsimsReturnByEid.OK(Serializer.deserialize content)
            | 400 -> return PostEsimsReturnByEid.BadRequest(Serializer.deserialize content)
            | 401 -> return PostEsimsReturnByEid.Unauthorized(Serializer.deserialize content)
            | _ -> return PostEsimsReturnByEid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Activate the enabled profile and subscribe it to the assigned product.
    ///</summary>
    member this.PutEsimsSubscribeByEid(payload: PutEsimsSubscribeByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/esims/{eid}/subscribe" headers requestParts
            return PutEsimsSubscribeByEid.OK
        }

    ///<summary>
    ///Suspend enabled profile.
    ///</summary>
    member this.PutEsimsSuspendByEid(payload: PutEsimsSuspendByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/esims/{eid}/suspend" headers requestParts
            return PutEsimsSuspendByEid.OK
        }

    ///<summary>
    ///Reactivate enabled profile.
    ///</summary>
    member this.PutEsimsUnsuspendByEid(payload: PutEsimsUnsuspendByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/esims/{eid}/unsuspend" headers requestParts
            return PutEsimsUnsuspendByEid.OK
        }

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutEsimsAlertsByEid(payload: PutEsimsAlertsByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.putAsync url "/esims/{eid}/alerts" headers requestParts
            return PutEsimsAlertsByEid.OK
        }

    ///<summary>
    ///Reactivate enabled profile.
    ///</summary>
    member this.PostEsimsPurgeByEid(payload: PostEsimsPurgeByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/purge" headers requestParts
            return PostEsimsPurgeByEid.OK
        }

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostEsimsSmsByEid(message: PostEsimsSmsByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent message ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/sms" headers requestParts
            return PostEsimsSmsByEid.OK
        }

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostEsimsLimitByEid(payload: PostEsimsLimitByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/limit" headers requestParts
            return PostEsimsLimitByEid.OK
        }

    ///<summary>
    ///DownloadProfile (ES2) creates a new Issuer Security Domain - Profile (ISD-P) on an eSIM, and then downloads and installs the specified profile onto the card.
    ///Optionally, the installed profile can also be enabled as part of this process. Otherwise, the new profile is installed in the Disabled state.
    ///</summary>
    member this.PostEsimsDownloadProfileByEid(payload: PostEsimsDownloadProfileByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/download-profile" headers requestParts
            return PostEsimsDownloadProfileByEid.OK
        }

    ///<summary>
    ///Moves a profile that is installed on an eSIM from the Disabled state to the Enabled state.
    ///To enable the target profile, the profile that is currently active on the card must also be disabled.
    ///</summary>
    member this.PostEsimsEnableProfileByEid(payload: PostEsimsEnableProfileByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/enable-profile" headers requestParts
            return PostEsimsEnableProfileByEid.OK
        }

    ///<summary>
    ///Moves a profile that is installed on an eSIM from the Enabled state to the Disabled state.
    ///To ensure that card connectivity is maintained, disabling the active profile automatically enables the Bootstrap Profile.
    ///</summary>
    member this.PostEsimsDisableProfileByEid(payload: PostEsimsDisableProfileByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/disable-profile" headers requestParts
            return PostEsimsDisableProfileByEid.OK
        }

    ///<summary>
    ///Removes a profile from an eSIM.
    ///If the profile to be deleted is in the Enabled state, it is first moved to the Disabled state.
    ///Then, to ensure that card connectivity is maintained, the Bootstrap Profile is enabled.
    ///The Bootstrap Profiles (BS) have the fallback attribute, hence cannot be deleted.
    ///</summary>
    member this.PostEsimsDeleteProfileByEid(payload: PostEsimsDeleteProfileByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/delete-profile" headers requestParts
            return PostEsimsDeleteProfileByEid.OK
        }

    ///<summary>
    ///Audit an eSIM and all its downloaded profiles.
    ///</summary>
    member this.PostEsimsAuditByEid(payload: PostEsimsAuditByEidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/esims/{eid}/audit" headers requestParts
            return PostEsimsAuditByEid.OK
        }

    ///<summary>
    ///Change eSimName and/or eSimGroupName of the eSIMs for the specified accountId
    ///</summary>
    member this.PutBulkEsims(body: PutBulkEsimsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/esims" headers requestParts
            return PutBulkEsims.Accepted
        }

    ///<summary>
    ///Transfer an eSIM and its profiles from an account to another in bulk
    ///</summary>
    member this.PostBulkEsimsTransfer(body: PostBulkEsimsTransferPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/esims/transfer" headers requestParts
            return PostBulkEsimsTransfer.Accepted
        }

    ///<summary>
    ///Return a ESim from a account to parent account
    ///</summary>
    member this.PostBulkEsimsReturn(body: PostBulkEsimsReturnPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/esims/return" headers requestParts
            return PostBulkEsimsReturn.Accepted
        }

    ///<summary>
    ///Activate a bulk of enabled profiles and subscribe the assets to the assigned product.
    ///</summary>
    member this.PostBulkEsimsSubscribe(body: PostBulkEsimsSubscribePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/esims/subscribe" headers requestParts
            return PostBulkEsimsSubscribe.Accepted
        }

    ///<summary>
    ///Suspend a bulk of enabled profiles
    ///</summary>
    member this.PutBulkEsimsSuspend() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/esims/suspend" headers requestParts
            return PutBulkEsimsSuspend.Accepted
        }

    ///<summary>
    ///Reactivate a bulk of enabled profiles
    ///</summary>
    member this.PutBulkEsimsUnsuspend() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/esims/unsuspend" headers requestParts
            return PutBulkEsimsUnsuspend.Accepted
        }

    ///<summary>
    ///- You can set data alerts (alerts array) and sms alerts (smsAlerts array). You can set both or one of them.
    ///- Use `in_cost` alert to notify when the SIMs in bundle cost has reached the limit. Use `out_cost` alert to notify when the SIMs out of bundle costs (overuse) has reached the limit. Use `in_percent` alert to notify when the promotion / bundle depletion has reached certain percent set by the limit. Use `out_percent` alert to notify when the promotion / bundle overuse has reached certain percent set by the limit.
    ///- Use `desktop` to put a notification on the web platforms only. Use `email_default` to send a notification to the default email set on the web platforms, or `email_alternative` to send to the additional alternative email set on the web platforms. A desktop will always show even when email notification is set onlY.
    ///</summary>
    member this.PutBulkEsimsAlerts(body: PutBulkEsimsAlertsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync url "/bulk/esims/alerts" headers requestParts
            return PutBulkEsimsAlerts.Accepted
        }

    ///<summary>
    ///Forces an update location on the SIM. Useful to remotely reset the network connection on a SIM (refresh connection).
    ///</summary>
    member this.PostBulkEsimsPurge(body: PostBulkEsimsPurgePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/esims/purge" headers requestParts
            return PostBulkEsimsPurge.Accepted
        }

    ///<summary>
    ///Sends a message to the SIM limited to 160 characters. A single SMS can hold up to 160 characters using "Stardard" coding, some special characters are allowed but they consume double space. Using characters out of the standard set requires UCS-2 coding and the message size is limited to 70 characters.&amp;lt;br&amp;gt;&amp;lt;br&amp;gt;Use data coding scheme 240 for a Flash SMS, and 0 for a Standard SMS. A Flash SMS is displayed on the screen immediately upon arrival but is not saved or stored on the device. A Standard SMS is displayed and saved to the device
    ///</summary>
    member this.PostBulkEsimsSms(body: PostBulkEsimsSmsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/esims/sms" headers requestParts
            return PostBulkEsimsSms.Accepted
        }

    ///<summary>
    ///You can set the limit for data (limit or datalimit) or sms (smslimit). Is mandatory to set one of them at least, but you can set both.&amp;lt;br&amp;gt;Datalimit must be set on bytes.
    ///</summary>
    member this.PostBulkEsimsLimit(body: PostBulkEsimsLimitPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/bulk/esims/limit" headers requestParts
            return PostBulkEsimsLimit.Accepted
        }

    ///<summary>
    ///Get bytes consumed by an account.Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph1(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/1" headers requestParts

            match int status with
            | 200 -> return GetGraph1.OK(Serializer.deserialize content)
            | 400 -> return GetGraph1.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph1.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph1.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get amount of cdrs by an account.Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph2(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/2" headers requestParts

            match int status with
            | 200 -> return GetGraph2.OK(Serializer.deserialize content)
            | 400 -> return GetGraph2.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph2.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph2.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get amount of cdrs aggregated by the mcc in an account. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph3(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/3" headers requestParts

            match int status with
            | 200 -> return GetGraph3.OK(Serializer.deserialize content)
            | 400 -> return GetGraph3.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph3.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph3.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get amount of cdrs aggregated by the mcc and the mnc in an account. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph4(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/4" headers requestParts

            match int status with
            | 200 -> return GetGraph4.OK(Serializer.deserialize content)
            | 400 -> return GetGraph4.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph4.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph4.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get CDRs aggregated by Products in an account. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph5(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/5" headers requestParts

            match int status with
            | 200 -> return GetGraph5.OK(Serializer.deserialize content)
            | 400 -> return GetGraph5.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph5.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph5.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get bytes consumed and aggregated by ICCID in an account. Type Table
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph6(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/6" headers requestParts

            match int status with
            | 200 -> return GetGraph6.OK(Serializer.deserialize content)
            | 400 -> return GetGraph6.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph6.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph6.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get CDRs aggregated by ICCID in an account. Type Table
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph7(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/7" headers requestParts

            match int status with
            | 200 -> return GetGraph7.OK(Serializer.deserialize content)
            | 400 -> return GetGraph7.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph7.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph7.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get bytes consumed and aggregated by account for the current month and the previous month. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph8(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/8" headers requestParts

            match int status with
            | 200 -> return GetGraph8.OK(Serializer.deserialize content)
            | 400 -> return GetGraph8.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph8.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph8.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get CDRs aggregated by account for the current month and the previous month. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph9(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/9" headers requestParts

            match int status with
            | 200 -> return GetGraph9.OK(Serializer.deserialize content)
            | 400 -> return GetGraph9.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph9.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph9.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get stats of products by account for the current month. The period is from the first day of the current month to today. Type Table
    ///</summary>
    member this.GetGraph10(xAccessToken: string, accountId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/10" headers requestParts

            match int status with
            | 200 -> return GetGraph10.OK(Serializer.deserialize content)
            | 400 -> return GetGraph10.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph10.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph10.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get status of simcards by account. Type Pie
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph11(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/11" headers requestParts

            match int status with
            | 200 -> return GetGraph11.OK(Serializer.deserialize content)
            | 400 -> return GetGraph11.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph11.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph11.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get cost consumed by an account. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraph12(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/12" headers requestParts

            match int status with
            | 200 -> return GetGraph12.OK(Serializer.deserialize content)
            | 400 -> return GetGraph12.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraph12.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraph12.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get the simcard status per day by an account. Type Line
    ///</summary>
    ///<param name="xAccessToken"></param>
    ///<param name="accountId"></param>
    ///<param name="unit">Unit of time. It is a range of time what you will search. By default is hours ('h')</param>
    ///<param name="quantity">Quantity of time. How many units you will search. By default is 7</param>
    member this.GetGraphStatusesperday(xAccessToken: string, accountId: string, ?unit: string, ?quantity: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId)
                  if unit.IsSome then
                      RequestPart.query ("unit", unit.Value)
                  if quantity.IsSome then
                      RequestPart.query ("quantity", quantity.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/graph/statusesperday" headers requestParts

            match int status with
            | 200 -> return GetGraphStatusesperday.OK(Serializer.deserialize content)
            | 400 -> return GetGraphStatusesperday.BadRequest(Serializer.deserialize content)
            | 401 -> return GetGraphStatusesperday.Unauthorized(Serializer.deserialize content)
            | _ -> return GetGraphStatusesperday.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get components that are not fully operational and are being used in this account
    ///</summary>
    member this.GetStatusComponents(xAccessToken: string, accountId: string) =
        async {
            let requestParts =
                [ RequestPart.header ("x-access-token", xAccessToken)
                  RequestPart.query ("accountId", accountId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/status/components" headers requestParts
            return GetStatusComponents.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Add a short dial entry
    ///</summary>
    member this.PostAssetsQuickDialByIccid(payload: PostAssetsQuickDialByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/quick-dial" headers requestParts

            match int status with
            | 200 -> return PostAssetsQuickDialByIccid.OK(Serializer.deserialize content)
            | 400 -> return PostAssetsQuickDialByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAssetsQuickDialByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAssetsQuickDialByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a list of all quick dial entries for the SIM
    ///</summary>
    member this.GetAssetsQuickDialByIccid() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/assets/{iccid}/quick-dial" headers requestParts

            match int status with
            | 200 -> return GetAssetsQuickDialByIccid.OK(Serializer.deserialize content)
            | 400 -> return GetAssetsQuickDialByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAssetsQuickDialByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAssetsQuickDialByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Show an individual quick dial entry in the SIM
    ///</summary>
    member this.GetAssetsQuickDialByIccidAndLocation() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync url "/assets/{iccid}/quick-dial/{location}" headers requestParts

            match int status with
            | 200 -> return GetAssetsQuickDialByIccidAndLocation.OK(Serializer.deserialize content)
            | 400 -> return GetAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
            | 401 -> return GetAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
            | _ -> return GetAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Update a short dial entry
    ///</summary>
    member this.PutAssetsQuickDialByIccidAndLocation(payload: PutAssetsQuickDialByIccidAndLocationPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]

            let! (status, content) =
                OpenApiHttp.putAsync url "/assets/{iccid}/quick-dial/{location}" headers requestParts

            match int status with
            | 200 -> return PutAssetsQuickDialByIccidAndLocation.OK(Serializer.deserialize content)
            | 400 -> return PutAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
            | 401 -> return PutAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
            | _ -> return PutAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Remove a short dial entry. Use "all" as location param if all locations for an user need to be removed.
    ///</summary>
    member this.DeleteAssetsQuickDialByIccidAndLocation() =
        async {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/assets/{iccid}/quick-dial/{location}" headers requestParts

            match int status with
            | 204 -> return DeleteAssetsQuickDialByIccidAndLocation.NoContent
            | 400 -> return DeleteAssetsQuickDialByIccidAndLocation.BadRequest(Serializer.deserialize content)
            | 401 -> return DeleteAssetsQuickDialByIccidAndLocation.Unauthorized(Serializer.deserialize content)
            | _ -> return DeleteAssetsQuickDialByIccidAndLocation.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Forces to make a MT voice call to a SIM. This function is used to make the client perform a task when it gets a call.
    ///</summary>
    member this.PostAssetsDialByIccid(payload: PostAssetsDialByIccidPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/assets/{iccid}/dial" headers requestParts

            match int status with
            | 200 -> return PostAssetsDialByIccid.OK(Serializer.deserialize content)
            | 400 -> return PostAssetsDialByIccid.BadRequest(Serializer.deserialize content)
            | 401 -> return PostAssetsDialByIccid.Unauthorized(Serializer.deserialize content)
            | _ -> return PostAssetsDialByIccid.InternalServerError(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrive the information stored about all steering lists for an account.
    ///</summary>
    member this.GetSteeringlists() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/steeringlists" headers requestParts
            return GetSteeringlists.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a new steering list
    ///</summary>
    member this.PostSteeringlists(payload: PostSteeringlistsPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent payload ]
            let! (status, content) = OpenApiHttp.postAsync url "/steeringlists" headers requestParts
            return PostSteeringlists.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete a steering list
    ///</summary>
    member this.DeleteSteeringlistsBySteeringListId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.deleteAsync url "/steeringlists/{steeringListId}" headers requestParts
            return DeleteSteeringlistsBySteeringListId.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Retrive the information stored about campaigns for an account.
    ///</summary>
    member this.GetCampaigns() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/campaigns" headers requestParts
            return GetCampaigns.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Create a campaign
    ///</summary>
    member this.PostCampaigns() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/campaigns" headers requestParts
            return PostCampaigns.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Edit a campaign
    ///</summary>
    member this.PutCampaigns() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.putAsync url "/campaigns" headers requestParts
            return PutCampaigns.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete a campaign
    ///</summary>
    member this.DeleteCampaigns() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.deleteAsync url "/campaigns" headers requestParts
            return DeleteCampaigns.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Get a campaign
    ///</summary>
    member this.GetCampaignsByCampaignId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/campaigns/{campaignId}" headers requestParts
            return GetCampaignsByCampaignId.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Get campaign completion forecast
    ///</summary>
    member this.PostCampaignsCompletionForecast() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/campaigns/completion-forecast" headers requestParts
            return PostCampaignsCompletionForecast.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Get items of a campaign
    ///</summary>
    member this.GetCampaignsItemsByCampaignId() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/campaigns/{campaignId}/items" headers requestParts
            return GetCampaignsItemsByCampaignId.OK(Serializer.deserialize content)
        }
