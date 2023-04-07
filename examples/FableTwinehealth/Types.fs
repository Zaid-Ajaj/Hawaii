namespace rec FableTwinehealth.Types

type Maximum =
    { unit: Option<string>
      value: Option<float> }
    ///Creates an instance of Maximum with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Maximum = { unit = None; value = None }

type Minimum =
    { unit: Option<string>
      value: Option<float> }
    ///Creates an instance of Minimum with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Minimum = { unit = None; value = None }

type Validations =
    { maximum: Option<Maximum>
      minimum: Option<Minimum> }
    ///Creates an instance of Validations with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Validations = { maximum = None; minimum = None }

type ActionMetric =
    { goal: Option<obj>
      metric_type: Option<string>
      unit: Option<string>
      validations: Option<Validations> }
    ///Creates an instance of ActionMetric with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ActionMetric =
        { goal = None
          metric_type = None
          unit = None
          validations = None }

type Streak =
    { count: Option<int>
      updated_at: Option<string> }
    ///Creates an instance of Streak with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Streak = { count = None; updated_at = None }

type Adherence =
    { complete: Option<int>
      due: Option<int>
      streak: Option<Streak> }
    ///Creates an instance of Adherence with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Adherence =
        { complete = None
          due = None
          streak = None }

[<RequireQualifiedAccess>]
type Days =
    | Days0 = 0
    | Days1 = 1
    | Days2 = 2
    | Days3 = 3
    | Days4 = 4
    | Days5 = 5
    | Days6 = 6

type Weeks =
    { days: Option<list<Days>> }
    ///Creates an instance of Weeks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Weeks = { days = None }

type Frequencygoal =
    { weeks: Option<Weeks> }
    ///Creates an instance of Frequencygoal with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Frequencygoal = { weeks = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Type =
    | [<CompiledName "other_lifestyle">] Other_lifestyle
    member this.Format() =
        match this with
        | Other_lifestyle -> "other_lifestyle"

type Attributes =
    { _thread: Option<string>
      adherence: Option<Adherence>
      details: Option<obj>
      effective_from: string
      effective_to: Option<string>
      frequency_goal: Option<Frequencygoal>
      identifiers: Option<list<Identifier>>
      intake: Option<obj>
      metric_required: Option<bool>
      metrics: Option<list<ActionMetric>>
      static_title: Option<string>
      title: string
      tracking: Option<bool>
      ``type``: Type
      windows: Option<list<ActionWindow>> }
    ///Creates an instance of Attributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (effective_from: string, title: string, ``type``: Type): Attributes =
        { _thread = None
          adherence = None
          details = None
          effective_from = effective_from
          effective_to = None
          frequency_goal = None
          identifiers = None
          intake = None
          metric_required = None
          metrics = None
          static_title = None
          title = title
          tracking = None
          ``type`` = ``type``
          windows = None }

type Data =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of Data with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Data = { id = None; ``type`` = None }

type Plan =
    { data: Option<Data>
      links: Option<obj> }
    ///Creates an instance of Plan with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Plan = { data = None; links = None }

type Relationships =
    { plan: Option<Plan> }
    ///Creates an instance of Relationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Relationships = { plan = None }

type ActionResource =
    { attributes: Option<Attributes>
      id: string
      relationships: Option<Relationships>
      ``type``: string }
    ///Creates an instance of ActionResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): ActionResource =
        { attributes = None
          id = id
          relationships = None
          ``type`` = ``type`` }

type ActionWindow =
    { _id: Option<string>
      title: Option<string>
      ``type``: string }
    ///Creates an instance of ActionWindow with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): ActionWindow =
        { _id = None
          title = None
          ``type`` = ``type`` }

type Address =
    { city: Option<string>
      country: Option<string>
      district: Option<string>
      lines: Option<list<string>>
      postal_code: Option<string>
      state: Option<string>
      text: Option<string>
      ``type``: Option<string>
      ``use``: Option<string> }
    ///Creates an instance of Address with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Address =
        { city = None
          country = None
          district = None
          lines = None
          postal_code = None
          state = None
          text = None
          ``type`` = None
          ``use`` = None }

type ArchiveHistory =
    { archived: Option<bool>
      modified_at: Option<string>
      notes: Option<string>
      reason: Option<string> }
    ///Creates an instance of ArchiveHistory with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ArchiveHistory =
        { archived = None
          modified_at = None
          notes = None
          reason = None }

type BundleResourceAttributes =
    { _thread: Option<string>
      effective_from: string
      effective_to: Option<string>
      title: string
      ``type``: string }
    ///Creates an instance of BundleResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (effective_from: string, title: string, ``type``: string): BundleResourceAttributes =
        { _thread = None
          effective_from = effective_from
          effective_to = None
          title = title
          ``type`` = ``type`` }

type ActionsData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of ActionsData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ActionsData = { id = None; ``type`` = None }

type Actions =
    { data: Option<ActionsData>
      links: Option<obj> }
    ///Creates an instance of Actions with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Actions = { data = None; links = None }

type BundleResourceRelationshipsPlanData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of BundleResourceRelationshipsPlanData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BundleResourceRelationshipsPlanData = { id = None; ``type`` = None }

type BundleResourceRelationshipsPlan =
    { data: Option<BundleResourceRelationshipsPlanData>
      links: Option<obj> }
    ///Creates an instance of BundleResourceRelationshipsPlan with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BundleResourceRelationshipsPlan = { data = None; links = None }

type BundleResourceRelationships =
    { actions: Option<Actions>
      plan: Option<BundleResourceRelationshipsPlan> }
    ///Creates an instance of BundleResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BundleResourceRelationships = { actions = None; plan = None }

type BundleResource =
    { attributes: Option<BundleResourceAttributes>
      id: string
      relationships: Option<BundleResourceRelationships>
      ``type``: string }
    ///Creates an instance of BundleResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): BundleResource =
        { attributes = None
          id = id
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Responsestatus =
    | [<CompiledName "needsAction">] NeedsAction
    | [<CompiledName "declined">] Declined
    | [<CompiledName "tentative">] Tentative
    | [<CompiledName "accepted">] Accepted
    member this.Format() =
        match this with
        | NeedsAction -> "needsAction"
        | Declined -> "declined"
        | Tentative -> "tentative"
        | Accepted -> "accepted"

type Attendees =
    { ///Status of responses from attendees
      response_status: Option<Responsestatus>
      user: Option<string> }
    ///Creates an instance of Attendees with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Attendees = { response_status = None; user = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CalendarEventResourceAttributesType =
    | [<CompiledName "plan-check-in">] PlanCheckIn
    | [<CompiledName "reminder">] Reminder
    | [<CompiledName "telephone-call">] TelephoneCall
    | [<CompiledName "office-visit">] OfficeVisit
    | [<CompiledName "video-call">] VideoCall
    member this.Format() =
        match this with
        | PlanCheckIn -> "plan-check-in"
        | Reminder -> "reminder"
        | TelephoneCall -> "telephone-call"
        | OfficeVisit -> "office-visit"
        | VideoCall -> "video-call"

type CalendarEventResourceAttributes =
    { ///True if the calendar event is an all day event, false otherwise. Must be set to true for `plan-check-in` event type. If it is true, then `start_at` and `end_at` must also be set to beginning of day, except `plan-check-in` event type does not need an `end_at` date. If it is false, then `start_at` and `end_at` must be on the same day.
      all_day: Option<bool>
      ///List of attendees for the calendar event
      attendees: Option<list<Attendees>>
      ///The date and time when the calendar event is marked as completed. Only valid for `plan-check-in` event type.
      completed_at: Option<string>
      ///The coach who marked the calendar event as completed. Only valid for `plan-check-in` event type.
      completed_by: Option<obj>
      description: Option<string>
      ///The date and time when the calendar event ends. Not valid for `plan-check-in` event type.
      end_at: Option<string>
      location: Option<string>
      ///The date and time when the calendar event starts
      start_at: Option<string>
      ///The time zone in which the dates for the calendar event are specified
      time_zone: Option<string>
      ///The title of the calendar event. Must not be empty or null
      title: Option<string>
      ///The type of calendar event. Immutable after event creation.
      ``type``: Option<CalendarEventResourceAttributesType> }
    ///Creates an instance of CalendarEventResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResourceAttributes =
        { all_day = None
          attendees = None
          completed_at = None
          completed_by = None
          description = None
          end_at = None
          location = None
          start_at = None
          time_zone = None
          title = None
          ``type`` = None }

type Links =
    { self: Option<string> }
    ///Creates an instance of Links with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Links = { self = None }

type OwnerData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of OwnerData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): OwnerData = { id = None; ``type`` = None }

type OwnerLinks =
    { related: Option<string> }
    ///Creates an instance of OwnerLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): OwnerLinks = { related = None }

///The owner is the patient for whom the calendar event is created specificially for
type Owner =
    { data: OwnerData
      links: Option<OwnerLinks> }
    ///Creates an instance of Owner with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: OwnerData): Owner = { data = data; links = None }

type CalendarEventResourceRelationships =
    { ///The owner is the patient for whom the calendar event is created specificially for
      owner: Option<Owner> }
    ///Creates an instance of CalendarEventResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResourceRelationships = { owner = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CalendarEventResourceType =
    | [<CompiledName "calendar_event">] Calendar_event
    member this.Format() =
        match this with
        | Calendar_event -> "calendar_event"

