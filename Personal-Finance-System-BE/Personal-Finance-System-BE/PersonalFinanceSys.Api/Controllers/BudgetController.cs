using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Budgets;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/budget/")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetHandler _budgetHandler;

        public BudgetController(BudgetHandler budgetHandler)
        {
            _budgetHandler = budgetHandler;
        }

        [Authorize]
        [HttpGet("list-budget")]
        public async Task<IActionResult> ListBudgetAsync([FromQuery] Guid idUser)
        {
            var result = await _budgetHandler.GetListBudgetAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("create-budget")]
        public async Task<IActionResult> AddBudgetAsync([FromBody] BudgetCreationRequest budgetCreationRequest)
        {
            var result = await _budgetHandler.CreateBudgetAsync(budgetCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-budget")]
        public async Task<IActionResult> UpdateBudgetAsync([FromBody] BudgetUpdateRequest budgetUpdateRequest,
                                                           [FromQuery] Guid idBudget)
        {
            var result = await _budgetHandler.UpdateBudgetAsync(idBudget, budgetUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-transaction")]
        public async Task<IActionResult> DeleteBudgetAsync([FromQuery] Guid idBudget)
        {
            var result = await _budgetHandler.DeleteBudgetAsync(idBudget);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
