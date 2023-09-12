using DropMate2.Application.ServiceContracts;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(result.Data.deposits);
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllUserDeposits(string userId, [FromQuery]DepositRequestParameter requestParameter)
        {
            var result = await _services.DepositService.GetAllWalletDeposit(requestParameter,userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.deposits);
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
            return Ok("Payment Received...");
        }

        [HttpPost]
        public async Task<IActionResult> InitializeDeposit([FromBody] DepositRequestDto requestDto)
        {
            var result = await _services.DepositService.InitializeDeposit(requestDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            await _services.DepositService.DeleteDeposit(id);
            return NoContent();
        }
    }
}
