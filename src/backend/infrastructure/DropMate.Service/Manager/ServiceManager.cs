using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Models;
using DropMate.Service.Services;
using DropMate.Shared.HelperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper,UserManager<User> userManager, IPhotoService photoService,IEmailService mailService, IConfiguration configuration)
        {
            _userService = new Lazy<IUserService>(()=> new UserService(unitOfWork, mapper,photoService));
            _travelPlanService = new Lazy<ITravelPlanService>(()=> new TravelPlanService(unitOfWork,mapper));
            _packageService = new Lazy<IPackageService>(()=> new PackageService(unitOfWork,mapper, photoService, configuration));
            _reviewService = new Lazy<IReviewService>(() => new ReviewService(unitOfWork, mapper));
            _authenticationService =  new Lazy<IAuthenticationService>(()=>new AuthenticationService(unitOfWork,mapper,userManager,mailService,configuration));
        }
        public IUserService UserService => _userService.Value;

        public ITravelPlanService TravelPlanService => _travelPlanService.Value;

        public IPackageService PackageService => _packageService.Value;

        public IReviewService ReviewService => _reviewService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

    }
}
