using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebAPI.Controllers
{
    public class ReviewController : AppControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(ILogger<ReviewController> logger, 
                                IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> CreateNewReview([FromForm] CreateReviewRequest createReviewRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var reviewDto = new CreateReviewDto
            {
                UserId = GetUserId(),
                Title = createReviewRequest.Title,
                Description = createReviewRequest.Description,
                Rating = createReviewRequest.Rating,
                File = createReviewRequest.File,
                BookId = createReviewRequest.BookId,
            };

            await _reviewService.CreateReview(reviewDto);

            return Ok();
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewPagingByBook([FromQuery] string bookId, 
                                                                                      [FromQuery] int page = 1,
                                                                                      [FromQuery] int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            if (!Guid.TryParse(bookId, out Guid _bookId))
            {
                return BadRequest("Invalid book Id");
            }

            var reviews = await _reviewService.GetPagingReview(_bookId, page, pageSize);
            
            return Ok(reviews);
        }
    }
}
