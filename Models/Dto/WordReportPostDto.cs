namespace PuppyLearn.Models.Dto
{
    public class WordReportPostDto
    {
        public Guid WordId { get; set; }
        public Guid UserId { get; set; }
        public string? Fields { get; set; }
        public string? Comment { get; set; }
        public int Status { get; set; }
    }
}
