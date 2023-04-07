namespace rec Podiosuite.Types

type FilterCustom =
    { accountId: Option<string>
      name: Option<string>
      iccids: Option<list<string>>
      accountIds: Option<list<string>>
      locations: Option<list<string>>
      productIds: Option<list<string>>
      dateFrom: Option<string>
      dateTo: Option<string> }
    ///Creates an instance of FilterCustom with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FilterCustom =
        { accountId = None
          name = None
          iccids = None
          accountIds = None
          locations = None
          productIds = None
          dateFrom = None
          dateTo = None }

type Iccids =
    { iccid: Option<string>
      account: Option<string>
      product: Option<string>
      totalBytes: Option<float>
      totalSessions: Option<float> }
    ///Creates an instance of Iccids with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Iccids =
        { iccid = None
          account = None
          product = None
          totalBytes = None
          totalSessions = None }

type Accounts =
    { account: Option<string>
      totalBytes: Option<float>
      totalSessions: Option<float> }
    ///Creates an instance of Accounts with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Accounts =
        { account = None
          totalBytes = None
          totalSessions = None }

type Locations =
    { location: Option<string>
      totalBytes: Option<float>
      totalSessions: Option<float> }
    ///Creates an instance of Locations with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Locations =
        { location = None
          totalBytes = None
          totalSessions = None }

type Dates =
    { date: Option<string>
      totalBytes: Option<float>
      totalSessions: Option<float> }
    ///Creates an instance of Dates with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Dates =
        { date = None
          totalBytes = None
          totalSessions = None }

type Products =
    { product: Option<string>
      totalBytes: Option<float>
      totalSessions: Option<float> }
    ///Creates an instance of Products with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Products =
        { product = None
          totalBytes = None
          totalSessions = None }

type CustomData =
    { iccids: Option<list<Iccids>>
      accounts: Option<list<Accounts>>
      locations: Option<list<Locations>>
      dates: Option<list<Dates>>
      products: Option<list<Products>> }
    ///Creates an instance of CustomData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CustomData =
        { iccids = None
          accounts = None
          locations = None
          dates = None
          products = None }

type FilterCustomReturned =
    { iccids: Option<list<string>>
      accountIds: Option<list<string>>
      locations: Option<list<string>>
      products: Option<list<string>>
      dateFrom: Option<string>
      dateTo: Option<string> }
    ///Creates an instance of FilterCustomReturned with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): FilterCustomReturned =
        { iccids = None
          accountIds = None
          locations = None
          products = None
          dateFrom = None
          dateTo = None }

type Total =
    { totalBytes: Option<float>
      totalSims: Option<float>
      totalAccounts: Option<float>
      totalLocations: Option<float>
      totalDays: Option<float>
      totalProducts: Option<float> }
    ///Creates an instance of Total with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Total =
        { totalBytes = None
          totalSims = None
          totalAccounts = None
          totalLocations = None
          totalDays = None
          totalProducts = None }

type CustomReportNodata =
    { name: Option<string>
      accountId: Option<string>
      filter: Option<FilterCustomReturned>
      id: Option<string>
      status: Option<string>
      createdAt: Option<string>
      total: Option<Total> }
    ///Creates an instance of CustomReportNodata with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CustomReportNodata =
        { name = None
          accountId = None
          filter = None
          id = None
          status = None
          createdAt = None
          total = None }

type CustomReportDataTotal =
    { totalBytes: Option<float>
      totalSims: Option<float>
      totalAccounts: Option<float>
      totalLocations: Option<float>
      totalDays: Option<float>
      totalProducts: Option<float> }
    ///Creates an instance of CustomReportDataTotal with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CustomReportDataTotal =
        { totalBytes = None
          totalSims = None
          totalAccounts = None
          totalLocations = None
          totalDays = None
          totalProducts = None }

type CustomReportData =
    { name: Option<string>
      accountId: Option<string>
      filter: Option<FilterCustomReturned>
      id: Option<string>
      status: Option<string>
      createdAt: Option<string>
      data: Option<CustomData>
      total: Option<CustomReportDataTotal> }
    ///Creates an instance of CustomReportData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): CustomReportData =
        { name = None
          accountId = None
          filter = None
          id = None
          status = None
          createdAt = None
          data = None
          total = None }

type Alert =
    { alarmLink: Option<string>
      archived: Option<bool>
      description: Option<string>
      destinationEthernet: Option<string>
      destinationIP: Option<string>
      destinationPort: Option<string>
      iccid: Option<string>
      id: Option<float>
      protocol: Option<string>
      sourceEthernet: Option<string>
      sourceIP: Option<string>
      sourcePort: Option<string>
      time: Option<System.DateTimeOffset>
      timeStamp: Option<System.DateTimeOffset>
      ``type``: Option<string> }
    ///Creates an instance of Alert with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Alert =
        { alarmLink = None
          archived = None
          description = None
          destinationEthernet = None
          destinationIP = None
          destinationPort = None
          iccid = None
          id = None
          protocol = None
          sourceEthernet = None
          sourceIP = None
          sourcePort = None
          time = None
          timeStamp = None
          ``type`` = None }

type GraphsLine =
    { x: Option<string>
      y: Option<float>
      label: Option<string> }
    ///Creates an instance of GraphsLine with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): GraphsLine = { x = None; y = None; label = None }

type GraphsPie =
    { total: Option<string>
      element: Option<list<Newtonsoft.Json.Linq.JToken>> }
    ///Creates an instance of GraphsPie with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): GraphsPie = { total = None; element = None }

type GraphsTable =
    { id: Option<string>
      value: Option<float> }
    ///Creates an instance of GraphsTable with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): GraphsTable = { id = None; value = None }

type quickDial =
    { ///The memory location from "00" to "99"
      location: Option<string>
      ///Either another 'oa' to call another subscriber in the system, or a telephone number in full international format
      da: Option<string>
      ///Last change datetime in ISO format
      change: Option<string> }
    ///Creates an instance of quickDial with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): quickDial =
        { location = None
          da = None
          change = None }

