using BookStore.Domain.Exceptions;

namespace BookStore.Domain.Entities
{
    public class FavoriteBook
    {
        #region Private Fields
        private Guid _id;
        private Guid _userId;
        private User? _user;
        private Guid _bookId;
        private Book? _book;
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
        #endregion
    }
}
