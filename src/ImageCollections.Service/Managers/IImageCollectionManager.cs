using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;

namespace ImageCollections.Service.Managers
{
    public interface IImageCollectionManager
    {
        Task<List<ImageCollectionInternal>> GetCollectionsList(CollectionSearchRequestInternal request);

        Task<ImageCollectionInternal> GetCollection(CollectionGetRequestInternal request);

        Task<ImageCollectionInternal> CreateCollection(CollectionCreateRequestInternal request);

        Task<UpdateDeleteResponseInternal> UpdateCollection(CollectionUpdateRequestInternal request);

        Task<UpdateDeleteResponseInternal> DeleteCollection(CollectionDeleteRequestInternal request);

        Task<ImageInfoInternal> UploadFile(UploadFileRequestInternal request);

        Task<ImageInfoInternal> GetImage(GetImageRequestInternal request);

        Task<List<ImageInfoInternal>> GetImageList(GetImageListRequestInternal request);

        Task<UpdateDeleteResponseInternal> UpdateImageInfo(UpdateImageInfoRequestInternal request);

        Task<UpdateDeleteResponseInternal> DeleteImage(DeleteImageRequestInternal request);

        Task<UpdateDeleteResponseInternal> AddImageToCollection(AddImageToCollectionRequestInternal request);

        Task<UpdateDeleteResponseInternal> RemoveImageFromCollection(RemoveImageFromCollectionRequestInternal request);
    }
}