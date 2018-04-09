using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EasyNetQ;
using ImageCollections.Contracts.ImageInfos;
using ImageCollections.WebApi.Configuration;
using ImageCollections.WebApi.Managers.HashGenerator;
using ImageCollections.WebApi.Managers.StorageFactory;
using ImageCollections.WebApi.Models;
using ImageCollections.WebApi.Repositories;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ImageInfo = ImageCollections.Contracts.ImageInfos.ImageInfo;
using UploadFileRequest = ImageCollections.Contracts.ImageInfos.UploadFileRequest;

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

        public async Task<List<ImageInfo>> GetList(string name, int fetch, int offset)
        {
            var request = new GetImageListRequest(name, fetch, offset);
            var response = await _bus.RequestAsync<GetImageListRequest, List<ImageInfo>>(request);
            return response;
        }

        public async Task<FileContentResponse> GetContent(long id)
        {
            var request = new GetImageRequest(id);
            var response = await _bus.RequestAsync<GetImageRequest, ImageInfo>(request);
            if (response == null)
                return null;

            var bytes = await _storage.Get(response.FilePath);
            return new FileContentResponse
            {
                Content = bytes,
                ContentType = response.ContentType
            };
        }

        public async Task<ImageInfo> GetInfo(long id)
        {
            var request = new GetImageRequest(id);
            var response = await _bus.RequestAsync<GetImageRequest, ImageInfo>(request);
            return response;
        }

        public async Task<bool> Delete(long id)
        {
            var request = new DeleteImageRequest(id);
            var response = await _bus.RequestAsync<DeleteImageRequest, DeleteImageResponse>(request);
            return response.Success;
        }

        public async Task<ImageInfo> Upload(IFormFile file)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            int height, width;
            using (var image = new MagickImage(bytes))
            {
                var exifProfile = image.GetExifProfile();
                if (exifProfile != null)
                {
                    var xResolution = exifProfile.GetValue(ExifTag.XResolution);
                    var yResolution = exifProfile.GetValue(ExifTag.YResolution);
                    var dateTime = exifProfile.GetValue(ExifTag.DateTime);
                    var gpsLatitude = exifProfile.GetValue(ExifTag.GPSLatitude);
                    var gpsLongitude = exifProfile.GetValue(ExifTag.GPSLongitude);
                }

                height = image.Height;
                width = image.Width;
            }

            var fileHash = _hashGenerator.Generate(bytes).ToHashString();
            var path = await _storage.Save(bytes, fileHash);

            var request = new UploadFileRequest(file.FileName, path, file.ContentType, height, width);
            var response = await _bus.RequestAsync<UploadFileRequest, ImageInfo>(request);
            return response;
        }

        public Task<ImageInfo> UpdateImageInfo(UpdateImageRequest request)
        {
            var updateImageInfoRequest = new UpdateImageInfoRequest(request.Name);
        }
    }
}