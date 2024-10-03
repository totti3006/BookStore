namespace BookStore.Application.Models.Book
{
    public class FavoriteDto
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
