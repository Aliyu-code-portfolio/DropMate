using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Service.Services
{
    internal sealed class ReviewService:IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StandardResponse<ReviewResponseDto>> CreateReview(ReviewRequestDto requestDto)
        {
            Review review = _mapper.Map<Review>(requestDto);
            _unitOfWork.ReviewRepository.CreateReview(review);
            await _unitOfWork.SaveAsync();
            ReviewResponseDto reviewDto = _mapper.Map<ReviewResponseDto>(review);
            return new StandardResponse<ReviewResponseDto>(200, true, string.Empty, reviewDto);
        }

        public async Task DeleteReview(int id)
        {
            Review review = await GetReviewWithId(id, false);
            _unitOfWork.ReviewRepository.DeleteReview(review);
            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<(IEnumerable<ReviewResponseDto>,MetaData)>> GetAllReviewsAsync(ReviewRequestParameters requestParameters, bool trackChanges)
        {
            if (!requestParameters.IsValidRating)
            {
                throw new MaxRatingBadRequest();
            }
            PagedList<Review> reviews = await _unitOfWork.ReviewRepository.GetAllReviewsAsync(requestParameters ,trackChanges);
            IEnumerable<ReviewResponseDto> reviewsDtos = _mapper.Map<IEnumerable<ReviewResponseDto>>(reviews);
            return new StandardResponse<(IEnumerable<ReviewResponseDto>,MetaData)>(200, true, string.Empty, (reviewsDtos,reviews.MetaData));
        }

        public async Task<StandardResponse<ReviewResponseDto>> GetReviewByIdAsync(int id, bool trackChanges)
        {
            Review review = await GetReviewWithId(id, trackChanges);
            ReviewResponseDto reviewsDto = _mapper.Map<ReviewResponseDto>(review);
            return new StandardResponse<ReviewResponseDto>(200, true, string.Empty, reviewsDto);
        }
        private async Task<Review> GetReviewWithId(int id, bool trackChanges)
        {
            Review review = await _unitOfWork.ReviewRepository.GetReviewByIdAsync(id, trackChanges)
                ?? throw new ReviewNotFoundException(id);
            return review;
        }
    }
}
