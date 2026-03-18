using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PharmaStock.Models;

public partial class PharmaStockContext : DbContext
{
    public PharmaStockContext()
    {
    }

    public PharmaStockContext(DbContextOptions<PharmaStockContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LTIN718562\\SQLEXPRESS;initial catalog=PharmaStock;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Integrated Security=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__Audit__A17F23B83DB6828D");

            entity.ToTable("Audit");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.Resource)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Audits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audit_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC43EBEEC6");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4830F8013").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D1053495815C86").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(true)
                .HasColumnName("StatusID");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__UserRole__8AFACE3A577A0A3B");

            entity.ToTable("UserRole");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
