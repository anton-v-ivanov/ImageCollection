namespace ImageCollections.Contracts.ImageCollections
{
    public class AddImageToCollectionRequest
    {
        private long ImageId { get; set; }
        public long CollectionId { get; set; }

        public AddImageToCollectionRequest(long collectionId, long imageId)
        {
            CollectionId = collectionId;
            ImageId = imageId;
        }
    }
}