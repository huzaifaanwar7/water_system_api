using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PrescottAppBackend.Domain.DbModels;

public partial class PrescottContext : DbContext
{
    public PrescottContext()
    {
    }

    public PrescottContext(DbContextOptions<PrescottContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BilledItem> BilledItems { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<DropdownListChild> DropdownListChildren { get; set; }

    public virtual DbSet<DropdownListParent> DropdownListParents { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<IncomeTracker> IncomeTrackers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=Prescott;User=root;Password=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BilledItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("BilledItem", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.IncomeTrackerId).HasColumnType("int(11)");
            entity.Property(e => e.ProductId).HasColumnType("int(11)");
            entity.Property(e => e.UnitSold).HasColumnType("int(11)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Contacts", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<DropdownListChild>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DropdownListChild", "prescott");

            entity.HasIndex(e => e.ParentId, "ParentId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Label).HasMaxLength(255);
            entity.Property(e => e.ParentId).HasColumnType("int(11)");
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Parent).WithMany(p => p.DropdownListChildren)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("dropdownlistchild_ibfk_1");
        });

        modelBuilder.Entity<DropdownListParent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DropdownListParent", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Expense", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Account).HasMaxLength(255);
            entity.Property(e => e.Amount).HasPrecision(10);
            entity.Property(e => e.BillId).HasColumnType("int(11)");
            entity.Property(e => e.Billable).HasDefaultValueSql("'0'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.CustomerJob)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.Memo)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("timestamp");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<IncomeTracker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("IncomeTracker", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.AmountDue).HasPrecision(10);
            entity.Property(e => e.BillDate).HasColumnType("date");
            entity.Property(e => e.BillDue).HasColumnType("date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.Memo)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.RefNo)
                .HasMaxLength(50)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Terms)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("timestamp");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.VendorId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Products", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.ItemType)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("enum('Hardware','Software','Service')");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.PerUnitPrice).HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Reference)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.TotalUnits)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PurchaseOrders", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.Memo)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.Ponum)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("PONum");
            entity.Property(e => e.RefNo)
                .HasMaxLength(50)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.ShipTo)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.TotalOrderAmount).HasPrecision(22);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.VendorId).HasColumnType("int(11)");
            entity.Property(e => e.VendorMessage)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PurchaseOrderItems", "prescott");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Amount).HasPrecision(10);
            entity.Property(e => e.Class)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Customer)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.Item)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.PurchaseOrderId).HasColumnType("int(11)");
            entity.Property(e => e.Quantity).HasColumnType("int(11)");
            entity.Property(e => e.Rate).HasPrecision(10);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Roles", "prescott");

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

            entity.ToTable("Users", "prescott");

            entity.Property(e => e.Address)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
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
