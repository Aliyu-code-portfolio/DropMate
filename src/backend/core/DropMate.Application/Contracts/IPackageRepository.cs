using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.Contracts
{
    public interface IPackageRepository
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync(bool trackChanges);
        Task<IEnumerable<Package>> GetAllUserPackagesAsync(string userId, bool trackChanges);
        Task<IEnumerable<Package>> GetAllTravelPlanPackagesAsync(int id, bool trackChanges);
        Task<Package> GetPackageByIdAsync(int id, bool trackChanges);
        void UpdatePackage(Package package);
        void DeletePackage(Package package);
        void DeleteMultiPackage(IEnumerable<Package> packages);
        void CreatePackage(Package package);
    }
}
