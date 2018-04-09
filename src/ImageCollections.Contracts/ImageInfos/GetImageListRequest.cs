namespace ImageCollections.Contracts.ImageInfos
{
    public class GetImageListRequest
    {
        public string Name { get; set; }
        public int Fetch{ get; set; }
        public int Offset{ get; set; }

        public GetImageListRequest(string name, int fetch, int offset)
        {
            Name = name;
            Fetch = fetch;
            Offset = offset;
        }
    }
}