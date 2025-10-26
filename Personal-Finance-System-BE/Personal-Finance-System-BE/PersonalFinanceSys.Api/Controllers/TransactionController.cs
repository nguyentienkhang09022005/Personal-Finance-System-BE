using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Transactions;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/transaction/")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionHandler _transactionHandler;

        public TransactionController(TransactionHandler transactionHandler)
        {
            _transactionHandler = transactionHandler;
        }

        [Authorize]
        [HttpGet("list-transaction")]
        public async Task<IActionResult> ListTransactionAsync([FromQuery] Guid idUser)
        {
            var result = await _transactionHandler.GetListTransactionAsync(idUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("create-transaction")]
        public async Task<IActionResult> AddTransactionAsync([FromBody] TransactionCreationRequest transactionCreationRequest)
        {
            var result = await _transactionHandler.CreateTransactionAsync(transactionCreationRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-transaction")]
        public async Task<IActionResult> UpdateTransactionAsync([FromBody] TransactionUpdateRequest transactionUpdateRequest,
                                                                [FromQuery] Guid idTransaction)
        {
            var result = await _transactionHandler.UpdateTransactionAsync(idTransaction, transactionUpdateRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpDelete("delete-transaction")]
        public async Task<IActionResult> DeleteTransactionAsync([FromQuery] Guid idTransaction)
        {
            var result = await _transactionHandler.DeleteTransactionAsync(idTransaction);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
