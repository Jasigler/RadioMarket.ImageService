using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;
using Models.Interfaces;


namespace RadioMarket.ImageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _repository;
        public ImageController(IImageRepository repository)
        {
            _repository = repository;
        }
       
        
        [HttpPost("bucket/{imageId}")]
        public async Task<ActionResult<Guid>> CreateNewBucket(int imageId)
        {
            var newBucketId = await _repository.CreateNewImageBucket(imageId);
            return Ok(newBucketId);
        }

        [HttpPost("{bucketId}")]
        public async Task<ActionResult> AddImages(Guid bucketId, [FromForm] IFormFile[] image)
        {
            await _repository.AddImagesToBucket(bucketId, image);
            return StatusCode(201);
        }

        [HttpGet("bucket/{imageId}")]
        public async Task<ActionResult<Guid>> GetBucketIdForImage(int imageId)
        {
            var bucketId = await _repository.GetBucketIdForItem(imageId);
            return Ok(bucketId);
        }

        [HttpGet("{bucketId}")]
        public async Task<ActionResult<Item>> GetObjectListFromBucket(Guid bucketId)
        {
            var bucketObjects = await _repository.GetListOfImagesFromBucket(bucketId);
            return Ok(bucketObjects);
        }

        [HttpGet("url/{bucketId}}")]
        public async Task<ActionResult<string>> GetPresignedUrl (Guid bucketId, [FromBody] string objectName)
        {
            var objectUrl = _repository.GetPresignedGetObjectAsync(bucketId, objectName);
            return Ok(objectUrl);
        }

        [HttpDelete("{bucketId}")]
        public async Task<ActionResult> RemoveImageFromBucket(Guid bucketId, [FromBody] string objectName)
        {
            await _repository.RemoveImageFromBucket(bucketId, objectName);
            return Ok();
        }



    }
}
