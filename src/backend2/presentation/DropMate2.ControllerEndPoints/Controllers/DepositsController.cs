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
    [Route("api/deposits")]
    [ApiController]
    [Authorize]
    public class DepositsController : ControllerBase
    {
        private readonly IServiceManager _services;

        public DepositsController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllDeposits([FromQuery]DepositRequestParameter requestParameter)
        {
            var result = await _services.DepositService.GetAllDeposit(requestParameter, false);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<DepositResponseDto>>.Success("Retrieved successfully", result.Data.deposits, 200));
        }
        
        [HttpGet("user/id")]
        public async Task<IActionResult> GetAllUserDeposits([FromQuery]DepositRequestParameter requestParameter)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            var result = await _services.DepositService.GetAllWalletDeposit(requestParameter,userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<DepositResponseDto>>.Success("Retrieved successfully", result.Data.deposits, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepositById(int id)
        {
            var result = await _services.DepositService.GetDepositById(id, false);
            return Ok(result);
        }
        
        [HttpGet("confirm/{reference}")]
        public async Task<IActionResult> ConfirmDeposit(string reference)
        {
            await _services.DepositService.CompleteDeposit(reference);
            return Ok(StandardResponse<string>.Success("Payment received successfully", null, 200));
        }

        [HttpPost]
        public async Task<IActionResult> InitializeDeposit([FromBody] DepositRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            string email = userEmailClaim.Value;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            var result = await _services.DepositService.InitializeDeposit(requestDto,email, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            await _services.DepositService.DeleteDeposit(id);
            return Ok(StandardResponse<string>.Success("Deposit deleted successfully", null, 200));
        }
    }
}
