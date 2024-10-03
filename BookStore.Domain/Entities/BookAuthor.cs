namespace BookStore.Domain.Entities
{
    public class BookAuthor
    {
        #region Private Fields
        private Guid _bookId;
        private Book? _book;
        private Guid _authorId;
        private Author? _author;
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
        public virtual Book? Book
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
        public Guid AuthorId
        {
            get
            {
                return _authorId;
            }
            init
            {
                _authorId = value;
            }
        }
        public Author? Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
            }
        }
        #endregion
    }
}
