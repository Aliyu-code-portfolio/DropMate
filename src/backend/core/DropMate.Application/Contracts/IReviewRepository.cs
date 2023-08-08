using DropMate.Domain.Models;

namespace DropMate.Application.Contracts
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync(bool trackChanges);
        //Task<IEnumerable<Review>> GetAllTravelPlanReviewsAsync(int travelPlanId, bool trackChanges);
        //Task<IEnumerable<Review>> GetAllUserReviewsAsync(string userId, bool trackChanges);
        Task<Review> GetReviewByIdAsync(int id, bool trackChanges);
        void DeleteReview(Review review);
        void PermanentDeleteReview(Review review);
        void PermanentDeleteMultiReview(IEnumerable<Review> reviews);
        void CreateReview(Review review);
    }
}
