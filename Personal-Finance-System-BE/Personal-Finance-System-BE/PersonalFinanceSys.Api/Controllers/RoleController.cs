using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/role/")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleHandler _roleHandler;

        public RoleController(RoleHandler roleHandler) 
        {
            _roleHandler = roleHandler;
        }

        [Authorize]
        [HttpGet("list-role")]
        public async Task<IActionResult> ListRoles()
        {
            var result = await _roleHandler.GetAllRolesAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
