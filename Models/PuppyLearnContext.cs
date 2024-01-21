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

    public virtual DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountTypeId).HasColumnName("accountTypeId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.IsSuspend).HasColumnName("isSuspend");
            entity.Property(e => e.IsValid).HasColumnName("isValid");
            entity.Property(e => e.LastLoginTime)
                .HasColumnType("datetime")
                .HasColumnName("lastLoginTime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("passwordHash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(64)
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
