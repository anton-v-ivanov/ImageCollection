using ImageCollections.Contracts.ImageCollections;

namespace ImageCollections.WebApi.Models
{
    public class UpdateDeleteActionResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public UpdateDeleteActionResponse(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public UpdateDeleteActionResponse(UpdateDeleteResponseInternal internalResponse)
        {
            Success = internalResponse.Success;
            Error = internalResponse.Error;
        }
    }
}