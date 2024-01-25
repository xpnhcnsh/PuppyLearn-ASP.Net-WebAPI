using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Cognate
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public Guid BookId { get; set; }

    /// <summary>
    /// 同根词英文
    /// </summary>
    public string CognateEn { get; set; } = null!;

    /// <summary>
    /// 同根词中文
    /// </summary>
    public string CognateCn { get; set; } = null!;

    /// <summary>
    /// 词性
    /// </summary>
    public string Pos { get; set; } = null!;

    public virtual BooksEn Book { get; set; } = null!;
}
