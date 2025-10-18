using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/investment-fund/")]
    [ApiController]
    public class InvestmentFundController : ControllerBase
    {
        private readonly InvestmentFundHandler _investmentFundHandler;

        public InvestmentFundController(InvestmentFundHandler investmentFundHandler)
        {
            _investmentFundHandler = investmentFundHandler;
        }

        [Authorize]
        [HttpPost("create-investment-fund")]
        public async Task<IActionResult> AddInvestmentFund(InvestmentFundCreationRequest investmentFundCreationRequest)
        {
            var result = await _investmentFundHandler.CreateInvestmentHandleAsync(investmentFundCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-investment-fund")]
        public async Task<IActionResult> GetListInvestmentFundByIdUser([FromQuery] Guid idUser)
        {
            var result = await _investmentFundHandler.GetListInvestmentFundHandleAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("inf-investment-fund")]
        public async Task<IActionResult> GetInfInvestmentFundByIdUser([FromQuery] Guid idFund)
        {
            var result = await _investmentFundHandler.GetInfInvestmentFundHandleAsync(idFund);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPut("udpate-investment-fund")]
        public async Task<IActionResult> UpdateInfInvestmentFundAsync([FromQuery] Guid idFund,
                                                                      [FromBody] InvestmentFundUpdateRequest investmentFundUpdateRequest)
        {
            var result = await _investmentFundHandler.UpdateInvestmentFundAsync(idFund, investmentFundUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-investment-fund")]
        public async Task<IActionResult> DeleteInvestmentFundAsync([FromQuery] Guid idFund)
        {
            var result = await _investmentFundHandler.DeleteInvestmentHandleAsync(idFund);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
