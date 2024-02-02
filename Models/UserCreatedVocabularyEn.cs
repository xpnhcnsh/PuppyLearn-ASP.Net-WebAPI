﻿using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class UserCreatedVocabularyEn
{
    public Guid Id { get; set; }

    public string VocabularyName { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime? LastUpdateTime { get; set; }
}
