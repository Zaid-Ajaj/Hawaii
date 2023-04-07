namespace rec FablePetStore

open Browser.Types
open Fable.SimpleHttp
open FablePetStore.Types
open FablePetStore.Http

///This is a sample Pet Store Server based on the OpenAPI 3.0 specification.  You can find out more about
///Swagger at [http://swagger.io](http://swagger.io). In the third iteration of the pet store, we've switched to the design first approach!
///You can now help us improve the API whether it's by making changes to the definition itself or to the code.
///That way, with time, we can improve the API in general, and expose some of the new features in OAS3.
///Some useful links:
///- [The Pet Store repository](https://github.com/swagger-api/swagger-petstore)
///- [The source API definition for the Pet Store](https://github.com/swagger-api/swagger-petstore/blob/master/src/main/resources/openapi.yaml)
type FablePetStoreClient(url: string, headers: list<Header>) =
    new(url: string) = FablePetStoreClient(url, [])

    ///<summary>
    ///Update an existing pet by Id
    ///</summary>
    member this.UpdatePet(body: Pet) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.putAsync url "/pet" headers requestParts

            if status = 200 then
                return UpdatePet.OK(Serializer.deserialize content)
            else if status = 400 then
                return UpdatePet.BadRequest
            else if status = 404 then
                return UpdatePet.NotFound
            else
                return UpdatePet.MethodNotAllowed
        }

    ///<summary>
    ///Add a new pet to the store
    ///</summary>
    member this.AddPet(body: Pet) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/pet" headers requestParts

            if status = 200 then
                return AddPet.OK(Serializer.deserialize content)
            else
                return AddPet.MethodNotAllowed
        }

    ///<summary>
    ///Multiple status values can be provided with comma separated strings
    ///</summary>
    ///<param name="status">Status values that need to be considered for filter</param>
    member this.FindPetsByStatus(?status: string) =
        async {
            let requestParts =
                [ if status.IsSome then
                      RequestPart.query ("status", status.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/pet/findByStatus" headers requestParts

            if status = 200 then
                return FindPetsByStatus.OK(Serializer.deserialize content)
            else
                return FindPetsByStatus.BadRequest
        }

    ///<summary>
    ///Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
    ///</summary>
    ///<param name="tags">Tags to filter by</param>
    member this.FindPetsByTags(?tags: list<string>) =
        async {
            let requestParts =
                [ if tags.IsSome then
                      RequestPart.query ("tags", tags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/pet/findByTags" headers requestParts

            if status = 200 then
                return FindPetsByTags.OK(Serializer.deserialize content)
            else
                return FindPetsByTags.BadRequest
        }

    ///<summary>
    ///Returns a single pet
    ///</summary>
    ///<param name="petId">ID of pet to return</param>
    member this.GetPetById(petId: int64) =
        async {
            let requestParts = [ RequestPart.path ("petId", petId) ]
            let! (status, content) = OpenApiHttp.getAsync url "/pet/{petId}" headers requestParts

            if status = 200 then
                return GetPetById.OK(Serializer.deserialize content)
            else if status = 400 then
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
    member this.UpdatePetWithForm(petId: int64, ?name: string, ?status: string) =
        async {
            let requestParts =
                [ RequestPart.path ("petId", petId)
                  if name.IsSome then
                      RequestPart.query ("name", name.Value)
                  if status.IsSome then
                      RequestPart.query ("status", status.Value) ]

            let! (status, content) = OpenApiHttp.postAsync url "/pet/{petId}" headers requestParts

            if status = 405 then
                return UpdatePetWithForm.MethodNotAllowed
            else
                return UpdatePetWithForm.DefaultResponse
        }

    ///<summary>
    ///Deletes a pet
    ///</summary>
    ///<param name="petId">Pet id to delete</param>
    ///<param name="apiKey"></param>
    member this.DeletePet(petId: int64, ?apiKey: string) =
        async {
            let requestParts =
                [ RequestPart.path ("petId", petId)
                  if apiKey.IsSome then
                      RequestPart.header ("api_key", apiKey.Value) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/pet/{petId}" headers requestParts

            if status = 400 then
                return DeletePet.BadRequest
            else
                return DeletePet.DefaultResponse
        }

    ///<summary>
    ///uploads an image
    ///</summary>
    ///<param name="petId">ID of pet to update</param>
    ///<param name="additionalMetadata">Additional Metadata</param>
    ///<param name="requestBody"></param>
    member this.UploadFile(petId: int64, ?additionalMetadata: string, ?requestBody: byte []) =
        async {
            let requestParts =
                [ RequestPart.path ("petId", petId)
                  if additionalMetadata.IsSome then
                      RequestPart.query ("additionalMetadata", additionalMetadata.Value)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) = OpenApiHttp.postAsync url "/pet/{petId}/uploadImage" headers requestParts
            return UploadFile.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Returns a map of status codes to quantities
    ///</summary>
    member this.GetInventory() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/store/inventory" headers requestParts
            return GetInventory.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Place a new order in the store
    ///</summary>
    member this.PlaceOrder(body: Order) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/store/order" headers requestParts

            if status = 200 then
                return PlaceOrder.OK(Serializer.deserialize content)
            else
                return PlaceOrder.MethodNotAllowed
        }

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt;= 5 or &amp;gt; 10. Other values will generate exceptions.
    ///</summary>
    ///<param name="orderId">ID of order that needs to be fetched</param>
    member this.GetOrderById(orderId: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("orderId", orderId) ]

            let! (status, content) = OpenApiHttp.getAsync url "/store/order/{orderId}" headers requestParts

            if status = 200 then
                return GetOrderById.OK(Serializer.deserialize content)
            else if status = 400 then
                return GetOrderById.BadRequest
            else
                return GetOrderById.NotFound
        }

    ///<summary>
    ///For valid response try integer IDs with value &amp;lt; 1000. Anything above 1000 or nonintegers will generate API errors
    ///</summary>
    ///<param name="orderId">ID of the order that needs to be deleted</param>
    member this.DeleteOrder(orderId: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("orderId", orderId) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/store/order/{orderId}" headers requestParts

            if status = 400 then
                return DeleteOrder.BadRequest
            else if status = 404 then
                return DeleteOrder.NotFound
            else
                return DeleteOrder.DefaultResponse
        }

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    member this.CreateUser(body: User) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/user" headers requestParts
            return CreateUser.DefaultResponse(Serializer.deserialize content)
        }

    ///<summary>
    ///Creates list of users with given input array
    ///</summary>
    member this.CreateUsersWithListInput(body: CreateUsersWithListInputPayload) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/user/createWithList" headers requestParts

            if status = 200 then
                return CreateUsersWithListInput.OK(Serializer.deserialize content)
            else
                return CreateUsersWithListInput.DefaultResponse
        }

    ///<summary>
    ///Logs user into the system
    ///</summary>
    ///<param name="username">The user name for login</param>
    ///<param name="password">The password for login in clear text</param>
    member this.LoginUser(?username: string, ?password: string) =
        async {
            let requestParts =
                [ if username.IsSome then
                      RequestPart.query ("username", username.Value)
                  if password.IsSome then
                      RequestPart.query ("password", password.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/user/login" headers requestParts

            if status = 200 then
                return LoginUser.OK content
            else
                return LoginUser.BadRequest
        }

    ///<summary>
    ///Logs out current logged in user session
    ///</summary>
    member this.LogoutUser() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/user/logout" headers requestParts
            return LogoutUser.DefaultResponse
        }

    ///<summary>
    ///Get user by user name
    ///</summary>
    ///<param name="username">The name that needs to be fetched. Use user1 for testing. </param>
    member this.GetUserByName(username: string) =
        async {
            let requestParts =
                [ RequestPart.path ("username", username) ]

            let! (status, content) = OpenApiHttp.getAsync url "/user/{username}" headers requestParts

            if status = 200 then
                return GetUserByName.OK(Serializer.deserialize content)
            else if status = 400 then
                return GetUserByName.BadRequest
            else
                return GetUserByName.NotFound
        }

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">name that need to be deleted</param>
    ///<param name="body"></param>
    member this.UpdateUser(username: string, body: User) =
        async {
            let requestParts =
                [ RequestPart.path ("username", username)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.putAsync url "/user/{username}" headers requestParts
            return UpdateUser.DefaultResponse
        }

    ///<summary>
    ///This can only be done by the logged in user.
    ///</summary>
    ///<param name="username">The name that needs to be deleted</param>
    member this.DeleteUser(username: string) =
        async {
            let requestParts =
                [ RequestPart.path ("username", username) ]

            let! (status, content) = OpenApiHttp.deleteAsync url "/user/{username}" headers requestParts

            if status = 400 then
                return DeleteUser.BadRequest
            else if status = 404 then
                return DeleteUser.NotFound
            else
                return DeleteUser.DefaultResponse
        }
