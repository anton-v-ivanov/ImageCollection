namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionSearchRequest
    {
        public string Name { get; set; }
        public int Fetch { get; set; }
        public int Offset { get; set; }

        public CollectionSearchRequest(string name, int fetch, int offset)
        {
            Name = name;
            Fetch = fetch;
            Offset = offset;
        }
    }
}