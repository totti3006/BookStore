using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;

namespace BookStore.Infrastructure.Repositories
{
    public class OrderBookRepository : Repository<OrderBook>, IOrderBookRepository
    {
        public OrderBookRepository(AppDbContext context) : base(context)
        {
        }
    }
}
