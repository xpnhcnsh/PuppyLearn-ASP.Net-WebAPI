using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class UserBook
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public bool Finished { get; set; }

    public DateTime StartDateTime { get; set; }

    public int WordsPerday { get; set; }

    public int RepeatTimes { get; set; }

    public DateTime? LastUpdateTime { get; set; }

    public virtual BooksEn Book { get; set; } = null!;
}
