using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PuppyLearn.Models;

public partial class PuppyLearnContext : DbContext
{
    public PuppyLearnContext()
    {
    }

    public PuppyLearnContext(DbContextOptions<PuppyLearnContext> options)
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
        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_accountType");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("accountName");
        });

        modelBuilder.Entity<BooksEn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Books_en_1");

            entity.ToTable("Books_en");

            entity.HasIndex(e => e.BookName, "IX_Books_en").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookName)
                .HasMaxLength(20)
                .HasColumnName("bookName");
            entity.Property(e => e.WordsCount)
                .HasComment("共有多少单词")
                .HasColumnName("wordsCount");
        });

        modelBuilder.Entity<Cognate>(entity =>
        {
            entity.ToTable("Cognate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CognateCn)
                .HasComment("同根词中文")
                .HasColumnName("cognate_cn");
            entity.Property(e => e.CognateEn)
                .HasComment("同根词英文")
                .HasColumnName("cognate_en");
            entity.Property(e => e.Pos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("词性")
                .HasColumnName("pos");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Cognates)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cognate_Books_en");
        });

        modelBuilder.Entity<Phrase>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId)
                .HasComment("来自哪个单词书")
                .HasColumnName("bookId");
            entity.Property(e => e.PhraseCn)
                .HasComment("短语中文")
                .HasColumnName("phrase_cn");
            entity.Property(e => e.PhraseEn)
                .HasComment("英文短语")
                .HasColumnName("phrase_en");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Phrases)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phrases_Books_en");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.ToTable("Progress");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.LastUpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdateTime");
            entity.Property(e => e.Status)
                .HasComment("1：一天后复习；2：已背；3：3天后复习；7：7天后复习")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progress_Books_en");

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
            entity.Property(e => e.Method)
                .HasComment("记忆方法")
                .HasColumnName("method");
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
            entity.Property(e => e.BookId)
                .HasComment("书本Id")
                .HasColumnName("bookId");
            entity.Property(e => e.SentenceCn)
                .HasComment("例句中文")
                .HasColumnType("ntext")
                .HasColumnName("sentence_cn");
            entity.Property(e => e.SentenceEn)
                .HasComment("例句英文")
                .HasColumnType("ntext")
                .HasColumnName("sentence_en");
            entity.Property(e => e.WordId)
                .HasComment("单词Id")
                .HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Sentences)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sentences_Books_en");
        });

        modelBuilder.Entity<SingleChoiceQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Exam");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AnsExplainCn)
                .HasComment("答案中文解释")
                .HasColumnType("ntext")
                .HasColumnName("ansExplain_cn");
            entity.Property(e => e.AnswerIndex)
                .HasComment("答案index：1；2；3；4")
                .HasColumnName("answerIndex");
            entity.Property(e => e.BookId)
                .HasComment("该题属于某本书")
                .HasColumnName("bookId");
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
                .HasComment("英文问题")
                .HasColumnType("ntext")
                .HasColumnName("question_en");
            entity.Property(e => e.WordId)
                .HasComment("和这道题相关的单词")
                .HasColumnName("wordId");

            entity.HasOne(d => d.Word).WithMany(p => p.SingleChoiceQuestions)
                .HasForeignKey(d => d.WordId)
                .HasConstraintName("FK_SingleChoiceQuestions_Words");
        });

        modelBuilder.Entity<Synonymou>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId)
                .HasComment("书本Id")
                .HasColumnName("bookId");
            entity.Property(e => e.Pos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("词性")
                .HasColumnName("pos");
            entity.Property(e => e.SynoEn)
                .HasComment("同义词（组）")
                .HasColumnName("syno_en");
            entity.Property(e => e.TransCn)
                .HasComment("中文翻译")
                .HasColumnName("trans_cn");
            entity.Property(e => e.WordId)
                .HasComment("单词Id")
                .HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Synonymous)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Synonymous_Books_en");
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
                .HasComment("词性")
                .HasColumnName("pos");
            entity.Property(e => e.TransCn)
                .HasComment("中释")
                .HasColumnName("trans_cn");
            entity.Property(e => e.TransEn)
                .HasComment("英释")
                .HasColumnName("trans_en");
            entity.Property(e => e.WordId).HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.Trans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trans_Books_en");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.UserName, "IX_Users").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountTypeId)
                .HasComment("1：普通用户；2：vip用户；3：admin；4：superAdmin")
                .HasColumnName("accountTypeId");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsSuspend)
                .HasComment("是否被冻结")
                .HasColumnName("isSuspend");
            entity.Property(e => e.IsValid)
                .HasComment("是否被注销")
                .HasColumnName("isValid");
            entity.Property(e => e.LastLedBookId)
                .HasComment("上一次学习的BookId")
                .HasColumnName("lastLedBookId");
            entity.Property(e => e.LastLoginTime)
                .HasColumnType("datetime")
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
                .HasColumnType("datetime")
                .HasColumnName("signUpTime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasOne(d => d.AccountType).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_AccountTypes1");
        });

        modelBuilder.Entity<UserBook>(entity =>
        {
            entity.ToTable("User_Book");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Finished)
                .HasComment("是否学习完成")
                .HasColumnName("finished");
            entity.Property(e => e.LastUpdateTime)
                .HasComment("上次背这本词典的时间")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdateTime");
            entity.Property(e => e.RepeatTimes)
                .HasComment("学习了几遍")
                .HasColumnName("repeatTimes");
            entity.Property(e => e.StartDateTime)
                .HasComment("开始学习某本书的datetime")
                .HasColumnType("datetime")
                .HasColumnName("startDateTime");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WordsPerday)
                .HasComment("计划每天学习单词个数")
                .HasColumnName("wordsPerday");

            entity.HasOne(d => d.Book).WithMany(p => p.UserBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Book_Books_en");

            entity.HasOne(d => d.User).WithMany(p => p.UserBooks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Book_Users");
        });

        modelBuilder.Entity<UserCreatedVocabularyEn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserCreatedBooks_en");

            entity.ToTable("UserCreatedVocabulary_en");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasComment("单词本创建时间")
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.LastUpdateTime)
                .HasComment("上次修改时间")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdateTime");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VocabularyName)
                .HasMaxLength(50)
                .HasComment("单词本名称")
                .HasColumnName("vocabularyName");

            entity.HasOne(d => d.User).WithMany(p => p.UserCreatedVocabularyEns)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCreatedBooks_en_Users");
        });

        modelBuilder.Entity<UserVocabulary>(entity =>
        {
            entity.ToTable("User_Vocabulary");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AddTime)
                .HasComment("用户何时添加单词到单词本")
                .HasColumnType("datetime")
                .HasColumnName("addTime");
            entity.Property(e => e.BookId)
                .HasComment("该单词来自于哪本书")
                .HasColumnName("bookId");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VocabularyId)
                .HasComment("添加到哪个单词本")
                .HasColumnName("vocabularyId");
            entity.Property(e => e.WordId)
                .HasComment("添加了哪个单词")
                .HasColumnName("wordId");

            entity.HasOne(d => d.Book).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_Books_en");

            entity.HasOne(d => d.User).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_Users");

            entity.HasOne(d => d.Vocabulary).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.VocabularyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_UserCreatedBooks_en");

            entity.HasOne(d => d.Word).WithMany(p => p.UserVocabularies)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Vocabulary_Words");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_words");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .HasComment("有些只有一个phone")
                .HasColumnName("phone");
            entity.Property(e => e.Speech)
                .HasMaxLength(200)
                .HasColumnName("speech");
            entity.Property(e => e.Ukphone)
                .HasMaxLength(100)
                .HasComment("英式音标")
                .HasColumnName("ukphone");
            entity.Property(e => e.Ukspeech)
                .HasMaxLength(200)
                .HasComment("英式发音请求url")
                .HasColumnName("ukspeech");
            entity.Property(e => e.Usphone)
                .HasMaxLength(100)
                .HasComment("美式音标")
                .HasColumnName("usphone");
            entity.Property(e => e.Usspeech)
                .HasMaxLength(200)
                .HasComment("美式发音请求url")
                .HasColumnName("usspeech");
            entity.Property(e => e.WordName)
                .HasMaxLength(50)
                .HasComment("单词")
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
