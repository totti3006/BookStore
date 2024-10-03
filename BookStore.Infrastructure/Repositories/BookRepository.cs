using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> FindAndPaging(Expression<Func<Book, bool>> predicate, 
                                                           int pageNumber, 
                                                           int pageSize)
        {
            var books = _dbSet.Where(predicate)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking();

            return await books.ToListAsync();
        }

        public async Task<Book?> GetBookWithAuthorAndGenre(Guid id)
        {
            var book = await _dbSet.AsNoTracking()
                             .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                             .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                             .SingleOrDefaultAsync(b => b.Id == id);

            return book;
        }

        public Task<int> GetCountByAuthor(string author)
        {
            var books = _dbSet.Where(b => b.BookAuthors.Any(ba => ba.Author.Name
                                                                           .ToLower()
                                                                           .Contains(author.ToLower())))
                              .AsNoTracking();

            return books.CountAsync();
        }

        public Task<int> GetCountByGenre(string genre)
        {
            var books = _dbSet.Where(b => b.BookGenres.Any(ba => ba.Genre.Name
                                                                         .ToLower()
                                                                         .Contains(genre.ToLower())))
                              .AsNoTracking();

            return books.CountAsync();
        }

        public Task<int> GetCountByPublishYear(short publishYear)
        {
            var books = _dbSet.Where(b => b.PublishYear == publishYear)
                              .AsNoTracking();

            return books.CountAsync();
        }

        public Task<int> GetCountByTitle(string title)
        {
            var books = _dbSet.Where(b => b.Title.ToLower().Contains(title.ToLower()))
                              .AsNoTracking();
                              
            return books.CountAsync();
        }

        public async Task<IEnumerable<Book>> GetPagingByAuthor(string name, int pageNumber, int pageSize)
        {
            var books = _dbSet.Where(b => b.BookAuthors.Any(ba => ba.Author.Name
                                                                           .ToLower()
                                                                           .Contains(name.ToLower())))
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking();

            return await books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetPagingByGenre(string genre, int pageNumber, int pageSize)
        {
            var books = _dbSet.Where(b => b.BookGenres.Any(ba => ba.Genre.Name
                                                                           .ToLower()
                                                                           .Contains(genre.ToLower())))
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking();

            return await books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetPagingByPublishYear(short publishYear, int pageNumber, int pageSize)
        {
            var books = _dbSet.Where(b => b.PublishYear == publishYear)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking();

            return await books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetPagingByTitle(string title, int pageNumber, int pageSize)
        {
            var books = _dbSet.Where(b => b.Title.ToLower().Contains(title.ToLower()))
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking();

            return await books.ToListAsync();
        }
    }
}
