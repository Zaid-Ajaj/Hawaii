namespace rec Slicebox.Types

type anonymizationData =
    { profile: Option<anonymizationProfile>
      tagValues: Option<list<tagValue>> }
    ///Creates an instance of anonymizationData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): anonymizationData = { profile = None; tagValues = None }

type anonymizationKey =
    { anonPatientID: Option<string>
      anonPatientName: Option<string>
      anonSOPInstanceUID: Option<string>
      anonSeriesInstanceUID: Option<string>
      anonStudyInstanceUID: Option<string>
      created: Option<int64>
      id: Option<int64>
      imageId: Option<int64>
      patientID: Option<string>
      patientName: Option<string>
      seriesInstanceUID: Option<string>
      sopInstanceUID: Option<string>
      studyInstanceUID: Option<string> }
    ///Creates an instance of anonymizationKey with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): anonymizationKey =
        { anonPatientID = None
          anonPatientName = None
          anonSOPInstanceUID = None
          anonSeriesInstanceUID = None
          anonStudyInstanceUID = None
          created = None
          id = None
          imageId = None
          patientID = None
          patientName = None
          seriesInstanceUID = None
          sopInstanceUID = None
          studyInstanceUID = None }

type anonymizationKeyQuery =
    { count: int64
      order: Option<queryOrder>
      queryProperties: list<queryProperty>
      startIndex: int64 }
    ///Creates an instance of anonymizationKeyQuery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (count: int64, queryProperties: list<queryProperty>, startIndex: int64): anonymizationKeyQuery =
        { count = count
          order = None
          queryProperties = queryProperties
          startIndex = startIndex }

type anonymizationKeyValue =
    { anonymizationKeyId: Option<int64>
      anonymizedValue: Option<string>
      id: Option<int64>
      tagPath: Option<tagPathTag>
      value: Option<string> }
    ///Creates an instance of anonymizationKeyValue with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): anonymizationKeyValue =
        { anonymizationKeyId = None
          anonymizedValue = None
          id = None
          tagPath = None
          value = None }

type anonymizationProfile =
    { options: Option<list<confidentialityOption>> }
    ///Creates an instance of anonymizationProfile with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): anonymizationProfile = { options = None }

type box =
    { baseUrl: Option<string>
      id: Option<int64>
      name: Option<string>
      online: Option<bool>
      profile: Option<anonymizationProfile>
      sendMethod: Option<string>
      token: Option<string> }
    ///Creates an instance of box with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): box =
        { baseUrl = None
          id = None
          name = None
          online = None
          profile = None
          sendMethod = None
          token = None }

type bulkAnonymizationData =
    { imageTagValuesSet: Option<list<imageTagValues>>
      profile: Option<anonymizationProfile> }
    ///Creates an instance of bulkAnonymizationData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): bulkAnonymizationData =
        { imageTagValuesSet = None
          profile = None }

type confidentialityOption =
    { description: Option<string>
      name: Option<string>
      rank: Option<int>
      title: Option<string> }
    ///Creates an instance of confidentialityOption with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): confidentialityOption =
        { description = None
          name = None
          rank = None
          title = None }

type destination =
    { destinationId: Option<int64>
      destinationName: Option<string>
      destinationType: Option<string> }
    ///Creates an instance of destination with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): destination =
        { destinationId = None
          destinationName = None
          destinationType = None }

type dicomPropertyValue =
    { value: Option<string> }
    ///Creates an instance of dicomPropertyValue with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): dicomPropertyValue = { value = None }

type exportSetId =
    { value: Option<int64> }
    ///Creates an instance of exportSetId with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): exportSetId = { value = None }

type failedOutgoingTransactionImage =
    { message: Option<string>
      transactionImage: Option<outgoingTransactionImage> }
    ///Creates an instance of failedOutgoingTransactionImage with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): failedOutgoingTransactionImage =
        { message = None
          transactionImage = None }

type filter =
    { id: Option<int64>
      name: Option<string>
      tagFilterType: Option<string>
      tags: Option<list<tagPathTag>> }
    ///Creates an instance of filter with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): filter =
        { id = None
          name = None
          tagFilterType = None
          tags = None }

type flatSeries =
    { id: Option<int64>
      patient: Option<patient>
      series: Option<series>
      study: Option<study> }
    ///Creates an instance of flatSeries with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): flatSeries =
        { id = None
          patient = None
          series = None
          study = None }

