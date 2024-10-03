using BookStore.Application.Models.Book;

namespace BookStore.Application.Models.Order
{
    public class CreateOrderRequest
    {
        public ICollection<BookDto>? Books { get; set; }
    }
}
