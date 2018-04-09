namespace ImageCollections.Contracts.ImageInfos
{
    public class GetImageRequestInternal
    {
        public long Id { get; set; }

        public GetImageRequestInternal(long id)
        {
            Id = id;
        }
    }
}