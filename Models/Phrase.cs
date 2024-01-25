using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Phrase
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    /// <summary>
    /// 来自哪个单词书
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// 英文短语
    /// </summary>
    public string PhraseEn { get; set; } = null!;

    /// <summary>
    /// 短语中文
    /// </summary>
    public string PhraseCn { get; set; } = null!;

    public virtual BooksEn Book { get; set; } = null!;
}
