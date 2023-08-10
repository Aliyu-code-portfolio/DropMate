using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Application.ServiceContracts
{
    public interface IReviewService
    {
        Task<StandardResponse<(IEnumerable<ReviewResponseDto>,MetaData)>> GetAllReviewsAsync(ReviewRequestParameters requestParameters, bool trackChanges);
        Task<StandardResponse<ReviewResponseDto>> GetReviewByIdAsync(int id, bool trackChanges);
        Task DeleteReview(int id);
        Task<StandardResponse<ReviewResponseDto>> CreateReview(ReviewRequestDto requestDto);
    }
}
