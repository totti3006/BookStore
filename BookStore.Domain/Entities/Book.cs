using BookStore.Domain.Exceptions;
using System.Globalization;
using System.Text;

namespace BookStore.Domain.Entities
{
    public class Book
    {
        #region Private Fields
        private Guid _id;
        private string? _title;
        private string? _description;
        private decimal _price;
        private short _publishYear;
        private string? _imageUrl;
        private DateTime _createdDate;
        private DateTime _lastModifiedDate;
        private ICollection<BookAuthor>? _bookAuthors;
        private ICollection<BookGenre>? _bookGenres;
        private ICollection<Review>? _reviews;
        private ICollection<OrderBook>? _orderBooks;
        private ICollection<Cart>? _carts;
        private ICollection<FavoriteBook>? _favouriteBooks;
        #endregion

        #region Public Fields
        public Guid Id
        {
            get
            {
                return _id;
            }
            init
            {
                _id = value;
            }
        }
        public string? Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new BookException($"{nameof(Title)} must not be null or empty");
                }
                _title = value;
            }
        }
        public string? Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value < 0)
                {
                    throw new BookException($"{nameof(Price)} must be >= 0");
                }
                _price = value;
            }
        }
        public short PublishYear
        {
            get
            {
                return _publishYear;
            }
            set
            {
                if (value < 0)
                {
                    throw new BookException($"{nameof(PublishYear)} must be >= 0");
                }
                _publishYear = value;
            }
        }
        public string? ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
            }
        }
        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                if (value > DateTime.Now.ToLocalTime())
                {
                    throw new BookException($"{nameof(CreatedDate)} must not be greater than now");
                }
                _createdDate = value;
            }
        }
        public DateTime LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
            set
            {
                if (value > DateTime.Now.ToLocalTime())
                {
                    throw new BookException($"{nameof(LastModifiedDate)} must not be greater than now");
                }
                _lastModifiedDate = value;
            }
        }
        public ICollection<BookAuthor>? BookAuthors
        {
            get
            {
                return _bookAuthors;
            }
            set
            {
                _bookAuthors = value;
            }
        }
        public ICollection<BookGenre>? BookGenres
        {
            get
            {
                return _bookGenres;
            }
            set
            {
                _bookGenres = value;
            }
        }
        public ICollection<Review>? Reviews
        {
            get
            {
                return _reviews;
            }
            set
            {
                _reviews = value;
            }
        }
        public ICollection<OrderBook>? OrderBooks
        {
            get
            {
                return _orderBooks;
            }
            set
            {
                _orderBooks = value;
            }
        }
        public ICollection<Cart>? Carts
        {
            get
            {
                return _carts;
            }
            set
            {
                _carts = value;
            }
        }
        public ICollection<FavoriteBook>? FavouriteBooks
        {
            get
            {
                return _favouriteBooks;
            }
            set
            {
                _favouriteBooks = value;
            }
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}")
              .AppendLine($"Tên sách: {Title}")
              .AppendLine($"Giá: {Price.ToString("C", new CultureInfo("en-US"))}")
              .AppendLine($"Năm xuất bản: {PublishYear}")
              .AppendLine($"Mô tả: {Description?.Substring(0, 50)}")
              .AppendLine($"Chỉnh sửa lần cuối: {LastModifiedDate.ToString("dd/MM/yyyy")}");

            return sb.ToString();
        }
    }
}
