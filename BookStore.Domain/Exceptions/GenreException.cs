namespace BookStore.Domain.Exceptions
{
    public class GenreException : Exception
    {
        public GenreException(string message) : base(message) { }
        public GenreException(string message, Exception innerException) : base(message, innerException) { }
    }
}
