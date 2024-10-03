using BookStore.Domain.Entities;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<IEnumerable<Cart>> GetCartWithBook(Guid userId);
    }
}
