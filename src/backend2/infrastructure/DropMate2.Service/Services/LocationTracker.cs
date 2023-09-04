using DropMate2.Application.ServiceContracts;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Service.Services
{
    public class LocationTracker: Hub
    {
        public async Task SendLocation(string travelPlanId, double latitude, double longitude)
        {
            await Clients.Group(travelPlanId).SendAsync("RealTimeLocation", latitude, longitude);
        }
        public async Task JoinTravelPlan(string travelPlanId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, travelPlanId);
        }
        public async Task LeaveTravelPlan(string travelPlanId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,travelPlanId);
        }
    }
}
