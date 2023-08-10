using DropMate.Domain.Models;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Application.Contracts
{
    public interface ITravelPlanRepository
    {
        Task<PagedList<TravelPlan>> GetAllTravelPlanAsync(TravelPlanRequestParameters requestParameters, bool trackChanges);
        Task<TravelPlan> GetTravelPlanByIdAsync(int id, bool trackChanges);
        Task<PagedList<TravelPlan>> GetAllUserTravelPlanAsync(TravelPlanRequestParameters requestParameters, string userId, bool trackChanges);
        void UpdateTravelPlan(TravelPlan travelPlan);
        void DeleteTravelPlan(TravelPlan travelPlan);
        void PermanentDeleteTravelPlan(TravelPlan travelPlan);
        void PermanentDeleteMultiTravelPlan(IEnumerable<TravelPlan> travelPlans);
        void CreateTravelPlan(TravelPlan travelPlan);
    }
}
