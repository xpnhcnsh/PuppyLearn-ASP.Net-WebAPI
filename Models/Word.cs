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

    /// <summary>
    /// 来自哪本单词书
    /// </summary>
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
    /// 例句
    /// </summary>
    public Guid? SentenceId { get; set; }

    /// <summary>
    /// 近义同义词或短语
    /// </summary>
    public Guid? SynoId { get; set; }

    /// <summary>
    /// 短语
    /// </summary>
    public Guid? PhraseId { get; set; }

    /// <summary>
    /// 同根词
    /// </summary>
    public Guid? CognateId { get; set; }

    /// <summary>
    /// 翻译
    /// </summary>
    public Guid? TransId { get; set; }

    /// <summary>
    /// 习题Id，（如果有）
    /// </summary>
    public Guid? SingleChoiceQuestionId { get; set; }

    public virtual BooksEn Book { get; set; } = null!;

    public virtual Cognate? Cognate { get; set; }

    public virtual Phrase? Phrase { get; set; }

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual Sentence? Sentence { get; set; }

    public virtual ICollection<SingleChoiceQuestion> SingleChoiceQuestions { get; set; } = new List<SingleChoiceQuestion>();

    public virtual Synonymou? Syno { get; set; }

    public virtual Tran? Trans { get; set; }

    public virtual ICollection<UserVocabulary> UserVocabularies { get; set; } = new List<UserVocabulary>();
}
