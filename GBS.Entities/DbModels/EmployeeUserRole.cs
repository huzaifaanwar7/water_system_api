using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class EmployeeUserRole
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public int UserRoleIdFk { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual Lookup UserRoleIdFkNavigation { get; set; } = null!;
}
