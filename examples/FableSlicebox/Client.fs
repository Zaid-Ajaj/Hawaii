namespace rec FableSlicebox

open Browser.Types
open Fable.SimpleHttp
open FableSlicebox.Types
open FableSlicebox.Http

///Slicebox - safe sharing of medical images
type FableSliceboxClient(url: string, headers: list<Header>) =
    new(url: string) = FableSliceboxClient(url, [])

    ///<summary>
    ///anonymize the images corresponding to the supplied list of image IDs (each paired with a list of DICOM tag translation). This route corresponds to repeated use of the route /images/{id}/anonymize.
    ///</summary>
    member this.PostAnonymizationAnonymize(query: PostAnonymizationAnonymizePayload) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/anonymization/anonymize" headers requestParts
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
    member this.GetAnonymizationKeys
        (
            ?startindex: int64,
            ?count: int64,
            ?orderby: string,
            ?orderascending: bool,
            ?filter: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/anonymization/keys" headers requestParts
            return GetAnonymizationKeys.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///export all anonymization keys as a csv file
    ///</summary>
    member this.GetAnonymizationKeysExportCsv() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/anonymization/keys/export/csv" headers requestParts
            return GetAnonymizationKeysExportCsv.OK content
        }

    ///<summary>
    ///submit a query for anonymization keys
    ///</summary>
    member this.PostAnonymizationKeysQuery(query: anonymizationKeyQuery) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/anonymization/keys/query" headers requestParts
            return PostAnonymizationKeysQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///delete an anonymization key that is no longer of interest
    ///</summary>
    ///<param name="id">ID of anonymization key</param>
    member this.DeleteAnonymizationKeysById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/anonymization/keys/{id}" headers requestParts
            return DeleteAnonymizationKeysById.NoContent
        }

    ///<summary>
    ///get the anonymization key with the supplied ID
    ///</summary>
    ///<param name="id">ID of anonymization key</param>
    member this.GetAnonymizationKeysById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/anonymization/keys/{id}" headers requestParts

            match int status with
            | 200 -> return GetAnonymizationKeysById.OK(Serializer.deserialize content)
            | _ -> return GetAnonymizationKeysById.NotFound
        }

    ///<summary>
    ///get pointers to the images corresponding to the anonymization key with the supplied ID
    ///</summary>
    ///<param name="id">ID of anonymization key</param>
    member this.GetAnonymizationKeysKeyvaluesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/anonymization/keys/{id}/keyvalues" headers requestParts

            match int status with
            | 200 -> return GetAnonymizationKeysKeyvaluesById.OK(Serializer.deserialize content)
            | _ -> return GetAnonymizationKeysKeyvaluesById.NotFound
        }

    ///<summary>
    ///list all supported anonymization options defining an anonymization profile
    ///</summary>
    member this.GetAnonymizationOptions() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/anonymization/options" headers requestParts
            return GetAnonymizationOptions.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of box connections
    ///</summary>
    ///<param name="startindex">start index of returned slice of boxes</param>
    ///<param name="count">size of returned slice of boxes</param>
    member this.GetBoxes(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/boxes" headers requestParts
            return GetBoxes.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///connect to another box using a received URL. Used to connect to a public box.
    ///</summary>
    member this.PostBoxesConnect(remoteBox: remoteBox) =
        async {
            let requestParts = [ RequestPart.jsonContent remoteBox ]
            let! (status, content) = OpenApiHttp.postAsync url "/boxes/connect" headers requestParts
            return PostBoxesConnect.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///create a new box connection where the supplied entity holds the remote box name. Used by publicly available boxes.
    ///</summary>
    member this.PostBoxesCreateconnection(remoteBoxConnectionData: remoteBoxConnectionData) =
        async {
            let requestParts =
                [ RequestPart.jsonContent remoteBoxConnectionData ]

            let! (status, content) = OpenApiHttp.postAsync url "/boxes/createconnection" headers requestParts
            return PostBoxesCreateconnection.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///get incoming transactions (finished, currently receiving, waiting or failed)
    ///</summary>
    ///<param name="startindex">start index of returned slice of transactions</param>
    ///<param name="count">size of returned slice of transactions</param>
    member this.GetBoxesIncoming(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/boxes/incoming" headers requestParts
            return GetBoxesIncoming.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///delete an incoming transaction. If a currently active transaction is deleted, a new transaction with the remainder of the images is created when receiving the next incoming image.
    ///</summary>
    ///<param name="id">ID of incoming transaction</param>
    member this.DeleteBoxesIncomingById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/boxes/incoming/{id}" headers requestParts
            return DeleteBoxesIncomingById.NoContent
        }

    ///<summary>
    ///get the received images corresponding to the incoming transaction with the supplied ID
    ///</summary>
    ///<param name="id">ID of incoming transaction</param>
    member this.GetBoxesIncomingImagesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/boxes/incoming/{id}/images" headers requestParts

            match int status with
            | 200 -> return GetBoxesIncomingImagesById.OK(Serializer.deserialize content)
            | _ -> return GetBoxesIncomingImagesById.NotFound
        }

    ///<summary>
    ///get outgoing transactions (finished, currently sending, waiting or failed)
    ///</summary>
    ///<param name="startindex">start index of returned slice of transactions</param>
    ///<param name="count">size of returned slice of transactions</param>
    member this.GetBoxesOutgoing(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/boxes/outgoing" headers requestParts
            return GetBoxesOutgoing.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///delete an outgoing transaction. This will stop ongoing transactions.
    ///</summary>
    ///<param name="id">ID of outgoing transaction</param>
    member this.DeleteBoxesOutgoingById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/boxes/outgoing/{id}" headers requestParts
            return DeleteBoxesOutgoingById.NoContent
        }

    ///<summary>
    ///get the sent images corresponding to the outgoing transaction with the supplied ID
    ///</summary>
    ///<param name="id">ID of outgoing transaction</param>
    member this.GetBoxesOutgoingImagesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/boxes/outgoing/{id}/images" headers requestParts

            match int status with
            | 200 -> return GetBoxesOutgoingImagesById.OK(Serializer.deserialize content)
            | _ -> return GetBoxesOutgoingImagesById.NotFound
        }

    ///<summary>
    ///Delete the remote box with the supplied ID
    ///</summary>
    ///<param name="id">ID of box to remove</param>
    member this.DeleteBoxesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/boxes/{id}" headers requestParts
            return DeleteBoxesById.NoContent
        }

    ///<summary>
    ///send images corresponding to the supplied image ids to the remote box with the supplied ID
    ///</summary>
    ///<param name="id">ID of box to send images to</param>
    ///<param name="sequence of image tag values"></param>
    member this.PostBoxesSendById(id: int64, ``sequence of image tag values``: bulkAnonymizationData) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``sequence of image tag values`` ]

            let! (status, content) = OpenApiHttp.postAsync url "/boxes/{id}/send" headers requestParts

            match int status with
            | 201 -> return PostBoxesSendById.Created
            | _ -> return PostBoxesSendById.NotFound
        }

    ///<summary>
    ///Returns a list of currently available destinations. Possible destinations are box - sending data to a remote box, and scu - sending data a receiving SCP.
    ///</summary>
    member this.GetDestinations() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/destinations" headers requestParts
            return GetDestinations.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of watch directories. Each watch directory and its sub-directories are watched for incoming DICOM files, which are read and imported into slicebox.
    ///</summary>
    ///<param name="startindex">start index of returned slice of watched directories</param>
    ///<param name="count">size of returned slice of watched directories</param>
    member this.GetDirectorywatches(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/directorywatches" headers requestParts
            return GetDirectorywatches.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new directory to watch for incoming DICOM files
    ///</summary>
    member this.PostDirectorywatches(watchedDirectory: watchedDirectory) =
        async {
            let requestParts =
                [ RequestPart.jsonContent watchedDirectory ]

            let! (status, content) = OpenApiHttp.postAsync url "/directorywatches" headers requestParts
            return PostDirectorywatches.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///stop watching and remove the directory corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of directory to stop watching</param>
    member this.DeleteDirectorywatchesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/directorywatches/{id}" headers requestParts
            return DeleteDirectorywatchesById.NoContent
        }

    ///<summary>
    ///Get a list of source to filter associations.
    ///</summary>
    ///<param name="startindex">start index of returned slice of source &amp;lt;-&amp;gt; filter associations</param>
    ///<param name="count">size of returned slice of source &amp;lt;-&amp;gt; filter associations</param>
    member this.GetFilteringAssociations(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/filtering/associations" headers requestParts
            return GetFilteringAssociations.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Inserts or updates a source &amp;lt;-&amp;gt; filter associations. If the specified Source already  has an association this is updated, otherwise a new is inserted.
    ///</summary>
    member this.PostFilteringAssociations(sourcetagfilter: sourceTagFilter) =
        async {
            let requestParts =
                [ RequestPart.jsonContent sourcetagfilter ]

            let! (status, content) = OpenApiHttp.postAsync url "/filtering/associations" headers requestParts
            return PostFilteringAssociations.Created
        }

    ///<summary>
    ///remove the source &amp;lt;-&amp;gt; filter association corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of source &amp;lt;-&amp;gt; filter association to remove</param>
    member this.DeleteFilteringAssociationsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/filtering/associations/{id}" headers requestParts
            return DeleteFilteringAssociationsById.NoContent
        }

    ///<summary>
    ///List defined filters
    ///</summary>
    ///<param name="startindex">start index of returned slice of filters</param>
    ///<param name="count">size of returned slice of filters</param>
    member this.GetFilteringFilters(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/filtering/filters" headers requestParts
            return GetFilteringFilters.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Inserts or updates a filter. If a filter with same name as supplied filter exists this filter is updated, otherwise a new filter is inserted.
    ///</summary>
    member this.PostFilteringFilters(tagFilter: filter) =
        async {
            let requestParts = [ RequestPart.jsonContent tagFilter ]
            let! (status, content) = OpenApiHttp.postAsync url "/filtering/filters" headers requestParts
            return PostFilteringFilters.Created
        }

    ///<summary>
    ///remove the filter corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of filter to remove</param>
    member this.DeleteFilteringFiltersById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/filtering/filters/{id}" headers requestParts
            return DeleteFilteringFiltersById.NoContent
        }

    ///<summary>
    ///List tagpaths for the selected filter
    ///</summary>
    ///<param name="id">id of filter</param>
    member this.GetFilteringFiltersTagpathsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/filtering/filters/{id}/tagpaths" headers requestParts
            return GetFilteringFiltersTagpathsById.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a tagpath to a filter
    ///</summary>
    ///<param name="id">id of filter to remove</param>
    ///<param name="tagpath"></param>
    member this.PostFilteringFiltersTagpathsById(id: int64, tagpath: tagPathTag) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent tagpath ]

            let! (status, content) = OpenApiHttp.postAsync url "/filtering/filters/{id}/tagpaths" headers requestParts
            return PostFilteringFiltersTagpathsById.Created
        }

    ///<summary>
    ///remove the tagpath corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of filter</param>
    ///<param name="tagpathid">id of TagPath to remove</param>
    member this.DeleteFilteringFiltersTagpathsByIdAndTagpathid(id: int64, tagpathid: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.path ("tagpathid", tagpathid) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/filtering/filters/{id}/tagpaths/{tagpathid}" headers requestParts

            return DeleteFilteringFiltersTagpathsByIdAndTagpathid.NoContent
        }

    ///<summary>
    ///remove the forwarding rule corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of forwarding rule to remove</param>
    member this.DeleteForwardingRuleById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/forwarding/rule/{id}" headers requestParts
            return DeleteForwardingRuleById.NoContent
        }

    ///<summary>
    ///get a list of all forwarding rules. A forwarding rule specifies the automatic forwarding of images from a source (SCP, BOX, etc.) to a destimation (BOX, SCU, etc.)
    ///</summary>
    ///<param name="startindex">start index of returned slice of rules</param>
    ///<param name="count">size of returned slice of rules</param>
    member this.GetForwardingRules(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/forwarding/rules" headers requestParts
            return GetForwardingRules.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new forwarding rule
    ///</summary>
    member this.PostForwardingRules(fowardingRule: forwardingrule) =
        async {
            let requestParts =
                [ RequestPart.jsonContent fowardingRule ]

            let! (status, content) = OpenApiHttp.postAsync url "/forwarding/rules" headers requestParts
            return PostForwardingRules.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///add a DICOM dataset to slicebox
    ///</summary>
    member this.PostImages(body: string) =
        async {
            let requestParts = [ RequestPart.jsonContent body ]
            let! (status, content) = OpenApiHttp.postAsync url "/images" headers requestParts

            match int status with
            | 200 -> return PostImages.OK(Serializer.deserialize content)
            | _ -> return PostImages.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///bulk delete a sequence of images according to the supplied image IDs. This is the same as a sequence of DELETE requests to /images/{id}
    ///</summary>
    member this.PostImagesDelete(``image IDs``: list<int64>) =
        async {
            let requestParts =
                [ RequestPart.jsonContent ``image IDs`` ]

            let! (status, content) = OpenApiHttp.postAsync url "/images/delete" headers requestParts
            return PostImagesDelete.NoContent
        }

    ///<summary>
    ///download the export set with the supplied export set ID as a zip archive
    ///</summary>
    ///<param name="id">ID of export set to download</param>
    member this.GetImagesExport(id: int64) =
        async {
            let requestParts = [ RequestPart.query ("id", id) ]
            let! (status, contentBinary) = OpenApiHttp.getBinaryAsync url "/images/export" headers requestParts
            return GetImagesExport.OK contentBinary
        }

    ///<summary>
    ///create an export set, a group of image IDs of images to export. The export set will contain the selected images. The export set is available for download 12 hours before it is automatically deleted.
    ///</summary>
    member this.PostImagesExport(``image ids``: list<int64>) =
        async {
            let requestParts =
                [ RequestPart.jsonContent ``image ids`` ]

            let! (status, content) = OpenApiHttp.postAsync url "/images/export" headers requestParts

            match int status with
            | 200 -> return PostImagesExport.OK(Serializer.deserialize content)
            | _ -> return PostImagesExport.Created
        }

    ///<summary>
    ///add a JPEG image to slicebox. The image data will be wrapped in a DICOM file and added as a new series belonging to the study with the supplied ID
    ///</summary>
    ///<param name="studyid">ID of study to add new series to</param>
    ///<param name="description">DICOM series description of the resulting secondary capture series</param>
    ///<param name="requestBody"></param>
    member this.PostImagesJpeg(studyid: int64, ?description: string, ?requestBody: byte []) =
        async {
            let requestParts =
                [ RequestPart.query ("studyid", studyid)
                  if description.IsSome then
                      RequestPart.query ("description", description.Value)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) = OpenApiHttp.postAsync url "/images/jpeg" headers requestParts
            return PostImagesJpeg.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete the image with the supplied ID
    ///</summary>
    ///<param name="id">ID of image</param>
    member this.DeleteImagesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/images/{id}" headers requestParts
            return DeleteImagesById.NoContent
        }

    ///<summary>
    ///fetch dataset corresponding to the supplied image ID
    ///</summary>
    ///<param name="id">ID of image</param>
    member this.GetImagesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, contentBinary) = OpenApiHttp.getBinaryAsync url "/images/{id}" headers requestParts

            match int status with
            | 200 -> return GetImagesById.OK contentBinary
            | _ -> return GetImagesById.NotFound contentBinary
        }

    ///<summary>
    ///delete the selected image and replace it with an anonymized version
    ///</summary>
    ///<param name="id">ID of image to anonymize</param>
    ///<param name="tag values"></param>
    member this.PutImagesAnonymizeById(id: int64, ``tag values``: anonymizationData) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``tag values`` ]

            let! (status, content) = OpenApiHttp.putAsync url "/images/{id}/anonymize" headers requestParts

            match int status with
            | 200 -> return PutImagesAnonymizeById.OK(Serializer.deserialize content)
            | _ -> return PutImagesAnonymizeById.NotFound
        }

    ///<summary>
    ///get an anonymized version of the image with the supplied ID
    ///</summary>
    ///<param name="id">ID of image for which to get anonymized dataset</param>
    ///<param name="tag values"></param>
    member this.PostImagesAnonymizedById(id: int64, ``tag values``: anonymizationData) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``tag values`` ]

            let! (status, content) = OpenApiHttp.postAsync url "/images/{id}/anonymized" headers requestParts

            match int status with
            | 200 -> return PostImagesAnonymizedById.OK
            | _ -> return PostImagesAnonymizedById.NotFound
        }

    ///<summary>
    ///list all DICOM attributes of the dataset corresponding to the supplied image ID
    ///</summary>
    ///<param name="id">ID of image</param>
    member this.GetImagesAttributesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/images/{id}/attributes" headers requestParts

            match int status with
            | 200 -> return GetImagesAttributesById.OK(Serializer.deserialize content)
            | _ -> return GetImagesAttributesById.NotFound
        }

    ///<summary>
    ///get basic information about the pixel data of an image
    ///</summary>
    ///<param name="id">ID of image</param>
    member this.GetImagesImageinformationById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/images/{id}/imageinformation" headers requestParts

            match int status with
            | 200 -> return GetImagesImageinformationById.OK(Serializer.deserialize content)
            | _ -> return GetImagesImageinformationById.NotFound
        }

    ///<summary>
    ///modify and/or insert image attributes according to the input tagpath-value mappings
    ///</summary>
    ///<param name="id">ID of image to modify</param>
    ///<param name="tag path value mappings"></param>
    member this.PutImagesModifyById(id: int64, ``tag path value mappings``: PutImagesModifyByIdPayload) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent ``tag path value mappings`` ]

            let! (status, content) = OpenApiHttp.putAsync url "/images/{id}/modify" headers requestParts
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
    member this.GetImagesPngById(id: int64, ?framenumber: int, ?windowmin: int, ?windowmax: int, ?imageheight: int) =
        async {
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

            let! (status, contentBinary) = OpenApiHttp.getBinaryAsync url "/images/{id}/png" headers requestParts

            match int status with
            | 200 -> return GetImagesPngById.OK contentBinary
            | _ -> return GetImagesPngById.NotFound contentBinary
        }

    ///<summary>
    ///Returns a list of available import sessions.
    ///</summary>
    ///<param name="startindex">start index of returned slice of import sessions</param>
    ///<param name="count">size of returned slice of import sessions</param>
    member this.GetImportSessions(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/import/sessions" headers requestParts
            return GetImportSessions.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///create a new import sessions
    ///</summary>
    member this.PostImportSessions(``import session``: importSession) =
        async {
            let requestParts =
                [ RequestPart.jsonContent ``import session`` ]

            let! (status, content) = OpenApiHttp.postAsync url "/import/sessions" headers requestParts
            return PostImportSessions.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///deletes the import session with the supplied ID
    ///</summary>
    ///<param name="id">ID of import session to delete</param>
    member this.DeleteImportSessionsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/import/sessions/{id}" headers requestParts
            return DeleteImportSessionsById.NoContent
        }

    ///<summary>
    ///Returns the import sessions with the supplied ID
    ///</summary>
    ///<param name="id">ID of session</param>
    member this.GetImportSessionsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/import/sessions/{id}" headers requestParts

            match int status with
            | 200 -> return GetImportSessionsById.OK(Serializer.deserialize content)
            | _ -> return GetImportSessionsById.NotFound
        }

    ///<summary>
    ///get the imported images corresponding to the import session with the supplied ID
    ///</summary>
    ///<param name="id">ID of import session</param>
    member this.GetImportSessionsImagesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/import/sessions/{id}/images" headers requestParts

            match int status with
            | 200 -> return GetImportSessionsImagesById.OK(Serializer.deserialize content)
            | _ -> return GetImportSessionsImagesById.NotFound
        }

    ///<summary>
    ///add a DICOM dataset to the import session with the supplied ID
    ///</summary>
    ///<param name="id">ID of session</param>
    ///<param name="body"></param>
    member this.PostImportSessionsImagesById(id: int64, body: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent body ]

            let! (status, content) = OpenApiHttp.postAsync url "/import/sessions/{id}/images" headers requestParts

            match int status with
            | 200 -> return PostImportSessionsImagesById.OK(Serializer.deserialize content)
            | 201 -> return PostImportSessionsImagesById.Created(Serializer.deserialize content)
            | _ -> return PostImportSessionsImagesById.NotFound
        }

    ///<summary>
    ///delete all log messages
    ///</summary>
    member this.DeleteLog() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.deleteAsync url "/log" headers requestParts
            return DeleteLog.NoContent
        }

    ///<summary>
    ///get a list of slicebox log messages
    ///</summary>
    ///<param name="startindex">start index of returned slice of log messages</param>
    ///<param name="count">size of returned slice of log messages</param>
    ///<param name="subject">log subject to filter results by</param>
    ///<param name="type">log type (DEFAULT, INFO, WARN, ERROR) to filter results by</param>
    member this.GetLog(?startindex: int64, ?count: int64, ?subject: string, ?``type``: string) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value)
                  if subject.IsSome then
                      RequestPart.query ("subject", subject.Value)
                  if ``type``.IsSome then
                      RequestPart.query ("type", ``type``.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/log" headers requestParts
            return GetLog.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Delete the log entry with the supplied ID
    ///</summary>
    ///<param name="id">ID of log entry</param>
    member this.DeleteLogById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/log/{id}" headers requestParts
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
    member this.GetMetadataFlatseries
        (
            ?startindex: int64,
            ?count: int64,
            ?orderby: string,
            ?orderascending: bool,
            ?filter: string,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/flatseries" headers requestParts
            return GetMetadataFlatseries.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for flat series
    ///</summary>
    member this.PostMetadataFlatseriesQuery(query: query) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/metadata/flatseries/query" headers requestParts
            return PostMetadataFlatseriesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the flat series with the supplied ID
    ///</summary>
    ///<param name="id">ID of flat series</param>
    member this.GetMetadataFlatseriesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/flatseries/{id}" headers requestParts

            match int status with
            | 200 -> return GetMetadataFlatseriesById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataFlatseriesById.NotFound
        }

    ///<summary>
    ///Returns a list of metadata on the image level of the DICOM hierarchy
    ///</summary>
    ///<param name="seriesid">reference to series to list images for</param>
    ///<param name="startindex">start index of returned slice of images</param>
    ///<param name="count">size of returned slice of images</param>
    member this.GetMetadataImages(seriesid: int64, ?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ RequestPart.query ("seriesid", seriesid)
                  if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/images" headers requestParts
            return GetMetadataImages.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for images
    ///</summary>
    member this.PostMetadataImagesQuery(query: query) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/metadata/images/query" headers requestParts
            return PostMetadataImagesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the image with the supplied ID
    ///</summary>
    ///<param name="id">ID of image</param>
    member this.GetMetadataImagesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/images/{id}" headers requestParts

            match int status with
            | 200 -> return GetMetadataImagesById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataImagesById.NotFound
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
    member this.GetMetadataPatients
        (
            ?startindex: int64,
            ?count: int64,
            ?orderby: string,
            ?orderascending: bool,
            ?filter: string,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/patients" headers requestParts
            return GetMetadataPatients.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for patients
    ///</summary>
    member this.PostMetadataPatientsQuery(query: query) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/metadata/patients/query" headers requestParts
            return PostMetadataPatientsQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the patient with the supplied ID
    ///</summary>
    ///<param name="id">ID of patient</param>
    member this.GetMetadataPatientsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/patients/{id}" headers requestParts

            match int status with
            | 200 -> return GetMetadataPatientsById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataPatientsById.NotFound
        }

    ///<summary>
    ///Returns all images for the patient with the supplied patient ID
    ///</summary>
    ///<param name="id">ID of patient</param>
    ///<param name="sources">filter the results by matching on one or more series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows series with either of the series tags with ids 6, 2 and 11.</param>
    member this.GetMetadataPatientsImagesById(id: int64, ?sources: string, ?seriestypes: string, ?seriestags: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/patients/{id}/images" headers requestParts
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
    member this.GetMetadataSeries
        (
            studyid: int64,
            ?startindex: int64,
            ?count: int64,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/series" headers requestParts
            return GetMetadataSeries.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for series
    ///</summary>
    member this.PostMetadataSeriesQuery(query: query) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/metadata/series/query" headers requestParts
            return PostMetadataSeriesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    member this.GetMetadataSeriesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/series/{id}" headers requestParts

            match int status with
            | 200 -> return GetMetadataSeriesById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataSeriesById.NotFound
        }

    ///<summary>
    ///get the list of series tags for the series with the supplied ID.
    ///</summary>
    ///<param name="id">ID of series</param>
    member this.GetMetadataSeriesSeriestagsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/series/{id}/seriestags" headers requestParts

            match int status with
            | 200 -> return GetMetadataSeriesSeriestagsById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataSeriesSeriestagsById.NotFound
        }

    ///<summary>
    ///add a series tag to the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    ///<param name="query"></param>
    member this.PostMetadataSeriesSeriestagsById(id: int64, query: seriestag) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent query ]

            let! (status, content) = OpenApiHttp.postAsync url "/metadata/series/{id}/seriestags" headers requestParts

            match int status with
            | 201 -> return PostMetadataSeriesSeriestagsById.Created(Serializer.deserialize content)
            | _ -> return PostMetadataSeriesSeriestagsById.NotFound
        }

    ///<summary>
    ///Delete all series types for the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    member this.DeleteMetadataSeriesSeriestypesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/metadata/series/{id}/seriestypes" headers requestParts

            return DeleteMetadataSeriesSeriestypesById.NoContent
        }

    ///<summary>
    ///get the list of series types for the series with the supplied ID.
    ///</summary>
    ///<param name="id">ID of series</param>
    member this.GetMetadataSeriesSeriestypesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/series/{id}/seriestypes" headers requestParts

            match int status with
            | 200 -> return GetMetadataSeriesSeriestypesById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataSeriesSeriestypesById.NotFound
        }

    ///<summary>
    ///Return the source of the series with the supplied ID
    ///</summary>
    ///<param name="id">ID of series</param>
    member this.GetMetadataSeriesSourceById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/series/{id}/source" headers requestParts

            match int status with
            | 200 -> return GetMetadataSeriesSourceById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataSeriesSourceById.NotFound
        }

    ///<summary>
    ///Delete the series tag with the supplied series tag ID from the series with the supplied series ID
    ///</summary>
    ///<param name="seriesId">ID of series</param>
    ///<param name="seriesTagId">ID of series tag to remove</param>
    member this.DeleteMetadataSeriesSeriestagsBySeriesIdAndSeriesTagId(seriesId: int64, seriesTagId: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("seriesId", seriesId)
                  RequestPart.path ("seriesTagId", seriesTagId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/metadata/series/{seriesId}/seriestags/{seriesTagId}" headers requestParts

            return DeleteMetadataSeriesSeriestagsBySeriesIdAndSeriesTagId.NoContent
        }

    ///<summary>
    ///Delete the series type with the supplied series type ID from the series with the supplied series ID
    ///</summary>
    ///<param name="seriesId">ID of series</param>
    ///<param name="seriesTypeId">ID of series type to remove</param>
    member this.DeleteMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId(seriesId: int64, seriesTypeId: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("seriesId", seriesId)
                  RequestPart.path ("seriesTypeId", seriesTypeId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync
                    url
                    "/metadata/series/{seriesId}/seriestypes/{seriesTypeId}"
                    headers
                    requestParts

            return DeleteMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId.NoContent
        }

    ///<summary>
    ///Add the series type with the supplied series type ID to the series with the supplied series ID
    ///</summary>
    ///<param name="seriesId">ID of series</param>
    ///<param name="seriesTypeId">ID of series type to add</param>
    member this.PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId(seriesId: int64, seriesTypeId: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("seriesId", seriesId)
                  RequestPart.path ("seriesTypeId", seriesTypeId) ]

            let! (status, content) =
                OpenApiHttp.putAsync url "/metadata/series/{seriesId}/seriestypes/{seriesTypeId}" headers requestParts

            match int status with
            | 204 -> return PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId.NoContent
            | _ -> return PutMetadataSeriesSeriestypesBySeriesIdAndSeriesTypeId.NotFound
        }

    ///<summary>
    ///Returns a list of series tags currently currently in use.
    ///</summary>
    member this.GetMetadataSeriestags() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/seriestags" headers requestParts
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
    member this.GetMetadataStudies
        (
            patientid: int64,
            ?startindex: int64,
            ?count: int64,
            ?sources: string,
            ?seriestypes: string,
            ?seriestags: string
        ) =
        async {
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

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/studies" headers requestParts
            return GetMetadataStudies.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///submit a query for studies
    ///</summary>
    member this.PostMetadataStudiesQuery(query: query) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/metadata/studies/query" headers requestParts
            return PostMetadataStudiesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Return the study with the supplied ID
    ///</summary>
    ///<param name="id">ID of study</param>
    member this.GetMetadataStudiesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/metadata/studies/{id}" headers requestParts

            match int status with
            | 200 -> return GetMetadataStudiesById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataStudiesById.NotFound
        }

    ///<summary>
    ///Returns all images for the study with the supplied study ID
    ///</summary>
    ///<param name="id">ID of study</param>
    ///<param name="sources">filter the results by matching on one or more series sources. Examples of sources are user, box, directory or scp. The list of sources to filter results by must have the form TYPE1:ID1,TYPE2:ID2,...,TYPEN:IDN. For instance, the argument sources=box:1,user:5 shows results either sent from (slice)box with id 1 or uploaded by user with id 5.</param>
    ///<param name="seriestypes">filter the results by matching on one or more series types. The supplied list of series types must be a comma separated list of series type ids. For instance, the argument seriestypes=3,7,22 shows series assigned to either of the series types with ids 3, 7 and 22.</param>
    ///<param name="seriestags">filter the results by matching on one or more series tags. The supplied list of series tags must be a comma separated list of series tag ids. For instance, the argument seriestags=6,2,11 shows series with either of the series tags with ids 6, 2 and 11.</param>
    member this.GetMetadataStudiesImagesById(id: int64, ?sources: string, ?seriestypes: string, ?seriestags: string) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  if sources.IsSome then
                      RequestPart.query ("sources", sources.Value)
                  if seriestypes.IsSome then
                      RequestPart.query ("seriestypes", seriestypes.Value)
                  if seriestags.IsSome then
                      RequestPart.query ("seriestags", seriestags.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/metadata/studies/{id}/images" headers requestParts

            match int status with
            | 200 -> return GetMetadataStudiesImagesById.OK(Serializer.deserialize content)
            | _ -> return GetMetadataStudiesImagesById.NotFound
        }

    ///<summary>
    ///get a list of DICOM SCPs. Each SCP is a server for receiving DICOM images from e.g. a PACS system.
    ///</summary>
    ///<param name="startindex">start index of returned slice of SCPs</param>
    ///<param name="count">size of returned slice of SCPs</param>
    member this.GetScps(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/scps" headers requestParts
            return GetScps.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new SCP for receiving DICOM images
    ///</summary>
    member this.PostScps(scp: scp) =
        async {
            let requestParts = [ RequestPart.jsonContent scp ]
            let! (status, content) = OpenApiHttp.postAsync url "/scps" headers requestParts

            match int status with
            | 201 -> return PostScps.Created(Serializer.deserialize content)
            | _ -> return PostScps.BadRequest
        }

    ///<summary>
    ///shut down and remove the SCP corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of SCP to remove</param>
    member this.DeleteScpsById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/scps/{id}" headers requestParts
            return DeleteScpsById.NoContent
        }

    ///<summary>
    ///get a list of DICOM SCUs. Each SCU is a client for sending DICOM images to an SCP, e.g. a PACS system.
    ///</summary>
    ///<param name="startindex">start index of returned slice of SCUs</param>
    ///<param name="count">size of returned slice of SCUs</param>
    member this.GetScus(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/scus" headers requestParts
            return GetScus.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new SCU for sending DICOM images
    ///</summary>
    member this.PostScus(scu: scu) =
        async {
            let requestParts = [ RequestPart.jsonContent scu ]
            let! (status, content) = OpenApiHttp.postAsync url "/scus" headers requestParts

            match int status with
            | 201 -> return PostScus.Created(Serializer.deserialize content)
            | _ -> return PostScus.BadRequest
        }

    ///<summary>
    ///remove the SCU corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of SCU to remove</param>
    member this.DeleteScusById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/scus/{id}" headers requestParts
            return DeleteScusById.NoContent
        }

    ///<summary>
    ///send the images with the supplied image IDs to a DICOM SCP using the the SCU with the supplied scu ID
    ///</summary>
    ///<param name="id">id of SCU to use for sending</param>
    ///<param name="imageids"></param>
    member this.PostScusSendById(id: int64, imageids: list<int64>) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent imageids ]

            let! (status, content) = OpenApiHttp.postAsync url "/scus/{id}/send" headers requestParts

            match int status with
            | 204 -> return PostScusSendById.NoContent
            | _ -> return PostScusSendById.NotFound
        }

    ///<summary>
    ///get a list of all added series types. By filtering search results for certain series types, it is easier for applications to ensure that they read images of applicable types.
    ///</summary>
    ///<param name="startindex">start index of returned slice of series types</param>
    ///<param name="count">size of returned slice of series types</param>
    member this.GetSeriestypes(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/seriestypes" headers requestParts
            return GetSeriestypes.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new series type
    ///</summary>
    member this.PostSeriestypes(seriesType: seriestype) =
        async {
            let requestParts = [ RequestPart.jsonContent seriesType ]
            let! (status, content) = OpenApiHttp.postAsync url "/seriestypes" headers requestParts
            return PostSeriestypes.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///get a list of rules for assigning series types to series. A rule connects to a series of attributes with values and a resulting series type. If a series has the required values of the listed attributes, it is assigned to the series type of the rule.
    ///</summary>
    ///<param name="seriestypeid">ID of series type to list rules for</param>
    member this.GetSeriestypesRules(seriestypeid: int64) =
        async {
            let requestParts =
                [ RequestPart.query ("seriestypeid", seriestypeid) ]

            let! (status, content) = OpenApiHttp.getAsync url "/seriestypes/rules" headers requestParts
            return GetSeriestypesRules.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new series type rule
    ///</summary>
    member this.PostSeriestypesRules(seriesTypeRule: seriestyperule) =
        async {
            let requestParts =
                [ RequestPart.jsonContent seriesTypeRule ]

            let! (status, content) = OpenApiHttp.postAsync url "/seriestypes/rules" headers requestParts
            return PostSeriestypesRules.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///get the status of the internal process of updating series types for series following a change of series types, rules or attributes.
    ///</summary>
    member this.GetSeriestypesRulesUpdatestatus() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/seriestypes/rules/updatestatus" headers requestParts
            return GetSeriestypesRulesUpdatestatus.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///remove the series type rule corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of series type rule to remove</param>
    member this.DeleteSeriestypesRulesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/seriestypes/rules/{id}" headers requestParts
            return DeleteSeriestypesRulesById.NoContent
        }

    ///<summary>
    ///get the list of attributes for the series type rule with the supplied ID.
    ///</summary>
    ///<param name="id">index of series type rule to list rule attributes for</param>
    member this.GetSeriestypesRulesAttributesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.getAsync url "/seriestypes/rules/{id}/attributes" headers requestParts
            return GetSeriestypesRulesAttributesById.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///add a new series type rule attribute
    ///</summary>
    ///<param name="id">ID of rule</param>
    ///<param name="seriesTypeRuleAttribute"></param>
    member this.PostSeriestypesRulesAttributesById(id: int64, seriesTypeRuleAttribute: seriestyperuleattribute) =
        async {
            let requestParts =
                [ RequestPart.path ("id", id)
                  RequestPart.jsonContent seriesTypeRuleAttribute ]

            let! (status, content) = OpenApiHttp.postAsync url "/seriestypes/rules/{id}/attributes" headers requestParts
            return PostSeriestypesRulesAttributesById.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///remove the series type rule attribute corresponding to the supplied series type and attribute IDs
    ///</summary>
    ///<param name="ruleId">id of series type rule for which to remove an attribute</param>
    ///<param name="attributeId">id of attribute to remove</param>
    member this.DeleteSeriestypesRulesAttributesByRuleIdAndAttributeId(ruleId: int64, attributeId: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("ruleId", ruleId)
                  RequestPart.path ("attributeId", attributeId) ]

            let! (status, content) =
                OpenApiHttp.deleteAsync url "/seriestypes/rules/{ruleId}/attributes/{attributeId}" headers requestParts

            return DeleteSeriestypesRulesAttributesByRuleIdAndAttributeId.NoContent
        }

    ///<summary>
    ///submit a query for seriestypes for a list of series
    ///</summary>
    member this.PostSeriestypesSeriesQuery(query: idsquery) =
        async {
            let requestParts = [ RequestPart.jsonContent query ]
            let! (status, content) = OpenApiHttp.postAsync url "/seriestypes/series/query" headers requestParts
            return PostSeriestypesSeriesQuery.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///remove the series type corresponding to the supplied ID
    ///</summary>
    ///<param name="id">id of series type to remove</param>
    member this.DeleteSeriestypesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/seriestypes/{id}" headers requestParts
            return DeleteSeriestypesById.NoContent
        }

    ///<summary>
    ///request an asynchronous update of all series, labelling appropriate series with the series type corresponding to the supplied ID.
    ///</summary>
    ///<param name="id">id of series type to update series labels for</param>
    member this.PutSeriestypesById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.putAsync url "/seriestypes/{id}" headers requestParts
            return PutSeriestypesById.NoContent
        }

    ///<summary>
    ///Returns a list of currently available data sources. Possible source types are user - data imported by an API call by a user, box - data received from a remote box, directory - data imported via a watched directory, import - data imported into slicebox using import sessions, or scp - data received from a PACS.
    ///</summary>
    member this.GetSources() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/sources" headers requestParts
            return GetSources.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///No-op route for checking whether the service is alive or not
    ///</summary>
    member this.GetSystemHealth() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/system/health" headers requestParts
            return GetSystemHealth.OK
        }

    ///<summary>
    ///stop and shut down slicebox
    ///</summary>
    member this.PostSystemStop() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/system/stop" headers requestParts
            return PostSystemStop.OK
        }

    ///<summary>
    ///add an image (dataset) as part of a transaction. This method is used when sending images using the push method to a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the client's outgoing transaction</param>
    ///<param name="sequencenumber">the index of this image in the transaction</param>
    ///<param name="totalimagecount">the total number of images in this transaction</param>
    ///<param name="requestBody"></param>
    member this.PostTransactionsImageByToken
        (
            token: string,
            transactionid: int64,
            sequencenumber: int64,
            totalimagecount: int64,
            ?requestBody: byte []
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid)
                  RequestPart.query ("sequencenumber", sequencenumber)
                  RequestPart.query ("totalimagecount", totalimagecount)
                  if requestBody.IsSome then
                      RequestPart.binaryContent requestBody.Value ]

            let! (status, content) = OpenApiHttp.postAsync url "/transactions/{token}/image" headers requestParts

            match int status with
            | 204 -> return PostTransactionsImageByToken.NoContent
            | _ -> return PostTransactionsImageByToken.Unauthorized
        }

    ///<summary>
    ///fetch an image from the connected box as part of a transaction. This method is used when sending images using the poll method from a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the outgoing transaction</param>
    ///<param name="imageid">the ID of the outgoing transaction image</param>
    member this.GetTransactionsOutgoingByToken(token: string, transactionid: int64, imageid: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid)
                  RequestPart.query ("imageid", imageid) ]

            let! (status, contentBinary) =
                OpenApiHttp.getBinaryAsync url "/transactions/{token}/outgoing" headers requestParts

            match int status with
            | 200 -> return GetTransactionsOutgoingByToken.OK contentBinary
            | 401 -> return GetTransactionsOutgoingByToken.Unauthorized contentBinary
            | _ -> return GetTransactionsOutgoingByToken.NotFound contentBinary
        }

    ///<summary>
    ///signal that the supplied outgoing transaction and image was successfully received and can be marked as sent. This method is used when sending images using the poll method from a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="outgoing entry and image information block"></param>
    member this.PostTransactionsOutgoingDoneByToken
        (
            token: string,
            ``outgoing entry and image information block``: outgoingTransactionImage
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.jsonContent ``outgoing entry and image information block`` ]

            let! (status, content) =
                OpenApiHttp.postAsync url "/transactions/{token}/outgoing/done" headers requestParts

            match int status with
            | 204 -> return PostTransactionsOutgoingDoneByToken.NoContent
            | _ -> return PostTransactionsOutgoingDoneByToken.Unauthorized
        }

    ///<summary>
    ///signal that the image corresponding to the supplied outgoing transaction and image could not be read or stored properly on the receiving side, and that the transaction should be marked as failed.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="outgoing transaction and image, and error message"></param>
    member this.PostTransactionsOutgoingFailedByToken
        (
            token: string,
            ``outgoing transaction and image, and error message``: failedOutgoingTransactionImage
        ) =
        async {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.jsonContent ``outgoing transaction and image, and error message`` ]

            let! (status, content) =
                OpenApiHttp.postAsync url "/transactions/{token}/outgoing/failed" headers requestParts

            match int status with
            | 204 -> return PostTransactionsOutgoingFailedByToken.NoContent
            | _ -> return PostTransactionsOutgoingFailedByToken.Unauthorized
        }

    ///<summary>
    ///get next outgoing transaction and image (information on the next image that the connected box wishes to send to you), if any. This method is used when sending images using the poll method from a public slicebox.
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    member this.GetTransactionsOutgoingPollByToken(token: string) =
        async {
            let requestParts = [ RequestPart.path ("token", token) ]
            let! (status, content) = OpenApiHttp.getAsync url "/transactions/{token}/outgoing/poll" headers requestParts

            match int status with
            | 200 -> return GetTransactionsOutgoingPollByToken.OK(Serializer.deserialize content)
            | 401 -> return GetTransactionsOutgoingPollByToken.Unauthorized
            | _ -> return GetTransactionsOutgoingPollByToken.NotFound
        }

    ///<summary>
    ///get the status of the remote incoming transaction with the supplied transaction ID
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the client's outgoing transaction</param>
    member this.GetTransactionsStatusByToken(token: string, transactionid: int64) =
        async {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid) ]

            let! (status, content) = OpenApiHttp.getAsync url "/transactions/{token}/status" headers requestParts

            match int status with
            | 200 -> return GetTransactionsStatusByToken.OK
            | 401 -> return GetTransactionsStatusByToken.Unauthorized
            | _ -> return GetTransactionsStatusByToken.NotFound
        }

    ///<summary>
    ///update the status of the transaction with the supplied ID
    ///</summary>
    ///<param name="token">authentication token identifying the current box-to-box connection</param>
    ///<param name="transactionid">the ID of the client's outgoing transaction</param>
    ///<param name="transaction status"></param>
    member this.PutTransactionsStatusByToken(token: string, transactionid: int64, ``transaction status``: string) =
        async {
            let requestParts =
                [ RequestPart.path ("token", token)
                  RequestPart.query ("transactionid", transactionid)
                  RequestPart.jsonContent ``transaction status`` ]

            let! (status, content) = OpenApiHttp.putAsync url "/transactions/{token}/status" headers requestParts

            match int status with
            | 204 -> return PutTransactionsStatusByToken.NoContent
            | _ -> return PutTransactionsStatusByToken.NotFound
        }

    ///<summary>
    ///Returns all users of slicebox
    ///</summary>
    ///<param name="startindex">start index of returned slice of users</param>
    ///<param name="count">size of returned slice of users</param>
    member this.GetUsers(?startindex: int64, ?count: int64) =
        async {
            let requestParts =
                [ if startindex.IsSome then
                      RequestPart.query ("startindex", startindex.Value)
                  if count.IsSome then
                      RequestPart.query ("count", count.Value) ]

            let! (status, content) = OpenApiHttp.getAsync url "/users" headers requestParts
            return GetUsers.OK(Serializer.deserialize content)
        }

    ///<summary>
    ///Creates a new user. Dupicates are accepted but not added.
    ///</summary>
    member this.PostUsers(user: newUser) =
        async {
            let requestParts = [ RequestPart.jsonContent user ]
            let! (status, content) = OpenApiHttp.postAsync url "/users" headers requestParts
            return PostUsers.Created(Serializer.deserialize content)
        }

    ///<summary>
    ///obtain information on the currently logged in user as specified by the supplied session cookie, IP address and user agent.
    ///</summary>
    member this.GetUsersCurrent() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.getAsync url "/users/current" headers requestParts

            match int status with
            | 200 -> return GetUsersCurrent.OK(Serializer.deserialize content)
            | _ -> return GetUsersCurrent.NotFound
        }

    ///<summary>
    ///Obtain a session cookie that can be used to authenticate future API calls from the present IP address and with the present user agent.
    ///</summary>
    member this.PostUsersLogin(userPass: userPass) =
        async {
            let requestParts = [ RequestPart.jsonContent userPass ]
            let! (status, content) = OpenApiHttp.postAsync url "/users/login" headers requestParts

            match int status with
            | 201 -> return PostUsersLogin.Created
            | _ -> return PostUsersLogin.Unauthorized
        }

    ///<summary>
    ///Logout the current user by responding with a delete cookie header removing the session cookie for this user.
    ///</summary>
    member this.PostUsersLogout() =
        async {
            let requestParts = []
            let! (status, content) = OpenApiHttp.postAsync url "/users/logout" headers requestParts
            return PostUsersLogout.Created
        }

    ///<summary>
    ///deletes a single user based on the ID supplied
    ///</summary>
    ///<param name="id">ID of user to delete</param>
    member this.DeleteUsersById(id: int64) =
        async {
            let requestParts = [ RequestPart.path ("id", id) ]
            let! (status, content) = OpenApiHttp.deleteAsync url "/users/{id}" headers requestParts
            return DeleteUsersById.NoContent
        }
