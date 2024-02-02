using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Phrase
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public Guid BookId { get; set; }

    public string PhraseEn { get; set; } = null!;

    public string PhraseCn { get; set; } = null!;

    public virtual BooksEn Book { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
