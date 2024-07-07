using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime SignUpTime { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// 1：普通用户；2：vip用户；3：admin；4：superAdmin
    /// </summary>
    public int AccountTypeId { get; set; }

    /// <summary>
    /// 是否被冻结
    /// </summary>
    public bool IsSuspend { get; set; }

    /// <summary>
    /// 是否被注销
    /// </summary>
    public bool IsValid { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    /// <summary>
    /// 上一次学习的BookId
    /// </summary>
    public int? LastLedBookId { get; set; }

    public string? Settings { get; set; }

    public virtual AccountType AccountType { get; set; } = null!;

    public virtual ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();

    public virtual ICollection<UserCreatedVocabularyEn> UserCreatedVocabularyEns { get; set; } = new List<UserCreatedVocabularyEn>();

    public virtual ICollection<UserVocabulary> UserVocabularies { get; set; } = new List<UserVocabulary>();
}
