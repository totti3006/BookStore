using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Repositories
{
    public class FavoriteBookRepository : Repository<FavoriteBook>, IFavoriteBookRepository
    {
        public FavoriteBookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FavoriteBook>> FindAndPaging(Expression<Func<FavoriteBook, bool>> predicate, 
                                                     int pageNumber, 
                                                     int pageSize)
        {
            var favoriteBook = _dbSet.AsNoTracking()
                                     .Where(predicate)
                                     .Include(fb => fb.Book)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize);

            return await favoriteBook.ToListAsync();
        }
    }
}
