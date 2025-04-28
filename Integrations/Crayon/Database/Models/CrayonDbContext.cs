using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Integrations.Crayon.Database.Models;

public partial class CrayonDbContext : DbContext
{
    public CrayonDbContext(DbContextOptions<CrayonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<License> Licenses { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0705FAC55C");

            entity.ToTable("Account");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Account_Customer");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07200AB1DB");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ__tmp_ms_x__A9D1053455DA75F2").IsUnique();

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Salt)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.RefreshToken)
                .IsRequired(false)
                .HasMaxLength(255);
            entity.Property(e => e.RefreshTokenExpiry)
                .IsRequired(false);
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__License__3214EC073DD2B008");

            entity.ToTable("License");

            entity.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(36);

            entity.HasOne(d => d.Subscription).WithMany(p => p.Licenses)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK_License_Subscription");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0704A7B7D0");

            entity.ToTable("Subscription");

            entity.Property(e => e.SoftwareDescription).HasMaxLength(300);
            entity.Property(e => e.SoftwareName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.ValidUntil).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Subscription_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
