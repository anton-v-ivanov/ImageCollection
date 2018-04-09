namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionSearchRequestInternal
    {
        public string Name { get; set; }
        public int Fetch { get; set; }
        public int Offset { get; set; }

        public CollectionSearchRequestInternal(string name, int fetch, int offset)
        {
            Name = name;
            Fetch = fetch;
            Offset = offset;
        }
    }
}