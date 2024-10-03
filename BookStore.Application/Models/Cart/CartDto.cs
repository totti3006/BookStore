namespace BookStore.Application.Models.Cart
{
    public class CartDto
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
