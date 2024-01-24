using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Sentence
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
    /// 例句英文
    /// </summary>
    public string SentenceEn { get; set; } = null!;

    /// <summary>
    /// 例句中文
    /// </summary>
    public string SentenceCn { get; set; } = null!;

    public virtual BooksEn Book { get; set; } = null!;

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
