using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.AI;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/Ai-Chatting/")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly ChatHandler _chatHandler;

        public AiController(ChatHandler chatHandler)
        {
            _chatHandler = chatHandler;
        }

        [Authorize]
        [HttpPost("generate-message")]
        public async Task<IActionResult> SendMessageForAI([FromBody] ChatRequest chatRequest)
        {
            var result = await _chatHandler.SendMessageForAI(chatRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("welcome-message")]
        public async Task<IActionResult> GetWelcomeMessage()
        {
            var result = await _chatHandler.GetWelcomeMessageAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("get-history")]
        public async Task<IActionResult> GetHistoryChatWithAI([FromQuery] Guid idUser)
        {
            var result = await _chatHandler.GetChatHistoryAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-history")]
        public async Task<IActionResult> DeleteHistoryChatWithAI([FromQuery] Guid idUser)
        {
            var result = await _chatHandler.DeleteChatHistoryAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
