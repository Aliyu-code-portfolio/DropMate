using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.ServiceContracts
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        ITravelPlanService TravelPlanService { get; }
        IPackageService PackageService { get; }
        IReviewService ReviewService { get; }
        IAuthenticationService AuthenticationService { get; }
        IPhotoService PhotoService { get; }
    }
}
