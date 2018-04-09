using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.WebApi.Managers;
using ImageCollections.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageCollections.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageManager _imageManager;

        public ImagesController(IImageManager imageManager)
        {
            _imageManager = imageManager;
        }
        
        /// <summary>
        /// Get list of images with metadata
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery]string name, [FromQuery]int fetch, [FromQuery]int offset)
        {
            if (fetch == 0)
                fetch = 10;

            var response = await _imageManager.GetList(name, fetch, offset);
            var result = new List<ImageInfo>();
            foreach (var internalInfo in response)
            {
                var imageInfo = new ImageInfo
                {
                    Id = internalInfo.Id,
                    ContentType = internalInfo.ContentType,
                    Url = Url.Link("GetImage", new { id = internalInfo.Id }),
                    Info = Url.Link("GetInfo", new { id = internalInfo.Id }),
                    Name = internalInfo.Name,
                    Height = internalInfo.Height,
                    Weigth = internalInfo.Width,
                    XResolution = internalInfo.XResolution,
                    YResolution = internalInfo.YResolution,
                    DateTime = internalInfo.DateTime,
                };
                result.Add(imageInfo);
            }
            return Ok(result);
        }
    }
}
