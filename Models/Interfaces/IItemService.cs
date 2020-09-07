using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IItemService

    {
        public Task<ReqResult> CreateBucket(Guid itemId);

        public Task<ReqResult> AddImage(Guid itemId, IFormFile[] images);

        public Task<List<string>> GetImageList(Guid itemId);

        public ReqResult DeleteImage(Guid itemId, string[] images);
    }
}