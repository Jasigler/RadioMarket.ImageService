using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;
using RadioMarket.ImageService.Services;

namespace RadioMarket.ImageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IItemService _item;
        public ImageController(IItemService item) 
        {
            this._item = item;
        }
            
        [HttpPost("item/directory/{itemId}")] 
        public IActionResult CreateImageDirectory(int itemId)
        {
            ReqResult result = _item.CreateDirectory(itemId);

            if (result == ReqResult.Ok)
            {
                return Ok();
            }
            else return StatusCode(500);
        }

        [HttpPost("item/{itemId}")]
        public async Task<ActionResult> AddItemImages(int itemId, [FromForm] IFormFile[] image)
        {
            var results = await _item.AddImages(itemId, image);
            if (results.Contains(ReqResult.ImageExists))
            { 
                return BadRequest(results);
            }
            else return Ok();

        }

        [HttpGet("item/{itemId}")]
        public ActionResult GetItemImages(int itemId)
        {
            var result =  _item.GetImageList(itemId);
            return Ok(result);
        }

        [HttpDelete("item/{itemId}")]
        public ActionResult RemoveItemImage(int itemId,  [FromBody] string[] images)
        {
            var result = _item.DeleteImages(itemId, images);
            if (result != ReqResult.IO_Failure)
            {
                return Ok(result);
            }
            else return StatusCode(400);
        }
    }
}
