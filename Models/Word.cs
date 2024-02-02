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

    public virtual BooksEn Book { get; set; } = null!;

    public virtual ICollection<Cognate> Cognates { get; set; } = new List<Cognate>();

    public virtual ICollection<Phrase> Phrases { get; set; } = new List<Phrase>();

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual ICollection<RemMethod> RemMethods { get; set; } = new List<RemMethod>();

    public virtual ICollection<Sentence> Sentences { get; set; } = new List<Sentence>();

    public virtual ICollection<SingleChoiceQuestion> SingleChoiceQuestions { get; set; } = new List<SingleChoiceQuestion>();

    public virtual ICollection<Synonymou> Synonymous { get; set; } = new List<Synonymou>();

    public virtual ICollection<Tran> Trans { get; set; } = new List<Tran>();

    public virtual ICollection<UserVocabulary> UserVocabularies { get; set; } = new List<UserVocabulary>();
}
