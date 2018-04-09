using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.WebApi.Models;
using Microsoft.AspNetCore.Http;

namespace ImageCollections.WebApi.Managers
{
    public interface IImageManager
    {
        Task<List<Contracts.ImageInfos.ImageInfoInternal>> GetList(string name, int fetch, int offset);

        Task<FileContentResponse> GetContent(long id);

        Task<Contracts.ImageInfos.ImageInfoInternal> GetInfo(long id);

        Task<UpdateDeleteActionResponse> Delete(long id);

        Task<Contracts.ImageInfos.ImageInfoInternal> Upload(IFormFile file);

        Task<UpdateDeleteActionResponse> UpdateImageInfo(long id, UpdateImageRequest request);
    }
}