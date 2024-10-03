using BookStore.Domain.Entities;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> FindAndPaging(Expression<Func<Book, bool>> predicate, int pageNumber, int pageSize);
        Task<IEnumerable<Book>> GetPagingByTitle(string title, int pageNumber, int pageSize);
        Task<IEnumerable<Book>> GetPagingByAuthor(string name, int pageNumber, int pageSize);
        Task<IEnumerable<Book>> GetPagingByGenre(string genre, int pageNumber, int pageSize);
        Task<IEnumerable<Book>> GetPagingByPublishYear(short publishYear, int pageNumber, int pageSize);
        Task<int> GetCountByTitle(string title);
        Task<int> GetCountByAuthor(string author);
        Task<int> GetCountByGenre(string genre);
        Task<int> GetCountByPublishYear(short publishYear);
        Task<Book?> GetBookWithAuthorAndGenre(Guid id);
    }
}
