using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Author;
using BookStore.Application.Models.Book;
using BookStore.Application.Models.Genre;
using BookStore.Application.Utils;
using BookStore.Domain.Entities;

namespace BookStore.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddNewBook(CreateBookDto bookDto)
        {
            try
            {
                Guid bookId = Guid.NewGuid();

                var bookAuthors = bookDto.Authors?.Select(id => new BookAuthor
                {
                    AuthorId = id,
                    BookId = bookId
                }).ToList();

                var bookGenres = bookDto.Genres?.Select(id => new BookGenre
                {
                    GenreId = id,
                    BookId = bookId,
                }).ToList();

                string? extension = Path.GetExtension(bookDto.File?.FileName);
                string imageFileName = Guid.NewGuid().ToString() + extension;

                Book book = new Book
                {
                    Id = bookId,
                    Title = bookDto.Title,
                    Description = bookDto.Description,
                    Price = bookDto.Price,
                    ImageUrl = imageFileName,
                    PublishYear = bookDto.PublishYear,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    BookAuthors = bookAuthors,
                    BookGenres = bookGenres
                };

                await _unitOfWork.BookRepository.Add(book);
                await _unitOfWork.SaveChanges();

                await FileHelper.UploadFile(bookDto.File, @"\wwwroot\Books", imageFileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddToFavorite(FavoriteDto favoriteDto)
        {
            try
            {
                var favorite = new FavoriteBook
                {
                    Id = Guid.NewGuid(),
                    BookId = favoriteDto.BookId,
                    UserId = favoriteDto.UserId,
                    CreatedDate = DateTime.Now
                };

                await _unitOfWork.FavoriteBookRepository.Add(favorite);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBook(Guid id)
        {
            try
            {
                Book? book = await _unitOfWork.BookRepository.SingleOrDefault(b => b.Id == id);

                if (book is null)
                {
                    throw new BusinessException("Cannot find book Id");
                }

                await _unitOfWork.BookRepository.Delete(book);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BookDetailDto?> GetBook(Guid id)
        {
            try
            {
                Book? book = await _unitOfWork.BookRepository.GetBookWithAuthorAndGenre(id);

                if (book is null)
                {
                    throw new BusinessException("Cannot find book Id");
                }

                var authors = book.BookAuthors.Select(ba => new AuthorDto
                {
                    Id = ba.AuthorId,
                    Name = ba.Author?.Name
                }).ToList();

                var genres = book.BookGenres.Select(bg => new GenreDto
                {
                    Id = bg.GenreId,
                    Name = bg.Genre?.Name
                }).ToList();

                var bookDetailDto = new BookDetailDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Price = book.Price,
                    PublishYear = book.PublishYear,
                    ImageUrl = book.ImageUrl != null ? Path.Combine(Directory.GetCurrentDirectory(),
                                                                    "wwwroot",
                                                                    "Books",
                                                                    book.ImageUrl) : null,
                    Authors = authors,
                    Genres = genres,
                };

                return bookDetailDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BookDto>> GetBook(BookParameters bookParameters)
        {
            try
            {
                var books = await _unitOfWork.BookRepository.FindAndPaging(b => 
                        (bookParameters.Title != null ? b.Title.ToLower().Contains(bookParameters.Title.ToLower()) : true) &&
                        (bookParameters.Author != null ? b.BookAuthors.Any(ba => ba.Author.Name.ToLower().Contains(bookParameters.Author.ToLower())) : true) &&
                        (bookParameters.Genre != null ? b.BookGenres.Any(bg => bg.Genre.Name.ToLower().Contains(bookParameters.Genre.ToLower())) : true) &&
                        (bookParameters.PublishYear > 0 ? b.PublishYear == bookParameters.PublishYear : true),
                    bookParameters.Page,
                    bookParameters.PageSize);

                return books.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl != null ? Path.Combine(Directory.GetCurrentDirectory(), 
                                                                 "wwwroot", 
                                                                 "Books", 
                                                                 b.ImageUrl) : null,
                    Price = b.Price,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetCount()
        {
            try
            {
                return await _unitOfWork.BookRepository.GetCount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BookDto>> GetPagingFavorite(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                var favorites = await _unitOfWork.FavoriteBookRepository.FindAndPaging(fb => fb.UserId == userId,
                                                                                       pageNumber, 
                                                                                       pageSize);
                var books = favorites.Select(f => new BookDto
                {
                    Id = f.BookId,
                    Title = f.Book.Title,
                    ImageUrl = f.Book.ImageUrl != null ? Path.Combine(Directory.GetCurrentDirectory(),
                                                                     "wwwroot",
                                                                     "Books",
                                                                     f.Book.ImageUrl) : null,
                    Price = f.Book.Price,
                });

                return books;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BookDto>> GetPaging(int pageNumber, int pageSize)
        {
            try
            {
                var books = await _unitOfWork.BookRepository.GetPaging(pageNumber, pageSize);

                return books.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    ImageUrl = b.ImageUrl != null ? Path.Combine(Directory.GetCurrentDirectory(),
                                                                 "wwwroot",
                                                                 "Books",
                                                                 b.ImageUrl) : null,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateBook(UpdateBookDto bookDto)
        {
            try
            {
                Book? book = await _unitOfWork.BookRepository.SingleOrDefault(b => b.Id == bookDto.Id);

                if (book is null)
                {
                    throw new BusinessException("Cannot find book Id");
                }

                book.Title = bookDto.Title;
                book.Description = bookDto.Description;
                book.Price = bookDto.Price;
                book.ImageUrl = bookDto.ImageUrl;
                book.PublishYear = bookDto.PublishYear;
                book.BookAuthors = bookDto.Authors.Select(id => new BookAuthor 
                { 
                    AuthorId = id, 
                    BookId = bookDto.Id 
                }).ToList();
                book.BookGenres = bookDto.Genres.Select(id => new BookGenre
                {
                    GenreId = id,
                    BookId = bookDto.Id
                }).ToList();

                await _unitOfWork.BookRepository.Update(book);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFromFavorite(FavoriteDto favoriteDto)
        {
            try
            {
                FavoriteBook? favorite = await _unitOfWork.FavoriteBookRepository.SingleOrDefault(
                    fb => fb.BookId == favoriteDto.BookId &&
                          fb.UserId == favoriteDto.UserId
                );

                if (favorite is null)
                {
                    throw new BusinessException("Cannot find FavoriteBook Id");
                }

                await _unitOfWork.FavoriteBookRepository.Delete(favorite);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
