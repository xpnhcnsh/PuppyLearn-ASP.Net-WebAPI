using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class UserVocabulary
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    /// <summary>
    /// 添加了哪个单词
    /// </summary>
    public Guid WordId { get; set; }

    /// <summary>
    /// 该单词来自于哪本书
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// 添加到哪个单词本
    /// </summary>
    public Guid VocabularyId { get; set; }

    /// <summary>
    /// 用户何时添加单词到单词本
    /// </summary>
    public DateTime AddTime { get; set; }

    public virtual BooksEn Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual UserCreatedVocabularyEn Vocabulary { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