type forwardingrule =
    { destination: Option<destination>
      id: Option<int64>
      keepImages: Option<bool>
      source: Option<source> }
    ///Creates an instance of forwardingrule with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): forwardingrule =
        { destination = None
          id = None
          keepImages = None
          source = None }

type idsquery =
    { ids: list<int64> }
    ///Creates an instance of idsquery with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (ids: list<int64>): idsquery = { ids = ids }

type image =
    { id: Option<int64>
      imageType: Option<dicomPropertyValue>
      instanceNumber: Option<dicomPropertyValue>
      seriesId: Option<int64>
      sopInstanceUID: Option<dicomPropertyValue> }
    ///Creates an instance of image with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): image =
        { id = None
          imageType = None
          instanceNumber = None
          seriesId = None
          sopInstanceUID = None }

type imageAttribute =
    { depth: Option<int>
      element: Option<string>
      group: Option<string>
      length: Option<int>
      multiplicity: Option<int>
      name: Option<string>
      path: Option<string>
      value: Option<string>
      vr: Option<string> }
    ///Creates an instance of imageAttribute with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): imageAttribute =
        { depth = None
          element = None
          group = None
          length = None
          multiplicity = None
          name = None
          path = None
          value = None
          vr = None }

type imageInformation =
    { frameIndex: Option<int>
      maximumPixelValue: Option<int>
      minimumPixelValue: Option<int>
      numberOfFrames: Option<int> }
    ///Creates an instance of imageInformation with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): imageInformation =
        { frameIndex = None
          maximumPixelValue = None
          minimumPixelValue = None
          numberOfFrames = None }

type imageTagValues =
    { imageId: Option<int64>
      tagValues: Option<list<tagValue>> }
    ///Creates an instance of imageTagValues with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): imageTagValues = { imageId = None; tagValues = None }

type importSession =
    { created: Option<int64>
      filesAdded: Option<int>
      filesImported: Option<int>
      filesRejected: Option<int>
      id: Option<int64>
      lastUpdated: Option<int64>
      name: Option<string>
      user: Option<string>
      userId: Option<int64> }
    ///Creates an instance of importSession with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): importSession =
        { created = None
          filesAdded = None
          filesImported = None
          filesRejected = None
          id = None
          lastUpdated = None
          name = None
          user = None
          userId = None }

type incomingTransaction =
    { boxId: Option<int64>
      boxName: Option<string>
      id: Option<int64>
      outgoingTransactionId: Option<int64>
      receivedImageCount: Option<int64>
      status: Option<string>
      totalImageCount: Option<int64>
      updated: Option<int64> }
    ///Creates an instance of incomingTransaction with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): incomingTransaction =
        { boxId = None
          boxName = None
          id = None
          outgoingTransactionId = None
          receivedImageCount = None
          status = None
          totalImageCount = None
          updated = None }

type logEntry =
    { created: Option<int64>
      entryType: Option<string>
      id: Option<int64>
      message: Option<string>
      subject: Option<string> }
    ///Creates an instance of logEntry with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): logEntry =
        { created = None
          entryType = None
          id = None
          message = None
          subject = None }

type newUser =
    { password: Option<string>
      role: Option<string>
      user: Option<string> }
    ///Creates an instance of newUser with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): newUser =
        { password = None
          role = None
          user = None }

type outgoingImage =
    { id: Option<int64>
      imageId: Option<int64>
      outgoingTransactionId: Option<int64>
      sent: Option<bool>
      sequenceNumber: Option<int64> }
    ///Creates an instance of outgoingImage with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): outgoingImage =
        { id = None
          imageId = None
          outgoingTransactionId = None
          sent = None
          sequenceNumber = None }

type outgoingTransaction =
    { boxId: Option<int64>
      boxName: Option<string>
      id: Option<int64>
      profile: Option<anonymizationProfile>
      sentImageCount: Option<int64>
      status: Option<string>
      totalImageCount: Option<int64>
      updated: Option<int64> }
    ///Creates an instance of outgoingTransaction with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): outgoingTransaction =
        { boxId = None
          boxName = None
          id = None
          profile = None
          sentImageCount = None
          status = None
          totalImageCount = None
          updated = None }

