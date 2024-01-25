using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Tran
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    /// <summary>
    /// 中释
    /// </summary>
    public string TransCn { get; set; } = null!;

    /// <summary>
    /// 英释
    /// </summary>
    public string TransEn { get; set; } = null!;

    /// <summary>
    /// 词性
    /// </summary>
    public string Pos { get; set; } = null!;

    public Guid BookId { get; set; }

    public virtual BooksEn Book { get; set; } = null!;
}
