using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Product
{
    public int Id { get; set; }

    public string? Reference { get; set; }

    public string? Name { get; set; }

    public string? ItemType { get; set; }

    public double? PerUnitPrice { get; set; }

    public int? TotalUnits { get; set; }

    public bool IsBillable { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
