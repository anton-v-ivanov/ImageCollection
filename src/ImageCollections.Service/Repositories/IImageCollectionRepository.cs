using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;

namespace ImageCollections.Service.Repositories
{
    public interface IImageCollectionRepository
    {
        Task<IEnumerable<ImageCollection>> Search(string name, int fetch, int offset);

        Task<ImageCollection> GetCollection(long id);

        Task<ImageCollection> Create(string name);

        Task<ImageCollection> Update(long id, string name);

        Task<CollectionDeleteResponse> Delete(long id);

        Task<ImageInfo> UploadImage(string name, string content, string contentType, int height, int width);

        Task<ImageInfo> GetImage(long id);

        Task<IEnumerable<ImageInfo>> GetImageList(string name, int fetch, int offset);
    }
}