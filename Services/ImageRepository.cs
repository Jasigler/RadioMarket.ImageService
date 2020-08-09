using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ImageRepository : IImageRepository, IDisposable
    {
        private readonly ImageContext _context;
        private readonly MinioClient _client = new MinioClient("127.0.0.1:9000",
            "AKIAIOSFODNN7EXAMPLE",
            "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY");
        
        public ImageRepository(ImageContext context, IHostingEnvironment environment)
        {
            _context = context;
        }

       public async Task<Guid> CreateNewImageBucket(int itemId)
        {
            var newBucketId = Guid.NewGuid();

            await _client.MakeBucketAsync(newBucketId.ToString());

            var newRef = new ImageBucket()
            {
                bucket_id = newBucketId,
                item_id = itemId
            };

            await _context.ImageBuckets.AddAsync(newRef);
            await _context.SaveChangesAsync();

            return newBucketId;

        }

        public async Task<Guid> GetBucketIdForItem(int imageId)
        {
            var item = await _context.ImageBuckets
                .Where(item => item.item_id == imageId)
                .FirstOrDefaultAsync();

            return item.bucket_id;
        }

        public async Task AddImagesToBucket(Guid bucketId, IFormFile[] images)
        {
            foreach (var image in images)
            {
                await _client.PutObjectAsync(bucketId.ToString(), image.FileName, image.OpenReadStream(), image.Length);
            }
        }

        public async Task RemoveImageFromBucket(Guid bucketId, string objectName)
        {
           await _client.RemoveObjectAsync(bucketId.ToString(), objectName);
        }
      

        public async Task<List<Item>> GetListOfImagesFromBucket(Guid bucketId)
        {
            var target = await _context.ImageBuckets
                   .Where(reference => reference.bucket_id == bucketId)
                   .FirstOrDefaultAsync();

            var imageList = new List<Item>();

            var convertedTargetName = target.ToString();
            convertedTargetName.ToLower();

            IObservable<Item> itemObservable = _client.ListObjectsAsync(target.bucket_id.ToString(), null, true); ;
            IDisposable subscriber = itemObservable.ToList().Subscribe(
               x => imageList.AddRange(x),
               ex => Console.WriteLine("OnError: {0}", ex.Message),
               () => Console.WriteLine("Done"));
            itemObservable.Wait();
            subscriber.Dispose();

            return imageList;
        }

        public async Task<string> GetPresignedUrlForBucketObject(Guid bucketId, string objectName)
        {
            var objectUrl = await _client.PresignedGetObjectAsync(bucketId.ToString(), objectName, 60*60*24);
            return objectUrl;

        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

    }
}
