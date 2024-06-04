namespace PuppyLearn.Models.Dto
{
    public class UserBookDto
    {
        public Guid UserId { get; set; }

        public Guid BookId { get; set; }

        public bool Finished { get; set; }

        public DateTime StartDateTime { get; set; }

        public int WordsPerday { get; set; }

        public int RepeatTimes { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public virtual BooksEn Book { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
