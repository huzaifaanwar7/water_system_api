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

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeBankDetail> EmployeeBankDetails { get; set; }

    public virtual DbSet<EmployeeJobRole> EmployeeJobRoles { get; set; }

    public virtual DbSet<EmployeeLedger> EmployeeLedgers { get; set; }

    public virtual DbSet<EmployeeSalaryRate> EmployeeSalaryRates { get; set; }

    public virtual DbSet<EmployeeSalarySheet> EmployeeSalarySheets { get; set; }

    public virtual DbSet<EmployeeTechStack> EmployeeTechStacks { get; set; }

    public virtual DbSet<EmployeeUserRole> EmployeeUserRoles { get; set; }

    public virtual DbSet<Lookup> Lookups { get; set; }

    public virtual DbSet<MediaFile> MediaFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MUHAMMAD-ZEE;Database=gbs_dev;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentType).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ObjectType).HasMaxLength(50);

            entity.HasOne(d => d.MediaFileIdFkNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.MediaFileIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_MediaFile");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.Id, "IX_Employee_Id");

            entity.Property(e => e.Cnic)
                .HasMaxLength(16)
                .HasColumnName("CNIC");
            entity.Property(e => e.EmergencyPhone).HasMaxLength(16);
            entity.Property(e => e.FirstName).HasMaxLength(128);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JoiningDate).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(128);
            entity.Property(e => e.Password).HasMaxLength(128);
            entity.Property(e => e.PersonalEmail).HasMaxLength(64);
            entity.Property(e => e.PersonalPhone).HasMaxLength(16);
            entity.Property(e => e.SeparationDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(128);

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StatusIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Status");
        });

        modelBuilder.Entity<EmployeeBankDetail>(entity =>
        {
            entity.ToTable("EmployeeBankDetail");

            entity.Property(e => e.AccountNumber).HasMaxLength(128);
            entity.Property(e => e.AccountTitle).HasMaxLength(128);
            entity.Property(e => e.BankName).HasMaxLength(128);
            entity.Property(e => e.BranchCode).HasMaxLength(128);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Iban)
                .HasMaxLength(128)
                .HasColumnName("IBAN");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeBankDetails)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeBankDetail_Employee");
        });

        modelBuilder.Entity<EmployeeJobRole>(entity =>
        {
            entity.ToTable("EmployeeJobRole");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeJobRoles)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeJobRole_Employee");

            entity.HasOne(d => d.JobRoleIdFkNavigation).WithMany(p => p.EmployeeJobRoles)
                .HasForeignKey(d => d.JobRoleIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeJobRole_JobRole");
        });

        modelBuilder.Entity<EmployeeLedger>(entity =>
        {
            entity.ToTable("EmployeeLedger");

            entity.HasIndex(e => e.EmployeeIdFk, "IX_EmployeeLedger_EmployeeIdFk");

            entity.HasIndex(e => e.SalaryMonth, "IX_EmployeeLedger_SalaryMonth");

            entity.HasIndex(e => new { e.StatusIdFk, e.IsActive }, "IX_EmployeeLedger_Status_IsActive");

            entity.HasIndex(e => e.TransactionDate, "IX_EmployeeLedger_TransactionDate");

            entity.HasIndex(e => e.TransactionTypeIdFk, "IX_EmployeeLedger_TransactionType");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreditAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DebitAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RunningBalance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SalaryMonth).HasMaxLength(7);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeLedgers)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLedger_Employee");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.EmployeeLedgerStatusIdFkNavigations)
                .HasForeignKey(d => d.StatusIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLedger_Status");

            entity.HasOne(d => d.TransactionTypeIdFkNavigation).WithMany(p => p.EmployeeLedgerTransactionTypeIdFkNavigations)
                .HasForeignKey(d => d.TransactionTypeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLedger_TransactionTypeIdFk");
        });

        modelBuilder.Entity<EmployeeSalaryRate>(entity =>
        {
            entity.ToTable("EmployeeSalaryRate");

            entity.HasIndex(e => e.ApprovedBy, "IX_EmployeeSalaryRate_ApprovedBy");

            entity.HasIndex(e => e.EffectiveDate, "IX_EmployeeSalaryRate_EffectiveDate");

            entity.HasIndex(e => e.EmployeeIdFk, "IX_EmployeeSalaryRate_EmployeeIdFk");

            entity.HasIndex(e => new { e.EmployeeIdFk, e.EffectiveDate }, "IX_EmployeeSalaryRate_Employee_EffectiveDate").IsDescending(false, true);

            entity.HasIndex(e => e.IsActive, "IX_EmployeeSalaryRate_IsActive");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IncrementAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IncrementPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.PreviousRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SalaryRate).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<EmployeeSalarySheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalarySheet");

            entity.ToTable("EmployeeSalarySheet");

            entity.HasIndex(e => e.EmployeeIdFk, "IX_SalarySheet_EmployeeIdFk");

            entity.HasIndex(e => e.SalaryMonth, "IX_SalarySheet_SalaryMonth");

            entity.HasIndex(e => e.StatusIdFk, "IX_SalarySheet_Status");

            entity.HasIndex(e => new { e.EmployeeIdFk, e.SalaryMonth }, "UK_SalarySheet_Employee_Month").IsUnique();

            entity.Property(e => e.AdvanceDeduction).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Arrears).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DaysAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FuelAllowance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IncomeTaxDeduction).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MessDeduction).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.NetPayable).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OtherDeductions).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OvertimeAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OvertimeHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PayableAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProcessedDate).HasColumnType("datetime");
            entity.Property(e => e.SalaryMonth).HasMaxLength(7);
            entity.Property(e => e.SalaryRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalDeductions).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<EmployeeTechStack>(entity =>
        {
            entity.ToTable("EmployeeTechStack");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeTechStacks)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeTechStack_Employee");

            entity.HasOne(d => d.TechStackIdFkNavigation).WithMany(p => p.EmployeeTechStacks)
                .HasForeignKey(d => d.TechStackIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeTechStack_TechStack");
        });

        modelBuilder.Entity<EmployeeUserRole>(entity =>
        {
            entity.ToTable("EmployeeUserRole");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.EmployeeUserRoles)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeUserRole_Employee");

            entity.HasOne(d => d.UserRoleIdFkNavigation).WithMany(p => p.EmployeeUserRoles)
                .HasForeignKey(d => d.UserRoleIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeUserRole_UserRole");
        });

        modelBuilder.Entity<Lookup>(entity =>
        {
            entity.HasIndex(e => new { e.Type, e.IsActive }, "IX_Lookups_Type_IsActive");

            entity.HasIndex(e => new { e.Type, e.Name }, "UK_Lookups_Type_Name").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(512);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<MediaFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Media__3214EC07B01863DB");

            entity.ToTable("MediaFile");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Application).HasMaxLength(120);
            entity.Property(e => e.Bucket).HasMaxLength(120);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MimeType).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
