namespace ImageCollections.Contracts.ImageInfos
{
    public class UploadFileRequest
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public UploadFileRequest()
        {
        }

        public UploadFileRequest(string name, string path, string contentType, int height, int width)
        {
            Name = name;
            Path = path;
            ContentType = contentType;
            Height = height;
            Width = width;
        }
    }
}