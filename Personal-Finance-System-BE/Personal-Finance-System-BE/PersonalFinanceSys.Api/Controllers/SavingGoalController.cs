using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.SavingGoals;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/saving-goal/")]
    [ApiController]
    public class SavingGoalController : ControllerBase
    {
        private readonly SavingGoalHandler _savingGoalHandler;

        public SavingGoalController(SavingGoalHandler savingGoalHandler)
        {
            _savingGoalHandler = savingGoalHandler;
        }

        [HttpGet("list-saving-goal")]
        public async Task<IActionResult> ListSavingGoalAsync([FromQuery] Guid idUser)
        {
            var result = await _savingGoalHandler.GetListSavingGoalAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("inf-saving-goal")]
        public async Task<IActionResult> InfSavingGoalAsync([FromQuery] Guid idSavingGoal)
        {
            var result = await _savingGoalHandler.GetSavingGoalByIdAsync(idSavingGoal);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("create-saving-goal")]
        public async Task<IActionResult> AddSavingGoalAsync([FromBody] SavingGoalCreationRequest savingGoalCreationRequest)
        {
            var result = await _savingGoalHandler.CreateSavingGoalAsync(savingGoalCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-saving-goal")]
        public async Task<IActionResult> UpdateSavingGoalAsync([FromBody] SavingGoalUpdateRequest savingGoalUpdateRequest,
                                                                [FromQuery] Guid idSavingGoal)
        {
            var result = await _savingGoalHandler.UpdateSavingGoalAsync(idSavingGoal, savingGoalUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-saving-goal")]
        public async Task<IActionResult> DeleteSavingGoalAsync([FromQuery] Guid idSavingGoal)
        {
            var result = await _savingGoalHandler.DeleteSavingGoalAsync(idSavingGoal);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
