using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/cloud-image/")]
    [ApiController]
    public class CloudinaryController : ControllerBase
    {
        private readonly IUpLoadImageFileService _upLoadImageFileService;

        public CloudinaryController(IUpLoadImageFileService upLoadImageFileService)
        {
            _upLoadImageFileService = upLoadImageFileService;
        }

        [Authorize]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng upload hình ảnh!");
            }
            var result = await _upLoadImageFileService.UploadImageAsync(file);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
