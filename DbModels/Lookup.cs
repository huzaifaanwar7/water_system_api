using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class Lookup
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? SortOrder { get; set; }

    public int? ParentIdFk { get; set; }

    public virtual ICollection<EmployeeJobRole> EmployeeJobRoles { get; set; } = new List<EmployeeJobRole>();

    public virtual ICollection<EmployeeLedger> EmployeeLedgerStatusIdFkNavigations { get; set; } = new List<EmployeeLedger>();

    public virtual ICollection<EmployeeLedger> EmployeeLedgerTransactionTypeIdFkNavigations { get; set; } = new List<EmployeeLedger>();

    public virtual ICollection<EmployeeTechStack> EmployeeTechStacks { get; set; } = new List<EmployeeTechStack>();

    public virtual ICollection<EmployeeUserRole> EmployeeUserRoles { get; set; } = new List<EmployeeUserRole>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Lookup> InverseParentIdFkNavigation { get; set; } = new List<Lookup>();

    public virtual ICollection<Material> MaterialMaterialTypeIdFkNavigations { get; set; } = new List<Material>();

    public virtual ICollection<Material> MaterialUnitIdFkNavigations { get; set; } = new List<Material>();

    public virtual ICollection<OrderCost> OrderCosts { get; set; } = new List<OrderCost>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Lookup? ParentIdFkNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
