using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class PurchaseOrderItem
{
    public int Id { get; set; }

    public int PurchaseOrderId { get; set; }

    public string? Item { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public decimal Rate { get; set; }

    public string? Customer { get; set; }

    public string? Class { get; set; }

    public decimal Amount { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }
}
