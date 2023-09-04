using DropMate2.Application.ServiceContracts;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DropMate2.ControllerEndPoints.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IServiceManager _services;

        public WalletsController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllWallets([FromQuery] WalletRequestParameter requestParameter)
        {
            var result = await _services.WalletService.GetAllWallets(requestParameter, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.wallets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById(string id)
        {
            var result = await _services.WalletService.GetWalletById(id, false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] WalletRequestDto requestDto)
        {
           await _services.WalletService.CreateWallet(requestDto);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(string id)
        {
            await _services.WalletService.DeleteWallet(id);
            return NoContent();
        }
    }
}
