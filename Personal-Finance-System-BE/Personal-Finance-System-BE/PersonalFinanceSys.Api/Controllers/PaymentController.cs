using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Payments;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/payment/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentHandler _paymentHandler;

        public PaymentController(PaymentHandler paymentHandler)
        {
            _paymentHandler = paymentHandler;
        }

        [Authorize]
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody]PaymentRequest paymentRequest)
        {
            var result = await _paymentHandler.CreatePaymentIntentAsync(paymentRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("callback")]
        public async Task<IActionResult> CallBack(ZaloPayCallBackRequest zaloPayCallBackRequest)
        {
            var result = await _paymentHandler.ResolveZaloPayCallbackAsync(zaloPayCallBackRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("cancel-package-for-payment")]
        public async Task<IActionResult> CancelPackageForPayment([FromBody] PaymentCancelRequest paymentCancelRequest)
        {
            var result = await _paymentHandler.CancelPackageAsync(paymentCancelRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
