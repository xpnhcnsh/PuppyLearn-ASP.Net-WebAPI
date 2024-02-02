using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Tran
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public string TransCn { get; set; } = null!;

    public string TransEn { get; set; } = null!;

    public string Pos { get; set; } = null!;

    public Guid BookId { get; set; }
}
