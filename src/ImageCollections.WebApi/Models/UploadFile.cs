using Microsoft.AspNetCore.Http;

namespace ImageCollections.WebApi.Models
{
    public class UploadFileRequest
    {
        public IFormFileCollection Files { get; set; }
    }
}