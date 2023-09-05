using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/travel-plans")]
    [ApiController]
    public class TravelPlanV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public TravelPlanV1Controller(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllTravelPlans([FromQuery] TravelPlanRequestParameters requestParameters)
        {
            StandardResponse<(IEnumerable<TravelPlanResponse> plans, MetaData metaData)> result = await _services.TravelPlanService
                .GetAllTravelPlan(requestParameters,false);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.plans);
        } 
        
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllUserTravelPlans(string id, [FromQuery] TravelPlanRequestParameters requestParameters)
        {
            StandardResponse<(IEnumerable<TravelPlanResponse> plans, MetaData metaData)> result = await _services.TravelPlanService
                .GetAllUserTravelPlan(requestParameters,id,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.plans);
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

        [HttpGet("{id}/active")]
        public async Task<IActionResult> UpdateIsActive(int id, bool isActive)
        {
            await _services.TravelPlanService.UpdateIsActive(id, isActive);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTravelPlan([FromBody] TravelPlanRequestDto requestDto)
        {
            StandardResponse<TravelPlanResponse> plan = await _services.TravelPlanService.CreateTravelPlan(requestDto);
            return CreatedAtAction(nameof(GetTravelPlanById), new { Id = plan.Data.Id }, plan.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTravelPlan(int id, [FromBody] TravelPlanRequestDto requestDto)
        {
            await _services.TravelPlanService.UpdateTravelPlan(id, requestDto);
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
