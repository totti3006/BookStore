using BookStore.Application.Models.Book;

namespace BookStore.Application.Models.Cart
{
    public class CartItemDto
    {
        public Guid CartId { get; set; }
        public Guid BookId { get; set; }
        public string? BookTitle { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
