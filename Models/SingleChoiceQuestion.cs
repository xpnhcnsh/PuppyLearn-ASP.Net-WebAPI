using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class SingleChoiceQuestion
{
    public Guid Id { get; set; }

    /// <summary>
    /// 和这道题相关的单词
    /// </summary>
    public Guid? WordId { get; set; }

    /// <summary>
    /// 该题属于某本书
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// 英文问题
    /// </summary>
    public string? QuestionEn { get; set; }

    /// <summary>
    /// 答案中文解释
    /// </summary>
    public string? AnsExplainCn { get; set; }

    /// <summary>
    /// 答案index：1；2；3；4
    /// </summary>
    public int? AnswerIndex { get; set; }

    public string? Choice1 { get; set; }

    public string? Choice2 { get; set; }

    public string? Choice3 { get; set; }

    public string? Choice4 { get; set; }

    public virtual Word? Word { get; set; }
}
