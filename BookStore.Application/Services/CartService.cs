using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Cart;
using BookStore.Domain.Entities;

namespace BookStore.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddToCart(CartDto cartDto)
        {
            try
            {
                Cart cart = new()
                {
                    Id = Guid.NewGuid(),
                    BookId = cartDto.BookId,
                    UserId = cartDto.UserId,
                    CreatedDate = DateTime.Now,
                };

                await _unitOfWork.CartRepository.Add(cart);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CartDetailDto> GetAll(Guid userId)
        {
            try
            {
                var carts = await _unitOfWork.CartRepository.GetCartWithBook(userId);

                var cartDetail = new CartDetailDto
                {
                    Items = carts.Select(c => new CartItemDto
                    {
                        CartId = c.Id,
                        BookId = c.BookId,
                        BookTitle = c.Book.Title,
                        Price = c.Book.Price,
                        ImageUrl = c.Book.ImageUrl
                    }).ToList(),
                    TotalPrice = carts.Sum(c => c.Book.Price),
                    Quantity = carts.Count()
                };

                return cartDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveFromCart(CartItemRemoveDto cartItem)
        {
            try
            {
                Cart? cart = await _unitOfWork.CartRepository.SingleOrDefault(c => c.Id == cartItem.CartId);

                if (cart is null)
                {
                    throw new BusinessException("Cannot find cart Id");
                }

                if (cart.UserId != cartItem.UserId)
                {
                    throw new BusinessException("Not the right user");
                }

                await _unitOfWork.CartRepository.Delete(cart);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
