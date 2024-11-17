using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class Lookup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