type outgoingTransactionImage =
    { image: Option<outgoingImage>
      transaction: Option<outgoingTransaction> }
    ///Creates an instance of outgoingTransactionImage with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): outgoingTransactionImage = { image = None; transaction = None }

type patient =
    { id: Option<int64>
      patientBirthDate: Option<dicomPropertyValue>
      patientID: Option<dicomPropertyValue>
      patientName: Option<dicomPropertyValue>
      patientSex: Option<dicomPropertyValue> }
    ///Creates an instance of patient with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): patient =
        { id = None
          patientBirthDate = None
          patientID = None
          patientName = None
          patientSex = None }

type query =
    { count: int64
      filters: Option<queryFilters>
      order: Option<queryOrder>
      queryProperties: list<queryProperty>
      startIndex: int64 }
    ///Creates an instance of query with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (count: int64, queryProperties: list<queryProperty>, startIndex: int64): query =
        { count = count
          filters = None
          order = None
          queryProperties = queryProperties
          startIndex = startIndex }

type queryFilters =
    { seriesTagIds: Option<list<int64>>
      seriesTypeIds: Option<list<int64>>
      sourceRefs: Option<list<sourceRef>> }
    ///Creates an instance of queryFilters with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): queryFilters =
        { seriesTagIds = None
          seriesTypeIds = None
          sourceRefs = None }

type queryOrder =
    { orderAscending: Option<bool>
      orderBy: Option<string> }
    ///Creates an instance of queryOrder with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): queryOrder =
        { orderAscending = None
          orderBy = None }

type queryProperty =
    { operator: Option<string>
      propertyName: Option<string>
      propertyValue: Option<string> }
    ///Creates an instance of queryProperty with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): queryProperty =
        { operator = None
          propertyName = None
          propertyValue = None }

type remoteBox =
    { baseUrl: Option<string>
      defaultProfile: Option<anonymizationProfile>
      name: Option<string> }
    ///Creates an instance of remoteBox with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): remoteBox =
        { baseUrl = None
          defaultProfile = None
          name = None }

type remoteBoxConnectionData =
    { defaultProfile: Option<anonymizationProfile>
      name: Option<string> }
    ///Creates an instance of remoteBoxConnectionData with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): remoteBoxConnectionData = { defaultProfile = None; name = None }

type scp =
    { aeTitle: Option<string>
      id: Option<int64>
      name: Option<string>
      port: Option<int> }
    ///Creates an instance of scp with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): scp =
        { aeTitle = None
          id = None
          name = None
          port = None }

type scu =
    { aeTitle: Option<string>
      host: Option<string>
      id: Option<int64>
      name: Option<string>
      port: Option<int> }
    ///Creates an instance of scu with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): scu =
        { aeTitle = None
          host = None
          id = None
          name = None
          port = None }

type series =
    { bodyPartExamined: Option<dicomPropertyValue>
      frameOfReferenceUID: Option<dicomPropertyValue>
      id: Option<int64>
      manufacturer: Option<dicomPropertyValue>
      modality: Option<dicomPropertyValue>
      protocolName: Option<dicomPropertyValue>
      seriesDate: Option<dicomPropertyValue>
      seriesDescription: Option<dicomPropertyValue>
      seriesInstanceUID: Option<dicomPropertyValue>
      stationName: Option<dicomPropertyValue>
      studyId: Option<int64> }
    ///Creates an instance of series with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): series =
        { bodyPartExamined = None
          frameOfReferenceUID = None
          id = None
          manufacturer = None
          modality = None
          protocolName = None
          seriesDate = None
          seriesDescription = None
          seriesInstanceUID = None
          stationName = None
          studyId = None }

type seriesidseriestype =
    { seriesid: Option<int64>
      seriestype: Option<seriestype> }
    ///Creates an instance of seriesidseriestype with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): seriesidseriestype = { seriesid = None; seriestype = None }

type seriesidseriestypesresult =
    { seriesidseriestypes: Option<list<seriesidseriestype>> }
    ///Creates an instance of seriesidseriestypesresult with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): seriesidseriestypesresult = { seriesidseriestypes = None }

type seriestag =
    { id: Option<int64>
      name: Option<string> }
    ///Creates an instance of seriestag with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): seriestag = { id = None; name = None }

type seriestype =
    { id: Option<int64>
      name: Option<string> }
    ///Creates an instance of seriestype with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): seriestype = { id = None; name = None }

