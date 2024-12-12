using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class EmployeeTechStack
{
    public int Id { get; set; }

    public int EmployeeIdFk { get; set; }

    public int TeckStackIdFk { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual ICollection<EmployeeTechStack> InverseTeckStackIdFkNavigation { get; set; } = new List<EmployeeTechStack>();

    public virtual EmployeeTechStack TeckStackIdFkNavigation { get; set; } = null!;
}
