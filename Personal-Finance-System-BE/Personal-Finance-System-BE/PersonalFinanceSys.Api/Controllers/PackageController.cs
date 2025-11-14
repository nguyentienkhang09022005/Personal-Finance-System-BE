using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Packages;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/package/")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly PackageHandler _packageHandler;

        public PackageController(PackageHandler packageHandler)
        {
            _packageHandler = packageHandler;
        }

        [HttpPost("create-package")]
        public async Task<IActionResult> AddPackage([FromBody] PackageCreationRequest packageCreationRequest)
        {
            var result = await _packageHandler.CreatePackageAsync(packageCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("list-package")]
        public async Task<IActionResult> GetListPackage([FromQuery] Guid idUser)
        {
            var result = await _packageHandler.GetListPackageAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("udpate-package")]
        public async Task<IActionResult> UpdatePackageAsync([FromQuery] Guid idPackage,
                                                            [FromBody] PackageUpdateRequest packageUpdateRequest)
        {
            var result = await _packageHandler.UpdatePackageAsync(packageUpdateRequest, idPackage);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-package")]
        public async Task<IActionResult> DeletePackageAsync([FromQuery] Guid idPackage)
        {
            var result = await _packageHandler.DeletePackageAsync(idPackage);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
