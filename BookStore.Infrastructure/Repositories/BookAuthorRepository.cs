using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;

namespace BookStore.Infrastructure.Repositories
{
    public class BookAuthorRepository : Repository<BookAuthor>, IBookAuthorRepository
    {
        public BookAuthorRepository(AppDbContext context) : base(context)
        {
        }
    }
}
