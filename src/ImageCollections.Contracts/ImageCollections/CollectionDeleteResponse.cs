namespace ImageCollections.Contracts.ImageCollections
{
    public class CollectionDeleteResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public CollectionDeleteResponse()
        {
            Success = true;
        }

        public CollectionDeleteResponse(bool success, string error)
        {
            Success = success;
            Error = error;
        }
    }
}