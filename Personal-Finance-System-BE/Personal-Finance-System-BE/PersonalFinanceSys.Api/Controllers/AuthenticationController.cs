using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/authen/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenHandler _authenHandler;

        public AuthenticationController(AuthenHandler authenHandler)
        {
            _authenHandler = authenHandler;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest authenticationRequest)
        {
            var result = await _authenHandler.LoginHandleAsync(authenticationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenHandler.LogoutHandleAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("introspect")]
        public async Task<IActionResult> Introspect([FromBody] IntrospectRequest introspectRequest)
        {
            var result = await _authenHandler.IntrospectTokenHandleAsync(introspectRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authenHandler.RefreshTokenHandleAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
