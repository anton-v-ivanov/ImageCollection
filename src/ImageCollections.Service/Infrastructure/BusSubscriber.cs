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
            _bus.RespondAsync<CollectionSearchRequestInternal, List<ImageCollectionInternal>>(request => _imageCollectionManager.GetCollectionsList(request));

            _bus.RespondAsync<CollectionGetRequestInternal, ImageCollectionInternal>(request => _imageCollectionManager.GetCollection(request));

            _bus.RespondAsync<CollectionCreateRequestInternal, ImageCollectionInternal>(request => _imageCollectionManager.CreateCollection(request));

            _bus.RespondAsync<CollectionUpdateRequestInternal, UpdateDeleteResponseInternal>(request => _imageCollectionManager.UpdateCollection(request));

            _bus.RespondAsync<CollectionDeleteRequestInternal, UpdateDeleteResponseInternal>(request => _imageCollectionManager.DeleteCollection(request));

            _bus.RespondAsync<UploadFileRequestInternal, ImageInfoInternal>(request => _imageCollectionManager.UploadFile(request));

            _bus.RespondAsync<GetImageRequestInternal, ImageInfoInternal>(request => _imageCollectionManager.GetImage(request));

            _bus.RespondAsync<GetImageListRequestInternal, List<ImageInfoInternal>>(request => _imageCollectionManager.GetImageList(request));

            _bus.RespondAsync<UpdateImageInfoRequestInternal, UpdateDeleteResponseInternal>(request => _imageCollectionManager.UpdateImageInfo(request));

            _bus.RespondAsync<DeleteImageRequestInternal, UpdateDeleteResponseInternal>(request => _imageCollectionManager.DeleteImage(request));

            _bus.RespondAsync<AddImageToCollectionRequestInternal, UpdateDeleteResponseInternal>(request => _imageCollectionManager.AddImageToCollection(request));

            _bus.RespondAsync<RemoveImageFromCollectionRequestInternal, UpdateDeleteResponseInternal>(request => _imageCollectionManager.RemoveImageFromCollection(request));

            return _bus;
        }
    }
}