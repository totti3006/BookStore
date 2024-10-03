using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountNewCreatedAccountByDate()
        {
            var users = _dbSet.AsNoTracking()
                              .Where(u => u.CreatedDate >= DateTime.Today);

            return await users.CountAsync();
        }
    }
}
