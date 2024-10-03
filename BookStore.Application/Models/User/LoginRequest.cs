using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Models.User
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "{0} can't be empty or null")]
        [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} can't be empty or null")]
        public string? Password { get; set; }
    }
}
