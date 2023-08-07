using DropMate.Application.Contracts;

namespace DropMate.Application.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITravelPlanRepository TravelPlanRepository { get; }
        IPackageRepository PackageRepository { get; }
        IReviewRepository ReviewRepository { get; }
        Task SaveAsync();
    }
}
