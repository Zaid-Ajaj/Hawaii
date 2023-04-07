namespace rec Watchful.Types

type Audits = list<Audit>

type Audit =
    { ///Unique identifier for the audit
      id: int64 }
    ///Creates an instance of Audit with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: int64): Audit = { id = id }

type Extension =
    { ///Date of release
      date: Option<string>
      ///Extension name
      ext_name: Option<string>
      ///Unique identifier for the site
      idx_site: int64
      ///New version
      newVersion: Option<string>
      ///Datetime of the log
      ``type``: string
      ///Author URL
      url: Option<string>
      ///Update is available
      vUpdate: Option<int>
      ///Extension version
      version: Option<string> }
    ///Creates an instance of Extension with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (idx_site: int64, ``type``: string): Extension =
        { date = None
          ext_name = None
          idx_site = idx_site
          newVersion = None
          ``type`` = ``type``
          url = None
          vUpdate = None
          version = None }

type Feedback =
    { ///Unique identifier for the feedback
      id: int64 }
    ///Creates an instance of Feedback with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: int64): Feedback = { id = id }

type Log =
    { ///Name of the site / readyonly
      ``Site name``: Option<string>
      ///Unique identifier for the log
      id_log: int64
      ///Unique identifier for the site
      idx_site: int64
      ///Datetime of the log
      log_date: string
      ///Log information
      log_entry: string
      ///Level of log
      log_level: int64
      ///Type of log
      log_type: Option<string>
      ///Unique identifier for the user
      userid: int64 }
    ///Creates an instance of Log with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id_log: int64,
                          idx_site: int64,
                          log_date: string,
                          log_entry: string,
                          log_level: int64,
                          userid: int64): Log =
        { ``Site name`` = None
          id_log = id_log
          idx_site = idx_site
          log_date = log_date
          log_entry = log_entry
          log_level = log_level
          log_type = None
          userid = userid }

type PostLog =
    { ///Datetime of the log
      log_date: Option<string>
      ///Log information
      log_entry: string
      ///Level of log
      log_level: int64 }
    ///Creates an instance of PostLog with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (log_entry: string, log_level: int64): PostLog =
        { log_date = None
          log_entry = log_entry
          log_level = log_level }

type PostSite =
    { ///URL of the site
      access_url: string
      ///Adminsitration URL
      admin_url: Option<string>
      ///Akeeba Profile
      akeebaProfile: Option<string>
      ///Backup Schedule
      backupSchedule: Option<string>
      ///Date backup
      dateBackup: Option<string>
      ///Friendly name for the site
      name: Option<string>
      ///Personnal note for the site
      notes: Option<string>
      ///Published status of site
      published: Option<bool>
      ///Watchful secret word
      secret_word: Option<string>
      ///JSON encoded array of tags for the site (e.g. [{&amp;lt;q&amp;gt;name&amp;lt;/q&amp;gt;:&amp;lt;q&amp;gt;mytag&amp;lt;/q&amp;gt;},{&amp;lt;q&amp;gt;name&amp;lt;/q&amp;gt;:&amp;lt;q&amp;gt;anothertag&amp;lt;/q&amp;gt;}])
      tags: Option<string>
      ///Akeeba backup word
      word_akeeba: Option<string>
      ///Word checked for uptime
      word_check: Option<string> }
    ///Creates an instance of PostSite with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (access_url: string): PostSite =
        { access_url = access_url
          admin_url = None
          akeebaProfile = None
          backupSchedule = None
          dateBackup = None
          name = None
          notes = None
          published = None
          secret_word = None
          tags = None
          word_akeeba = None
          word_check = None }

