using DropMate.Application.Common;
using DropMate.Application.Contracts;
using DropMate.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<ITravelPlanRepository> _travelPlanRepository;
        private readonly Lazy<IPackageRepository> _packageRepository;
        private readonly Lazy<IReviewRepository> _reviewRepository;

        public UnitOfWork(RepositoryContext repository)
        {
            _context = repository;
            _userRepository = new Lazy<IUserRepository>(()=>new UserRepository(repository));
            _travelPlanRepository = new Lazy<ITravelPlanRepository>(()=>new TravelPlanRepository(repository));
            _packageRepository = new Lazy<IPackageRepository>(()=>new PackageRepository(repository));
            _reviewRepository = new Lazy<IReviewRepository>(()=>new ReviewRepository(repository));
        }

        public IUserRepository UserRepository =>_userRepository.Value;

        public ITravelPlanRepository TravelPlanRepository => _travelPlanRepository.Value;

        public IPackageRepository PackageRepository => _packageRepository.Value;

        public IReviewRepository ReviewRepository => _reviewRepository.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
