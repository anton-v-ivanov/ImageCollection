namespace ImageCollections.Contracts.ImageCollections
{
    public class AddImageToCollectionRequestInternal
    {
        public long ImageId { get; set; }
        public long CollectionId { get; set; }

        public AddImageToCollectionRequestInternal(long collectionId, long imageId)
        {
            CollectionId = collectionId;
            ImageId = imageId;
        }
    }
}