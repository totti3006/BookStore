using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> FindAndPaging(Expression<Func<Review, bool>> predicate, 
                                                             int pageNumber, 
                                                             int pageSize)
        {
            var reviews = _dbSet.AsNoTracking()
                                .Where(predicate)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize);

            return await reviews.ToListAsync();
        }
    }
}
