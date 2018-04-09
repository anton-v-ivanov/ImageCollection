using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.WebApi.Models;
using AddImageToCollectionRequest = ImageCollections.WebApi.Models.AddImageToCollectionRequest;

namespace ImageCollections.WebApi.Managers
{
    public interface ICollectionManager
    {
        Task<List<ImageCollectionInternal>> GetCollectionsList(string name, int fetch, int offset);

        Task<ImageCollectionInternal> GetCollection(long id);

        Task<ImageCollectionInternal> CreateCollection(CreateCollectionRequest createRequest);

        Task<UpdateDeleteActionResponse> UpdateCollection(long id, UpdateCollectionRequest name);

        Task<UpdateDeleteActionResponse> DeleteCollection(long id);

        Task<UpdateDeleteActionResponse> AddImageToCollection(long collectionId, AddImageToCollectionRequest addImageModel);

        Task<UpdateDeleteActionResponse> RemoveImageFromCollection(long collectionId, long imageId);
    }
}