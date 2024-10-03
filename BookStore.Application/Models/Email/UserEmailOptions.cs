using System.Net.Mail;

namespace BookStore.Application.Models.Email
{
    public class UserEmailOptions
    {
        public List<string> RecipientEmails { get; set; } = new();
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<Attachment> Attachments { get; set; } = new();
    }
}
