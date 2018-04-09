namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionUpdateRequestInternal
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public CollectionUpdateRequestInternal(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}