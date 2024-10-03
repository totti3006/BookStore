using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Models.Book
{
    public class CreateBookDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public short PublishYear { get; set; }
        public IFormFile? File { get; set; }
        public ICollection<Guid>? Authors { get; set; }
        public ICollection<Guid>? Genres { get; set; }
    }
}
