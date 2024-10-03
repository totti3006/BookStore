namespace BookStore.Application.Models.User
{
    public class CreateUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordRaw { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
