using BookStore.Application.Models.Cart;

namespace BookStore.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDetailDto> GetAll(Guid userId);
        Task AddToCart(CartDto cartDto);
        Task RemoveFromCart(CartItemRemoveDto cartItem);
    }
}
