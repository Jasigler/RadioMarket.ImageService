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
        public Task<Guid> CreteNewImageBucket(int imageId);
        public Task<Guid> GetBucketIdForItem(int imageId);
        public Task<IFormFileCollection> GetImagesFromBucket(Guid bucketId);
        public Task AddImagesToBucket(int id, IFormFile[] images);
        public Task RemoveImageFromBucket(Guid bucketId, string imageId);
        public Task<List<Item>> GetListOfImagesFromBucket(int id);
        public Task RemoveImageFromBucket(int itemId, int imageId);

    }
}
