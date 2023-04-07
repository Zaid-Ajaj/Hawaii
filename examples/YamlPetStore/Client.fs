namespace rec YamlPetStore

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open YamlPetStore.Types
open YamlPetStore.Http

///This is a sample Pet Store Server based on the OpenAPI 3.0 specification.  You can find out more about
///Swagger at [http://swagger.io](http://swagger.io). In the third iteration of the pet store, we've switched to the design first approach!
///You can now help us improve the API whether it's by making changes to the definition itself or to the code.
///That way, with time, we can improve the API in general, and expose some of the new features in OAS3.
///Some useful links:
///- [The Pet Store repository](https://github.com/swagger-api/swagger-petstore)
///- [The source API definition for the Pet Store](https://github.com/swagger-api/swagger-petstore/blob/master/src/main/resources/openapi.yaml)
type YamlPetStoreClient(httpClient: HttpClient) =
    ///<summary>
    ///Update an existing pet by Id
    ///</summary>
    member this.UpdatePet(body: Pet, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync httpClient "/pet" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return UpdatePet.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return UpdatePet.BadRequest
            else if status = HttpStatusCode.NotFound then
                return UpdatePet.NotFound
            else
                return UpdatePet.MethodNotAllowed
        }

    ///<summary>
    ///Add a new pet to the store
    ///</summary>
    member this.AddPet(body: Pet, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/pet" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return AddPet.OK(Serializer.deserialize content)
            else
                return AddPet.MethodNotAllowed
        }

    ///<summary>
    ///Multiple status values can be provided with comma separated strings
    ///</summary>
    ///<param name="status">Status values that need to be considered for filter</param>
    ///<param name="cancellationToken"></param>
    member this.FindPetsByStatus(?status: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if status.IsSome then
                      RequestPart.query ("status", status.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/pet/findByStatus" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return FindPetsByStatus.OK(Serializer.deserialize content)
            else
                return FindPetsByStatus.BadRequest
        }

    ///<summary>
    ///Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
    ///</summary>
    ///<param name="tags">Tags to filter by</param>
    ///<param name="cancellationToken"></param>
    member this.FindPetsByTags(?tags: list<string>, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if tags.IsSome then
                      RequestPart.query ("tags", tags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/pet/findByTags" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return FindPetsByTags.OK(Serializer.deserialize content)
            else
                return FindPetsByTags.BadRequest
        }

    ///<summary>
    ///Returns a single pet
    ///</summary>
    ///<param name="petId">ID of pet to return</param>
    ///<param name="cancellationToken"></param>
    member this.GetPetById(petId: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.path ("petId", petId) ]
            let! (status, content) = OpenApiHttp.getAsync httpClient "/pet/{petId}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetPetById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetPetById.BadRequest
            else
                return GetPetById.NotFound
        }

    ///<summary>
    ///Updates a pet in the store with form data
    ///</summary>
    ///<param name="petId">ID of pet that needs to be updated</param>
    ///<param name="name">Name of pet that needs to be updated</param>
    ///<param name="status">Status of pet that needs to be updated</param>
    ///<param name="cancellationToken"></param>
    member this.UpdatePetWithForm(petId: int64, ?name: string, ?status: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("petId", petId)
                  if name.IsSome then
                      RequestPart.query ("name", name.Value)
                  if status.IsSome then
                      RequestPart.query ("status", status.Value) ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/pet/{petId}" requestParts cancellationToken

            if status = HttpStatusCode.MethodNotAllowed then
                return UpdatePetWithForm.MethodNotAllowed
            else
                return UpdatePetWithForm.DefaultResponse
        }

    ///<summary>
    ///Deletes a pet
    ///</summary>
    ///<param name="petId">Pet id to delete</param>
    ///<param name="apiKey"></param>
    ///<param name="cancellationToken"></param>
    member this.DeletePet(petId: int64, ?apiKey: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("petId", petId)
                  if apiKey.IsSome then
                      RequestPart.header ("api_key", apiKey.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/pet/{petId}" requestParts cancellationToken

            if status = HttpStatusCode.BadRequest then
                return DeletePet.BadRequest
            else
                return DeletePet.DefaultResponse
        }

    ///<summary>
    ///uploads an image
    ///</summary>
    ///<param name="petId">ID of pet to update</param>
    ///<param name="additionalMetadata">Additional Metadata</param>
    ///<param name="cancellationToken"></param>
    ///<param name="requestBody"></param>
    member this.UploadFile
        (
            petId: int64,
            ?additionalMetadata: string,
            ?cancellationToken: CancellationToken,
            ?requestBody: byte []
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("petId", petId)
                  if additionalMetadata.IsSome then
                      RequestPart.query ("additionalMetadata", additionalMetadata.Value)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/pet/{petId}/uploadImage" requestParts cancellationToken

            return UploadFile.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Returns a map of status codes to quantities
    ///</summary>
    member this.GetInventory(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/store/inventory" requestParts cancellationToken
            return GetInventory.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Place a new order in the store
    ///</summary>
    member this.PlaceOrder(body: Order, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/store/order" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PlaceOrder.OK(Serializer.deserialize content)
            else
                return PlaceOrder.MethodNotAllowed
        }

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt;= 5 or &amp;gt; 10. Other values will generate exceptions.
    ///</summary>
    ///<param name="orderId">ID of order that needs to be fetched</param>
    ///<param name="cancellationToken"></param>
    member this.GetOrderById(orderId: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("orderId", orderId) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/store/order/{orderId}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetOrderById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetOrderById.BadRequest
            else
                return GetOrderById.NotFound
        }

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt; 1000. Anything above 1000 or nonintegers will generate API errors
    ///</summary>
    ///<param name="orderId">ID of the order that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteOrder(orderId: int64, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("orderId", orderId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/store/order/{orderId}" requestParts cancellationToken

            if status = HttpStatusCode.BadRequest then
                return DeleteOrder.BadRequest
            else if status = HttpStatusCode.NotFound then
                return DeleteOrder.NotFound
            else
                return DeleteOrder.DefaultResponse
        }

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    member this.CreateUser(body: User, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/user" requestParts cancellationToken
            return CreateUser.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Creates list of users with given input array
    ///</summary>
    member this.CreateUsersWithListInput(body: CreateUsersWithListInputPayload, ?cancellationToken: CancellationToken) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/user/createWithList" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return CreateUsersWithListInput.OK(Serializer.deserialize content)
            else
                return CreateUsersWithListInput.DefaultResponse
        }

    ///<summary>
    ///Logs user into the system
    ///</summary>
    ///<param name="username">The user name for login</param>
    ///<param name="password">The password for login in clear text</param>
    ///<param name="cancellationToken"></param>
    member this.LoginUser(?username: string, ?password: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ if username.IsSome then
                      RequestPart.query ("username", username.Value)
                  if password.IsSome then
                      RequestPart.query ("password", password.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/user/login" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return LoginUser.OK content
            else
                return LoginUser.BadRequest
        }

    ///<summary>
    ///Logs out current logged in user session
    ///</summary>
    member this.LogoutUser(?cancellationToken: CancellationToken) =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/user/logout" requestParts cancellationToken
            return LogoutUser.DefaultResponse
        }

    ///<summary>
    ///Get user by user name
    ///</summary>
    ///<param name="username">The name that needs to be fetched. Use user1 for testing. </param>
    ///<param name="cancellationToken"></param>
    member this.GetUserByName(username: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("username", username) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/user/{username}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetUserByName.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.BadRequest then
                return GetUserByName.BadRequest
            else
                return GetUserByName.NotFound
        }

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">name that need to be deleted</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateUser(username: string, body: User, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("username", username)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync httpClient "/user/{username}" requestParts cancellationToken
            return UpdateUser.DefaultResponse
        }

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">The name that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteUser(username: string, ?cancellationToken: CancellationToken) =
        async {
            let requestParts =
                [ RequestPart.path ("username", username) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/user/{username}" requestParts cancellationToken

            if status = HttpStatusCode.BadRequest then
                return DeleteUser.BadRequest
            else if status = HttpStatusCode.NotFound then
                return DeleteUser.NotFound
            else
                return DeleteUser.DefaultResponse
        }
