using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Progress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public Guid WordId { get; set; }

    public int Status { get; set; }

    public DateTime? LastUpdateTime { get; set; }
}
