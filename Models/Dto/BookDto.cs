namespace PuppyLearn.Models.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string BookName { get; set; } = null!;
        public int? WordsCount { get; set; }
        public string? BookNameCh { get; set; }
        public string? Catalog { get; set; }
    }
}
