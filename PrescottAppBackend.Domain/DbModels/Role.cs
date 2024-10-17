using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
