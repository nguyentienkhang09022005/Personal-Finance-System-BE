using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/favorite/")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteHandler _favoriteHandler;

        public FavoriteController(FavoriteHandler favoriteHandler)
        {
            _favoriteHandler = favoriteHandler;
        }

        [Authorize]
        [HttpPost("create-favorite")]
        public async Task<IActionResult> AddFavoriteAsync([FromBody] FavoriteRequest favoriteRequest)
        {
            var result = await _favoriteHandler.CreateFavoriteAsync(favoriteRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-favorite")]
        public async Task<IActionResult> DeleteFavoriteAsync([FromForm] FavoriteRequest favoriteRequest)
        {
            var result = await _favoriteHandler.DeleteFavoriteAsync(favoriteRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
