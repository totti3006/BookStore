using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cart>> GetCartWithBook(Guid userId)
        {
            var cart = _dbSet.AsNoTracking()
                             .Where(c => c.UserId == userId)
                             .Include(c => c.Book);

            return await cart.ToListAsync();
        }
    }
}
