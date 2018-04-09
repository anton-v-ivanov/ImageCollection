using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCollections.WebApi.Managers;
using ImageCollections.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageCollections.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CollectionController : Controller
    {
        private readonly ICollectionManager _imageCollectionManager;

        public CollectionController(ICollectionManager imageCollectionManager)
        {
            _imageCollectionManager = imageCollectionManager;
        }

        /// <summary>
        /// Get collection by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name="GetCollection")]
        public async Task<IActionResult> Get(long id)
        {
            var collection = await _imageCollectionManager.Get(id);
            if (collection == null)
                return NotFound();

            return Ok(collection);
        }

        /// <summary>
        /// Create collection with specified name
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateCollectionRequest request)
        {
            var response = await _imageCollectionManager.Create(request);
            return CreatedAtRoute("GetCollection", new { id = response.Id }, response);
        }

        /// <summary>
        /// Update collection
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]string name)
        {
            await _imageCollectionManager.Update(id, name);
            return Ok();
        }

        /// <summary>
        /// Delete collection
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _imageCollectionManager.Delete(id);
            return Ok();
        }

        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImagesInCollection(long id)
        {
            var collection = await _imageCollectionManager.Get(id);
            if (collection == null)
                return NotFound();

            var result = new List<string>();
            foreach (var imageId in collection.Images)
            {
                var imageUrl = Url.Action("Get", "Image", imageId);
                result.Add(imageUrl);
            }
            return Ok(result);
        }

        [HttpPost("{collectionId}/images")]
        public async Task<IActionResult> AddImageToCollection(long collectionId, [FromBody]AddImageToCollectionRequest addImageModel)
        {
            var result = await _imageCollectionManager.AddImageToCollection(collectionId, addImageModel);
            return Ok(result);
        }

        [HttpDelete("{collectionId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImageFromCollection(long collectionId, long imageId)
        {
            await _imageCollectionManager.RemoveImage(collectionId, imageId);
            return Ok();
        }
    }
}