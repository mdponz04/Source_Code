using Microsoft.AspNetCore.Http;

namespace Repositories.Interfaces
{
    public interface IImageRepo
    {
        Task<string> UploadImageAsync(IFormFile file, string fileName);
        Task<bool> DeleteImageAsync(string url);
    }
}
