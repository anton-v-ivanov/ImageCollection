namespace ImageCollections.WebApi.Models
{
    public class ImageInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Info { get; set; }
        public string ContentType { get; set; }
        public int Height { get; set; }
        public int Weigth { get; set; }
        public string XResolution { get; set; }
        public string YResolution { get; set; }
        public string DateTime { get; set; }
    }
}