using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Get(Guid id);
        Task<IEnumerable<T>> GetPaging(int pageNumber, int pageSize);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T?> SingleOrDefault(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> GetCount();
        Task<bool> Any(Expression<Func<T, bool>> predicate);
    }
}