type seriestyperule =
    { id: Option<int64>
      seriesTypeId: Option<int64> }
    ///Creates an instance of seriestyperule with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): seriestyperule = { id = None; seriesTypeId = None }

type seriestyperuleattribute =
    { element: int
      group: int
      id: int64
      path: Option<string>
      seriesTypeRuleId: int64
      value: string }
    ///Creates an instance of seriestyperuleattribute with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (element: int, group: int, id: int64, seriesTypeRuleId: int64, value: string): seriestyperuleattribute =
        { element = element
          group = group
          id = id
          path = None
          seriesTypeRuleId = seriesTypeRuleId
          value = value }

type seriestypeupdatestatus =
    { running: bool }
    ///Creates an instance of seriestypeupdatestatus with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (running: bool): seriestypeupdatestatus = { running = running }

type source =
    { sourceId: Option<int64>
      sourceName: Option<string>
      sourceType: Option<string> }
    ///Creates an instance of source with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): source =
        { sourceId = None
          sourceName = None
          sourceType = None }

type sourceRef =
    { sourceId: Option<int64>
      sourceType: Option<string> }
    ///Creates an instance of sourceRef with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): sourceRef = { sourceId = None; sourceType = None }

type sourceTagFilter =
    { id: Option<int64>
      sourceId: Option<int64>
      sourceType: Option<string>
      tagFilterId: Option<int64> }
    ///Creates an instance of sourceTagFilter with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): sourceTagFilter =
        { id = None
          sourceId = None
          sourceType = None
          tagFilterId = None }

type study =
    { accessionNumber: Option<dicomPropertyValue>
      id: Option<int64>
      patientAge: Option<dicomPropertyValue>
      patientId: Option<int64>
      studyDate: Option<dicomPropertyValue>
      studyDescription: Option<dicomPropertyValue>
      studyID: Option<dicomPropertyValue>
      studyInstanceUID: Option<dicomPropertyValue> }
    ///Creates an instance of study with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): study =
        { accessionNumber = None
          id = None
          patientAge = None
          patientId = None
          studyDate = None
          studyDescription = None
          studyID = None
          studyInstanceUID = None }

type tagMapping =
    { tagPath: Option<tagPathTag>
      value: Option<string> }
    ///Creates an instance of tagMapping with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): tagMapping = { tagPath = None; value = None }

type tagPathTag =
    { previous: Option<tagPathTrunk>
      tag: Option<int> }
    ///Creates an instance of tagPathTag with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): tagPathTag = { previous = None; tag = None }

type tagPathTrunk =
    { item: Option<string>
      previous: Option<tagPathTrunk>
      tag: Option<int> }
    ///Creates an instance of tagPathTrunk with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): tagPathTrunk =
        { item = None
          previous = None
          tag = None }

type tagValue =
    { tagPath: Option<tagPathTag>
      value: Option<string> }
    ///Creates an instance of tagValue with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): tagValue = { tagPath = None; value = None }

type user =
    { hashedPassword: Option<string>
      id: int64
      role: string
      user: string }
    ///Creates an instance of user with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (id: int64, role: string, user: string): user =
        { hashedPassword = None
          id = id
          role = role
          user = user }

type userInfo =
    { id: Option<int64>
      role: Option<string>
      user: Option<string> }
    ///Creates an instance of userInfo with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): userInfo = { id = None; role = None; user = None }

type userPass =
    { pass: Option<string>
      user: Option<string> }
    ///Creates an instance of userPass with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): userPass = { pass = None; user = None }

type watchedDirectory =
    { id: Option<int64>
      path: Option<string> }
    ///Creates an instance of watchedDirectory with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): watchedDirectory = { id = None; path = None }

type PostAnonymizationAnonymizePayloadArrayItem =
    { imageId: Option<int64>
      tagValues: Option<list<tagValue>> }
    ///Creates an instance of PostAnonymizationAnonymizePayloadArrayItem with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PostAnonymizationAnonymizePayloadArrayItem = { imageId = None; tagValues = None }

type PostAnonymizationAnonymizePayload = list<PostAnonymizationAnonymizePayloadArrayItem>

[<RequireQualifiedAccess>]
type PostAnonymizationAnonymize =
    ///the list of newly created anonymous images
    | OK of payload: list<image>

[<RequireQualifiedAccess>]
type GetAnonymizationKeys =
    ///anonymization keys, one per DICOM image
    | OK of payload: list<anonymizationKey>

