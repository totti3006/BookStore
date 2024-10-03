namespace BookStore.Domain.Entities
{
    public class OrderBook
    {
        #region Private Fields
        private Guid _bookId;
        private Book? _book;
        private Guid _orderId;
        private Order? _order;
        #endregion

        #region Public Fields
        public Guid BookId
        {
            get
            {
                return _bookId;
            }
            init
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
        public Guid OrderId
        {
            get
            {
                return _orderId;
            }
            init
            {
                _orderId = value;
            }
        }
        public Order? Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
            }
        }
        #endregion
    }
}
