namespace BookStore.Application.Models.Report
{
    public class DailyReportModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalIncome { get; set; }
        public int TotalNewCreatedAccount { get; set; }
    }
}
