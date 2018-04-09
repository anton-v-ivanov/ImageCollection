namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionGetRequest
    {
        public long Id { get; set; }

        public CollectionGetRequest(long id)
        {
            Id = id;
        }
    }
}