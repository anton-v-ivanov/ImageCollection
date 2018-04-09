namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionUpdateRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public CollectionUpdateRequest(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}