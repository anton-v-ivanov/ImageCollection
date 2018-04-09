namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionGetRequestInternal
    {
        public long Id { get; set; }

        public CollectionGetRequestInternal(long id)
        {
            Id = id;
        }
    }
}