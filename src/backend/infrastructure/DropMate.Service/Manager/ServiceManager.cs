using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Service.Manager
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ITravelPlanService> _travelPlanService;
        private readonly Lazy<IPackageService> _packageService;
        private readonly Lazy<IReviewService> _reviewService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userService = new Lazy<IUserService>(()=> new UserService(unitOfWork, mapper));
            _travelPlanService = new Lazy<ITravelPlanService>(()=> new TravelPlanService(unitOfWork,mapper));
            _packageService = new Lazy<IPackageService>(()=> new PackageService(unitOfWork,mapper));
            _reviewService = new Lazy<IReviewService>(() => new ReviewService(unitOfWork, mapper));
        }
        public IUserService UserService => _userService.Value;

        public ITravelPlanService TravelPlanService => _travelPlanService.Value;

        public IPackageService PackageService => _packageService.Value;

        public IReviewService ReviewService => _reviewService.Value;
    }
}
