using DropMate.Domain.Models;

namespace DropMate.Application.Contracts
{
    public interface ITravelPlanRepository
    {
        Task<IEnumerable<TravelPlan>> GetAllTravelPlanAsync(bool trackChanges);
        Task<TravelPlan> GetTravelPlanByIdAsync(int id, bool trackChanges);
        void UpdateTravelPlan(TravelPlan travelPlan);
        void DeleteTravelPlan(TravelPlan travelPlan);
        void DeleteMultiTravelPlan(IEnumerable<TravelPlan> travelPlans);
        void CreateTravelPlan(TravelPlan travelPlan);
    }
}
