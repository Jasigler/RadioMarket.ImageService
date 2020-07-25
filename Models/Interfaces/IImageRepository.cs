using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Internal;

namespace Models.Interfaces
{
    public interface IImageRepository
    {
        public Task<Guid> PushImagesToNewBucket(int id, IFormFile[] images);
        public Task AddImagesToItemBucket(int id, IFormFile[] images);
       
        public Task<List<Item>> GetImagesByItem(int id);
    }
}
