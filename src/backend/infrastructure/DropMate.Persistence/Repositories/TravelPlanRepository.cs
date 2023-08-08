using DropMate.Application.Contracts;
using DropMate.Domain.Models;
using DropMate.Persistence.Common;
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
        public TravelPlanRepository(RepositoryContext repository):base(repository)
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

        public async Task<IEnumerable<TravelPlan>> GetAllTravelPlanAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Where(t => !t.IsDeleted).Include(t => t.Packages)
                .Include(t => t.Traveler).ToListAsync();
        }

        public async Task<TravelPlan> GetTravelPlanByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(t => !t.IsDeleted && t.Id.Equals(id), trackChanges).Where(t => !t.IsDeleted)
                .Include(t => t.Packages).Include(t => t.Traveler).FirstOrDefaultAsync();
        }

        public void UpdateTravelPlan(TravelPlan travelPlan)
        {
            Update(travelPlan);
        }

    }
}
