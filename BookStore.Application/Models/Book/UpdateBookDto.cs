namespace BookStore.Application.Models.Book
{
    public class UpdateBookDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public short PublishYear { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Guid>? Authors { get; set; }
        public ICollection<Guid>? Genres { get; set; }
    }
}
