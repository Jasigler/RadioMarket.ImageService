using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RadioMarket.ImageService.Services
{
    public class ItemService : IItemService
    {
        private readonly IConfiguration config;
        private readonly string itemPath;

        public ItemService(IConfiguration configuration)
        {
            config = configuration;
            this.itemPath = config.GetValue<string>("StoragePath:Item");

        }

        public ReqResult CreateDirectory (int itemId)
        {
            string pathString = Path.Combine(this.itemPath, itemId.ToString());

            if (!Directory.Exists(pathString))
            {
                try
                {
                   Directory.CreateDirectory(pathString);
                    return ReqResult.Ok;

                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception);
                    return ReqResult.IO_Failure;
                }
            }
            else return ReqResult.Directory_Exists; 
        }

        public async Task<ReqResult[]> AddImages (int itemId, IFormFile[] images)
        {
            var processResults = new ReqResult[images.Length];
            string itemFolder = Path.Combine(this.itemPath, itemId.ToString()); 
             
            if (!Directory.Exists(itemFolder))
            {
                this.CreateDirectory(itemId);
            }
            foreach (var image in images)
            {
                string imagePath = Path.Combine(itemFolder, image.FileName);

                if (!File.Exists(imagePath))
                {
                    var fileStream = new FileStream(imagePath, FileMode.Create);
                    await image.CopyToAsync(fileStream);
                    fileStream.Close();
                    processResults.Append(ReqResult.Ok);

                }
                else processResults.Append(ReqResult.ImageExists);
            }

            return processResults;
        }

        public object  GetImageList(int itemId)
        {
            var imagePath = Path.Combine(this.itemPath, itemId.ToString());
            try
            {
                return Directory.GetFiles(imagePath);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception);
                return exception;
            } 
            
        }

        public ReqResult DeleteImages(int itemId, string[] images)
        {
            var itemDirectory = Path.Combine(this.itemPath, itemId.ToString());
            
            try
            {
                foreach (var image in images)
                {
                    var targetImage = Path.Combine(itemDirectory, image);
                    if (!File.Exists(targetImage))
                    {
                        throw new IOException("File Does Not Exist");
                    }
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception);
                return ReqResult.IO_Failure;
            }

            return ReqResult.Ok;
        }

    }
}
