using BookStore.Application.Models.Email;

namespace BookStore.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendEmailForResetPassword(string email);
        public Task SendEmailForSuccessOrder(InvoiceEmailDto invoiceEmailDto);
        public Task SendEmailForDailyReport(string filePath);
    }
}
