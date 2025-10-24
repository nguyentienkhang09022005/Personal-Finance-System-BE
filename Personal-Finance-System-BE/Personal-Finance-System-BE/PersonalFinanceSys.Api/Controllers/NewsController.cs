using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsHandler _newsHandler;

        public NewsController(NewsHandler newsHandler)
        {
            _newsHandler = newsHandler;
        }

        [Authorize]
        [HttpGet("list-news-trending")]
        public async Task<IActionResult> GetListNewsTrending()
        {
            var result = await _newsHandler.GetTrendingNewsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-special-reports")]
        public async Task<IActionResult> GetSpecialReports()
        {
            var result = await _newsHandler.GetSpecialReportsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-news")]
        public async Task<IActionResult> GetNews()
        {
            var result = await _newsHandler.GetNewsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
