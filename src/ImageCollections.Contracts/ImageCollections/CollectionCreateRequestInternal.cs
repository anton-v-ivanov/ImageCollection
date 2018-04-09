namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionCreateRequestInternal
    {
        public string Name { get; set; }

        public CollectionCreateRequestInternal(string name)
        {
            Name = name;
        }
    }
}