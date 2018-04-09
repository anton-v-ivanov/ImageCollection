using System.Collections.Generic;
using EasyNetQ;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;
using ImageCollections.Service.Managers;

namespace ImageCollections.Service.Infrastructure
{
    public class BusSubscriber: IBusSubscriber
    {
        private readonly IBus _bus;
        private readonly IImageCollectionManager _imageCollectionManager;

        public BusSubscriber(IBus bus, IImageCollectionManager imageCollectionManager)
        {
            _bus = bus;
            _imageCollectionManager = imageCollectionManager;
        }

        public IBus Subscribe()
        {
            _bus.RespondAsync<CollectionSearchRequest, List<ImageCollection>>(request => _imageCollectionManager.Search(request));
            _bus.RespondAsync<CollectionGetRequest, ImageCollection>(request => _imageCollectionManager.Get(request));
            _bus.RespondAsync<CollectionCreateRequest, ImageCollection>(request => _imageCollectionManager.Create(request));
            _bus.RespondAsync<CollectionUpdateRequest, ImageCollection>(request => _imageCollectionManager.Update(request));
            _bus.RespondAsync<CollectionDeleteRequest, CollectionDeleteResponse>(request => _imageCollectionManager.Delete(request));
            _bus.RespondAsync<UploadFileRequest, ImageInfo>(request => _imageCollectionManager.Upload(request));
            _bus.RespondAsync<GetImageRequest, ImageInfo>(request => _imageCollectionManager.GetImage(request));
            _bus.RespondAsync<GetImageListRequest, List<ImageInfo>>(request => _imageCollectionManager.GetImageList(request));
            return _bus;
        }
    }
}