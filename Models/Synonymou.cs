using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Synonymou
{
    public Guid Id { get; set; }

    /// <summary>
    /// 单词Id
    /// </summary>
    public Guid WordId { get; set; }

    /// <summary>
    /// 书本Id
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// 词性
    /// </summary>
    public string Pos { get; set; } = null!;

    /// <summary>
    /// 中文翻译
    /// </summary>
    public string TransCn { get; set; } = null!;

    /// <summary>
    /// 同义词（组）
    /// </summary>
    public string SynoEn { get; set; } = null!;

    public virtual BooksEn Book { get; set; } = null!;
}
