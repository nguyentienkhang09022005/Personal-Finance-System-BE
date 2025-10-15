using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/crypto/")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly CryptoHandler _cryptoHandler;

        public CryptoController(CryptoHandler cryptoHandler)
        {
            _cryptoHandler = cryptoHandler;
        }

        [HttpGet("list-crypto")]
        public async Task<IActionResult> GetListCryto()
        {
            var result = await _cryptoHandler.GetListCryptoAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("inf-crypto")]
        public async Task<IActionResult> GetInfCryto([FromQuery] string idCrypto)
        {
            var result = await _cryptoHandler.GetCurrentPriceCryptoAsync(idCrypto);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
