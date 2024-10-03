using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Models.User
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "{0} can't be empty or null")]
        public string? OtpCode { get; set; }

        [Required(ErrorMessage = "{0} can't be empty or null")]
        public string? NewPassword { get; set; }
    }
}
