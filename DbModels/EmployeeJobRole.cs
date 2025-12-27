using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class EmployeeJobRole
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public int JobRoleIdFk { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual Lookup JobRoleIdFkNavigation { get; set; } = null!;
}
