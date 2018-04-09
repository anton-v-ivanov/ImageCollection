using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.WebApi.Models;
using AddImageToCollectionRequest = ImageCollections.WebApi.Models.AddImageToCollectionRequest;

namespace ImageCollections.WebApi.Managers
{
    public interface ICollectionManager
    {
        Task<List<Models.ImageCollection>> Search(string name, int fetch, int offset);

        Task<Models.ImageCollection> Get(long id);

        Task<Models.ImageCollection> Create(CreateCollectionRequest createRequest);

        Task<Models.ImageCollection> Update(long id, string name);

        Task<CollectionDeleteResponse> Delete(long id);

        Task<RemoveImageFromCollectionResponse> RemoveImage(long collectionId, long imageId);

        Task<ImageInfo> AddImageToCollection(long collectionId, AddImageToCollectionRequest addImageModel);
    }
}