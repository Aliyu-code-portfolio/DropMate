using DropMate.Domain.Enums;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
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
        Task<StandardResponse<(IEnumerable<TravelPlanResponse>, MetaData)>> GetAllUserTravelPlan(TravelPlanRequestParameters requestParameters, string userId, bool trackChanges);
        Task<StandardResponse<(IEnumerable<TravelPlanResponse>, MetaData)>> GetAllTravelPlan(TravelPlanRequestParameters requestParameters, bool trackChanges);
        Task UpdateTravelPlan(int id, TravelPlanRequestDto requestDto);
        Task UpdateCompleted(int plabId, Status status);
        Task UpdateIsActive(int plabId, bool isActive);
        Task DeleteTravelPlan(int id);
    }
}
