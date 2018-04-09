namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionDeleteRequest
    {
        public long Id { get; set; }

        public CollectionDeleteRequest(long id)
        {
            Id = id;
        }
    }
}