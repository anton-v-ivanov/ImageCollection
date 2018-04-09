using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EasyNetQ;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;
using ImageCollections.WebApi.Configuration;
using ImageCollections.WebApi.Managers.HashGenerator;
using ImageCollections.WebApi.Managers.StorageFactory;
using ImageCollections.WebApi.Models;
using ImageCollections.WebApi.Repositories;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Extensions.Options;
using ImageInfoInternal = ImageCollections.Contracts.ImageInfos.ImageInfoInternal;
using UpdateImageRequest = ImageCollections.WebApi.Models.UpdateImageRequest;
using UploadFileRequestInternal = ImageCollections.Contracts.ImageInfos.UploadFileRequestInternal;

namespace ImageCollections.WebApi.Managers
{
    public class ImageManager : IImageManager
    {
        private readonly IBus _bus;
        private readonly IHashGenerator _hashGenerator;
        private readonly IStorage _storage;

        public ImageManager(IBus bus, IHashGenerator hashGenerator, IOptions<FileRepositorySetting> settings, IStorageFactory storageFactory)
        {
            _bus = bus;
            _hashGenerator = hashGenerator;
            _storage = storageFactory.Create(settings.Value.ActiveStorage);
        }

        public async Task<List<ImageInfoInternal>> GetList(string name, int fetch, int offset)
        {
            var request = new GetImageListRequestInternal(name, fetch, offset);
            var response = await _bus.RequestAsync<GetImageListRequestInternal, List<ImageInfoInternal>>(request);
            return response;
        }

        public async Task<FileContentResponse> GetContent(long id)
        {
            var request = new GetImageRequestInternal(id);
            var response = await _bus.RequestAsync<GetImageRequestInternal, ImageInfoInternal>(request);
            if (response.Id == 0)
                return null;

            var bytes = await _storage.Get(response.FilePath);
            return new FileContentResponse
            {
                Content = bytes,
                ContentType = response.ContentType
            };
        }

        public async Task<ImageInfoInternal> GetInfo(long id)
        {
            var request = new GetImageRequestInternal(id);
            var response = await _bus.RequestAsync<GetImageRequestInternal, ImageInfoInternal>(request);
            return response.Id == 0 ? null : response;
        }

        public async Task<UpdateDeleteActionResponse> Delete(long id)
        {
            var request = new DeleteImageRequestInternal(id);
            var response = await _bus.RequestAsync<DeleteImageRequestInternal, UpdateDeleteResponseInternal>(request);
            return new UpdateDeleteActionResponse(response);
        }

        public async Task<ImageInfoInternal> Upload(IFormFile file)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            int height, width;
            ExifValue xResolution = null, yResolution = null, dateTime = null;
            bool exifProfileExists = false;
            using (var image = new MagickImage(bytes))
            {
                var exifProfile = image.GetExifProfile();
                if (exifProfile != null)
                {
                    exifProfileExists = true;
                    xResolution = exifProfile.GetValue(ExifTag.XResolution);
                    yResolution = exifProfile.GetValue(ExifTag.YResolution);
                    dateTime = exifProfile.GetValue(ExifTag.DateTime);
                }

                height = image.Height;
                width = image.Width;
            }

            var fileHash = _hashGenerator.Generate(bytes).ToHashString();
            var path = await _storage.Save(bytes, fileHash);

            var request = new UploadFileRequestInternal(file.FileName, path, file.ContentType, height, width);
            if (exifProfileExists)
            {
                request.XResolution = xResolution != null ? xResolution.Value.ToString() : null;
                request.YResolution = yResolution != null ? yResolution.Value.ToString() : null;
                request.DateTime = dateTime != null ? dateTime.Value.ToString() : null;
            }
            var response = await _bus.RequestAsync<UploadFileRequestInternal, ImageInfoInternal>(request);
            return response;
        }

        public async Task<UpdateDeleteActionResponse> UpdateImageInfo(long id, UpdateImageRequest request)
        {
            var internalRequest = new UpdateImageInfoRequestInternal(id, request.Name);
            var response = await _bus.RequestAsync<UpdateImageInfoRequestInternal, UpdateDeleteResponseInternal>(internalRequest);
            return new UpdateDeleteActionResponse(response);
        }
    }
}