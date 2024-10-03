using BookStore.Application.Interfaces.Repositories;
using BookStore.Infrastructure.Context;

namespace BookStore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            AuthorRepository = new AuthorRepository(_appDbContext);
            BookAuthorRepository = new BookAuthorRepository(_appDbContext);
            BookGenreRepository = new BookGenreRepository(_appDbContext);
            BookRepository = new BookRepository(_appDbContext);
            CartRepository = new CartRepository(_appDbContext);
            FavoriteBookRepository = new FavoriteBookRepository(_appDbContext);
            GenreRepository = new GenreRepository(_appDbContext);
            OrderBookRepository = new OrderBookRepository(_appDbContext);
            OrderRepository = new OrderRepository(_appDbContext);
            ReviewRepository = new ReviewRepository(_appDbContext);
            UserRepository = new UserRepository(_appDbContext);
            OtpRepository = new OtpRepository(_appDbContext);
        }

        public IAuthorRepository AuthorRepository { get; private set; }

        public IBookAuthorRepository BookAuthorRepository { get; private set; }

        public IBookGenreRepository BookGenreRepository { get; private set; }

        public IBookRepository BookRepository { get; private set; }

        public ICartRepository CartRepository { get; private set; }

        public IFavoriteBookRepository FavoriteBookRepository { get; private set; }

        public IGenreRepository GenreRepository { get; private set; }

        public IOrderBookRepository OrderBookRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public IReviewRepository ReviewRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }
        public IOtpRepository OtpRepository { get; private set; }

        public async Task SaveChanges()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
