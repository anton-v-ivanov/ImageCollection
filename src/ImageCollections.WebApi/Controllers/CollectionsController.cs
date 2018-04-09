using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.WebApi.Managers;
using ImageCollections.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageCollections.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CollectionsController : Controller
    {
        private readonly ICollectionManager _imageCollectionManager;

        public CollectionsController(ICollectionManager imageCollectionManager)
        {
            _imageCollectionManager = imageCollectionManager;
        }

        /// <summary>
        /// Get all collections
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ImageCollection>> Get([FromQuery]string name, [FromQuery]int select, [FromQuery]int omit)
        {
            if (select == 0)
                select = 10;
            var internalCollections = await _imageCollectionManager.GetCollectionsList(name, select, omit);

            var imageCollections = new List<ImageCollection>(internalCollections.Count);
            foreach (var internalCollection in internalCollections)
            {
                var imageCollection = new ImageCollection
                {
                    Id = internalCollection.Id,
                    Name = internalCollection.Name
                };

                foreach (var imageId in internalCollection.Images)
                {
                    var imageUrl = Url.Link("GetImage", new {id = imageId});
                    imageCollection.Images.Add(imageUrl);
                }
                imageCollections.Add(imageCollection);
            }
            return imageCollections;
        }
    }
}
