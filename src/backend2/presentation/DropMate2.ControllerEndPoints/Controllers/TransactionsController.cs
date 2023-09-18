using DropMate2.Application.ServiceContracts;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace DropMate2.ControllerEndPoints.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IServiceManager _services;

        public TransactionsController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionRequestParameters requestParameter)
        {
            var result = await _services.TransactionService.GetAllTransaction(requestParameter, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<TransactionResponseDto>>.Success("Retrieved successfully", result.Data.transactions, 200));
        }

        [HttpGet("user/id")]
        public async Task<IActionResult> GetAllUserTransactions([FromQuery] TransactionRequestParameters requestParameter)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            var result = await _services.TransactionService.GetAllUserTransaction(requestParameter, userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<TransactionResponseDto>>.Success("Retrieved successfully", result.Data.transactions, 200));
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
            return Ok(StandardResponse<string>.Success("Payment disbursed successfully", null, 200));
        }
        
        [HttpPost("refund/{packageId}")]
        public async Task<IActionResult> RefundTransaction(int packageId)
        {
            await _services.TransactionService.RefundPackagePayment(packageId);
            return Ok(StandardResponse<string>.Success("Payment refunded successfully", null, 200));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequestDto requestDto)
        {
            var result = await _services.TransactionService.CreateTransaction(requestDto);
            return CreatedAtAction(nameof(GetTransactionById), new {Id = result.Data.Id}, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody]TransactionRequestDto requestDto)
        {
            await _services.TransactionService.UpdateTransaction(id, requestDto);
            return Ok(StandardResponse<string>.Success("Transaction updated successfully", null, 200));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _services.TransactionService.DeleteTransaction(id);
            return Ok(StandardResponse<string>.Success("Transaction deleted successfully", null, 200));
        }
    }
}
