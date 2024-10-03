namespace BookStore.Application.Models.Cart
{
    public class CartItemRemoveDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
    }
}
