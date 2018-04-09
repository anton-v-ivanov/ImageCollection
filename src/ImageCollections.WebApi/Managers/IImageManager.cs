using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.WebApi.Models;
using Microsoft.AspNetCore.Http;

namespace ImageCollections.WebApi.Managers
{
    public interface IImageManager
    {
        Task<List<Contracts.ImageInfos.ImageInfo>> GetList(string name, int fetch, int offset);

        Task<FileContentResponse> GetContent(long id);

        Task<Contracts.ImageInfos.ImageInfo> GetInfo(long id);

        Task<bool> Delete(long id);

        Task<Contracts.ImageInfos.ImageInfo> Upload(IFormFile file);

        Task<Contracts.ImageInfos.ImageInfo> UpdateImageInfo(UpdateImageRequest request);
    }
}