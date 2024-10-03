using BookStore.Domain.Entities;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> CountNewCreatedAccountByDate();
    }
}