type CalendarEventResource =
    { attributes: Option<CalendarEventResourceAttributes>
      id: Option<string>
      links: Option<Links>
      relationships: Option<CalendarEventResourceRelationships>
      ``type``: Option<CalendarEventResourceType> }
    ///Creates an instance of CalendarEventResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResource =
        { attributes = None
          id = None
          links = None
          relationships = None
          ``type`` = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CalendarEventResponseResourceAttributesResponsestatus =
    | [<CompiledName "accepted">] Accepted
    | [<CompiledName "declined">] Declined
    | [<CompiledName "tentative">] Tentative
    member this.Format() =
        match this with
        | Accepted -> "accepted"
        | Declined -> "declined"
        | Tentative -> "tentative"

type CalendarEventResponseResourceAttributes =
    { ///The attendee in the attendees list of the calendar event.
      attendee: Option<obj>
      ///The response status for the attendee.
      response_status: Option<CalendarEventResponseResourceAttributesResponsestatus> }
    ///Creates an instance of CalendarEventResponseResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResponseResourceAttributes =
        { attendee = None
          response_status = None }

type CalendarEventResponseResourceLinks =
    { self: Option<string> }
    ///Creates an instance of CalendarEventResponseResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResponseResourceLinks = { self = None }

type CalendareventData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of CalendareventData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendareventData = { id = None; ``type`` = None }

type CalendareventLinks =
    { related: Option<string> }
    ///Creates an instance of CalendareventLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendareventLinks = { related = None }

///The calendar_event is the calendar event for which the calendar event response is created specificially for
type Calendarevent =
    { data: CalendareventData
      links: Option<CalendareventLinks> }
    ///Creates an instance of Calendarevent with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: CalendareventData): Calendarevent = { data = data; links = None }

type UserData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of UserData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UserData = { id = None; ``type`` = None }

type UserLinks =
    { related: Option<string> }
    ///Creates an instance of UserLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UserLinks = { related = None }

///The user is the coach or patient for whom the calendar event response is created specificially for
type User =
    { data: UserData
      links: Option<UserLinks> }
    ///Creates an instance of User with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: UserData): User = { data = data; links = None }

type CalendarEventResponseResourceRelationships =
    { ///The calendar_event is the calendar event for which the calendar event response is created specificially for
      calendar_event: Option<Calendarevent>
      ///The user is the coach or patient for whom the calendar event response is created specificially for
      user: Option<User> }
    ///Creates an instance of CalendarEventResponseResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResponseResourceRelationships = { calendar_event = None; user = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CalendarEventResponseResourceType =
    | [<CompiledName "calendar_event_response">] Calendar_event_response
    member this.Format() =
        match this with
        | Calendar_event_response -> "calendar_event_response"

type CalendarEventResponseResource =
    { attributes: Option<CalendarEventResponseResourceAttributes>
      id: Option<string>
      links: Option<CalendarEventResponseResourceLinks>
      relationships: Option<CalendarEventResponseResourceRelationships>
      ``type``: Option<CalendarEventResponseResourceType> }
    ///Creates an instance of CalendarEventResponseResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CalendarEventResponseResource =
        { attributes = None
          id = None
          links = None
          relationships = None
          ``type`` = None }

type CoachResourceAttributes =
    { first_name: Option<string>
      last_name: Option<string> }
    ///Creates an instance of CoachResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CoachResourceAttributes = { first_name = None; last_name = None }

type CoachResourceLinks =
    { self: string }
    ///Creates an instance of CoachResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): CoachResourceLinks = { self = self }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CoachResourceType =
    | [<CompiledName "coach">] Coach
    member this.Format() =
        match this with
        | Coach -> "coach"

type CoachResource =
    { attributes: CoachResourceAttributes
      id: string
      links: Option<CoachResourceLinks>
      ``type``: CoachResourceType }
    ///Creates an instance of CoachResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: CoachResourceAttributes, id: string, ``type``: CoachResourceType): CoachResource =
        { attributes = attributes
          id = id
          links = None
          ``type`` = ``type`` }

type CollectionResponseLinks =
    { last: Option<string>
      next: Option<string>
      prev: Option<string>
      self: Option<string> }
    ///Creates an instance of CollectionResponseLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CollectionResponseLinks =
        { last = None
          next = None
          prev = None
          self = None }

type CreateActionRequest =
    { data: ActionResource }
    ///Creates an instance of CreateActionRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: ActionResource): CreateActionRequest = { data = data }

type CreateActionResponse =
    { data: ActionResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateActionResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: ActionResource): CreateActionResponse = { data = data; meta = None }

type CreateBundleRequest =
    { data: BundleResource }
    ///Creates an instance of CreateBundleRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: BundleResource): CreateBundleRequest = { data = data }

type CreateBundleResponse =
    { data: BundleResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateBundleResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: BundleResource): CreateBundleResponse = { data = data; meta = None }

type CreateCalendarEventRequestDataRelationshipsOwner = Map<string, obj>

type CreateCalendarEventRequestDataRelationships =
    { owner: Option<CreateCalendarEventRequestDataRelationshipsOwner> }
    ///Creates an instance of CreateCalendarEventRequestDataRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreateCalendarEventRequestDataRelationships = { owner = None }

type CreateCalendarEventRequestData =
    { attributes: obj
      relationships: CreateCalendarEventRequestDataRelationships }
    ///Creates an instance of CreateCalendarEventRequestData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: obj, relationships: CreateCalendarEventRequestDataRelationships): CreateCalendarEventRequestData =
        { attributes = attributes
          relationships = relationships }

type CreateCalendarEventRequest =
    { data: CreateCalendarEventRequestData }
    ///Creates an instance of CreateCalendarEventRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: CreateCalendarEventRequestData): CreateCalendarEventRequest = { data = data }

type CreateCalendarEventResponse =
    { data: Option<CalendarEventResource>
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateCalendarEventResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreateCalendarEventResponse = { data = None; meta = None }

type CreateCalendarEventResponseRequestDataRelationshipsCalendarevent = Map<string, obj>
type CreateCalendarEventResponseRequestDataRelationshipsUser = Map<string, obj>

type CreateCalendarEventResponseRequestDataRelationships =
    { calendar_event: Option<CreateCalendarEventResponseRequestDataRelationshipsCalendarevent>
      user: Option<CreateCalendarEventResponseRequestDataRelationshipsUser> }
    ///Creates an instance of CreateCalendarEventResponseRequestDataRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreateCalendarEventResponseRequestDataRelationships =
        { calendar_event = None; user = None }

type CreateCalendarEventResponseRequestData =
    { attributes: obj
      relationships: CreateCalendarEventResponseRequestDataRelationships }
    ///Creates an instance of CreateCalendarEventResponseRequestData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: obj, relationships: CreateCalendarEventResponseRequestDataRelationships): CreateCalendarEventResponseRequestData =
        { attributes = attributes
          relationships = relationships }

type CreateCalendarEventResponseRequest =
    { data: CreateCalendarEventResponseRequestData }
    ///Creates an instance of CreateCalendarEventResponseRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: CreateCalendarEventResponseRequestData): CreateCalendarEventResponseRequest =
        { data = data }

type CreateGroupRequest =
    { data: GroupResource }
    ///Creates an instance of CreateGroupRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: GroupResource): CreateGroupRequest = { data = data }

type CreateGroupResponse =
    { data: GroupResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateGroupResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: GroupResource): CreateGroupResponse = { data = data; meta = None }

type CreateOrUpdateErrorResponse =
    { errors: Option<Error>
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateOrUpdateErrorResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreateOrUpdateErrorResponse = { errors = None; meta = None }

type CreateOrUpdateMetaResponse =
    { ignored: Option<list<string>>
      req_id: Option<string> }
    ///Creates an instance of CreateOrUpdateMetaResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreateOrUpdateMetaResponse = { ignored = None; req_id = None }

type Meta =
    { ///If `true`, the patient health metric will be ignored if there is an existing patient health metric for
      ///the same patient, with the same `type` and same `occurred_at`.
      ignore_duplicates: Option<bool> }
    ///Creates an instance of Meta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Meta = { ignore_duplicates = None }

type CreatePatientHealthMetricRequest =
    { data: PatientHealthMetricCreateResource
      meta: Option<Meta> }
    ///Creates an instance of CreatePatientHealthMetricRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientHealthMetricCreateResource): CreatePatientHealthMetricRequest =
        { data = data; meta = None }

type CreatePatientHealthMetricResponse =
    { data: PatientHealthMetricResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreatePatientHealthMetricResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientHealthMetricResource): CreatePatientHealthMetricResponse =
        { data = data; meta = None }

type CreatePatientRequestMeta =
    { ///If `true`, patients with any conflicting identifiers (same `system` and `value`) will be ignored.
      ///Useful for gracefully skipping duplicates after errors occur during bulk create.
      ignore_duplicates: Option<bool> }
    ///Creates an instance of CreatePatientRequestMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CreatePatientRequestMeta = { ignore_duplicates = None }

type CreatePatientRequest =
    { data: PatientCreateResource
      meta: Option<CreatePatientRequestMeta> }
    ///Creates an instance of CreatePatientRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientCreateResource): CreatePatientRequest = { data = data; meta = None }

type CreatePatientResponse =
    { data: PatientResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreatePatientResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientResource): CreatePatientResponse = { data = data; meta = None }

type CreateRewardEarningFulfillmentRequest =
    { data: RewardEarningFulfillmentResource }
    ///Creates an instance of CreateRewardEarningFulfillmentRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardEarningFulfillmentResource): CreateRewardEarningFulfillmentRequest =
        { data = data }

type CreateRewardEarningFulfillmentResponse =
    { data: RewardEarningFulfillmentResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateRewardEarningFulfillmentResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardEarningFulfillmentResource): CreateRewardEarningFulfillmentResponse =
        { data = data; meta = None }

type CreateRewardEarningRequest =
    { data: RewardEarningResource }
    ///Creates an instance of CreateRewardEarningRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardEarningResource): CreateRewardEarningRequest = { data = data }

type CreateRewardEarningResponse =
    { data: RewardEarningResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateRewardEarningResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardEarningResource): CreateRewardEarningResponse = { data = data; meta = None }

type CreateRewardProgramActivationRequest =
    { data: RewardProgramActivationResource }
    ///Creates an instance of CreateRewardProgramActivationRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramActivationResource): CreateRewardProgramActivationRequest = { data = data }

type CreateRewardProgramActivationResponse =
    { data: RewardProgramActivationResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateRewardProgramActivationResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramActivationResource): CreateRewardProgramActivationResponse =
        { data = data; meta = None }

type CreateRewardProgramRequest =
    { data: RewardProgramResource }
    ///Creates an instance of CreateRewardProgramRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramResource): CreateRewardProgramRequest = { data = data }

type CreateRewardProgramResponse =
    { data: RewardProgramResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateRewardProgramResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramResource): CreateRewardProgramResponse = { data = data; meta = None }

type CreateRewardRequest =
    { data: RewardResource }
    ///Creates an instance of CreateRewardRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardResource): CreateRewardRequest = { data = data }

type CreateRewardResponse =
    { data: RewardResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateRewardResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardResource): CreateRewardResponse = { data = data; meta = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Granttype =
    | [<CompiledName "refresh_token">] Refresh_token
    | [<CompiledName "client_credentials">] Client_credentials
    member this.Format() =
        match this with
        | Refresh_token -> "refresh_token"
        | Client_credentials -> "client_credentials"

type CreateTokenRequestDataAttributes =
    { ///Contact Fitbit Plus API Support to get a client id and secret.
      client_id: string
      ///Contact Fitbit Plus API Support to get a client id and secret. Secret is required if grant_type is "client_credentials"
      client_secret: Option<string>
      grant_type: Granttype
      ///Required if grant_type is "refresh_token"
      refresh_token: Option<string> }
    ///Creates an instance of CreateTokenRequestDataAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (client_id: string, grant_type: Granttype): CreateTokenRequestDataAttributes =
        { client_id = client_id
          client_secret = None
          grant_type = grant_type
          refresh_token = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CreateTokenRequestDataType =
    | [<CompiledName "token">] Token
    member this.Format() =
        match this with
        | Token -> "token"

type CreateTokenRequestData =
    { attributes: CreateTokenRequestDataAttributes
      ``type``: Option<CreateTokenRequestDataType> }
    ///Creates an instance of CreateTokenRequestData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: CreateTokenRequestDataAttributes): CreateTokenRequestData =
        { attributes = attributes
          ``type`` = None }

type CreateTokenRequest =
    { data: CreateTokenRequestData }
    ///Creates an instance of CreateTokenRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: CreateTokenRequestData): CreateTokenRequest = { data = data }

type CreateTokenResponse =
    { data: TokenResource
      included: Option<list<GroupResource>>
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of CreateTokenResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: TokenResource): CreateTokenResponse =
        { data = data
          included = None
          meta = None }

///Status of email. Multiple statuses may be defined. The current status is the one with the most recent date.
type Statustimes =
    { ///Time email was bounced.
      bounce: Option<string>
      ///Time email was clicked.
      click: Option<string>
      ///Time email was deferred.
      deferred: Option<string>
      ///Time email was delivered.
      delivered: Option<string>
      ///Time email was dropped.
      dropped: Option<string>
      ///Time email was opened.
      ``open``: Option<string>
      ///Time email was processed.
      processed: Option<string>
      ///Time email was reported as spam.
      spamreport: Option<string>
      ///Time email was unsubscribed from.
      unsubscribe: Option<string> }
    ///Creates an instance of Statustimes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Statustimes =
        { bounce = None
          click = None
          deferred = None
          delivered = None
          dropped = None
          ``open`` = None
          processed = None
          spamreport = None
          unsubscribe = None }

type EmailHistoryResourceAttributes =
    { ///Address email was sent to.
      email_address: Option<string>
      ///Type of email.
      email_type: Option<string>
      ///Time email was sent.
      send_time: Option<string>
      ///Status of email. Multiple statuses may be defined. The current status is the one with the most recent date.
      status_times: Option<Statustimes>
      ///Unique identifier for this email.
      twine_email_id: Option<string> }
    ///Creates an instance of EmailHistoryResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): EmailHistoryResourceAttributes =
        { email_address = None
          email_type = None
          send_time = None
          status_times = None
          twine_email_id = None }

type ReceiverData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of ReceiverData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ReceiverData = { id = None; ``type`` = None }

type Receiver =
    { data: Option<ReceiverData> }
    ///Creates an instance of Receiver with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Receiver = { data = None }

type SenderData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of SenderData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): SenderData = { id = None; ``type`` = None }

type Sender =
    { data: Option<SenderData> }
    ///Creates an instance of Sender with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Sender = { data = None }

type EmailHistoryResourceRelationships =
    { receiver: Option<Receiver>
      sender: Option<Sender> }
    ///Creates an instance of EmailHistoryResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): EmailHistoryResourceRelationships = { receiver = None; sender = None }

type EmailHistoryResource =
    { attributes: Option<EmailHistoryResourceAttributes>
      id: Option<string>
      relationships: Option<EmailHistoryResourceRelationships>
      ``type``: string }
    ///Creates an instance of EmailHistoryResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): EmailHistoryResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Code =
    | [<CompiledName "Forbidden">] Forbidden
    | [<CompiledName "InvalidParameter">] InvalidParameter
    | [<CompiledName "InvalidBodyParameter">] InvalidBodyParameter
    | [<CompiledName "ResourceNotFound">] ResourceNotFound
    | [<CompiledName "Unauthorized">] Unauthorized
    | [<CompiledName "InvalidCredentials">] InvalidCredentials
    | [<CompiledName "InvalidGrantType">] InvalidGrantType
    member this.Format() =
        match this with
        | Forbidden -> "Forbidden"
        | InvalidParameter -> "InvalidParameter"
        | InvalidBodyParameter -> "InvalidBodyParameter"
        | ResourceNotFound -> "ResourceNotFound"
        | Unauthorized -> "Unauthorized"
        | InvalidCredentials -> "InvalidCredentials"
        | InvalidGrantType -> "InvalidGrantType"

type Source =
    { parameter: Option<string>
      pointer: Option<string> }
    ///Creates an instance of Source with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Source = { parameter = None; pointer = None }

type Error =
    { code: Option<Code>
      detail: Option<string>
      source: Option<Source>
      status: Option<string>
      title: Option<string> }
    ///Creates an instance of Error with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Error =
        { code = None
          detail = None
          source = None
          status = None
          title = None }

type FetchActionResponse =
    { data: ActionResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchActionResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: ActionResource): FetchActionResponse = { data = data; meta = None }

type FetchBundleResponse =
    { data: BundleResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchBundleResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: BundleResource): FetchBundleResponse = { data = data; meta = None }

type FetchCalendarEventResponse =
    { data: Option<CalendarEventResource>
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [patient](#operation/fetchPatient)
      included: Option<list<Resource>>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchCalendarEventResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FetchCalendarEventResponse =
        { data = None
          included = None
          meta = None }

type FetchCalendarEventsResponse =
    { data: Option<list<CalendarEventResource>>
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [patient](#operation/fetchPatient)
      included: Option<list<Resource>>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchCalendarEventsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FetchCalendarEventsResponse =
        { data = None
          included = None
          links = None
          meta = None }

type FetchCoachResponse =
    { data: CoachResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchCoachResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: CoachResource): FetchCoachResponse = { data = data; meta = None }

type FetchCoachesResponse =
    { data: list<CoachResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchCoachesResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<CoachResource>): FetchCoachesResponse = { data = data; meta = None }

type FetchEmailHistoriesResponse =
    { data: list<EmailHistoryResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchEmailHistoriesResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<EmailHistoryResource>): FetchEmailHistoriesResponse = { data = data; meta = None }

type FetchEmailHistoryResponse =
    { data: EmailHistoryResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchEmailHistoryResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: EmailHistoryResource): FetchEmailHistoryResponse = { data = data; meta = None }

type FetchErrorResponse =
    { errors: Option<Error>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchErrorResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FetchErrorResponse = { errors = None; meta = None }

type FetchGroupResponse =
    { data: GroupResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchGroupResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: GroupResource): FetchGroupResponse = { data = data; meta = None }

type FetchGroupsResponse =
    { data: list<GroupResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchGroupsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<GroupResource>): FetchGroupsResponse = { data = data; meta = None }

type FetchHealthProfileAnswerResponse =
    { data: HealthProfileAnswerResource
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [patient](#operation/fetchPatient)
      included: Option<list<Resource>>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthProfileAnswerResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: HealthProfileAnswerResource): FetchHealthProfileAnswerResponse =
        { data = data
          included = None
          meta = None }

type FetchHealthProfileAnswersResponse =
    { data: list<HealthProfileAnswerResource>
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [patient](#operation/fetchPatient)
      included: Option<list<Resource>>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthProfileAnswersResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<HealthProfileAnswerResource>): FetchHealthProfileAnswersResponse =
        { data = data
          included = None
          links = None
          meta = None }

type FetchHealthProfileQuestionResponse =
    { data: HealthProfileQuestionResource
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [question_definition](#operation/fetchHealthQuestionDefinition), [answer](#operation/fetchHealthProfileAnswer)
      included: Option<list<Resource>>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthProfileQuestionResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: HealthProfileQuestionResource): FetchHealthProfileQuestionResponse =
        { data = data
          included = None
          meta = None }

type FetchHealthProfileQuestionsResponse =
    { data: list<HealthProfileQuestionResource>
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [question_definition](#operation/fetchHealthQuestionDefinition), [answer](#operation/fetchHealthProfileAnswer)
      included: Option<list<Resource>>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthProfileQuestionsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<HealthProfileQuestionResource>): FetchHealthProfileQuestionsResponse =
        { data = data
          included = None
          links = None
          meta = None }

type FetchHealthProfileResponse =
    { data: HealthProfileResource
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [patient](#operation/fetchPatient), [question](#operation/fetchHealthProfileQuestion)
      included: Option<list<Resource>>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthProfileResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: HealthProfileResource): FetchHealthProfileResponse =
        { data = data
          included = None
          meta = None }

type FetchHealthProfilesResponse =
    { data: list<HealthProfileResource>
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [patient](#operation/fetchPatient), [question](#operation/fetchHealthProfileQuestion)
      included: Option<list<Resource>>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthProfilesResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<HealthProfileResource>): FetchHealthProfilesResponse =
        { data = data
          included = None
          links = None
          meta = None }

type FetchHealthQuestionDefinitionResponse =
    { data: HealthQuestionDefinitionResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthQuestionDefinitionResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: HealthQuestionDefinitionResource): FetchHealthQuestionDefinitionResponse =
        { data = data; meta = None }

type FetchHealthQuestionDefinitionsResponse =
    { data: list<HealthQuestionDefinitionResource>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchHealthQuestionDefinitionsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<HealthQuestionDefinitionResource>): FetchHealthQuestionDefinitionsResponse =
        { data = data
          links = None
          meta = None }

type FetchMetaResponse =
    { count: Option<int>
      req_id: Option<string> }
    ///Creates an instance of FetchMetaResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FetchMetaResponse = { count = None; req_id = None }

type FetchOrganizationResponse =
    { data: OrganizationResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchOrganizationResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: OrganizationResource): FetchOrganizationResponse = { data = data; meta = None }

type FetchPatientHealthMetricResponse =
    { data: list<PatientHealthMetricResource>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchPatientHealthMetricResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<PatientHealthMetricResource>): FetchPatientHealthMetricResponse =
        { data = data
          links = None
          meta = None }

type FetchPatientHealthResultResponse =
    { data: list<PatientHealthResultResource>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchPatientHealthResultResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<PatientHealthResultResource>): FetchPatientHealthResultResponse =
        { data = data
          links = None
          meta = None }

type FetchPatientPlanSummariesResponse =
    { data: list<PatientPlanSummaryResource>
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [action](#operation/fetchAction), [bundle](#operation/fetchBundle) and [patient](#operation/fetchPatient)
      included: Option<list<Resource>>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchPatientPlanSummariesResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<PatientPlanSummaryResource>): FetchPatientPlanSummariesResponse =
        { data = data
          included = None
          meta = None }

type FetchPatientPlanSummaryResponse =
    { data: PatientPlanSummaryResource
      ///Related resources which are included in the response based on the `include` param.
      ///Attributes of each resource will vary depending on the type.
      ///See [action](#operation/fetchAction), [bundle](#operation/fetchBundle) and [patient](#operation/fetchPatient)
      included: Option<list<Resource>>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchPatientPlanSummaryResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientPlanSummaryResource): FetchPatientPlanSummaryResponse =
        { data = data
          included = None
          meta = None }

type FetchPatientResponse =
    { data: PatientResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchPatientResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientResource): FetchPatientResponse = { data = data; meta = None }

type FetchPatientsResponse =
    { data: list<PatientResource>
      links: Option<CollectionResponseLinks>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchPatientsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<PatientResource>): FetchPatientsResponse =
        { data = data
          links = None
          meta = None }

type FetchRewardEarningFulfillmentResponse =
    { data: RewardEarningFulfillmentResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardEarningFulfillmentResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardEarningFulfillmentResource): FetchRewardEarningFulfillmentResponse =
        { data = data; meta = None }

type FetchRewardEarningFulfillmentsResponse =
    { data: list<RewardEarningFulfillmentResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardEarningFulfillmentsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<RewardEarningFulfillmentResource>): FetchRewardEarningFulfillmentsResponse =
        { data = data; meta = None }

type FetchRewardEarningResponse =
    { data: RewardEarningResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardEarningResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardEarningResource): FetchRewardEarningResponse = { data = data; meta = None }

type FetchRewardEarningsResponse =
    { data: list<RewardEarningResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardEarningsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<RewardEarningResource>): FetchRewardEarningsResponse = { data = data; meta = None }

type FetchRewardProgramActivationResponse =
    { data: RewardProgramActivationResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardProgramActivationResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramActivationResource): FetchRewardProgramActivationResponse =
        { data = data; meta = None }

type FetchRewardProgramActivationsResponse =
    { data: list<RewardProgramActivationResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardProgramActivationsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<RewardProgramActivationResource>): FetchRewardProgramActivationsResponse =
        { data = data; meta = None }

type FetchRewardProgramResponse =
    { data: RewardProgramResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardProgramResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramResource): FetchRewardProgramResponse = { data = data; meta = None }

type FetchRewardProgramsResponse =
    { data: list<RewardProgramResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardProgramsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<RewardProgramResource>): FetchRewardProgramsResponse = { data = data; meta = None }

type FetchRewardResponse =
    { data: RewardResource
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardResource): FetchRewardResponse = { data = data; meta = None }

type FetchRewardsResponse =
    { data: list<RewardResource>
      meta: Option<FetchMetaResponse> }
    ///Creates an instance of FetchRewardsResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<RewardResource>): FetchRewardsResponse = { data = data; meta = None }

type GroupResourceAttributes =
    { ///A description of the group
      bio: Option<string>
      ///The name of the group
      name: string }
    ///Creates an instance of GroupResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (name: string): GroupResourceAttributes = { bio = None; name = name }

type GroupResourceLinks =
    { self: string }
    ///Creates an instance of GroupResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): GroupResourceLinks = { self = self }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type GroupResourceType =
    | [<CompiledName "group">] Group
    member this.Format() =
        match this with
        | Group -> "group"

type GroupResource =
    { attributes: GroupResourceAttributes
      id: string
      links: Option<GroupResourceLinks>
      ``type``: GroupResourceType }
    ///Creates an instance of GroupResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: GroupResourceAttributes, id: string, ``type``: GroupResourceType): GroupResource =
        { attributes = attributes
          id = id
          links = None
          ``type`` = ``type`` }

///The details of a previous answer for a health profile question
type History =
    { ///The id of the patient or coach who answered the health profile question
      _created_by: Option<string>
      ///The date when the health profile question is answered
      answered_at: Option<string>
      ///The value of the answer entered for the health profile question
      value: Option<string> }
    ///Creates an instance of History with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): History =
        { _created_by = None
          answered_at = None
          value = None }

///The details of the latest answer for a health profile question
type Latest =
    { ///The id of the patient or coach who answered the health profile question
      _created_by: Option<string>
      ///The date when the health profile question is answered
      answered_at: Option<string>
      ///The value of the answer entered for the health profile question
      value: Option<string> }
    ///Creates an instance of Latest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Latest =
        { _created_by = None
          answered_at = None
          value = None }

type HealthProfileAnswerResourceAttributes =
    { ///List of details of previous answers for a health profile question
      history: Option<list<History>>
      ///The details of the latest answer for a health profile question
      latest: Option<Latest>
      question_id: Option<string> }
    ///Creates an instance of HealthProfileAnswerResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileAnswerResourceAttributes =
        { history = None
          latest = None
          question_id = None }

type HealthProfileAnswerResourceLinks =
    { self: string }
    ///Creates an instance of HealthProfileAnswerResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): HealthProfileAnswerResourceLinks = { self = self }

type PatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of PatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientData = { id = None; ``type`` = None }

type PatientLinks =
    { related: Option<string> }
    ///Creates an instance of PatientLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientLinks = { related = None }

type Patient =
    { data: Option<PatientData>
      links: Option<PatientLinks> }
    ///Creates an instance of Patient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Patient = { data = None; links = None }

type HealthProfileAnswerResourceRelationships =
    { patient: Option<Patient> }
    ///Creates an instance of HealthProfileAnswerResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileAnswerResourceRelationships = { patient = None }

type HealthProfileAnswerResource =
    { attributes: Option<HealthProfileAnswerResourceAttributes>
      id: string
      links: Option<HealthProfileAnswerResourceLinks>
      relationships: Option<HealthProfileAnswerResourceRelationships>
      ``type``: string }
    ///Creates an instance of HealthProfileAnswerResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): HealthProfileAnswerResource =
        { attributes = None
          id = id
          links = None
          relationships = None
          ``type`` = ``type`` }

type HealthProfileQuestionResourceLinks =
    { self: string }
    ///Creates an instance of HealthProfileQuestionResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): HealthProfileQuestionResourceLinks = { self = self }

type AnswerData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of AnswerData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): AnswerData = { id = None; ``type`` = None }

type AnswerLinks =
    { related: Option<string> }
    ///Creates an instance of AnswerLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): AnswerLinks = { related = None }

type Answer =
    { data: Option<AnswerData>
      links: Option<AnswerLinks> }
    ///Creates an instance of Answer with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Answer = { data = None; links = None }

type ProfileLinks =
    { related: Option<string> }
    ///Creates an instance of ProfileLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProfileLinks = { related = None }

type Profile =
    { links: Option<ProfileLinks> }
    ///Creates an instance of Profile with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Profile = { links = None }

type QuestiondefinitionData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of QuestiondefinitionData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): QuestiondefinitionData = { id = None; ``type`` = None }

type Questiondefinition =
    { data: Option<QuestiondefinitionData>
      links: Option<obj> }
    ///Creates an instance of Questiondefinition with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Questiondefinition = { data = None; links = None }

type HealthProfileQuestionResourceRelationships =
    { answer: Option<Answer>
      profile: Option<Profile>
      question_definition: Option<Questiondefinition> }
    ///Creates an instance of HealthProfileQuestionResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileQuestionResourceRelationships =
        { answer = None
          profile = None
          question_definition = None }

type HealthProfileQuestionResource =
    { ///A health profile question does not have any attribute since it only relates an answer to the corresponding question definition.
      attributes: Option<obj>
      id: string
      links: Option<HealthProfileQuestionResourceLinks>
      relationships: Option<HealthProfileQuestionResourceRelationships>
      ``type``: string }
    ///Creates an instance of HealthProfileQuestionResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): HealthProfileQuestionResource =
        { attributes = None
          id = id
          links = None
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Category =
    | [<CompiledName "lifestyle_behaviors">] Lifestyle_behaviors
    | [<CompiledName "mental_wellbeing">] Mental_wellbeing
    | [<CompiledName "preventative_care">] Preventative_care
    | [<CompiledName "overall">] Overall
    member this.Format() =
        match this with
        | Lifestyle_behaviors -> "lifestyle_behaviors"
        | Mental_wellbeing -> "mental_wellbeing"
        | Preventative_care -> "preventative_care"
        | Overall -> "overall"

///The category, answered and total counts for questions in the health profile
type Stats =
    { answered: Option<float>
      category: Option<Category>
      total: Option<float> }
    ///Creates an instance of Stats with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Stats =
        { answered = None
          category = None
          total = None }

type HealthProfileResourceAttributes =
    { ///List of category, answered and total counts for questions in the health profile
      stats: Option<list<Stats>> }
    ///Creates an instance of HealthProfileResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileResourceAttributes = { stats = None }

type HealthProfileResourceLinks =
    { self: string }
    ///Creates an instance of HealthProfileResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): HealthProfileResourceLinks = { self = self }

type HealthProfileResourceRelationshipsPatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of HealthProfileResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileResourceRelationshipsPatientData = { id = None; ``type`` = None }

type HealthProfileResourceRelationshipsPatientLinks =
    { related: Option<string> }
    ///Creates an instance of HealthProfileResourceRelationshipsPatientLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileResourceRelationshipsPatientLinks = { related = None }

type HealthProfileResourceRelationshipsPatient =
    { data: Option<HealthProfileResourceRelationshipsPatientData>
      links: Option<HealthProfileResourceRelationshipsPatientLinks> }
    ///Creates an instance of HealthProfileResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileResourceRelationshipsPatient = { data = None; links = None }

type QuestionsData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of QuestionsData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): QuestionsData = { id = None; ``type`` = None }

type QuestionsLinks =
    { related: Option<string> }
    ///Creates an instance of QuestionsLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): QuestionsLinks = { related = None }

type Questions =
    { data: Option<list<QuestionsData>>
      links: Option<QuestionsLinks> }
    ///Creates an instance of Questions with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Questions = { data = None; links = None }

type HealthProfileResourceRelationships =
    { patient: Option<HealthProfileResourceRelationshipsPatient>
      questions: Option<Questions> }
    ///Creates an instance of HealthProfileResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthProfileResourceRelationships = { patient = None; questions = None }

type HealthProfileResource =
    { attributes: Option<HealthProfileResourceAttributes>
      id: Option<string>
      links: Option<HealthProfileResourceLinks>
      relationships: Option<HealthProfileResourceRelationships>
      ``type``: string }
    ///Creates an instance of HealthProfileResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): HealthProfileResource =
        { attributes = None
          id = None
          links = None
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type HealthQuestionDefinitionResourceAttributesCategory =
    | [<CompiledName "lifestyle_behaviors">] Lifestyle_behaviors
    | [<CompiledName "mental_wellbeing">] Mental_wellbeing
    | [<CompiledName "preventative_care">] Preventative_care
    member this.Format() =
        match this with
        | Lifestyle_behaviors -> "lifestyle_behaviors"
        | Mental_wellbeing -> "mental_wellbeing"
        | Preventative_care -> "preventative_care"

///The label, value and icon for the answer choices for the question
type FormatData =
    { icon: Option<string>
      label: Option<string>
      value: Option<string> }
    ///Creates an instance of FormatData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FormatData =
        { icon = None
          label = None
          value = None }

///The list of formats for the health profile definition
type Format =
    { data: Option<list<FormatData>>
      ``type``: Option<string> }
    ///Creates an instance of Format with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Format = { data = None; ``type`` = None }

///The age and gender requirement for the question to be included
type Requirements =
    { property: Option<string>
      ///Specifies if the value in property should be equal to the one in value
      shouldBeEqual: Option<bool>
      ///Specifies if the value in property should be greater than the one in value
      shouldBeGreaterThan: Option<float>
      ///Specifies if the value in property should be less than the one in value
      shouldBeLessThan: Option<float>
      ///The value to be compared with the one in property, based on shouldBeEqual, shouldBeGreaterThan, or shouldBeLessThan
      value: Option<string> }
    ///Creates an instance of Requirements with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Requirements =
        { property = None
          shouldBeEqual = None
          shouldBeGreaterThan = None
          shouldBeLessThan = None
          value = None }

type HealthQuestionDefinitionResourceAttributes =
    { ///The category for the health profile definition
      category: Option<HealthQuestionDefinitionResourceAttributesCategory>
      ///The list of formats for the health profile definition
      format: Option<Format>
      ///The lsit of age and gender requirements for the question to be included
      requirements: Option<list<Requirements>>
      ///The question text which corresponds to the answer choices
      text: Option<string> }
    ///Creates an instance of HealthQuestionDefinitionResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): HealthQuestionDefinitionResourceAttributes =
        { category = None
          format = None
          requirements = None
          text = None }

type HealthQuestionDefinitionResourceLinks =
    { self: string }
    ///Creates an instance of HealthQuestionDefinitionResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): HealthQuestionDefinitionResourceLinks = { self = self }

type HealthQuestionDefinitionResource =
    { attributes: Option<HealthQuestionDefinitionResourceAttributes>
      id: string
      links: Option<HealthQuestionDefinitionResourceLinks>
      relationships: Option<obj>
      ``type``: string }
    ///Creates an instance of HealthQuestionDefinitionResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): HealthQuestionDefinitionResource =
        { attributes = None
          id = id
          links = None
          relationships = None
          ``type`` = ``type`` }

type Identifier =
    { label: Option<string>
      system: string
      value: string }
    ///Creates an instance of Identifier with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): Identifier =
        { label = None
          system = system
          value = value }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type OrganizationResourceType =
    | [<CompiledName "organization">] Organization
    member this.Format() =
        match this with
        | Organization -> "organization"

type OrganizationResource =
    { attributes: obj
      id: string
      links: Option<obj>
      ``type``: OrganizationResourceType }
    ///Creates an instance of OrganizationResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: obj, id: string, ``type``: OrganizationResourceType): OrganizationResource =
        { attributes = attributes
          id = id
          links = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Gender =
    | [<CompiledName "male">] Male
    | [<CompiledName "female">] Female
    | [<CompiledName "other">] Other
    member this.Format() =
        match this with
        | Male -> "male"
        | Female -> "female"
        | Other -> "other"

///A patient's motivation statement.
type Statement =
    { updated_at: Option<string>
      updated_by: Option<string>
      value: Option<string> }
    ///Creates an instance of Statement with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Statement =
        { updated_at = None
          updated_by = None
          value = None }

type PatientCreateResourceAttributes =
    { addresses: Option<list<Address>>
      archive_history: Option<list<ArchiveHistory>>
      archived: Option<bool>
      birth_date: Option<string>
      email_address: Option<string>
      enrolled_at: Option<string>
      first_access_at: Option<string>
      first_name: Option<string>
      gender: Option<Gender>
      identifiers: Option<list<PatientIdentifier>>
      invited_at: Option<string>
      last_access_at: Option<string>
      last_name: Option<string>
      ///Coach's note about the patient. Not visible to the patient.
      note: Option<string>
      phone_numbers: Option<list<PhoneNumber>>
      ///A patient's motivation statement.
      statement: Option<Statement>
      updated_at: Option<string> }
    ///Creates an instance of PatientCreateResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientCreateResourceAttributes =
        { addresses = None
          archive_history = None
          archived = None
          birth_date = None
          email_address = None
          enrolled_at = None
          first_access_at = None
          first_name = None
          gender = None
          identifiers = None
          invited_at = None
          last_access_at = None
          last_name = None
          note = None
          phone_numbers = None
          statement = None
          updated_at = None }

type PatientCreateResourceLinks =
    { self: Option<string>
      ///A link to the patient record in the Fitbit Plus web application.
      twine_web_app: Option<string> }
    ///Creates an instance of PatientCreateResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientCreateResourceLinks = { self = None; twine_web_app = None }

type PatientCreateResourceRelationships =
    { coaches: Option<obj>
      groups: obj }
    ///Creates an instance of PatientCreateResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (groups: obj): PatientCreateResourceRelationships = { coaches = None; groups = groups }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PatientCreateResourceType =
    | [<CompiledName "patient">] Patient
    member this.Format() =
        match this with
        | Patient -> "patient"

type CoachesDataMeta =
    { primary: Option<bool> }
    ///Creates an instance of CoachesDataMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CoachesDataMeta = { primary = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CoachesDataType =
    | [<CompiledName "coach">] Coach
    member this.Format() =
        match this with
        | Coach -> "coach"

type CoachesData =
    { id: string
      meta: Option<CoachesDataMeta>
      ``type``: CoachesDataType }
    ///Creates an instance of CoachesData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: CoachesDataType): CoachesData =
        { id = id
          meta = None
          ``type`` = ``type`` }

type Coaches =
    { data: list<CoachesData>
      links: Option<obj> }
    ///Creates an instance of Coaches with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<CoachesData>): Coaches = { data = data; links = None }

///1. If the query does not return any groups, a group with the specified name will be created and related to the patient.
///2. If the query returns one group, that group will be related to the patient.
///3. If the query returns more than one group, the creation of the patient will fail.
type Query =
    { name: string
      organization: string }
    ///Creates an instance of Query with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (name: string, organization: string): Query =
        { name = name
          organization = organization }

///Allows the specification of a query for a group rather than providing a group id directly
type GroupsDataMeta =
    { ///1. If the query does not return any groups, a group with the specified name will be created and related to the patient.
      ///2. If the query returns one group, that group will be related to the patient.
      ///3. If the query returns more than one group, the creation of the patient will fail.
      query: Query }
    ///Creates an instance of GroupsDataMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (query: Query): GroupsDataMeta = { query = query }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type GroupsDataType =
    | [<CompiledName "group">] Group
    member this.Format() =
        match this with
        | Group -> "group"

type GroupsData =
    { ///Required if the `meta.query` is not defined.
      id: Option<string>
      ///Allows the specification of a query for a group rather than providing a group id directly
      meta: Option<GroupsDataMeta>
      ``type``: GroupsDataType }
    ///Creates an instance of GroupsData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: GroupsDataType): GroupsData =
        { id = None
          meta = None
          ``type`` = ``type`` }

type Groups =
    { data: list<GroupsData> }
    ///Creates an instance of Groups with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: list<GroupsData>): Groups = { data = data }

type CoachesAndGroups =
    { coaches: Option<Coaches>
      groups: Groups }
    ///Creates an instance of CoachesAndGroups with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (groups: Groups): CoachesAndGroups = { coaches = None; groups = groups }

type PatientCreateResource =
    { attributes: Option<PatientCreateResourceAttributes>
      id: Option<string>
      links: Option<PatientCreateResourceLinks>
      relationships: Option<PatientCreateResourceRelationships>
      ``type``: Option<PatientCreateResourceType> }
    ///Creates an instance of PatientCreateResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientCreateResource =
        { attributes = None
          id = None
          links = None
          relationships = None
          ``type`` = None }

type PatientHealthMetricCreateResourceAttributesCode =
    { system: string
      value: string }
    ///Creates an instance of PatientHealthMetricCreateResourceAttributesCode with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): PatientHealthMetricCreateResourceAttributesCode =
        { system = system; value = value }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PatientHealthMetricCreateResourceAttributesType =
    | [<CompiledName "blood_pressure_systolic">] Blood_pressure_systolic
    | [<CompiledName "blood_pressure_diastolic">] Blood_pressure_diastolic
    | [<CompiledName "hemoglobin_a1c">] Hemoglobin_a1c
    | [<CompiledName "hdl_cholesterol">] Hdl_cholesterol
    | [<CompiledName "ldl_cholesterol">] Ldl_cholesterol
    | [<CompiledName "total_cholesterol">] Total_cholesterol
    | [<CompiledName "triglycerides">] Triglycerides
    | [<CompiledName "blood_urea_nitrogen">] Blood_urea_nitrogen
    | [<CompiledName "creatinine">] Creatinine
    | [<CompiledName "hemoglobin">] Hemoglobin
    | [<CompiledName "hematocrit">] Hematocrit
    | [<CompiledName "total_serum_iron">] Total_serum_iron
    | [<CompiledName "thyroid_stimulating_hormone">] Thyroid_stimulating_hormone
    | [<CompiledName "free_thyroxine">] Free_thyroxine
    | [<CompiledName "free_triiodothyronine">] Free_triiodothyronine
    | [<CompiledName "total_triiodothyronine">] Total_triiodothyronine
    | [<CompiledName "cd4_cell_count">] Cd4_cell_count
    | [<CompiledName "hiv_viral_load">] Hiv_viral_load
    | [<CompiledName "inr">] Inr
    | [<CompiledName "free_testosterone">] Free_testosterone
    | [<CompiledName "total_testosterone">] Total_testosterone
    | [<CompiledName "c_reactive_protein">] C_reactive_protein
    | [<CompiledName "prostate_specific_antigen">] Prostate_specific_antigen
    | [<CompiledName "cotinine">] Cotinine
    | [<CompiledName "c_peptide">] C_peptide
    | [<CompiledName "blood_pressure">] Blood_pressure
    | [<CompiledName "blood_glucose">] Blood_glucose
    | [<CompiledName "weight">] Weight
    | [<CompiledName "heart_rate">] Heart_rate
    | [<CompiledName "body_fat_percentage">] Body_fat_percentage
    | [<CompiledName "body_mass_index">] Body_mass_index
    | [<CompiledName "body_temperature">] Body_temperature
    | [<CompiledName "forced_expiratory_volume1">] Forced_expiratory_volume1
    | [<CompiledName "forced_vital_capacity">] Forced_vital_capacity
    | [<CompiledName "lean_body_mass">] Lean_body_mass
    | [<CompiledName "nausea_level">] Nausea_level
    | [<CompiledName "oxygen_saturation">] Oxygen_saturation
    | [<CompiledName "pain_level">] Pain_level
    | [<CompiledName "peak_expiratory_flow_rate">] Peak_expiratory_flow_rate
    | [<CompiledName "peripheral_perfusion_index">] Peripheral_perfusion_index
    | [<CompiledName "respiratory_rate">] Respiratory_rate
    | [<CompiledName "inhaler_usage">] Inhaler_usage
    member this.Format() =
        match this with
        | Blood_pressure_systolic -> "blood_pressure_systolic"
        | Blood_pressure_diastolic -> "blood_pressure_diastolic"
        | Hemoglobin_a1c -> "hemoglobin_a1c"
        | Hdl_cholesterol -> "hdl_cholesterol"
        | Ldl_cholesterol -> "ldl_cholesterol"
        | Total_cholesterol -> "total_cholesterol"
        | Triglycerides -> "triglycerides"
        | Blood_urea_nitrogen -> "blood_urea_nitrogen"
        | Creatinine -> "creatinine"
        | Hemoglobin -> "hemoglobin"
        | Hematocrit -> "hematocrit"
        | Total_serum_iron -> "total_serum_iron"
        | Thyroid_stimulating_hormone -> "thyroid_stimulating_hormone"
        | Free_thyroxine -> "free_thyroxine"
        | Free_triiodothyronine -> "free_triiodothyronine"
        | Total_triiodothyronine -> "total_triiodothyronine"
        | Cd4_cell_count -> "cd4_cell_count"
        | Hiv_viral_load -> "hiv_viral_load"
        | Inr -> "inr"
        | Free_testosterone -> "free_testosterone"
        | Total_testosterone -> "total_testosterone"
        | C_reactive_protein -> "c_reactive_protein"
        | Prostate_specific_antigen -> "prostate_specific_antigen"
        | Cotinine -> "cotinine"
        | C_peptide -> "c_peptide"
        | Blood_pressure -> "blood_pressure"
        | Blood_glucose -> "blood_glucose"
        | Weight -> "weight"
        | Heart_rate -> "heart_rate"
        | Body_fat_percentage -> "body_fat_percentage"
        | Body_mass_index -> "body_mass_index"
        | Body_temperature -> "body_temperature"
        | Forced_expiratory_volume1 -> "forced_expiratory_volume1"
        | Forced_vital_capacity -> "forced_vital_capacity"
        | Lean_body_mass -> "lean_body_mass"
        | Nausea_level -> "nausea_level"
        | Oxygen_saturation -> "oxygen_saturation"
        | Pain_level -> "pain_level"
        | Peak_expiratory_flow_rate -> "peak_expiratory_flow_rate"
        | Peripheral_perfusion_index -> "peripheral_perfusion_index"
        | Respiratory_rate -> "respiratory_rate"
        | Inhaler_usage -> "inhaler_usage"

type PatientHealthMetricCreateResourceAttributes =
    { code: Option<PatientHealthMetricCreateResourceAttributesCode>
      diastolic: Option<float>
      occurred_at: Option<string>
      systolic: Option<float>
      ``type``: Option<PatientHealthMetricCreateResourceAttributesType>
      unit: Option<string>
      ///Can be any value (number, boolean, string, object) depending on the metric type. Most values are of type number.
      value: Option<obj> }
    ///Creates an instance of PatientHealthMetricCreateResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricCreateResourceAttributes =
        { code = None
          diastolic = None
          occurred_at = None
          systolic = None
          ``type`` = None
          unit = None
          value = None }

type SystemAndValue =
    { system: string
      value: string }
    ///Creates an instance of SystemAndValue with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): SystemAndValue = { system = system; value = value }

///The query must return one and only one patient.
type PatientHealthMetricCreateResourceRelationshipsPatientDataMetaQuery =
    { groups: Option<list<string>>
      identifier: SystemAndValue
      organization: Option<string> }
    ///Creates an instance of PatientHealthMetricCreateResourceRelationshipsPatientDataMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (identifier: SystemAndValue): PatientHealthMetricCreateResourceRelationshipsPatientDataMetaQuery =
        { groups = None
          identifier = identifier
          organization = None }

///Allows the specification of a query for a patient rather than providing a patient id directly
type PatientHealthMetricCreateResourceRelationshipsPatientDataMeta =
    { ///The query must return one and only one patient.
      query: PatientHealthMetricCreateResourceRelationshipsPatientDataMetaQuery }
    ///Creates an instance of PatientHealthMetricCreateResourceRelationshipsPatientDataMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (query: PatientHealthMetricCreateResourceRelationshipsPatientDataMetaQuery): PatientHealthMetricCreateResourceRelationshipsPatientDataMeta =
        { query = query }

type PatientHealthMetricCreateResourceRelationshipsPatientData =
    { ///Required if the `meta.query` is not defined.
      id: Option<string>
      ///Allows the specification of a query for a patient rather than providing a patient id directly
      meta: Option<PatientHealthMetricCreateResourceRelationshipsPatientDataMeta>
      ``type``: Option<string> }
    ///Creates an instance of PatientHealthMetricCreateResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricCreateResourceRelationshipsPatientData =
        { id = None
          meta = None
          ``type`` = None }

type PatientHealthMetricCreateResourceRelationshipsPatient =
    { data: Option<PatientHealthMetricCreateResourceRelationshipsPatientData>
      links: Option<obj> }
    ///Creates an instance of PatientHealthMetricCreateResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricCreateResourceRelationshipsPatient = { data = None; links = None }

type PatientHealthMetricCreateResourceRelationships =
    { patient: Option<PatientHealthMetricCreateResourceRelationshipsPatient> }
    ///Creates an instance of PatientHealthMetricCreateResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricCreateResourceRelationships = { patient = None }

type PatientHealthMetricCreateResource =
    { attributes: Option<PatientHealthMetricCreateResourceAttributes>
      id: Option<string>
      relationships: Option<PatientHealthMetricCreateResourceRelationships>
      ``type``: Option<string> }
    ///Creates an instance of PatientHealthMetricCreateResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricCreateResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = None }

type PatientHealthMetricResourceAttributesCode =
    { system: string
      value: string }
    ///Creates an instance of PatientHealthMetricResourceAttributesCode with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): PatientHealthMetricResourceAttributesCode =
        { system = system; value = value }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PatientHealthMetricResourceAttributesType =
    | [<CompiledName "blood_pressure_systolic">] Blood_pressure_systolic
    | [<CompiledName "blood_pressure_diastolic">] Blood_pressure_diastolic
    | [<CompiledName "hemoglobin_a1c">] Hemoglobin_a1c
    | [<CompiledName "hdl_cholesterol">] Hdl_cholesterol
    | [<CompiledName "ldl_cholesterol">] Ldl_cholesterol
    | [<CompiledName "total_cholesterol">] Total_cholesterol
    | [<CompiledName "triglycerides">] Triglycerides
    | [<CompiledName "blood_urea_nitrogen">] Blood_urea_nitrogen
    | [<CompiledName "creatinine">] Creatinine
    | [<CompiledName "hemoglobin">] Hemoglobin
    | [<CompiledName "hematocrit">] Hematocrit
    | [<CompiledName "total_serum_iron">] Total_serum_iron
    | [<CompiledName "thyroid_stimulating_hormone">] Thyroid_stimulating_hormone
    | [<CompiledName "free_thyroxine">] Free_thyroxine
    | [<CompiledName "free_triiodothyronine">] Free_triiodothyronine
    | [<CompiledName "total_triiodothyronine">] Total_triiodothyronine
    | [<CompiledName "cd4_cell_count">] Cd4_cell_count
    | [<CompiledName "hiv_viral_load">] Hiv_viral_load
    | [<CompiledName "inr">] Inr
    | [<CompiledName "free_testosterone">] Free_testosterone
    | [<CompiledName "total_testosterone">] Total_testosterone
    | [<CompiledName "c_reactive_protein">] C_reactive_protein
    | [<CompiledName "prostate_specific_antigen">] Prostate_specific_antigen
    | [<CompiledName "cotinine">] Cotinine
    | [<CompiledName "c_peptide">] C_peptide
    | [<CompiledName "blood_pressure">] Blood_pressure
    | [<CompiledName "blood_glucose">] Blood_glucose
    | [<CompiledName "weight">] Weight
    | [<CompiledName "heart_rate">] Heart_rate
    | [<CompiledName "body_fat_percentage">] Body_fat_percentage
    | [<CompiledName "body_mass_index">] Body_mass_index
    | [<CompiledName "body_temperature">] Body_temperature
    | [<CompiledName "forced_expiratory_volume1">] Forced_expiratory_volume1
    | [<CompiledName "forced_vital_capacity">] Forced_vital_capacity
    | [<CompiledName "lean_body_mass">] Lean_body_mass
    | [<CompiledName "nausea_level">] Nausea_level
    | [<CompiledName "oxygen_saturation">] Oxygen_saturation
    | [<CompiledName "pain_level">] Pain_level
    | [<CompiledName "peak_expiratory_flow_rate">] Peak_expiratory_flow_rate
    | [<CompiledName "peripheral_perfusion_index">] Peripheral_perfusion_index
    | [<CompiledName "respiratory_rate">] Respiratory_rate
    | [<CompiledName "inhaler_usage">] Inhaler_usage
    member this.Format() =
        match this with
        | Blood_pressure_systolic -> "blood_pressure_systolic"
        | Blood_pressure_diastolic -> "blood_pressure_diastolic"
        | Hemoglobin_a1c -> "hemoglobin_a1c"
        | Hdl_cholesterol -> "hdl_cholesterol"
        | Ldl_cholesterol -> "ldl_cholesterol"
        | Total_cholesterol -> "total_cholesterol"
        | Triglycerides -> "triglycerides"
        | Blood_urea_nitrogen -> "blood_urea_nitrogen"
        | Creatinine -> "creatinine"
        | Hemoglobin -> "hemoglobin"
        | Hematocrit -> "hematocrit"
        | Total_serum_iron -> "total_serum_iron"
        | Thyroid_stimulating_hormone -> "thyroid_stimulating_hormone"
        | Free_thyroxine -> "free_thyroxine"
        | Free_triiodothyronine -> "free_triiodothyronine"
        | Total_triiodothyronine -> "total_triiodothyronine"
        | Cd4_cell_count -> "cd4_cell_count"
        | Hiv_viral_load -> "hiv_viral_load"
        | Inr -> "inr"
        | Free_testosterone -> "free_testosterone"
        | Total_testosterone -> "total_testosterone"
        | C_reactive_protein -> "c_reactive_protein"
        | Prostate_specific_antigen -> "prostate_specific_antigen"
        | Cotinine -> "cotinine"
        | C_peptide -> "c_peptide"
        | Blood_pressure -> "blood_pressure"
        | Blood_glucose -> "blood_glucose"
        | Weight -> "weight"
        | Heart_rate -> "heart_rate"
        | Body_fat_percentage -> "body_fat_percentage"
        | Body_mass_index -> "body_mass_index"
        | Body_temperature -> "body_temperature"
        | Forced_expiratory_volume1 -> "forced_expiratory_volume1"
        | Forced_vital_capacity -> "forced_vital_capacity"
        | Lean_body_mass -> "lean_body_mass"
        | Nausea_level -> "nausea_level"
        | Oxygen_saturation -> "oxygen_saturation"
        | Pain_level -> "pain_level"
        | Peak_expiratory_flow_rate -> "peak_expiratory_flow_rate"
        | Peripheral_perfusion_index -> "peripheral_perfusion_index"
        | Respiratory_rate -> "respiratory_rate"
        | Inhaler_usage -> "inhaler_usage"

type PatientHealthMetricResourceAttributes =
    { code: Option<PatientHealthMetricResourceAttributesCode>
      diastolic: Option<float>
      occurred_at: Option<string>
      systolic: Option<float>
      ``type``: Option<PatientHealthMetricResourceAttributesType>
      unit: Option<string>
      ///Can be any value (number, boolean, string, object) depending on the metric type. Most values are of type number.
      value: Option<obj> }
    ///Creates an instance of PatientHealthMetricResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricResourceAttributes =
        { code = None
          diastolic = None
          occurred_at = None
          systolic = None
          ``type`` = None
          unit = None
          value = None }

type IdentifierFromPatientHealthMetricResourceRelationshipsPatientDataMetaQuery =
    { system: string
      value: string }
    ///Creates an instance of IdentifierFromPatientHealthMetricResourceRelationshipsPatientDataMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): IdentifierFromPatientHealthMetricResourceRelationshipsPatientDataMetaQuery =
        { system = system; value = value }

///The query must return one and only one patient.
type PatientHealthMetricResourceRelationshipsPatientDataMetaQuery =
    { groups: Option<list<string>>
      identifier: IdentifierFromPatientHealthMetricResourceRelationshipsPatientDataMetaQuery
      organization: Option<string> }
    ///Creates an instance of PatientHealthMetricResourceRelationshipsPatientDataMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (identifier: IdentifierFromPatientHealthMetricResourceRelationshipsPatientDataMetaQuery): PatientHealthMetricResourceRelationshipsPatientDataMetaQuery =
        { groups = None
          identifier = identifier
          organization = None }

///Allows the specification of a query for a patient rather than providing a patient id directly
type PatientHealthMetricResourceRelationshipsPatientDataMeta =
    { ///The query must return one and only one patient.
      query: PatientHealthMetricResourceRelationshipsPatientDataMetaQuery }
    ///Creates an instance of PatientHealthMetricResourceRelationshipsPatientDataMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (query: PatientHealthMetricResourceRelationshipsPatientDataMetaQuery): PatientHealthMetricResourceRelationshipsPatientDataMeta =
        { query = query }

type PatientHealthMetricResourceRelationshipsPatientData =
    { ///Required if the `meta.query` is not defined.
      id: Option<string>
      ///Allows the specification of a query for a patient rather than providing a patient id directly
      meta: Option<PatientHealthMetricResourceRelationshipsPatientDataMeta>
      ``type``: Option<string> }
    ///Creates an instance of PatientHealthMetricResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricResourceRelationshipsPatientData =
        { id = None
          meta = None
          ``type`` = None }

type PatientHealthMetricResourceRelationshipsPatient =
    { data: Option<PatientHealthMetricResourceRelationshipsPatientData>
      links: Option<obj> }
    ///Creates an instance of PatientHealthMetricResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricResourceRelationshipsPatient = { data = None; links = None }

type PatientHealthMetricResourceRelationships =
    { patient: Option<PatientHealthMetricResourceRelationshipsPatient> }
    ///Creates an instance of PatientHealthMetricResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthMetricResourceRelationships = { patient = None }

type PatientHealthMetricResource =
    { attributes: Option<PatientHealthMetricResourceAttributes>
      id: string
      relationships: Option<PatientHealthMetricResourceRelationships>
      ``type``: string }
    ///Creates an instance of PatientHealthMetricResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): PatientHealthMetricResource =
        { attributes = None
          id = id
          relationships = None
          ``type`` = ``type`` }

type Annotations =
    { text: Option<string>
      title: Option<string> }
    ///Creates an instance of Annotations with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Annotations = { text = None; title = None }

type PatientHealthResultResourceAttributesData =
    { ///Can be any value (number, boolean, string, object) depending on the metric type. Most values are of type number
      value: Option<obj> }
    ///Creates an instance of PatientHealthResultResourceAttributesData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthResultResourceAttributesData = { value = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Metrictype =
    | [<CompiledName "blood_pressure_systolic">] Blood_pressure_systolic
    | [<CompiledName "blood_pressure_diastolic">] Blood_pressure_diastolic
    | [<CompiledName "hemoglobin_a1c">] Hemoglobin_a1c
    | [<CompiledName "hdl_cholesterol">] Hdl_cholesterol
    | [<CompiledName "ldl_cholesterol">] Ldl_cholesterol
    | [<CompiledName "total_cholesterol">] Total_cholesterol
    | [<CompiledName "triglycerides">] Triglycerides
    | [<CompiledName "blood_urea_nitrogen">] Blood_urea_nitrogen
    | [<CompiledName "creatinine">] Creatinine
    | [<CompiledName "hemoglobin">] Hemoglobin
    | [<CompiledName "hematocrit">] Hematocrit
    | [<CompiledName "total_serum_iron">] Total_serum_iron
    | [<CompiledName "thyroid_stimulating_hormone">] Thyroid_stimulating_hormone
    | [<CompiledName "free_thyroxine">] Free_thyroxine
    | [<CompiledName "free_triiodothyronine">] Free_triiodothyronine
    | [<CompiledName "total_triiodothyronine">] Total_triiodothyronine
    | [<CompiledName "cd4_cell_count">] Cd4_cell_count
    | [<CompiledName "hiv_viral_load">] Hiv_viral_load
    | [<CompiledName "inr">] Inr
    | [<CompiledName "free_testosterone">] Free_testosterone
    | [<CompiledName "total_testosterone">] Total_testosterone
    | [<CompiledName "c_reactive_protein">] C_reactive_protein
    | [<CompiledName "prostate_specific_antigen">] Prostate_specific_antigen
    | [<CompiledName "cotinine">] Cotinine
    | [<CompiledName "c_peptide">] C_peptide
    | [<CompiledName "blood_pressure">] Blood_pressure
    | [<CompiledName "blood_glucose">] Blood_glucose
    | [<CompiledName "weight">] Weight
    | [<CompiledName "heart_rate">] Heart_rate
    | [<CompiledName "body_fat_percentage">] Body_fat_percentage
    | [<CompiledName "body_mass_index">] Body_mass_index
    | [<CompiledName "body_temperature">] Body_temperature
    | [<CompiledName "forced_expiratory_volume1">] Forced_expiratory_volume1
    | [<CompiledName "forced_vital_capacity">] Forced_vital_capacity
    | [<CompiledName "lean_body_mass">] Lean_body_mass
    | [<CompiledName "nausea_level">] Nausea_level
    | [<CompiledName "oxygen_saturation">] Oxygen_saturation
    | [<CompiledName "pain_level">] Pain_level
    | [<CompiledName "peak_expiratory_flow_rate">] Peak_expiratory_flow_rate
    | [<CompiledName "peripheral_perfusion_index">] Peripheral_perfusion_index
    | [<CompiledName "respiratory_rate">] Respiratory_rate
    | [<CompiledName "inhaler_usage">] Inhaler_usage
    member this.Format() =
        match this with
        | Blood_pressure_systolic -> "blood_pressure_systolic"
        | Blood_pressure_diastolic -> "blood_pressure_diastolic"
        | Hemoglobin_a1c -> "hemoglobin_a1c"
        | Hdl_cholesterol -> "hdl_cholesterol"
        | Ldl_cholesterol -> "ldl_cholesterol"
        | Total_cholesterol -> "total_cholesterol"
        | Triglycerides -> "triglycerides"
        | Blood_urea_nitrogen -> "blood_urea_nitrogen"
        | Creatinine -> "creatinine"
        | Hemoglobin -> "hemoglobin"
        | Hematocrit -> "hematocrit"
        | Total_serum_iron -> "total_serum_iron"
        | Thyroid_stimulating_hormone -> "thyroid_stimulating_hormone"
        | Free_thyroxine -> "free_thyroxine"
        | Free_triiodothyronine -> "free_triiodothyronine"
        | Total_triiodothyronine -> "total_triiodothyronine"
        | Cd4_cell_count -> "cd4_cell_count"
        | Hiv_viral_load -> "hiv_viral_load"
        | Inr -> "inr"
        | Free_testosterone -> "free_testosterone"
        | Total_testosterone -> "total_testosterone"
        | C_reactive_protein -> "c_reactive_protein"
        | Prostate_specific_antigen -> "prostate_specific_antigen"
        | Cotinine -> "cotinine"
        | C_peptide -> "c_peptide"
        | Blood_pressure -> "blood_pressure"
        | Blood_glucose -> "blood_glucose"
        | Weight -> "weight"
        | Heart_rate -> "heart_rate"
        | Body_fat_percentage -> "body_fat_percentage"
        | Body_mass_index -> "body_mass_index"
        | Body_temperature -> "body_temperature"
        | Forced_expiratory_volume1 -> "forced_expiratory_volume1"
        | Forced_vital_capacity -> "forced_vital_capacity"
        | Lean_body_mass -> "lean_body_mass"
        | Nausea_level -> "nausea_level"
        | Oxygen_saturation -> "oxygen_saturation"
        | Pain_level -> "pain_level"
        | Peak_expiratory_flow_rate -> "peak_expiratory_flow_rate"
        | Peripheral_perfusion_index -> "peripheral_perfusion_index"
        | Respiratory_rate -> "respiratory_rate"
        | Inhaler_usage -> "inhaler_usage"

type PatientHealthResultResourceAttributesSource =
    { ///Can be any value
      metadata: Option<obj>
      name: Option<string>
      source_id: Option<string> }
    ///Creates an instance of PatientHealthResultResourceAttributesSource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthResultResourceAttributesSource =
        { metadata = None
          name = None
          source_id = None }

type PatientHealthResultResourceAttributes =
    { ///Links together results. This should be the same as the thread of _action, if it is defined
      _thread: Option<string>
      aggregation: Option<string>
      annotations: Option<list<Annotations>>
      channel: Option<string>
      data: Option<PatientHealthResultResourceAttributesData>
      external_id: Option<string>
      metric_type: Option<Metrictype>
      occurred_at: Option<string>
      occurred_at_time_zone: Option<string>
      skipped: Option<bool>
      source: Option<PatientHealthResultResourceAttributesSource>
      ///Type of result. Usually the same as metric_type except for lifestyle actions
      ``type``: Option<string>
      window: Option<string> }
    ///Creates an instance of PatientHealthResultResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthResultResourceAttributes =
        { _thread = None
          aggregation = None
          annotations = None
          channel = None
          data = None
          external_id = None
          metric_type = None
          occurred_at = None
          occurred_at_time_zone = None
          skipped = None
          source = None
          ``type`` = None
          window = None }

type ActionData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of ActionData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ActionData = { id = None; ``type`` = None }

type Action =
    { data: Option<ActionData>
      links: Option<obj> }
    ///Creates an instance of Action with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Action = { data = None; links = None }

type MetricData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of MetricData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): MetricData = { id = None; ``type`` = None }

type Metric =
    { data: Option<MetricData>
      links: Option<obj> }
    ///Creates an instance of Metric with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Metric = { data = None; links = None }

type IdentifierFromPatientHealthResultResourceRelationshipsPatientDataMetaQuery =
    { system: string
      value: string }
    ///Creates an instance of IdentifierFromPatientHealthResultResourceRelationshipsPatientDataMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): IdentifierFromPatientHealthResultResourceRelationshipsPatientDataMetaQuery =
        { system = system; value = value }

///The query must return one and only one patient.
type PatientHealthResultResourceRelationshipsPatientDataMetaQuery =
    { groups: Option<list<string>>
      identifier: IdentifierFromPatientHealthResultResourceRelationshipsPatientDataMetaQuery
      organization: Option<string> }
    ///Creates an instance of PatientHealthResultResourceRelationshipsPatientDataMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (identifier: IdentifierFromPatientHealthResultResourceRelationshipsPatientDataMetaQuery): PatientHealthResultResourceRelationshipsPatientDataMetaQuery =
        { groups = None
          identifier = identifier
          organization = None }

///Allows the specification of a query for a patient rather than providing a patient id directly
type PatientHealthResultResourceRelationshipsPatientDataMeta =
    { ///The query must return one and only one patient.
      query: PatientHealthResultResourceRelationshipsPatientDataMetaQuery }
    ///Creates an instance of PatientHealthResultResourceRelationshipsPatientDataMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (query: PatientHealthResultResourceRelationshipsPatientDataMetaQuery): PatientHealthResultResourceRelationshipsPatientDataMeta =
        { query = query }

type PatientHealthResultResourceRelationshipsPatientData =
    { ///Required if the `meta.query` is not defined.
      id: Option<string>
      ///Allows the specification of a query for a patient rather than providing a patient id directly
      meta: Option<PatientHealthResultResourceRelationshipsPatientDataMeta>
      ``type``: Option<string> }
    ///Creates an instance of PatientHealthResultResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthResultResourceRelationshipsPatientData =
        { id = None
          meta = None
          ``type`` = None }

type PatientHealthResultResourceRelationshipsPatient =
    { data: Option<PatientHealthResultResourceRelationshipsPatientData>
      links: Option<obj> }
    ///Creates an instance of PatientHealthResultResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthResultResourceRelationshipsPatient = { data = None; links = None }

type PatientHealthResultResourceRelationships =
    { action: Option<Action>
      metric: Option<Metric>
      patient: Option<PatientHealthResultResourceRelationshipsPatient> }
    ///Creates an instance of PatientHealthResultResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientHealthResultResourceRelationships =
        { action = None
          metric = None
          patient = None }

type PatientHealthResultResource =
    { attributes: Option<PatientHealthResultResourceAttributes>
      id: string
      relationships: Option<PatientHealthResultResourceRelationships>
      ``type``: string }
    ///Creates an instance of PatientHealthResultResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): PatientHealthResultResource =
        { attributes = None
          id = id
          relationships = None
          ``type`` = ``type`` }

type PatientIdentifier =
    { label: Option<string>
      system: string
      ///If `true`, the combination of system and value must be global unique among all patients and coaches in Fitbit Plus.
      unique: Option<bool>
      value: string }
    ///Creates an instance of PatientIdentifier with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (system: string, value: string): PatientIdentifier =
        { label = None
          system = system
          unique = None
          value = value }

type Windownotificationtimes =
    { afternoon: Option<int>
      evening: Option<int>
      morning: Option<int>
      night: Option<int> }
    ///Creates an instance of Windownotificationtimes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Windownotificationtimes =
        { afternoon = None
          evening = None
          morning = None
          night = None }

type Windoworder =
    { _actions: Option<list<string>>
      ``type``: Option<string> }
    ///Creates an instance of Windoworder with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Windoworder = { _actions = None; ``type`` = None }

type PatientPlanSummaryResourceAttributes =
    { adherence: Option<obj>
      critical: Option<obj>
      effective_from: Option<string>
      time_zone: Option<string>
      window_notification_times: Option<Windownotificationtimes>
      window_order: Option<list<Windoworder>> }
    ///Creates an instance of PatientPlanSummaryResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientPlanSummaryResourceAttributes =
        { adherence = None
          critical = None
          effective_from = None
          time_zone = None
          window_notification_times = None
          window_order = None }

type PatientPlanSummaryResourceLinks =
    { self: string }
    ///Creates an instance of PatientPlanSummaryResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (self: string): PatientPlanSummaryResourceLinks = { self = self }

type PatientPlanSummaryResourceRelationshipsActionsData =
    { id: string
      ``type``: string }
    ///Creates an instance of PatientPlanSummaryResourceRelationshipsActionsData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): PatientPlanSummaryResourceRelationshipsActionsData =
        { id = id; ``type`` = ``type`` }

type PatientPlanSummaryResourceRelationshipsActionsLinks =
    { related: Option<string> }
    ///Creates an instance of PatientPlanSummaryResourceRelationshipsActionsLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientPlanSummaryResourceRelationshipsActionsLinks = { related = None }

type PatientPlanSummaryResourceRelationshipsActions =
    { data: Option<list<PatientPlanSummaryResourceRelationshipsActionsData>>
      links: Option<PatientPlanSummaryResourceRelationshipsActionsLinks> }
    ///Creates an instance of PatientPlanSummaryResourceRelationshipsActions with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientPlanSummaryResourceRelationshipsActions = { data = None; links = None }

type BundlesData =
    { id: string
      ``type``: string }
    ///Creates an instance of BundlesData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): BundlesData = { id = id; ``type`` = ``type`` }

type BundlesLinks =
    { related: Option<string> }
    ///Creates an instance of BundlesLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BundlesLinks = { related = None }

type Bundles =
    { data: Option<list<BundlesData>>
      links: Option<BundlesLinks> }
    ///Creates an instance of Bundles with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Bundles = { data = None; links = None }

type CurrentresultsData =
    { id: string
      ``type``: string }
    ///Creates an instance of CurrentresultsData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): CurrentresultsData = { id = id; ``type`` = ``type`` }

type CurrentresultsLinks =
    { related: Option<string> }
    ///Creates an instance of CurrentresultsLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CurrentresultsLinks = { related = None }

type Currentresults =
    { data: Option<list<CurrentresultsData>>
      links: Option<CurrentresultsLinks> }
    ///Creates an instance of Currentresults with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Currentresults = { data = None; links = None }

type PatientPlanSummaryResourceRelationshipsPatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of PatientPlanSummaryResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientPlanSummaryResourceRelationshipsPatientData = { id = None; ``type`` = None }

type PatientPlanSummaryResourceRelationshipsPatientLinks =
    { related: Option<string> }
    ///Creates an instance of PatientPlanSummaryResourceRelationshipsPatientLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientPlanSummaryResourceRelationshipsPatientLinks = { related = None }

type PatientPlanSummaryResourceRelationshipsPatient =
    { data: Option<PatientPlanSummaryResourceRelationshipsPatientData>
      links: Option<PatientPlanSummaryResourceRelationshipsPatientLinks> }
    ///Creates an instance of PatientPlanSummaryResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientPlanSummaryResourceRelationshipsPatient = { data = None; links = None }

type PatientPlanSummaryResourceRelationships =
    { actions: PatientPlanSummaryResourceRelationshipsActions
      bundles: Bundles
      current_results: Option<Currentresults>
      patient: PatientPlanSummaryResourceRelationshipsPatient }
    ///Creates an instance of PatientPlanSummaryResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (actions: PatientPlanSummaryResourceRelationshipsActions,
                          bundles: Bundles,
                          patient: PatientPlanSummaryResourceRelationshipsPatient): PatientPlanSummaryResourceRelationships =
        { actions = actions
          bundles = bundles
          current_results = None
          patient = patient }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PatientPlanSummaryResourceType =
    | [<CompiledName "patient_plan_summary">] Patient_plan_summary
    member this.Format() =
        match this with
        | Patient_plan_summary -> "patient_plan_summary"

type PatientPlanSummaryResource =
    { attributes: Option<PatientPlanSummaryResourceAttributes>
      id: string
      links: Option<PatientPlanSummaryResourceLinks>
      relationships: Option<PatientPlanSummaryResourceRelationships>
      ``type``: PatientPlanSummaryResourceType }
    ///Creates an instance of PatientPlanSummaryResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: PatientPlanSummaryResourceType): PatientPlanSummaryResource =
        { attributes = None
          id = id
          links = None
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PatientResourceAttributesGender =
    | [<CompiledName "male">] Male
    | [<CompiledName "female">] Female
    | [<CompiledName "other">] Other
    member this.Format() =
        match this with
        | Male -> "male"
        | Female -> "female"
        | Other -> "other"

///A patient's motivation statement.
type PatientResourceAttributesStatement =
    { updated_at: Option<string>
      updated_by: Option<string>
      value: Option<string> }
    ///Creates an instance of PatientResourceAttributesStatement with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientResourceAttributesStatement =
        { updated_at = None
          updated_by = None
          value = None }

type PatientResourceAttributes =
    { addresses: Option<list<Address>>
      archive_history: Option<list<ArchiveHistory>>
      archived: Option<bool>
      birth_date: Option<string>
      email_address: Option<string>
      enrolled_at: Option<string>
      first_access_at: Option<string>
      first_name: Option<string>
      gender: Option<PatientResourceAttributesGender>
      identifiers: Option<list<PatientIdentifier>>
      invited_at: Option<string>
      last_access_at: Option<string>
      last_name: Option<string>
      ///Coach's note about the patient. Not visible to the patient.
      note: Option<string>
      phone_numbers: Option<list<PhoneNumber>>
      ///A patient's motivation statement.
      statement: Option<PatientResourceAttributesStatement>
      updated_at: Option<string> }
    ///Creates an instance of PatientResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientResourceAttributes =
        { addresses = None
          archive_history = None
          archived = None
          birth_date = None
          email_address = None
          enrolled_at = None
          first_access_at = None
          first_name = None
          gender = None
          identifiers = None
          invited_at = None
          last_access_at = None
          last_name = None
          note = None
          phone_numbers = None
          statement = None
          updated_at = None }

type PatientResourceLinks =
    { self: Option<string>
      ///A link to the patient record in the Fitbit Plus web application.
      twine_web_app: Option<string> }
    ///Creates an instance of PatientResourceLinks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PatientResourceLinks = { self = None; twine_web_app = None }

type PatientResourceRelationships =
    { coaches: Option<obj>
      groups: obj }
    ///Creates an instance of PatientResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (groups: obj): PatientResourceRelationships = { coaches = None; groups = groups }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PatientResourceType =
    | [<CompiledName "patient">] Patient
    member this.Format() =
        match this with
        | Patient -> "patient"

type PatientResource =
    { attributes: PatientResourceAttributes
      id: Option<string>
      links: Option<PatientResourceLinks>
      relationships: Option<PatientResourceRelationships>
      ``type``: PatientResourceType }
    ///Creates an instance of PatientResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (attributes: PatientResourceAttributes, ``type``: PatientResourceType): PatientResource =
        { attributes = attributes
          id = None
          links = None
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PhoneNumberType =
    | [<CompiledName "home">] Home
    | [<CompiledName "work">] Work
    | [<CompiledName "mobile">] Mobile
    | [<CompiledName "home-fax">] HomeFax
    | [<CompiledName "work-fax">] WorkFax
    | [<CompiledName "other">] Other
    member this.Format() =
        match this with
        | Home -> "home"
        | Work -> "work"
        | Mobile -> "mobile"
        | HomeFax -> "home-fax"
        | WorkFax -> "work-fax"
        | Other -> "other"

type PhoneNumber =
    { primary: bool
      ``type``: PhoneNumberType
      value: string }
    ///Creates an instance of PhoneNumber with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (primary: bool, ``type``: PhoneNumberType, value: string): PhoneNumber =
        { primary = primary
          ``type`` = ``type``
          value = value }

type Resource =
    { attributes: Option<obj>
      id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of Resource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Resource =
        { attributes = None
          id = None
          ``type`` = None }

type RewardEarningFulfillmentResourceAttributes =
    { ///Date at which the reward earning was fulfilled. (Must be at the same time or after the reward was earned)
      fulfilled_at: string
      ///Unit of the earned reward that has been fulfilled. (Read-only property)
      fulfilled_unit: Option<string>
      ///Value of the earned reward that has been fulfilled. (Must be greater than or equal to 0)
      fulfilled_value: float }
    ///Creates an instance of RewardEarningFulfillmentResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (fulfilled_at: string, fulfilled_value: float): RewardEarningFulfillmentResourceAttributes =
        { fulfilled_at = fulfilled_at
          fulfilled_unit = None
          fulfilled_value = fulfilled_value }

type RewardEarningFulfillmentResourceRelationshipsPatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardEarningFulfillmentResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardEarningFulfillmentResourceRelationshipsPatientData = { id = None; ``type`` = None }

type RewardEarningFulfillmentResourceRelationshipsPatient =
    { data: Option<RewardEarningFulfillmentResourceRelationshipsPatientData> }
    ///Creates an instance of RewardEarningFulfillmentResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardEarningFulfillmentResourceRelationshipsPatient = { data = None }

type RewardearningData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardearningData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardearningData = { id = None; ``type`` = None }

type Rewardearning =
    { data: Option<RewardearningData> }
    ///Creates an instance of Rewardearning with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Rewardearning = { data = None }

type RewardEarningFulfillmentResourceRelationships =
    { patient: Option<RewardEarningFulfillmentResourceRelationshipsPatient>
      reward_earning: Rewardearning }
    ///Creates an instance of RewardEarningFulfillmentResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (reward_earning: Rewardearning): RewardEarningFulfillmentResourceRelationships =
        { patient = None
          reward_earning = reward_earning }

type RewardEarningFulfillmentResource =
    { attributes: Option<RewardEarningFulfillmentResourceAttributes>
      id: Option<string>
      relationships: Option<RewardEarningFulfillmentResourceRelationships>
      ``type``: string }
    ///Creates an instance of RewardEarningFulfillmentResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): RewardEarningFulfillmentResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = ``type`` }

type RewardEarningResourceAttributes =
    { ///Date at which the reward was earned. (Must be after the reward was allocated and before the reward program activation was deactivated or expired)
      earned_at: string
      ///Unit of the reward that has been earned. (Read-only property)
      earned_unit: Option<string>
      ///Value of the reward that has been earned. (Must not exceed the allocated value for the reward)
      earned_value: float
      ///Date at which the reward earning was fulfilled. (Read-only property)
      fulfilled_at: Option<string>
      ///Value of the earned reward that has been fulfilled. (Read-only property)
      fulfilled_value: Option<float>
      ///If true, the reward earning is ready to be fulfilled, either because the reward program activation was fulfill_as_earned or because the reward program activation has been deactivated. (Read-only property)
      ready_for_fulfillment: Option<bool> }
    ///Creates an instance of RewardEarningResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (earned_at: string, earned_value: float): RewardEarningResourceAttributes =
        { earned_at = earned_at
          earned_unit = None
          earned_value = earned_value
          fulfilled_at = None
          fulfilled_value = None
          ready_for_fulfillment = None }

type GroupData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of GroupData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): GroupData = { id = None; ``type`` = None }

type Group =
    { data: Option<GroupData> }
    ///Creates an instance of Group with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Group = { data = None }

type RewardEarningResourceRelationshipsPatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardEarningResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardEarningResourceRelationshipsPatientData = { id = None; ``type`` = None }

type RewardEarningResourceRelationshipsPatient =
    { data: Option<RewardEarningResourceRelationshipsPatientData> }
    ///Creates an instance of RewardEarningResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardEarningResourceRelationshipsPatient = { data = None }

type RewardData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardData = { id = None; ``type`` = None }

type Reward =
    { data: Option<RewardData> }
    ///Creates an instance of Reward with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Reward = { data = None }

type RewardprogramactivationData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardprogramactivationData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardprogramactivationData = { id = None; ``type`` = None }

type Rewardprogramactivation =
    { data: Option<RewardprogramactivationData> }
    ///Creates an instance of Rewardprogramactivation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Rewardprogramactivation = { data = None }

type RewardEarningResourceRelationships =
    { group: Option<Group>
      patient: Option<RewardEarningResourceRelationshipsPatient>
      reward: Reward
      reward_program_activation: Option<Rewardprogramactivation> }
    ///Creates an instance of RewardEarningResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (reward: Reward): RewardEarningResourceRelationships =
        { group = None
          patient = None
          reward = reward
          reward_program_activation = None }

type RewardEarningResource =
    { attributes: Option<RewardEarningResourceAttributes>
      id: Option<string>
      relationships: Option<RewardEarningResourceRelationships>
      ``type``: string }
    ///Creates an instance of RewardEarningResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): RewardEarningResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = ``type`` }

type RewardProgramActivationResourceAttributes =
    { ///Date at which the reward program was activated for the patient. (Must be between the start_at and end_at dates for the reward program)
      activated_at: string
      ///If true, the reward program is currently active.
      active: Option<bool>
      ///Number of rewards allocated. (Read-only property)
      allocated_count: Option<float>
      ///Unit of the reward program budget. (Read-only property)
      budget_unit: Option<string>
      ///Date at which the reward program was deactivated. (Must be after the activated_at date)
      deactivated_at: Option<string>
      ///Number of reward earnings. (Read-only property)
      earned_count: Option<float>
      ///Date at which the reward program activation expires. (Read-only property set by adding the days_active from the reward program to the activated_at date)
      expires_at: Option<string>
      ///If true, the rewards created for a patient for the program can be fulfulled as they are earned. If false, the rewards should only be fulfilled when the program is deactivated. (Read-only property denormalized from the reward program)
      fulfill_as_earned: Option<bool>
      ///Total value of reward allocated. (Read-only property)
      total_allocated_value: Option<float>
      ///Total value of reward earnings. (Read-only property)
      total_earned_value: Option<float> }
    ///Creates an instance of RewardProgramActivationResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (activated_at: string): RewardProgramActivationResourceAttributes =
        { activated_at = activated_at
          active = None
          allocated_count = None
          budget_unit = None
          deactivated_at = None
          earned_count = None
          expires_at = None
          fulfill_as_earned = None
          total_allocated_value = None
          total_earned_value = None }

type RewardProgramActivationResourceRelationshipsPatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardProgramActivationResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardProgramActivationResourceRelationshipsPatientData = { id = None; ``type`` = None }

type RewardProgramActivationResourceRelationshipsPatient =
    { data: Option<RewardProgramActivationResourceRelationshipsPatientData> }
    ///Creates an instance of RewardProgramActivationResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardProgramActivationResourceRelationshipsPatient = { data = None }

type RewardprogramData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardprogramData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardprogramData = { id = None; ``type`` = None }

type Rewardprogram =
    { data: Option<RewardprogramData> }
    ///Creates an instance of Rewardprogram with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Rewardprogram = { data = None }

type RewardProgramActivationResourceRelationships =
    { patient: RewardProgramActivationResourceRelationshipsPatient
      reward_program: Rewardprogram }
    ///Creates an instance of RewardProgramActivationResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (patient: RewardProgramActivationResourceRelationshipsPatient, reward_program: Rewardprogram): RewardProgramActivationResourceRelationships =
        { patient = patient
          reward_program = reward_program }

type RewardProgramActivationResource =
    { attributes: Option<RewardProgramActivationResourceAttributes>
      id: Option<string>
      relationships: Option<RewardProgramActivationResourceRelationships>
      ``type``: string }
    ///Creates an instance of RewardProgramActivationResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): RewardProgramActivationResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = ``type`` }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Budgetunit =
    | [<CompiledName "dollar">] Dollar
    | [<CompiledName "point">] Point
    | [<CompiledName "credit">] Credit
    member this.Format() =
        match this with
        | Dollar -> "dollar"
        | Point -> "point"
        | Credit -> "credit"

type RewardProgramResourceAttributes =
    { ///Unit of the budget for the reard program.
      budget_unit: Option<Budgetunit>
      ///Value of the budget for the reward program. (Must be greater than 0)
      budget_value: float
      ///Description of the reward program - designed to be a comprehensive text description
      description: Option<string>
      ///Number of days that a program can be active after it has been activated for a patient. (Must be greater than 0)
      duration_active: Option<float>
      ///Date at which the reward program ends. (Must be after the start_at)
      end_at: string
      ///If true, the reward program cannot be activated for a patient and new rewards cannot be created for the program.
      frozen: Option<bool>
      ///If true, the rewards created for a patient for the program can be fulfulled as they are earned. If false, the rewards should only be fulfilled when the program is deactivated.
      fulfill_as_earned: Option<bool>
      ///Name of the reward program
      name: string
      ///Date at which the reward program starts.
      start_at: string
      ///Tagline of the reward program - designed to be one line
      tagline: Option<string> }
    ///Creates an instance of RewardProgramResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (budget_value: float, end_at: string, name: string, start_at: string): RewardProgramResourceAttributes =
        { budget_unit = None
          budget_value = budget_value
          description = None
          duration_active = None
          end_at = end_at
          frozen = None
          fulfill_as_earned = None
          name = name
          start_at = start_at
          tagline = None }

type RewardProgramResourceRelationshipsGroupData =
    { id: string
      ``type``: string }
    ///Creates an instance of RewardProgramResourceRelationshipsGroupData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: string): RewardProgramResourceRelationshipsGroupData =
        { id = id; ``type`` = ``type`` }

type RewardProgramResourceRelationshipsGroup =
    { data: RewardProgramResourceRelationshipsGroupData }
    ///Creates an instance of RewardProgramResourceRelationshipsGroup with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: RewardProgramResourceRelationshipsGroupData): RewardProgramResourceRelationshipsGroup =
        { data = data }

type RewardProgramResourceRelationships =
    { group: RewardProgramResourceRelationshipsGroup }
    ///Creates an instance of RewardProgramResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (group: RewardProgramResourceRelationshipsGroup): RewardProgramResourceRelationships =
        { group = group }

type RewardProgramResource =
    { attributes: Option<RewardProgramResourceAttributes>
      id: Option<string>
      relationships: Option<RewardProgramResourceRelationships>
      ``type``: string }
    ///Creates an instance of RewardProgramResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): RewardProgramResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = ``type`` }

type RewardResourceAttributes =
    { ///Unique string identifying the health action with which the reward is associated.
      _thread: Option<string>
      ///Date at which the reward was allocated. (Must be after the reward program is activated and before it is deactivated or expires)
      allocated_at: string
      ///Unit of the reward program. (Read-only property)
      allocated_unit: Option<string>
      ///Value of the reward program budget allocated for the reward. (Must not exceed the remaining budget for the reward program activation)
      allocated_value: float
      ///Description of the reward.
      description: string
      ///Date at which the reward was earned. (Read-only property)
      earned_at: Option<string>
      ///Value of the reward that has been earned. (Read-only property)
      earned_value: Option<float>
      ///Date at which the reward earning was fulfilled. (Read-only property)
      fulfilled_at: Option<string>
      ///Value of the earned reward that has been fulfilled. (Read-only property)
      fulfilled_value: Option<float>
      ///Date at which the patient aspires to achieve the reward. (Must be the same or after the allocated_at date)
      target_at: Option<string> }
    ///Creates an instance of RewardResourceAttributes with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (allocated_at: string, allocated_value: float, description: string): RewardResourceAttributes =
        { _thread = None
          allocated_at = allocated_at
          allocated_unit = None
          allocated_value = allocated_value
          description = description
          earned_at = None
          earned_value = None
          fulfilled_at = None
          fulfilled_value = None
          target_at = None }

type RewardResourceRelationshipsPatientData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardResourceRelationshipsPatientData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardResourceRelationshipsPatientData = { id = None; ``type`` = None }

type RewardResourceRelationshipsPatient =
    { data: Option<RewardResourceRelationshipsPatientData> }
    ///Creates an instance of RewardResourceRelationshipsPatient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardResourceRelationshipsPatient = { data = None }

type RewardResourceRelationshipsRewardprogramactivationData =
    { id: Option<string>
      ``type``: Option<string> }
    ///Creates an instance of RewardResourceRelationshipsRewardprogramactivationData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardResourceRelationshipsRewardprogramactivationData = { id = None; ``type`` = None }

type RewardResourceRelationshipsRewardprogramactivation =
    { data: Option<RewardResourceRelationshipsRewardprogramactivationData> }
    ///Creates an instance of RewardResourceRelationshipsRewardprogramactivation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): RewardResourceRelationshipsRewardprogramactivation = { data = None }

type RewardResourceRelationships =
    { patient: Option<RewardResourceRelationshipsPatient>
      reward_program_activation: RewardResourceRelationshipsRewardprogramactivation }
    ///Creates an instance of RewardResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (reward_program_activation: RewardResourceRelationshipsRewardprogramactivation): RewardResourceRelationships =
        { patient = None
          reward_program_activation = reward_program_activation }

type RewardResource =
    { attributes: Option<RewardResourceAttributes>
      id: Option<string>
      relationships: Option<RewardResourceRelationships>
      ``type``: string }
    ///Creates an instance of RewardResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (``type``: string): RewardResource =
        { attributes = None
          id = None
          relationships = None
          ``type`` = ``type`` }

type TokenResourceRelationships =
    { groups: Option<obj>
      organization: Option<obj> }
    ///Creates an instance of TokenResourceRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): TokenResourceRelationships = { groups = None; organization = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type TokenResourceType =
    | [<CompiledName "token">] Token
    member this.Format() =
        match this with
        | Token -> "token"

type TokenResource =
    { attributes: Option<obj>
      id: string
      relationships: Option<TokenResourceRelationships>
      ``type``: TokenResourceType }
    ///Creates an instance of TokenResource with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: string, ``type``: TokenResourceType): TokenResource =
        { attributes = None
          id = id
          relationships = None
          ``type`` = ``type`` }

type UpdateActionRequest =
    { data: ActionResource }
    ///Creates an instance of UpdateActionRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: ActionResource): UpdateActionRequest = { data = data }

type UpdateActionResponse =
    { data: ActionResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of UpdateActionResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: ActionResource): UpdateActionResponse = { data = data; meta = None }

type UpdateBundleRequest =
    { data: BundleResource }
    ///Creates an instance of UpdateBundleRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: BundleResource): UpdateBundleRequest = { data = data }

type UpdateBundleResponse =
    { data: BundleResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of UpdateBundleResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: BundleResource): UpdateBundleResponse = { data = data; meta = None }

type UpdateCalendarEventRequestDataRelationshipsOwner = Map<string, obj>

type UpdateCalendarEventRequestDataRelationships =
    { owner: Option<UpdateCalendarEventRequestDataRelationshipsOwner> }
    ///Creates an instance of UpdateCalendarEventRequestDataRelationships with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UpdateCalendarEventRequestDataRelationships = { owner = None }

type UpdateCalendarEventRequestData =
    { attributes: Option<obj>
      relationships: Option<UpdateCalendarEventRequestDataRelationships> }
    ///Creates an instance of UpdateCalendarEventRequestData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UpdateCalendarEventRequestData =
        { attributes = None
          relationships = None }

type UpdateCalendarEventRequest =
    { data: Option<UpdateCalendarEventRequestData> }
    ///Creates an instance of UpdateCalendarEventRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UpdateCalendarEventRequest = { data = None }

type UpdateCalendarEventResponse =
    { data: Option<CalendarEventResource>
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of UpdateCalendarEventResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): UpdateCalendarEventResponse = { data = None; meta = None }

type UpdatePatientPlanSummaryRequest =
    { data: PatientPlanSummaryResource }
    ///Creates an instance of UpdatePatientPlanSummaryRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientPlanSummaryResource): UpdatePatientPlanSummaryRequest = { data = data }

type UpdatePatientPlanSummaryResponse =
    { data: PatientPlanSummaryResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of UpdatePatientPlanSummaryResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientPlanSummaryResource): UpdatePatientPlanSummaryResponse =
        { data = data; meta = None }

type UpdatePatientRequest =
    { data: PatientResource }
    ///Creates an instance of UpdatePatientRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientResource): UpdatePatientRequest = { data = data }

type UpdatePatientResponse =
    { data: PatientResource
      meta: Option<CreateOrUpdateMetaResponse> }
    ///Creates an instance of UpdatePatientResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientResource): UpdatePatientResponse = { data = data; meta = None }

///Identifier to match patient
type IdentifierFromUpsertPatientRequestMetaQuery =
    { ///Name of system
      system: Option<string>
      ///Value in system
      value: Option<string> }
    ///Creates an instance of IdentifierFromUpsertPatientRequestMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): IdentifierFromUpsertPatientRequestMetaQuery = { system = None; value = None }

type UpsertPatientRequestMetaQuery =
    { ///Group to create/update patient in.
      groups: list<string>
      ///Identifier to match patient
      identifier: IdentifierFromUpsertPatientRequestMetaQuery }
    ///Creates an instance of UpsertPatientRequestMetaQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (groups: list<string>, identifier: IdentifierFromUpsertPatientRequestMetaQuery): UpsertPatientRequestMetaQuery =
        { groups = groups
          identifier = identifier }

type UpsertPatientRequestMeta =
    { query: UpsertPatientRequestMetaQuery }
    ///Creates an instance of UpsertPatientRequestMeta with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (query: UpsertPatientRequestMetaQuery): UpsertPatientRequestMeta = { query = query }

type UpsertPatientRequest =
    { data: PatientResource
      meta: UpsertPatientRequestMeta }
    ///Creates an instance of UpsertPatientRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (data: PatientResource, meta: UpsertPatientRequestMeta): UpsertPatientRequest =
        { data = data; meta = meta }

[<RequireQualifiedAccess>]
type CreateAction =
    ///OK
    | Created of payload: CreateActionResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchAction =
    ///OK
    | OK of payload: FetchActionResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type UpdateAction =
    ///OK
    | OK of payload: UpdateActionResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type CreateBundle =
    ///OK
    | OK of payload: CreateBundleResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchBundle =
    ///OK
    | OK of payload: FetchBundleResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type UpdateBundle =
    ///OK
    | OK of payload: UpdateBundleResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchCalendarEvents =
    ///OK
    | OK of payload: FetchCalendarEventsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateCalendarEvent =
    ///OK
    | Created of payload: CreateCalendarEventResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type DeleteCalendarEvent =
    ///OK
    | OK
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchCalendarEvent =
    ///OK
    | OK of payload: FetchCalendarEventResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type UpdateCalendarEvent =
    ///OK
    | OK of payload: UpdateCalendarEventResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type PostCreateCalendarEventResponse =
    ///OK
    | Created of payload: CreateCalendarEventResponseRequest
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchCoaches =
    ///OK
    | OK of payload: FetchCoachesResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchCoach =
    ///OK
    | OK of payload: FetchCoachResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchEmailHistories =
    ///OK
    | OK of payload: FetchEmailHistoriesResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchEmailHistory =
    ///OK
    | OK of payload: FetchEmailHistoryResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchGroups =
    ///OK
    | OK of payload: FetchGroupsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateGroup =
    ///Created
    | Created of payload: CreateGroupResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchGroup =
    ///OK
    | OK of payload: FetchGroupResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthProfiles =
    ///OK
    | OK of payload: FetchHealthProfilesResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthProfile =
    ///OK
    | OK of payload: FetchHealthProfileResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthProfileAnswers =
    ///OK
    | OK of payload: FetchHealthProfileAnswersResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthProfileAnswer =
    ///OK
    | OK of payload: FetchHealthProfileAnswerResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthProfileQuestions =
    ///OK
    | OK of payload: FetchHealthProfileQuestionsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthProfileQuestion =
    ///OK
    | OK of payload: FetchHealthProfileQuestionResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthQuestionDefinitions =
    ///OK
    | OK of payload: FetchHealthQuestionDefinitionsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchHealthQuestionDefinition =
    ///OK
    | OK of payload: FetchHealthQuestionDefinitionResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateToken =
    ///Created
    | Created of payload: CreateTokenResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchTokenGroups =
    ///OK
    | OK of payload: FetchGroupsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchTokenOrganization =
    ///OK
    | OK of payload: FetchOrganizationResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchOrganization =
    ///OK
    | OK of payload: FetchOrganizationResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchPatients =
    ///OK
    | OK of payload: FetchPatientsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreatePatient =
    ///Created
    | Created of payload: CreatePatientResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type UpsertPatient =
    ///OK
    | OK of payload: CreatePatientResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchPatient =
    ///OK
    | OK of payload: FetchPatientResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type UpdatePatient =
    ///OK
    | OK of payload: UpdatePatientResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientCoaches =
    ///OK
    | OK of payload: FetchCoachesResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientGroups =
    ///OK
    | OK of payload: FetchGroupsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientHealthMetrics =
    ///OK
    | OK of payload: FetchPatientHealthMetricResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreatePatientHealthMetric =
    ///OK
    | OK of payload: CreatePatientHealthMetricResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientHealthMetric =
    ///OK
    | OK of payload: FetchPatientHealthMetricResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientPlanSummaries =
    ///OK
    | OK of payload: FetchPatientPlanSummariesResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientPlanSummary =
    ///OK
    | OK of payload: FetchPatientPlanSummaryResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type UpdatePatientPlanSummary =
    ///OK
    | OK of payload: UpdatePatientPlanSummaryResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientHealthResults =
    ///OK
    | OK of payload: FetchPatientHealthResultResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
    ///Invalid Request
    | Conflict of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchPatientHealthResult =
    ///OK
    | OK of payload: FetchPatientHealthResultResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchRewards =
    ///OK
    | OK of payload: FetchRewardsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateReward =
    ///OK
    | OK of payload: CreateRewardResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchReward =
    ///OK
    | OK of payload: FetchRewardResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardEarnings =
    ///OK
    | OK of payload: FetchRewardEarningsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateRewardEarning =
    ///OK
    | OK of payload: CreateRewardEarningResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardEarning =
    ///OK
    | OK of payload: FetchRewardEarningResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardEarningFulfillments =
    ///OK
    | OK of payload: FetchRewardEarningFulfillmentsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateRewardEarningFulfillment =
    ///OK
    | OK of payload: CreateRewardEarningFulfillmentResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardEarningFulfillment =
    ///OK
    | OK of payload: FetchRewardEarningFulfillmentResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardPrograms =
    ///OK
    | OK of payload: FetchRewardProgramsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateRewardProgram =
    ///OK
    | OK of payload: CreateRewardProgramResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardProgram =
    ///OK
    | OK of payload: FetchRewardProgramResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardProgramGroup =
    ///OK
    | OK of payload: FetchGroupsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardProgramActivations =
    ///OK
    | OK of payload: FetchRewardProgramActivationsResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse

[<RequireQualifiedAccess>]
type CreateRewardProgramActivation =
    ///OK
    | OK of payload: CreateRewardProgramActivationResponse
    ///Unauthorized
    | Unauthorized of payload: CreateOrUpdateErrorResponse
    ///Forbidden
    | Forbidden of payload: CreateOrUpdateErrorResponse
    ///Invalid Request
    | Conflict of payload: CreateOrUpdateErrorResponse

[<RequireQualifiedAccess>]
type FetchRewardProgramActivation =
    ///OK
    | OK of payload: FetchRewardProgramActivationResponse
    ///Unauthorized
    | Unauthorized of payload: FetchErrorResponse
    ///Forbidden
    | Forbidden of payload: FetchErrorResponse
