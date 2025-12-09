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
        public async Task<IActionResult> ListMessageAsync([FromQuery] ListMessageRequest request)
        {
            var result = await _messageHandler.GetListMessageAsync(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
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
        public async Task<IActionResult> DeleteMessageAsync([FromQuery] Guid idMessage)
        {
            var result = await _messageHandler.DeleteMessageAsync(idMessage);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
