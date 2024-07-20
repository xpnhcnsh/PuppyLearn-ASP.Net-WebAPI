using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class WordReport
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid WordId { get; set; }

    public string? Fields { get; set; }

    public string? Comment { get; set; }

    /// <summary>
    /// 1:已处理；0未处理
    /// </summary>
    public bool Status { get; set; }

    public DateTime SubmitTime { get; set; }

    public DateTime? ProcessTime { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
