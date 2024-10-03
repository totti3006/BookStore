using BookStore.Application.Interfaces.Repositories;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Context;

namespace BookStore.Infrastructure.Repositories
{
    public class BookGenreRepository : Repository<BookGenre>, IBookGenreRepository
    {
        public BookGenreRepository(AppDbContext context) : base(context)
        {
        }
    }
}
