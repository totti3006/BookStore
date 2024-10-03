using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Book;
using BookStore.Application.Models.Review;
using BookStore.Application.Utils;
using BookStore.Domain.Entities;

namespace BookStore.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateReview(CreateReviewDto createReviewDto)
        {
            try
            {
                string? extension = Path.GetExtension(createReviewDto.File?.FileName);
                string imageFileName = Guid.NewGuid().ToString() + extension;

                var review = new Review
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Title = createReviewDto.Title,
                    Description = createReviewDto.Description,
                    Rating = createReviewDto.Rating,
                    ImageUrl = imageFileName,
                    BookId = createReviewDto.BookId,
                    UserId = createReviewDto.UserId,
                };

                await _unitOfWork.ReviewRepository.Add(review);
                await _unitOfWork.SaveChanges();

                await FileHelper.UploadFile(createReviewDto.File, @"\wwwroot\Reviews", imageFileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ReviewDto>> GetPagingReview(Guid bookId, int pageNumber, int pageSize)
        {
            try
            {
                var reviews = await _unitOfWork.ReviewRepository.FindAndPaging(r => r.BookId == bookId, 
                                                                               pageNumber, 
                                                                               pageSize);

                var reviewDtos = reviews.Select(r => new ReviewDto
                {
                    Title = r.Title,
                    Description = r.Description,
                    Rating = r.Rating,
                    ImageUrl = r.ImageUrl != null ? Path.Combine(Directory.GetCurrentDirectory(), 
                                                                 "wwwroot", 
                                                                 "Reviews", 
                                                                 r.ImageUrl) : null,
                    CreatedDate = r.CreatedDate,
                    UserId = r.UserId,
                });


                return reviewDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
