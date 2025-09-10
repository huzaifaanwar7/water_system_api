using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class Lookup
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? SortOrder { get; set; }

    public virtual ICollection<EmployeeJobRole> EmployeeJobRoles { get; set; } = new List<EmployeeJobRole>();

    public virtual ICollection<EmployeeLedger> EmployeeLedgerStatusIdFkNavigations { get; set; } = new List<EmployeeLedger>();

    public virtual ICollection<EmployeeLedger> EmployeeLedgerTransactionTypeIdFkNavigations { get; set; } = new List<EmployeeLedger>();

    public virtual ICollection<EmployeeTechStack> EmployeeTechStacks { get; set; } = new List<EmployeeTechStack>();

    public virtual ICollection<EmployeeUserRole> EmployeeUserRoles { get; set; } = new List<EmployeeUserRole>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
