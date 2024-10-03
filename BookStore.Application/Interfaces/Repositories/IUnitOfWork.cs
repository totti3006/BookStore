namespace BookStore.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
        IAuthorRepository AuthorRepository { get; }
        IBookAuthorRepository BookAuthorRepository { get; }
        IBookGenreRepository BookGenreRepository { get; }
        IBookRepository BookRepository { get; }
        ICartRepository CartRepository { get; }
        IFavoriteBookRepository FavoriteBookRepository { get; }
        IGenreRepository GenreRepository { get; }
        IOrderBookRepository OrderBookRepository { get; }
        IOrderRepository OrderRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IUserRepository UserRepository { get; }
        IOtpRepository OtpRepository { get; }
    }
}
