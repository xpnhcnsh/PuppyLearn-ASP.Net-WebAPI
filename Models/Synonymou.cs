using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Synonymou
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public Guid BookId { get; set; }

    public string Pos { get; set; } = null!;

    public string TransCn { get; set; } = null!;

    public string SynoEn { get; set; } = null!;
}
