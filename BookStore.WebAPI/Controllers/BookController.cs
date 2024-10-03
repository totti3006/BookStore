using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebAPI.Controllers
{
    public class BookController : AppControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<AuthController> logger, 
                              IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBook([FromQuery] BookParameters bookParameters)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var books = await _bookService.GetBook(bookParameters);

            return Ok(books);
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBook([FromQuery] int page = 1, 
                                                                      [FromQuery] int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var books = await _bookService.GetPaging(page, pageSize);

            return Ok(books);
        }

        [HttpGet("get-detail/{bookId}")]
        public async Task<ActionResult<BookDetailDto>> GetBookDetail()
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            string? bookId = Convert.ToString(Request.RouteValues["bookId"]);

            var book = await _bookService.GetBook(Guid.Parse(bookId));

            return Ok(book);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBook([FromForm] CreateBookDto book)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            await _bookService.AddNewBook(book);

            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook([FromQuery] string bookId)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            if (!Guid.TryParse(bookId, out Guid _bookId))
            {
                return BadRequest("Invalid book Id");
            }

            await _bookService.DeleteBook(_bookId);

            return Ok();
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBook(UpdateBookDto book)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            await _bookService.UpdateBook(book);

            return Ok();
        }

        [HttpPost("mark-favorite")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> AddToFavorite(FavoriteRequest favoriteRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var favorite = new FavoriteDto
            {
                BookId = favoriteRequest.BookId,
                UserId = GetUserId()
            };

            await _bookService.AddToFavorite(favorite);

            return Ok();
        }

        [HttpPost("unmark-favorite")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> RemoveFromFavorite(FavoriteRequest favoriteRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var favorite = new FavoriteDto
            {
                BookId = favoriteRequest.BookId,
                UserId = GetUserId()
            };

            await _bookService.DeleteFromFavorite(favorite);

            return Ok();
        }

        [HttpGet("get-favorite")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult<BookDto>> GetUserFavorite([FromQuery] int page = 1,
                                                                 [FromQuery] int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            Guid userId = GetUserId();

            var books = await _bookService.GetPagingFavorite(userId, page, pageSize);

            return Ok(books);
        }
    }
}
