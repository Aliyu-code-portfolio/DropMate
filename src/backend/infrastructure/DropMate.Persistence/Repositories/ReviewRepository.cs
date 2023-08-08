using DropMate.Application.Contracts;
using DropMate.Domain.Models;
using DropMate.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Repositories
{
    internal sealed class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(RepositoryContext repository):base(repository)
        {
            
        }
        public void CreateReview(Review review)
        {
            Add(review);
        }

        public void DeleteReview(Review review)
        {
            Delete(review);
        }
        public void PermanentDeleteReview(Review review)
        {
            PermanentDelete(review);
        }
        public void PermanentDeleteMultiReview(IEnumerable<Review> reviews)
        {
            PermanentDeleteRange(reviews);
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Include(r=>r.Package).ToListAsync();
        }

        /*public async Task<IEnumerable<Review>> GetAllTravelPlanReviewsAsync(int travelPlanId, bool trackChanges)
        {
            return await FindByCondition(r=>r.)
        }*/

        /*public async Task<IEnumerable<Review>> GetAllUserReviewsAsync(string userId, bool trackChanges)
        {
            return await FindByCondition(r=>r.UserId.Equals(userId),trackChanges).Include(r => r.Package).ToListAsync();
        }*/

        public async Task<Review> GetReviewByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(r=>r.Id.Equals(id),trackChanges).Include(r => r.Package).FirstOrDefaultAsync();
        }

    }
}
