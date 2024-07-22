namespace PuppyLearn.Models.Dto
{
    public class WordReportGetDto
    {
        public Guid Id { get; set; }
        public string WordName { get; set; } = null!;
        public string? Fields { get; set; }
        public string? Comments { get; set; }
        public string Status { get; set; } = null!;
        public DateTime ReportDate { get; set; }
        public string UserEmail { get; set; } = null!;
        public string BookNameCh { get; set; } = null!;
    }
}
