using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel;
using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RadioMarket.ImageService.Services
{
    public class ItemService : IItemService

    {
        private static MinioClient _itemClient = new MinioClient(
            "Localhost:9000",
            "AKIAIOSFODNN7EXAMPLE",
            "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY");
        public ItemService()
        {
        }

        public async Task<ReqResult> AddImage(Guid itemId, IFormFile[] images)
        {
            var exists = await _itemClient.BucketExistsAsync(itemId.ToString());

            if (!exists)
            {
                return ReqResult.NotFound;
            }

            foreach (var image in images)
            {
                await _itemClient.PutObjectAsync(itemId.ToString(), image.FileName, image.OpenReadStream(), image.Length);
            }
            return ReqResult.Ok;
        }

        public async Task<ReqResult> CreateBucket(Guid itemId)
        {
            var exists = await _itemClient.BucketExistsAsync(itemId.ToString());
            if (exists)
            {
                return ReqResult.BucketExists;
            }

            await _itemClient.MakeBucketAsync(itemId.ToString());
            return ReqResult.Ok;
        }

        public ReqResult DeleteImage(Guid itemId, string[] images)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetImageList(Guid itemId)
        {
            var imageList = new List<Item>();
            var imageLinks = new List<string>();

            IObservable<Item> itemObservable = _itemClient.ListObjectsAsync(itemId.ToString(), null, true); 
            IDisposable subscriber = itemObservable.ToList().Subscribe(
               x => imageList.AddRange(x),
               ex => Console.WriteLine("OnError: {0}", ex.Message),
               () => Console.WriteLine("Done"));
            itemObservable.Wait();
            subscriber.Dispose();

            foreach (var image in imageList)
            {
                var link = await _itemClient.PresignedGetObjectAsync(itemId.ToString(), image.Key.ToString(), 60 * 60 * 24);
                imageLinks.Add(link);
            }

            return imageLinks;
        }
    }
}