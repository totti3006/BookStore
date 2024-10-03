namespace BookStore.Application.Models.Book
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
