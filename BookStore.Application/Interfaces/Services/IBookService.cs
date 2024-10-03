using BookStore.Application.Models.Book;

namespace BookStore.Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBook(BookParameters bookParameters);
        Task<IEnumerable<BookDto>> GetPaging(int pageNumber, int pageSize);
        Task<int> GetCount();
        Task<BookDetailDto?> GetBook(Guid id);
        Task UpdateBook(UpdateBookDto bookDto);
        Task DeleteBook(Guid id);
        Task AddNewBook(CreateBookDto bookDto);
        Task AddToFavorite(FavoriteDto favoriteDto);
        Task DeleteFromFavorite(FavoriteDto favoriteDto);
        Task<IEnumerable<BookDto>> GetPagingFavorite(Guid userId, int pageNumber, int pageSize);
    }
}
