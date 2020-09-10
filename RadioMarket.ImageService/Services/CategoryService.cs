using Microsoft.AspNetCore.Http;
using Models.Interfaces;
using System;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Models;

namespace RadioMarket.ImageService.Services
{
    public class CategoryService : ICategoryService
    {
        private static MinioClient _categoryClient = new MinioClient(
            "Localhost:9001",
            "AKIAIOSFODNN7EXAMPLE",
            "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY");

        public async Task<ReqResult> AddImage(int categoryId, IFormFile file)
        {
            var exists = await _categoryClient.BucketExistsAsync(categoryId.ToString());

            if (!exists)
            {
                return ReqResult.NotFound;
            }

            var existingImages = new List<Item>();

            IObservable<Item> itemObservable = _categoryClient.ListObjectsAsync(categoryId.ToString(), null, true);
            IDisposable subscriber = itemObservable.ToList().Subscribe(
                x => existingImages.AddRange(x),
                ex => Console.WriteLine(ex.Message));
            subscriber.Dispose();

            if (!existingImages.Any())
            {
                await _categoryClient.PutObjectAsync(categoryId.ToString(),
                    file.FileName, file.OpenReadStream(), file.Length);

                return ReqResult.Ok;
            }
            else return ReqResult.ImageExists;
        }

        public async Task<ReqResult> CreateBucket(int categoryId)
        {
            var exists = await _categoryClient.BucketExistsAsync(categoryId.ToString());
            if (!exists)
            {
                await _categoryClient.MakeBucketAsync(categoryId.ToString());
                return ReqResult.Ok;
            }
            else return ReqResult.BucketExists;
        }

        public ReqResult DeleteImage(int categoryId)
        {
            throw new NotImplementedException();
        }


        public Task<string> GetImageLink(int categoryId)
        {

            var categoryItem = new Item();

            IObservable<Item> itemObservable = _categoryClient.ListObjectsAsync(categoryId.ToString(), null, true); ;
            IDisposable subscriber = itemObservable.ToList().Subscribe(
               x => categoryItem = x.First(),
               ex => Console.WriteLine("OnError: {0}", ex.Message),
               () => Console.WriteLine("Done")); 
            itemObservable.Wait();
            subscriber.Dispose();


           
            var imageLink = _categoryClient.PresignedGetObjectAsync(categoryId.ToString(), categoryItem.Key, 60 * 60 * 24); 
            return imageLink;
        }
    }
}