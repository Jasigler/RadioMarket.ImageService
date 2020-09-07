using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;
using Models;
using Models.Interfaces;

namespace RadioMarket.ImageService.Controllers
{
    [Route("api/image/category")]
    [ApiController]
    public class CategoryImageController : ControllerBase
    {
        private readonly ICategoryService _category;
        public CategoryImageController(ICategoryService category)
        {
            this._category = category;
        }


        [HttpPost("bucket/{categoryId}")]
        public async Task<ActionResult> CreateBucket([FromRoute] int categoryId)
        {

            var result = await _category.CreateBucket(categoryId);

            if (result == ReqResult.Ok)
            {
                return Ok("Bucket created");
            }
            else return NotFound("Category bucket already exists");
        }


        [HttpPost("{categoryId}")]
        public async Task<ActionResult> AddImage([FromRoute] int categoryId, [FromForm] IFormFile image )
        {
            var result = await _category.AddImage(categoryId, image);

            if (result == ReqResult.Ok)
            {
                return Ok("Image added");
            }
            else return NotFound("Category does not exist");
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetImage([FromRoute] int categoryId)
        {
            var link = await _category.GetImageLink(categoryId);
            return Ok(link);
        }

    }
}
