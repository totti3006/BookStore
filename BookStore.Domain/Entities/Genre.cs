using BookStore.Domain.Exceptions;

namespace BookStore.Domain.Entities
{
    public class Genre
    {
        #region Private Fields
        private Guid _id;
        private string? _name;
        private ICollection<BookGenre>? _bookGenres;
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
                    throw new GenreException($"{nameof(Name)} must not be null or empty");
                }
                _name = value;
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
        #endregion
    }
}
