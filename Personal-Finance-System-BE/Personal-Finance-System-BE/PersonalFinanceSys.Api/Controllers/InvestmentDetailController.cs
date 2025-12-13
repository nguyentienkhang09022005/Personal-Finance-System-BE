using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Transactions;

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

        [Authorize]
        [HttpGet("list-investment-detail")]
        public async Task<IActionResult> GetListInvestmentDetail([FromQuery] Guid idAsset)
        {
            var result = await _investmentDetailHandler.GetInfInvestmentAssetHandleAsync(idAsset);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
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

        [HttpPost("compare-investment-detail-by-month")]
        public async Task<IActionResult> CompareInvestmentDetailByMonthAsync([FromBody] CompareInvestmentDetailByMonthRequest compareInvestmentDetailByMonthRequest)
        {
            var result = await _investmentDetailHandler.CompareInvestmentDetailByMonthAsync(compareInvestmentDetailByMonthRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("compare-investment-detail-by-year")]
        public async Task<IActionResult> CompareInvestmentDetailByYearAsync([FromBody] CompareInvestmentDetailByYearRequest compareInvestmentDetailByYearRequest)
        {
            var result = await _investmentDetailHandler.CompareInvestmentDetailByYearAsync(compareInvestmentDetailByYearRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
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
