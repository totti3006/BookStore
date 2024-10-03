using BookStore.Domain.Enums;
using BookStore.Domain.Exceptions;
using System.Text;

namespace BookStore.Domain.Entities
{
    public class Review
    {
        #region Private Fields
        private Guid _id;
        private string? _title;
        private string? _description;
        private Rating _rating;
        private string? _imageUrl;
        private Guid _bookId;
        private Book? _book;
        private Guid _userId;
        private User? _user;
        private DateTime _createdDate;
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
                    throw new ReviewException($"{nameof(Title)} must not be null or empty");
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
        public Rating Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
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
        public Guid BookId
        {
            get
            {
                return _bookId;
            }
            set
            {
                _bookId = value;
            }
        }
        public Book? Book
        {
            get
            {
                return _book;
            }
            set
            {
                _book = value;
            }
        }
        public Guid UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }
        public User? User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
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
                _createdDate = value;
            }
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Tiêu đề: {Title}")
              .AppendLine($"Ngày tạo: {CreatedDate.ToString("dd/MM/yyyy HH/mm/ss")}")
              .AppendLine($"Xếp hạng: {(int)Rating}/5")
              .AppendLine($"Mô tả: {Description?.Substring(0, 50)}");

            return sb.ToString();
        }
    }
}