type Site =
    { ///URL of the site
      access_url: Option<string>
      ///Adminsitration URL
      admin_url: Option<string>
      ///Akeeba Profile
      akeebaProfile: Option<string>
      ///Backup Schedule
      backupSchedule: Option<string>
      ///Site can be backuped
      canBackup: Option<bool>
      ///Site can use remote installer
      canUpdate: Option<bool>
      ///Date backup
      dateBackup: Option<string>
      ///Watchful Last check
      date_last_check: Option<string>
      ///Error status of site
      error: Option<bool>
      ///server IP
      ip: Option<string>
      ///?
      jUpdate: Option<bool>
      ///Joomla site version
      j_version: Option<string>
      ///Id of the associated UptimeRobot monitor
      monitorid: Option<bool>
      ///Friendly name for the site
      name: Option<string>
      ///Number of updates
      nbUpdates: Option<string>
      ///Joomla site version
      new_j_version: Option<string>
      ///Personnal note for the site
      notes: Option<string>
      ///Published status of site
      published: Option<bool>
      ///Watchful secret word
      secret_word: Option<string>
      ///Unique identifier for the site
      siteid: int64
      ///List of tags for this site
      tags: Option<list<string>>
      ///Site status
      up: Option<bool>
      ///Akeeba backup word
      word_akeeba: Option<string>
      ///Word checked for uptime
      word_check: Option<string> }
    ///Creates an instance of Site with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (siteid: int64): Site =
        { access_url = None
          admin_url = None
          akeebaProfile = None
          backupSchedule = None
          canBackup = None
          canUpdate = None
          dateBackup = None
          date_last_check = None
          error = None
          ip = None
          jUpdate = None
          j_version = None
          monitorid = None
          name = None
          nbUpdates = None
          new_j_version = None
          notes = None
          published = None
          secret_word = None
          siteid = siteid
          tags = None
          up = None
          word_akeeba = None
          word_check = None }

type SsoUsers =
    { ///Email of the SSO User
      email: string
      ///Security Joomla group ID
      groupid: int64
      ///Unique identifier for the SSO User
      id: int64
      ///Last login date on remote site
      lastLoginDate: Option<System.DateTimeOffset>
      ///Site Id of the last remote login
      lastLoginSite: Option<int64>
      ///Account display name
      name: string
      ///Password of the SSO User
      password: string
      ///Watchful user account
      userid: int64
      ///Username of the SSO User
      username: string }
    ///Creates an instance of SsoUsers with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (email: string,
                          groupid: int64,
                          id: int64,
                          name: string,
                          password: string,
                          userid: int64,
                          username: string): SsoUsers =
        { email = email
          groupid = groupid
          id = id
          lastLoginDate = None
          lastLoginSite = None
          name = name
          password = password
          userid = userid
          username = username }

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type Type =
    | [<CompiledName "default">] Default
    | [<CompiledName "success">] Success
    | [<CompiledName "warning">] Warning
    | [<CompiledName "important">] Important
    | [<CompiledName "info">] Info
    | [<CompiledName "inverse">] Inverse
    member this.Format() =
        match this with
        | Default -> "default"
        | Success -> "success"
        | Warning -> "warning"
        | Important -> "important"
        | Info -> "info"
        | Inverse -> "inverse"

type Tag =
    { ///Unique identifier for the tag
      id: int64
      ///Friendly name for the tag
      name: string
      ///Number of sites use this tag (required field id)
      nbSites: Option<int>
      ///Bootstrap color of the tag
      ``type``: Option<Type> }
    ///Creates an instance of Tag with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: int64, name: string): Tag =
        { id = id
          name = name
          nbSites = None
          ``type`` = None }

[<RequireQualifiedAccess>]
type GetAudits =
    ///No response was specified
    | OK of payload: Audit
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type GetFieldsAudits =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type DeleteAuditById =
    ///Audit correctly deleted
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetAuditById =
    ///No response was specified
    | OK of payload: Audit
    ///Invalid ID
    | BadRequest
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type GetExtensions =
    ///No response was specified
    | OK of payload: Extension
    ///Invalid API Key
    | Forbidden
    ///Invalid
    | NotFound

[<RequireQualifiedAccess>]
type GetFieldsExtensions =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type IgnoreExtensionUpdate =
    ///Extension successfully updated
    | OK of payload: string
    ///Update not found for the given extension
    | NotFound

[<RequireQualifiedAccess>]
type UnignoreExtensionUpdate =
    ///Extension successfully updated
    | OK of payload: string
    ///Update not found for the given extension
    | NotFound

[<RequireQualifiedAccess>]
type UpdateExtension =
    ///Extension successfully updated
    | OK of payload: string
    ///Update not found for the given extension
    | NotFound

[<RequireQualifiedAccess>]
type GetFeedbacks =
    ///No response was specified
    | OK of payload: Feedback
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type CreateFeedbacks =
    ///No response was specified
    | OK of payload: Audit
    ///Saved successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type GetFieldsFeedbacks =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type GetLogs =
    ///No response was specified
    | OK of payload: Log
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type GetExportLogs =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type GetFieldsLogs =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type GetTypesLogs =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type DeleteLogById =
    ///Log correctly deleted
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type PostPackages =
    ///No description
    | DefaultResponse

