using BookStore.Application.Models.Book;

namespace BookStore.Application.Models.Email
{
    public class InvoiceEmailDto
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public Guid OrderId { get; set; }
        public ICollection<BookDto>? Books { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
