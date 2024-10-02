using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public int VendorId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalOrderAmount { get; set; }

    public string? RefNo { get; set; }

    public int? Ponum { get; set; }

    public string? ShipTo { get; set; }

    public string? VendorMessage { get; set; }

    public string? Memo { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }
}
