using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace RadioMarket.ImageService.Controllers
{
    [Route("api/image/item")]
    [ApiController]
    public class ItemImageController : ControllerBase
    {
        private readonly IItemService _item;

        public ItemImageController(IItemService item)
        {
            this._item = item;
        }

        [HttpPost("{itemId}")]
        public async Task<ActionResult> CreateItemBucket([FromRoute] Guid itemId)
        {
            var result = await _item.CreateBucket(itemId);

            if (result == ReqResult.Ok)
            {
                return Ok();
            }
            else return BadRequest("Bucket Already Exists.");
        }

        [HttpPost("add/{itemId}")]
        public async Task<ActionResult> AddItemImages([FromRoute] Guid itemId, [FromForm] IFormFile[] image)
        {
            var result = await _item.AddImage(itemId, image);

            if (result == ReqResult.Ok)
            {
                return Ok();
            }
            else return NotFound();
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult> GetImageLinks([FromRoute] Guid itemId)
        {
            var result = await _item.GetImageList(itemId);
            return Ok(result);
        }
    }
}