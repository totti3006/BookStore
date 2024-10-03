using BookStore.Domain.Enums;
using BookStore.Domain.Exceptions;
using System.Diagnostics;

namespace BookStore.Domain.Entities
{
    public class User
    {
        #region Private Fields
        private Guid _id;
        private string? _name;
        private string? _email;
        private string? _passwordHash;
        private decimal _balance;
        private Role _role;
        private DateTime _createdDate;
        private DateTime _lastModifiedDate;
        private ICollection<Otp>? _resetPasswordOtps;
        private ICollection<Order>? _orders;
        private ICollection<Review>? _reviews;
        private ICollection<Cart>? _carts;
        private ICollection<FavoriteBook>? _favouriteBooks;
        #endregion

        #region
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
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new UserException($"{nameof(Name)} must not be null or empty");
                }
                _name = value;
            }
        }
        public string? Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new UserException($"{nameof(Email)} must not be null or empty");
                }
                _email = value;
            }
        }
        public string? PasswordHash
        {
            get
            {
                return _passwordHash;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new UserException($"{nameof(PasswordHash)} must not be null or empty");
                }
                _passwordHash = value;
            }
        }
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if (value < 0)
                {
                    throw new UserException($"{nameof(Balance)} must be >= 0");
                }
                _balance = value;
            }
        }
        public Role Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
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
                    throw new UserException($"{nameof(LastModifiedDate)} must not be greater than now");
                }
                _lastModifiedDate = value;
            }
        }
        public ICollection<Otp>? ResetPasswordOtps
        {
            get
            {
                return _resetPasswordOtps;
            }
            set
            {
                _resetPasswordOtps = value;
            }
        }
        public ICollection<Order>? Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
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
    }
}
