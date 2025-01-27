﻿namespace BookStore.Domain.Exceptions
{
    public class ReviewException : Exception
    {
        public ReviewException(string message) : base(message) { }
        public ReviewException(string message, Exception innerException) : base(message, innerException) { }
    }
}
