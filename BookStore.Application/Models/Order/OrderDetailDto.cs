using BookStore.Application.Models.Book;

namespace BookStore.Application.Models.Order
{
    public class OrderDetailDto
    {
        public ICollection<BookDto>? Books { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
