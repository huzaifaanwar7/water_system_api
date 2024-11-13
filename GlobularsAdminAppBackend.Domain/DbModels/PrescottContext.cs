using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GlobularsAdminAppBackend.Domain.DbModels;

public partial class PrescottContext : DbContext
{
    public PrescottContext()
    {
    }

    public PrescottContext(DbContextOptions<PrescottContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<AmenityImage> AmenityImages { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<AnnouncementImage> AnnouncementImages { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Dropdownlistchild> Dropdownlistchildren { get; set; }

    public virtual DbSet<Dropdownlistparent> Dropdownlistparents { get; set; }

    public virtual DbSet<ReportedProblem> ReportedProblems { get; set; }

    public virtual DbSet<ReportedProblemImage> ReportedProblemImages { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=127.0.0.1;Database=Prescott;User=root;Password=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("amenity", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AmenityName).HasMaxLength(255);
            entity.Property(e => e.BuildingId).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<AmenityImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("amenity_images", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AmenityId).HasColumnType("int(11)");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.FileType)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("announcements", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.BuildingId).HasColumnType("int(11)");
            entity.Property(e => e.Content)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<AnnouncementImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("announcement_images", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AnnouncementId).HasColumnType("int(11)");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.FileType)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("building", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.BuildingDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.BuildingName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Dropdownlistchild>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("dropdownlistchild", "prescott");

            entity.HasIndex(e => e.ParentId, "ParentId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Label).HasMaxLength(255);
            entity.Property(e => e.ParentId).HasColumnType("int(11)");
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Parent).WithMany(p => p.Dropdownlistchildren)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("dropdownlistchild_ibfk_1");
        });

        modelBuilder.Entity<Dropdownlistparent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("dropdownlistparent", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<ReportedProblem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reported_problem", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Problem)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<ReportedProblemImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reported_problem_images", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.FileType).HasMaxLength(255);
            entity.Property(e => e.ReportedProblemId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reservation", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AmenityId).HasColumnType("int(11)");
            entity.Property(e => e.BuildingId).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Reason)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles", "prescott");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.RoleDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.RoleName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users", "prescott");

            entity.Property(e => e.Address)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.BuildingId).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirebaseId).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("PhotoURL");
            entity.Property(e => e.RoleId).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.UserSignUpType).HasMaxLength(25);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
