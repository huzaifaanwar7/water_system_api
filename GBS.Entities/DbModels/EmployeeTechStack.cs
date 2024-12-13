using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class EmployeeTechStack
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public int TeckStackIdFk { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual TechStack TeckStackIdFkNavigation { get; set; } = null!;
}
