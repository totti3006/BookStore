using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories
{
    public class OtpRepository : Repository<Otp>, IOtpRepository
    {
        public OtpRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Otp?> GetNonExpiredLastestOtp(Guid userId)
        {
            Otp? item = await _dbSet.AsNoTracking()
                                    .Where(o => o.UserId == userId && 
                                                o.ExpiredDate < DateTime.Now)
                                    .OrderByDescending(o => o.CreatedDate)
                                    .FirstOrDefaultAsync();

            return item;
        }
    }
}
