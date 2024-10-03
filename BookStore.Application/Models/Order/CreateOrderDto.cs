using BookStore.Application.Models.Book;

namespace BookStore.Application.Models.Order
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }
        public ICollection<BookDto>? Books { get; set; }
    }
}
