using DropMate.Domain.Models;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Application.Contracts
{
    public interface IReviewRepository
    {
        Task<PagedList<Review>> GetAllReviewsAsync(ReviewRequestParameters requestParameters, bool trackChanges);
        //Task<IEnumerable<Review>> GetAllTravelPlanReviewsAsync(int travelPlanId, bool trackChanges);
        //Task<IEnumerable<Review>> GetAllUserReviewsAsync(string userId, bool trackChanges);
        Task<Review> GetReviewByIdAsync(int id, bool trackChanges);
        void DeleteReview(Review review);
        void PermanentDeleteReview(Review review);
        void PermanentDeleteMultiReview(IEnumerable<Review> reviews);
        void CreateReview(Review review);
    }
}
