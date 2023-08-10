using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {

        private readonly IServiceManager _services;

        public PackagesController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackages([FromQuery]PackageRequestParameter requestParameter)
        {
            StandardResponse<(IEnumerable<PackageResponseDto> packages, MetaData metaData)> result = await _services.PackageService
                .GetAllPackagesAsync(requestParameter,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.packages);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllUserPackages(string userId, [FromQuery] PackageRequestParameter requestParameter)
        {
            StandardResponse<(IEnumerable<PackageResponseDto> packages, MetaData metaData)> result = await _services.PackageService
                .GetAllUserPackagesAsync(requestParameter, userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result);
        }

        [HttpGet("plan/{planId}")]
        public async Task<IActionResult> GetAllPackages(int planId, [FromQuery] PackageRequestParameter requestParameter)
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

        [HttpGet("{id}/recieve")]
        public async Task<IActionResult> UpdateRecievedStatus(int id,int code, Status status)
        {
            await _services.PackageService.UpdateStatusRecieved(id,code, status);
            return Ok();
        }
        
        [HttpGet("{id}/deliver")]
        public async Task<IActionResult> UpdateDeliveredStatus(int id,int code, Status status)
        {
            await _services.PackageService.UpdateStatusDelivered(id,code, status);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] PackageRequestDto requestDto)
        {
            StandardResponse<PackageResponseDto> package = await _services.PackageService.CreatePackage(requestDto);
            return CreatedAtAction(nameof(GetPackageById), new { Id = package.Data.Id }, package.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] PackageRequestDto requestDto)
        {
            await _services.PackageService.UpdatePackage(id, requestDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            await _services.PackageService.DeletePackage(id);
            return Ok();
        }

    }
}
