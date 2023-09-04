using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Application.ServiceContracts
{
    public interface ILocationTracker
    {
        Task SendLocation(string travelPlanId, double latitude, double longitude);
        Task JoinTravelPlan(string travelPlanId);
        Task LeaveTravelPlan(string travelPlanId);
    }
}
