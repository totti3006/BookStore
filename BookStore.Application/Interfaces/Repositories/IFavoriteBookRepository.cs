using BookStore.Domain.Entities;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IFavoriteBookRepository : IRepository<FavoriteBook>
    {
        Task<IEnumerable<FavoriteBook>> FindAndPaging(Expression<Func<FavoriteBook, bool>> predicate, 
                                                      int pageNumber, 
                                                      int pageSize);
    }
}
