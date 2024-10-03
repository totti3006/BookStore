using BookStore.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Models.Review
{
    public class CreateReviewDto
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Rating Rating { get; set; }
        public IFormFile? File { get; set; }
        public Guid BookId { get; set; }
    }
}