[<RequireQualifiedAccess>]
type GetAnonymizationKeysExportCsv =
    ///all anonymization keys as a csv file
    | OK of payload: string

[<RequireQualifiedAccess>]
type PostAnonymizationKeysQuery =
    ///anonymization keys
    | OK of payload: list<anonymizationKey>

[<RequireQualifiedAccess>]
type DeleteAnonymizationKeysById =
    ///anonymization key deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetAnonymizationKeysById =
    ///anonymization key for the supplied ID
    | OK of payload: anonymizationKey
    ///if no anonymization key could be found for the supplied ID
    | NotFound

[<RequireQualifiedAccess>]
type GetAnonymizationKeysKeyvaluesById =
    ///an array of anonymization key-value pairs corresponding to the anonymization key for the supplied ID
    | OK of payload: list<anonymizationKeyValue>
    ///if no anonymization key could be found for the supplied ID
    | NotFound

[<RequireQualifiedAccess>]
type GetAnonymizationOptions =
    ///supported anonymization options
    | OK of payload: list<confidentialityOption>

[<RequireQualifiedAccess>]
type GetBoxes =
    ///box connections
    | OK of payload: list<box>

[<RequireQualifiedAccess>]
type PostBoxesConnect =
    ///connected box
    | Created of payload: box

[<RequireQualifiedAccess>]
type PostBoxesCreateconnection =
    ///remote box of the connection
    | Created of payload: box

[<RequireQualifiedAccess>]
type GetBoxesIncoming =
    ///incoming transactions, sorted from most to least recently updated
    | OK of payload: list<incomingTransaction>

[<RequireQualifiedAccess>]
type DeleteBoxesIncomingById =
    ///incoming transaction deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetBoxesIncomingImagesById =
    ///images received corresponding to the specified incoming transaction
    | OK of payload: list<image>
    ///incoming transaction not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetBoxesOutgoing =
    ///outgoing transactions, finished, sending, waiting or failed
    | OK of payload: list<outgoingTransaction>

[<RequireQualifiedAccess>]
type DeleteBoxesOutgoingById =
    ///outgoing transaction deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetBoxesOutgoingImagesById =
    ///images sent corresponding to the specified outgoing transaction
    | OK of payload: list<image>
    ///outgoing transaction not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type DeleteBoxesById =
    ///box deleted
    | NoContent

[<RequireQualifiedAccess>]
type PostBoxesSendById =
    ///images sent
    | Created
    ///box not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetDestinations =
    ///currently available destinations
    | OK of payload: list<destination>

[<RequireQualifiedAccess>]
type GetDirectorywatches =
    ///the list of watched directories
    | OK of payload: list<watchedDirectory>

[<RequireQualifiedAccess>]
type PostDirectorywatches =
    ///the directory now being watched
    | Created of payload: watchedDirectory

[<RequireQualifiedAccess>]
type DeleteDirectorywatchesById =
    ///directory watch removed
    | NoContent

[<RequireQualifiedAccess>]
type GetFilteringAssociations =
    ///the list of source &amp;lt;-&amp;gt; filter associations
    | OK of payload: list<sourceTagFilter>

[<RequireQualifiedAccess>]
type PostFilteringAssociations =
    ///Upserted source &amp;lt;-&amp;gt; filter association
    | Created

[<RequireQualifiedAccess>]
type DeleteFilteringAssociationsById =
    ///source &amp;lt;-&amp;gt; filter association removed
    | NoContent

[<RequireQualifiedAccess>]
type GetFilteringFilters =
    ///the list of filters
    | OK of payload: list<filter>

[<RequireQualifiedAccess>]
type PostFilteringFilters =
    ///Filter upserted
    | Created

[<RequireQualifiedAccess>]
type DeleteFilteringFiltersById =
    ///Filter removed
    | NoContent

[<RequireQualifiedAccess>]
type GetFilteringFiltersTagpathsById =
    ///the list of tagpaths
    | OK of payload: list<tagPathTag>

[<RequireQualifiedAccess>]
type PostFilteringFiltersTagpathsById =
    ///TagPath added
    | Created

[<RequireQualifiedAccess>]
type DeleteFilteringFiltersTagpathsByIdAndTagpathid =
    ///TagPath removed
    | NoContent

[<RequireQualifiedAccess>]
type DeleteForwardingRuleById =
    ///forwarding rule removed
    | NoContent

