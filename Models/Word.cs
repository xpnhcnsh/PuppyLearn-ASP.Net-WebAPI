using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Word
{
    public Guid Id { get; set; }

    public string WordName { get; set; } = null!;

    public Guid BookId { get; set; }

    public string? Ukphone { get; set; }

    public string? Usphone { get; set; }

    public string? Ukspeech { get; set; }

    public string? Usspeech { get; set; }

    public string? Phone { get; set; }

    public string? Speech { get; set; }
}
