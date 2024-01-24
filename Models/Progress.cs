using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Progress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public Guid WordId { get; set; }

    /// <summary>
    /// 1：一天后复习；2：已背；3：3天后复习；7：7天后复习
    /// </summary>
    public int Status { get; set; }

    public DateTime? LastUpdateTime { get; set; }

    public virtual BooksEn Book { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
