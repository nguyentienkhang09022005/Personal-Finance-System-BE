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

        [HttpGet("price-gold-sjc")]
        public async Task<IActionResult> GetPriceGoldSjc()
        {
            var result = await _goldHandler.GetSJCGoldPricesAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-gold-doji")]
        public async Task<IActionResult> GetPriceGoldDoji()
        {
            var result = await _goldHandler.GetDOJIGoldPricesAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-gold-pnj")]
        public async Task<IActionResult> GetPriceGoldPnj()
        {
            var result = await _goldHandler.GetPNJGoldPricesAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
