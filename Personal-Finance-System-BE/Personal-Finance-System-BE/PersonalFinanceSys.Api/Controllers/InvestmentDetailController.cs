using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/investment-detail/")]
    [ApiController]
    public class InvestmentDetailController : ControllerBase
    {
        private readonly InvestmentDetailHandler _investmentDetailHandler;

        public InvestmentDetailController(InvestmentDetailHandler investmentDetailHandler)
        {
            _investmentDetailHandler = investmentDetailHandler;
        }

        [HttpGet("list-investment-detail")]
        public async Task<IActionResult> GetListInvestmentDetail([FromQuery] Guid idAsset)
        {
            var result = await _investmentDetailHandler.GetInfInvestmentDetailHandleAsync(idAsset);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("create-investment-detail")]
        public async Task<IActionResult> AddInvestmentDetail([FromBody] InvestmentDetailRequest investmentDetailRequest)
        {
            var result = await _investmentDetailHandler.CreateInvestmentDetailHandleAsync(investmentDetailRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete-investment-detail")]
        public async Task<IActionResult> DeleteInvestmentDetail([FromQuery] Guid idDetail)
        {
            var result = await _investmentDetailHandler.DeleteInvestmentDetailAsync(idDetail);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
