namespace rec TaskSlicebox

open System.Net
open System.Net.Http
open System.Text
open System.Threading
open TaskSlicebox.Types
open TaskSlicebox.Http
open FSharp.Control.Tasks

///Slicebox - safe sharing of medical images
type TaskSliceboxClient(httpClient: HttpClient) =
    ///<summary>
    ///anonymize the images corresponding to the supplied list of image IDs (each paired with a list of DICOM tag translation). This route corresponds to repeated use of the route /images/{id}/anonymize.
    ///</summary>
    member this.PostAnonymizationAnonymize
        (
            query: PostAnonymizationAnonymizePayload,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/anonymization/anonymize" requestParts cancellationToken

            return PostAnonymizationAnonymize.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of anonymization keys, each specifying how vital DICOM attributes have been anonymized for a particular image
    ///</summary>
    ///<param name="startindex">start index of returned slice of anonymization keys</param>
    ///<param name="count">size of returned slice of anonymization keys</param>
    ///<param name="orderby">property to order results by</param>
    ///<param name="orderascending">order result ascendingly if true, descendingly otherwise</param>
    ///<param name="filter">filter the results by matching substrings of properties against this value</param>
    ///<param name="cancellationToken"></param>
    member this.GetAnonymizationKeys
        (
            ?startindex: int64,
            ?count: int64,
            ?orderby: string,
            ?orderascending: bool,
            ?filter: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("orderby", orderby.Value)
                  if orderascending.IsSome then
                      RequestPart.query ("orderascending", orderascending.Value)
                  if filter.IsSome then
                      RequestPart.query ("filter", filter.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/anonymization/keys" requestParts cancellationToken

            return GetAnonymizationKeys.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///export all anonymization keys as a csv file
    ///</summary>
    member this.GetAnonymizationKeysExportCsv(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/anonymization/keys/export/csv" requestParts cancellationToken

            return GetAnonymizationKeysExportCsv.OK content
        }

    ///<summary>
    ///submit a query for anonymization keys
    ///</summary>
    member this.PostAnonymizationKeysQuery(query: anonymizationKeyQuery, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/anonymization/keys/query" requestParts cancellationToken

            return PostAnonymizationKeysQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///delete an anonymization key that is no longer of interest
    ///</summary>
    ///<param name="id">ID of anonymization key</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteAnonymizationKeysById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/anonymization/keys/{id}" requestParts cancellationToken

            return DeleteAnonymizationKeysById.NoContent
        }

    ///<summary>
    ///get the anonymization key with the supplied ID
    ///</summary>
    ///<param name="id">ID of anonymization key</param>
    ///<param name="cancellationToken"></param>
    member this.GetAnonymizationKeysById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/anonymization/keys/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetAnonymizationKeysById.OK(Serializer.deserialize content)
            else
                return GetAnonymizationKeysById.NotFound
        }

    ///<summary>
    ///get pointers to the images corresponding to the anonymization key with the supplied ID
    ///</summary>
    ///<param name="id">ID of anonymization key</param>
    ///<param name="cancellationToken"></param>
    member this.GetAnonymizationKeysKeyvaluesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/anonymization/keys/{id}/keyvalues" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetAnonymizationKeysKeyvaluesById.OK(Serializer.deserialize content)
            else
                return GetAnonymizationKeysKeyvaluesById.NotFound
        }

    ///<summary>
    ///list all supported anonymization options defining an anonymization profile
    ///</summary>
    member this.GetAnonymizationOptions(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/anonymization/options" requestParts cancellationToken

            return GetAnonymizationOptions.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of box connections
    ///</summary>
    ///<param name="startindex">start index of returned slice of boxes</param>
    ///<param name="count">size of returned slice of boxes</param>
    ///<param name="cancellationToken"></param>
    member this.GetBoxes(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/boxes" requestParts cancellationToken
            return GetBoxes.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///connect to another box using a received URL. Used to connect to a public box.
    ///</summary>
    member this.PostBoxesConnect(remoteBox: remoteBox, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent remoteBox ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/boxes/connect" requestParts cancellationToken
            return PostBoxesConnect.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///create a new box connection where the supplied entity holds the remote box name. Used by publicly available boxes.
    ///</summary>
    member this.PostBoxesCreateconnection
        (
            remoteBoxConnectionData: remoteBoxConnectionData,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.jsonContent remoteBoxConnectionData ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/boxes/createconnection" requestParts cancellationToken

            return PostBoxesCreateconnection.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///get incoming transactions (finished, currently receiving, waiting or failed)
    ///</summary>
    ///<param name="startindex">start index of returned slice of transactions</param>
    ///<param name="count">size of returned slice of transactions</param>
    ///<param name="cancellationToken"></param>
    member this.GetBoxesIncoming(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/boxes/incoming" requestParts cancellationToken
            return GetBoxesIncoming.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///delete an incoming transaction. If a currently active transaction is deleted, a new transaction with the remainder of the images is created when receiving the next incoming image.
    ///</summary>
    ///<param name="id">ID of incoming transaction</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteBoxesIncomingById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/boxes/incoming/{id}" requestParts cancellationToken

            return DeleteBoxesIncomingById.NoContent
        }

    ///<summary>
    ///get the received images corresponding to the incoming transaction with the supplied ID
    ///</summary>
    ///<param name="id">ID of incoming transaction</param>
    ///<param name="cancellationToken"></param>
    member this.GetBoxesIncomingImagesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/boxes/incoming/{id}/images" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetBoxesIncomingImagesById.OK(Serializer.deserialize content)
            else
                return GetBoxesIncomingImagesById.NotFound
        }

    ///<summary>
    ///get outgoing transactions (finished, currently sending, waiting or failed)
    ///</summary>
    ///<param name="startindex">start index of returned slice of transactions</param>
    ///<param name="count">size of returned slice of transactions</param>
    ///<param name="cancellationToken"></param>
    member this.GetBoxesOutgoing(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/boxes/outgoing" requestParts cancellationToken
            return GetBoxesOutgoing.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///delete an outgoing transaction. This will stop ongoing transactions.
    ///</summary>
    ///<param name="id">ID of outgoing transaction</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteBoxesOutgoingById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/boxes/outgoing/{id}" requestParts cancellationToken

            return DeleteBoxesOutgoingById.NoContent
        }

    ///<summary>
    ///get the sent images corresponding to the outgoing transaction with the supplied ID
    ///</summary>
    ///<param name="id">ID of outgoing transaction</param>
    ///<param name="cancellationToken"></param>
    member this.GetBoxesOutgoingImagesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/boxes/outgoing/{id}/images" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetBoxesOutgoingImagesById.OK(Serializer.deserialize content)
            else
                return GetBoxesOutgoingImagesById.NotFound
        }

    ///<summary>
    ///Delete the remote box with the supplied ID
    ///</summary>
    ///<param name="id">ID of box to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteBoxesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/boxes/{id}" requestParts cancellationToken
            return DeleteBoxesById.NoContent
        }

    ///<summary>
    ///send images corresponding to the supplied image ids to the remote box with the supplied ID
    ///</summary>
    ///<param name="id">ID of box to send images to</param>
    ///<param name="sequence of image tag values"></param>
    ///<param name="cancellationToken"></param>
    member this.PostBoxesSendById
        (
            id: int64,
            ``sequence of image tag values``: bulkAnonymizationData,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``sequence of image tag values`` ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/boxes/{id}/send" requestParts cancellationToken

            if status = HttpStatusCode.Created then
                return PostBoxesSendById.Created
            else
                return PostBoxesSendById.NotFound
        }

    ///<summary>
    ///Returns a list of currently available destinations. Possible destinations are box - sending data to a remote box, and scu - sending data a receiving SCP.
    ///</summary>
    member this.GetDestinations(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/destinations" requestParts cancellationToken
            return GetDestinations.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of watch directories. Each watch directory and its sub-directories are watched for incoming DICOM files, which are read and imported into slicebox.
    ///</summary>
    ///<param name="startindex">start index of returned slice of watched directories</param>
    ///<param name="count">size of returned slice of watched directories</param>
    ///<param name="cancellationToken"></param>
    member this.GetDirectorywatches(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/directorywatches" requestParts cancellationToken
            return GetDirectorywatches.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new directory to watch for incoming DICOM files
    ///</summary>
    member this.PostDirectorywatches(watchedDirectory: watchedDirectory, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent watchedDirectory ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/directorywatches" requestParts cancellationToken
            return PostDirectorywatches.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///stop watching and remove the directory corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of directory to stop watching</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteDirectorywatchesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/directorywatches/{id}" requestParts cancellationToken

            return DeleteDirectorywatchesById.NoContent
        }

    ///<summary>
    ///Get a list of source to filter associations.
    ///</summary>
    ///<param name="startindex">start index of returned slice of source &amp;lt;-&amp;gt; filter associations</param>
    ///<param name="count">size of returned slice of source &amp;lt;-&amp;gt; filter associations</param>
    ///<param name="cancellationToken"></param>
    member this.GetFilteringAssociations(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/filtering/associations" requestParts cancellationToken

            return GetFilteringAssociations.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Inserts or updates a source &amp;lt;-&amp;gt; filter associations. If the specified Source already  has an association this is updated, otherwise a new is inserted.
    ///</summary>
    member this.PostFilteringAssociations(sourcetagfilter: sourceTagFilter, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent sourcetagfilter ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/filtering/associations" requestParts cancellationToken

            return PostFilteringAssociations.Created
        }

    ///<summary>
    ///remove the source &amp;lt;-&amp;gt; filter association corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of source &amp;lt;-&amp;gt; filter association to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteFilteringAssociationsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/filtering/associations/{id}" requestParts cancellationToken

            return DeleteFilteringAssociationsById.NoContent
        }

    ///<summary>
    ///List defined filters
    ///</summary>
    ///<param name="startindex">start index of returned slice of filters</param>
    ///<param name="count">size of returned slice of filters</param>
    ///<param name="cancellationToken"></param>
    member this.GetFilteringFilters(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/filtering/filters" requestParts cancellationToken
            return GetFilteringFilters.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Inserts or updates a filter. If a filter with same name as supplied filter exists this filter is updated, otherwise a new filter is inserted.
    ///</summary>
    member this.PostFilteringFilters(tagFilter: filter, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent tagFilter ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/filtering/filters" requestParts cancellationToken

            return PostFilteringFilters.Created
        }

    ///<summary>
    ///remove the filter corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of filter to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteFilteringFiltersById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/filtering/filters/{id}" requestParts cancellationToken

            return DeleteFilteringFiltersById.NoContent
        }

    ///<summary>
    ///List tagpaths for the selected filter
    ///</summary>
    ///<param name="id">id of filter</param>
    ///<param name="cancellationToken"></param>
    member this.GetFilteringFiltersTagpathsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/filtering/filters/{id}/tagpaths" requestParts cancellationToken

            return GetFilteringFiltersTagpathsById.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a tagpath to a filter
    ///</summary>
    ///<param name="id">id of filter to remove</param>
    ///<param name="tagpath"></param>
    ///<param name="cancellationToken"></param>
    member this.PostFilteringFiltersTagpathsById
        (
            id: int64,
            tagpath: tagPathTag,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent tagpath ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/filtering/filters/{id}/tagpaths" requestParts cancellationToken

            return PostFilteringFiltersTagpathsById.Created
        }

    ///<summary>
    ///remove the tagpath corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of filter</param>
    ///<param name="tagpathid">id of TagPath to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteFilteringFiltersTagpathsByIdAndTagpathid
        (
            id: int64,
            tagpathid: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.path ("tagpathid", tagpathid) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    httpClient
                    "/filtering/filters/{id}/tagpaths/{tagpathid}"
                    requestParts
                    cancellationToken

            return DeleteFilteringFiltersTagpathsByIdAndTagpathid.NoContent
        }

    ///<summary>
    ///remove the forwarding rule corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of forwarding rule to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteForwardingRuleById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/forwarding/rule/{id}" requestParts cancellationToken

            return DeleteForwardingRuleById.NoContent
        }

    ///<summary>
    ///get a list of all forwarding rules. A forwarding rule specifies the automatic forwarding of images from a source (SCP, BOX, etc.) to a destimation (BOX, SCU, etc.)
    ///</summary>
    ///<param name="startindex">start index of returned slice of rules</param>
    ///<param name="count">size of returned slice of rules</param>
    ///<param name="cancellationToken"></param>
    member this.GetForwardingRules(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/forwarding/rules" requestParts cancellationToken
            return GetForwardingRules.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new forwarding rule
    ///</summary>
    member this.PostForwardingRules(fowardingRule: forwardingrule, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent fowardingRule ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/forwarding/rules" requestParts cancellationToken
            return PostForwardingRules.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///add a DICOM dataset to slicebox
    ///</summary>
    member this.PostImages(body: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/images" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PostImages.OK(Serializer.deserialize content)
            else
                return PostImages.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///bulk delete a sequence of images according to the supplied image IDs. This is the same as a sequence of DELETE requests to /images/{id}
    ///</summary>
    member this.PostImagesDelete(``image IDs``: list<int64>, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent ``image IDs`` ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/images/delete" requestParts cancellationToken
            return PostImagesDelete.NoContent
        }

    ///<summary>
    ///download the export set with the supplied export set ID as a zip archive
    ///</summary>
    ///<param name="id">ID of export set to download</param>
    ///<param name="cancellationToken"></param>
    member this.GetImagesExport(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.query ("id", id) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/images/export" requestParts cancellationToken

            return GetImagesExport.OK contentBinary
        }

    ///<summary>
    ///create an export set, a group of image IDs of images to export. The export set will contain the selected images. The export set is available for download 12 hours before it is automatically deleted.
    ///</summary>
    member this.PostImagesExport(``image ids``: list<int64>, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent ``image ids`` ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/images/export" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PostImagesExport.OK(Serializer.deserialize content)
            else
                return PostImagesExport.Created
        }

    ///<summary>
    ///add a JPEG image to slicebox. The image data will be wrapped in a DICOM file and added as a new series belonging to the study with the supplied ID
    ///</summary>
    ///<param name="studyid">ID of study to add new series to</param>
    ///<param name="description">DICOM series description of the resulting secondary capture series</param>
    ///<param name="cancellationToken"></param>
    ///<param name="requestBody"></param>
    member this.PostImagesJpeg
        (
            studyid: int64,
            ?description: string,
            ?cancellationToken: CancellationToken,
            ?requestBody: byte []
        ) =
        task {
            let requestParts =
                [ RequestPart.query ("studyid", studyid)
                  if description.IsSome then
                      RequestPart.query ("description", description.Value)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/images/jpeg" requestParts cancellationToken
            return PostImagesJpeg.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete the image with the supplied ID
    ///</summary>
    ///<param name="id">ID of image</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteImagesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/images/{id}" requestParts cancellationToken
            return DeleteImagesById.NoContent
        }

    ///<summary>
    ///fetch dataset corresponding to the supplied image ID
    ///</summary>
    ///<param name="id">ID of image</param>
    ///<param name="cancellationToken"></param>
    member this.GetImagesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/images/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetImagesById.OK contentBinary
            else
                return GetImagesById.NotFound contentBinary
        }

    ///<summary>
    ///delete the selected image and replace it with an anonymized version
    ///</summary>
    ///<param name="id">ID of image to anonymize</param>
    ///<param name="tag values"></param>
    ///<param name="cancellationToken"></param>
    member this.PutImagesAnonymizeById
        (
            id: int64,
            ``tag values``: anonymizationData,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``tag values`` ]

            let! (status, content) =
                OpenApiHttp.putAsync httpClient "/images/{id}/anonymize" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PutImagesAnonymizeById.OK(Serializer.deserialize content)
            else
                return PutImagesAnonymizeById.NotFound
        }

    ///<summary>
    ///get an anonymized version of the image with the supplied ID
    ///</summary>
    ///<param name="id">ID of image for which to get anonymized dataset</param>
    ///<param name="tag values"></param>
    ///<param name="cancellationToken"></param>
    member this.PostImagesAnonymizedById
        (
            id: int64,
            ``tag values``: anonymizationData,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``tag values`` ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/images/{id}/anonymized" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PostImagesAnonymizedById.OK
            else
                return PostImagesAnonymizedById.NotFound
        }

    ///<summary>
    ///list all DICOM attributes of the dataset corresponding to the supplied image ID
    ///</summary>
    ///<param name="id">ID of image</param>
    ///<param name="cancellationToken"></param>
    member this.GetImagesAttributesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/images/{id}/attributes" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetImagesAttributesById.OK(Serializer.deserialize content)
            else
                return GetImagesAttributesById.NotFound
        }

    ///<summary>
    ///get basic information about the pixel data of an image
    ///</summary>
    ///<param name="id">ID of image</param>
    ///<param name="cancellationToken"></param>
    member this.GetImagesImageinformationById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/images/{id}/imageinformation" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetImagesImageinformationById.OK(Serializer.deserialize content)
            else
                return GetImagesImageinformationById.NotFound
        }

    ///<summary>
    ///modify and/or insert image attributes according to the input tagpath-value mappings
    ///</summary>
    ///<param name="id">ID of image to modify</param>
    ///<param name="tag path value mappings"></param>
    ///<param name="cancellationToken"></param>
    member this.PutImagesModifyById
        (
            id: int64,
            ``tag path value mappings``: PutImagesModifyByIdPayload,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``tag path value mappings`` ]

            let! (status, content) =
                OpenApiHttp.putAsync httpClient "/images/{id}/modify" requestParts cancellationToken

            return PutImagesModifyById.Created
        }

    ///<summary>
    ///get a PNG image representation of the image corresponding to the supplied ID
    ///</summary>
    ///<param name="id">ID of image</param>
    ///<param name="framenumber">frame/slice to show</param>
    ///<param name="windowmin">intensity window minimum value. If not specified or set to zero, windowing will be selected from relevant DICOM attributes</param>
    ///<param name="windowmax">intensity window maximum value. If not specified or set to zero, windowing will be selected from relevant DICOM attributes</param>
    ///<param name="imageheight">height of PNG image. If not specified or set to zero, the image height will equal that of the data</param>
    ///<param name="cancellationToken"></param>
    member this.GetImagesPngById
        (
            id: int64,
            ?framenumber: int,
            ?windowmin: int,
            ?windowmax: int,
            ?imageheight: int,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if framenumber.IsSome then
                      RequestPart.query ("framenumber", framenumber.Value)
                  if windowmin.IsSome then
                      RequestPart.query ("windowmin", windowmin.Value)
                  if windowmax.IsSome then
                      RequestPart.query ("windowmax", windowmax.Value)
                  if imageheight.IsSome then
                      RequestPart.query ("imageheight", imageheight.Value) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/images/{id}/png" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetImagesPngById.OK contentBinary
            else
                return GetImagesPngById.NotFound contentBinary
        }

    ///<summary>
    ///Returns a list of available import sessions.
    ///</summary>
    ///<param name="startindex">start index of returned slice of import sessions</param>
    ///<param name="count">size of returned slice of import sessions</param>
    ///<param name="cancellationToken"></param>
    member this.GetImportSessions(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/import/sessions" requestParts cancellationToken
            return GetImportSessions.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///create a new import sessions
    ///</summary>
    member this.PostImportSessions(``import session``: importSession, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent ``import session`` ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/import/sessions" requestParts cancellationToken
            return PostImportSessions.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///deletes the import session with the supplied ID
    ///</summary>
    ///<param name="id">ID of import session to delete</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteImportSessionsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/import/sessions/{id}" requestParts cancellationToken

            return DeleteImportSessionsById.NoContent
        }

    ///<summary>
    ///Returns the import sessions with the supplied ID
    ///</summary>
    ///<param name="id">ID of session</param>
    ///<param name="cancellationToken"></param>
    member this.GetImportSessionsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/import/sessions/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetImportSessionsById.OK(Serializer.deserialize content)
            else
                return GetImportSessionsById.NotFound
        }

    ///<summary>
    ///get the imported images corresponding to the import session with the supplied ID
    ///</summary>
    ///<param name="id">ID of import session</param>
    ///<param name="cancellationToken"></param>
    member this.GetImportSessionsImagesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/import/sessions/{id}/images" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetImportSessionsImagesById.OK(Serializer.deserialize content)
            else
                return GetImportSessionsImagesById.NotFound
        }

    ///<summary>
    ///add a DICOM dataset to the import session with the supplied ID
    ///</summary>
    ///<param name="id">ID of session</param>
    ///<param name="body"></param>
    ///<param name="cancellationToken"></param>
    member this.PostImportSessionsImagesById(id: int64, body: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/import/sessions/{id}/images" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return PostImportSessionsImagesById.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Created then
                return PostImportSessionsImagesById.Created(Serializer.deserialize content)
            else
                return PostImportSessionsImagesById.NotFound
        }

    ///<summary>
    ///delete all log messages
    ///</summary>
    member this.DeleteLog(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/log" requestParts cancellationToken
            return DeleteLog.NoContent
        }

    ///<summary>
    ///get a list of slicebox log messages
    ///</summary>
    ///<param name="startindex">start index of returned slice of log messages</param>
    ///<param name="count">size of returned slice of log messages</param>
    ///<param name="subject">log subject to filter results by</param>
    ///<param name="type">log type (DEFAULT, INFO, WARN, ERROR) to filter results by</param>
    ///<param name="cancellationToken"></param>
    member this.GetLog
        (
            ?startindex: int64,
            ?count: int64,
            ?subject: string,
            ?``type``: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if subject.IsSome then
                      RequestPart.query ("subject", subject.Value)
                  if ``type``.IsSome then
                      RequestPart.query ("type", ``type``.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/log" requestParts cancellationToken
            return GetLog.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete the log entry with the supplied ID
    ///</summary>
    ///<param name="id">ID of log entry</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteLogById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/log/{id}" requestParts cancellationToken
            return DeleteLogById.NoContent
        }

    ///<summary>
    ///Returns a list of flattened metadata on the patient, study and series levels
    ///</summary>
    ///<param name="startindex">start index of returned slice of flat series</param>
    ///<param name="count">size of returned slice of flat series</param>
    ///<param name="orderby">flat series property to order results by</param>
    ///<param name="orderascending">order result ascendingly if true, descendingly otherwise</param>
    ///<param name="filter">filter the results by matching substrings of flat series properties against this value</param>
    ///<param name="sources">filter the results by matching on one or more series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows series with either of the series tags with ids 6, 2 and 11.</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataFlatseries
        (
            ?startindex: int64,
            ?count: int64,
            ?orderby: string,
            ?orderascending: bool,
            ?filter: string,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("orderby", orderby.Value)
                  if orderascending.IsSome then
                      RequestPart.query ("orderascending", orderascending.Value)
                  if filter.IsSome then
                      RequestPart.query ("filter", filter.Value)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/flatseries" requestParts cancellationToken

            return GetMetadataFlatseries.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for flat series
    ///</summary>
    member this.PostMetadataFlatseriesQuery(query: query, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/metadata/flatseries/query" requestParts cancellationToken

            return PostMetadataFlatseriesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the flat series with the supplied ID
    ///</summary>
    ///<param name="id">ID of flat series</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataFlatseriesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/flatseries/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataFlatseriesById.OK(Serializer.deserialize content)
            else
                return GetMetadataFlatseriesById.NotFound
        }

    ///<summary>
    ///Returns a list of metadata on the image level of the DICOM hierarchy
    ///</summary>
    ///<param name="seriesid">reference to series to list images for</param>
    ///<param name="startindex">start index of returned slice of images</param>
    ///<param name="count">size of returned slice of images</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataImages
        (
            seriesid: int64,
            ?startindex: int64,
            ?count: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.query ("seriesid", seriesid)
                  if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/metadata/images" requestParts cancellationToken
            return GetMetadataImages.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for images
    ///</summary>
    member this.PostMetadataImagesQuery(query: query, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/metadata/images/query" requestParts cancellationToken

            return PostMetadataImagesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the image with the supplied ID
    ///</summary>
    ///<param name="id">ID of image</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataImagesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/images/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataImagesById.OK(Serializer.deserialize content)
            else
                return GetMetadataImagesById.NotFound
        }

    ///<summary>
    ///Returns a list of metadata on the patient level of the DICOM hierarchy
    ///</summary>
    ///<param name="startindex">start index of returned slice of patients</param>
    ///<param name="count">size of returned slice of patients</param>
    ///<param name="orderby">patient property to order results by</param>
    ///<param name="orderascending">order result ascendingly if true, descendingly otherwise</param>
    ///<param name="filter">filter the results by matching substrings of patient properties against this value</param>
    ///<param name="sources">filter the results by matching on one or more underlying series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more underlying series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows results including series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more underlying series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows results including series with either of the series tags with ids 6, 2 and 11.</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataPatients
        (
            ?startindex: int64,
            ?count: int64,
            ?orderby: string,
            ?orderascending: bool,
            ?filter: string,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if orderby.IsSome then
                      RequestPart.query ("orderby", orderby.Value)
                  if orderascending.IsSome then
                      RequestPart.query ("orderascending", orderascending.Value)
                  if filter.IsSome then
                      RequestPart.query ("filter", filter.Value)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/metadata/patients" requestParts cancellationToken
            return GetMetadataPatients.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for patients
    ///</summary>
    member this.PostMetadataPatientsQuery(query: query, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/metadata/patients/query" requestParts cancellationToken

            return PostMetadataPatientsQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the patient with the supplied ID
    ///</summary>
    ///<param name="id">ID of patient</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataPatientsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/patients/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataPatientsById.OK(Serializer.deserialize content)
            else
                return GetMetadataPatientsById.NotFound
        }

    ///<summary>
    ///Returns all images for the patient with the supplied patient ID
    ///</summary>
    ///<param name="id">ID of patient</param>
    ///<param name="sources">filter the results by matching on one or more series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows series with either of the series tags with ids 6, 2 and 11.</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataPatientsImagesById
        (
            id: int64,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/patients/{id}/images" requestParts cancellationToken

            return GetMetadataPatientsImagesById.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Returns a list of metadata on the series level of the DICOM hierarchy
    ///</summary>
    ///<param name="studyid">reference to study to list series for</param>
    ///<param name="startindex">start index of returned slice of series</param>
    ///<param name="count">size of returned slice of series</param>
    ///<param name="sources">filter the results by matching on one or more series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows series with either of the series tags with ids 6, 2 and 11.</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataSeries
        (
            studyid: int64,
            ?startindex: int64,
            ?count: int64,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.query ("studyid", studyid)
                  if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/metadata/series" requestParts cancellationToken
            return GetMetadataSeries.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for series
    ///</summary>
    member this.PostMetadataSeriesQuery(query: query, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/metadata/series/query" requestParts cancellationToken

            return PostMetadataSeriesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataSeriesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/series/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataSeriesById.OK(Serializer.deserialize content)
            else
                return GetMetadataSeriesById.NotFound
        }

    ///<summary>
    ///get the list of series tags for the series with the supplied ID.
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataSeriesSeriestagsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/series/{id}/seriestags" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataSeriesSeriestagsById.OK(Serializer.deserialize content)
            else
                return GetMetadataSeriesSeriestagsById.NotFound
        }

    ///<summary>
    ///add a series tag to the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="query"></param>
    ///<param name="cancellationToken"></param>
    member this.PostMetadataSeriesSeriestagsById(id: int64, query: seriestag, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/metadata/series/{id}/seriestags" requestParts cancellationToken

            if status = HttpStatusCode.Created then
                return PostMetadataSeriesSeriestagsById.Created(Serializer.deserialize content)
            else
                return PostMetadataSeriesSeriestagsById.NotFound
        }

    ///<summary>
    ///Delete all series types for the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteMetadataSeriesSeriestypesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/metadata/series/{id}/seriestypes" requestParts cancellationToken

            return DeleteMetadataSeriesSeriestypesById.NoContent
        }

    ///<summary>
    ///get the list of series types for the series with the supplied ID.
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataSeriesSeriestypesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/series/{id}/seriestypes" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataSeriesSeriestypesById.OK(Serializer.deserialize content)
            else
                return GetMetadataSeriesSeriestypesById.NotFound
        }

    ///<summary>
    ///Return the source of the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataSeriesSourceById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/series/{id}/source" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataSeriesSourceById.OK(Serializer.deserialize content)
            else
                return GetMetadataSeriesSourceById.NotFound
        }

    ///<summary>
    ///Delete the series tag with the supplied series tag ID from the series with the supplied series ID
    ///</summary>
    ///<param name="seriesId">ID of series</param>
    ///<param name="seriesTagId">ID of series tag to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteMetadataSeriesSeriestagsBySeriesIdAndSeriesTagId
        (
            seriesId: int64,
            seriesTagId: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("seriesId", seriesId)
                  RequestPart.path ("seriesTagId", seriesTagId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    httpClient
                    "/metadata/series/{seriesId}/seriestags/{seriesTagId}"
                    requestParts
                    cancellationToken

            return DeleteMetadataSeriesSeriestagsBySeriesIdAndSeriesTagId.NoContent
        }

    ///<summary>
    ///Delete the series type with the supplied series type ID from the series with the supplied series ID
    ///</summary>
    ///<param name="seriesId">ID of series</param>
    ///<param name="seriesTypeId">ID of series type to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId
        (
            seriesId: int64,
            seriesTypeId: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("seriesId", seriesId)
                  RequestPart.path ("seriesTypeId", seriesTypeId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    httpClient
                    "/metadata/series/{seriesId}/seriestypes/{seriesTypeId}"
                    requestParts
                    cancellationToken

            return DeleteMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId.NoContent
        }

    ///<summary>
    ///Add the series type with the supplied series type ID to the series with the supplied series ID
    ///</summary>
    ///<param name="seriesId">ID of series</param>
    ///<param name="seriesTypeId">ID of series type to add</param>
    ///<param name="cancellationToken"></param>
    member this.PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId
        (
            seriesId: int64,
            seriesTypeId: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("seriesId", seriesId)
                  RequestPart.path ("seriesTypeId", seriesTypeId) ]

            let! (status, content) =
                OpenApiHttp.putAsync
                    httpClient
                    "/metadata/series/{seriesId}/seriestypes/{seriesTypeId}"
                    requestParts
                    cancellationToken

            if status = HttpStatusCode.NoContent then
                return PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId.NoContent
            else
                return PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId.NotFound
        }

    ///<summary>
    ///Returns a list of series tags currently currently in use.
    ///</summary>
    member this.GetMetadataSeriestags(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/seriestags" requestParts cancellationToken

            return GetMetadataSeriestags.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Returns a list of metadata on the study level of the DICOM hierarchy
    ///</summary>
    ///<param name="patientid">reference to patient to list studies for</param>
    ///<param name="startindex">start index of returned slice of studies</param>
    ///<param name="count">size of returned slice of studies</param>
    ///<param name="sources">filter the results by matching on one or more underlying series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more underlying series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows results including series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more underlying series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows results including series with either of the series tags with ids 6, 2 and 11.</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataStudies
        (
            patientid: int64,
            ?startindex: int64,
            ?count: int64,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.query ("patientid", patientid)
                  if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/metadata/studies" requestParts cancellationToken
            return GetMetadataStudies.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for studies
    ///</summary>
    member this.PostMetadataStudiesQuery(query: query, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/metadata/studies/query" requestParts cancellationToken

            return PostMetadataStudiesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the study with the supplied ID
    ///</summary>
    ///<param name="id">ID of study</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataStudiesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/studies/{id}" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataStudiesById.OK(Serializer.deserialize content)
            else
                return GetMetadataStudiesById.NotFound
        }

    ///<summary>
    ///Returns all images for the study with the supplied study ID
    ///</summary>
    ///<param name="id">ID of study</param>
    ///<param name="sources">filter the results by matching on one or more series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows series with either of the series tags with ids 6, 2 and 11.</param>
    ///<param name="cancellationToken"></param>
    member this.GetMetadataStudiesImagesById
        (
            id: int64,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/metadata/studies/{id}/images" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetMetadataStudiesImagesById.OK(Serializer.deserialize content)
            else
                return GetMetadataStudiesImagesById.NotFound
        }

    ///<summary>
    ///get a list of DICOM SCPs. Each SCP is a server for receiving DICOM images from e.g. a PACS system.
    ///</summary>
    ///<param name="startindex">start index of returned slice of SCPs</param>
    ///<param name="count">size of returned slice of SCPs</param>
    ///<param name="cancellationToken"></param>
    member this.GetScps(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/scps" requestParts cancellationToken
            return GetScps.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new SCP for receiving DICOM images
    ///</summary>
    member this.PostScps(scp: scp, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent scp ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/scps" requestParts cancellationToken

            if status = HttpStatusCode.Created then
                return PostScps.Created(Serializer.deserialize content)
            else
                return PostScps.BadRequest
        }

    ///<summary>
    ///shut down and remove the SCP corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of SCP to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteScpsById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/scps/{id}" requestParts cancellationToken
            return DeleteScpsById.NoContent
        }

    ///<summary>
    ///get a list of DICOM SCUs. Each SCU is a client for sending DICOM images to an SCP, e.g. a PACS system.
    ///</summary>
    ///<param name="startindex">start index of returned slice of SCUs</param>
    ///<param name="count">size of returned slice of SCUs</param>
    ///<param name="cancellationToken"></param>
    member this.GetScus(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/scus" requestParts cancellationToken
            return GetScus.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new SCU for sending DICOM images
    ///</summary>
    member this.PostScus(scu: scu, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent scu ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/scus" requestParts cancellationToken

            if status = HttpStatusCode.Created then
                return PostScus.Created(Serializer.deserialize content)
            else
                return PostScus.BadRequest
        }

    ///<summary>
    ///remove the SCU corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of SCU to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteScusById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/scus/{id}" requestParts cancellationToken
            return DeleteScusById.NoContent
        }

    ///<summary>
    ///send the images with the supplied image IDs to a DICOM SCP using the the SCU with the supplied scu ID
    ///</summary>
    ///<param name="id">id of SCU to use for sending</param>
    ///<param name="imageids"></param>
    ///<param name="cancellationToken"></param>
    member this.PostScusSendById(id: int64, imageids: list<int64>, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent imageids ]

            let! (status, content) = OpenApiHttp.postAsync httpClient "/scus/{id}/send" requestParts cancellationToken

            if status = HttpStatusCode.NoContent then
                return PostScusSendById.NoContent
            else
                return PostScusSendById.NotFound
        }

    ///<summary>
    ///get a list of all added series types. By filtering search results for certain series types, it is easier for applications to ensure that they read images of applicable types.
    ///</summary>
    ///<param name="startindex">start index of returned slice of series types</param>
    ///<param name="count">size of returned slice of series types</param>
    ///<param name="cancellationToken"></param>
    member this.GetSeriestypes(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/seriestypes" requestParts cancellationToken
            return GetSeriestypes.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new series type
    ///</summary>
    member this.PostSeriestypes(seriesType: seriestype, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent seriesType ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/seriestypes" requestParts cancellationToken
            return PostSeriestypes.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of rules for assigning series types to series. A rule connects to a series of attributes with values and a resulting series type. If a series has the required values of the listed attributes, it is assigned to the series type of the rule.
    ///</summary>
    ///<param name="seriestypeid">ID of series type to list rules for</param>
    ///<param name="cancellationToken"></param>
    member this.GetSeriestypesRules(seriestypeid: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.query ("seriestypeid", seriestypeid) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/seriestypes/rules" requestParts cancellationToken
            return GetSeriestypesRules.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new series type rule
    ///</summary>
    member this.PostSeriestypesRules(seriesTypeRule: seriestyperule, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ RequestPart.jsonContent seriesTypeRule ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/seriestypes/rules" requestParts cancellationToken

            return PostSeriestypesRules.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///get the status of the internal process of updating series types for series following a change of series types, rules or attributes.
    ///</summary>
    member this.GetSeriestypesRulesUpdatestatus(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/seriestypes/rules/updatestatus" requestParts cancellationToken

            return GetSeriestypesRulesUpdatestatus.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///remove the series type rule corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of series type rule to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSeriestypesRulesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/seriestypes/rules/{id}" requestParts cancellationToken

            return DeleteSeriestypesRulesById.NoContent
        }

    ///<summary>
    ///get the list of attributes for the series type rule with the supplied ID.
    ///</summary>
    ///<param name="id">index of series type rule to list rule attributes for</param>
    ///<param name="cancellationToken"></param>
    member this.GetSeriestypesRulesAttributesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/seriestypes/rules/{id}/attributes" requestParts cancellationToken

            return GetSeriestypesRulesAttributesById.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new series type rule attribute
    ///</summary>
    ///<param name="id">ID of rule</param>
    ///<param name="seriesTypeRuleAttribute"></param>
    ///<param name="cancellationToken"></param>
    member this.PostSeriestypesRulesAttributesById
        (
            id: int64,
            seriesTypeRuleAttribute: seriestyperuleattribute,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent seriesTypeRuleAttribute ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/seriestypes/rules/{id}/attributes" requestParts cancellationToken

            return PostSeriestypesRulesAttributesById.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///remove the series type rule attribute corresponding to the supplied series type and attribute IDs
    ///</summary>
    ///<param name="ruleId">id of series type rule for which to remove an attribute</param>
    ///<param name="attributeId">id of attribute to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSeriestypesRulesAttributesByRuleIdAndAttributeId
        (
            ruleId: int64,
            attributeId: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("ruleId", ruleId)
                  RequestPart.path ("attributeId", attributeId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    httpClient
                    "/seriestypes/rules/{ruleId}/attributes/{attributeId}"
                    requestParts
                    cancellationToken

            return DeleteSeriestypesRulesAttributesByRuleIdAndAttributeId.NoContent
        }

    ///<summary>
    ///submit a query for seriestypes for a list of series
    ///</summary>
    member this.PostSeriestypesSeriesQuery(query: idsquery, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent query ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/seriestypes/series/query" requestParts cancellationToken

            return PostSeriestypesSeriesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///remove the series type corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of series type to remove</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteSeriestypesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync httpClient "/seriestypes/{id}" requestParts cancellationToken

            return DeleteSeriestypesById.NoContent
        }

    ///<summary>
    ///request an asynchronous update of all series, labelling appropriate series with the series type corresponding to the supplied ID.
    ///</summary>
    ///<param name="id">id of series type to update series labels for</param>
    ///<param name="cancellationToken"></param>
    member this.PutSeriestypesById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.putAsync httpClient "/seriestypes/{id}" requestParts cancellationToken
            return PutSeriestypesById.NoContent
        }

    ///<summary>
    ///Returns a list of currently available data sources. Possible source types are user - data imported by an API call by a user, box - data received from a remote box, directory - data imported via a watched directory, import - data imported into slicebox using import sessions, or scp - data received from a PACS.
    ///</summary>
    member this.GetSources(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/sources" requestParts cancellationToken
            return GetSources.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///No-op route for checking whether the service is alive or not
    ///</summary>
    member this.GetSystemHealth(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/system/health" requestParts cancellationToken
            return GetSystemHealth.OK
        }

    ///<summary>
    ///stop and shut down slicebox
    ///</summary>
    member this.PostSystemStop(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync httpClient "/system/stop" requestParts cancellationToken
            return PostSystemStop.OK
        }

    ///<summary>
    ///add an image (dataset) as part of a transaction. This method is used when sending images using the push method to a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the client's outgoing transaction</param>
    ///<param name="sequencenumber">the index of this image in the transaction</param>
    ///<param name="totalimagecount">the total number of images in this transaction</param>
    ///<param name="cancellationToken"></param>
    ///<param name="requestBody"></param>
    member this.PostTransactionsImageByToken
        (
            token: string,
            transactionid: int64,
            sequencenumber: int64,
            totalimagecount: int64,
            ?cancellationToken: CancellationToken,
            ?requestBody: byte []
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid)
                  RequestPart.query ("sequencenumber", sequencenumber)
                  RequestPart.query ("totalimagecount", totalimagecount)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/transactions/{token}/image" requestParts cancellationToken

            if status = HttpStatusCode.NoContent then
                return PostTransactionsImageByToken.NoContent
            else
                return PostTransactionsImageByToken.Unauthorized
        }

    ///<summary>
    ///fetch an image from the connected box as part of a transaction. This method is used when sending images using the poll method from a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the outgoing transaction</param>
    ///<param name="imageid">the ID of the outgoing transaction image</param>
    ///<param name="cancellationToken"></param>
    member this.GetTransactionsOutgoingByToken
        (
            token: string,
            transactionid: int64,
            imageid: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid)
                  RequestPart.query ("imageid", imageid) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync httpClient "/transactions/{token}/outgoing" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetTransactionsOutgoingByToken.OK contentBinary
            else if status = HttpStatusCode.Unauthorized then
                return GetTransactionsOutgoingByToken.Unauthorized contentBinary
            else
                return GetTransactionsOutgoingByToken.NotFound contentBinary
        }

    ///<summary>
    ///signal that the supplied outgoing transaction and image was successfully received and can be marked as sent. This method is used when sending images using the poll method from a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="outgoing entry and image information block"></param>
    ///<param name="cancellationToken"></param>
    member this.PostTransactionsOutgoingDoneByToken
        (
            token: string,
            ``outgoing entry and image information block``: outgoingTransactionImage,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.jsonContent ``outgoing entry and image information block`` ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/transactions/{token}/outgoing/done" requestParts cancellationToken

            if status = HttpStatusCode.NoContent then
                return PostTransactionsOutgoingDoneByToken.NoContent
            else
                return PostTransactionsOutgoingDoneByToken.Unauthorized
        }

    ///<summary>
    ///signal that the image corresponding to the supplied outgoing transaction and image could not be read or stored properly on the receiving side, and that the transaction should be marked as failed.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="outgoing transaction and image, and error message"></param>
    ///<param name="cancellationToken"></param>
    member this.PostTransactionsOutgoingFailedByToken
        (
            token: string,
            ``outgoing transaction and image, and error message``: failedOutgoingTransactionImage,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.jsonContent ``outgoing transaction and image, and error message`` ]

            let! (status, content) =
                OpenApiHttp.postAsync httpClient "/transactions/{token}/outgoing/failed" requestParts cancellationToken

            if status = HttpStatusCode.NoContent then
                return PostTransactionsOutgoingFailedByToken.NoContent
            else
                return PostTransactionsOutgoingFailedByToken.Unauthorized
        }

    ///<summary>
    ///get next outgoing transaction and image (information on the next image that the connected box wishes to send to you), if any. This method is used when sending images using the poll method from a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="cancellationToken"></param>
    member this.GetTransactionsOutgoingPollByToken(token: string, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("token", token) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/transactions/{token}/outgoing/poll" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetTransactionsOutgoingPollByToken.OK(Serializer.deserialize content)
            else if status = HttpStatusCode.Unauthorized then
                return GetTransactionsOutgoingPollByToken.Unauthorized
            else
                return GetTransactionsOutgoingPollByToken.NotFound
        }

    ///<summary>
    ///get the status of the remote incoming transaction with the supplied transaction ID
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the client's outgoing transaction</param>
    ///<param name="cancellationToken"></param>
    member this.GetTransactionsStatusByToken
        (
            token: string,
            transactionid: int64,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid) ]

            let! (status, content) =
                OpenApiHttp.getAsync httpClient "/transactions/{token}/status" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetTransactionsStatusByToken.OK
            else if status = HttpStatusCode.Unauthorized then
                return GetTransactionsStatusByToken.Unauthorized
            else
                return GetTransactionsStatusByToken.NotFound
        }

    ///<summary>
    ///update the status of the transaction with the supplied ID
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the client's outgoing transaction</param>
    ///<param name="transaction status"></param>
    ///<param name="cancellationToken"></param>
    member this.PutTransactionsStatusByToken
        (
            token: string,
            transactionid: int64,
            ``transaction status``: string,
            ?cancellationToken: CancellationToken
        ) =
        task {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid)
                  RequestPart.jsonContent ``transaction status`` ]

            let! (status, content) =
                OpenApiHttp.putAsync httpClient "/transactions/{token}/status" requestParts cancellationToken

            if status = HttpStatusCode.NoContent then
                return PutTransactionsStatusByToken.NoContent
            else
                return PutTransactionsStatusByToken.NotFound
        }

    ///<summary>
    ///Returns all users of slicebox
    ///</summary>
    ///<param name="startindex">start index of returned slice of users</param>
    ///<param name="count">size of returned slice of users</param>
    ///<param name="cancellationToken"></param>
    member this.GetUsers(?startindex: int64, ?count: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync httpClient "/users" requestParts cancellationToken
            return GetUsers.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Creates a new user. Dupicates are accepted but not added.
    ///</summary>
    member this.PostUsers(user: newUser, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent user ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/users" requestParts cancellationToken
            return PostUsers.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///obtain information on the currently logged in user as specified by the supplied session cookie, IP address and user agent.
    ///</summary>
    member this.GetUsersCurrent(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync httpClient "/users/current" requestParts cancellationToken

            if status = HttpStatusCode.OK then
                return GetUsersCurrent.OK(Serializer.deserialize content)
            else
                return GetUsersCurrent.NotFound
        }

    ///<summary>
    ///Obtain a session cookie that can be used to authenticate future API calls from the present IP address and with the present user agent.
    ///</summary>
    member this.PostUsersLogin(userPass: userPass, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.jsonContent userPass ]
            let! (status, content) = OpenApiHttp.postAsync httpClient "/users/login" requestParts cancellationToken

            if status = HttpStatusCode.Created then
                return PostUsersLogin.Created
            else
                return PostUsersLogin.Unauthorized
        }

    ///<summary>
    ///Logout the current user by responding with a delete cookie header removing the session cookie for this user.
    ///</summary>
    member this.PostUsersLogout(?cancellationToken: CancellationToken) =
        task {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync httpClient "/users/logout" requestParts cancellationToken
            return PostUsersLogout.Created
        }

    ///<summary>
    ///deletes a single user based on the ID supplied
    ///</summary>
    ///<param name="id">ID of user to delete</param>
    ///<param name="cancellationToken"></param>
    member this.DeleteUsersById(id: int64, ?cancellationToken: CancellationToken) =
        task {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync httpClient "/users/{id}" requestParts cancellationToken
            return DeleteUsersById.NoContent
        }
