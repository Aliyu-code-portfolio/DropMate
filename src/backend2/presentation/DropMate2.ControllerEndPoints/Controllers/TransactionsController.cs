using DropMate2.Application.ServiceContracts;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DropMate2.ControllerEndPoints.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IServiceManager _services;

        public TransactionsController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionRequestParameters requestParameter)
        {
            var result = await _services.TransactionService.GetAllTransaction(requestParameter, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.transactions);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllTransactions(string userId, [FromQuery] TransactionRequestParameters requestParameter)
        {
            var result = await _services.TransactionService.GetAllUserTransaction(requestParameter, userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var result = await _services.TransactionService.GetTransactionById(id, false);
            return Ok(result);
        }

        [HttpPost("confirm/{packageId}/{isComplete}")]
        public async Task<IActionResult> ConfirmTransaction(int packageId, bool isComplete)
        {
            await _services.TransactionService.CompleteTransaction(packageId,isComplete);
            return Ok("Completed and payment disbursed...");
        }
        
        [HttpPost("refund/{userId}/{packageId}")]
        public async Task<IActionResult> RefundTransaction(int packageId)
        {
            await _services.TransactionService.RefundPackagePayment(packageId);
            return Ok("Completed and payment refunded...");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequestDto requestDto)
        {
            var result = await _services.TransactionService.CreateTransaction(requestDto);
            return CreatedAtAction(nameof(GetTransactionById), new {Id = result.Data.Id}, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody]TransactionRequestDto requestDto)
        {
            await _services.TransactionService.UpdateTransaction(id, requestDto);
            return Ok("Successfully updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _services.TransactionService.DeleteTransaction(id);
            return NoContent();
        }
    }
}
