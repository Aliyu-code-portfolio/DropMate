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
    internal sealed class PackageRepository : RepositoryBase<Package>, IPackageRepository
    {
        public PackageRepository(RepositoryContext repository) : base(repository)
        {
        }

        public void CreatePackage(Package package)
        {
            Add(package);
        }

        public void DeletePackage(Package package)
        {
            Delete(package);
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Where(p => !p.IsDeleted).Include(p => p.Owner)
                .Include(p => p.TravelPlan).Include(p => p.Review).ToListAsync();
        }

        public async Task<IEnumerable<Package>> GetAllTravelPlanPackagesAsync(int travelPlanId, bool trackChanges)
        {
            return await FindByCondition(p => p.TravelPlanId.Equals(travelPlanId), trackChanges).Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Package>> GetAllUserPackagesAsync(string userId, bool trackChanges)
        {
            return await FindByCondition(p => p.PackageOwnerId.Equals(userId), trackChanges).Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<Package> GetPackageByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(p=>p.Id.Equals(id), trackChanges).Where(p=>!p.IsDeleted).FirstOrDefaultAsync();
        }

        public void PermanentDeletePackage(Package package)
        {
            PermanentDelete(package);
        }

        public void PermanentDeleteMultiPackage(IEnumerable<Package> packages)
        {
            PermanentDeleteRange(packages);
        }

        public void UpdatePackage(Package package)
        {
            Update(package);
        }
    }
}
