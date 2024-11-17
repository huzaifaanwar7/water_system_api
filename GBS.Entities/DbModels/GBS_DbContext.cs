using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GBS.Entities.DbModels;

public partial class GBS_DbContext : DbContext
{
    public GBS_DbContext()
    {
    }

    public GBS_DbContext(DbContextOptions<GBS_DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lookup> Lookups { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.227.81.51,58243;Database=gbs_dev;User=ihs;Password=ihs@1234;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lookup>(entity =>
        {
            entity.ToTable("lookups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(512)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(64)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_employee");

            entity.ToTable("user");

            entity.HasIndex(e => e.Id, "IX_employee");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(128)
                .HasColumnName("account_number");
            entity.Property(e => e.AccountTitle)
                .HasMaxLength(128)
                .HasColumnName("account_title");
            entity.Property(e => e.BankName)
                .HasMaxLength(128)
                .HasColumnName("bank_name");
            entity.Property(e => e.BranchCode)
                .HasMaxLength(128)
                .HasColumnName("branch_code");
            entity.Property(e => e.Cnic)
                .HasMaxLength(16)
                .HasColumnName("cnic");
            entity.Property(e => e.EmergencyPhone)
                .HasMaxLength(16)
                .HasColumnName("emergency_phone");
            entity.Property(e => e.EmployeeCode)
                .ValueGeneratedOnAdd()
                .HasColumnName("employee_code");
            entity.Property(e => e.FirstName)
                .HasMaxLength(128)
                .HasColumnName("first_name");
            entity.Property(e => e.Iban)
                .HasMaxLength(128)
                .HasColumnName("iban");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.JoiningDate)
                .HasColumnType("datetime")
                .HasColumnName("joining_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(128)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(64)
                .HasColumnName("personal_email");
            entity.Property(e => e.PersonalPhone)
                .HasMaxLength(16)
                .HasColumnName("personal_phone");
            entity.Property(e => e.Role)
                .HasMaxLength(128)
                .HasColumnName("role");
            entity.Property(e => e.SeparationDate)
                .HasColumnType("datetime")
                .HasColumnName("separation_date");
            entity.Property(e => e.StatusFk).HasColumnName("status_fk");
            entity.Property(e => e.TechStack)
                .HasMaxLength(128)
                .HasColumnName("tech_stack");
            entity.Property(e => e.UserName)
                .HasMaxLength(128)
                .HasColumnName("user_name");

            entity.HasOne(d => d.StatusFkNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_lookups");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
