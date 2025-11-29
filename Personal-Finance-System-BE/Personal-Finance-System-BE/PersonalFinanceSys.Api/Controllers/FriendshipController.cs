using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/friendship/")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly FriendshipHandler _friendshipHandler;

        public FriendshipController(FriendshipHandler friendshipHandler)
        {
            _friendshipHandler = friendshipHandler;
        }

        [HttpGet("list-friendship-of-user")]
        public async Task<IActionResult> ListFriendshipOfUserAsync([FromQuery] Guid idUser)
        {
            var result = await _friendshipHandler.GetListGetListFriendshipOfUserAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("list-friendship-sent-of-user")]
        public async Task<IActionResult> ListFriendshipSentOfUserAsync([FromQuery] Guid idUser)
        {
            var result = await _friendshipHandler.GetListFriendshipSentWithStatusPendingByUserAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("list-friendship-received-of-user")]
        public async Task<IActionResult> ListFriendshipReceivedOfUserAsync([FromQuery] Guid idUser)
        {
            var result = await _friendshipHandler.GetListFriendshipReceivedWithStatusPendingByUserAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("create-friendship")]
        public async Task<IActionResult> AddFriendshipAsync([FromBody] FriendshipCreationRequest friendshipCreationRequest)
        {
            var result = await _friendshipHandler.CreateFriendshipAsync(friendshipCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("accept-friendship")]
        public async Task<IActionResult> AcceptFriendshipAsync([FromBody] FriendshipUpdateRequest friendshipUpdateRequest,
                                                               [FromQuery] Guid idFriendship)
        {
            var result = await _friendshipHandler.AcceptFriendshipAsync(idFriendship, friendshipUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("reject-friendship")]
        public async Task<IActionResult> RejectFriendshipAsync([FromBody] FriendshipUpdateRequest friendshipUpdateRequest,
                                                               [FromQuery] Guid idFriendship)
        {
            var result = await _friendshipHandler.RejectFriendshipAsync(idFriendship, friendshipUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete-friendship")]
        public async Task<IActionResult> DeleteFriendshipAsync([FromQuery] Guid idFriendship)
        {
            var result = await _friendshipHandler.DeleteFriendshipAsync(idFriendship);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
