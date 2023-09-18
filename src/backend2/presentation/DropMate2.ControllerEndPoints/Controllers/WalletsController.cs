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
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllWallets([FromQuery] WalletRequestParameter requestParameter)
        {
            var result = await _services.WalletService.GetAllWallets(requestParameter, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<WalletResponseDto>>.Success("Retrieved successfully", result.Data.wallets, 200));
        }

        [HttpGet("id")]
        [Authorize]
        public async Task<IActionResult> GetWalletById()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            var result = await _services.WalletService.GetWalletById(userId, false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] WalletRequestDto requestDto)
        {
           await _services.WalletService.CreateWallet(requestDto);
            return NoContent();
        }


        [HttpDelete("id")]
        [Authorize]
        public async Task<IActionResult> DeleteWallet()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            await _services.WalletService.DeleteWallet(userId);
            return NoContent();
        }
    }
}
