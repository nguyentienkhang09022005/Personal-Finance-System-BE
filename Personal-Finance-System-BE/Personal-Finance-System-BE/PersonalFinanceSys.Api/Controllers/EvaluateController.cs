using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/evaluate/")]
    [ApiController]
    public class EvaluateController : ControllerBase
    {
        private readonly EvaluateHandler _evaluateHandler;

        public EvaluateController(EvaluateHandler evaluateHandler)
        {
            _evaluateHandler = evaluateHandler;
        }

        [HttpPost("create-evaluate")]
        public async Task<IActionResult> AddEvaluateAsync([FromBody] EvaluateCreationRequest evaluateCreationRequest)
        {
            var result = await _evaluateHandler.CreateEvaluateAsync(evaluateCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-evaluate")]
        public async Task<IActionResult> UpdateEvaluateAsync([FromBody] EvaluateUpdateRequest evaluateUpdateRequest,
                                                             [FromQuery] Guid idEvaluate)
        {
            var result = await _evaluateHandler.UpdateEvaluateAsync(idEvaluate, evaluateUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-evaluate")]
        public async Task<IActionResult> DeleteEvaluateAsync([FromQuery] Guid idEvaluate)
        {
            var result = await _evaluateHandler.DeleteEvaluateAsync(idEvaluate);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
