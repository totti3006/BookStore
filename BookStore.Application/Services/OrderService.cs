using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Book;
using BookStore.Application.Models.Cart;
using BookStore.Application.Models.Email;
using BookStore.Application.Models.Order;
using BookStore.Domain.Entities;

namespace BookStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ICartService _cartService;

        public OrderService(IUnitOfWork unitOfWork, IEmailService emailService, ICartService cartService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _cartService = cartService;
        }

        public async Task CreateOrder(CreateOrderDto createOrder)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Id == createOrder.UserId);

                if (user is null)
                {
                    throw new BusinessException("User not existed");
                }

                var cart = await _cartService.GetAll(user.Id);

                if (!createOrder.Books.All(b => cart.Items.Any(c => c.BookId == b.Id)))
                {
                    throw new BusinessException("All order item should exist in cart");
                }

                Guid orderId = Guid.NewGuid();

                var orderBooks = createOrder.Books.Select(b => new OrderBook
                {
                    BookId = b.Id,
                    OrderId = orderId
                });

                Order order = new()
                {
                    Id = orderId,
                    CreatedDate = DateTime.Now,
                    UserId = createOrder.UserId,
                    OrderBooks = orderBooks.ToList(),
                };

                decimal totalPrice = createOrder.Books.Sum(b => b.Price);

                await DebitMoneyFromWallet(user, totalPrice);

                var invoice = new InvoiceEmailDto
                {
                    Email = user.Email,
                    Books = createOrder.Books,
                    CreatedDate = order.CreatedDate,
                    OrderId = orderId,
                    Name = user.Name,
                    Quantity = createOrder.Books.Count(),
                    TotalPrice = createOrder.Books.Sum(b => b.Price)
                };

                var cartIdList = cart.Items.Where(c => createOrder.Books.Any(b => b.Id == c.BookId))
                                           .Select(c => c.CartId);

                // all item in cart that exist in order should be removed
                foreach(Guid id in cartIdList)
                {
                    await _cartService.RemoveFromCart(new CartItemRemoveDto
                    {
                        UserId = user.Id,
                        CartId = id
                    });
                }

                await _emailService.SendEmailForSuccessOrder(invoice);

                await _unitOfWork.OrderRepository.Add(order);

                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderDetailDto>> GetOrdersPaging(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.FindAndPaging(o => o.UserId == userId,
                                                                             pageNumber, 
                                                                             pageSize);

                var orderDetails = orders.Select(o => new OrderDetailDto
                {
                    TotalPrice = o.OrderBooks.Sum(ob => ob.Book.Price),
                    Quantity = o.OrderBooks.Count(),
                    CreatedDate = o.CreatedDate,
                    Books = o.OrderBooks.Select(ob => new BookDto
                    {
                        Id = ob.BookId,
                        Title = ob.Book.Title,
                        ImageUrl = ob.Book.ImageUrl,
                        Price = ob.Book.Price
                    }).ToList(),
                });

                return orderDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task DebitMoneyFromWallet(User user, decimal total)
        {
            try
            {
                if (total > user.Balance)
                {
                    throw new BusinessException("User balance is not enough");
                }

                user.Balance -= total;

                await _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
