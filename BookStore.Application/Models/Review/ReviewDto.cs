using BookStore.Domain.Enums;

namespace BookStore.Application.Models.Review
{
    public class ReviewDto
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Rating Rating { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
