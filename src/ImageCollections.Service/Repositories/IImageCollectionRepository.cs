using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;

namespace ImageCollections.Service.Repositories
{
    public interface IImageCollectionRepository
    {
        Task<IEnumerable<ImageCollectionInternal>> GetCollectionsList(string name, int fetch, int offset);

        Task<ImageCollectionInternal> GetCollection(long id);

        Task<ImageCollectionInternal> CreateCollection(string name);

        Task UpdateCollection(long id, string name);

        Task DeleteCollection(long id);

        Task<ImageInfoInternal> UploadImage(string name, string content, string contentType, int height, int width, string xResolution, string yResolution, string dateTime);

        Task<ImageInfoInternal> GetImage(long id);

        Task<IEnumerable<ImageInfoInternal>> GetImageList(string name, int fetch, int offset);

        Task UpdateImageInfo(long id, string name);

        Task AddImageToCollection(long collectionId, long imageId);

        Task RemoveImageFromCollection(long collectionId, long imageId);

        Task DeleteImage(long id);
    }
}