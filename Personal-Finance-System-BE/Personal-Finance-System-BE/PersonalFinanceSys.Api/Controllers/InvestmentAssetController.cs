using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/investment-asset/")]
    [ApiController]
    public class InvestmentAssetController : ControllerBase
    {
        private readonly InvestmentAssetHandler _investmentAssetHandler;

        public InvestmentAssetController(InvestmentAssetHandler investmentAssetHandler)
        {
            _investmentAssetHandler = investmentAssetHandler;
        }

        [Authorize]
        [HttpGet("list-investment-asset")]
        public async Task<IActionResult> GetListInvestmentAssetByFund([FromQuery] Guid idFund)
        {
            var result = await _investmentAssetHandler.GetInfInvestmentFundHandleAsync(idFund);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-investment-asset-by-user")]
        public async Task<IActionResult> GetListInvestmentAssetByUser([FromQuery] Guid idUser)
        {
            var result = await _investmentAssetHandler.ListInvestmentAssetAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("create-investment-asset")]
        public async Task<IActionResult> AddInvestmentAsset([FromBody]InvestmentAssetRequest investmentAssetRequest)
        {
            var result = await _investmentAssetHandler.CreateInvestmentAssetHandleAsync(investmentAssetRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-investment-asset")]
        public async Task<IActionResult> DeleteInvestmentAsset([FromQuery] Guid idAsset)
        {
            var result = await _investmentAssetHandler.DeleteInvestmentAssetAsync(idAsset);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
