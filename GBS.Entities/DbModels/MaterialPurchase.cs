using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class MaterialPurchase
{
    public int Id { get; set; }

    public string PurchaseNumber { get; set; } = null!;

    public int? MaterialIdFk { get; set; }

    public string? VendorName { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? Notes { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Material? MaterialIdFkNavigation { get; set; }
}
