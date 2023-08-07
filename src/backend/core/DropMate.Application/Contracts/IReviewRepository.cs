using DropMate.Domain.Models;

namespace DropMate.Application.Contracts
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync(bool trackChanges);
        Task<IEnumerable<Review>> GetAllTravelPlanReviewsAsync(int id, bool trackChanges);
        Task<IEnumerable<Review>> GetAllUserReviewsAsync(string id, bool trackChanges);
        Task<Review> GetReviewByIdAsync(int id, bool trackChanges);
        void DeleteMultiReview(IEnumerable<Review> reviews);
        void DeleteReview(Review review);
        void CreateReview(Review review);
    }
}
