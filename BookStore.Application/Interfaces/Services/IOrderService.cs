using BookStore.Application.Models.Order;

namespace BookStore.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task CreateOrder(CreateOrderDto createOrder);
        Task<IEnumerable<OrderDetailDto>> GetOrdersPaging(Guid userId, int pageNumber, int pageSize);
    }
}
