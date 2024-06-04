using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class AccountType
{
    /// <summary>
    /// 1:normalUser;2:vip;3:teacher;4:admin;5:superAdmin
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 1:normalUser;2:vip;3:teacher;4:admin;5:superAdmin
    /// </summary>
    public string AccountName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
