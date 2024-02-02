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

    public virtual BooksEn IdNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual UserCreatedVocabularyEn Vocabulary { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
