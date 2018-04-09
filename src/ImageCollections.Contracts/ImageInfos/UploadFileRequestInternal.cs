namespace ImageCollections.Contracts.ImageInfos
{
    public class UploadFileRequestInternal
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string XResolution { get; set; }
        public string YResolution { get; set; }
        public string DateTime { get; set; }

        public UploadFileRequestInternal()
        {
        }

        public UploadFileRequestInternal(string name, string path, string contentType, int height, int width)
        {
            Name = name;
            Path = path;
            ContentType = contentType;
            Height = height;
            Width = width;
        }
    }
}