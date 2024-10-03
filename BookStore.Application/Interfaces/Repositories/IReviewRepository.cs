using BookStore.Domain.Entities;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> FindAndPaging(Expression<Func<Review, bool>> predicate, int pageNumber, int pageSize);
    }
}
