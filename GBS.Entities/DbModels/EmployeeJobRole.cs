using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class EmployeeJobRole
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public int JobRoleIdFk { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual JobRole JobRoleIdFkNavigation { get; set; } = null!;
}