[<RequireQualifiedAccess>]
type GetForwardingRules =
    ///the list of forwarding rules
    | OK of payload: list<forwardingrule>

[<RequireQualifiedAccess>]
type PostForwardingRules =
    ///the created forwarding rule
    | Created of payload: forwardingrule

[<RequireQualifiedAccess>]
type PostImages =
    ///meta data for added dataset on the image level of the DICOM hierarchy. Status code 200 signifies that this image was already present in the slicebox database.
    | OK of payload: image
    ///meta data for added dataset on the image level of the DICOM hierarchy
    | Created of payload: image

[<RequireQualifiedAccess>]
type PostImagesDelete =
    ///Images deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetImagesExport =
    ///zip archive of images
    | OK of payload: byte []

[<RequireQualifiedAccess>]
type PostImagesExport =
    ///ID of created export set. To be used with the associated GET method for downloading.
    | OK of payload: exportSetId
    ///if the supplied list of image ids is empty or no if images could be found
    | Created

[<RequireQualifiedAccess>]
type PostImagesJpeg =
    ///meta data for added dataset on the image level of the DICOM hierarchy
    | Created of payload: image

[<RequireQualifiedAccess>]
type DeleteImagesById =
    ///image deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetImagesById =
    ///binary data of dataset
    | OK of payload: byte []
    ///if no image was found for the supplied image ID
    | NotFound of payload: byte []

[<RequireQualifiedAccess>]
type PutImagesAnonymizeById =
    ///the newly created anonymous image
    | OK of payload: image
    ///image or corresponding dataset not found
    | NotFound

[<RequireQualifiedAccess>]
type PostImagesAnonymizedById =
    ///binary data of anonymized dataset
    | OK
    ///if no image was found for the supplied image ID
    | NotFound

[<RequireQualifiedAccess>]
type GetImagesAttributesById =
    ///list of DICOM attributes
    | OK of payload: list<imageAttribute>
    ///if no image was found for the supplied image ID
    | NotFound

[<RequireQualifiedAccess>]
type GetImagesImageinformationById =
    ///basic information about the pixeldata of an image
    | OK of payload: imageInformation
    ///if no image was found for the supplied image ID
    | NotFound

type PutImagesModifyByIdPayloadArrayItem =
    { tagPath: Option<tagPathTag>
      value: Option<string> }
    ///Creates an instance of PutImagesModifyByIdPayloadArrayItem with all optional fields initialized to None. The required fields are parameters of this function
    static member Create (): PutImagesModifyByIdPayloadArrayItem = { tagPath = None; value = None }

type PutImagesModifyByIdPayload = list<PutImagesModifyByIdPayloadArrayItem>

[<RequireQualifiedAccess>]
type PutImagesModifyById =
    ///image attributes successfully modified
    | Created

[<RequireQualifiedAccess>]
type GetImagesPngById =
    ///image data
    | OK of payload: byte []
    ///if no image was found for the supplied image ID
    | NotFound of payload: byte []

[<RequireQualifiedAccess>]
type GetImportSessions =
    ///available import sessions
    | OK of payload: list<importSession>

[<RequireQualifiedAccess>]
type PostImportSessions =
    ///the created import session
    | Created of payload: importSession

[<RequireQualifiedAccess>]
type DeleteImportSessionsById =
    ///import session deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetImportSessionsById =
    ///the import session with the supplied ID
    | OK of payload: importSession
    ///import session not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetImportSessionsImagesById =
    ///images corresponding to the specified import session
    | OK of payload: list<image>
    ///import session not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type PostImportSessionsImagesById =
    ///meta data for the imported dataset on the image level of the DICOM hierarchy. Status code 200 signifies that this image was already present in the slicebox database.
    | OK of payload: image
    ///meta data for the imported dataset on the image level of the DICOM hierarchy
    | Created of payload: image
    ///import session not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type DeleteLog =
    ///log messages successfully
    | NoContent

[<RequireQualifiedAccess>]
type GetLog =
    ///log messages
    | OK of payload: list<logEntry>

[<RequireQualifiedAccess>]
type DeleteLogById =
    ///log entry deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetMetadataFlatseries =
    ///flat series
    | OK of payload: list<flatSeries>

[<RequireQualifiedAccess>]
type PostMetadataFlatseriesQuery =
    ///flat series
    | OK of payload: list<flatSeries>

