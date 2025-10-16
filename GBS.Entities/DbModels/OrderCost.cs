using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class OrderCost
{
    public int Id { get; set; }

    public int? OrderIdFk { get; set; }

    public int? CostCategoryIdFk { get; set; }

    public string? CostDescription { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? UnitCost { get; set; }

    public decimal? TotalCost { get; set; }

    public DateTime? CostDate { get; set; }

    public string? VendorName { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? Notes { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Lookup? CostCategoryIdFkNavigation { get; set; }

    public virtual Order? OrderIdFkNavigation { get; set; }
}
