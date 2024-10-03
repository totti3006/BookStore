namespace BookStore.Application.Models.Book
{
    public class BookParameters
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public short PublishYear { get; set; } = -1;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
