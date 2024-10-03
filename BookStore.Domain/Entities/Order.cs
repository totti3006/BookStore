using BookStore.Domain.Exceptions;

namespace BookStore.Domain.Entities
{
    public class Order
    {
        #region Private Fields
        private Guid _id;
        private Guid _userId;
        private User? _user;
        private ICollection<OrderBook>? _orderBooks;
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
                    throw new OrderException($"{nameof(CreatedDate)} must not be greater than now");
                }
                _createdDate = value;
            }
        }
        #endregion
    }
}
