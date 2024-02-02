﻿using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime SignUpTime { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int AccountTypeId { get; set; }

    public bool IsSuspend { get; set; }

    public bool IsValid { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public int? LastLedBookId { get; set; }
}