type dialResponse =
    { response: Option<string>
      responseReason: Option<string> }
    ///Creates an instance of dialResponse with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): dialResponse =
        { response = None
          responseReason = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Currency =
    | [<CompiledName "USD">] USD
    | [<CompiledName "EUR">] EUR
    | [<CompiledName "GBP">] GBP
    | [<CompiledName "MXN">] MXN
    | [<CompiledName "HKD">] HKD
    | [<CompiledName "CNY">] CNY
    | [<CompiledName "MYR">] MYR
    | [<CompiledName "AUD">] AUD
    member this.Format() =
        match this with
        | USD -> "USD"
        | EUR -> "EUR"
        | GBP -> "GBP"
        | MXN -> "MXN"
        | HKD -> "HKD"
        | CNY -> "CNY"
        | MYR -> "MYR"
        | AUD -> "AUD"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Status =
    | [<CompiledName "active">] Active
    | [<CompiledName "inactive">] Inactive
    | [<CompiledName "suspended">] Suspended
    member this.Format() =
        match this with
        | Active -> "active"
        | Inactive -> "inactive"
        | Suspended -> "suspended"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Type =
    | [<CompiledName "customer">] Customer
    | [<CompiledName "reseller">] Reseller
    | [<CompiledName "test">] Test
    | [<CompiledName "master">] Master
    member this.Format() =
        match this with
        | Customer -> "customer"
        | Reseller -> "reseller"
        | Test -> "test"
        | Master -> "master"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type BalanceAndTypeType =
    | [<CompiledName "prepay">] Prepay
    | [<CompiledName "postpay">] Postpay
    member this.Format() =
        match this with
        | Prepay -> "prepay"
        | Postpay -> "postpay"

type BalanceAndType =
    { balance: Option<string>
      ``type``: Option<BalanceAndTypeType> }
    ///Creates an instance of BalanceAndType with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BalanceAndType = { balance = None; ``type`` = None }

type Branding =
    { logoLarge: Option<string>
      logoSmall: Option<string>
      logoLogin: Option<string>
      loginPageMessage: Option<string>
      theme: Option<string>
      supportEmail: Option<string>
      notificationSenderEmail: Option<string>
      phoneSupport: Option<string>
      supportPageMessage: Option<string>
      domainURL: Option<string>
      emailDomain: Option<string>
      showBranding: Option<bool>
      pageTitle: Option<string> }
    ///Creates an instance of Branding with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Branding =
        { logoLarge = None
          logoSmall = None
          logoLogin = None
          loginPageMessage = None
          theme = None
          supportEmail = None
          notificationSenderEmail = None
          phoneSupport = None
          supportPageMessage = None
          domainURL = None
          emailDomain = None
          showBranding = None
          pageTitle = None }

type Operations =
    { dfSync: Option<bool>
      parentSync: Option<bool>
      userSync: Option<bool> }
    ///Creates an instance of Operations with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Operations =
        { dfSync = None
          parentSync = None
          userSync = None }

type Tags =
    { key: Option<string>
      label: Option<string>
      slug: Option<string> }
    ///Creates an instance of Tags with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Tags =
        { key = None
          label = None
          slug = None }

type Account =
    { _id: Option<string>
      id: Option<string>
      name: Option<string>
      legalName: Option<string>
      defaultEmail: Option<string>
      defaultSmsOrigin: Option<string>
      currency: Option<Currency>
      resellerId: Option<string>
      bundles: Option<list<string>>
      userIds: Option<list<string>>
      childIds: Option<list<string>>
      signUpDate: Option<string>
      status: Option<Status>
      timezone: Option<string>
      ``type``: Option<Type>
      billing: Option<BalanceAndType>
      setup: Option<string>
      dashboardWidgets: Option<Newtonsoft.Json.Linq.JObject>
      branding: Option<Branding>
      theme: Option<string>
      operations: Option<Operations>
      allowSupervision: Option<bool>
      lastAccess: Option<string>
      addresses: Option<list<AccountAddresses>>
      tags: Option<list<Tags>>
      securitySettings: Option<list<AccountSecuritySetting>> }
    ///Creates an instance of Account with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Account =
        { _id = None
          id = None
          name = None
          legalName = None
          defaultEmail = None
          defaultSmsOrigin = None
          currency = None
          resellerId = None
          bundles = None
          userIds = None
          childIds = None
          signUpDate = None
          status = None
          timezone = None
          ``type`` = None
          billing = None
          setup = None
          dashboardWidgets = None
          branding = None
          theme = None
          operations = None
          allowSupervision = None
          lastAccess = None
          addresses = None
          tags = None
          securitySettings = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type AccountSecuritySettingType =
    | [<CompiledName "vpn_client_access">] Vpn_client_access
    | [<CompiledName "vpn_ipsec">] Vpn_ipsec
    | [<CompiledName "vpn_acl">] Vpn_acl
    | [<CompiledName "destined_ip_range">] Destined_ip_range
    member this.Format() =
        match this with
        | Vpn_client_access -> "vpn_client_access"
        | Vpn_ipsec -> "vpn_ipsec"
        | Vpn_acl -> "vpn_acl"
        | Destined_ip_range -> "destined_ip_range"

type AccountSecuritySetting =
    { poolId: Option<string>
      ///Security Service Name
      name: Option<string>
      ///Security Service Type.
      ``type``: Option<AccountSecuritySettingType>
      ///Network type
      carrier: Option<string>
      ranges: Option<list<string>> }
    ///Creates an instance of AccountSecuritySetting with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): AccountSecuritySetting =
        { poolId = None
          name = None
          ``type`` = None
          carrier = None
          ranges = None }

type AccountAddresses =
    { fullName: Option<string>
      addressLine1: Option<string>
      addressLine2: Option<string>
      addressCity: Option<string>
      addressStateOrRegion: Option<string>
      addressPostalCode: Option<string>
      addressPhoneNumber: Option<string>
      addressCountry: Option<string>
      taxIdNumber: Option<string>
      ``default``: Option<string>
      billingDisclaimer: Option<string> }
    ///Creates an instance of AccountAddresses with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): AccountAddresses =
        { fullName = None
          addressLine1 = None
          addressLine2 = None
          addressCity = None
          addressStateOrRegion = None
          addressPostalCode = None
          addressPhoneNumber = None
          addressCountry = None
          taxIdNumber = None
          ``default`` = None
          billingDisclaimer = None }

type Permissions =
    { accountId: Option<string>
      roles: Option<list<string>> }
    ///Creates an instance of Permissions with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Permissions = { accountId = None; roles = None }

type Favorites =
    { disims: Option<list<string>>
      summaries: Option<list<string>>
      billing: Option<list<string>>
      users: Option<list<string>>
      products: Option<list<string>>
      accounts: Option<list<string>>
      assets: Option<list<string>> }
    ///Creates an instance of Favorites with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Favorites =
        { disims = None
          summaries = None
          billing = None
          users = None
          products = None
          accounts = None
          assets = None }

type Profile =
    { picture: Option<string>
      language: Option<string>
      timezone: Option<string> }
    ///Creates an instance of Profile with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Profile =
        { picture = None
          language = None
          timezone = None }

type User =
    { _id: Option<string>
      username: Option<string>
      email: Option<string>
      lastAccess: Option<string>
      status: Option<string>
      permissions: Option<list<Permissions>>
      favorites: Option<Favorites>
      profile: Option<Profile> }
    ///Creates an instance of User with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): User =
        { _id = None
          username = None
          email = None
          lastAccess = None
          status = None
          permissions = None
          favorites = None
          profile = None }

type ServingNetwork =
    { mcc: Option<string>
      mnc: Option<string> }
    ///Creates an instance of ServingNetwork with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ServingNetwork = { mcc = None; mnc = None }

type LastCall =
    { servingNetwork: Option<ServingNetwork>
      startTime: Option<string>
      endTime: Option<string>
      ipAddress: Option<string>
      imei: Option<string>
      bytes: Option<float>
      roundedBytes: Option<float> }
    ///Creates an instance of LastCall with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): LastCall =
        { servingNetwork = None
          startTime = None
          endTime = None
          ipAddress = None
          imei = None
          bytes = None
          roundedBytes = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type AssetSimcardStatus =
    | [<CompiledName "active">] Active
    | [<CompiledName "inactive">] Inactive
    | [<CompiledName "suspended">] Suspended
    | [<CompiledName "preactive">] Preactive
    member this.Format() =
        match this with
        | Active -> "active"
        | Inactive -> "inactive"
        | Suspended -> "suspended"
        | Preactive -> "preactive"

type Carriers =
    { UKJ: Option<bool>
      UKAT: Option<bool>
      UKRO: Option<bool>
      UKTM: Option<bool>
      UKA: Option<bool>
      UKK: Option<bool>
      UKMX: Option<bool>
      UKJBB: Option<bool>
      UKP: Option<bool>
      UKX: Option<bool>
      ROCT: Option<bool>
      NAQM: Option<bool>
      NAVZ: Option<bool>
      ROPD: Option<bool>
      NAMP: Option<bool>
      EXPTMOB: Option<bool>
      EXPBICS: Option<bool>
      ROMK: Option<bool>
      ROMT: Option<bool>
      ROTL: Option<bool>
      ROTN: Option<bool>
      NAMR: Option<bool>
      NATM: Option<bool>
      NAMB: Option<bool>
      NAC3: Option<bool>
      ROAS: Option<bool>
      ROWT: Option<bool>
      NAEX: Option<bool>
      ROPT: Option<bool>
      ROPU: Option<bool>
      ROPI: Option<bool>
      ROPE: Option<bool>
      TEST: Option<bool> }
    ///Creates an instance of Carriers with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Carriers =
        { UKJ = None
          UKAT = None
          UKRO = None
          UKTM = None
          UKA = None
          UKK = None
          UKMX = None
          UKJBB = None
          UKP = None
          UKX = None
          ROCT = None
          NAQM = None
          NAVZ = None
          ROPD = None
          NAMP = None
          EXPTMOB = None
          EXPBICS = None
          ROMK = None
          ROMT = None
          ROTL = None
          ROTN = None
          NAMR = None
          NATM = None
          NAMB = None
          NAC3 = None
          ROAS = None
          ROWT = None
          NAEX = None
          ROPT = None
          ROPU = None
          ROPI = None
          ROPE = None
          TEST = None }

type Subscriptions =
    { bundles: Option<list<string>>
      id: Option<string>
      accountId: Option<string>
      limit: Option<float>
      smsLimit: Option<float> }
    ///Creates an instance of Subscriptions with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Subscriptions =
        { bundles = None
          id = None
          accountId = None
          limit = None
          smsLimit = None }

type Alerts =
    { notification: Option<string>
      ``type``: Option<string>
      limit: Option<float>
      enabled: Option<bool> }
    ///Creates an instance of Alerts with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Alerts =
        { notification = None
          ``type`` = None
          limit = None
          enabled = None }

type SmsAlerts =
    { notification: Option<string>
      ``type``: Option<string>
      limit: Option<float>
      enabled: Option<bool> }
    ///Creates an instance of SmsAlerts with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): SmsAlerts =
        { notification = None
          ``type`` = None
          limit = None
          enabled = None }

type SetupsTags = Map<string, Newtonsoft.Json.Linq.JToken>

type Setups =
    { accountId: Option<string>
      assetName: Option<string>
      alerts: Option<list<Alerts>>
      smsAlerts: Option<list<SmsAlerts>>
      tags: Option<list<SetupsTags>> }
    ///Creates an instance of Setups with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Setups =
        { accountId = None
          assetName = None
          alerts = None
          smsAlerts = None
          tags = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProfileState =
    | [<CompiledName "onstock">] Onstock
    | [<CompiledName "disabled">] Disabled
    | [<CompiledName "enabled">] Enabled
    | [<CompiledName "created">] Created
    member this.Format() =
        match this with
        | Onstock -> "onstock"
        | Disabled -> "disabled"
        | Enabled -> "enabled"
        | Created -> "created"

type LastSMSServingNetwork =
    { mcc: Option<string>
      mnc: Option<string> }
    ///Creates an instance of LastSMSServingNetwork with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): LastSMSServingNetwork = { mcc = None; mnc = None }

type LastSMS =
    { ``type``: Option<string>
      endTime: Option<string>
      originatingAddress: Option<string>
      destinationAddress: Option<string>
      servingNetwork: Option<LastSMSServingNetwork> }
    ///Creates an instance of LastSMS with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): LastSMS =
        { ``type`` = None
          endTime = None
          originatingAddress = None
          destinationAddress = None
          servingNetwork = None }

type AssetSimcard =
    { lastCall: Option<LastCall>
      status: Option<AssetSimcardStatus>
      ownership: Option<list<string>>
      id: Option<string>
      ownerAccountId: Option<string>
      ownerAccountName: Option<string>
      iccid: string
      fixedIPs: Option<list<string>>
      carriers: Option<Carriers>
      limit: Option<float>
      subscriptions: Option<Subscriptions>
      setups: Option<list<Setups>>
      msisdn: Option<string>
      ``type``: Option<string>
      model: Option<string>
      profileState: Option<ProfileState>
      bootstrapEid: Option<string>
      activationDate: Option<string>
      reactivationDate: Option<string>
      subscriptionDate: Option<string>
      suspensionDate: Option<string>
      smsLimit: Option<float>
      lastSMS: Option<LastSMS> }
    ///Creates an instance of AssetSimcard with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (iccid: string): AssetSimcard =
        { lastCall = None
          status = None
          ownership = None
          id = None
          ownerAccountId = None
          ownerAccountName = None
          iccid = iccid
          fixedIPs = None
          carriers = None
          limit = None
          subscriptions = None
          setups = None
          msisdn = None
          ``type`` = None
          model = None
          profileState = None
          bootstrapEid = None
          activationDate = None
          reactivationDate = None
          subscriptionDate = None
          suspensionDate = None
          smsLimit = None
          lastSMS = None }

type Bundle =
    { promotionName: Option<string>
      cost: Option<float>
      bytes: Option<float>
      overuse: Option<float>
      currency: Option<string> }
    ///Creates an instance of Bundle with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Bundle =
        { promotionName = None
          cost = None
          bytes = None
          overuse = None
          currency = None }

type BillingFromBilling =
    { accountId: Option<string>
      name: Option<string>
      bundle: Option<Bundle>
      numberOfSims: Option<float>
      numberOfSimsActive: Option<float>
      lineRental: Option<float>
      prorataLineRentalLastCycle: Option<float>
      totalBytesIncluded: Option<float>
      bytesConsumed: Option<float>
      overuseBytes: Option<float>
      overuseCost: Option<float>
      totalCost: Option<float> }
    ///Creates an instance of BillingFromBilling with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BillingFromBilling =
        { accountId = None
          name = None
          bundle = None
          numberOfSims = None
          numberOfSimsActive = None
          lineRental = None
          prorataLineRentalLastCycle = None
          totalBytesIncluded = None
          bytesConsumed = None
          overuseBytes = None
          overuseCost = None
          totalCost = None }

type Billing =
    { _id: Option<string>
      name: Option<string>
      accountId: Option<string>
      dateFrom: Option<string>
      dateTo: Option<string>
      Currency: Option<string>
      status: Option<string>
      billing: Option<list<BillingFromBilling>> }
    ///Creates an instance of Billing with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Billing =
        { _id = None
          name = None
          accountId = None
          dateFrom = None
          dateTo = None
          Currency = None
          status = None
          billing = None }

type Events =
    { userId: Option<string>
      accountId: Option<string>
      iccid: Option<string>
      eid: Option<string>
      action: Option<string>
      result: Option<string>
      raw: Option<Newtonsoft.Json.Linq.JObject>
      created: Option<System.DateTimeOffset>
      modified: Option<System.DateTimeOffset> }
    ///Creates an instance of Events with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Events =
        { userId = None
          accountId = None
          iccid = None
          eid = None
          action = None
          result = None
          raw = None
          created = None
          modified = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProfilesStatus =
    | [<CompiledName "active">] Active
    | [<CompiledName "inactive">] Inactive
    | [<CompiledName "suspended">] Suspended
    | [<CompiledName "preactive">] Preactive
    member this.Format() =
        match this with
        | Active -> "active"
        | Inactive -> "inactive"
        | Suspended -> "suspended"
        | Preactive -> "preactive"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProfilesProfileState =
    | [<CompiledName "onstock">] Onstock
    | [<CompiledName "disabled">] Disabled
    | [<CompiledName "enabled">] Enabled
    | [<CompiledName "created">] Created
    member this.Format() =
        match this with
        | Onstock -> "onstock"
        | Disabled -> "disabled"
        | Enabled -> "enabled"
        | Created -> "created"

type Profiles =
    { iccid: Option<string>
      status: Option<ProfilesStatus>
      profileState: Option<ProfilesProfileState>
      enabled: Option<bool>
      bootstrap: Option<bool> }
    ///Creates an instance of Profiles with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Profiles =
        { iccid = None
          status = None
          profileState = None
          enabled = None
          bootstrap = None }

type eSIMSetups =
    { accountId: Option<string>
      eSimName: Option<string>
      eSimGroupName: Option<string> }
    ///Creates an instance of eSIMSetups with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): eSIMSetups =
        { accountId = None
          eSimName = None
          eSimGroupName = None }

type eSIM =
    { ///Unique identifier of the eSIM
      eid: string
      ownership: Option<list<string>>
      ownerAccountId: Option<string>
      ownerAccountName: Option<string>
      ///Indicates the Unix timestamp when current operation expires. 0 for no current operation
      callbackStatus: Option<float>
      ///Significants properties of installed profiles
      profiles: Option<list<Profiles>>
      setups: Option<list<eSIMSetups>>
      enabledProfile: Option<AssetSimcard> }
    ///Creates an instance of eSIM with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (eid: string): eSIM =
        { eid = eid
          ownership = None
          ownerAccountId = None
          ownerAccountName = None
          callbackStatus = None
          profiles = None
          setups = None
          enabledProfile = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ImsisType =
    | [<CompiledName "singleimsis">] Singleimsis
    | [<CompiledName "multiimsis">] Multiimsis
    member this.Format() =
        match this with
        | Singleimsis -> "singleimsis"
        | Multiimsis -> "multiimsis"

type Networks =
    { imsisType: Option<ImsisType>
      singleimsis: Option<string>
      multiimsis: Option<list<string>> }
    ///Creates an instance of Networks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Networks =
        { imsisType = None
          singleimsis = None
          multiimsis = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProductCurrency =
    | [<CompiledName "USD">] USD
    | [<CompiledName "EUR">] EUR
    | [<CompiledName "GBP">] GBP
    | [<CompiledName "MXN">] MXN
    | [<CompiledName "HKD">] HKD
    | [<CompiledName "CNY">] CNY
    | [<CompiledName "MYR">] MYR
    | [<CompiledName "AUD">] AUD
    member this.Format() =
        match this with
        | USD -> "USD"
        | EUR -> "EUR"
        | GBP -> "GBP"
        | MXN -> "MXN"
        | HKD -> "HKD"
        | CNY -> "CNY"
        | MYR -> "MYR"
        | AUD -> "AUD"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProductType =
    | [<CompiledName "PerMb">] PerMb
    | [<CompiledName "Aggregate">] Aggregate
    | [<CompiledName "Single">] Single
    | [<CompiledName "SharedDataPool">] SharedDataPool
    member this.Format() =
        match this with
        | PerMb -> "PerMb"
        | Aggregate -> "Aggregate"
        | Single -> "Single"
        | SharedDataPool -> "SharedDataPool"

type PerMb =
    { lineRental: Option<bool>
      lineRentalCost: Option<float>
      perMbCost: Option<float>
      equallyZone: Option<bool> }
    ///Creates an instance of PerMb with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PerMb =
        { lineRental = None
          lineRentalCost = None
          perMbCost = None
          equallyZone = None }

type Aggregate =
    { initialSize: Option<float>
      lineRentalCost: Option<float>
      perMbCost: Option<float> }
    ///Creates an instance of Aggregate with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Aggregate =
        { initialSize = None
          lineRentalCost = None
          perMbCost = None }

type Single =
    { initialSize: Option<float>
      lineRentalCost: Option<float>
      perMbCost: Option<float> }
    ///Creates an instance of Single with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Single =
        { initialSize = None
          lineRentalCost = None
          perMbCost = None }

type SharedDataPool =
    { initialSize: Option<float>
      lineRentalCost: Option<float>
      perMbCost: Option<float>
      sharedDataPoolCost: Option<float> }
    ///Creates an instance of SharedDataPool with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): SharedDataPool =
        { initialSize = None
          lineRentalCost = None
          perMbCost = None
          sharedDataPoolCost = None }

type DataPool =
    { initialSize: Option<float>
      lineRentalCost: Option<float> }
    ///Creates an instance of DataPool with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): DataPool =
        { initialSize = None
          lineRentalCost = None }

type ProductTypeScheme =
    { PerMb: Option<PerMb>
      Aggregate: Option<Aggregate>
      Single: Option<Single>
      SharedDataPool: Option<SharedDataPool>
      DataPool: Option<DataPool> }
    ///Creates an instance of ProductTypeScheme with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductTypeScheme =
        { PerMb = None
          Aggregate = None
          Single = None
          SharedDataPool = None
          DataPool = None }

type Sms =
    { active: Option<bool>
      bundleIncludeQuantity: Option<float>
      bundlePriceOveruse: Option<float> }
    ///Creates an instance of Sms with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Sms =
        { active = None
          bundleIncludeQuantity = None
          bundlePriceOveruse = None }

type Voice =
    { active: Option<bool>
      bundleIncludeQuantity: Option<float>
      bundlePriceOveruse: Option<float> }
    ///Creates an instance of Voice with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Voice =
        { active = None
          bundleIncludeQuantity = None
          bundlePriceOveruse = None }

type Services =
    { sms: Option<Sms>
      voice: Option<Voice> }
    ///Creates an instance of Services with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Services = { sms = None; voice = None }

type Preactivation =
    { active: Option<bool>
      data: Option<float>
      sms: Option<float>
      voice: Option<float> }
    ///Creates an instance of Preactivation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Preactivation =
        { active = None
          data = None
          sms = None
          voice = None }

type ProductCarriers =
    { UKJ: Option<bool>
      UKAT: Option<bool>
      UKRO: Option<bool>
      UKTM: Option<bool>
      UKA: Option<bool>
      UKK: Option<bool>
      UKMX: Option<bool>
      UKJBB: Option<bool>
      UKP: Option<bool>
      UKX: Option<bool>
      ROCT: Option<bool>
      NAQM: Option<bool>
      NAVZ: Option<bool>
      ROPD: Option<bool>
      NAMP: Option<bool>
      EXPTMOB: Option<bool>
      EXPBICS: Option<bool>
      ROMK: Option<bool>
      ROMT: Option<bool>
      ROTL: Option<bool>
      ROTN: Option<bool>
      NAMR: Option<bool>
      NATM: Option<bool>
      NAMB: Option<bool>
      NAC3: Option<bool>
      NAEX: Option<bool>
      ROPT: Option<bool>
      ROPU: Option<bool>
      ROPI: Option<bool>
      ROPE: Option<bool>
      TEST: Option<bool> }
    ///Creates an instance of ProductCarriers with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductCarriers =
        { UKJ = None
          UKAT = None
          UKRO = None
          UKTM = None
          UKA = None
          UKK = None
          UKMX = None
          UKJBB = None
          UKP = None
          UKX = None
          ROCT = None
          NAQM = None
          NAVZ = None
          ROPD = None
          NAMP = None
          EXPTMOB = None
          EXPBICS = None
          ROMK = None
          ROMT = None
          ROTL = None
          ROTN = None
          NAMR = None
          NATM = None
          NAMB = None
          NAC3 = None
          NAEX = None
          ROPT = None
          ROPU = None
          ROPI = None
          ROPE = None
          TEST = None }

type ProductZoneSetup =
    { schemeId: Option<string>
      zones: Option<list<string>> }
    ///Creates an instance of ProductZoneSetup with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductZoneSetup = { schemeId = None; zones = None }

type Product =
    { name: string
      accountId: string
      accountIds: Option<list<string>>
      resellerProductId: Option<string>
      networks: Option<Networks>
      currency: Option<ProductCurrency>
      cycle: Option<float>
      cycleUnits: Option<string>
      renewOnExpiry: Option<bool>
      renewOnDepletion: Option<bool>
      contractLength: Option<float>
      allowOveruse: Option<bool>
      productType: Option<ProductType>
      productTypeScheme: Option<ProductTypeScheme>
      services: Option<Services>
      preactivation: Option<Preactivation>
      carriers: Option<ProductCarriers>
      bundleId: Option<string>
      productZoneSetup: Option<ProductZoneSetup> }
    ///Creates an instance of Product with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (name: string, accountId: string): Product =
        { name = name
          accountId = accountId
          accountIds = None
          resellerProductId = None
          networks = None
          currency = None
          cycle = None
          cycleUnits = None
          renewOnExpiry = None
          renewOnDepletion = None
          contractLength = None
          allowOveruse = None
          productType = None
          productTypeScheme = None
          services = None
          preactivation = None
          carriers = None
          bundleId = None
          productZoneSetup = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProductNoBundleIdNetworksImsisType =
    | [<CompiledName "singleimsis">] Singleimsis
    | [<CompiledName "multiimsis">] Multiimsis
    member this.Format() =
        match this with
        | Singleimsis -> "singleimsis"
        | Multiimsis -> "multiimsis"

type ProductNoBundleIdNetworks =
    { imsisType: Option<ProductNoBundleIdNetworksImsisType>
      singleimsis: Option<string>
      multiimsis: Option<list<string>> }
    ///Creates an instance of ProductNoBundleIdNetworks with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdNetworks =
        { imsisType = None
          singleimsis = None
          multiimsis = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProductNoBundleIdCurrency =
    | [<CompiledName "USD">] USD
    | [<CompiledName "EUR">] EUR
    | [<CompiledName "GBP">] GBP
    | [<CompiledName "MXN">] MXN
    | [<CompiledName "HKD">] HKD
    | [<CompiledName "CNY">] CNY
    | [<CompiledName "MYR">] MYR
    | [<CompiledName "AUD">] AUD
    member this.Format() =
        match this with
        | USD -> "USD"
        | EUR -> "EUR"
        | GBP -> "GBP"
        | MXN -> "MXN"
        | HKD -> "HKD"
        | CNY -> "CNY"
        | MYR -> "MYR"
        | AUD -> "AUD"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ProductNoBundleIdProductType =
    | [<CompiledName "PerMb">] PerMb
    | [<CompiledName "Aggregate">] Aggregate
    | [<CompiledName "Single">] Single
    | [<CompiledName "SharedDataPool">] SharedDataPool
    member this.Format() =
        match this with
        | PerMb -> "PerMb"
        | Aggregate -> "Aggregate"
        | Single -> "Single"
        | SharedDataPool -> "SharedDataPool"

type ProductNoBundleIdProductTypeSchemePerMb =
    { lineRental: Option<bool>
      lineRentalCost: Option<float>
      perMbCost: Option<float>
      equallyZone: Option<bool> }
    ///Creates an instance of ProductNoBundleIdProductTypeSchemePerMb with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductTypeSchemePerMb =
        { lineRental = None
          lineRentalCost = None
          perMbCost = None
          equallyZone = None }

type ProductNoBundleIdProductTypeSchemeAggregate =
    { initialSize: Option<float>
      lineRentalCost: Option<float>
      perMbCost: Option<float> }
    ///Creates an instance of ProductNoBundleIdProductTypeSchemeAggregate with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductTypeSchemeAggregate =
        { initialSize = None
          lineRentalCost = None
          perMbCost = None }

type ProductNoBundleIdProductTypeSchemeSingle =
    { initialSize: Option<float>
      lineRentalCost: Option<float>
      perMbCost: Option<float> }
    ///Creates an instance of ProductNoBundleIdProductTypeSchemeSingle with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductTypeSchemeSingle =
        { initialSize = None
          lineRentalCost = None
          perMbCost = None }

type ProductNoBundleIdProductTypeSchemeSharedDataPool =
    { initialSize: Option<float>
      lineRentalCost: Option<float>
      perMbCost: Option<float>
      sharedDataPoolCost: Option<float> }
    ///Creates an instance of ProductNoBundleIdProductTypeSchemeSharedDataPool with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductTypeSchemeSharedDataPool =
        { initialSize = None
          lineRentalCost = None
          perMbCost = None
          sharedDataPoolCost = None }

type ProductNoBundleIdProductTypeSchemeDataPool =
    { initialSize: Option<float>
      lineRentalCost: Option<float> }
    ///Creates an instance of ProductNoBundleIdProductTypeSchemeDataPool with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductTypeSchemeDataPool =
        { initialSize = None
          lineRentalCost = None }

type ProductNoBundleIdProductTypeScheme =
    { PerMb: Option<ProductNoBundleIdProductTypeSchemePerMb>
      Aggregate: Option<ProductNoBundleIdProductTypeSchemeAggregate>
      Single: Option<ProductNoBundleIdProductTypeSchemeSingle>
      SharedDataPool: Option<ProductNoBundleIdProductTypeSchemeSharedDataPool>
      DataPool: Option<ProductNoBundleIdProductTypeSchemeDataPool> }
    ///Creates an instance of ProductNoBundleIdProductTypeScheme with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductTypeScheme =
        { PerMb = None
          Aggregate = None
          Single = None
          SharedDataPool = None
          DataPool = None }

type ProductNoBundleIdServicesSms =
    { active: Option<bool>
      bundleIncludeQuantity: Option<float>
      bundlePriceOveruse: Option<float> }
    ///Creates an instance of ProductNoBundleIdServicesSms with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdServicesSms =
        { active = None
          bundleIncludeQuantity = None
          bundlePriceOveruse = None }

type ProductNoBundleIdServicesVoice =
    { active: Option<bool>
      bundleIncludeQuantity: Option<float>
      bundlePriceOveruse: Option<float> }
    ///Creates an instance of ProductNoBundleIdServicesVoice with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdServicesVoice =
        { active = None
          bundleIncludeQuantity = None
          bundlePriceOveruse = None }

type ProductNoBundleIdServices =
    { sms: Option<ProductNoBundleIdServicesSms>
      voice: Option<ProductNoBundleIdServicesVoice> }
    ///Creates an instance of ProductNoBundleIdServices with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdServices = { sms = None; voice = None }

type ProductNoBundleIdCarriers =
    { UKJ: Option<bool>
      UKAT: Option<bool>
      UKRO: Option<bool>
      UKTM: Option<bool>
      UKA: Option<bool>
      UKK: Option<bool>
      UKMX: Option<bool>
      UKJBB: Option<bool>
      UKP: Option<bool>
      UKX: Option<bool>
      ROCT: Option<bool>
      NAQM: Option<bool>
      NAVZ: Option<bool>
      ROPD: Option<bool>
      NAMP: Option<bool>
      EXPTMOB: Option<bool>
      EXPBICS: Option<bool>
      ROMK: Option<bool>
      ROMT: Option<bool>
      ROTL: Option<bool>
      ROTN: Option<bool>
      NAMR: Option<bool>
      NATM: Option<bool>
      NAMB: Option<bool>
      NAC3: Option<bool>
      NAEX: Option<bool>
      ROPT: Option<bool>
      ROPU: Option<bool>
      ROPI: Option<bool>
      ROPE: Option<bool>
      TEST: Option<bool> }
    ///Creates an instance of ProductNoBundleIdCarriers with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdCarriers =
        { UKJ = None
          UKAT = None
          UKRO = None
          UKTM = None
          UKA = None
          UKK = None
          UKMX = None
          UKJBB = None
          UKP = None
          UKX = None
          ROCT = None
          NAQM = None
          NAVZ = None
          ROPD = None
          NAMP = None
          EXPTMOB = None
          EXPBICS = None
          ROMK = None
          ROMT = None
          ROTL = None
          ROTN = None
          NAMR = None
          NATM = None
          NAMB = None
          NAC3 = None
          NAEX = None
          ROPT = None
          ROPU = None
          ROPI = None
          ROPE = None
          TEST = None }

type ProductNoBundleIdProductZoneSetup =
    { schemeId: Option<string>
      zones: Option<list<string>> }
    ///Creates an instance of ProductNoBundleIdProductZoneSetup with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): ProductNoBundleIdProductZoneSetup = { schemeId = None; zones = None }

type ProductNoBundleId =
    { name: string
      accountId: string
      accountIds: Option<list<string>>
      resellerProductId: Option<string>
      networks: Option<ProductNoBundleIdNetworks>
      currency: Option<ProductNoBundleIdCurrency>
      cycle: Option<float>
      cycleUnits: Option<string>
      renewOnExpiry: Option<bool>
      renewOnDepletion: Option<bool>
      contractLength: Option<float>
      allowOveruse: Option<bool>
      productType: Option<ProductNoBundleIdProductType>
      productTypeScheme: Option<ProductNoBundleIdProductTypeScheme>
      services: Option<ProductNoBundleIdServices>
      carriers: Option<ProductNoBundleIdCarriers>
      productZoneSetup: Option<ProductNoBundleIdProductZoneSetup> }
    ///Creates an instance of ProductNoBundleId with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (name: string, accountId: string): ProductNoBundleId =
        { name = name
          accountId = accountId
          accountIds = None
          resellerProductId = None
          networks = None
          currency = None
          cycle = None
          cycleUnits = None
          renewOnExpiry = None
          renewOnDepletion = None
          contractLength = None
          allowOveruse = None
          productType = None
          productTypeScheme = None
          services = None
          carriers = None
          productZoneSetup = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type StatusName =
    | [<CompiledName "Operational">] Operational
    | [<CompiledName "Performance Issues">] PerformanceIssues
    | [<CompiledName "Partial Outage">] PartialOutage
    | [<CompiledName "Major Outage">] MajorOutage
    member this.Format() =
        match this with
        | Operational -> "Operational"
        | PerformanceIssues -> "Performance Issues"
        | PartialOutage -> "Partial Outage"
        | MajorOutage -> "Major Outage"

type statusComponent =
    { id: Option<float>
      name: string
      description: Option<string>
      link: Option<string>
      statusName: Option<StatusName>
      tags: list<string>
      created: string
      modified: string }
    ///Creates an instance of statusComponent with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (name: string, tags: list<string>, created: string, modified: string): statusComponent =
        { id = None
          name = name
          description = None
          link = None
          statusName = None
          tags = tags
          created = created
          modified = modified }

type steeringList =
    { accountId: Option<string>
      name: Option<string>
      value: Option<string> }
    ///Creates an instance of steeringList with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): steeringList =
        { accountId = None
          name = None
          value = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type campaignItemStatus =
    | [<CompiledName "pending">] Pending
    | [<CompiledName "success">] Success
    | [<CompiledName "error">] Error
    member this.Format() =
        match this with
        | Pending -> "pending"
        | Success -> "success"
        | Error -> "error"

type Error =
    { code: Option<string>
      message: Option<string>
      detail: Option<string> }
    ///Creates an instance of Error with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Error =
        { code = None
          message = None
          detail = None }

type campaignItem =
    { ///Identifier of the subscriber
      iccid: Option<string>
      ///Identifier of the parent campaign
      campaignId: Option<string>
      status: Option<campaignItemStatus>
      attempts: Option<float>
      error: Option<Error> }
    ///Creates an instance of campaignItem with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): campaignItem =
        { iccid = None
          campaignId = None
          status = None
          attempts = None
          error = None }

type campaingDownloadProfileProfiles =
    { eid: Option<string>
      iccid: Option<string> }
    ///Creates an instance of campaingDownloadProfileProfiles with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): campaingDownloadProfileProfiles = { eid = None; iccid = None }

type campaingDownloadProfile =
    { enable: Option<bool>
      retry: Option<bool>
      expirationTime: Option<float>
      profiles: Option<list<campaingDownloadProfileProfiles>> }
    ///Creates an instance of campaingDownloadProfile with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): campaingDownloadProfile =
        { enable = None
          retry = None
          expirationTime = None
          profiles = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type DeleteMode =
    | [<CompiledName "enabled_profile">] Enabled_profile
    | [<CompiledName "last_disabled_profile">] Last_disabled_profile
    | [<CompiledName "all_disabled_profiles">] All_disabled_profiles
    member this.Format() =
        match this with
        | Enabled_profile -> "enabled_profile"
        | Last_disabled_profile -> "last_disabled_profile"
        | All_disabled_profiles -> "all_disabled_profiles"

type campaingDeleteProfile =
    { deleteMode: Option<DeleteMode> }
    ///Creates an instance of campaingDeleteProfile with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): campaingDeleteProfile = { deleteMode = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type campaignType =
    | [<CompiledName "steering">] Steering
    | [<CompiledName "download_profile">] Download_profile
    | [<CompiledName "disable_profile">] Disable_profile
    | [<CompiledName "delete_profile">] Delete_profile
    | [<CompiledName "audit_esim">] Audit_esim
    member this.Format() =
        match this with
        | Steering -> "steering"
        | Download_profile -> "download_profile"
        | Disable_profile -> "disable_profile"
        | Delete_profile -> "delete_profile"
        | Audit_esim -> "audit_esim"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type campaignStatus =
    | [<CompiledName "pending">] Pending
    | [<CompiledName "scheduled">] Scheduled
    | [<CompiledName "running">] Running
    | [<CompiledName "completed">] Completed
    | [<CompiledName "expired">] Expired
    | [<CompiledName "discarded">] Discarded
    member this.Format() =
        match this with
        | Pending -> "pending"
        | Scheduled -> "scheduled"
        | Running -> "running"
        | Completed -> "completed"
        | Expired -> "expired"
        | Discarded -> "discarded"

type Process =
    { total: Option<float>
      pending: Option<float>
      success: Option<float>
      error: Option<float> }
    ///Creates an instance of Process with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Process =
        { total = None
          pending = None
          success = None
          error = None }

type campaign =
    { ///Unique identifier of the campaign
      _id: Option<string>
      accountId: Option<string>
      userId: Option<string>
      ``type``: Option<campaignType>
      name: Option<string>
      status: Option<campaignStatus>
      scheduled: Option<string>
      startTime: Option<string>
      endTime: Option<string>
      filters: Option<Newtonsoft.Json.Linq.JObject>
      campaignReferences: Option<Newtonsoft.Json.Linq.JObject>
      ``process``: Option<Process>
      steeringList: Option<string>
      downloadProfile: Option<campaingDownloadProfile>
      deleteProfile: Option<campaingDeleteProfile> }
    ///Creates an instance of campaign with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): campaign =
        { _id = None
          accountId = None
          userId = None
          ``type`` = None
          name = None
          status = None
          scheduled = None
          startTime = None
          endTime = None
          filters = None
          campaignReferences = None
          ``process`` = None
          steeringList = None
          downloadProfile = None
          deleteProfile = None }

type Bulk =
    { id: Option<string>
      accountId: Option<string>
      count: Option<float>
      modified: Option<string>
      created: Option<string>
      status: Option<string> }
    ///Creates an instance of Bulk with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Bulk =
        { id = None
          accountId = None
          count = None
          modified = None
          created = None
          status = None }

type BadRequest =
    { code: Option<string>
      message: Option<string> }
    ///Creates an instance of BadRequest with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BadRequest = { code = None; message = None }

type Forbidden =
    { code: Option<string>
      message: Option<string> }
    ///Creates an instance of Forbidden with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Forbidden = { code = None; message = None }

type Unauthorized =
    { code: Option<string>
      message: Option<string> }
    ///Creates an instance of Unauthorized with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Unauthorized = { code = None; message = None }

type PaymentRequired =
    { code: Option<string>
      message: Option<string> }
    ///Creates an instance of PaymentRequired with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PaymentRequired = { code = None; message = None }

type Internal =
    { code: Option<string>
      message: Option<string> }
    ///Creates an instance of Internal with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Internal = { code = None; message = None }

type NotFound =
    { code: Option<string>
      message: Option<string> }
    ///Creates an instance of NotFound with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): NotFound = { code = None; message = None }

type PostAuthTokenPayload =
    { username: string
      password: string }
    ///Creates an instance of PostAuthTokenPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (username: string, password: string): PostAuthTokenPayload =
        { username = username
          password = password }

type PostAuthToken_OK =
    { token: Option<string>
      user: Option<User> }

[<RequireQualifiedAccess>]
type PostAuthToken =
    ///Returns an object containing the session token and the user data.
    | OK of payload: PostAuthToken_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostAuthRecoverPasswordPayload =
    { email: string }
    ///Creates an instance of PostAuthRecoverPasswordPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (email: string): PostAuthRecoverPasswordPayload = { email = email }

type PostAuthRecoverPassword_OK =
    { success: Option<bool>
      message: Option<string> }

[<RequireQualifiedAccess>]
type PostAuthRecoverPassword =
    ///Returns a true and a message if is successfull.
    | OK of payload: PostAuthRecoverPassword_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostAuthResetByMailtokenPayload =
    { password: string }
    ///Creates an instance of PostAuthResetByMailtokenPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (password: string): PostAuthResetByMailtokenPayload = { password = password }

type PostAuthResetByMailtoken_OK =
    { success: Option<bool>
      message: Option<string> }

[<RequireQualifiedAccess>]
type PostAuthResetByMailtoken =
    ///Returns a true and a message if success
    | OK of payload: PostAuthResetByMailtoken_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostAuthRevokeTokenPayload =
    { accountId: Option<string> }
    ///Creates an instance of PostAuthRevokeTokenPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAuthRevokeTokenPayload = { accountId = None }

type PostAuthRevokeToken_OK =
    { success: Option<bool>
      message: Option<string> }

[<RequireQualifiedAccess>]
type PostAuthRevokeToken =
    ///Returns a true value and a message if successfull
    | OK of payload: PostAuthRevokeToken_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostAuthChangePasswordPayload =
    { accountId: string
      oldPassword: string
      newPassword: string
      repeatnewPassword: string }
    ///Creates an instance of PostAuthChangePasswordPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, oldPassword: string, newPassword: string, repeatnewPassword: string): PostAuthChangePasswordPayload =
        { accountId = accountId
          oldPassword = oldPassword
          newPassword = newPassword
          repeatnewPassword = repeatnewPassword }

type PostAuthChangePassword_OK = { token: Option<string> }

[<RequireQualifiedAccess>]
type PostAuthChangePassword =
    ///New token is returned if everything goes well.
    | OK of payload: PostAuthChangePassword_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostUsersPayload =
    { accountId: string
      username: string
      password: string
      email: string
      status: Option<string>
      permissions: list<string> }
    ///Creates an instance of PostUsersPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string,
                          username: string,
                          password: string,
                          email: string,
                          permissions: list<string>): PostUsersPayload =
        { accountId = accountId
          username = username
          password = password
          email = email
          status = None
          permissions = permissions }

[<RequireQualifiedAccess>]
type PostUsers =
    ///- Returns an object with the new user data.
    ///- Sends email to the new user email address with the instructions to log in.
    ///- string, “Success! New user created, email instructions sent.”
    | OK of payload: User
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetUsers =
    ///Returns an array of user, empty array if nothing is found.
    | OK of payload: list<User>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostUsersbulkPayload =
    { username: Option<string>
      email: Option<string> }
    ///Creates an instance of PostUsersbulkPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostUsersbulkPayload = { username = None; email = None }

[<RequireQualifiedAccess>]
type PostUsersbulk =
    ///Returns an array of user, empty array if nothing is found.
    | OK of payload: list<User>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetUsersMe =
    ///Returns your user
    | OK of payload: User
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostUsersMeAcceptTc =
    ///Empty response
    | Created
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetUsersByUserId =
    ///Returns an user.
    | OK of payload: User
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutUsersByUserIdPayload =
    { accountId: string
      username: Option<string>
      email: Option<string>
      picture: Option<string>
      language: Option<string>
      timezone: Option<string> }
    ///Creates an instance of PutUsersByUserIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutUsersByUserIdPayload =
        { accountId = accountId
          username = None
          email = None
          picture = None
          language = None
          timezone = None }

[<RequireQualifiedAccess>]
type PutUsersByUserId =
    ///Returns the user modified.
    | OK of payload: User
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type DeleteUsersByUserIdPayload =
    { accountId: string }
    ///Creates an instance of DeleteUsersByUserIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): DeleteUsersByUserIdPayload = { accountId = accountId }

type DeleteUsersByUserId_OK = { success: Option<bool> }

[<RequireQualifiedAccess>]
type DeleteUsersByUserId =
    ///Returns a success message.
    | OK of payload: DeleteUsersByUserId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutUsersChangePasswordByUserIdPayload =
    { accountId: string
      password: string }
    ///Creates an instance of PutUsersChangePasswordByUserIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, password: string): PutUsersChangePasswordByUserIdPayload =
        { accountId = accountId
          password = password }

[<RequireQualifiedAccess>]
type PutUsersChangePasswordByUserId =
    ///Returns the user modified.
    | OK of payload: User
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutUsersFavoritesByUserIdPayload =
    { accountId: string
      assets: Option<list<string>>
      accounts: Option<list<string>>
      products: Option<list<string>>
      users: Option<list<string>>
      billings: Option<list<string>>
      summaries: Option<list<string>> }
    ///Creates an instance of PutUsersFavoritesByUserIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutUsersFavoritesByUserIdPayload =
        { accountId = accountId
          assets = None
          accounts = None
          products = None
          users = None
          billings = None
          summaries = None }

type PutUsersFavoritesByUserId_OK = { user: Option<User> }

[<RequireQualifiedAccess>]
type PutUsersFavoritesByUserId =
    ///Returns the user modified.
    | OK of payload: PutUsersFavoritesByUserId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutUsersCustomizationByUserIdPayload =
    { accountId: string
      customization: Option<Newtonsoft.Json.Linq.JObject> }
    ///Creates an instance of PutUsersCustomizationByUserIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutUsersCustomizationByUserIdPayload =
        { accountId = accountId
          customization = None }

type PutUsersCustomizationByUserId_OK = { user: Option<User> }

[<RequireQualifiedAccess>]
type PutUsersCustomizationByUserId =
    ///Returns the user modified.
    | OK of payload: PutUsersCustomizationByUserId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutUsersPermissionsByUserIdPayload =
    { accountId: string
      roles: list<string> }
    ///Creates an instance of PutUsersPermissionsByUserIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, roles: list<string>): PutUsersPermissionsByUserIdPayload =
        { accountId = accountId; roles = roles }

type PutUsersPermissionsByUserId_OK = { user: Option<User> }

[<RequireQualifiedAccess>]
type PutUsersPermissionsByUserId =
    ///Returns the user modified.
    | OK of payload: PutUsersPermissionsByUserId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccountsNotifications =
    ///Returns an array of notifications
    | OK of payload: Newtonsoft.Json.Linq.JArray
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PutAccountsNotificationsByNotificationId =
    ///Returns an array of notifications
    | OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type DeleteAccountsNotificationsByNotificationId_OK = { success: Option<bool> }

[<RequireQualifiedAccess>]
type DeleteAccountsNotificationsByNotificationId =
    ///null
    | OK of payload: DeleteAccountsNotificationsByNotificationId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccounts =
    ///Returns an array of accounts.
    | OK of payload: list<Account>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PostAccountsPayloadCurrency =
    | [<CompiledName "USD">] USD
    | [<CompiledName "EUR">] EUR
    | [<CompiledName "GBP">] GBP
    | [<CompiledName "MXN">] MXN
    | [<CompiledName "HKD">] HKD
    | [<CompiledName "CNY">] CNY
    | [<CompiledName "MYR">] MYR
    | [<CompiledName "AUD">] AUD
    member this.Format() =
        match this with
        | USD -> "USD"
        | EUR -> "EUR"
        | GBP -> "GBP"
        | MXN -> "MXN"
        | HKD -> "HKD"
        | CNY -> "CNY"
        | MYR -> "MYR"
        | AUD -> "AUD"

type PostAccountsPayloadBranding =
    { supportEmail: Option<string>
      supportPageMessage: Option<string>
      theme: Option<string>
      logoURL: Option<string>
      brandName: Option<string>
      domainURL: Option<string>
      emailDomain: Option<string>
      showBranding: Option<bool> }
    ///Creates an instance of PostAccountsPayloadBranding with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAccountsPayloadBranding =
        { supportEmail = None
          supportPageMessage = None
          theme = None
          logoURL = None
          brandName = None
          domainURL = None
          emailDomain = None
          showBranding = None }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type BillingFromPostAccountsPayloadType =
    | [<CompiledName "prepay">] Prepay
    | [<CompiledName "postpay">] Postpay
    member this.Format() =
        match this with
        | Prepay -> "prepay"
        | Postpay -> "postpay"

type BillingFromPostAccountsPayload =
    { ``type``: Option<BillingFromPostAccountsPayloadType> }
    ///Creates an instance of BillingFromPostAccountsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): BillingFromPostAccountsPayload = { ``type`` = None }

type PostAccountsPayload =
    { accountId: string
      name: string
      defaultEmail: Option<string>
      defaultSmsOrigin: Option<string>
      currency: Option<PostAccountsPayloadCurrency>
      resellerId: string
      status: Option<string>
      timezone: Option<string>
      ``type``: Option<string>
      minSubDataLimit: Option<float>
      minSubSMSLimit: Option<float>
      branding: Option<PostAccountsPayloadBranding>
      addresses: Option<list<string>>
      billing: Option<BillingFromPostAccountsPayload> }
    ///Creates an instance of PostAccountsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, name: string, resellerId: string): PostAccountsPayload =
        { accountId = accountId
          name = name
          defaultEmail = None
          defaultSmsOrigin = None
          currency = None
          resellerId = resellerId
          status = None
          timezone = None
          ``type`` = None
          minSubDataLimit = None
          minSubSMSLimit = None
          branding = None
          addresses = None
          billing = None }

[<RequireQualifiedAccess>]
type PostAccounts =
    ///Returns the account created.
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostAccountsbulk =
    ///Returns an array of accounts.
    | OK of payload: list<Account>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccountsByAccountId =
    ///Returns one account
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PutAccountsByAccountIdPayloadStatus =
    | [<CompiledName "active">] Active
    | [<CompiledName "inactive">] Inactive
    | [<CompiledName "suspended">] Suspended
    member this.Format() =
        match this with
        | Active -> "active"
        | Inactive -> "inactive"
        | Suspended -> "suspended"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PutAccountsByAccountIdPayloadType =
    | [<CompiledName "reseller">] Reseller
    | [<CompiledName "customer">] Customer
    member this.Format() =
        match this with
        | Reseller -> "reseller"
        | Customer -> "customer"

type PutAccountsByAccountIdPayload =
    { defaultEmail: Option<string>
      defaultSmsOrigin: Option<string>
      status: Option<PutAccountsByAccountIdPayloadStatus>
      timezone: Option<string>
      ``type``: Option<PutAccountsByAccountIdPayloadType>
      name: Option<string>
      legalName: Option<string>
      addresses: Option<list<Newtonsoft.Json.Linq.JToken>> }
    ///Creates an instance of PutAccountsByAccountIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutAccountsByAccountIdPayload =
        { defaultEmail = None
          defaultSmsOrigin = None
          status = None
          timezone = None
          ``type`` = None
          name = None
          legalName = None
          addresses = None }

[<RequireQualifiedAccess>]
type PutAccountsByAccountId =
    ///Returns the account modified.
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type DeleteAccountsByAccountIdPayload =
    { accountId: string }
    ///Creates an instance of DeleteAccountsByAccountIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): DeleteAccountsByAccountIdPayload = { accountId = accountId }

type DeleteAccountsByAccountId_OK = { success: Option<bool> }

[<RequireQualifiedAccess>]
type DeleteAccountsByAccountId =
    ///Returns an object containing a boolean if everything goes well.
    | OK of payload: DeleteAccountsByAccountId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutAccountsBrandingByAccountIdPayloadBranding =
    { logoLarge: Option<string>
      logoSmall: Option<string>
      logoLogin: Option<string>
      favicon: Option<string>
      theme: Option<string>
      supportEmail: Option<string>
      phoneSupport: Option<string>
      supportPageMessage: Option<string>
      loginPageMessage: Option<string>
      domainURL: Option<string>
      emailDomain: Option<string>
      showBranding: Option<bool> }
    ///Creates an instance of PutAccountsBrandingByAccountIdPayloadBranding with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutAccountsBrandingByAccountIdPayloadBranding =
        { logoLarge = None
          logoSmall = None
          logoLogin = None
          favicon = None
          theme = None
          supportEmail = None
          phoneSupport = None
          supportPageMessage = None
          loginPageMessage = None
          domainURL = None
          emailDomain = None
          showBranding = None }

type PutAccountsBrandingByAccountIdPayload =
    { accountId: Option<string>
      branding: Option<PutAccountsBrandingByAccountIdPayloadBranding> }
    ///Creates an instance of PutAccountsBrandingByAccountIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutAccountsBrandingByAccountIdPayload = { accountId = None; branding = None }

[<RequireQualifiedAccess>]
type PutAccountsBrandingByAccountId =
    ///Returns the account modified.
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccountsBrandingVerifyByAccountId =
    ///Returns the account modified.
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type GetAccountsRolesByAccountId_OK =
    { _id: Option<string>
      name: Option<string> }

[<RequireQualifiedAccess>]
type GetAccountsRolesByAccountId =
    ///Returns an array of roles.
    | OK of payload: list<GetAccountsRolesByAccountId_OK>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccountsRolesActionsByAccountIdAndRolename =
    ///Returns an array of actions.
    | OK of payload: list<string>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccountsNotificationsByAccountId =
    ///Returns an array of notifications
    | OK of payload: Newtonsoft.Json.Linq.JArray
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PutAccountsNotificationsByAccountIdAndNotificationId =
    ///Returns the notication modified.
    | OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type DeleteAccountsNotificationsByAccountIdAndNotificationId =
    ///Returns notification deleted
    | OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAccountsProductsByAccountId =
    ///Array of products, empty array if nothing is found..
    | OK of payload: list<Product>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PutAccountsProductsAlertsPayload =
    { accountId: string
      productId: string
      alternativeEmail: Option<string>
      alerts: Option<list<string>> }
    ///Creates an instance of PutAccountsProductsAlertsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, productId: string): PutAccountsProductsAlertsPayload =
        { accountId = accountId
          productId = productId
          alternativeEmail = None
          alerts = None }

[<RequireQualifiedAccess>]
type PutAccountsProductsAlerts =
    ///Alerts configuration after operation
    | OK
    ///Bad request
    | BadRequest
    ///Unauthorized
    | Unauthorized
    ///Internal error
    | InternalServerError

type PutAccountsTopupDirectByAccountIdPayload =
    { increase: float }
    ///Creates an instance of PutAccountsTopupDirectByAccountIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (increase: float): PutAccountsTopupDirectByAccountIdPayload = { increase = increase }

[<RequireQualifiedAccess>]
type PutAccountsTopupDirectByAccountId =
    ///Returns an account.
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostAccountsSecuritySettingsByAccountId =
    ///Returns edited account.
    | OK

[<RequireQualifiedAccess>]
type PutAccountsSecuritySettingsByAccountIdAndSecuritySettingId =
    ///Returns edited account.
    | OK

[<RequireQualifiedAccess>]
type DeleteAccountsSecuritySettingsByAccountIdAndSecuritySettingId =
    ///Returns edited account.
    | OK

type GetAccountsSecuritySettingsAvailableGapsByAccountId_OK =
    { apn: Option<string>
      cidrBlocks: Option<list<string>> }

[<RequireQualifiedAccess>]
type GetAccountsSecuritySettingsAvailableGapsByAccountId =
    ///Returns available GAPs
    | OK of payload: list<GetAccountsSecuritySettingsAvailableGapsByAccountId_OK>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type AvailableIPs =
    { cidrBlocks: Option<string>
      iPsCount: Option<float> }

type GetAccountsSecuritySettingsAvailableIpsByAccountId_OK =
    { availableIPs: Option<list<AvailableIPs>> }

[<RequireQualifiedAccess>]
type GetAccountsSecuritySettingsAvailableIpsByAccountId =
    ///Returns available IPs
    | OK of payload: GetAccountsSecuritySettingsAvailableIpsByAccountId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type GetPing_OK =
    { success: Option<bool>
      message: Option<string> }

[<RequireQualifiedAccess>]
type GetPing =
    ///OK
    | OK of payload: GetPing_OK

type PostMailPayload =
    { accountId: string
      subject: string
      body: string
      ``type``: Option<string>
      sendcopy: Option<bool> }
    ///Creates an instance of PostMailPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, subject: string, body: string): PostMailPayload =
        { accountId = accountId
          subject = subject
          body = body
          ``type`` = None
          sendcopy = None }

type PostMail_OK =
    { from: Option<string>
      ``to``: Option<string>
      subject: Option<string>
      html: Option<string> }

[<RequireQualifiedAccess>]
type PostMail =
    ///null
    | OK of payload: PostMail_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostUpload =
    ///null
    | OK

[<RequireQualifiedAccess>]
type GetFileByFileId =
    ///null
    | OK

type GetLogincustomization_OK =
    { loginPageMessage: Option<string>
      logoLogin: Option<string>
      theme: Option<string> }

[<RequireQualifiedAccess>]
type GetLogincustomization =
    ///null
    | OK of payload: GetLogincustomization_OK

type PostSimulatecdrPayload =
    { accountId: string
      iccid: string
      starttime: string
      endtime: string
      bytes: string
      mcc: string
      mnc: string
      profilemcc: string
      profilemnc: string }
    ///Creates an instance of PostSimulatecdrPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string,
                          iccid: string,
                          starttime: string,
                          endtime: string,
                          bytes: string,
                          mcc: string,
                          mnc: string,
                          profilemcc: string,
                          profilemnc: string): PostSimulatecdrPayload =
        { accountId = accountId
          iccid = iccid
          starttime = starttime
          endtime = endtime
          bytes = bytes
          mcc = mcc
          mnc = mnc
          profilemcc = profilemcc
          profilemnc = profilemnc }

type PostSimulatecdr_OK = { callId: Option<string> }

[<RequireQualifiedAccess>]
type PostSimulatecdr =
    ///null
    | OK of payload: PostSimulatecdr_OK

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PostWebhookPayloadType =
    | [<CompiledName "simAppsInfo">] SimAppsInfo
    member this.Format() =
        match this with
        | SimAppsInfo -> "simAppsInfo"

type PostWebhookPayload =
    { accountId: Option<string>
      ``type``: Option<PostWebhookPayloadType>
      ///payload
      data: Option<Newtonsoft.Json.Linq.JObject> }
    ///Creates an instance of PostWebhookPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostWebhookPayload =
        { accountId = None
          ``type`` = None
          data = None }

[<RequireQualifiedAccess>]
type PostWebhook =
    ///accepted
    | NoContent

[<RequireQualifiedAccess>]
type GetProducts =
    ///Array of products, empty array if nothing is found..
    | OK of payload: list<Product>
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

type PostProductsPayload =
    { accountId: string
      name: string }
    ///Creates an instance of PostProductsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, name: string): PostProductsPayload = { accountId = accountId; name = name }

[<RequireQualifiedAccess>]
type PostProducts =
    ///- Returns the created product
    | OK of payload: Product
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetProductsByProductId =
    ///Return created product.
    | OK of payload: Product
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

[<RequireQualifiedAccess>]
type PatchProductsByProductId =
    ///- Returns the patched product
    | OK of payload: Product
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type DeleteProductsByProductId_OK = { success: Option<bool> }

[<RequireQualifiedAccess>]
type DeleteProductsByProductId =
    ///Returns a success message.
    | OK of payload: DeleteProductsByProductId_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetSchemaProduct =
    ///- Returns an product Schema
    | OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostProductsTransferByProductIdPayload =
    { accountId: string
      originAccountId: string
      destinyAccountId: string }
    ///Creates an instance of PostProductsTransferByProductIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, originAccountId: string, destinyAccountId: string): PostProductsTransferByProductIdPayload =
        { accountId = accountId
          originAccountId = originAccountId
          destinyAccountId = destinyAccountId }

[<RequireQualifiedAccess>]
type PostProductsTransferByProductId =
    ///Return created product.
    | OK of payload: Product
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

type DataServingNetwork =
    { mcc: Option<string>
      mnc: Option<string> }

type Data =
    { servingNetwork: Option<DataServingNetwork>
      ///Only for Data cdr's
      startTime: Option<string>
      endTime: Option<string>
      billTime: Option<string>
      ///Only for Data cdr's
      ipAddress: Option<string>
      ///Only for Data cdr's
      bytes: Option<float>
      ///Only for Data cdr's
      roundedBytes: Option<float>
      ///Only for sms cdr's
      originatingAddress: Option<string>
      ///Only for sms cdr's
      destinationAddress: Option<string> }

type RemainingCredit =
    { amount: Option<float>
      currency: Option<string> }

type ProductFromRatings =
    { remainingCredit: Option<RemainingCredit>
      id: Option<string>
      name: Option<string>
      ///Only for Data cdr's
      remainingBytes: Option<float>
      ///Only for sms cdr's
      remainingSms: Option<float> }

type Cost =
    { amount: Option<float>
      currency: Option<float> }

type Ratings =
    { product: Option<ProductFromRatings>
      _id: Option<string>
      ///Only for Data cdr's
      dataUsed: Option<float>
      cost: Option<Cost> }

type GetCdr_OK =
    { data: Option<Data>
      _id: Option<string>
      iccid: Option<string>
      msisdn: Option<string>
      imei: Option<string>
      accountId: Option<string>
      bundleId: Option<string>
      ``type``: Option<string>
      ratings: Option<Ratings>
      created: Option<string> }

[<RequireQualifiedAccess>]
type GetCdr =
    ///CDR info Info.
    | OK of payload: GetCdr_OK
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///NotFound
    | NotFound of payload: NotFound
    ///Internal error
    | InternalServerError of payload: Internal

type GetCdrStats_OK =
    { _id: Option<string>
      cdrs: Option<float>
      ///only for data CDRS
      bytes: Option<float>
      ///only for sms CDRs
      smsNumber: Option<float> }

[<RequireQualifiedAccess>]
type GetCdrStats =
    ///CDR info Info.
    | OK of payload: GetCdrStats_OK

[<RequireQualifiedAccess>]
type GetAssets =
    ///SIM card Info.
    | OK of payload: list<AssetSimcard>
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PostAssetsPayloadType =
    | [<CompiledName "SIM">] SIM
    | [<CompiledName "eSIM Profile">] ESIMProfile
    member this.Format() =
        match this with
        | SIM -> "SIM"
        | ESIMProfile -> "eSIM Profile"

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PostAssetsPayloadProfileState =
    | [<CompiledName "">] EmptyString
    | [<CompiledName "onstock">] Onstock
    | [<CompiledName "disabled">] Disabled
    | [<CompiledName "enabled">] Enabled
    member this.Format() =
        match this with
        | EmptyString -> ""
        | Onstock -> "onstock"
        | Disabled -> "disabled"
        | Enabled -> "enabled"

type PostAssetsPayload =
    { accountId: string
      iccid: string
      carriers: Newtonsoft.Json.Linq.JObject
      name: Option<string>
      ``type``: Option<PostAssetsPayloadType>
      model: Option<string>
      profileState: Option<PostAssetsPayloadProfileState>
      bootstrapEid: Option<string> }
    ///Creates an instance of PostAssetsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, iccid: string, carriers: Newtonsoft.Json.Linq.JObject): PostAssetsPayload =
        { accountId = accountId
          iccid = iccid
          carriers = carriers
          name = None
          ``type`` = None
          model = None
          profileState = None
          bootstrapEid = None }

[<RequireQualifiedAccess>]
type PostAssets =
    ///Created SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostAssetsbulkPayload =
    { iccid: Option<string>
      msisdn: Option<string>
      name: Option<string>
      accountName: Option<string> }
    ///Creates an instance of PostAssetsbulkPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsbulkPayload =
        { iccid = None
          msisdn = None
          name = None
          accountName = None }

[<RequireQualifiedAccess>]
type PostAssetsbulk =
    ///SIM card Info.
    | OK of payload: list<AssetSimcard>
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAssetsByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsByIccidPayload =
    { accountId: string
      name: string }
    ///Creates an instance of PutAssetsByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, name: string): PutAssetsByIccidPayload =
        { accountId = accountId; name = name }

[<RequireQualifiedAccess>]
type PutAssetsByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type DeleteAssetsByIccidPayload =
    { accountId: string }
    ///Creates an instance of DeleteAssetsByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): DeleteAssetsByIccidPayload = { accountId = accountId }

[<RequireQualifiedAccess>]
type DeleteAssetsByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsGroupnameByIccidPayload =
    { accountId: string
      groupname: Option<string> }
    ///Creates an instance of PutAssetsGroupnameByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutAssetsGroupnameByIccidPayload =
        { accountId = accountId
          groupname = None }

[<RequireQualifiedAccess>]
type PutAssetsGroupnameByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostAssetsTransferByIccidPayload =
    { accountId: string
      originAccountId: string
      destinyAccountId: string }
    ///Creates an instance of PostAssetsTransferByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, originAccountId: string, destinyAccountId: string): PostAssetsTransferByIccidPayload =
        { accountId = accountId
          originAccountId = originAccountId
          destinyAccountId = destinyAccountId }

[<RequireQualifiedAccess>]
type PostAssetsTransferByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type IpPools =
    { carrier: Option<string>
      poolId: Option<string> }
    ///Creates an instance of IpPools with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): IpPools = { carrier = None; poolId = None }

type Subscription =
    { subscriberAccountId: Option<string>
      productId: Option<string>
      startTime: Option<string>
      ipPools: Option<list<IpPools>> }
    ///Creates an instance of Subscription with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): Subscription =
        { subscriberAccountId = None
          productId = None
          startTime = None
          ipPools = None }

type PutAssetsSubscribeByIccidPayload =
    { accountId: string
      subscription: Option<Subscription> }
    ///Creates an instance of PutAssetsSubscribeByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutAssetsSubscribeByIccidPayload =
        { accountId = accountId
          subscription = None }

[<RequireQualifiedAccess>]
type PutAssetsSubscribeByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsUnsubscribeByIccidPayload =
    { accountId: string
      subscriptionId: string }
    ///Creates an instance of PutAssetsUnsubscribeByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, subscriptionId: string): PutAssetsUnsubscribeByIccidPayload =
        { accountId = accountId
          subscriptionId = subscriptionId }

[<RequireQualifiedAccess>]
type PutAssetsUnsubscribeByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsResubscribeByIccidPayloadSubscription =
    { subscriberAccountId: Option<string>
      productId: Option<string>
      startTime: Option<string> }
    ///Creates an instance of PutAssetsResubscribeByIccidPayloadSubscription with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutAssetsResubscribeByIccidPayloadSubscription =
        { subscriberAccountId = None
          productId = None
          startTime = None }

type PutAssetsResubscribeByIccidPayload =
    { accountId: string
      subscription: Option<PutAssetsResubscribeByIccidPayloadSubscription> }
    ///Creates an instance of PutAssetsResubscribeByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutAssetsResubscribeByIccidPayload =
        { accountId = accountId
          subscription = None }

[<RequireQualifiedAccess>]
type PutAssetsResubscribeByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsSuspendByIccidPayload =
    { accountId: string }
    ///Creates an instance of PutAssetsSuspendByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutAssetsSuspendByIccidPayload = { accountId = accountId }

[<RequireQualifiedAccess>]
type PutAssetsSuspendByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsUnsuspendByIccidPayload =
    { accountId: string }
    ///Creates an instance of PutAssetsUnsuspendByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutAssetsUnsuspendByIccidPayload = { accountId = accountId }

[<RequireQualifiedAccess>]
type PutAssetsUnsuspendByIccid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsAlertsByIccidPayload =
    { accountId: string
      alternativeEmail: Option<string> }
    ///Creates an instance of PutAssetsAlertsByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutAssetsAlertsByIccidPayload =
        { accountId = accountId
          alternativeEmail = None }

[<RequireQualifiedAccess>]
type PutAssetsAlertsByIccid =
    ///SIM card Info.
    | OK
    ///Bad request
    | BadRequest
    ///Unauthorized
    | Unauthorized
    ///Internal error
    | InternalServerError

type PostAssetsPurgeByIccidPayload =
    { accountId: string
      confirm: bool }
    ///Creates an instance of PostAssetsPurgeByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, confirm: bool): PostAssetsPurgeByIccidPayload =
        { accountId = accountId
          confirm = confirm }

[<RequireQualifiedAccess>]
type PostAssetsPurgeByIccid =
    ///Purge operation sent.
    | OK
    ///Bad request
    | BadRequest
    ///Unauthorized
    | Unauthorized
    ///Internal error
    | InternalServerError

type PostAssetsSmsByIccidPayload =
    { accountId: string
      message: string
      dcs: Option<string>
      origin: Option<string> }
    ///Creates an instance of PostAssetsSmsByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, message: string): PostAssetsSmsByIccidPayload =
        { accountId = accountId
          message = message
          dcs = None
          origin = None }

[<RequireQualifiedAccess>]
type PostAssetsSmsByIccid =
    ///send sms to the destiny iccid.
    | OK
    ///Bad request
    | BadRequest
    ///Unauthorized
    | Unauthorized
    ///Internal error
    | InternalServerError

type PostAssetsLimitByIccidPayload =
    { accountId: string
      datalimit: Option<string>
      smslimit: Option<string> }
    ///Creates an instance of PostAssetsLimitByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PostAssetsLimitByIccidPayload =
        { accountId = accountId
          datalimit = None
          smslimit = None }

[<RequireQualifiedAccess>]
type PostAssetsLimitByIccid =
    ///SIM card Info.
    | OK
    ///Bad request
    | BadRequest
    ///Unauthorized
    | Unauthorized
    ///Internal error
    | InternalServerError

type PostAssetsTagsByIccidPayloadTags =
    { key: Option<string>
      value: Option<string> }
    ///Creates an instance of PostAssetsTagsByIccidPayloadTags with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsTagsByIccidPayloadTags = { key = None; value = None }

type PostAssetsTagsByIccidPayload =
    { accountId: Option<string>
      tags: Option<list<PostAssetsTagsByIccidPayloadTags>> }
    ///Creates an instance of PostAssetsTagsByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsTagsByIccidPayload = { accountId = None; tags = None }

[<RequireQualifiedAccess>]
type PostAssetsTagsByIccid =
    ///SIM card Info.
    | OK
    ///Bad request
    | BadRequest
    ///Unauthorized
    | Unauthorized
    ///Internal error
    | InternalServerError

type DfProducts =
    { DataPoolProduct: Option<string>
      PerMbProduct: Option<string>
      SharedDataPoolProduct: Option<string>
      PerSMSProduct: Option<string>
      SMSPoolProduct: Option<string>
      PerSecondProduct: Option<string>
      VoicePoolProduct: Option<string> }

type Bundles =
    { dfProducts: Option<DfProducts>
      bundleId: Option<string>
      localProductId: Option<string>
      localProductName: Option<string>
      initialSize: Option<float>
      remainingBytes: Option<float>
      dataUsed: Option<float>
      startTime: Option<string>
      endTime: Option<string>
      cost: Option<float>
      remainingCredit: Option<float>
      creditUsed: Option<float>
      perMbCost: Option<float>
      ``type``: Option<string> }

type ProvisioningSubscriptions =
    { bundles: Option<list<Bundles>>
      accoundtId: Option<string>
      id: Option<string>
      limit: Option<float> }

type ProvisioningLastCallServingNetwork =
    { mcc: Option<string>
      mnc: Option<string> }

type ProvisioningLastCall =
    { servingNetwork: Option<ProvisioningLastCallServingNetwork>
      startTime: Option<string>
      endTime: Option<string>
      ipAddress: Option<string>
      imei: Option<string>
      bytes: Option<float>
      roundedBytes: Option<float> }

type Provisioning =
    { id: Option<string>
      iccid: Option<string>
      ownerAccountId: Option<string>
      ownerAccountName: Option<string>
      status: Option<string>
      limit: Option<float>
      subscriptions: Option<list<ProvisioningSubscriptions>>
      activationDate: Option<string>
      lastCall: Option<ProvisioningLastCall>
      setups: Option<list<string>>
      ownership: Option<list<string>> }

type NetworkServices =
    { smsMO: Option<bool>
      smsMT: Option<bool>
      voiceMO: Option<bool>
      voiceMT: Option<bool> }

type LastRegistration =
    { startTime: Option<string>
      mcc: Option<string>
      mnc: Option<string>
      lac: Option<string>
      cellId: Option<string> }

type Network =
    { services: Option<NetworkServices>
      lastRegistration: Option<LastRegistration>
      imsi: Option<string>
      msisdn: Option<string>
      fixedIP: Option<string> }

type GetAssetsDiagnosticByIccid_OKDataLastRegistration = { startTime: Option<string> }

type LiveDataSession =
    { startTime: Option<string>
      ``type``: Option<string>
      provider: Option<string> }

type LastActiveSession =
    { startTime: Option<string>
      endTime: Option<string>
      outcome: Option<string> }

type GetAssetsDiagnosticByIccid_OKData =
    { apn: Option<string>
      ip: Option<string>
      imei: Option<string>
      lastRegistration: Option<GetAssetsDiagnosticByIccid_OKDataLastRegistration>
      liveDataSession: Option<LiveDataSession>
      lastActiveSession: Option<LastActiveSession> }

type AppsCompatibility =
    { eSIMeUICC: Option<string>
      multiIMSI: Option<string>
      createdAt: Option<string> }

type GetAssetsDiagnosticByIccid_OK =
    { result: Option<string>
      description: Option<string>
      notice: Option<Newtonsoft.Json.Linq.JToken>
      provisioning: Option<Provisioning>
      network: Option<Network>
      data: Option<GetAssetsDiagnosticByIccid_OKData>
      appsCompatibility: Option<AppsCompatibility> }

[<RequireQualifiedAccess>]
type GetAssetsDiagnosticByIccid =
    ///SIM card Info plus the network state.
    | OK of payload: GetAssetsDiagnosticByIccid_OK
    ///Authentication error.
    | Unauthorized
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

type Location =
    { lat: Option<float>
      lng: Option<float> }

type GetAssetsLocationByIccid_OK =
    { outcode: Option<string>
      location: Option<Location>
      resolution: Option<float>
      message: Option<string> }

[<RequireQualifiedAccess>]
type GetAssetsLocationByIccid =
    ///Data of the location of the sims. If outcode is equal to mcc, the resolution will have the mcc
    | OK of payload: GetAssetsLocationByIccid_OK
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Unauthorized
    | NotFound of payload: NotFound
    ///Internal error
    | InternalServerError of payload: Internal

type GetAssetsSessionsByIccid_OK =
    { id: Option<string>
      iccid: Option<string>
      msisdn: Option<string>
      imsi: Option<string>
      imei: Option<string>
      ipAddress: Option<string>
      SGSNAddress: Option<string>
      GGSNAddress: Option<string>
      RATType: Option<float>
      userLocationInfo: Option<string>
      servingNetworkMcc: Option<float>
      servingNetworkMnc: Option<float>
      homeNetworkMcc: Option<float>
      homeNetworkMnc: Option<float>
      startTime: Option<string>
      quota: Option<float>
      usage: Option<list<string>> }

[<RequireQualifiedAccess>]
type GetAssetsSessionsByIccid =
    ///Get a sessions active object
    | OK of payload: GetAssetsSessionsByIccid_OK
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Unauthorized
    | NotFound of payload: NotFound
    ///Internal error
    | InternalServerError of payload: Internal

type PostAssetsReallocateIpByIccidPayloadIpPools =
    { carrier: Option<string>
      poolId: Option<string> }
    ///Creates an instance of PostAssetsReallocateIpByIccidPayloadIpPools with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsReallocateIpByIccidPayloadIpPools = { carrier = None; poolId = None }

type PostAssetsReallocateIpByIccidPayload =
    { accountId: Option<string>
      ipPools: Option<list<PostAssetsReallocateIpByIccidPayloadIpPools>> }
    ///Creates an instance of PostAssetsReallocateIpByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsReallocateIpByIccidPayload = { accountId = None; ipPools = None }

[<RequireQualifiedAccess>]
type PostAssetsReallocateIpByIccid =
    ///SIM card Info.
    | OK

[<RequireQualifiedAccess>]
type GetReportsCustom =
    ///A list of reports filtered by the user
    | OK of payload: list<CustomReportNodata>
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostReportsCustom =
    ///No Content
    | OK
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Forbidden
    | Forbidden of payload: Forbidden
    ///Not Found
    | NotFound of payload: NotFound
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type DeleteReportsCustom =
    ///No Content
    | NoContent
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetReportsCustomByReportId =
    ///A report with all the information related
    | OK of payload: CustomReportData
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Not Found
    | NotFound of payload: NotFound
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type DeleteReportsCustomByReportId =
    ///No Content
    | NoContent
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Not Found
    | NotFound of payload: NotFound
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetEvents =
    ///Returns an array of events.
    | OK of payload: Events
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetZonesSchemes =
    ///A list of zone schemes
    | OK of payload: list<string>

[<RequireQualifiedAccess>]
type GetZonesSchemesByZoneSchemeId =
    ///A list of accounts
    | OK of payload: Newtonsoft.Json.Linq.JArray

type PostBulkAssetsSubscribePayloadDataSubscriptionIpPools =
    { carrier: Option<string>
      poolId: Option<string> }
    ///Creates an instance of PostBulkAssetsSubscribePayloadDataSubscriptionIpPools with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsSubscribePayloadDataSubscriptionIpPools = { carrier = None; poolId = None }

type PostBulkAssetsSubscribePayloadDataSubscription =
    { subscriberAccount: Option<string>
      productId: Option<string>
      startTime: Option<string>
      ipPools: Option<list<PostBulkAssetsSubscribePayloadDataSubscriptionIpPools>> }
    ///Creates an instance of PostBulkAssetsSubscribePayloadDataSubscription with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsSubscribePayloadDataSubscription =
        { subscriberAccount = None
          productId = None
          startTime = None
          ipPools = None }

type PostBulkAssetsSubscribePayloadData =
    { subscription: Option<PostBulkAssetsSubscribePayloadDataSubscription> }
    ///Creates an instance of PostBulkAssetsSubscribePayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsSubscribePayloadData = { subscription = None }

type PostBulkAssetsSubscribePayload =
    { data: Option<PostBulkAssetsSubscribePayloadData> }
    ///Creates an instance of PostBulkAssetsSubscribePayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsSubscribePayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsSubscribe =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsTransferPayloadData =
    { originAccount: string
      destinyAccount: string }
    ///Creates an instance of PostBulkAssetsTransferPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (originAccount: string, destinyAccount: string): PostBulkAssetsTransferPayloadData =
        { originAccount = originAccount
          destinyAccount = destinyAccount }

type PostBulkAssetsTransferPayload =
    { data: Option<PostBulkAssetsTransferPayloadData> }
    ///Creates an instance of PostBulkAssetsTransferPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsTransferPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsTransfer =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsReturnPayloadData =
    { accountId: string }
    ///Creates an instance of PostBulkAssetsReturnPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PostBulkAssetsReturnPayloadData = { accountId = accountId }

type PostBulkAssetsReturnPayload =
    { data: Option<PostBulkAssetsReturnPayloadData> }
    ///Creates an instance of PostBulkAssetsReturnPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsReturnPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsReturn =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PutBulkAssetsSuspend =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PutBulkAssetsUnsuspend =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsResubscribePayloadDataSubscription =
    { subscriberAccount: Option<string>
      productId: Option<string>
      startTime: Option<string> }
    ///Creates an instance of PostBulkAssetsResubscribePayloadDataSubscription with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsResubscribePayloadDataSubscription =
        { subscriberAccount = None
          productId = None
          startTime = None }

type PostBulkAssetsResubscribePayloadData =
    { subscription: Option<PostBulkAssetsResubscribePayloadDataSubscription> }
    ///Creates an instance of PostBulkAssetsResubscribePayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsResubscribePayloadData = { subscription = None }

type PostBulkAssetsResubscribePayload =
    { data: Option<PostBulkAssetsResubscribePayloadData> }
    ///Creates an instance of PostBulkAssetsResubscribePayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsResubscribePayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsResubscribe =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsPayload =
    { name: Option<string> }
    ///Creates an instance of PostBulkAssetsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsPayload = { name = None }

[<RequireQualifiedAccess>]
type PostBulkAssets =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutBulkAssetsPayloadData =
    { name: Option<string> }
    ///Creates an instance of PutBulkAssetsPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkAssetsPayloadData = { name = None }

type PutBulkAssetsPayload =
    { data: Option<PutBulkAssetsPayloadData> }
    ///Creates an instance of PutBulkAssetsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkAssetsPayload = { data = None }

[<RequireQualifiedAccess>]
type PutBulkAssets =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutBulkAssetsGroupnamePayloadData =
    { groupname: Option<string> }
    ///Creates an instance of PutBulkAssetsGroupnamePayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkAssetsGroupnamePayloadData = { groupname = None }

type PutBulkAssetsGroupnamePayload =
    { data: Option<PutBulkAssetsGroupnamePayloadData> }
    ///Creates an instance of PutBulkAssetsGroupnamePayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkAssetsGroupnamePayload = { data = None }

[<RequireQualifiedAccess>]
type PutBulkAssetsGroupname =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsLimitPayloadData =
    { limit: Option<float>
      smslimit: Option<float> }
    ///Creates an instance of PostBulkAssetsLimitPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsLimitPayloadData = { limit = None; smslimit = None }

type PostBulkAssetsLimitPayload =
    { data: Option<PostBulkAssetsLimitPayloadData> }
    ///Creates an instance of PostBulkAssetsLimitPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsLimitPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsLimit =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutBulkAssetsAlertsPayloadData =
    { alternativeEmail: Option<string> }
    ///Creates an instance of PutBulkAssetsAlertsPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkAssetsAlertsPayloadData = { alternativeEmail = None }

type PutBulkAssetsAlertsPayload =
    { data: Option<PutBulkAssetsAlertsPayloadData> }
    ///Creates an instance of PutBulkAssetsAlertsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkAssetsAlertsPayload = { data = None }

[<RequireQualifiedAccess>]
type PutBulkAssetsAlerts =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsSmsPayloadData =
    { message: Option<string>
      dcs: Option<string>
      origin: Option<string> }
    ///Creates an instance of PostBulkAssetsSmsPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsSmsPayloadData =
        { message = None
          dcs = None
          origin = None }

type PostBulkAssetsSmsPayload =
    { data: Option<PostBulkAssetsSmsPayloadData> }
    ///Creates an instance of PostBulkAssetsSmsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsSmsPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsSms =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostBulkAssetsPurgePayloadData =
    { confirm: Option<string> }
    ///Creates an instance of PostBulkAssetsPurgePayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsPurgePayloadData = { confirm = None }

type PostBulkAssetsPurgePayload =
    { data: Option<PostBulkAssetsPurgePayloadData> }
    ///Creates an instance of PostBulkAssetsPurgePayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsPurgePayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsPurge =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostBulkAssetsUpdateTemplate =
    ///Template in CSV.
    | OK

[<RequireQualifiedAccess>]
type PostBulkAssetsUpdateProcess =
    ///Bulk process object.
    | Accepted of payload: Bulk

type PostBulkAssetsReallocateIpPayloadIpPools =
    { carrier: Option<string>
      poolId: Option<string> }
    ///Creates an instance of PostBulkAssetsReallocateIpPayloadIpPools with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsReallocateIpPayloadIpPools = { carrier = None; poolId = None }

type PostBulkAssetsReallocateIpPayload =
    { accountId: Option<string>
      ipPools: Option<list<PostBulkAssetsReallocateIpPayloadIpPools>> }
    ///Creates an instance of PostBulkAssetsReallocateIpPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkAssetsReallocateIpPayload = { accountId = None; ipPools = None }

[<RequireQualifiedAccess>]
type PostBulkAssetsReallocateIp =
    ///Bulk process object.
    | Accepted of payload: Bulk
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostPaymentsTopupPaypalPayload =
    { accountId: string
      amount: float
      currency: Option<string>
      returnUrl: Option<string>
      cancelUrl: Option<string> }
    ///Creates an instance of PostPaymentsTopupPaypalPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, amount: float): PostPaymentsTopupPaypalPayload =
        { accountId = accountId
          amount = amount
          currency = None
          returnUrl = None
          cancelUrl = None }

type PostPaymentsTopupPaypal_OK =
    { orderId: Option<string>
      providerId: Option<string> }

[<RequireQualifiedAccess>]
type PostPaymentsTopupPaypal =
    ///Returns the 2 ids you need for the Paypal modal.
    | OK of payload: PostPaymentsTopupPaypal_OK
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

type PostPaymentsConfirmTopupPaypalPayload =
    { accountId: string
      orderId: string
      status: bool }
    ///Creates an instance of PostPaymentsConfirmTopupPaypalPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, orderId: string, status: bool): PostPaymentsConfirmTopupPaypalPayload =
        { accountId = accountId
          orderId = orderId
          status = status }

[<RequireQualifiedAccess>]
type PostPaymentsConfirmTopupPaypal =
    ///Returns an array of accounts.
    | OK of payload: Account
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetSecurityAlerts =
    ///Array of Alerts, empty array if nothing is found..
    | OK of payload: list<Alert>
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

[<RequireQualifiedAccess>]
type GetSecurityAlertsByAlertId =
    ///Alert
    | OK of payload: Alert
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

type PutSecurityAlertsByAlertIdPayload =
    { alarmLink: Option<string>
      archived: Option<bool>
      description: Option<string>
      destinationEthernet: Option<string>
      destinationIP: Option<string>
      destinationPort: Option<string>
      protocol: Option<string>
      sourceEthernet: Option<string>
      sourceIP: Option<string>
      sourcePort: Option<string>
      time: Option<System.DateTimeOffset>
      timeStamp: Option<System.DateTimeOffset>
      ``type``: Option<string>
      visible: Option<bool> }
    ///Creates an instance of PutSecurityAlertsByAlertIdPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutSecurityAlertsByAlertIdPayload =
        { alarmLink = None
          archived = None
          description = None
          destinationEthernet = None
          destinationIP = None
          destinationPort = None
          protocol = None
          sourceEthernet = None
          sourceIP = None
          sourcePort = None
          time = None
          timeStamp = None
          ``type`` = None
          visible = None }

[<RequireQualifiedAccess>]
type PutSecurityAlertsByAlertId =
    ///Updated Alert
    | OK of payload: Alert
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

[<RequireQualifiedAccess>]
type DeleteSecurityAlertsByAlertId =
    ///Acknowledgement of successful request
    | NoContent
    ///Invalid Parameters.
    | BadRequest
    ///Authentication error.
    | Unauthorized
    ///Not Authorized
    | Forbidden
    ///Internal Application Error.
    | InternalServerError
    ///Service Unavailable.
    | ServiceUnavailable

[<RequireQualifiedAccess>]
type GetGraphSecurityTopthreaten =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsPie>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraphSecurityByalarmtype =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsTable>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetEsims =
    ///eSIM cards Info.
    | OK of payload: list<eSIM>

type PostEsimsPayloadProfiles =
    { iccid: Option<string> }
    ///Creates an instance of PostEsimsPayloadProfiles with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsPayloadProfiles = { iccid = None }

type PostEsimsPayload =
    { accountId: string
      eid: string
      profiles: list<PostEsimsPayloadProfiles>
      eSimName: Option<string>
      eSimGroupName: Option<string> }
    ///Creates an instance of PostEsimsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, eid: string, profiles: list<PostEsimsPayloadProfiles>): PostEsimsPayload =
        { accountId = accountId
          eid = eid
          profiles = profiles
          eSimName = None
          eSimGroupName = None }

[<RequireQualifiedAccess>]
type PostEsims =
    ///Created eSIM card Info.
    | OK of payload: eSIM
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type PostEsimsbulk =
    ///eSIM cards Info.
    | OK of payload: list<eSIM>

[<RequireQualifiedAccess>]
type GetEsimsByEid =
    ///eSIM card Info.
    | OK of payload: eSIM

type PutEsimsByEidPayload =
    { accountId: Option<string>
      eSimName: Option<string>
      eSimGroupName: Option<string> }
    ///Creates an instance of PutEsimsByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutEsimsByEidPayload =
        { accountId = None
          eSimName = None
          eSimGroupName = None }

[<RequireQualifiedAccess>]
type PutEsimsByEid =
    ///eSIM card Info.
    | OK

type PostEsimsTransferByEidPayload =
    { accountId: string
      originAccountId: string
      destinyAccountId: string }
    ///Creates an instance of PostEsimsTransferByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string, originAccountId: string, destinyAccountId: string): PostEsimsTransferByEidPayload =
        { accountId = accountId
          originAccountId = originAccountId
          destinyAccountId = destinyAccountId }

[<RequireQualifiedAccess>]
type PostEsimsTransferByEid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostEsimsReturnByEidPayload =
    { accountId: string }
    ///Creates an instance of PostEsimsReturnByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PostEsimsReturnByEidPayload = { accountId = accountId }

[<RequireQualifiedAccess>]
type PostEsimsReturnByEid =
    ///SIM card Info.
    | OK of payload: AssetSimcard
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutEsimsSubscribeByEidPayloadSubscriptionIpPools =
    { carrier: Option<string>
      poolId: Option<string> }
    ///Creates an instance of PutEsimsSubscribeByEidPayloadSubscriptionIpPools with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutEsimsSubscribeByEidPayloadSubscriptionIpPools = { carrier = None; poolId = None }

type PutEsimsSubscribeByEidPayloadSubscription =
    { subscriberAccountId: Option<string>
      productId: Option<string>
      startTime: Option<string>
      ipPools: Option<list<PutEsimsSubscribeByEidPayloadSubscriptionIpPools>> }
    ///Creates an instance of PutEsimsSubscribeByEidPayloadSubscription with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutEsimsSubscribeByEidPayloadSubscription =
        { subscriberAccountId = None
          productId = None
          startTime = None
          ipPools = None }

type PutEsimsSubscribeByEidPayload =
    { accountId: string
      subscription: Option<PutEsimsSubscribeByEidPayloadSubscription> }
    ///Creates an instance of PutEsimsSubscribeByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutEsimsSubscribeByEidPayload =
        { accountId = accountId
          subscription = None }

[<RequireQualifiedAccess>]
type PutEsimsSubscribeByEid =
    ///SIM card Info.
    | OK

type PutEsimsSuspendByEidPayload =
    { accountId: Option<string> }
    ///Creates an instance of PutEsimsSuspendByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutEsimsSuspendByEidPayload = { accountId = None }

[<RequireQualifiedAccess>]
type PutEsimsSuspendByEid =
    ///SIM card Info.
    | OK

type PutEsimsUnsuspendByEidPayload =
    { accountId: Option<string> }
    ///Creates an instance of PutEsimsUnsuspendByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutEsimsUnsuspendByEidPayload = { accountId = None }

[<RequireQualifiedAccess>]
type PutEsimsUnsuspendByEid =
    ///SIM card Info.
    | OK

type PutEsimsAlertsByEidPayload =
    { accountId: string
      alternativeEmail: Option<string> }
    ///Creates an instance of PutEsimsAlertsByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (accountId: string): PutEsimsAlertsByEidPayload =
        { accountId = accountId
          alternativeEmail = None }

[<RequireQualifiedAccess>]
type PutEsimsAlertsByEid =
    ///SIM card Info.
    | OK

type PostEsimsPurgeByEidPayload =
    { accountId: Option<string> }
    ///Creates an instance of PostEsimsPurgeByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsPurgeByEidPayload = { accountId = None }

[<RequireQualifiedAccess>]
type PostEsimsPurgeByEid =
    ///SIM card Info.
    | OK

type PostEsimsSmsByEidPayload =
    { accountId: Option<string>
      message: Option<string>
      dcs: Option<string>
      origin: Option<string> }
    ///Creates an instance of PostEsimsSmsByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsSmsByEidPayload =
        { accountId = None
          message = None
          dcs = None
          origin = None }

[<RequireQualifiedAccess>]
type PostEsimsSmsByEid =
    ///SIM card Info.
    | OK

type PostEsimsLimitByEidPayload =
    { accountId: Option<string>
      datalimit: Option<float>
      smslimit: Option<float> }
    ///Creates an instance of PostEsimsLimitByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsLimitByEidPayload =
        { accountId = None
          datalimit = None
          smslimit = None }

[<RequireQualifiedAccess>]
type PostEsimsLimitByEid =
    ///SIM card Info.
    | OK

type PostEsimsDownloadProfileByEidPayload =
    { accountId: Option<string>
      iccid: Option<string>
      enable: Option<bool>
      uuid: Option<string> }
    ///Creates an instance of PostEsimsDownloadProfileByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsDownloadProfileByEidPayload =
        { accountId = None
          iccid = None
          enable = None
          uuid = None }

[<RequireQualifiedAccess>]
type PostEsimsDownloadProfileByEid =
    ///SIM card Info.
    | OK

type PostEsimsEnableProfileByEidPayload =
    { accountId: Option<string>
      iccid: Option<string> }
    ///Creates an instance of PostEsimsEnableProfileByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsEnableProfileByEidPayload = { accountId = None; iccid = None }

[<RequireQualifiedAccess>]
type PostEsimsEnableProfileByEid =
    ///SIM card Info.
    | OK

type PostEsimsDisableProfileByEidPayload =
    { accountId: Option<string> }
    ///Creates an instance of PostEsimsDisableProfileByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsDisableProfileByEidPayload = { accountId = None }

[<RequireQualifiedAccess>]
type PostEsimsDisableProfileByEid =
    ///eSIM card Info.
    | OK

type PostEsimsDeleteProfileByEidPayload =
    { accountId: Option<string>
      iccid: Option<string>
      ///used for delete more than one profile, if is set, iccid will be ignored
      iccids: Option<list<string>> }
    ///Creates an instance of PostEsimsDeleteProfileByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsDeleteProfileByEidPayload =
        { accountId = None
          iccid = None
          iccids = None }

[<RequireQualifiedAccess>]
type PostEsimsDeleteProfileByEid =
    ///eSIM card Info.
    | OK

type PostEsimsAuditByEidPayload =
    { accountId: Option<string> }
    ///Creates an instance of PostEsimsAuditByEidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostEsimsAuditByEidPayload = { accountId = None }

[<RequireQualifiedAccess>]
type PostEsimsAuditByEid =
    ///eSIM card Info.
    | OK

type PutBulkEsimsPayloadData =
    { eSimName: Option<string>
      eSimGroupName: Option<string> }
    ///Creates an instance of PutBulkEsimsPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkEsimsPayloadData =
        { eSimName = None
          eSimGroupName = None }

type PutBulkEsimsPayload =
    { data: Option<PutBulkEsimsPayloadData> }
    ///Creates an instance of PutBulkEsimsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkEsimsPayload = { data = None }

[<RequireQualifiedAccess>]
type PutBulkEsims =
    ///Bulk process object.
    | Accepted

type PostBulkEsimsTransferPayloadData =
    { originAccountId: Option<string>
      destinyAccountId: Option<string> }
    ///Creates an instance of PostBulkEsimsTransferPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsTransferPayloadData =
        { originAccountId = None
          destinyAccountId = None }

type PostBulkEsimsTransferPayload =
    { data: Option<PostBulkEsimsTransferPayloadData> }
    ///Creates an instance of PostBulkEsimsTransferPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsTransferPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkEsimsTransfer =
    ///Bulk process object.
    | Accepted

type PostBulkEsimsReturnPayloadData =
    { accountId: Option<string> }
    ///Creates an instance of PostBulkEsimsReturnPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsReturnPayloadData = { accountId = None }

type PostBulkEsimsReturnPayload =
    { data: Option<PostBulkEsimsReturnPayloadData> }
    ///Creates an instance of PostBulkEsimsReturnPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsReturnPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkEsimsReturn =
    ///Bulk process object.
    | Accepted

type PostBulkEsimsSubscribePayloadDataSubscriptionIpPools =
    { carrier: Option<string>
      poolId: Option<string> }
    ///Creates an instance of PostBulkEsimsSubscribePayloadDataSubscriptionIpPools with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsSubscribePayloadDataSubscriptionIpPools = { carrier = None; poolId = None }

type PostBulkEsimsSubscribePayloadDataSubscription =
    { subscriberAccount: Option<string>
      productId: Option<string>
      startTime: Option<string>
      ipPools: Option<list<PostBulkEsimsSubscribePayloadDataSubscriptionIpPools>> }
    ///Creates an instance of PostBulkEsimsSubscribePayloadDataSubscription with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsSubscribePayloadDataSubscription =
        { subscriberAccount = None
          productId = None
          startTime = None
          ipPools = None }

type PostBulkEsimsSubscribePayloadData =
    { subscription: Option<PostBulkEsimsSubscribePayloadDataSubscription> }
    ///Creates an instance of PostBulkEsimsSubscribePayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsSubscribePayloadData = { subscription = None }

type PostBulkEsimsSubscribePayload =
    { data: Option<PostBulkEsimsSubscribePayloadData> }
    ///Creates an instance of PostBulkEsimsSubscribePayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsSubscribePayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkEsimsSubscribe =
    ///Bulk process object.
    | Accepted

[<RequireQualifiedAccess>]
type PutBulkEsimsSuspend =
    ///Bulk process object.
    | Accepted

[<RequireQualifiedAccess>]
type PutBulkEsimsUnsuspend =
    ///Bulk process object.
    | Accepted

type PutBulkEsimsAlertsPayloadData =
    { confirm: Option<string> }
    ///Creates an instance of PutBulkEsimsAlertsPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkEsimsAlertsPayloadData = { confirm = None }

type PutBulkEsimsAlertsPayload =
    { data: Option<PutBulkEsimsAlertsPayloadData> }
    ///Creates an instance of PutBulkEsimsAlertsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutBulkEsimsAlertsPayload = { data = None }

[<RequireQualifiedAccess>]
type PutBulkEsimsAlerts =
    ///Bulk process object.
    | Accepted

type PostBulkEsimsPurgePayloadData =
    { confirm: Option<string> }
    ///Creates an instance of PostBulkEsimsPurgePayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsPurgePayloadData = { confirm = None }

type PostBulkEsimsPurgePayload =
    { data: Option<PostBulkEsimsPurgePayloadData> }
    ///Creates an instance of PostBulkEsimsPurgePayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsPurgePayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkEsimsPurge =
    ///Bulk process object.
    | Accepted

type PostBulkEsimsSmsPayloadData =
    { message: Option<string>
      dcs: Option<string>
      origin: Option<string> }
    ///Creates an instance of PostBulkEsimsSmsPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsSmsPayloadData =
        { message = None
          dcs = None
          origin = None }

type PostBulkEsimsSmsPayload =
    { data: Option<PostBulkEsimsSmsPayloadData> }
    ///Creates an instance of PostBulkEsimsSmsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsSmsPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkEsimsSms =
    ///Bulk process object.
    | Accepted

type PostBulkEsimsLimitPayloadData =
    { limit: Option<float>
      smslimit: Option<float> }
    ///Creates an instance of PostBulkEsimsLimitPayloadData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsLimitPayloadData = { limit = None; smslimit = None }

type PostBulkEsimsLimitPayload =
    { data: Option<PostBulkEsimsLimitPayloadData> }
    ///Creates an instance of PostBulkEsimsLimitPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostBulkEsimsLimitPayload = { data = None }

[<RequireQualifiedAccess>]
type PostBulkEsimsLimit =
    ///Bulk process object.
    | Accepted

[<RequireQualifiedAccess>]
type GetGraph1 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph2 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph3 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsPie>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph4 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsPie>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph5 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph6 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsTable>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph7 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsTable>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph8 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph9 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph10 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsTable>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph11 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsPie>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraph12 =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetGraphStatusesperday =
    ///Returns an array of accounts.
    | OK of payload: list<GraphsLine>
    ///Bad request.
    | BadRequest of payload: BadRequest
    ///Unauthorized.
    | Unauthorized of payload: Unauthorized
    ///Internal error.
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetStatusComponents =
    ///List of components with their status.
    | OK of payload: list<statusComponent>

type PostAssetsQuickDialByIccidPayload =
    { ///Account ID for user context.
      accountId: Option<string>
      ///The memory location from "00" to "99"
      location: Option<string>
      ///Destination address. Either another 'oa' to call another subscriber in the system, or a telephone number in full international format.
      da: Option<string> }
    ///Creates an instance of PostAssetsQuickDialByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsQuickDialByIccidPayload =
        { accountId = None
          location = None
          da = None }

[<RequireQualifiedAccess>]
type PostAssetsQuickDialByIccid =
    ///QuickDial entry.
    | OK of payload: quickDial
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAssetsQuickDialByIccid =
    ///A list of quickDial entries.
    | OK of payload: list<quickDial>
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetAssetsQuickDialByIccidAndLocation =
    ///A quickDial entry.
    | OK of payload: quickDial
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PutAssetsQuickDialByIccidAndLocationPayload =
    { ///Account ID for user context.
      accountId: Option<string>
      ///Destination address. Either another 'oa' to call another subscriber in the system, or a telephone number in full international format.
      da: Option<string> }
    ///Creates an instance of PutAssetsQuickDialByIccidAndLocationPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutAssetsQuickDialByIccidAndLocationPayload = { accountId = None; da = None }

[<RequireQualifiedAccess>]
type PutAssetsQuickDialByIccidAndLocation =
    ///A quickDial entry.
    | OK of payload: quickDial
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type DeleteAssetsQuickDialByIccidAndLocation =
    ///Deleted. No Content.
    | NoContent
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

type PostAssetsDialByIccidPayload =
    { ///Account ID for user context.
      accountId: Option<string>
      ///Origin address. The CLI to send for the call.
      oa: Option<string>
      ///The time it will ring for. [0-30]
      timer: Option<float> }
    ///Creates an instance of PostAssetsDialByIccidPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAssetsDialByIccidPayload =
        { accountId = None
          oa = None
          timer = None }

[<RequireQualifiedAccess>]
type PostAssetsDialByIccid =
    ///call completed
    | OK of payload: dialResponse
    ///Bad request
    | BadRequest of payload: BadRequest
    ///Unauthorized
    | Unauthorized of payload: Unauthorized
    ///Internal error
    | InternalServerError of payload: Internal

[<RequireQualifiedAccess>]
type GetSteeringlists =
    ///steeringLists Info.
    | OK of payload: list<steeringList>

type PostSteeringlistsPayload =
    { accountId: Option<string>
      name: Option<string>
      value: Option<string> }
    ///Creates an instance of PostSteeringlistsPayload with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostSteeringlistsPayload =
        { accountId = None
          name = None
          value = None }

[<RequireQualifiedAccess>]
type PostSteeringlists =
    ///Created steering list Info.
    | OK of payload: steeringList

type DeleteSteeringlistsBySteeringListId_OK = { success: Option<bool> }

[<RequireQualifiedAccess>]
type DeleteSteeringlistsBySteeringListId =
    ///Returns a success message.
    | OK of payload: DeleteSteeringlistsBySteeringListId_OK

[<RequireQualifiedAccess>]
type GetCampaigns =
    ///campaigns Info.
    | OK of payload: list<campaign>

[<RequireQualifiedAccess>]
type PostCampaigns =
    ///campaign Info.
    | OK of payload: campaign

[<RequireQualifiedAccess>]
type PutCampaigns =
    ///campaign Info.
    | OK of payload: campaign

[<RequireQualifiedAccess>]
type DeleteCampaigns =
    ///campaign Info.
    | OK of payload: campaign

[<RequireQualifiedAccess>]
type GetCampaignsByCampaignId =
    ///campaign Info.
    | OK of payload: campaign

type PostCampaignsCompletionForecast_OK =
    { complete: Option<float>
      total: Option<float>
      minutes: Option<float>
      errors: Option<Newtonsoft.Json.Linq.JObject> }

[<RequireQualifiedAccess>]
type PostCampaignsCompletionForecast =
    ///Campaign completion forecast info.
    | OK of payload: PostCampaignsCompletionForecast_OK

[<RequireQualifiedAccess>]
type GetCampaignsItemsByCampaignId =
    ///campaign items Info.
    | OK of payload: list<campaignItem>
