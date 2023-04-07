namespace rec SyncPetStore

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open SyncPetStore.Types
open SyncPetStore.Http

///This is a sample Pet Store Server based on the OpenAPI 3.0 specification.  You can find out more about
///Swagger at [http://swagger.io](http://swagger.io). In the third iteration of the pet store, we've switched to the design first approach!
///You can now help us improve the API whether it's by making changes to the definition itself or to the code.
///That way, with time, we can improve the API in general, and expose some of the new features in OAS3.
///Some useful links:
///- [The Pet Store repository](https://github.com/swagger-api/swagger-petstore)
///- [The source API definition for the Pet Store](https://github.com/swagger-api/swagger-petstore/blob/master/src/main/resources/openapi.yaml)
type SyncPetStoreClient(httpClient: HttpClient) =
    ///<summary>
    ///Update an existing pet by Id
    ///</summary>
    member this.UpdatePet(body: Pet, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/pet" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            UpdatePet.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            UpdatePet.BadRequest
        else if status = HttpStatusCode.NotFound then
            UpdatePet.NotFound
        else
            UpdatePet.MethodNotAllowed

    ///<summary>
    ///Add a new pet to the store
    ///</summary>
    member this.AddPet(body: Pet, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/pet" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            AddPet.OK(Serializer.deserialize content)
        else
            AddPet.MethodNotAllowed

    ///<summary>
    ///Multiple status values can be provided with comma separated strings
    ///</summary>
    ///<param name="status">Status values that need to be considered for filter</param>
    ///<param name="cancellationToken"></param>
    member this.FindPetsByStatus(?status: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if status.IsSome then
                  RequestPart.query ("status", status.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/pet/findByStatus" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            FindPetsByStatus.OK(Serializer.deserialize content)
        else
            FindPetsByStatus.BadRequest

    ///<summary>
    ///Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
    ///</summary>
    ///<param name="tags">Tags to filter by</param>
    ///<param name="cancellationToken"></param>
    member this.FindPetsByTags(?tags: list<string>, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if tags.IsSome then
                  RequestPart.query ("tags", tags.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/pet/findByTags" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            FindPetsByTags.OK(Serializer.deserialize content)
        else
            FindPetsByTags.BadRequest

    ///<summary>
    ///Returns a single pet
    ///</summary>
    ///<param name="petId">ID of pet to return</param>
    ///<param name="cancellationToken"></param>
    member this.GetPetById(petId: int64, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.path ("petId", petId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/pet/{petId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetPetById.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetPetById.BadRequest
        else
            GetPetById.NotFound

    ///<summary>
    ///Updates a pet in the store with form data
    ///</summary>
    ///<param name="petId">ID of pet that needs to be updated</param>
    ///<param name="name">Name of pet that needs to be updated</param>
    ///<param name="status">Status of pet that needs to be updated</param>
    ///<param name="cancellationToken"></param>
    member this.UpdatePetWithForm(petId: int64, ?name: string, ?status: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("petId", petId)
              if name.IsSome then
                  RequestPart.query ("name", name.Value)
              if status.IsSome then
                  RequestPart.query ("status", status.Value) ]

        let (status, content) =
            OpenApiHttp.post httpClient "/pet/{petId}" requestParts cancellationToken

        if status = HttpStatusCode.MethodNotAllowed then
            UpdatePetWithForm.MethodNotAllowed
        else
            UpdatePetWithForm.DefaultResponse

    ///<summary>
    ///Deletes a pet
    ///</summary>
    ///<param name="petId">Pet id to delete</param>
    ///<param name="apiKey"></param>
    ///<param name="cancellationToken"></param>
    member this.DeletePet(petId: int64, ?apiKey: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("petId", petId)
              if apiKey.IsSome then
                  RequestPart.header ("api_key", apiKey.Value) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/pet/{petId}" requestParts cancellationToken

        if status = HttpStatusCode.BadRequest then
            DeletePet.BadRequest
        else
            DeletePet.DefaultResponse

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
        let requestParts =
            [ RequestPart.path ("petId", petId)
              if additionalMetadata.IsSome then
                  RequestPart.query ("additionalMetadata", additionalMetadata.Value)
              if requestBody.IsSome then
                  RequestPart.binaryContent requestBody.Value ]

        let (status, content) =
            OpenApiHttp.post httpClient "/pet/{petId}/uploadImage" requestParts cancellationToken

        UploadFile.OK(Serializer.deserialize content)

    ///<summary>
    ///Returns a map of status codes to quantities
    ///</summary>
    member this.GetInventory(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/store/inventory" requestParts cancellationToken

        GetInventory.OK(Serializer.deserialize content)

    ///<summary>
    ///Place a new order in the store
    ///</summary>
    member this.PlaceOrder(body: Order, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/store/order" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            PlaceOrder.OK(Serializer.deserialize content)
        else
            PlaceOrder.MethodNotAllowed

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt;= 5 or &amp;gt; 10. Other values will generate exceptions.
    ///</summary>
    ///<param name="orderId">ID of order that needs to be fetched</param>
    ///<param name="cancellationToken"></param>
    member this.GetOrderById(orderId: int64, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("orderId", orderId) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/store/order/{orderId}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetOrderById.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetOrderById.BadRequest
        else
            GetOrderById.NotFound

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt; 1000. Anything above 1000 or nonintegers will generate API errors
    ///</summary>
    ///<param name="orderId">ID of the order that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteOrder(orderId: int64, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("orderId", orderId) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/store/order/{orderId}" requestParts cancellationToken

        if status = HttpStatusCode.BadRequest then
            DeleteOrder.BadRequest
        else if status = HttpStatusCode.NotFound then
            DeleteOrder.NotFound
        else
            DeleteOrder.DefaultResponse

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    member this.CreateUser(body: User, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/user" requestParts cancellationToken

        CreateUser.DefaultResponse(Serializer.deserialize content)

    ///<summary>
    ///Creates list of users with given input array
    ///</summary>
    member this.CreateUsersWithListInput(body: CreateUsersWithListInputPayload, ?cancellationToken: CancellationToken) =
        let requestParts = [ RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.post httpClient "/user/createWithList" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            CreateUsersWithListInput.OK(Serializer.deserialize content)
        else
            CreateUsersWithListInput.DefaultResponse

    ///<summary>
    ///Logs user into the system
    ///</summary>
    ///<param name="username">The user name for login</param>
    ///<param name="password">The password for login in clear text</param>
    ///<param name="cancellationToken"></param>
    member this.LoginUser(?username: string, ?password: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ if username.IsSome then
                  RequestPart.query ("username", username.Value)
              if password.IsSome then
                  RequestPart.query ("password", password.Value) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/user/login" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            LoginUser.OK content
        else
            LoginUser.BadRequest

    ///<summary>
    ///Logs out current logged in user session
    ///</summary>
    member this.LogoutUser(?cancellationToken: CancellationToken) =
        let requestParts = []

        let (status, content) =
            OpenApiHttp.get httpClient "/user/logout" requestParts cancellationToken

        LogoutUser.DefaultResponse

    ///<summary>
    ///Get user by user name
    ///</summary>
    ///<param name="username">The name that needs to be fetched. Use user1 for testing. </param>
    ///<param name="cancellationToken"></param>
    member this.GetUserByName(username: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("username", username) ]

        let (status, content) =
            OpenApiHttp.get httpClient "/user/{username}" requestParts cancellationToken

        if status = HttpStatusCode.OK then
            GetUserByName.OK(Serializer.deserialize content)
        else if status = HttpStatusCode.BadRequest then
            GetUserByName.BadRequest
        else
            GetUserByName.NotFound

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">name that need to be deleted</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.UpdateUser(username: string, body: User, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("username", username)
              RequestPart.jsonContent body ]

        let (status, content) =
            OpenApiHttp.put httpClient "/user/{username}" requestParts cancellationToken

        UpdateUser.DefaultResponse

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">The name that needs to be deleted</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteUser(username: string, ?cancellationToken: CancellationToken) =
        let requestParts =
            [ RequestPart.path ("username", username) ]

        let (status, content) =
            OpenApiHttp.delete httpClient "/user/{username}" requestParts cancellationToken

        if status = HttpStatusCode.BadRequest then
            DeleteUser.BadRequest
        else if status = HttpStatusCode.NotFound then
            DeleteUser.NotFound
        else
            DeleteUser.DefaultResponse
