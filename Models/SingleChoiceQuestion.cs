using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class SingleChoiceQuestion
{
    public Guid Id { get; set; }

    public Guid? WordId { get; set; }

    public Guid BookId { get; set; }

    public string? QuestionEn { get; set; }

    public string? AnsExplainCn { get; set; }

    public int? AnswerIndex { get; set; }

    public string? Choice1 { get; set; }

    public string? Choice2 { get; set; }

    public string? Choice3 { get; set; }

    public string? Choice4 { get; set; }
}
