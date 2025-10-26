using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.SavingGoals;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/saving-detail/")]
    [ApiController]
    public class SavingDetailController : ControllerBase
    {
        private readonly SavingDetailHandler _savingDetailHandler;

        public SavingDetailController(SavingDetailHandler savingDetailHandler)
        {
            _savingDetailHandler = savingDetailHandler;
        }

        [HttpGet("list-saving-detail")]
        public async Task<IActionResult> ListSavingDetailAsync([FromQuery] Guid idSavingGoal)
        {
            var result = await _savingDetailHandler.GetListSavingDetailsAsync(idSavingGoal);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("create-saving-detail")]
        public async Task<IActionResult> AddSavingDetailAsync([FromBody] SavingDetailRequest savingDetailRequest)
        {
            var result = await _savingDetailHandler.CreateSavingDetailAsync(savingDetailRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete-saving-detail")]
        public async Task<IActionResult> DeleteSavingDetailAsync([FromQuery] Guid idSavingDetail)
        {
            var result = await _savingDetailHandler.DeleteSavingDetailAsync(idSavingDetail);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
