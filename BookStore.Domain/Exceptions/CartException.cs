namespace BookStore.Domain.Exceptions
{
    public class CartException : Exception
    {
        public CartException(string message) : base(message) { }
        public CartException(string message, Exception innerException) : base(message, innerException) { }
    }
}
