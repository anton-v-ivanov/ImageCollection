namespace ImageCollections.Contracts.ImageInfos
{
    public class GetImageRequest
    {
        public long Id { get; set; }

        public GetImageRequest(long id)
        {
            Id = id;
        }
    }
}