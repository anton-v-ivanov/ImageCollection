using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;
using ImageCollections.Service.Repositories;
using Serilog;

namespace ImageCollections.Service.Managers
{
    public class ImageCollectionManager: IImageCollectionManager 
    {
        private readonly IImageCollectionRepository _imageCollectionRepository;

        public ImageCollectionManager(IImageCollectionRepository imageCollectionRepository)
        {
            _imageCollectionRepository = imageCollectionRepository;
        }

        public async Task<List<ImageCollectionInternal>> GetCollectionsList(CollectionSearchRequestInternal request)
        {
            var result = await _imageCollectionRepository.GetCollectionsList(request.Name, request.Fetch, request.Offset);
            return result.ToList();
        }

        public async Task<ImageCollectionInternal> GetCollection(CollectionGetRequestInternal request)
        {
            var collection = await _imageCollectionRepository.GetCollection(request.Id);
            return collection ?? new ImageCollectionInternal();
        }

        public async Task<ImageCollectionInternal> CreateCollection(CollectionCreateRequestInternal request)
        {
            return await _imageCollectionRepository.CreateCollection(request.Name);
        }

        public async Task<UpdateDeleteResponseInternal> UpdateCollection(CollectionUpdateRequestInternal request)
        {
            try
            {
                await _imageCollectionRepository.UpdateCollection(request.Id, request.Name);
                return new UpdateDeleteResponseInternal { Success = true };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unable to update collection with id = {id}", request.Id);
                return new UpdateDeleteResponseInternal
                {
                    Success = false,
                    Error = exception.Message
                };
            }
        }

        public async Task<UpdateDeleteResponseInternal> DeleteCollection(CollectionDeleteRequestInternal request)
        {
            try
            {
                await _imageCollectionRepository.DeleteCollection(request.Id);
                return new UpdateDeleteResponseInternal
                {
                    Success = true
                };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unable delete collection with id = {id}", request.Id);
                return new UpdateDeleteResponseInternal
                {
                    Success = false,
                    Error = exception.Message
                };
            }
        }

        public async Task<ImageInfoInternal> UploadFile(UploadFileRequestInternal request)
        {
            var result = await _imageCollectionRepository.UploadImage(request.Name, request.Path, request.ContentType, request.Height, request.Width,
                request.XResolution, request.YResolution, request.DateTime);
            return result;
        }

        public async Task<ImageInfoInternal> GetImage(GetImageRequestInternal request)
        {
            var result = await _imageCollectionRepository.GetImage(request.Id);
            return result ?? new ImageInfoInternal();
        }

        public async Task<List<ImageInfoInternal>> GetImageList(GetImageListRequestInternal request)
        {
            var result = await _imageCollectionRepository.GetImageList(request.Name, request.Fetch, request.Offset);
            return result.ToList();
        }

        public async Task<UpdateDeleteResponseInternal> UpdateImageInfo(UpdateImageInfoRequestInternal request)
        {
            try
            {
                await _imageCollectionRepository.UpdateImageInfo(request.Id, request.Name);
                return new UpdateDeleteResponseInternal { Success = true };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unable to update image info with id = {id}", request.Id);
                return new UpdateDeleteResponseInternal
                {
                    Success = false,
                    Error = exception.Message
                };
            }
        }

        public async Task<UpdateDeleteResponseInternal> DeleteImage(DeleteImageRequestInternal request)
        {
            try
            {
                await _imageCollectionRepository.DeleteImage(request.Id);
                return new UpdateDeleteResponseInternal { Success = true };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unable to delte image with id = {id}", request.Id);
                return new UpdateDeleteResponseInternal
                {
                    Success = false,
                    Error = exception.Message
                };
            }
        }

        public async Task<UpdateDeleteResponseInternal> AddImageToCollection(AddImageToCollectionRequestInternal request)
        {
            try
            {
                await _imageCollectionRepository.AddImageToCollection(request.CollectionId, request.ImageId);
                return new UpdateDeleteResponseInternal { Success = true };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unable to add image with id = {imageId} to collection with id = {collectionId}", request.ImageId, request.CollectionId);
                return new UpdateDeleteResponseInternal
                {
                    Success = false,
                    Error = exception.Message
                };
            }
        }

        public async Task<UpdateDeleteResponseInternal> RemoveImageFromCollection(RemoveImageFromCollectionRequestInternal request)
        {
            try
            {
                await _imageCollectionRepository.RemoveImageFromCollection(request.CollectionId, request.ImageId);
                return new UpdateDeleteResponseInternal { Success = true };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unable to remove image with id = {imageId} from collection with id = {collectionId}", request.ImageId, request.CollectionId);
                return new UpdateDeleteResponseInternal
                {
                    Success = false,
                    Error = exception.Message
                };
            }
        }
    }
}