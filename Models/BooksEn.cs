using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class BooksEn
{
    public Guid Id { get; set; }

    public string BookName { get; set; } = null!;

    public int? WordsCount { get; set; }
}
