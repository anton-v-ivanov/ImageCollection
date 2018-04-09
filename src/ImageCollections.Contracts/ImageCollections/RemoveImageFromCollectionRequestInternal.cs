namespace ImageCollections.Contracts.ImageCollections
{
    public class RemoveImageFromCollectionRequestInternal
    {
        public long CollectionId { get; set; }
        public long ImageId { get; set; }

        public RemoveImageFromCollectionRequestInternal(long collectionId, long imageId)
        {
            CollectionId = collectionId;
            ImageId = imageId;
        }
    }
}