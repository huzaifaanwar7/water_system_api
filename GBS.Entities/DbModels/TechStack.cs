using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class TechStack
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }
}