[<RequireQualifiedAccess>]
type GetReportsSitesById =
    ///No response was specified
    | OK of payload: byte []
    ///Invalid API Key
    | Forbidden of payload: byte []
    ///Invalid ID
    | NotFound of payload: byte []

[<RequireQualifiedAccess>]
type GetReportsTagsById =
    ///No response was specified
    | OK of payload: byte []
    ///Invalid API Key
    | Forbidden of payload: byte []
    ///Invalid ID
    | NotFound of payload: byte []

[<RequireQualifiedAccess>]
type GetSites =
    ///No response was specified
    | OK of payload: Site
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type CreateSite =
    ///No response was specified
    | OK of payload: Site
    ///Saved successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Not allowed to add sites
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type GetSitesMetadata =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type DeleteSitesById =
    ///Deleted successfully
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSiteById =
    ///No response was specified
    | OK of payload: Site
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type PutSitesById =
    ///Updated successfully
    | OK of payload: Site
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSiteAudits =
    ///No response was specified
    | OK of payload: list<Audit>
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type CreateAudits =
    ///No response was specified
    | OK of payload: Audit
    ///Saved successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type AddSiteToBackupQueue =
    ///No response was specified
    | OK of payload: Site
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetBackupProfiles =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetListBackups =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type StartSiteBackup =
    ///No response was specified
    | OK of payload: Site
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type StepSiteBackup =
    ///No response was specified
    | OK of payload: Site
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSitesExtensionsById =
    ///No response was specified
    | OK of payload: Extension
    ///Invalid API Key
    | Forbidden
    ///Invalid
    | NotFound

[<RequireQualifiedAccess>]
type InstallExtension =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSitesLogsById =
    ///No response was specified
    | OK of payload: Log
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type CreateLog =
    ///No response was specified
    | OK of payload: Log
    ///Saved successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type DeleteMonitor =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type PostMonitor =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type Scanner =
    ///No response was specified
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type SeoAnalyze =
    ///No response was specified
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSitesTagsById =
    ///No response was specified
    | OK of payload: Tag
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type PostTags =
    ///No response was specified
    | OK of payload: Site
    ///Saved successfully
    | Created
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type UpdateJoomla =
    ///Joomla core successfully updated
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID or Joomla Update not found
    | NotFound

[<RequireQualifiedAccess>]
type GetUptime =
    ///No response was specified
    | OK
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type ValidateSite =
    ///No response was specified
    | OK of payload: Log
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type ValidateDebugSite =
    ///No response was specified
    | OK of payload: Log
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSsoUsers =
    ///No response was specified
    | OK of payload: SsoUsers
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type CreateSsoUsers =
    ///No response was specified
    | OK of payload: SsoUsers
    ///Saved successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type DeleteSsoUserById =
    ///SSO User correctly deleted
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSsoUsersById =
    ///No response was specified
    | OK of payload: SsoUsers
    ///Invalid ID
    | BadRequest
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type UpdateSsoUsers =
    ///No response was specified
    | OK of payload: SsoUsers
    ///Updated successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type GetTags =
    ///No response was specified
    | OK of payload: Tag
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type CreateTags =
    ///No response was specified
    | OK of payload: Tag
    ///Saved successfully
    | Created
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Not saved
    | NotFound

[<RequireQualifiedAccess>]
type GetTagsMetadata =
    ///No response was specified
    | OK of payload: string

[<RequireQualifiedAccess>]
type DeleteTagsById =
    ///Tag correctly deleted
    | OK of payload: string
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetTagById =
    ///No response was specified
    | OK of payload: Tag
    ///Invalid ID
    | BadRequest
    ///Invalid API Key
    | Forbidden

[<RequireQualifiedAccess>]
type UpdateTag =
    ///Updated successfully
    | OK of payload: Tag
    ///Invalid data
    | BadRequest
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound

[<RequireQualifiedAccess>]
type GetSitesByTags =
    ///No response was specified
    | OK of payload: Site
    ///Invalid API Key
    | Forbidden
    ///Invalid ID
    | NotFound
