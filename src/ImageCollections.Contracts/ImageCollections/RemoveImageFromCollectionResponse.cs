namespace ImageCollections.Contracts.ImageCollections
{
    public class RemoveImageFromCollectionResponse
    {
        public bool Success { get; set; }

        public RemoveImageFromCollectionResponse()
        {
        }

        public RemoveImageFromCollectionResponse(bool success)
        {
            Success = success;
        }
    }
}