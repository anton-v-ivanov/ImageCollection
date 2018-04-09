namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionCreateRequest
    {
        public string Name { get; set; }

        public CollectionCreateRequest(string name)
        {
            Name = name;
        }
    }
}