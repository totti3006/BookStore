using BookStore.Domain.Exceptions;

namespace BookStore.Domain.Entities
{
    public class Otp
    {
        #region Private Field
        private Guid _id;
        private string? _code;
        private DateTime _createdDate;
        private DateTime _expiredDate;
        private bool _isVerified = false;
        private Guid _userId;
        private User? _user;
        #endregion

        #region Public Field
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
        public string? Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 6)
                {
                    throw new OtpException("Otp code must contain 6 digit characters");
                }
                _code = value;
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
        public DateTime ExpiredDate
        {
            get
            {
                return _expiredDate;
            }
            set
            {
                if (value <= DateTime.Now)
                {
                    throw new OtpException("Expired date is invalid");
                }
                _expiredDate = value;
            }
        }
        public bool IsVerified
        {
            get
            {
                return _isVerified;
            }
            set
            {
                _isVerified = value;
            }
        }
        public Guid UserId
        {
            get
            {
                return _userId;
            }
            init
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
        #endregion
    }
}
