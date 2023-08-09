using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.ServiceContracts
{
    public interface ITravelPlanService
    {
        Task<StandardResponse<TravelPlanResponse>> CreateTravelPlan(TravelPlanRequestDto requestDto);
        Task<StandardResponse<TravelPlanResponse>> GetTravelPlanById(int id, bool trackChanges);
        Task<StandardResponse<IEnumerable<TravelPlanResponse>>> GetAllUserTravelPlan(string userId, bool trackChanges);
        Task<StandardResponse<IEnumerable<TravelPlanResponse>>> GetAllTravelPlan(bool trackChanges);
        Task UpdateTravelPlan(int id, TravelPlanRequestDto requestDto);
        Task DeleteTravelPlan(int id);
    }
}
