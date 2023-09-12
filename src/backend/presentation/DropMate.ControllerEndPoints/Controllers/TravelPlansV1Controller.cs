using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/travel-plans")]
    public class TravelPlanV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public TravelPlanV1Controller(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [HttpHead]
        [ResponseCache(CacheProfileName = "20 minutes cache")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTravelPlans([FromQuery] TravelPlanRequestParameters requestParameters)
        {
            StandardResponse<(IEnumerable<TravelPlanResponse> plans, MetaData metaData)> result = await _services.TravelPlanService
                .GetAllTravelPlan(requestParameters,false);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<TravelPlanResponse>>.Success("Retrieved successfully", result.Data.plans));
        } 
        
        [HttpGet("user/id")]
        [ResponseCache(CacheProfileName = "20 minutes cache")]
        public async Task<IActionResult> GetAllUserTravelPlans([FromQuery] TravelPlanRequestParameters requestParameters)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = userIdClaim.Value;
            StandardResponse<(IEnumerable<TravelPlanResponse> plans, MetaData metaData)> result = await _services.TravelPlanService
                .GetAllUserTravelPlan(requestParameters,id,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<TravelPlanResponse>>.Success("Retrieved successfully", result.Data.plans));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTravelPlanById(int id)
        {
            StandardResponse<TravelPlanResponse> plan = await _services.TravelPlanService.GetTravelPlanById(id, false);
            return Ok(plan);
        }


        [HttpGet("{id}/complete")]
        public async Task<IActionResult> UpdateIsCompleted(int id,[FromForm] Status status)
        {
            await _services.TravelPlanService.UpdateCompleted(id, status);
            return Ok();
        }

        [HttpPost("{id}/active")]
        public async Task<IActionResult> UpdateIsActive(int id, bool isActive)
        {
            await _services.TravelPlanService.UpdateIsActive(id, isActive);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTravelPlan([FromForm] TravelPlanRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            StandardResponse<TravelPlanResponse> plan = await _services.TravelPlanService.CreateTravelPlan(userId, requestDto);
            return CreatedAtAction(nameof(GetTravelPlanById), new { Id = plan.Data.Id }, plan.Data);
        }
        
        [HttpPost("add-package")]
        public async Task<IActionResult> AddTravelPlanPackage([FromForm] BookingRequestDto requestDto)
        {
            string token = Request.Headers.Authorization;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            await _services.TravelPlanService.AddPackageToTravelPlan(userId,requestDto.travelPlanId, requestDto.packageId, token);
            return Ok("Package added successfully");
        }
        
        [HttpPost("remove-package")]
        public async Task<IActionResult> RemoveTravelPlanPackage([FromForm] BookingRequestDto requestDto)
        {
            string token = Request.Headers.Authorization;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            await _services.TravelPlanService.RemovePackageFromTravelPlan(userId,requestDto.travelPlanId, requestDto.packageId, token);
            return Ok("Package removed successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTravelPlan(int id, [FromForm] TravelPlanRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            await _services.TravelPlanService.UpdateTravelPlan(userId, id, requestDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelPlan(int id)
        {
            await _services.TravelPlanService.DeleteTravelPlan(id);
            return Ok();
        }
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
            return Ok();
        }
    }
    }
