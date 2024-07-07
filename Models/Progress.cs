using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Progress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public Guid WordId { get; set; }

    /// <summary>
    /// 1:1级；2:2级；3:3级（等级越高，表示本单词背错的次数越多，每次生成背诵List时，从3级选择较最多单词，2级选择中等个数单词，从1级选择最少的单词；如果背刺背错，则升级，反之降级；最高3级，当为0级时，从Progress表里删除本条目。）
    /// </summary>
    public int Status { get; set; }

    public DateTime? LastUpdateTime { get; set; }

    public virtual BooksEn Book { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