[<RequireQualifiedAccess>]
type GetMetadataFlatseriesById =
    ///flat series response
    | OK of payload: flatSeries
    ///flat series not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataImages =
    ///images
    | OK of payload: list<image>

[<RequireQualifiedAccess>]
type PostMetadataImagesQuery =
    ///images
    | OK of payload: list<image>

[<RequireQualifiedAccess>]
type GetMetadataImagesById =
    ///image response
    | OK of payload: image
    ///image not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataPatients =
    ///patients
    | OK of payload: list<patient>

[<RequireQualifiedAccess>]
type PostMetadataPatientsQuery =
    ///patients
    | OK of payload: list<patient>

[<RequireQualifiedAccess>]
type GetMetadataPatientsById =
    ///patient response
    | OK of payload: patient
    ///patient not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataPatientsImagesById =
    ///list of images
    | OK of payload: list<image>

[<RequireQualifiedAccess>]
type GetMetadataSeries =
    ///series
    | OK of payload: list<series>

[<RequireQualifiedAccess>]
type PostMetadataSeriesQuery =
    ///series
    | OK of payload: list<series>

[<RequireQualifiedAccess>]
type GetMetadataSeriesById =
    ///series response
    | OK of payload: series
    ///series not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataSeriesSeriestagsById =
    ///the list of series tags
    | OK of payload: list<seriestag>
    ///series not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type PostMetadataSeriesSeriestagsById =
    ///added series tag
    | Created of payload: seriestag
    ///if no series with the supplied ID exists
    | NotFound

[<RequireQualifiedAccess>]
type DeleteMetadataSeriesSeriestypesById =
    ///series types deleted
    | NoContent

[<RequireQualifiedAccess>]
type GetMetadataSeriesSeriestypesById =
    ///the list of series types
    | OK of payload: list<seriestype>
    ///series not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataSeriesSourceById =
    ///source for series
    | OK of payload: source
    ///series not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type DeleteMetadataSeriesSeriestagsBySeriesIdAndSeriesTagId =
    ///series tag removed
    | NoContent

[<RequireQualifiedAccess>]
type DeleteMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId =
    ///series type removed
    | NoContent

[<RequireQualifiedAccess>]
type PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId =
    ///series type added
    | NoContent
    ///no series or series type found for the supplied ID(s)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataSeriestags =
    ///a list of unique series tags currently used to tag series
    | OK of payload: list<seriestag>

[<RequireQualifiedAccess>]
type GetMetadataStudies =
    ///studies
    | OK of payload: list<study>

[<RequireQualifiedAccess>]
type PostMetadataStudiesQuery =
    ///studies
    | OK of payload: list<study>

[<RequireQualifiedAccess>]
type GetMetadataStudiesById =
    ///study response
    | OK of payload: study
    ///study not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetMetadataStudiesImagesById =
    ///list of images
    | OK of payload: list<image>
    ///study not found (invalid ID)
    | NotFound

[<RequireQualifiedAccess>]
type GetScps =
    ///the list of SCPs
    | OK of payload: list<scp>

[<RequireQualifiedAccess>]
type PostScps =
    ///the created SCP
    | Created of payload: scp
    ///Invalid port number or AE title
    | BadRequest

[<RequireQualifiedAccess>]
type DeleteScpsById =
    ///SCP removed
    | NoContent

[<RequireQualifiedAccess>]
type GetScus =
    ///the list of SCUs
    | OK of payload: list<scu>

[<RequireQualifiedAccess>]
type PostScus =
    ///the created SCU
    | Created of payload: scu
    ///Invalid port number or AE title
    | BadRequest

[<RequireQualifiedAccess>]
type DeleteScusById =
    ///SCU removed
    | NoContent

[<RequireQualifiedAccess>]
type PostScusSendById =
    ///Series sent
    | NoContent
    ///Series not found or SCU not found
    | NotFound

[<RequireQualifiedAccess>]
type GetSeriestypes =
    ///the list of series types
    | OK of payload: list<seriestype>

[<RequireQualifiedAccess>]
type PostSeriestypes =
    ///the created series type
    | Created of payload: seriestype

[<RequireQualifiedAccess>]
type GetSeriestypesRules =
    ///the list of series type rules for the series type with the supplied ID
    | OK of payload: list<seriestyperule>

