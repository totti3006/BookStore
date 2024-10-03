using BookStore.Application.Models.Author;
using BookStore.Application.Models.Genre;

namespace BookStore.Application.Models.Book
{
    public class BookDetailDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public short PublishYear { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<AuthorDto>? Authors { get; set; }
        public ICollection<GenreDto>? Genres { get; set; }
    }
}
