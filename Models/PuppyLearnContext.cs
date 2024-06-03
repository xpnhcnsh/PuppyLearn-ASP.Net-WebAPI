using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PuppyLearn.Models;

public partial class PuppylearnContext : DbContext
{
    public PuppylearnContext()
    {
    }

    public PuppylearnContext(DbContextOptions<PuppylearnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<BooksEn> BooksEns { get; set; }

    public virtual DbSet<Cognate> Cognates { get; set; }

    public virtual DbSet<Phrase> Phrases { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<RemMethod> RemMethods { get; set; }

    public virtual DbSet<Sentence> Sentences { get; set; }

    public virtual DbSet<SingleChoiceQuestion> SingleChoiceQuestions { get; set; }

    public virtual DbSet<Synonymou> Synonymous { get; set; }

    public virtual DbSet<Tran> Trans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBook> UserBooks { get; set; }

    public virtual DbSet<UserCreatedVocabularyEn> UserCreatedVocabularyEns { get; set; }

    public virtual DbSet<UserVocabulary> UserVocabularies { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_PRC_CI_AS");

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("1:normalUser;2:vip;3:teacher;4:admin;5:superAdmin")
                .HasColumnName("id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasComment("1:normalUser;2:vip;3:teacher;4:admin;5:superAdmin")
                .HasColumnName("accountName");
        });

        modelBuilder.Entity<BooksEn>(entity =>
        {
            entity.ToTable("Books_en");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookName)
                .HasMaxLength(50)
                .HasColumnName("bookName");
            entity.Property(e => e.BookNameCh)
                .HasMaxLength(50)
                .HasColumnName("bookNameCh");
            entity.Property(e => e.WordsCount).HasColumnName("wordsCount");
        });

        modelBuilder.Entity<Cognate>(entity =>
        {
            entity.ToTable("Cognate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CognateCn).HasColumnName("cognate_cn");
            entity.Property(e => e.CognateEn).HasColumnName("cognate_en");
            entity.Property(e => e.Pos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Cognates)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cognate_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.Cognates)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cognate_Words");
        });

        modelBuilder.Entity<Phrase>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.PhraseCn).HasColumnName("phrase_cn");
            entity.Property(e => e.PhraseEn).HasColumnName("phrase_en");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Phrases)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phrases_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.Phrases)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phrases_Words");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.ToTable("Progress");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.LastUpdateTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("lastUpdateTime");
            entity.Property(e => e.Status)
                .HasComment("1：一天后复习；2：已背；3：3天后复习；7：7天后复习")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.User).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progress_Users");

            entity.HasOne(d => d.Word).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progress_Words");
        });

        modelBuilder.Entity<RemMethod>(entity =>
        {
            entity.ToTable("RemMethod");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Method).HasColumnName("method");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.RemMethods)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RemMethod_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.RemMethods)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RemMethod_Words");
        });

        modelBuilder.Entity<Sentence>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.SentenceCn)
                .HasColumnType("ntext")
                .HasColumnName("sentence_cn");
            entity.Property(e => e.SentenceEn)
                .HasColumnType("ntext")
                .HasColumnName("sentence_en");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Sentences)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sentences_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.Sentences)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sentences_Words");
        });

        modelBuilder.Entity<SingleChoiceQuestion>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AnsExplainCn)
                .HasColumnType("ntext")
                .HasColumnName("ansExplain_cn");
            entity.Property(e => e.AnswerIndex).HasColumnName("answerIndex");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Choice1)
                .HasMaxLength(50)
                .HasColumnName("choice1");
            entity.Property(e => e.Choice2)
                .HasMaxLength(50)
                .HasColumnName("choice2");
            entity.Property(e => e.Choice3)
                .HasMaxLength(50)
                .HasColumnName("choice3");
            entity.Property(e => e.Choice4)
                .HasMaxLength(50)
                .HasColumnName("choice4");
            entity.Property(e => e.QuestionEn)
                .HasColumnType("ntext")
                .HasColumnName("question_en");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.SingleChoiceQuestions)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SingleChoiceQuestions_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.SingleChoiceQuestions)
                .HasForeignKey(d => d.WordId)
                .HasConstraintName("FK_SingleChoiceQuestions_Words");
        });

        modelBuilder.Entity<Synonymou>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Pos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos");
            entity.Property(e => e.SynoEn).HasColumnName("syno_en");
            entity.Property(e => e.TransCn).HasColumnName("trans_cn");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Synonymous)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Synonymous_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.Synonymous)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Synonymous_Words");
        });

        modelBuilder.Entity<Tran>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Pos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos");
            entity.Property(e => e.TransCn).HasColumnName("trans_cn");
            entity.Property(e => e.TransEn).HasColumnName("trans_en");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Trans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trans_Books_en");

            entity.HasOne(d => d.Word).WithMany(p => p.Trans)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trans_Words");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountTypeId)
                .HasComment("1:normalUser;2:vip;3:teacher;4:admin;5:superAdmin")
                .HasColumnName("accountTypeId");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsSuspend).HasColumnName("isSuspend");
            entity.Property(e => e.IsValid).HasColumnName("isValid");
            entity.Property(e => e.LastLedBookId).HasColumnName("lastLedBookId");
            entity.Property(e => e.LastLoginTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("lastLoginTime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("passwordHash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(512)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("passwordSalt");
            entity.Property(e => e.SignUpTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("signUpTime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<UserBook>(entity =>
        {
            entity.ToTable("User_Book");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Finished).HasColumnName("finished");
            entity.Property(e => e.LastUpdateTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("lastUpdateTime");
            entity.Property(e => e.RepeatTimes).HasColumnName("repeatTimes");
            entity.Property(e => e.StartDateTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("startDateTime");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WordsPerday).HasColumnName("wordsPerday");

            entity.HasOne(d => d.Book).WithMany(p => p.UserBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Book_Books_en");
        });

        modelBuilder.Entity<UserCreatedVocabularyEn>(entity =>
        {
            entity.ToTable("UserCreatedVocabulary_en");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("createTime");
            entity.Property(e => e.LastUpdateTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("lastUpdateTime");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VocabularyName)
                .HasMaxLength(50)
                .HasColumnName("vocabularyName");

            entity.HasOne(d => d.User).WithMany(p => p.UserCreatedVocabularyEns)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCreatedVocabulary_en_Users");
        });

        modelBuilder.Entity<UserVocabulary>(entity =>
        {
            entity.ToTable("User_Vocabulary");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AddTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("addTime");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VocabularyId).HasColumnName("vocabularyId");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.UserVocabulary)
                .HasForeignKey<UserVocabulary>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_Books_en");

            entity.HasOne(d => d.User).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_Users");

            entity.HasOne(d => d.Vocabulary).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.VocabularyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_UserCreatedVocabulary_en");

            entity.HasOne(d => d.Word).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_Words");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .HasColumnName("phone");
            entity.Property(e => e.Speech)
                .HasMaxLength(200)
                .HasColumnName("speech");
            entity.Property(e => e.Ukphone)
                .HasMaxLength(100)
                .HasColumnName("ukphone");
            entity.Property(e => e.Ukspeech)
                .HasMaxLength(200)
                .HasColumnName("ukspeech");
            entity.Property(e => e.Usphone)
                .HasMaxLength(100)
                .HasColumnName("usphone");
            entity.Property(e => e.Usspeech)
                .HasMaxLength(200)
                .HasColumnName("usspeech");
            entity.Property(e => e.WordName)
                .HasMaxLength(50)
                .HasColumnName("wordName");

            entity.HasOne(d => d.Book).WithMany(p => p.Words)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Words_Books_en");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
