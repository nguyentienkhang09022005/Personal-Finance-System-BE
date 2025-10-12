using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/investment-fund")]
    [ApiController]
    public class InvestmentFundController : ControllerBase
    {
        private readonly InvestmentFundHandler _investmentFundHandler;

        public InvestmentFundController(InvestmentFundHandler investmentFundHandler)
        {
            _investmentFundHandler = investmentFundHandler;
        }

        [HttpPost("create-investment-fund")]
        public async Task<IActionResult> AddInvestmentFund(InvestmentFundRequest investmentFundRequest)
        {
            var result = await _investmentFundHandler.CreateInvestmentHandleAsync(investmentFundRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
