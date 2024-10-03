using BookStore.Domain.Entities;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IOtpRepository : IRepository<Otp>
    {
        Task<Otp?> GetNonExpiredLastestOtp(Guid userId);
    }
}
