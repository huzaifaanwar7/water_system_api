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

    public virtual DbSet<ApprovalHistory> ApprovalHistories { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeBankDetail> EmployeeBankDetails { get; set; }

    public virtual DbSet<EmployeeJobRole> EmployeeJobRoles { get; set; }

    public virtual DbSet<EmployeeTechStack> EmployeeTechStacks { get; set; }

    public virtual DbSet<EmployeeUserRole> EmployeeUserRoles { get; set; }

    public virtual DbSet<JobRole> JobRoles { get; set; }

    public virtual DbSet<MediaFile> MediaFiles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TechStack> TechStacks { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MUHAMMAD-ZEE;Database=gbs_dev;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApprovalHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ApprovalHistory", "Common");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DoneByName).HasMaxLength(256);
            entity.Property(e => e.LegacyUser).HasMaxLength(128);
            entity.Property(e => e.ObjectType).HasMaxLength(64);
            entity.Property(e => e.PendingWith).HasMaxLength(64);
            entity.Property(e => e.WaitingFor).HasMaxLength(64);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employee");

            entity.HasIndex(e => e.Id, "IX_employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cnic)
                .HasMaxLength(16)
                .HasColumnName("cnic");
            entity.Property(e => e.EmergencyPhone)
                .HasMaxLength(16)
                .HasColumnName("emergency_phone");
            entity.Property(e => e.EmployeeCode).HasColumnName("employee_code");
            entity.Property(e => e.FirstName)
                .HasMaxLength(128)
                .HasColumnName("first_name");
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
            entity.Property(e => e.ProfilePictureId).HasColumnName("profile_picture_Id");
            entity.Property(e => e.SeparationDate)
                .HasColumnType("datetime")
                .HasColumnName("separation_date");
            entity.Property(e => e.StatusIdFk).HasColumnName("status_id_fk");
            entity.Property(e => e.Username)
                .HasMaxLength(128)
                .HasColumnName("username");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StatusIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_status");
        });

        modelBuilder.Entity<EmployeeBankDetail>(entity =>
        {
            entity.ToTable("employee_bank_detail");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EmployeeIdFk).HasColumnName("employee_id_fk");
            entity.Property(e => e.Iban)
                .HasMaxLength(128)
                .HasColumnName("iban");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeBankDetails)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_bank_detail_employee");
        });

        modelBuilder.Entity<EmployeeJobRole>(entity =>
        {
            entity.ToTable("employee_job_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EmployeeIdFk).HasColumnName("employee_id_fk");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.JobRoleIdFk).HasColumnName("job_role_id_fk");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeJobRoles)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_job_role_employee");

            entity.HasOne(d => d.JobRoleIdFkNavigation).WithMany(p => p.EmployeeJobRoles)
                .HasForeignKey(d => d.JobRoleIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_job_role_job_role");
        });

        modelBuilder.Entity<EmployeeTechStack>(entity =>
        {
            entity.ToTable("employee_tech_stack");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EmployeeIdFk).HasColumnName("employee_id_fk");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.TeckStackIdFk).HasColumnName("teck_stack_id_fk");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeTechStacks)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_tech_stack_employee");

            entity.HasOne(d => d.TeckStackIdFkNavigation).WithMany(p => p.EmployeeTechStacks)
                .HasForeignKey(d => d.TeckStackIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_tech_stack_tech_stack");
        });

        modelBuilder.Entity<EmployeeUserRole>(entity =>
        {
            entity.ToTable("employee_user_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EmployeeIdFk).HasColumnName("employee_id_fk");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
            entity.Property(e => e.UserRoleIdFk).HasColumnName("user_role_id_fk");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeUserRoles)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_user_role_employee");

            entity.HasOne(d => d.UserRoleIdFkNavigation).WithMany(p => p.EmployeeUserRoles)
                .HasForeignKey(d => d.UserRoleIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_user_role_user_role");
        });

        modelBuilder.Entity<JobRole>(entity =>
        {
            entity.ToTable("job_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MediaFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Media__3214EC07B01863DB");

            entity.ToTable("media_file");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Application)
                .HasMaxLength(120)
                .HasColumnName("application");
            entity.Property(e => e.Bucket)
                .HasMaxLength(120)
                .HasColumnName("bucket");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.MimeType)
                .HasMaxLength(250)
                .HasColumnName("mime_type");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
            entity.Property(e => e.Size).HasColumnName("size");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_lookups");

            entity.ToTable("status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(512)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TechStack>(entity =>
        {
            entity.ToTable("tech_stack");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
