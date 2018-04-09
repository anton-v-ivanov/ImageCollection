using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.WebApi.Models;
using AddImageToCollectionRequest = ImageCollections.WebApi.Models.AddImageToCollectionRequest;
using ImageCollection = ImageCollections.Contracts.ImageCollections.ImageCollection;

namespace ImageCollections.WebApi.Managers
{
    public class CollectionManager : ICollectionManager
    {
        private readonly IBus _bus;

        public CollectionManager(IBus bus)
        {
            _bus = bus;
        }

        public async Task<List<Models.ImageCollection>> Search(string name, int fetch, int offset)
        {
            var request = new CollectionSearchRequest(name, fetch, offset);
            var response = await _bus.RequestAsync<CollectionSearchRequest, List<ImageCollection>>(request);
            return response.Select(s => new Models.ImageCollection(s)).ToList();
        }

        public async Task<Models.ImageCollection> Get(long id)
        {
            var request = new CollectionGetRequest(id);
            var response = await _bus.RequestAsync<CollectionGetRequest, Contracts.ImageCollections.ImageCollection>(request);
            // we have to pass empty value instead of null because of bug in EasyNetQ https://github.com/EasyNetQ/EasyNetQ/issues/111
            return response.Id == 0 ? null : new Models.ImageCollection(response);
        }

        public async Task<Models.ImageCollection> Create(CreateCollectionRequest createRequest)
        {
            var request = new CollectionCreateRequest(createRequest.Name);
            var response = await _bus.RequestAsync<CollectionCreateRequest, ImageCollection>(request);
            return new Models.ImageCollection(response);
        }

        public async Task<Models.ImageCollection> Update(long id, string name)
        {
            var request = new CollectionUpdateRequest(id, name);
            var response = await _bus.RequestAsync<CollectionUpdateRequest, ImageCollection>(request);
            return new Models.ImageCollection(response);
        }

        public async Task<CollectionDeleteResponse> Delete(long id)
        {
            var request = new CollectionDeleteRequest(id);
            var response = await _bus.RequestAsync<CollectionDeleteRequest, CollectionDeleteResponse>(request);
            return response;
        }

        public async Task<RemoveImageFromCollectionResponse> RemoveImage(long collectionId, long imageId)
        {
            var request = new RemoveImageFromCollectionRequest(collectionId, imageId);
            var response = await _bus.RequestAsync<RemoveImageFromCollectionRequest, RemoveImageFromCollectionResponse>(request);
            return response;
        }

        public async Task<ImageInfo> AddImageToCollection(long collectionId, AddImageToCollectionRequest addImageModel)
        {
            var request = new Contracts.ImageCollections.AddImageToCollectionRequest(collectionId, addImageModel.ImageId);
            var response = await _bus.RequestAsync<Contracts.ImageCollections.AddImageToCollectionRequest, ImageInfo>(request);
            return response;
        }
    }
}