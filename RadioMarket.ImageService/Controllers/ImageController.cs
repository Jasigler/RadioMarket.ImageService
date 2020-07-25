using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{item}")]
        public async Task<ActionResult> GetImageListForItem(int item)
        {
            var result = await _repository.GetImagesByItem(item);
            return Ok(result);
        }

        [HttpPost("{item}")]
        public async Task<ActionResult<Guid>> CreateBucketAndStoreNewItemImages(int item, [FromForm] IFormFile[] image)
        {
            if (image.Length > 0)
            {
                var createdBucketId = await _repository.PushImagesToNewBucket(item, image);
                return Ok(createdBucketId);
            }
            else return BadRequest("Images Expected");
        }

        [HttpPost("/add/{item}")]
        public async Task<ActionResult> AddImagesToBucket(int item, [FromForm] IFormFile[] images)
        {
            if (images.Length > 0)
            {
                await _repository.AddImagesToItemBucket(item, images);
                return StatusCode(201);
            }
            else return BadRequest("Images expected");
        }
    }
}
