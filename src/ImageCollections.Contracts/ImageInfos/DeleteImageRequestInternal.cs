namespace ImageCollections.Contracts.ImageInfos
{
    public class DeleteImageRequestInternal
    {
        public long Id { get; set; }

        public DeleteImageRequestInternal(long id)
        {
            Id = id;
        }
    }
}