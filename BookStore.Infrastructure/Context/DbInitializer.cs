using BookStore.Domain.Entities;
using BookStore.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Context
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private readonly string _jsonPath;

        public DbInitializer(AppDbContext context)
        {
            _context = context;
            _jsonPath = Directory.GetCurrentDirectory() + "\\MockData";
        }

        public async Task Initialize()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            await SeedUsers();
            await SeedAuthors();
            await SeedGenres();
            await SeedBooks();
            await SeedBookAuthors();
            await SeedBookGenres();
        }

        public async Task SeedUsers()
        {
            try
            {
                if (await _context.Users.AnyAsync())
                {
                    return;
                }

                // password: kedat123
                // iso 8601 datetime standard has been converted into utc

                List<User> users = await JsonFileReader.ReadAsync<User>($"{_jsonPath}/users.json");

                await _context.Users.AddRangeAsync(users);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SeedAuthors()
        {
            try
            {
                if (await _context.Authors.AnyAsync())
                {
                    return;
                }

                List<Author> authors = await JsonFileReader.ReadAsync<Author>($"{_jsonPath}/authors.json");

                await _context.Authors.AddRangeAsync(authors);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SeedGenres()
        {
            try
            {
                if (await _context.Genres.AnyAsync())
                {
                    return;
                }

                List<Genre> genres = await JsonFileReader.ReadAsync<Genre>($"{_jsonPath}/genres.json");

                await _context.Genres.AddRangeAsync(genres);

                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task SeedBooks()
        {
            try
            {
                if (await _context.Books.AnyAsync())
                {
                    return;
                }

                List<Book> books = await JsonFileReader.ReadAsync<Book>($"{_jsonPath}/books.json");

                await _context.Books.AddRangeAsync(books);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SeedBookAuthors()
        {
            try
            {
                if (await _context.BookAuthors.AnyAsync())
                {
                    return;
                }

                List<BookAuthor> bookAuthors = await JsonFileReader.ReadAsync<BookAuthor>($"{_jsonPath}/bookAuthor.json");

                await _context.BookAuthors.AddRangeAsync(bookAuthors);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SeedBookGenres()
        {
            try
            {
                if (await _context.BookGenres.AnyAsync())
                {
                    return;
                }

                List<BookGenre> bookGenres = await JsonFileReader.ReadAsync<BookGenre>($"{_jsonPath}/bookGenre.json");

                await _context.BookGenres.AddRangeAsync(bookGenres);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
