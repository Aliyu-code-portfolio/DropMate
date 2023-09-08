using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/packages")]
    public class PackagesV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public PackagesV1Controller(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [HttpHead]
        [ResponseCache(CacheProfileName = "20 minutes cache")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPackages([FromQuery]PackageRequestParameter requestParameter)
        {
            StandardResponse<(IEnumerable<PackageResponseDto> packages, MetaData metaData)> result = await _services.PackageService
                .GetAllPackagesAsync(requestParameter,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.packages);
        }

        [HttpGet("user/id")]
        [ResponseCache(CacheProfileName = "20 minutes cache")]
        public async Task<IActionResult> GetAllUserPackages([FromQuery] PackageRequestParameter requestParameter)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            StandardResponse<(IEnumerable<PackageResponseDto> packages, MetaData metaData)> result = await _services.PackageService
                .GetAllUserPackagesAsync(requestParameter, userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result);
        }

        [HttpGet("plan/{planId}")]
        [ResponseCache(CacheProfileName = "20 minutes cache")]
        public async Task<IActionResult> GetAllTravelPlanPackages(int planId, [FromQuery] PackageRequestParameter requestParameter)
        {
            StandardResponse<(IEnumerable<PackageResponseDto>packages, MetaData metaData)> result = await _services.PackageService
                .GetAllTravelPlanPackagesAsync(requestParameter, planId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            StandardResponse<PackageResponseDto> package = await _services.PackageService.GetPackageByIdAsync(id, false);
            return Ok(package);
        }

        [Authorize]
        [HttpGet("{id}/recieve")]
        public async Task<IActionResult> UpdateRecievedStatus(int id,int code)
        {
            await _services.PackageService.UpdateStatusRecieved(id,code);
            return Ok();
        }
        
        [HttpGet("{id}/deliver")]
        public async Task<IActionResult> UpdateDeliveredStatus(int id,int code)
        {
            string token = Request.Headers.Authorization;
            await _services.PackageService.UpdateStatusDelivered(id,code, token);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromForm] PackageRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            StandardResponse<(PackageResponseDto, IEnumerable<TravelPlanResponse>)> result = await _services.PackageService.CreatePackage(userId, requestDto);
            return CreatedAtAction(nameof(GetPackageById), new { Id = result.Data.Item1.Id }, result.Data.Item2);
        }
        
        [HttpPost("{id}/package-img")]
        public async Task<IActionResult> UploadPackageImg(int id, IFormFile file)
        {
            StandardResponse<string> result = await _services.PackageService.UploadPackageImg(id,file);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromForm] PackageRequestDto requestDto)
        {
            await _services.PackageService.UpdatePackage(id, requestDto);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            string bearToken = Request.Headers.Authorization;
            await _services.PackageService.DeletePackage(id, bearToken);
            return Ok("Refunded and deleted successsfully");
        }
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
            return Ok();
        }

    }
}
