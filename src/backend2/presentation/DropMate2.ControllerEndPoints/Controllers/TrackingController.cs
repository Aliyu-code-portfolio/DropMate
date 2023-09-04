using DropMate2.Service.Services;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.ControllerEndPoints.Controllers
{
    [Route("api/tracking")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly IHubContext<LocationTracker> _locationHunContext;

        public TrackingController(IHubContext<LocationTracker> locationHunContext)
        {
            _locationHunContext = locationHunContext;
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationBroadCastDto broadcastDto)
        {
            await _locationHunContext.Clients.Group(broadcastDto.TravelPlanId.ToString())
                .SendAsync("RealTimeLocation", broadcastDto.Latitude, broadcastDto.Longitude);
            return Ok();
        }
        
        [HttpPost("add-group")]
        public async Task<IActionResult> AddToGroup(string userId,int travelPlanId)
        {
            await _locationHunContext.Groups.AddToGroupAsync(userId, travelPlanId.ToString());
            return Ok();
        }
        
        [HttpPost("remove-group")]
        public async Task<IActionResult> RemoveFromGroup(string userId, int travelPlanId)
        {
            await _locationHunContext.Groups.RemoveFromGroupAsync(userId, travelPlanId.ToString());
            return Ok();
        }

    }
}
