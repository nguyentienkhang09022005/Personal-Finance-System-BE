using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/permission/")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionHandler _permissionHandler;

        public PermissionController(PermissionHandler permissionHandler)
        {
            _permissionHandler = permissionHandler;
        }

        //[Authorize]
        [HttpGet("list-permission")]
        public async Task<IActionResult> ListPermissionsAsync()
        {
            var result = await _permissionHandler.GetAllPermissionsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