[<RequireQualifiedAccess>]
type PostSeriestypesRules =
    ///the created series type rule
    | Created of payload: seriestyperule

[<RequireQualifiedAccess>]
type GetSeriestypesRulesUpdatestatus =
    ///a status message, indicating if an update is running
    | OK of payload: seriestypeupdatestatus

[<RequireQualifiedAccess>]
type DeleteSeriestypesRulesById =
    ///series type rule removed
    | NoContent

[<RequireQualifiedAccess>]
type GetSeriestypesRulesAttributesById =
    ///the list of series type rule attributes for the series type rule with the supplied ID
    | OK of payload: list<seriestyperuleattribute>

[<RequireQualifiedAccess>]
type PostSeriestypesRulesAttributesById =
    ///the created series type rule attribute
    | Created of payload: seriestyperuleattribute

[<RequireQualifiedAccess>]
type DeleteSeriestypesRulesAttributesByRuleIdAndAttributeId =
    ///series type rule attribute removed
    | NoContent

[<RequireQualifiedAccess>]
type PostSeriestypesSeriesQuery =
    ///series
    | OK of payload: seriesidseriestypesresult

[<RequireQualifiedAccess>]
type DeleteSeriestypesById =
    ///series type removed
    | NoContent

[<RequireQualifiedAccess>]
type PutSeriestypesById =
    ///update successfully added to queue of series type updates
    | NoContent

[<RequireQualifiedAccess>]
type GetSources =
    ///currently available sources
    | OK of payload: list<source>

[<RequireQualifiedAccess>]
type GetSystemHealth =
    ///The service is up and running
    | OK

[<RequireQualifiedAccess>]
type PostSystemStop =
    ///shutdown message
    | OK

[<RequireQualifiedAccess>]
type PostTransactionsImageByToken =
    ///image data received
    | NoContent
    ///unauthorized, invalid token
    | Unauthorized

[<RequireQualifiedAccess>]
type GetTransactionsOutgoingByToken =
    ///binary data of dataset
    | OK of payload: byte []
    ///unauthorized, invalid token
    | Unauthorized of payload: byte []
    ///no outgoing trensaction and/or image found for the supplied transaction id and transaction image id
    | NotFound of payload: byte []

[<RequireQualifiedAccess>]
type PostTransactionsOutgoingDoneByToken =
    ///done message received
    | NoContent
    ///unauthorized, invalid token
    | Unauthorized

[<RequireQualifiedAccess>]
type PostTransactionsOutgoingFailedByToken =
    ///failed message received
    | NoContent
    ///unauthorized, invalid token
    | Unauthorized

[<RequireQualifiedAccess>]
type GetTransactionsOutgoingPollByToken =
    ///next outgoing transaction and image information block
    | OK of payload: list<outgoingTransactionImage>
    ///unauthorized, invalid token
    | Unauthorized
    ///there are currently no outgoing transactions to fetch for the box connection with the supplied token
    | NotFound

[<RequireQualifiedAccess>]
type GetTransactionsStatusByToken =
    ///string representation of the transaction status (FINISHED, FAILED, WAITING or PROCESSING)
    | OK
    ///unauthorized, invalid token
    | Unauthorized
    ///no transaction found for the supplied transaction ID and box token
    | NotFound

[<RequireQualifiedAccess>]
type PutTransactionsStatusByToken =
    ///status update successfully applied to transaction
    | NoContent
    ///no transaction found for the supplied transaction ID and box token
    | NotFound

[<RequireQualifiedAccess>]
type GetUsers =
    ///user response
    | OK of payload: list<user>

[<RequireQualifiedAccess>]
type PostUsers =
    ///user response
    | Created of payload: user

[<RequireQualifiedAccess>]
type GetUsersCurrent =
    ///user information
    | OK of payload: userInfo
    ///no user found for the supplied session cookie, IP address and user agent, or if any of the required headers are missing.
    | NotFound

[<RequireQualifiedAccess>]
type PostUsersLogin =
    ///if the supplied credentials are valid. The response headers will contain Set-Cookie.
    | Created
    ///if the supplied credentials are invalid.
    | Unauthorized

[<RequireQualifiedAccess>]
type PostUsersLogout =
    ///the user was logged out
    | Created

[<RequireQualifiedAccess>]
type DeleteUsersById =
    ///user deleted
    | NoContent
