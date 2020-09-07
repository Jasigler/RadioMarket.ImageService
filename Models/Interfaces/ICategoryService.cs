using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface ICategoryService
    {
        public Task<ReqResult> CreateBucket(int categoryId);
        public Task<ReqResult> AddImage(int categoryId, IFormFile file);
        public Task<string> GetImageLink(int categoryId);
        public ReqResult DeleteImage(int categoryId);

    }
}
