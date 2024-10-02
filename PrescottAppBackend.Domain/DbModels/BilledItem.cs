using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class BilledItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int IncomeTrackerId { get; set; }

    public int UnitSold { get; set; }

    public double TotalPrice { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
