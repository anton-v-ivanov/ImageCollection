namespace ImageCollections.Contracts.ImageInfos
{
    public class ImageInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}