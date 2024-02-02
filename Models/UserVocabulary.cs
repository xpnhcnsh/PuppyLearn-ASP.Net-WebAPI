using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class UserVocabulary
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid WordId { get; set; }

    public Guid BookId { get; set; }

    public Guid VocabularyId { get; set; }

    public DateTime AddTime { get; set; }
}
