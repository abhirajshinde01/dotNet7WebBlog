using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file) 
        {
            //call repository
            var imageURL = await imageRepository.UploadAsync(file);

            if (imageURL is null) {
                return Problem("Something went wrong",null,(int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { link = imageURL});
        }
    }
}
