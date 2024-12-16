using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class JobRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeJobRole> EmployeeJobRoles { get; set; } = new List<EmployeeJobRole>();
}
