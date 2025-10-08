using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly OtpHandler _otpHandler;
        private readonly RegisterHandler _registerHandler;

        public RegisterController(OtpHandler otpHandler, RegisterHandler registerHandler)
        {
            _otpHandler = otpHandler;
            _registerHandler = registerHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await _registerHandler.SendOtpToRegisterAsync(registerRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("confirm-otp-register")]
        public async Task<IActionResult> ConfirmOtpRegister([FromBody] ConfirmOTPRequest confirmOTPRequest)
        {
            var result = await _otpHandler.ConfirmOTPForRegisterHandleAsync(confirmOTPRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
