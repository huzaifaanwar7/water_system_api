using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Dropdownlistparent
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Dropdownlistchild> Dropdownlistchildren { get; set; } = new List<Dropdownlistchild>();
}
