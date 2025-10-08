using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen;
using System.Runtime.CompilerServices;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/authen/")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly OtpHandler _otpHandler;

        public ForgotPasswordController(OtpHandler otpHandler)
        {
            _otpHandler = otpHandler;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            var result = await _otpHandler.SendOTPForForgotPasswordHandleAsync(forgotPasswordRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var result = await _otpHandler.ConfirmOTPForForgotPasswordHandleAsync(changePasswordRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
