﻿using DropMate.Domain.Models;

namespace DropMate.Application.Contracts
{
    public interface ITravelPlanRepository
    {
        Task<IEnumerable<TravelPlan>> GetAllTravelPlanAsync(bool trackChanges);
        Task<TravelPlan> GetTravelPlanByIdAsync(int id, bool trackChanges);
        Task<IEnumerable<TravelPlan>> GetAllUserTravelPlanAsync(string userId, bool trackChanges);
        void UpdateTravelPlan(TravelPlan travelPlan);
        void DeleteTravelPlan(TravelPlan travelPlan);
        void PermanentDeleteTravelPlan(TravelPlan travelPlan);
        void PermanentDeleteMultiTravelPlan(IEnumerable<TravelPlan> travelPlans);
        void CreateTravelPlan(TravelPlan travelPlan);
    }
}
