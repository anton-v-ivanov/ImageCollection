namespace ImageCollections.Contracts.ImageInfos
{
    public class GetImageListRequestInternal
    {
        public string Name { get; set; }
        public int Fetch{ get; set; }
        public int Offset{ get; set; }

        public GetImageListRequestInternal(string name, int fetch, int offset)
        {
            Name = name;
            Fetch = fetch;
            Offset = offset;
        }
    }
}