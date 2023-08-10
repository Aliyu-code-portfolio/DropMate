using DropMate.Domain.Models;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.Contracts
{
    public interface IPackageRepository
    {
        Task<PagedList<Package>> GetAllPackagesAsync(PackageRequestParameter requestParameter, bool trackChanges);
        Task<PagedList<Package>> GetAllUserPackagesAsync(PackageRequestParameter requestParameter, string userId, bool trackChanges);
        Task<PagedList<Package>> GetAllTravelPlanPackagesAsync(PackageRequestParameter requestParameter, int travelPlanId, bool trackChanges);
        Task<Package> GetPackageByIdAsync(int id, bool trackChanges);
        void UpdatePackage(Package package);
        void DeletePackage(Package package);
        void PermanentDeletePackage(Package package);
        void PermanentDeleteMultiPackage(IEnumerable<Package> packages);
        void CreatePackage(Package package);
    }
}
