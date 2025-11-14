using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Users;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserHandler _userHandler;

        public UserController(UserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [Authorize]
        [HttpGet("list-user")]
        public async Task<IActionResult> ListUser()
        {
            var result = await _userHandler.GetListUserHandleAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("inf-user")]
        public async Task<IActionResult> InfUser([FromQuery] Guid idUser)
        {
            var result = await _userHandler.GetInfUserHandleAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserCreationRequest userCreationRequest)
        {
            var result = await _userHandler.CreateUserHandleAsync(userCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid idUser)
        {
            var result = await _userHandler.deleteUserHandleAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-user")]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid idUser, 
                                                    [FromBody] UserUpdateRequest userUpdateRequest)
        {
            var result = await _userHandler.UpdateUserHandleAsync(idUser, userUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
