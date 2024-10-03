using BookStore.Application.Models.Book;
using BookStore.Application.Models.Review;

namespace BookStore.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task CreateReview(CreateReviewDto createReviewDto);
        Task<IEnumerable<ReviewDto>> GetPagingReview(Guid bookId, int pageNumber, int pageSize);
    }
}
