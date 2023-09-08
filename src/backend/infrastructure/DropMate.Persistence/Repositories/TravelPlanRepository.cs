using DropMate.Application.Contracts;
using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using DropMate.Persistence.Common;
using DropMate.Persistence.Extensions;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Repositories
{
    internal sealed class TravelPlanRepository : RepositoryBase<TravelPlan>, ITravelPlanRepository
    {
        public TravelPlanRepository(RepositoryContext repository) : base(repository)
        {

        }

        public void CreateTravelPlan(TravelPlan travelPlan)
        {
            Add(travelPlan);
        }
        public void DeleteTravelPlan(TravelPlan travelPlan)
        {
            Delete(travelPlan);
        }
        public void PermanentDeleteTravelPlan(TravelPlan travelPlan)
        {
            PermanentDelete(travelPlan);
        }

        public void PermanentDeleteMultiTravelPlan(IEnumerable<TravelPlan> travelPlans)
        {
            PermanentDeleteMultiTravelPlan(travelPlans);
        }

        public async Task<PagedList<TravelPlan>> GetAllTravelPlanAsync(TravelPlanRequestParameters requestParameters, bool trackChanges)
        {
            List<TravelPlan> plans = await FindAll(trackChanges).Where(t => !t.IsDeleted)
                .Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
                .Take(requestParameters.PageSize).Sort(requestParameters.OrderBy).Include(t => t.Packages)
                .Include(t => t.Traveler).ToListAsync();
            int count = await FindAll(trackChanges).Where(t => !t.IsDeleted).CountAsync();
            return new PagedList<TravelPlan>(plans, count, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<IEnumerable<TravelPlan>> GetTravelPlanByDestinationAsync(LagosLocation destination, bool trackChanges)
        {
            return await FindByCondition(t => !t.IsDeleted && t.IsActive && t.ArrivalLocation.Equals(destination) && t.DepartureDateTime>DateTime.Now, trackChanges)
                .Include(t => t.Traveler).ToListAsync();
        }
        
        public async Task<IEnumerable<TravelPlan>> GetTravelPlanPackageByDestinationAsync(LagosLocation destination, bool trackChanges)
        {
            return await FindByCondition(t => !t.IsDeleted && t.IsActive && t.ArrivalLocation.Equals(destination) && t.DepartureDateTime>DateTime.Now, trackChanges)
                .Include(t => t.Packages).ToListAsync();
        }
        
        public async Task<TravelPlan> GetTravelPlanByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(t => !t.IsDeleted && t.Id.Equals(id), trackChanges)
                .Include(t => t.Packages).Include(t => t.Traveler).FirstOrDefaultAsync();
        }

        public void UpdateTravelPlan(TravelPlan travelPlan)
        {
            Update(travelPlan);
        }

        public async Task<PagedList<TravelPlan>> GetAllUserTravelPlanAsync(TravelPlanRequestParameters requestParameters, string userId, bool trackChanges)
        {
            List<TravelPlan> plans = await FindByCondition(t => !t.IsDeleted && t.TravelerId.ToLower()
            .Contains(userId.ToLower()), trackChanges).Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize)
            .Take(requestParameters.PageSize).Include(t => t.Packages).Sort(requestParameters.OrderBy)
            .Include(t => t.Traveler).ToListAsync();
            int count = await FindByCondition(t => !t.IsDeleted && t.TravelerId.ToLower().Contains(userId.ToLower()), trackChanges)
                .CountAsync();
            return new PagedList<TravelPlan>(plans, count,requestParameters.PageNumber,requestParameters.PageSize);
        }
    }
}
