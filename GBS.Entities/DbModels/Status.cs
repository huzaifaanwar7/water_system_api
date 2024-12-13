using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
