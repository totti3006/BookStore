using BookStore.Application.Models.Report;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookStore.Application.PdfDocument
{
    public class DailyReportDocument : IDocument
    {
        private readonly DailyReportModel _dailyReport;

        public DailyReportDocument(DailyReportModel dailyReport)
        {
            _dailyReport = dailyReport;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Daily Report #{_dailyReport.Id}")
                          .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"{_dailyReport.CreatedDate.ToLocalTime()}");
                    });
                });

                //row.ConstantItem(175).Image();
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.Spacing(5);
                    row.AutoItem().Text("1. ");
                    row.RelativeItem().Text($"Total order: {_dailyReport.TotalOrder}");
                });

                column.Item().Row(row =>
                {
                    row.Spacing(5);
                    row.AutoItem().Text("2. ");
                    row.RelativeItem().Text($"Total income: {_dailyReport.TotalIncome}");
                });

                column.Item().Row(row =>
                {
                    row.Spacing(5);
                    row.AutoItem().Text("3. ");
                    row.RelativeItem().Text($"Total new created account: {_dailyReport.TotalNewCreatedAccount}");
                });
            });
        }
    }
}
