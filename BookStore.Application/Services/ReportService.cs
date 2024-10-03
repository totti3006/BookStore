using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Report;
using BookStore.Application.PdfDocument;
using QuestPDF.Fluent;
using QuestPDF;
using QuestPDF.Infrastructure;

namespace BookStore.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public ReportService(IUnitOfWork unitOfWork, 
                             IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task GenerateDailyReport()
        {
            int totalOrderInDay = await _unitOfWork.OrderRepository.CountOrderByDate();
            decimal totalIncomeInDay = await _unitOfWork.OrderRepository.TotalIncomeByDate();
            int totalNewCreatedAccountInDay = await _unitOfWork.UserRepository.CountNewCreatedAccountByDate();

            var reportModel = new DailyReportModel
            {
                Id = Guid.NewGuid(),
                TotalOrder = totalOrderInDay,
                TotalIncome = totalIncomeInDay,
                TotalNewCreatedAccount = totalNewCreatedAccountInDay,
                CreatedDate = DateTime.Now,
            };

            var reportDoc = new DailyReportDocument(reportModel);

            string reportFolder = Directory.GetCurrentDirectory() + $"\\report\\report-{reportModel.Id}.pdf";

            await GeneratePdf(reportDoc, reportFolder);

            await _emailService.SendEmailForDailyReport(reportFolder);
        }

        private Task GeneratePdf(IDocument document, string resultPath)
        {
            Settings.License = LicenseType.Community;
            document.GeneratePdf(resultPath);
            return Task.CompletedTask;
        }
    }
}
