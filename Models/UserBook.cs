using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class UserBook
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    /// <summary>
    /// 是否学习完成
    /// </summary>
    public bool Finished { get; set; }

    /// <summary>
    /// 开始学习某本书的datetime
    /// </summary>
    public DateTime StartDateTime { get; set; }

    /// <summary>
    /// 计划每天学习单词个数
    /// </summary>
    public int WordsPerday { get; set; }

    /// <summary>
    /// 学习了几遍
    /// </summary>
    public int RepeatTimes { get; set; }

    /// <summary>
    /// 上次背这本词典的时间
    /// </summary>
    public DateTime? LastUpdateTime { get; set; }

    public virtual BooksEn Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
