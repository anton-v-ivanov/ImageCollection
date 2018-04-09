using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.WebApi.Models;

namespace ImageCollections.WebApi.Managers
{
    public class CollectionManager : ICollectionManager
    {
        private readonly IBus _bus;

        public CollectionManager(IBus bus)
        {
            _bus = bus;
        }

        public async Task<List<ImageCollectionInternal>> GetCollectionsList(string name, int fetch, int offset)
        {
            var request = new CollectionSearchRequestInternal(name, fetch, offset);
            var response = await _bus.RequestAsync<CollectionSearchRequestInternal, List<ImageCollectionInternal>>(request);
            return response;
        }

        public async Task<ImageCollectionInternal> GetCollection(long id)
        {
            var request = new CollectionGetRequestInternal(id);
            var response = await _bus.RequestAsync<CollectionGetRequestInternal, ImageCollectionInternal>(request);
            // we have to pass empty value instead of null because of bug in EasyNetQ https://github.com/EasyNetQ/EasyNetQ/issues/111
            return response.Id == 0 ? null : response;
        }

        public async Task<ImageCollectionInternal> CreateCollection(CreateCollectionRequest createRequest)
        {
            var request = new CollectionCreateRequestInternal(createRequest.Name);
            var response = await _bus.RequestAsync<CollectionCreateRequestInternal, ImageCollectionInternal>(request);
            return response;
        }

        public async Task<UpdateDeleteActionResponse> UpdateCollection(long id, UpdateCollectionRequest updateCollectionRequest)
        {
            var collection = await GetCollection(id);
            if(collection == null)
                return new UpdateDeleteActionResponse(false, $"Collection with id {id} doesn't exists");

            var request = new CollectionUpdateRequestInternal(id, updateCollectionRequest.Name);
            var response = await _bus.RequestAsync<CollectionUpdateRequestInternal, UpdateDeleteResponseInternal>(request);
            // we have to pass empty value instead of null because of bug in EasyNetQ https://github.com/EasyNetQ/EasyNetQ/issues/111
            return new UpdateDeleteActionResponse(response);
        }

        public async Task<UpdateDeleteActionResponse> DeleteCollection(long id)
        {
            var request = new CollectionDeleteRequestInternal(id);
            var response = await _bus.RequestAsync<CollectionDeleteRequestInternal, UpdateDeleteResponseInternal>(request);
            return new UpdateDeleteActionResponse(response);
        }

        public async Task<UpdateDeleteActionResponse> RemoveImageFromCollection(long collectionId, long imageId)
        {
            var request = new RemoveImageFromCollectionRequestInternal(collectionId, imageId);
            var response = await _bus.RequestAsync<RemoveImageFromCollectionRequestInternal, UpdateDeleteResponseInternal>(request);
            return new UpdateDeleteActionResponse(response);
        }

        public async Task<UpdateDeleteActionResponse> AddImageToCollection(long collectionId, AddImageToCollectionRequest addImageModel)
        {
            var request = new AddImageToCollectionRequestInternal(collectionId, addImageModel.ImageId);
            var response = await _bus.RequestAsync<AddImageToCollectionRequestInternal, UpdateDeleteResponseInternal>(request);
            return new UpdateDeleteActionResponse(response);
        }
    }
}