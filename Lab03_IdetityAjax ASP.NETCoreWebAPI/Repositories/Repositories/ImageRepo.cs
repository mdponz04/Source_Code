using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class ImageRepo : IImageRepo
    {
        private readonly Cloudinary _cloudinary;

        public ImageRepo(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string fileName)
        {
            if (file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                await using var stream = file.OpenReadStream();

                // Remove extension from fileName if present
                fileName = Path.GetFileNameWithoutExtension(fileName)
                    .Replace(" ", "")
                    .ToLower();

                var rawParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "uploads",
                    PublicId = fileName,
                    UseFilename = false,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var result = await _cloudinary.UploadAsync(rawParams);
                return result.SecureUrl.ToString();
            }
            return null;
        }
        public async Task<bool> DeleteImageAsync(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var folder = segments[^2];
            var file = Path.GetFileName(segments[^1]);

            var publicId = $"{folder}/{Path.GetFileName(file)}";

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }

    }

    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }

}