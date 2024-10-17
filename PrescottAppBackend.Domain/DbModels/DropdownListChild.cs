using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Dropdownlistchild
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public string Label { get; set; } = null!;

    public string Value { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Dropdownlistparent Parent { get; set; } = null!;
}
