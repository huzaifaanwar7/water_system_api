using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.DbModels;

public partial class GBS_DbContext : DbContext
{
    public GBS_DbContext()
    {
    }

    public GBS_DbContext(DbContextOptions<GBS_DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

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

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialPurchase> MaterialPurchases { get; set; }

    public virtual DbSet<MediaFile> MediaFiles { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderCost> OrderCosts { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderLabor> OrderLabors { get; set; }

    public virtual DbSet<OrderMaterial> OrderMaterials { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.227.71.86,4431;Database=gbs_dev;User=ihs;Password=ihs@5678;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__E67E1A0453801724");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ClientName).HasMaxLength(256);
            entity.Property(e => e.ContactPerson).HasMaxLength(256);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(256)
                .HasColumnName("GSTNumber");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(128);
            entity.Property(e => e.PostalCode).HasMaxLength(256);
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(50);
        });

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
            entity.Property(e => e.Reference).HasMaxLength(50);
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
            entity.HasIndex(e => e.Type, "IX_Lookups_Type");

            entity.HasIndex(e => new { e.Type, e.IsActive }, "IX_Lookups_Type_IsActive");

            entity.HasIndex(e => new { e.Type, e.Name }, "UK_Lookups_Type_Name").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(512);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.ParentIdFkNavigation).WithMany(p => p.InverseParentIdFkNavigation)
                .HasForeignKey(d => d.ParentIdFk)
                .HasConstraintName("FK_Lookups_Parent");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC072E147A60");

            entity.HasIndex(e => e.MaterialCode, "UQ__Material__170C54BA10D3DA80").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentStock).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastPurchasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaterialCode).HasMaxLength(50);
            entity.Property(e => e.MaterialName).HasMaxLength(200);
            entity.Property(e => e.MinStockLevel).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.MaterialTypeIdFkNavigation).WithMany(p => p.MaterialMaterialTypeIdFkNavigations)
                .HasForeignKey(d => d.MaterialTypeIdFk)
                .HasConstraintName("FK_Materials_Type");

            entity.HasOne(d => d.UnitIdFkNavigation).WithMany(p => p.MaterialUnitIdFkNavigations)
                .HasForeignKey(d => d.UnitIdFk)
                .HasConstraintName("FK_Materials_Unit");
        });

        modelBuilder.Entity<MaterialPurchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC07C3F03DBB");

            entity.HasIndex(e => e.PurchaseNumber, "UQ__Material__373B5B6EFDB5F230").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PurchaseNumber).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", true)
                .HasColumnType("decimal(21, 4)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VendorName).HasMaxLength(200);

            entity.HasOne(d => d.MaterialIdFkNavigation).WithMany(p => p.MaterialPurchases)
                .HasForeignKey(d => d.MaterialIdFk)
                .HasConstraintName("FK_MaterialPurchases_Material");
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

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07CF5E0C34");

            entity.HasIndex(e => e.ClientIdFk, "IX_Orders_ClientID");

            entity.HasIndex(e => e.OrderDate, "IX_Orders_OrderDate");

            entity.HasIndex(e => e.StatusIdFk, "IX_Orders_StatusID");

            entity.HasIndex(e => e.Reference, "UQ__Orders__CAC5E74376887B20").IsUnique();

            entity.Property(e => e.AdvanceAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.BalanceAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.ClientIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Client");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Status");
        });

        modelBuilder.Entity<OrderCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderCos__3214EC07C22AD18E");

            entity.HasIndex(e => e.OrderIdFk, "IX_OrderCosts_OrderID");

            entity.Property(e => e.CostDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CostDescription).HasMaxLength(500);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VendorName).HasMaxLength(200);

            entity.HasOne(d => d.CostCategoryIdFkNavigation).WithMany(p => p.OrderCosts)
                .HasForeignKey(d => d.CostCategoryIdFk)
                .HasConstraintName("FK_OrderCosts_Category");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.OrderCosts)
                .HasForeignKey(d => d.OrderIdFk)
                .HasConstraintName("FK_OrderCosts_Order");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07F78B0A1C");

            entity.HasIndex(e => e.OrderIdFk, "IX_OrderItems_OrderID");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.CompletedQuantity).HasDefaultValue(0);
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.SpecialInstructions).HasMaxLength(500);
            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", true)
                .HasColumnType("decimal(21, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderIdFk)
                .HasConstraintName("FK_OrderItems_Order");

            entity.HasOne(d => d.ProductIdFkNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductIdFk)
                .HasConstraintName("FK_OrderItems_Product");

            entity.HasOne(d => d.SizeIdFkNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SizeIdFk)
                .HasConstraintName("FK_OrderItems_Size");
        });

        modelBuilder.Entity<OrderLabor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderLab__3214EC07462254CA");

            entity.ToTable("OrderLabor");

            entity.HasIndex(e => e.OrderIdFk, "IX_OrderLabor_OrderID");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HoursWorked).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.RatePerPiece).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalLaborCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.WorkDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.OrderLabors)
                .HasForeignKey(d => d.OrderIdFk)
                .HasConstraintName("FK_OrderLabor_Order");

            entity.HasOne(d => d.OrderItemIdFkNavigation).WithMany(p => p.OrderLabors)
                .HasForeignKey(d => d.OrderItemIdFk)
                .HasConstraintName("FK_OrderLabor_OrderItem");
        });

        modelBuilder.Entity<OrderMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderMat__3214EC07F9E864EC");

            entity.HasIndex(e => e.OrderIdFk, "IX_OrderMaterials_OrderID");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.QuantityUsed).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalCost)
                .HasComputedColumnSql("([QuantityUsed]*[UnitCost])", true)
                .HasColumnType("decimal(21, 4)");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsageDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaterialIdFkNavigation).WithMany(p => p.OrderMaterials)
                .HasForeignKey(d => d.MaterialIdFk)
                .HasConstraintName("FK_OrderMaterials_Material");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.OrderMaterials)
                .HasForeignKey(d => d.OrderIdFk)
                .HasConstraintName("FK_OrderMaterials_Order");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC07AB71105B");

            entity.ToTable("OrderStatusHistory");

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.StatusDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.OrderIdFk)
                .HasConstraintName("FK_StatusHistory_Order");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK_StatusHistory_Status");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC070298FE33");

            entity.HasIndex(e => e.OrderIdFk, "IX_Payments_OrderID");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Reference).HasMaxLength(100);

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Order");

            entity.HasOne(d => d.PaymentMethodIdFkNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Method");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07F5E036C9");

            entity.HasIndex(e => e.Reference, "UQ__Products__2F4E024F31FB17A3").IsUnique();

            entity.Property(e => e.BaseStitchingCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.Reference).HasMaxLength(50);

            entity.HasOne(d => d.CategoryIdFkNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Category");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
