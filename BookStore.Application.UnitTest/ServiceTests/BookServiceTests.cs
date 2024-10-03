using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Book;
using BookStore.Application.Services;
using System.Linq.Expressions;

namespace BookStore.Application.UnitTest.ServiceTests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IBookService _bookService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _bookService = new BookService(_unitOfWork.Object);
        }

        [Test]
        public async Task AddNewBook_ValidBook_NewBookAdded()
        {
            var bookDto = new CreateBookDto
            {
                Title = "Harry Potter"
            };

            _unitOfWork.Setup(u => u.BookRepository.Add(It.IsAny<Book>()));

            await _bookService.AddNewBook(bookDto);

            _unitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public async Task DeleteBook_ExistingBook_BookIsDeleted()
        {
            Guid bookId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
            };

            _unitOfWork.Setup(u => u.BookRepository.SingleOrDefault(It.IsAny<Expression<Func<Book, bool>>>()))
                       .ReturnsAsync(book);

            _unitOfWork.Setup(u => u.BookRepository.Delete(It.Is<Book>(b => b.Id == bookId)));

            await _bookService.DeleteBook(bookId);

            _unitOfWork.Verify(u => u.BookRepository.Delete(book), Times.Once);
            _unitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteBook_NonExistingBook_ThrowException()
        {
            _unitOfWork.Setup(u => u.BookRepository.SingleOrDefault(It.IsAny<Expression<Func<Book, bool>>>()))
                       .ReturnsAsync((Book?)null);

            _unitOfWork.Verify(u => u.SaveChanges(), Times.Never);

            Assert.That(async () => await _bookService.DeleteBook(Guid.NewGuid()),
                    Throws.Exception.TypeOf<BusinessException>());

        }
    }
}
