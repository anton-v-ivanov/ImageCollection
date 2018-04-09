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
            var internalCollection = await _imageCollectionManager.GetCollection(id);
            if (internalCollection == null)
                return NotFound();

            var imageCollection = new ImageCollection
            {
                Id = internalCollection.Id,
                Name = internalCollection.Name
            };

            if (internalCollection.Images != null)
            {
                foreach (var imageId in internalCollection.Images)
                {
                    var imageUrl = Url.Link("GetImage", new {id = imageId});
                    imageCollection.Images.Add(imageUrl);
                }
            }

            return Ok(imageCollection);
        }

        /// <summary>
        /// Create collection with specified name
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateCollectionRequest request)
        {
            var response = await _imageCollectionManager.CreateCollection(request);
            return CreatedAtRoute("GetCollection", new { id = response.Id }, response);
        }

        /// <summary>
        /// Update collection
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long id, [FromBody]UpdateCollectionRequest request)
        {
            var response = await _imageCollectionManager.UpdateCollection(id, request);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Delete collection
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _imageCollectionManager.DeleteCollection(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImagesInCollection(long id)
        {
            var collection = await _imageCollectionManager.GetCollection(id);
            if (collection == null)
                return NotFound();

            var result = new List<string>();
            foreach (var imageId in collection.Images)
            {
                var imageUrl = Url.Link("GetImage", new { id = imageId });
                result.Add(imageUrl);
            }
            return Ok(result);
        }

        [HttpPost("{collectionId}/images")]
        public async Task<IActionResult> AddImageToCollection(long collectionId, [FromBody]AddImageToCollectionRequest addImageModel)
        {
            var response = await _imageCollectionManager.AddImageToCollection(collectionId, addImageModel);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{collectionId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImageFromCollection(long collectionId, long imageId)
        {
            var response = await _imageCollectionManager.RemoveImageFromCollection(collectionId, imageId);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}