using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class UserRole
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<EmployeeUserRole> EmployeeUserRoles { get; set; } = new List<EmployeeUserRole>();
}
