using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;

namespace ImageCollections.Service.Managers
{
    public interface IImageCollectionManager
    {
        Task<List<ImageCollection>> Search(CollectionSearchRequest request);

        Task<ImageCollection> Get(CollectionGetRequest request);

        Task<ImageCollection> Create(CollectionCreateRequest request);

        Task<ImageCollection> Update(CollectionUpdateRequest request);

        Task<CollectionDeleteResponse> Delete(CollectionDeleteRequest request);

        Task<ImageInfo> Upload(UploadFileRequest request);

        Task<ImageInfo> GetImage(GetImageRequest request);

        Task<List<ImageInfo>> GetImageList(GetImageListRequest request);
    }
}