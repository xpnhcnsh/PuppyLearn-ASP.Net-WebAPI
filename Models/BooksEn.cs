using System;
using System.Collections.Generic;

namespace PuppyLearn.Models;

public partial class BooksEn
{
    public Guid Id { get; set; }

    public string BookName { get; set; } = null!;

    public int? WordsCount { get; set; }

    public virtual ICollection<Cognate> Cognates { get; set; } = new List<Cognate>();

    public virtual ICollection<Phrase> Phrases { get; set; } = new List<Phrase>();

    public virtual ICollection<RemMethod> RemMethods { get; set; } = new List<RemMethod>();

    public virtual ICollection<Sentence> Sentences { get; set; } = new List<Sentence>();

    public virtual ICollection<SingleChoiceQuestion> SingleChoiceQuestions { get; set; } = new List<SingleChoiceQuestion>();

    public virtual ICollection<Synonymou> Synonymous { get; set; } = new List<Synonymou>();

    public virtual ICollection<Tran> Trans { get; set; } = new List<Tran>();

    public virtual ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();

    public virtual UserVocabulary? UserVocabulary { get; set; }

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
