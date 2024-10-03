using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountOrderByDate()
        {
            var orders = _dbSet.AsNoTracking()
                               .Where(o => o.CreatedDate >= DateTime.Today);

            return await orders.CountAsync();
        }

        public async Task<IEnumerable<Order>> FindAndPaging(Expression<Func<Order, bool>> predicate, 
                                                      int pageNumber, 
                                                      int pageSize)
        {
            var orders = _dbSet.AsNoTracking()
                               .Where(predicate)
                               .Include(o => o.OrderBooks)
                               .ThenInclude(ob => ob.Book)
                               .Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize);

            return await orders.ToListAsync();
        }

        public async Task<decimal> TotalIncomeByDate()
        {
            var orders = await _dbSet.AsNoTracking()
                                   .Where(o => o.CreatedDate >= DateTime.Today)
                                   .Include(o => o.OrderBooks)
                                   .ThenInclude(ob => ob.Book)
                                   .ToListAsync();

            decimal total = orders.Sum(o => o.OrderBooks.Sum(ob => ob.Book.Price));

            return total;
        }
    }
}
