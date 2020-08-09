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
        public Task<Guid> CreateNewImageBucket(int itemId);
        public Task<Guid> GetBucketIdForItem(int imageId);
        public Task AddImagesToBucket(Guid bucketId, IFormFile[] images);
        public Task RemoveImageFromBucket(Guid bucketId, string objectName);
        public Task<List<Item>> GetListOfImagesFromBucket(Guid bucketId);
        public Task<string> GetPresignedGetObjectAsync(Guid bucketId, string objectName);

    }
}
