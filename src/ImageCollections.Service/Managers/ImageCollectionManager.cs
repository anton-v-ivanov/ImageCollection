using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;
using ImageCollections.Service.Repositories;

namespace ImageCollections.Service.Managers
{
    public class ImageCollectionManager: IImageCollectionManager 
    {
        private readonly IImageCollectionRepository _imageCollectionRepository;

        public ImageCollectionManager(IImageCollectionRepository imageCollectionRepository)
        {
            _imageCollectionRepository = imageCollectionRepository;
        }

        public async Task<List<ImageCollection>> Search(CollectionSearchRequest request)
        {
            var result = await _imageCollectionRepository.Search(request.Name, request.Fetch, request.Offset);
            return result.ToList();
        }

        public async Task<ImageCollection> Get(CollectionGetRequest request)
        {
            var collection = await _imageCollectionRepository.GetCollection(request.Id);
            return collection ?? new ImageCollection();
        }

        public async Task<ImageCollection> Create(CollectionCreateRequest request)
        {
            return await _imageCollectionRepository.Create(request.Name);
        }

        public async Task<ImageCollection> Update(CollectionUpdateRequest request)
        {
            return await _imageCollectionRepository.Update(request.Id, request.Name);
        }

        public async Task<CollectionDeleteResponse> Delete(CollectionDeleteRequest request)
        {
            return await _imageCollectionRepository.Delete(request.Id);
        }

        public async Task<ImageInfo> Upload(UploadFileRequest request)
        {
            var result = await _imageCollectionRepository.UploadImage(request.Name, request.Path, request.ContentType, request.Height, request.Width);
            return result;
        }

        public async Task<ImageInfo> GetImage(GetImageRequest request)
        {
            var result = await _imageCollectionRepository.GetImage(request.Id);
            return result;
        }

        public async Task<List<ImageInfo>> GetImageList(GetImageListRequest request)
        {
            var result = await _imageCollectionRepository.GetImageList(request.Name, request.Fetch, request.Offset);
            return result.ToList();
        }
    }
}