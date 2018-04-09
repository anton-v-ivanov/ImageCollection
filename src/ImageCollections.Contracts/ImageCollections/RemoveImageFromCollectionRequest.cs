namespace ImageCollections.Contracts.ImageCollections
{
    public class RemoveImageFromCollectionRequest
    {
        public long CollectionId { get; set; }
        public long ImageId { get; set; }

        public RemoveImageFromCollectionRequest(long collectionId, long imageId)
        {
            CollectionId = collectionId;
            ImageId = imageId;
        }
    }
}