using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/message/")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageHandler _messageHandler;

        public MessageController(MessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        [Authorize]
        [HttpGet("list-message")]
        public async Task<IActionResult> ListMessageAsync([FromQuery] Guid idFriendship)
        {
            var result = await _messageHandler.GetListMessageAsync(idFriendship);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageRequest messageRequest)
        {
            var result = await _messageHandler.CreateMessageAsync(messageRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-message")]
        public async Task<IActionResult> DeleteMessageAsync([FromQuery] Guid idFriendship)
        {
            var result = await _messageHandler.DeleteMessageAsync(idFriendship);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
