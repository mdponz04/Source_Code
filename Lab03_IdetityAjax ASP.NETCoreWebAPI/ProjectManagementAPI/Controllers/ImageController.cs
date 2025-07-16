using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepo _photoService;

        public ImageController(IImageRepo photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string fileName)
        {
            var result = await _photoService.UploadImageAsync(file, fileName);

            if (result == null)
            {
                return BadRequest("Upload failed");
            }

            
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromQuery] string url)
        {
            var result = await _photoService.DeleteImageAsync(url);

            if (result)
            {
                return Ok("Delete image success.");
            }

            return BadRequest("Image delete failed.");
        }
    }
}
