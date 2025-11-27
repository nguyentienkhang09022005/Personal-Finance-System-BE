using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Posts;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/post/")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostHandler _postHandler;

        public PostController(PostHandler postHandler)
        {
            _postHandler = postHandler;
        }

        [Authorize]
        [HttpGet("list-post-by-user")]
        public async Task<IActionResult> ListPostByUserAsync([FromQuery] Guid idUser)
        {
            var result = await _postHandler.GetListPostsByUserIdAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-post-approved")]
        public async Task<IActionResult> ListPostApprovedAsync([FromQuery] Guid idUser)
        {
            var result = await _postHandler.GetListPostsApprovedAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-post-not-approved")]
        public async Task<IActionResult> ListPostNotApprovedAsync()
        {
            var result = await _postHandler.GetListPostsNotApprovedAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("create-transaction-post")]
        public async Task<IActionResult> AddTransactionPostAsync([FromBody] TransactionPostCreationRequest transactionPostCreationRequest)
        {
            var result = await _postHandler.CreateTransactionPostAsync(transactionPostCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("create-asset-post")]
        public async Task<IActionResult> AddInvestmentAssetPostAsync([FromBody] InvestmentAssetPostCreationRequest investmentAssetPostCreationRequest)
        {
            var result = await _postHandler.CreateInvestmentAssetPostAsync(investmentAssetPostCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-post")]
        public async Task<IActionResult> DeletePostAsync([FromQuery] Guid idPost)
        {
            var result = await _postHandler.DeletePostAsync(idPost);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-approve-post")]
        public async Task<IActionResult> ApprovePostAsync([FromBody] ApprovePostRequest approvePostRequest ,
                                                          [FromQuery] Guid idPost)
        {
            var result = await _postHandler.ApprovePostAsync(approvePostRequest, idPost);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("update-transaction-post")]
        public async Task<IActionResult> UpdateTransactionPostAsync([FromBody] TransactionPostUpdateRequest transactionPostUpdateRequest,
                                                                    [FromQuery] Guid idPost)
        {
            var result = await _postHandler.UpdateTransactionPostAsync(transactionPostUpdateRequest, idPost);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-asset-post")]
        public async Task<IActionResult> UpdateAssetPostAsync([FromBody] InvestmentAssetPostUpdateRequest investmentAssetPostUpdateRequest,
                                                              [FromQuery] Guid idPost)
        {
            var result = await _postHandler.UpdateInvestmentAssetPostAsync(investmentAssetPostUpdateRequest, idPost);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-post")]
        public async Task<IActionResult> UpdatePostAsync([FromBody] PostUpdateRequest postUpdateRequest,
                                                         [FromQuery] Guid idPost)
        {
            var result = await _postHandler.UpdatePostAsync(postUpdateRequest, idPost);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
