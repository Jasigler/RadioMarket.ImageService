using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Models.Interfaces
{
   public interface IItemService
    {
        public ReqResult CreateDirectory(int itemId);
        public Task<ReqResult[]> AddImages(int itemId, IFormFile[] images);
        public object GetImageList(int itemId);
        public ReqResult DeleteImages(int itemId, string[] images);
    }
}
