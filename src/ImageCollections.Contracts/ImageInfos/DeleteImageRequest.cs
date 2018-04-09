namespace ImageCollections.Contracts.ImageInfos
{
    public class DeleteImageRequest
    {
        public long Id { get; set; }

        public DeleteImageRequest(long id)
        {
            Id = id;
        }
    }
}