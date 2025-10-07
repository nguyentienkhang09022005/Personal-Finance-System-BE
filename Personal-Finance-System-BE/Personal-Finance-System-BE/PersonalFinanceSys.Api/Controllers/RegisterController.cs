using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterHandler _registerHandler;

        public RegisterController(RegisterHandler registerHandler)
        {
            _registerHandler = registerHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await _registerHandler.HandleAsync(registerRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
