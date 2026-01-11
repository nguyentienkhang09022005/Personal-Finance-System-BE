using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/gold/")]
    [ApiController]
    public class GoldController : ControllerBase
    {
        private readonly GoldHandler _goldHandler;

        public GoldController(GoldHandler goldHandler)
        {
            _goldHandler = goldHandler;
        }

        [Authorize]
        [HttpGet("all-price-gold")]
        public async Task<IActionResult> GetAllPriceGold()
        {
            var result = await _goldHandler.GetAllGoldPricesAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
