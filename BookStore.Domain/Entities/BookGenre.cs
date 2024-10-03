namespace BookStore.Domain.Entities
{
    public class BookGenre
    {
        #region Private Fields
        private Guid _bookId;
        private Book? _book;
        private Guid _genreId;
        private Genre? _genre;
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
        public Guid GenreId
        {
            get
            {
                return _genreId;
            }
            init
            {
                _genreId = value;
            }
        }
        public Genre? Genre
        {
            get
            {
                return _genre;
            }
            set
            {
                _genre = value;
            }
        }
        #endregion
    }
}
