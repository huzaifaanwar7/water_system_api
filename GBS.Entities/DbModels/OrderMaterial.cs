using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class OrderMaterial
{
    public int Id { get; set; }

    public int? OrderIdFk { get; set; }

    public int? MaterialIdFk { get; set; }

    public decimal? QuantityUsed { get; set; }

    public decimal? UnitCost { get; set; }

    public decimal? TotalCost { get; set; }

    public DateTime? UsageDate { get; set; }

    public string? Notes { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Material? MaterialIdFkNavigation { get; set; }

    public virtual Order? OrderIdFkNavigation { get; set; }
}
