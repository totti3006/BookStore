namespace BookStore.Domain.Exceptions
{
    public class AuthorException : Exception
    {
        public AuthorException(string message) : base(message) { }
        public AuthorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
