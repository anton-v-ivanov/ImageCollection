using System.Net;
using System.Threading.Tasks;
using ImageCollections.WebApi.Configuration;
using ImageCollections.WebApi.Managers;
using ImageCollections.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageCollections.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageManager _imageManager;
        private readonly FileRepositorySetting _fileRepositorySettings;

        public ImageController(IImageManager imageManager, IOptions<FileRepositorySetting> settings)
        {
            _imageManager = imageManager;
            _fileRepositorySettings = settings.Value;
        }

        /// <summary>
        /// Get file content
        /// </summary>
        [HttpGet("{id}", Name = "GetImage")]
        public async Task<IActionResult> Get(long id)
        {
            var fileInfo = await _imageManager.GetContent(id);
            if (fileInfo == null)
                return NotFound();

            return File(fileInfo.Content, fileInfo.ContentType);
        }

        /// <summary>
        /// Get file metadata
        /// </summary>
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetInfo(long id)
        {
            var response = await _imageManager.GetInfo(id);
            if (response == null)
                return NotFound();

            var imageInfo = new ImageInfo
            {
                Id = response.Id,
                ContentType = response.ContentType,
                Url = Url.Link("GetImage", new { id = response.Id }),
                Name = response.Name,
                Height = response.Height,
                Weigth = response.Width
            };
            return Ok(imageInfo);
        }

        /// <summary>
        /// Upload file
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");
            
            //checking max length
            if (file.Length > _fileRepositorySettings.MaxFileLengthMb * 1024 * 1024)
                return StatusCode(413);

            if (string.IsNullOrEmpty(file.ContentType) || !file.ContentType.StartsWith("image"))
            {
                return new UnsupportedMediaTypeResult();
            }

            var response = await _imageManager.Upload(file);
            var imageInfo = new ImageInfo
            {
                Id = response.Id,
                ContentType = response.ContentType,
                Url = Url.Link("GetImage", new { id = response.Id }),
                Name = response.Name,
                Height = response.Height,
                Weigth = response.Width
            };
            return Ok(imageInfo);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateName(UpdateImageRequest request)
        {
            var result = await _imageManager.UpdateImageInfo(request);
            return Ok(result);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _imageManager.Delete(id);
            return Ok();
        }
    }
}
