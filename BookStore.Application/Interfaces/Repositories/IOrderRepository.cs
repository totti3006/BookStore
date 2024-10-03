using BookStore.Domain.Entities;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> FindAndPaging(Expression<Func<Order, bool>> predicate, int pageNumber, int pageSize);
        Task<int> CountOrderByDate();
        Task<decimal> TotalIncomeByDate();
    }
}
