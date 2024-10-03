namespace BookStore.Application.Models.Cart
{
    public class CartDetailDto
    {
        public ICollection<CartItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
