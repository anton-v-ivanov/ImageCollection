namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionDeleteRequestInternal
    {
        public long Id { get; set; }

        public CollectionDeleteRequestInternal(long id)
        {
            Id = id;
        }
    }
}