using DropMate.Application.ServiceContracts;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/reviews")]
    public class ReviewsV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public ReviewsV1Controller(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [HttpHead]
        [ResponseCache(CacheProfileName = "20 minutes cache")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllReviews([FromQuery]ReviewRequestParameters requestParameters)
        {
            StandardResponse<(IEnumerable<ReviewResponseDto> reviews,MetaData metaData)> result = await _services.ReviewService
                .GetAllReviewsAsync(requestParameters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<ReviewResponseDto>>.Success("Retrieved successfully", result.Data.reviews, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            StandardResponse<ReviewResponseDto> review = await _services.ReviewService.GetReviewByIdAsync(id, false);
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] ReviewRequestDto requestDto)
        {
            StandardResponse<ReviewResponseDto> review = await _services.ReviewService.CreateReview(requestDto);
            return CreatedAtAction(nameof(GetReviewById), new { Id = review.Data.Id }, review.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _services.ReviewService.DeleteReview(id);
            return Ok(StandardResponse<string>.Success("Package deleted successfully", null, 200));
        }
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
            return Ok();
        }
    }
    }
