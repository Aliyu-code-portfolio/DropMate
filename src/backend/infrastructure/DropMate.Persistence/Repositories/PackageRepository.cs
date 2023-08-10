using DropMate.Application.Contracts;
using DropMate.Domain.Models;
using DropMate.Persistence.Common;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagedList<Package>> GetAllPackagesAsync(PackageRequestParameter requestParameter,bool trackChanges)
        {
            List<Package> packages = await FindAll(trackChanges).Where(p => !p.IsDeleted).Skip((requestParameter.PageNumber-1)*requestParameter.PageSize)
                .Take(requestParameter.PageSize).Include(p => p.Owner)
                .Include(p => p.TravelPlan).Include(p => p.Review).ToListAsync();
            int count = await FindAll(trackChanges).Where(p => !p.IsDeleted).CountAsync();  
            return new PagedList<Package>(packages, count,requestParameter.PageNumber,requestParameter.PageSize);
        }

        public async Task<PagedList<Package>> GetAllTravelPlanPackagesAsync(PackageRequestParameter requestParameter, int travelPlanId, bool trackChanges)
        {
            List<Package> packages = await FindByCondition(p => p.TravelPlanId.Equals(travelPlanId), trackChanges)
                .Skip((requestParameter.PageNumber - 1) * requestParameter.PageSize)
                .Take(requestParameter.PageSize)
                .Where(p => !p.IsDeleted).ToListAsync();
            int count = await FindAll(trackChanges).Where(p => !p.IsDeleted).CountAsync();
            return new PagedList<Package>(packages, count, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<PagedList<Package>> GetAllUserPackagesAsync(PackageRequestParameter requestParameter, string userId, bool trackChanges)
        {
            List<Package> packages = await FindByCondition(p => p.PackageOwnerId.Contains(userId), trackChanges)
                .Skip((requestParameter.PageNumber - 1) * requestParameter.PageSize)
                .Take(requestParameter.PageSize)
                .Where(p => !p.IsDeleted).ToListAsync();
            int count = await FindAll(trackChanges).Where(p => !p.IsDeleted).CountAsync();
            return new PagedList<Package>(packages, count, requestParameter.PageNumber, requestParameter.PageSize);
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
