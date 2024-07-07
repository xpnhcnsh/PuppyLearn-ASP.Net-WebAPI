using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class UserCreatedVocabularyEn
{
    public Guid Id { get; set; }

    /// <summary>
    /// 单词本名称
    /// </summary>
    public string VocabularyName { get; set; } = null!;

    public Guid UserId { get; set; }

    /// <summary>
    /// 单词本创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 上次修改时间
    /// </summary>
    public DateTime? LastUpdateTime { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserVocabulary> UserVocabularies { get; set; } = new List<UserVocabulary>();
}
