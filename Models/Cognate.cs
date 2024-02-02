using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Cognate
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public Guid BookId { get; set; }

    public string CognateEn { get; set; } = null!;

    public string CognateCn { get; set; } = null!;

    public string Pos { get; set; } = null!;

    public virtual BooksEn Book { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
