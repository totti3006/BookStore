using BookStore.Domain.Exceptions;
using System.Text;

namespace BookStore.Domain.Entities
{
    public class Author
    {
        #region Private Fields
        private Guid _id;
        private string? _name;
        private ICollection<BookAuthor>? _bookAuthors;
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
                    throw new AuthorException($"{nameof(Name)} must not be null or empty");
                }
                _name = value;
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
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}")
              .AppendLine($"Tên: {Name}");

            return sb.ToString();
        }
    }
}
