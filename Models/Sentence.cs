using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Sentence
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public Guid BookId { get; set; }

    public string SentenceEn { get; set; } = null!;

    public string SentenceCn { get; set; } = null!;
}
