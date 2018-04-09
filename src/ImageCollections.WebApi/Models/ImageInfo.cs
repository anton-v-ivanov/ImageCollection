namespace ImageCollections.WebApi.Models
{
    public class ImageInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public int Height { get; set; }
        public int Weigth { get; set; }
    }
}