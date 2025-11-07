using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/Role-Permission/")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly RolePermissionHandler _rolePermissionHandler;

        public RolePermissionController(RolePermissionHandler rolePermissionHandler)
        {
            _rolePermissionHandler = rolePermissionHandler;
        }

        [Authorize]
        [HttpGet("list-role-permission")]
        public async Task<IActionResult> ListRolePermission()
        {
            var result = await _rolePermissionHandler.ListRolePermissionHandleAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("add-role-permission")]
        public async Task<IActionResult> AddRolePermission(RolePermissionRequest rolePermissionRequest)
        {
            var result = await _rolePermissionHandler.AddRolePermissionHandleAsync(rolePermissionRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-role-permission")]
        public async Task<IActionResult> DeleteRolePermission(RolePermissionRequest rolePermissionRequest)
        {
            var result = await _rolePermissionHandler.RemoveRolePermissionHandleAsync(rolePermissionRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
