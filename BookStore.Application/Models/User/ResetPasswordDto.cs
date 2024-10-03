using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Models.User
{
    public class ResetPasswordDto
    {
        public string? Email { get; set; }

        public string? OtpCode { get; set; }

        public string? NewPasswordRaw { get; set; }
    }
}
