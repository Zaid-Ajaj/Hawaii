namespace rec DefaultPetStore.Types

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Status =
    | [<CompiledName "placed">] Placed
    | [<CompiledName "approved">] Approved
    | [<CompiledName "delivered">] Delivered
    member this.Format() =
        match this with
        | Placed -> "placed"
        | Approved -> "approved"
        | Delivered -> "delivered"

type Order =
    { id: Option<int64>
      petId: Option<int64>
      quantity: Option<int>
      shipDate: Option<System.DateTimeOffset>
      ///Order Status
      status: Option<Status>
      complete: Option<bool> }
    ///Creates an instance of Order with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Order =
        { id = None
          petId = None
          quantity = None
          shipDate = None
          status = None
          complete = None }

type Customer =
    { id: Option<int64>
      username: Option<string>
      address: Option<list<Address>> }
    ///Creates an instance of Customer with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Customer =
        { id = None
          username = None
          address = None }

type Address =
    { street: Option<string>
      city: Option<string>
      state: Option<string>
      zip: Option<string> }
    ///Creates an instance of Address with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Address =
        { street = None
          city = None
          state = None
          zip = None }

type Category =
    { id: Option<int64>
      name: Option<string> }
    ///Creates an instance of Category with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Category = { id = None; name = None }

type User =
    { id: Option<int64>
      username: Option<string>
      firstName: Option<string>
      lastName: Option<string>
      email: Option<string>
      password: Option<string>
      phone: Option<string>
      ///User Status
      userStatus: Option<int> }
    ///Creates an instance of User with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): User =
        { id = None
          username = None
          firstName = None
          lastName = None
          email = None
          password = None
          phone = None
          userStatus = None }

type Tag =
    { id: Option<int64>
      name: Option<string> }
    ///Creates an instance of Tag with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Tag = { id = None; name = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PetStatus =
    | [<CompiledName "available">] Available
    | [<CompiledName "pending">] Pending
    | [<CompiledName "sold">] Sold
    member this.Format() =
        match this with
        | Available -> "available"
        | Pending -> "pending"
        | Sold -> "sold"

type Pet =
    { id: Option<int64>
      name: string
      category: Option<Category>
      photoUrls: list<string>
      tags: Option<list<Tag>>
      ///pet status in the store
      status: Option<PetStatus> }
    ///Creates an instance of Pet with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (name: string, photoUrls: list<string>): Pet =
        { id = None
          name = name
          category = None
          photoUrls = photoUrls
          tags = None
          status = None }

type ApiResponse =
    { code: Option<int>
      ``type``: Option<string>
      message: Option<string> }
    ///Creates an instance of ApiResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ApiResponse =
        { code = None
          ``type`` = None
          message = None }

[<RequireQualifiedAccess>]
type UpdatePet =
    ///Successful operation
    | OK of payload: Pet
    ///Invalid ID supplied
    | BadRequest
    ///Pet not found
    | NotFound
    ///Validation exception
    | MethodNotAllowed

[<RequireQualifiedAccess>]
type AddPet =
    ///Successful operation
    | OK of payload: Pet
    ///Invalid input
    | MethodNotAllowed

[<RequireQualifiedAccess>]
type FindPetsByStatus =
    ///successful operation
    | OK of payload: list<Pet>
    ///Invalid status value
    | BadRequest

[<RequireQualifiedAccess>]
type FindPetsByTags =
    ///successful operation
    | OK of payload: list<Pet>
    ///Invalid tag value
    | BadRequest

[<RequireQualifiedAccess>]
type GetPetById =
    ///successful operation
    | OK of payload: Pet
    ///Invalid ID supplied
    | BadRequest
    ///Pet not found
    | NotFound

[<RequireQualifiedAccess>]
type UpdatePetWithForm =
    ///Invalid input
    | MethodNotAllowed
    | DefaultResponse

[<RequireQualifiedAccess>]
type DeletePet =
    ///Invalid pet value
    | BadRequest
    | DefaultResponse

[<RequireQualifiedAccess>]
type UploadFile =
    ///successful operation
    | OK of payload: ApiResponse

[<RequireQualifiedAccess>]
type GetInventory =
    ///successful operation
    | OK of payload: Map<string, int>

[<RequireQualifiedAccess>]
type PlaceOrder =
    ///successful operation
    | OK of payload: Order
    ///Invalid input
    | MethodNotAllowed

[<RequireQualifiedAccess>]
type GetOrderById =
    ///successful operation
    | OK of payload: Order
    ///Invalid ID supplied
    | BadRequest
    ///Order not found
    | NotFound

[<RequireQualifiedAccess>]
type DeleteOrder =
    ///Invalid ID supplied
    | BadRequest
    ///Order not found
    | NotFound
    | DefaultResponse

[<RequireQualifiedAccess>]
type CreateUser =
    ///successful operation
    | DefaultResponse of payload: User

type CreateUsersWithListInputPayloadArrayItem =
    { id: Option<int64>
      username: Option<string>
      firstName: Option<string>
      lastName: Option<string>
      email: Option<string>
      password: Option<string>
      phone: Option<string>
      ///User Status
      userStatus: Option<int> }
    ///Creates an instance of CreateUsersWithListInputPayloadArrayItem with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreateUsersWithListInputPayloadArrayItem =
        { id = None
          username = None
          firstName = None
          lastName = None
          email = None
          password = None
          phone = None
          userStatus = None }

type CreateUsersWithListInputPayload = list<CreateUsersWithListInputPayloadArrayItem>

[<RequireQualifiedAccess>]
type CreateUsersWithListInput =
    ///Successful operation
    | OK of payload: User
    ///successful operation
    | DefaultResponse

[<RequireQualifiedAccess>]
type LoginUser =
    ///successful operation
    | OK of payload: string
    ///Invalid username/password supplied
    | BadRequest

[<RequireQualifiedAccess>]
type LogoutUser =
    ///successful operation
    | DefaultResponse

[<RequireQualifiedAccess>]
type GetUserByName =
    ///successful operation
    | OK of payload: User
    ///Invalid username supplied
    | BadRequest
    ///User not found
    | NotFound

[<RequireQualifiedAccess>]
type UpdateUser =
    ///successful operation
    | DefaultResponse

[<RequireQualifiedAccess>]
type DeleteUser =
    ///Invalid username supplied
    | BadRequest
    ///User not found
    | NotFound
    | DefaultResponse
