using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class Word
{
    public Guid Id { get; set; }

    /// <summary>
    /// 单词
    /// </summary>
    public string WordName { get; set; } = null!;

    public Guid BookId { get; set; }

    /// <summary>
    /// 英式音标
    /// </summary>
    public string? Ukphone { get; set; }

    /// <summary>
    /// 美式音标
    /// </summary>
    public string? Usphone { get; set; }

    /// <summary>
    /// 英式发音请求url
    /// </summary>
    public string? Ukspeech { get; set; }

    /// <summary>
    /// 美式发音请求url
    /// </summary>
    public string? Usspeech { get; set; }

    /// <summary>
    /// 有些只有一个phone
    /// </summary>
    public string? Phone { get; set; }

    public string? Speech { get; set; }

    public virtual BooksEn Book { get; set; } = null!;

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual ICollection<RemMethod> RemMethods { get; set; } = new List<RemMethod>();

    public virtual ICollection<SingleChoiceQuestion> SingleChoiceQuestions { get; set; } = new List<SingleChoiceQuestion>();

    public virtual ICollection<UserVocabulary> UserVocabularies { get; set; } = new List<UserVocabulary>();
}
